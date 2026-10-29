using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using LicenPro.Models.License;
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
            txtAppVersion.Text = GetDefaultAppVersionDisplay();
            chkAutoCheckProductUpdates.Checked = SdkConfiguration.AutoCheckProductUpdates;
            chkAutoCheckProductUpdates.CheckedChanged += (_, _) =>
            {
                SdkConfiguration.SetAutoCheckProductUpdates(chkAutoCheckProductUpdates.Checked);
                ApplyPeriodicProductUpdateCheck();
            };
            txtAppVersion.TextChanged += (_, _) => RefreshEffectiveUpdateVersionDisplay();
            SdkUpdateManager.UpdateAvailable += OnSdkProductUpdateAvailable;
            SetupFormTooltips();
            RefreshEffectiveUpdateVersionDisplay();
            Shown += async (_, __) => await RunStartupLicenseCheckAsync();
            FormClosing += async (_, __) =>
            {
                if (_licenseClient != null)
                    await _licenseClient.DisposeAsync();
            };
        }

        private void SetupFormTooltips()
        {
            void T(Control c, string text) => toolTipUi.SetToolTip(c, text);

            T(lblTitle,
                "Sample LicenPro validator: load your binary license, public key, and license key, then validate online or from cache.");
            T(grpLicenseFile,
                "Path to the encrypted license file (.bin) downloaded from the dashboard. If empty and a cache exists, validation uses the SDK cache (licenpro.cache) next to the app.");
            T(lblLicenseFile, "Must match the file you downloaded for this license.");
            T(txtLicenseFile, "Full path to the downloaded license file, e.g. license.bin (or Browse). Leave empty to validate from the SDK cache (licenpro.cache).");
            T(btnBrowseLicense, "Pick a .bin license file from disk.");

            T(grpPublicKey,
                "RSA public key for the product that signed the license. Copy from Dashboard → Product → Settings (Base64, no PEM headers).");
            T(lblPublicKey, "Must be the key pair that signed this license file.");
            T(txtPublicKey, "Paste Base64 public key or use Load to read a .pem/.txt file (headers stripped automatically).");
            T(btnBrowsePublicKey, "Load public key from a PEM or text file.");

            T(grpCredentials, "The license key string exactly as shown in the dashboard (used with the file for online validation).");
            T(lblLicenseKey, "Same key you copy from the license record; required for online validation and cache unlock.");
            T(txtLicenseKey, "Enter the full license key. Cached credentials may pre-fill this field.");

            T(grpUpdates,
                "Dashboard compares your current build (X-Current-Version) to releases you are allowed to use. " +
                "If the license file includes an assigned release version, that value is used automatically; otherwise the editable field is used.");
            T(lblUpdAppVer,
                "Product / app version string. After validation, this may auto-fill from the license’s assigned release when embedded in the file.");
            T(txtAppVersion,
                "Semantic version of the build you are running (e.g. 1.0.0). Used for update checks when the license has no embedded release version.");
            T(lblUpdProductId,
                "Product GUID from the dashboard. Optional if your license type embeds product scope; required for some perpetual licenses.");
            T(txtProductId,
                "Paste the product Id (GUID) from Product Settings. Sent as X-Product-Id on update checks when set.");
            T(lblEffectiveUpdateVersion,
                "Read-only: the exact version string sent to the server as X-Current-Version. " +
                "Priority: (1) release version embedded in the validated license, (2) the editable field, (3) 1.0.0 if empty.");
            T(chkAutoCheckProductUpdates,
                "When enabled and the license is valid, the SDK asks the server periodically (24h) whether a newer allowed release exists.");
            T(btnCheckUpdates,
                "Calls the license-aware update API now using your license key, hardware id, product id, and effective current version.");
            T(btnOpenLicensedDownload,
                "Asks the server for the download link of the newest released build your license may use (same rules as updates), then opens it in your browser. " +
                "Requires license key; validate online once so the API URL is configured.");

            T(pnlStatus, "Shows the result of the last validation attempt and a local timestamp.");
            T(lblStatusTitle, "Heading for the validation result line.");
            T(lblStatus, "VALID, INVALID, or other status from the SDK after validate or cache read.");
            T(lblStatusDetail, "Detailed validation message (errors show here instead of a popup).");
            T(lblValidationTime, "Local time of the last status change.");

            T(btnClearCachedLicense,
                "Deletes the SDK cache (licenpro.cache) in the app folder. Your downloaded license file is not touched. Next online validation caches a fresh copy.");
            T(btnValidate,
                "Validates online: uploads license file, checks signature and server rules, activates session when applicable.");
            T(btnValidateOffline,
                "Reads the SDK cache (licenpro.cache) + keys from disk and validates using cached flow (requires prior online validate).");
            T(btnClear,
                "Disconnects the session, disposes the license client, clears inputs, resets app version to assembly default, and stops auto update checks.");

            T(lblFeatures, "Feature flags enabled on the license after successful validation.");
            T(lstFeatures, "One line per feature from the validated license.");
            T(grpLicenseDetails, "License metadata from the signed file and server (name from server on online / first offline run).");
            T(lblDetailName, "Dashboard license name.");
            T(lblDetailIssued, "When the license was issued (UTC).");
            T(lblDetailIssuer, "Organization or issuer recorded on the license.");
            T(lblDetailMode, "Online = server validation every run. Offline = server once on first run, then local.");
            T(lblDetailType, "License type from the signed file (Perpetual, Trial, etc.).");
        }

        private void RefreshEffectiveUpdateVersionDisplay()
        {
            var v = GetProductVersionForUpdateChecks();
            var fromLicense = !string.IsNullOrWhiteSpace(_licenseClient?.License?.AssignedReleaseVersion);
            var source = fromLicense
                ? "assigned release embedded in license file"
                : "editable app version field (or default 1.0.0 if empty)";
            lblEffectiveUpdateVersion.Text =
                $"Effective version for update check (X-Current-Version): {v}{Environment.NewLine}Source: {source}";
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

            // No file selected: validate from the SDK cache (LicenseCache.LicensePath). A selected file,
            // including a vendor license.bin, is always validated online as a downloaded license file.
            var licensePath = txtLicenseFile.Text;
            if (string.IsNullOrWhiteSpace(licensePath) && LicenseCache.Exists())
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
                    UpdateStatus("VALID", Color.Green, "License validated successfully.");
                    pnlStatus.BackColor = Color.FromArgb(220, 252, 231);
                    RefreshFeaturesList();
                    RefreshLicenseDetailsPanel();

                    var license = cacheResult.ValidationResult.License;
                    MessageBox.Show(
                        $"License Valid!\n\nType: {license.Type}\nKey: {license.LicenseKey}\nIssued: {license.IssuedOn:yyyy-MM-dd}" +
                        (license.ExpirationDate.HasValue ? $"\nExpires: {license.ExpirationDate.Value:yyyy-MM-dd HH:mm}" : ""),
                        "Valid", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ShowSubscriptionExpiryAlertIfNeeded(cacheResult.ValidationResult, useMessageBox: true);

                    await ConnectSessionAndReportAsync();
                }
                else if (cacheResult.ValidationResult != null)
                {
                    ApplyValidationFailureFeedback(cacheResult.ValidationResult);
                }
                else
                {
                    UpdateStatus("INVALID", Color.Red, cacheResult.Message);
                    pnlStatus.BackColor = Color.FromArgb(254, 226, 226);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Error", Color.Red, ex.Message);
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
                UpdateStatus("VALID (CACHED)", Color.Green, "Cached license validated successfully.");
                pnlStatus.BackColor = Color.FromArgb(220, 252, 231);
                RefreshFeaturesList();
                RefreshLicenseDetailsPanel();

                var license = r.ValidationResult.License;
                MessageBox.Show(
                    $"Cached License Valid!\n\nType: {license.Type}\nKey: {license.LicenseKey}\nIssued: {license.IssuedOn:yyyy-MM-dd}" +
                    (license.ExpirationDate.HasValue ? $"\nExpires: {license.ExpirationDate.Value:yyyy-MM-dd HH:mm}" : ""),
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

            UpdateStatus("INVALID", Color.Red,
                string.IsNullOrWhiteSpace(r.Message)
                    ? "Cached validation failed. Please confirm the License Key and Public Key are correct."
                    : r.Message);
            pnlStatus.BackColor = Color.FromArgb(254, 226, 226);
        }

        /// <summary>
        /// Connects the license session and shows a failure in the status panel (the license itself stays valid).
        /// </summary>
        private async Task ConnectSessionAndReportAsync()
        {
            if (_licenseClient == null)
                return;

            SessionConnectResult result;
            try
            {
                result = await _licenseClient.ConnectSessionAsync();
            }
            catch (Exception ex)
            {
                result = new SessionConnectResult { Success = false, Message = ex.Message };
            }

            if (result.Success)
                return;

            UpdateStatus("VALID — session not connected", Color.DarkOrange,
                string.IsNullOrWhiteSpace(result.Message)
                    ? "License is valid, but the session could not be connected."
                    : $"License is valid, but the session could not be connected: {result.Message}");
            pnlStatus.BackColor = Color.FromArgb(255, 247, 237);
        }

        private async void btnClear_Click(object? sender, EventArgs e)
        {
            var disconnectFailed = false;
            if (_licenseClient != null)
            {
                try
                {
                    disconnectFailed = !await _licenseClient.DisconnectAsync();
                }
                catch
                {
                    disconnectFailed = true;
                }

                await _licenseClient.DisposeAsync();
                _licenseClient = null;
            }

            SdkUpdateManager.DisableAutoCheck();

            txtLicenseFile.Text = "";
            txtPublicKey.Text = "";
            txtLicenseKey.Text = "";
            txtAppVersion.Text = GetDefaultAppVersionDisplay();
            RefreshEffectiveUpdateVersionDisplay();
            if (disconnectFailed)
            {
                UpdateStatus("Not Validated — disconnect failed", Color.OrangeRed,
                    "The server did not confirm the session disconnect. The session may stay active until it times out.");
                pnlStatus.BackColor = Color.FromArgb(254, 226, 226);
            }
            else
            {
                UpdateStatus("Not Validated", Color.Gray, null);
                pnlStatus.BackColor = Color.FromArgb(243, 244, 246);
            }
            lstFeatures.Items.Clear();
            ClearLicenseDetailsPanel();
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
                RefreshLicenseDetailsPanel();
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
            SyncAppVersionFieldFromValidatedLicense(newClient?.License);
            RefreshEffectiveUpdateVersionDisplay();
            RefreshLicenseDetailsPanel();
            ApplyPeriodicProductUpdateCheck();
        }

        /// <summary>
        /// Starts or stops SDK periodic update checks based on local settings and a validated <see cref="LicenseClient"/>.
        /// </summary>
        private void ApplyPeriodicProductUpdateCheck()
        {
            SdkUpdateManager.DisableAutoCheck();
            if (_licenseClient?.License == null || !chkAutoCheckProductUpdates.Checked)
                return;

            var appVer = GetProductVersionForUpdateChecks();
            var pid = string.IsNullOrWhiteSpace(txtProductId.Text) ? null : txtProductId.Text.Trim();

            _licenseClient.EnableAutoUpdates(
                currentAppVersion: appVer,
                productId: pid,
                checkIntervalHours: 24,
                onUpdateAvailable: null);
        }

        /// <summary>Outcome of the update attempt that restarted this process (set in Program.Main).</summary>
        internal static LicenPro.SDK.Updates.UpdateRestartInfo? UpdateRestartInfo { get; set; }

        private UpdatePromptForm? _updatePrompt;

        /// <summary>One update window at a time; it downloads, verifies and installs on "Update & restart".</summary>
        private void ShowUpdatePrompt(UpdateInfo info)
        {
            if (_updatePrompt is { IsDisposed: false })
            {
                _updatePrompt.Activate();
                return;
            }

            _updatePrompt = new UpdatePromptForm(info);
            _updatePrompt.FormClosed += (_, _) => _updatePrompt = null;
            _updatePrompt.Show(this);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (UpdateRestartInfo is { } restart)
            {
                UpdateRestartInfo = null;
                if (restart.Succeeded)
                    MessageBox.Show($"Updated from version {restart.FromVersion}.", "Update installed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show($"The update was not installed; the previous version is running.\n\n{restart.Error}",
                        "Update failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OnSdkProductUpdateAvailable(object? sender, UpdateAvailableEventArgs e)
        {
            if (!chkAutoCheckProductUpdates.Checked || e.Update == null)
                return;

            void Show() => ShowUpdatePrompt(e.Update);

            if (InvokeRequired)
                BeginInvoke(Show);
            else
                Show();
        }

        private void ApplyValidationFailureFeedback(LicenseValidationResult result)
        {
            var f = LicenseValidationFeedback.From(result);
            var detail = string.IsNullOrWhiteSpace(result.ErrorMessage) ? f.DialogTitle : result.ErrorMessage;
            if (detail.Equals("Success", StringComparison.OrdinalIgnoreCase))
                detail = "License not found.";

            UpdateStatus(f.StatusLabel, Color.FromArgb(f.StatusTextColorArgb), detail);
            pnlStatus.BackColor = Color.FromArgb(f.PanelBackgroundArgb);
            lstFeatures.Items.Clear();
            ClearLicenseDetailsPanel();
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

        private void RefreshLicenseDetailsPanel()
        {
            var info = LicenseInfoHelper.GetLicenseInfo(_licenseClient?.License);
            if (info == null)
            {
                ClearLicenseDetailsPanel();
                return;
            }

            lblDetailNameValue.Text = string.IsNullOrWhiteSpace(info.Name) ? "(not set)" : info.Name;
            lblDetailIssuedValue.Text = info.IssuedOn.ToString("yyyy-MM-dd HH:mm") + " UTC";
            lblDetailIssuerValue.Text = string.IsNullOrWhiteSpace(info.Issuer) ? "—" : info.Issuer;
            lblDetailModeValue.Text = info.ValidationMode;
            lblDetailTypeValue.Text = info.LicenseType;
        }

        private void ClearLicenseDetailsPanel()
        {
            lblDetailNameValue.Text = "—";
            lblDetailIssuedValue.Text = "—";
            lblDetailIssuerValue.Text = "—";
            lblDetailModeValue.Text = "—";
            lblDetailTypeValue.Text = "—";
        }

        private static string GetDefaultAppVersionDisplay() =>
            SdkAssemblyInfo.GetSemanticVersionString(Assembly.GetExecutingAssembly());

        /// <summary>Prefer release version embedded in the validated license over the sample app's assembly version.</summary>
        private void SyncAppVersionFieldFromValidatedLicense(BaseLicense? license)
        {
            if (!string.IsNullOrWhiteSpace(license?.AssignedReleaseVersion))
                txtAppVersion.Text = license.AssignedReleaseVersion.Trim();
        }

        /// <summary>Version sent as X-Current-Version: license assignment wins so update checks match dashboard release.</summary>
        private string GetProductVersionForUpdateChecks()
        {
            if (!string.IsNullOrWhiteSpace(_licenseClient?.License?.AssignedReleaseVersion))
                return _licenseClient.License.AssignedReleaseVersion.Trim();
            var ui = txtAppVersion.Text.Trim();
            return string.IsNullOrEmpty(ui) ? "1.0.0" : ui;
        }

        private void UpdateStatus(string message, Color color, string? detail = null)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;
            lblStatusDetail.Text = detail ?? string.Empty;
            lblStatusDetail.MaximumSize = new Size(430, 0);
            lblValidationTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Dashboard release notes are often HTML (rich editor). MessageBox only shows plain text.
        /// </summary>
        internal static string PlainTextFromHtmlChangelog(string? html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            var s = WebUtility.HtmlDecode(html.Trim());
            s = s.Replace('\u00A0', ' ');

            s = Regex.Replace(s, @"</?(p|div|li|h[1-6]|tr|section|article|blockquote)\b[^>]*>", "\n", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, @"<br\s*/?>", "\n", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, @"<[^>]+>", string.Empty);

            s = Regex.Replace(s, @"[ \t\f\v]{2,}", " ");
            s = Regex.Replace(s, @"(\r?\n){3,}", "\n\n");
            return s.Trim();
        }

        private async void btnOpenLicensedDownload_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
            {
                MessageBox.Show("Enter your license key (same as for validation).", "Licensed download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var productIdFromUi = string.IsNullOrWhiteSpace(txtProductId.Text) ? null : txtProductId.Text.Trim();
            var hw = new PersistedHardwareIdentifier().GetHardwareIdentifier();

            btnOpenLicensedDownload.Enabled = false;
            try
            {
                if (_licenseClient?.License != null)
                {
                    var productId = productIdFromUi ?? _licenseClient.ResolvedProductIdForUpdates;
                    SdkUpdateManager.SetLicenseContext(txtLicenseKey.Text.Trim(), hw, productId);
                }
                else
                {
                    SdkUpdateManager.SetLicenseContext(txtLicenseKey.Text.Trim(), hw, productIdFromUi);
                }

                var res = await SdkUpdateManager.GetEntitledReleaseDownloadAsync();
                if (!res.HasDownload || string.IsNullOrWhiteSpace(res.DownloadUrl))
                {
                    var msg = string.IsNullOrWhiteSpace(res.Message)
                        ? "The server did not return a download link."
                        : res.Message;
                    MessageBox.Show(msg, "Licensed download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!Uri.TryCreate(res.DownloadUrl.Trim(), UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    MessageBox.Show(
                        "The server returned a link that cannot be opened automatically. Copy it from the dashboard release page instead.\n\n" + res.DownloadUrl,
                        "Licensed download",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var ver = string.IsNullOrWhiteSpace(res.Version) ? "" : $" ({res.Version})";
                var opened = MessageBox.Show(
                    $"Open download{ver} in your browser?\n\n{res.DownloadUrl}",
                    "Licensed download",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (opened != DialogResult.Yes)
                    return;

                Process.Start(new ProcessStartInfo
                {
                    FileName = res.DownloadUrl.Trim(),
                    UseShellExecute = true
                });
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
                MessageBox.Show($"Could not resolve download:\n{ex.Message}", "Licensed download", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnOpenLicensedDownload.Enabled = true;
            }
        }

        private async void btnCheckUpdates_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
            {
                MessageBox.Show("Enter your license key (same as for validation).", "Check for updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var appVer = GetProductVersionForUpdateChecks();
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
                    ShowUpdatePrompt(info);
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
