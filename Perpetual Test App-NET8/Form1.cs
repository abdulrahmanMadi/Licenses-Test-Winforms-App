using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LicenPro.Exceptions;
using LicenPro.SDK;
using LicenPro.Utilities;

namespace Perpetual_Test_App_NET8
{
    public partial class Form1 : Form
    {
        private const string PagesEditFeatureName = "Pages Edit";
        private string _pagesEditFeatureKey = PagesEditFeatureName;
        private LicenseClient? _licenseClient;

        public Form1()
        {
            InitializeComponent();
            btnPagesEdit.Enabled = false;

            FormClosing += async (_, __) =>
            {
                try
                {
                    if (_licenseClient != null)
                        await _licenseClient.DisconnectAsync();
                }
                catch
                {
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
                    Title = "Select LicenPro Binary License File (.bin)",
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

            _licenseClient = new LicenseClient(new LicenseClientOptions
            {
                LicenseFilePath = txtLicenseFile.Text,
                PublicKey = txtPublicKey.Text,
                UserName = txtUserName.Text,
                LicenseKey = txtLicenseKey.Text,
                ExpectedLicenseType = LicenPro.Enums.LicenseType.Perpetual,
                AllowNonBinFiles = true,
                ForceOnlineValidation = true
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
                UpdateStatus("License is VALID!", Color.Green);
                pnlStatus.BackColor = Color.FromArgb(220, 252, 231);
                
                // Connect to session for dashboard visibility
                await _licenseClient.ConnectSessionAsync();
                
                // Show assigned features
                RefreshFeaturesList();
                
                MessageBox.Show(
                    "License Validation Successful!\n\nThe license is valid.",
                    "Valid License",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                RefreshFeatureUi();
            }
            else
            {
                var icon = result.Status switch
                {
                    LicenseValidationStatus.DeviceBlocked => MessageBoxIcon.Stop,
                    LicenseValidationStatus.Expired => MessageBoxIcon.Warning,
                    _ => MessageBoxIcon.Warning
                };

                MessageBox.Show(result.ErrorMessage, result.ErrorTitle, MessageBoxButtons.OK, icon);

                var (statusText, statusColor, panelColor) = result.Status switch
                {
                    LicenseValidationStatus.DeviceBlocked => ("DEVICE BLOCKED", Color.DarkRed, Color.FromArgb(254, 202, 202)),
                    LicenseValidationStatus.Expired => ("EXPIRED", Color.OrangeRed, Color.FromArgb(254, 226, 226)),
                    LicenseValidationStatus.CredentialsMismatch => ("Credentials don't match", Color.Orange, Color.FromArgb(254, 243, 199)),
                    LicenseValidationStatus.WrongLicenseType => ("Wrong type", Color.Orange, Color.FromArgb(254, 243, 199)),
                    LicenseValidationStatus.WrongFileType => ("Wrong file type", Color.Orange, Color.FromArgb(254, 243, 199)),
                    LicenseValidationStatus.SignatureMismatch => ("Wrong public key", Color.Red, Color.FromArgb(254, 226, 226)),
                    LicenseValidationStatus.InvalidFormat => ("Invalid license format", Color.Red, Color.FromArgb(254, 226, 226)),
                    _ => ("Validation failed", Color.Red, Color.FromArgb(254, 226, 226))
                };
                UpdateStatus(statusText, statusColor);
                pnlStatus.BackColor = panelColor;
                btnPagesEdit.Enabled = false;
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
            btnPagesEdit.Enabled = false;
            lstFeatures.Items.Clear();
        }

        private void RefreshFeatureUi()
        {
            var all = FeatureManager.GetAllFeatures();
            if (all != null)
            {
                var exact = all.Keys.FirstOrDefault(k => string.Equals(k, PagesEditFeatureName, StringComparison.Ordinal));
                if (exact != null)
                    _pagesEditFeatureKey = exact;
                else
                {
                    var match = all.Keys.FirstOrDefault(k => string.Equals(k?.Trim(), PagesEditFeatureName, StringComparison.OrdinalIgnoreCase));
                    if (match != null) _pagesEditFeatureKey = match;
                }
            }

            btnPagesEdit.Enabled = FeatureManager.IsFeatureEnabled(_pagesEditFeatureKey);
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
                var featureType = feature.Value.Type.ToString() ?? "Unknown";
                var displayText = $"{status} {feature.Key} ({featureType})";
                lstFeatures.Items.Add(displayText);
            }
        }

        private void btnPagesEdit_Click(object? sender, EventArgs e)
        {
            using var form = new PagesEditorForm(_pagesEditFeatureKey);
            form.ShowDialog(this);
            RefreshFeatureUi();
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

    public sealed class PagesEditorForm : Form
    {
        private readonly string _featureKey;
        private readonly RichTextBox _editor;
        private readonly Button _btnSave;
        private readonly Label _lblTitle;

        public PagesEditorForm(string featureKey)
        {
            _featureKey = featureKey;

            Text = "Pages Editor";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(640, 420);

            _lblTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(16, 16),
                Text = "Pages Edit"
            };

            _editor = new RichTextBox
            {
                Location = new Point(16, 56),
                Size = new Size(608, 300),
                Font = new Font("Segoe UI", 10F),
                Text = "Try editing this text. Save should only be available when the feature is licensed."
            };

            _btnSave = new Button
            {
                Location = new Point(16, 372),
                Size = new Size(180, 32),
                Text = "Save",
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Click += OnSaveClick;

            Controls.Add(_lblTitle);
            Controls.Add(_editor);
            Controls.Add(_btnSave);

            Shown += OnShown;
        }

        private void OnShown(object? sender, EventArgs e)
        {
            try
            {
                FeatureManager.ThrowIfNotAllowed(_featureKey, trackUsage: true);
                _btnSave.Enabled = true;
                _editor.ReadOnly = false;
            }
            catch (FeatureNotLicensedException ex)
            {
                _btnSave.Enabled = false;
                _editor.ReadOnly = true;
                MessageBox.Show(ex.Message, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }

        private void OnSaveClick(object? sender, EventArgs e)
        {
            FeatureManager.TrackFeatureUsage(_featureKey, new() { ["action"] = "save" });
            MessageBox.Show("Saved (demo).", "Pages Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
