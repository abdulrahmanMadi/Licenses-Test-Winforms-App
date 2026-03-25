using System;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using LicenPro.Enums;
using LicenPro.SDK;
using LicenPro.Utilities;

namespace Licenses_Test_Winforms_App
{
    public partial class Form1 : Form
    {
        private LicenseClient? _licenseClient;
        private const string ProtectedLicenseFileName = "license.secure.bin";
        private const string RuntimeLicenseFileName = "license.runtime.bin";
        private const string ProtectedCredentialsFileName = "credentials.secure.bin";

        public Form1()
        {
            InitializeComponent();

            LoadStoredCredentialsIntoUi();

            Shown += async (_, __) =>
            {
                try
                {
                    await TryAutoValidateFromCacheAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Auto-validate error: {ex.Message}");
                }
            };

            FormClosing += async (_, __) =>
            {
                try
                {
                    if (_licenseClient != null)
                        await _licenseClient.DisposeAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Disconnect error: {ex.Message}");
                }
            };
        }

        private void btnBrowseLicense_Click(object? sender, EventArgs e)
        {
            try
            {
                var dlg = new OpenFileDialog
                {
                    ShowHelp = true,
                    Filter = "LicenPro Binary License (*.bin)|*.bin|Legacy License Files (*.lic)|*.lic|All Files (*.*)|*.*",
                    Title = "Select LicenPro License File",
                    RestoreDirectory = true
                };
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    txtLicenseFile.Text = dlg.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBrowsePublicKey_Click(object? sender, EventArgs e)
        {
            try
            {
                var dlg = new OpenFileDialog
                {
                    ShowHelp = true,
                    Filter = "Key Files (*.pem;*.key;*.txt)|*.pem;*.key;*.txt|All Files (*.*)|*.*",
                    Title = "Select Public Key File",
                    RestoreDirectory = true
                };
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    string content = System.IO.File.ReadAllText(dlg.FileName);
                    txtPublicKey.Text = SecurityUtils.StripPemHeaders(content);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnValidate_Click(object? sender, EventArgs e)
        {
            LoadStoredCredentialsIntoUi();

            // Step 1: Check if cached license file exists
            var protectedPath = GetProtectedLicensePath();
            var hasCachedLicense = File.Exists(protectedPath);

            // Step 2: If no cached file and no license file selected, prompt for license key
            if (!hasCachedLicense && string.IsNullOrWhiteSpace(txtLicenseFile.Text))
            {
                if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
                {
                    MessageBox.Show(
                        "No license file selected and no cached license found.\n\n" +
                        "Please either:\n" +
                        "• Browse and select a license file (.bin), OR\n" +
                        "• Enter your License Key to validate online",
                        "Credentials Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                // License key provided - proceed with online validation (will download license)
                UpdateStatus("Validating online...", Color.DodgerBlue);
            }

            // Step 3: If cached file exists but license key is missing, ask for it
            if (hasCachedLicense && string.IsNullOrWhiteSpace(txtLicenseKey.Text))
            {
                MessageBox.Show(
                    "A cached license file was found.\n\n" +
                    "Please enter your License Key to decrypt and validate the license.",
                    "Credentials Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Step 4: Ensure license path is set (prefer cached if exists)
            if (hasCachedLicense && string.IsNullOrWhiteSpace(txtLicenseFile.Text))
            {
                txtLicenseFile.Text = protectedPath;
                UpdateStatus("Using cached license file", Color.DodgerBlue);
            }

            EnsureLicensePathFromProtectedStore();
            var runtimePath = await PrepareRuntimeLicenseFileAsync();
            if (runtimePath == null)
                return;

            if (_licenseClient != null)
            {
                await _licenseClient.DisposeAsync();
                _licenseClient = null;
            }

            // SDK will auto-detect license type
            _licenseClient = new LicenseClient(new LicenseClientOptions
            {
                LicenseFilePath = runtimePath,
                PublicKey = txtPublicKey.Text,
                LicenseKey = txtLicenseKey.Text,
                ExpectedLicenseType = null, // Auto-detect
                AllowNonBinFiles = true
            });

            _licenseClient.SessionDisconnected += (_, msg) =>
            {
                if (InvokeRequired)
                    Invoke(() => OnSessionDisconnected(msg));
                else
                    OnSessionDisconnected(msg);
            };

            try
            {
                var result = await _licenseClient.ValidateAsync();
                if (result.IsValid)
                {
                    SaveCredentialsSecurely(txtLicenseKey.Text, txtPublicKey.Text);
                    await SaveLicenseToProtectedStoreAsync(runtimePath);
                    UpdateStatus("VALID!", Color.Green);
                    pnlStatus.BackColor = Color.FromArgb(220, 252, 231);

                    // SDK handles session connection and device activation internally
                    // Fire and forget - don't block UI
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await _licenseClient.ConnectSessionAsync();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Session connection error: {ex.Message}");
                        }
                    });

                    RefreshFeaturesList();

                    var license = result.License!;
                    MessageBox.Show(
                        $"License Validation Successful!\n\n" +
                        $"Type: {license.Type}\n" +
                        $"License Key: {license.LicenseKey}\n" +
                        $"Issued: {license.IssuedOn:yyyy-MM-dd}" +
                        (license.ExpirationDate.HasValue ? $"\nExpires: {license.ExpirationDate.Value:yyyy-MM-dd}" : ""),
                        "Valid License",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    var icon = result.Status switch
                    {
                        LicenseValidationStatus.DeviceBlocked => MessageBoxIcon.Stop,
                        LicenseValidationStatus.Revoked => MessageBoxIcon.Stop,
                        LicenseValidationStatus.Expired => MessageBoxIcon.Warning,
                        _ => MessageBoxIcon.Warning
                    };

                    MessageBox.Show(result.ErrorMessage, result.ErrorTitle, MessageBoxButtons.OK, icon);

                    var (statusText, statusColor, panelColor) = result.Status switch
                    {
                        LicenseValidationStatus.DeviceBlocked => ("DEVICE BLOCKED", Color.DarkRed, Color.FromArgb(254, 202, 202)),
                        LicenseValidationStatus.Revoked => ("REVOKED", Color.DarkRed, Color.FromArgb(254, 202, 202)),
                        LicenseValidationStatus.Expired => ("EXPIRED", Color.OrangeRed, Color.FromArgb(254, 226, 226)),
                        LicenseValidationStatus.CredentialsMismatch => ("Credentials mismatch", Color.Orange, Color.FromArgb(254, 243, 199)),
                        LicenseValidationStatus.WrongLicenseType => ("Wrong license type", Color.Orange, Color.FromArgb(254, 243, 199)),
                        LicenseValidationStatus.WrongFileType => ("Wrong file type", Color.Orange, Color.FromArgb(254, 243, 199)),
                        LicenseValidationStatus.SignatureMismatch => ("Signature mismatch", Color.Red, Color.FromArgb(254, 226, 226)),
                        LicenseValidationStatus.InvalidFormat => ("Invalid format", Color.Red, Color.FromArgb(254, 226, 226)),
                        _ => ("Validation failed", Color.Red, Color.FromArgb(254, 226, 226))
                    };
                    UpdateStatus(statusText, statusColor);
                    pnlStatus.BackColor = panelColor;
                    lstFeatures.Items.Clear();
                }
            }
            finally
            {
                TryDeleteRuntimeLicense();
            }
        }

        private async void btnValidateOffline_Click(object? sender, EventArgs e)
        {
            LoadStoredCredentialsIntoUi();

            // Check if cached license exists
            var protectedPath = GetProtectedLicensePath();
            if (!File.Exists(protectedPath))
            {
                MessageBox.Show(
                    "No cached license found.\n\n" +
                    "Please validate online first to create a cached license file.",
                    "Cache Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // Check if credentials are provided
            if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
            {
                MessageBox.Show(
                    "Please enter License Key to decrypt the cached license.",
                    "Credentials Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (_licenseClient != null)
            {
                await _licenseClient.DisposeAsync();
                _licenseClient = null;
            }

            try
            {
                // Load and decrypt the cached protected license
                var protectedBytes = await File.ReadAllBytesAsync(protectedPath);
                byte[] plainBytes;

                try
                {
                    plainBytes = await LicenseFileProtector.UnprotectAsync(protectedBytes, txtLicenseKey.Text);
                }
                catch (CryptographicException)
                {
                    MessageBox.Show(
                        "Cannot decrypt cached license.\n\n" +
                        "Check that your License Key matches the original validation.",
                        "Decryption Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // Save to runtime path
                var runtimePath = GetRuntimeLicensePath();
                await File.WriteAllBytesAsync(runtimePath, plainBytes);

                // Create client for offline validation
                _licenseClient = new LicenseClient(new LicenseClientOptions
                {
                    LicenseFilePath = runtimePath,
                    PublicKey = txtPublicKey.Text,
                    LicenseKey = txtLicenseKey.Text,
                    ExpectedLicenseType = null,
                    AllowNonBinFiles = true
                });

                // Validate offline (signature check only, no server call)
                var result = await _licenseClient.ValidateOfflineAsync();

                if (result.IsValid)
                {
                    UpdateStatus("VALID (OFFLINE)", Color.DodgerBlue);
                    pnlStatus.BackColor = Color.FromArgb(219, 234, 254);
                    RefreshFeaturesList();

                    var license = result.License!;
                    MessageBox.Show(
                        $"Offline License Validation Successful!\n\n" +
                        $"Type: {license.Type}\n" +
                        $"License Key: {license.LicenseKey}\n" +
                        $"Issued: {license.IssuedOn:yyyy-MM-dd}" +
                        (license.ExpirationDate.HasValue ? $"\nExpires: {license.ExpirationDate.Value:yyyy-MM-dd}" : "") +
                        "\n\n(No server connection required)",
                        "Valid License (Offline)",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        result.ErrorMessage ?? "Offline validation failed.",
                        "Validation Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    UpdateStatus("OFFLINE VALIDATION FAILED", Color.OrangeRed);
                    pnlStatus.BackColor = Color.FromArgb(254, 226, 226);
                    lstFeatures.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error during offline validation:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                UpdateStatus("Error", Color.Red);
                pnlStatus.BackColor = Color.FromArgb(254, 226, 226);
            }
            finally
            {
                TryDeleteRuntimeLicense();
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
            lblStatus.Text = "Not Validated";
            lblStatus.ForeColor = Color.Gray;
            lblValidationTime.Text = "";
            pnlStatus.BackColor = Color.FromArgb(243, 244, 246);
            lstFeatures.Items.Clear();
        }

        private void btnUseCachedLicense_Click(object? sender, EventArgs e)
        {
            var protectedPath = GetProtectedLicensePath();
            if (!File.Exists(protectedPath))
            {
                MessageBox.Show("No protected license was found in app bin folder yet. Validate once with a license file first.",
                    "Cache Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtLicenseFile.Text = protectedPath;
            UpdateStatus("Protected bin license selected", Color.DodgerBlue);
            pnlStatus.BackColor = Color.FromArgb(219, 234, 254);
        }

        private void btnClearCachedLicense_Click(object? sender, EventArgs e)
        {
            var protectedPath = GetProtectedLicensePath();
            var runtimePath = GetRuntimeLicensePath();
            var hasProtected = File.Exists(protectedPath);
            var hasRuntime = File.Exists(runtimePath);

            if (!hasProtected && !hasRuntime)
            {
                MessageBox.Show("Protected license is already empty.", "Cache", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (hasProtected) File.Delete(protectedPath);
                if (hasRuntime) File.Delete(runtimePath);

                ClearStoredCredentials();
                if (string.Equals(txtLicenseFile.Text, protectedPath, StringComparison.OrdinalIgnoreCase))
                    txtLicenseFile.Text = "";

                UpdateStatus("Protected license cleared", Color.OrangeRed);
                pnlStatus.BackColor = Color.FromArgb(254, 242, 242);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to clear protected license:\n" + ex.Message, "Cache Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshFeaturesList()
        {
            lstFeatures.Items.Clear();
            
            var allFeatures = FeatureManager.GetAllFeatures();
            if (allFeatures == null || allFeatures.Count == 0)
            {
                lstFeatures.Items.Add("(No features assigned)");
                return;
            }

            foreach (var feature in allFeatures)
            {
                var isEnabled = FeatureManager.IsFeatureEnabled(feature.Key);
                var status = isEnabled ? "✓" : "✗";
                var displayText = $"{status} {feature.Key}";
                lstFeatures.Items.Add(displayText);
            }
        }

        private void UpdateStatus(string message, Color color)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;
            lblValidationTime.Text = "Validated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void OnSessionDisconnected(string message)
        {
            UpdateStatus("Session Disconnected", Color.Orange);
            MessageBox.Show($"Session disconnected:\n{message}", "Session Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static string GetProtectedLicensePath()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDir, ProtectedLicenseFileName);
        }

        private static string GetProtectedCredentialsPath()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDir, ProtectedCredentialsFileName);
        }

        private static string GetRuntimeLicensePath()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDir, RuntimeLicenseFileName);
        }

        private void EnsureLicensePathFromProtectedStore()
        {
            if (!string.IsNullOrWhiteSpace(txtLicenseFile.Text) && File.Exists(txtLicenseFile.Text))
                return;

            var protectedPath = GetProtectedLicensePath();
            if (File.Exists(protectedPath))
            {
                txtLicenseFile.Text = protectedPath;
                UpdateStatus("Using protected bin license file", Color.DodgerBlue);
                pnlStatus.BackColor = Color.FromArgb(219, 234, 254);
            }
        }

        private async Task<string?> PrepareRuntimeLicenseFileAsync()
        {
            var sourcePath = txtLicenseFile.Text;
            if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
            {
                MessageBox.Show("Please select a valid license file.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            var runtimePath = GetRuntimeLicensePath();
            try
            {
                var sourceBytes = await File.ReadAllBytesAsync(sourcePath);
                byte[] plainBytes;

                if (LicenseFileProtector.LooksProtected(sourceBytes))
                {
                    plainBytes = await LicenseFileProtector.UnprotectAsync(sourceBytes, txtLicenseKey.Text);
                }
                else
                {
                    plainBytes = sourceBytes;
                }

                await File.WriteAllBytesAsync(runtimePath, plainBytes);
                return runtimePath;
            }
            catch (CryptographicException)
            {
                MessageBox.Show("Cannot decrypt protected license. Check your License Key.", "Decryption Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to prepare runtime license file:\n" + ex.Message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private async Task<bool> SaveLicenseToProtectedStoreAsync(string sourcePath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
                return false;

            if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
            {
                MessageBox.Show(
                    "License Key is required to protect and cache the license file.",
                    "Credentials Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                var plainBytes = await File.ReadAllBytesAsync(sourcePath);
                var protectedBytes = await LicenseFileProtector.ProtectAsync(plainBytes, txtLicenseKey.Text);
                var protectedPath = GetProtectedLicensePath();
                await File.WriteAllBytesAsync(protectedPath, protectedBytes);
                
                // Verify file was created
                if (File.Exists(protectedPath))
                {
                    System.Diagnostics.Debug.WriteLine($"Protected license saved: {protectedPath}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to save protected license:\n\n{ex.Message}",
                    "Cache Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
        }

        private sealed class StoredCredentials
        {
            public string LicenseKey { get; set; } = string.Empty;
            public string PublicKey { get; set; } = string.Empty;
        }

        private static byte[] GetCredentialsEntropy()
        {
            return Encoding.UTF8.GetBytes("LicenPro.SDK.TestApp.Credentials.v1");
        }

        private void SaveCredentialsSecurely(string licenseKey, string publicKey)
        {
            try
            {
                licenseKey = licenseKey?.Trim() ?? string.Empty;
                publicKey = publicKey?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(licenseKey) || string.IsNullOrWhiteSpace(publicKey))
                    return;

                var payload = new StoredCredentials
                {
                    LicenseKey = licenseKey,
                    PublicKey = publicKey
                };

                var json = JsonSerializer.Serialize(payload);
                var plain = Encoding.UTF8.GetBytes(json);
                var protectedBytes = ProtectedData.Protect(plain, GetCredentialsEntropy(), DataProtectionScope.CurrentUser);
                File.WriteAllBytes(GetProtectedCredentialsPath(), protectedBytes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveCredentialsSecurely error: {ex.Message}");
            }
        }

        private StoredCredentials? LoadStoredCredentials()
        {
            try
            {
                var path = GetProtectedCredentialsPath();
                if (!File.Exists(path))
                    return null;

                var protectedBytes = File.ReadAllBytes(path);
                var plain = ProtectedData.Unprotect(protectedBytes, GetCredentialsEntropy(), DataProtectionScope.CurrentUser);
                var json = Encoding.UTF8.GetString(plain);
                return JsonSerializer.Deserialize<StoredCredentials>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadStoredCredentials error: {ex.Message}");
                return null;
            }
        }

        private void LoadStoredCredentialsIntoUi()
        {
            var creds = LoadStoredCredentials();
            if (creds == null)
                return;

            if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
                txtLicenseKey.Text = creds.LicenseKey;

            if (string.IsNullOrWhiteSpace(txtPublicKey.Text))
                txtPublicKey.Text = creds.PublicKey;
        }

        private void ClearStoredCredentials()
        {
            try
            {
                var path = GetProtectedCredentialsPath();
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ClearStoredCredentials error: {ex.Message}");
            }
        }

        private async Task TryAutoValidateFromCacheAsync()
        {
            var protectedPath = GetProtectedLicensePath();
            if (!File.Exists(protectedPath))
                return;

            LoadStoredCredentialsIntoUi();

            if (string.IsNullOrWhiteSpace(txtLicenseKey.Text) || string.IsNullOrWhiteSpace(txtPublicKey.Text))
                return;

            txtLicenseFile.Text = protectedPath;
            UpdateStatus("Auto validating...", Color.DodgerBlue);
            pnlStatus.BackColor = Color.FromArgb(219, 234, 254);

            var runtimePath = await PrepareRuntimeLicenseFileAsync();
            if (runtimePath == null)
                return;

            if (_licenseClient != null)
            {
                await _licenseClient.DisposeAsync();
                _licenseClient = null;
            }

            _licenseClient = new LicenseClient(new LicenseClientOptions
            {
                LicenseFilePath = runtimePath,
                PublicKey = txtPublicKey.Text,
                LicenseKey = txtLicenseKey.Text,
                ExpectedLicenseType = null,
                AllowNonBinFiles = true
            });

            var result = await _licenseClient.ValidateOfflineAsync();
            if (result.IsValid)
            {
                UpdateStatus("VALID (AUTO)", Color.Green);
                pnlStatus.BackColor = Color.FromArgb(220, 252, 231);
                RefreshFeaturesList();

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _licenseClient.ConnectSessionAsync();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Session connection error: {ex.Message}");
                    }
                });
            }
            else
            {
                UpdateStatus("AUTO VALIDATION FAILED", Color.OrangeRed);
                pnlStatus.BackColor = Color.FromArgb(254, 226, 226);
                lstFeatures.Items.Clear();
            }
        }

        private static void TryDeleteRuntimeLicense()
        {
            try
            {
                var runtimePath = GetRuntimeLicensePath();
                if (File.Exists(runtimePath))
                    File.Delete(runtimePath);
            }
            catch
            {
            }
        }
    }
}
