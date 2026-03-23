using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LicenPro.Enums;
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
            if (_licenseClient != null)
            {
                await _licenseClient.DisposeAsync();
                _licenseClient = null;
            }

            // SDK will auto-detect license type
            _licenseClient = new LicenseClient(new LicenseClientOptions
            {
                LicenseFilePath = txtLicenseFile.Text,
                PublicKey = txtPublicKey.Text,
                UserName = txtUserName.Text,
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

            var result = await _licenseClient.ValidateAsync();
            if (result.IsValid)
            {
                UpdateStatus("VALID!", Color.Green);
                pnlStatus.BackColor = Color.FromArgb(220, 252, 231);
                
                // SDK handles session connection and device activation internally
                await _licenseClient.ConnectSessionAsync();
                
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

        private async void btnClear_Click(object? sender, EventArgs e)
        {
            if (_licenseClient != null)
            {
                await _licenseClient.DisposeAsync();
                _licenseClient = null;
            }

            txtLicenseFile.Text = "";
            txtPublicKey.Text = "";
            txtUserName.Text = "";
            txtLicenseKey.Text = "";
            lblStatus.Text = "Not Validated";
            lblStatus.ForeColor = Color.Gray;
            lblValidationTime.Text = "";
            pnlStatus.BackColor = Color.FromArgb(243, 244, 246);
            lstFeatures.Items.Clear();
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
    }
}
