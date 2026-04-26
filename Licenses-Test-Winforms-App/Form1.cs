using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using LicenPro.SDK;
using LicenPro.SDK.AppHosting;
using LicenPro.SDK.Updates;
using LicenPro.Utilities;

namespace Licenses_Test_Winforms_App
{
    public partial class Form1 : Form
    {
        private LicenseClient? _licenseClient;

        public Form1()
        {
            InitializeComponent();
            txtAppVersion.Text = SdkAssemblyInfo.GetSemanticVersionString(Assembly.GetExecutingAssembly());
            Shown += async (_, __) => await RunStartupLicenseCheckAsync();
            FormClosing += async (_, __) =>
            {
                if (_licenseClient != null)
                    await _licenseClient.DisposeAsync();
            };
        }

        private void btnBrowseLicense_Click(object? sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "LicenPro License (*.bin)|*.bin|All Files (*.*)|*.*",
                Title = "Select License File",
                RestoreDirectory = true
            };
            if (dlg.ShowDialog(this) == DialogResult.OK)
                txtLicenseFile.Text = dlg.FileName;
        }

        private void btnBrowsePublicKey_Click(object? sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Key Files (*.pem;*.key;*.txt)|*.pem;*.key;*.txt|All Files (*.*)|*.*",
                Title = "Select Public Key",
                RestoreDirectory = true
            };
            if (dlg.ShowDialog(this) == DialogResult.OK)
                txtPublicKey.Text = SecurityUtils.StripPemHeaders(File.ReadAllText(dlg.FileName));
        }

        private async void btnValidate_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
            {
                MessageBox.Show("Please enter your License Key.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Use cached license if no file selected
            var licensePath = txtLicenseFile.Text;
            if (string.IsNullOrWhiteSpace(licensePath) && LicenseCache.Exists())
                licensePath = LicenseCache.LicensePath;

            if (!string.IsNullOrWhiteSpace(licensePath) &&
                Path.GetFileName(licensePath).Equals("license.bin", StringComparison.OrdinalIgnoreCase))
            {
                await ValidateFromCacheAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(licensePath) || !File.Exists(licensePath))
            {
                MessageBox.Show("Please select a license file.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdateStatus("Validating...", Color.DodgerBlue);

            try
            {
                var productId = string.IsNullOrWhiteSpace(txtProductId.Text) ? null : txtProductId.Text.Trim();
                var cacheResult = await LicenseClient.ValidateAndCacheAsync(
                    licensePath,
                    txtPublicKey.Text,
                    txtLicenseKey.Text,
                    expectedLicenseType: null,
                    productId: productId);

                if (cacheResult.IsSuccess && cacheResult.ValidationResult?.License != null)
                {
                    await ReplaceLicenseClientAsync(cacheResult.Client);
                    UpdateStatus("VALID", Color.Green);
                    pnlStatus.BackColor = Color.FromArgb(220, 252, 231);
                    RefreshFeaturesList();

                    var license = cacheResult.ValidationResult.License;
                    MessageBox.Show(
                        $"License Valid!\n\nType: {license.Type}\nKey: {license.LicenseKey}\nIssued: {license.IssuedOn:yyyy-MM-dd}" +
                        (license.ExpirationDate.HasValue ? $"\nExpires: {license.ExpirationDate.Value:yyyy-MM-dd}" : ""),
                        "Valid", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ShowSubscriptionExpiryAlertIfNeeded(cacheResult.ValidationResult, useMessageBox: true);

                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            if (_licenseClient != null)
                                await _licenseClient.ConnectSessionAsync();
                        }
                        catch
                        {
                            /* non-fatal */
                        }
                    });
                }
                else if (cacheResult.ValidationResult != null)
                {
                    ApplyValidationFailureFeedback(cacheResult.ValidationResult);
                }
                else
                {
                    MessageBox.Show(cacheResult.Message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateStatus("Error", Color.Red);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error", Color.Red);
            }
        }

        private async void btnValidateOffline_Click(object? sender, EventArgs e)
        {
            if (!LicenseCache.Exists())
            {
                MessageBox.Show("No cached license. Validate online first.", "Cache Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await ValidateFromCacheAsync();
        }

        private async Task ValidateFromCacheAsync()
        {
            if (!LicenseCache.Exists())
            {
                MessageBox.Show("No cached license. Validate online first.", "Cache Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UpdateStatus("Validating from cache...", Color.DodgerBlue);

            var r = await LicenseClient.TryAutoValidateAsync(
                string.IsNullOrWhiteSpace(txtLicenseKey.Text) ? null : txtLicenseKey.Text,
                string.IsNullOrWhiteSpace(txtPublicKey.Text) ? null : txtPublicKey.Text);

            ApplyCacheCredentialHints(r);

            if (r.Status == CacheValidationStatus.CacheNotFound)
            {
                MessageBox.Show("No cached license. Validate online first.", "Cache Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (r.Status == CacheValidationStatus.LicenseKeyRequired)
            {
                MessageBox.Show(r.Message, "License Key Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateStatus("Enter License Key to read cache", Color.Orange);
                pnlStatus.BackColor = Color.FromArgb(254, 243, 199);
                return;
            }

            if (r.Status == CacheValidationStatus.CacheCorrupted)
            {
                MessageBox.Show(r.Message, "Cache Read Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateStatus("Cache read failed", Color.OrangeRed);
                pnlStatus.BackColor = Color.FromArgb(254, 226, 226);
                return;
            }

            if (r.IsSuccess && r.ValidationResult?.License != null)
            {
                await ReplaceLicenseClientAsync(r.Client);
                UpdateStatus("VALID (CACHED)", Color.Green);
                pnlStatus.BackColor = Color.FromArgb(220, 252, 231);
                RefreshFeaturesList();

                var license = r.ValidationResult.License;
                MessageBox.Show(
                    $"Cached License Valid!\n\nType: {license.Type}\nKey: {license.LicenseKey}\nIssued: {license.IssuedOn:yyyy-MM-dd}" +
                    (license.ExpirationDate.HasValue ? $"\nExpires: {license.ExpirationDate.Value:yyyy-MM-dd}" : ""),
                    "Valid (Cached)",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ShowSubscriptionExpiryAlertIfNeeded(r.ValidationResult, useMessageBox: true);
                return;
            }

            if (r.ValidationResult != null)
            {
                ApplyValidationFailureFeedback(r.ValidationResult);
                return;
            }

            MessageBox.Show(
                string.IsNullOrWhiteSpace(r.Message)
                    ? "Cached validation failed. Please confirm the License Key and Public Key are correct."
                    : r.Message,
                "Invalid",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            UpdateStatus("INVALID", Color.Red);
            pnlStatus.BackColor = Color.FromArgb(254, 226, 226);
        }

        private async void btnClear_Click(object? sender, EventArgs e)
        {
            if (_licenseClient != null)
            {
                await _licenseClient.DisposeAsync();
                _licenseClient = null;
            }

            txtLicenseFile.Text = "";
            txtPublicKey.Text = "";
            txtLicenseKey.Text = "";
            UpdateStatus("Not Validated", Color.Gray);
            pnlStatus.BackColor = Color.FromArgb(243, 244, 246);
            lstFeatures.Items.Clear();
        }

        private void btnClearCachedLicense_Click(object? sender, EventArgs e)
        {
            LicenseCache.Clear();
            UpdateStatus("Cache cleared", Color.Orange);
            pnlStatus.BackColor = Color.FromArgb(254, 243, 199);
        }

        private async Task RunStartupLicenseCheckAsync()
        {
            if (!LicenseCache.Exists())
                return;

            UpdateStatus("Auto validating...", Color.DodgerBlue);

            var r = await LicenseClient.TryAutoValidateAsync(
                string.IsNullOrWhiteSpace(txtLicenseKey.Text) ? null : txtLicenseKey.Text,
                string.IsNullOrWhiteSpace(txtPublicKey.Text) ? null : txtPublicKey.Text);

            ApplyCacheCredentialHints(r);

            if (r.Status == CacheValidationStatus.CacheNotFound)
                return;

            if (r.Status == CacheValidationStatus.LicenseKeyRequired)
            {
                UpdateStatus("Cache found - enter License Key to validate", Color.Orange);
                pnlStatus.BackColor = Color.FromArgb(254, 243, 199);
                return;
            }

            if (r.IsSuccess)
            {
                await ReplaceLicenseClientAsync(r.Client);
                if (r.ValidationResult?.ExpiryNotice is { } exp)
                {
                    UpdateStatus($"VALID (AUTO) — renew within {exp.WholeDaysRemaining} day(s)", Color.DarkOrange);
                    pnlStatus.BackColor = Color.FromArgb(255, 247, 237);
                }
                else
                {
                    UpdateStatus("VALID (AUTO)", Color.Green);
                    pnlStatus.BackColor = Color.FromArgb(220, 252, 231);
                }

                RefreshFeaturesList();
                return;
            }

            if (r.ValidationResult != null)
            {
                ApplyValidationFailureFeedback(r.ValidationResult);
                return;
            }

            UpdateStatus("Auto validation failed", Color.OrangeRed);
        }

        private void ApplyCacheCredentialHints(CacheValidationResult r)
        {
            if (!string.IsNullOrEmpty(r.CachedLicenseKey) && string.IsNullOrWhiteSpace(txtLicenseKey.Text))
                txtLicenseKey.Text = r.CachedLicenseKey;
            if (!string.IsNullOrEmpty(r.CachedPublicKey) && string.IsNullOrWhiteSpace(txtPublicKey.Text))
                txtPublicKey.Text = r.CachedPublicKey;
        }

        private async Task ReplaceLicenseClientAsync(LicenseClient? newClient)
        {
            if (_licenseClient != null)
                await _licenseClient.DisposeAsync();
            _licenseClient = newClient;
        }

        private void ApplyValidationFailureFeedback(LicenseValidationResult result)
        {
            var f = LicenseValidationFeedback.From(result);
            MessageBox.Show(f.DialogMessage, f.DialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            UpdateStatus(f.StatusLabel, Color.FromArgb(f.StatusTextColorArgb));
            pnlStatus.BackColor = Color.FromArgb(f.PanelBackgroundArgb);
            lstFeatures.Items.Clear();
        }

        /// <summary>
        /// Uses <see cref="LicenseValidationResult.ExpiryNotice"/> from the SDK (subscription / optional trial window).
        /// </summary>
        private static void ShowSubscriptionExpiryAlertIfNeeded(LicenseValidationResult? validationResult, bool useMessageBox)
        {
            if (validationResult?.ExpiryNotice is not { } notice)
                return;

            if (useMessageBox)
                MessageBox.Show(notice.Message, notice.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void RefreshFeaturesList()
        {
            lstFeatures.Items.Clear();
            var features = FeatureManager.GetAllFeatures();
            if (features == null || features.Count == 0)
            {
                lstFeatures.Items.Add("(No features)");
                return;
            }

            foreach (var f in features)
            {
                var status = FeatureManager.IsFeatureEnabled(f.Key) ? "✓" : "✗";
                lstFeatures.Items.Add($"{status} {f.Key}");
            }
        }

        private void UpdateStatus(string message, Color color)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;
            lblValidationTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private async void btnCheckUpdates_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
            {
                MessageBox.Show("Enter your license key (same as for validation).", "Check for updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var appVer = string.IsNullOrWhiteSpace(txtAppVersion.Text) ? "1.0.0" : txtAppVersion.Text.Trim();
            var productIdFromUi = string.IsNullOrWhiteSpace(txtProductId.Text) ? null : txtProductId.Text.Trim();

            btnCheckUpdates.Enabled = false;
            try
            {
                UpdateInfo? info;
                if (_licenseClient?.License != null)
                {
                    var productId = productIdFromUi ?? _licenseClient.ResolvedProductIdForUpdates;
                    info = await SdkUpdateManager.CheckForUpdateAsync(
                        currentAppVersion: appVer,
                        productId: productId);
                }
                else
                {
                    SdkUpdateManager.SetLicenseContext(
                        txtLicenseKey.Text.Trim(),
                        new PersistedHardwareIdentifier().GetHardwareIdentifier(),
                        productIdFromUi);
                    info = await SdkUpdateManager.CheckForUpdateAsync(currentAppVersion: appVer);
                }

                if (info == null)
                {
                    var serverHint = SdkUpdateManager.LastUpdateCheckMessage;
                    var detail = string.IsNullOrWhiteSpace(serverHint)
                        ? ""
                        : "\n\nServer: " + serverHint;
                    MessageBox.Show(
                        "No update returned. You may already be on the latest build allowed for this license, or the server could not match the license/product." +
                        detail +
                        "\n\nTip: validate online once first (so the API base URL is configured). Set Product ID if the license is not linked to a release. " +
                        "Compare \"App version\" below with dashboard release numbers (must be lower than a released version you are allowed to use).",
                        "No update",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    var notes = string.IsNullOrWhiteSpace(info.Changelog) ? "" : "\n\n" + info.Changelog;
                    MessageBox.Show(
                        $"Version {info.Version} is available for this license.{notes}",
                        "Update available",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(
                    ex.Message + "\n\nValidate your license online once, or set ServerBaseEndpoint in licenpro.settings.json next to this app.",
                    "SDK configuration",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update check failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnCheckUpdates.Enabled = true;
            }
        }
    }
}
