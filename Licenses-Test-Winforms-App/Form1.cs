using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LicenPro.SDK;
using LicenPro.Utilities;

namespace Licenses_Test_Winforms_App
{
    public partial class Form1 : Form
    {
        private LicenseClient? _licenseClient;

        public Form1()
        {
            InitializeComponent();
            Shown += async (_, __) => await TryAutoValidateAsync();
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
                await ValidateFromCacheAsync().ConfigureAwait(false);
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
                var licenseBytes = await File.ReadAllBytesAsync(licensePath);
                byte[] plainBytes;

                // Decrypt if protected
                if (LicenseFileProtector.LooksProtected(licenseBytes))
                    plainBytes = await LicenseFileProtector.UnprotectAsync(licenseBytes, txtLicenseKey.Text);
                else
                    plainBytes = licenseBytes;

                // Save to temp file for validation
                var tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "license.tmp");
                await File.WriteAllBytesAsync(tempPath, plainBytes);

                try
                {
                    _licenseClient = new LicenseClient(new LicenseClientOptions
                    {
                        LicenseFilePath = tempPath,
                        PublicKey = txtPublicKey.Text,
                        LicenseKey = txtLicenseKey.Text,
                        AllowNonBinFiles = true
                    });

                    var result = await _licenseClient.ValidateAsync();

                    if (result.IsValid)
                    {
                        // Cache license + credentials
                        await LicenseCache.SaveAsync(plainBytes, txtLicenseKey.Text, txtPublicKey.Text);

                        UpdateStatus("VALID", Color.Green);
                        pnlStatus.BackColor = Color.FromArgb(220, 252, 231);
                        RefreshFeaturesList();

                        var license = result.License!;
                        MessageBox.Show(
                            $"License Valid!\n\nType: {license.Type}\nKey: {license.LicenseKey}\nIssued: {license.IssuedOn:yyyy-MM-dd}" +
                            (license.ExpirationDate.HasValue ? $"\nExpires: {license.ExpirationDate.Value:yyyy-MM-dd}" : ""),
                            "Valid", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Connect session in background
                        _ = Task.Run(async () =>
                        {
                            try { await _licenseClient.ConnectSessionAsync(); }
                            catch { }
                        });
                    }
                    else
                    {
                        ShowValidationError(result);
                    }
                }
                finally
                {
                    if (File.Exists(tempPath)) File.Delete(tempPath);
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

            await ValidateFromCacheAsync().ConfigureAwait(false);
        }

        private async Task ValidateFromCacheAsync()
        {
            if (!LicenseCache.Exists())
            {
                MessageBox.Show("No cached license. Validate online first.", "Cache Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UpdateStatus("Validating from cache...", Color.DodgerBlue);

            var payload = await LicenseCache.LoadAsync().ConfigureAwait(false);
            if (payload == null)
            {
                if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
                {
                    MessageBox.Show(
                        "This cached license was created using the old encrypted cache format.\n\nPlease enter the License Key once to read it, then validate online again to recreate the cache in the new format.",
                        "License Key Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    UpdateStatus("Enter License Key to read cache", Color.Orange);
                    pnlStatus.BackColor = Color.FromArgb(254, 243, 199);
                    return;
                }

                payload = await LicenseCache.LoadAsync(txtLicenseKey.Text).ConfigureAwait(false);
                if (payload == null)
                {
                    MessageBox.Show(
                        "Unable to read cached license. This usually means the License Key is wrong, or the cache file is corrupted.\n\nTry validating online again to re-create the cache.",
                        "Cache Read Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    UpdateStatus("Cache read failed", Color.OrangeRed);
                    pnlStatus.BackColor = Color.FromArgb(254, 226, 226);
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
                txtLicenseKey.Text = payload.LicenseKey;
            if (string.IsNullOrWhiteSpace(txtPublicKey.Text))
                txtPublicKey.Text = payload.PublicKey;

            var result = await LicenseCache.ValidateCachedAsync(txtPublicKey.Text, txtLicenseKey.Text).ConfigureAwait(false);

            if (result?.IsValid == true)
            {
                UpdateStatus("VALID (CACHED)", Color.Green);
                pnlStatus.BackColor = Color.FromArgb(220, 252, 231);
                RefreshFeaturesList();

                var license = result.License!;
                MessageBox.Show(
                    $"Cached License Valid!\n\nType: {license.Type}\nKey: {license.LicenseKey}\nIssued: {license.IssuedOn:yyyy-MM-dd}" +
                    (license.ExpirationDate.HasValue ? $"\nExpires: {license.ExpirationDate.Value:yyyy-MM-dd}" : ""),
                    "Valid (Cached)",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (result != null)
            {
                ShowValidationError(result);
            }
            else
            {
                MessageBox.Show(
                    "Cached validation failed. Please confirm the License Key and Public Key are correct.",
                    "Invalid",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                UpdateStatus("INVALID", Color.Red);
                pnlStatus.BackColor = Color.FromArgb(254, 226, 226);
            }
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

        private async Task TryAutoValidateAsync()
        {
            if (!LicenseCache.Exists())
                return;

            var payload = await LicenseCache.LoadAsync().ConfigureAwait(false);
            if (payload == null)
            {
                UpdateStatus("Cache found - enter License Key to validate", Color.Orange);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
                txtLicenseKey.Text = payload.LicenseKey;
            if (string.IsNullOrWhiteSpace(txtPublicKey.Text))
                txtPublicKey.Text = payload.PublicKey;

            UpdateStatus("Auto validating...", Color.DodgerBlue);

            var result = await LicenseCache.ValidateCachedAsync(txtPublicKey.Text, txtLicenseKey.Text).ConfigureAwait(false);

            if (result?.IsValid == true)
            {
                UpdateStatus("VALID (AUTO)", Color.Green);
                pnlStatus.BackColor = Color.FromArgb(220, 252, 231);
                RefreshFeaturesList();
            }
            else
            {
                UpdateStatus("Auto validation failed", Color.OrangeRed);
            }
        }

        private void ShowValidationError(LicenseValidationResult result)
        {
            MessageBox.Show(result.ErrorMessage, result.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);

            var (status, color, panel) = result.Status switch
            {
                LicenseValidationStatus.Expired => ("EXPIRED", Color.OrangeRed, Color.FromArgb(254, 226, 226)),
                LicenseValidationStatus.DeviceBlocked => ("BLOCKED", Color.DarkRed, Color.FromArgb(254, 202, 202)),
                LicenseValidationStatus.Revoked => ("REVOKED", Color.DarkRed, Color.FromArgb(254, 202, 202)),
                _ => ("INVALID", Color.Red, Color.FromArgb(254, 226, 226))
            };

            UpdateStatus(status, color);
            pnlStatus.BackColor = panel;
            lstFeatures.Items.Clear();
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
    }
}
