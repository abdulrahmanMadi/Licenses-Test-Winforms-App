namespace Licenses_Test_Winforms_App
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            grpLicenseFile = new GroupBox();
            btnBrowseLicense = new Button();
            txtLicenseFile = new TextBox();
            lblLicenseFile = new Label();
            grpPublicKey = new GroupBox();
            btnBrowsePublicKey = new Button();
            txtPublicKey = new TextBox();
            lblPublicKey = new Label();
            grpCredentials = new GroupBox();
            txtLicenseKey = new TextBox();
            lblLicenseKey = new Label();
            grpUpdates = new GroupBox();
            btnOpenLicensedDownload = new Button();
            btnCheckUpdates = new Button();
            chkAutoCheckProductUpdates = new CheckBox();
            lblEffectiveUpdateVersion = new Label();
            txtProductId = new TextBox();
            lblUpdProductId = new Label();
            txtAppVersion = new TextBox();
            lblUpdAppVer = new Label();
            pnlStatus = new Panel();
            lblValidationTime = new Label();
            lblStatus = new Label();
            lblStatusTitle = new Label();
            btnClearCachedLicense = new Button();
            btnValidate = new Button();
            btnValidateOffline = new Button();
            btnClear = new Button();
            lstFeatures = new ListBox();
            lblFeatures = new Label();
            toolTipUi = new ToolTip(components);
            grpLicenseFile.SuspendLayout();
            grpPublicKey.SuspendLayout();
            grpCredentials.SuspendLayout();
            grpUpdates.SuspendLayout();
            pnlStatus.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitle.Location = new Point(23, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(353, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "LicenPro License Validator";
            // 
            // grpLicenseFile
            // 
            grpLicenseFile.Controls.Add(btnBrowseLicense);
            grpLicenseFile.Controls.Add(txtLicenseFile);
            grpLicenseFile.Controls.Add(lblLicenseFile);
            grpLicenseFile.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpLicenseFile.Location = new Point(23, 73);
            grpLicenseFile.Margin = new Padding(3, 4, 3, 4);
            grpLicenseFile.Name = "grpLicenseFile";
            grpLicenseFile.Padding = new Padding(3, 4, 3, 4);
            grpLicenseFile.Size = new Size(526, 107);
            grpLicenseFile.TabIndex = 0;
            grpLicenseFile.TabStop = false;
            grpLicenseFile.Text = "License File";
            // 
            // btnBrowseLicense
            // 
            btnBrowseLicense.Font = new Font("Segoe UI", 9F);
            btnBrowseLicense.Location = new Point(429, 59);
            btnBrowseLicense.Margin = new Padding(3, 4, 3, 4);
            btnBrowseLicense.Name = "btnBrowseLicense";
            btnBrowseLicense.Size = new Size(80, 33);
            btnBrowseLicense.TabIndex = 1;
            btnBrowseLicense.Text = "Browse...";
            btnBrowseLicense.Click += btnBrowseLicense_Click;
            // 
            // txtLicenseFile
            // 
            txtLicenseFile.Font = new Font("Segoe UI", 9F);
            txtLicenseFile.Location = new Point(17, 60);
            txtLicenseFile.Margin = new Padding(3, 4, 3, 4);
            txtLicenseFile.Name = "txtLicenseFile";
            txtLicenseFile.PlaceholderText = "C:\\path\\to\\license.bin";
            txtLicenseFile.Size = new Size(399, 27);
            txtLicenseFile.TabIndex = 0;
            // 
            // lblLicenseFile
            // 
            lblLicenseFile.AutoSize = true;
            lblLicenseFile.Font = new Font("Segoe UI", 9F);
            lblLicenseFile.Location = new Point(17, 33);
            lblLicenseFile.Name = "lblLicenseFile";
            lblLicenseFile.Size = new Size(157, 20);
            lblLicenseFile.TabIndex = 2;
            lblLicenseFile.Text = "License File Path (.bin):";
            // 
            // grpPublicKey
            // 
            grpPublicKey.Controls.Add(btnBrowsePublicKey);
            grpPublicKey.Controls.Add(txtPublicKey);
            grpPublicKey.Controls.Add(lblPublicKey);
            grpPublicKey.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpPublicKey.Location = new Point(23, 193);
            grpPublicKey.Margin = new Padding(3, 4, 3, 4);
            grpPublicKey.Name = "grpPublicKey";
            grpPublicKey.Padding = new Padding(3, 4, 3, 4);
            grpPublicKey.Size = new Size(526, 147);
            grpPublicKey.TabIndex = 1;
            grpPublicKey.TabStop = false;
            grpPublicKey.Text = "Public Key";
            // 
            // btnBrowsePublicKey
            // 
            btnBrowsePublicKey.Font = new Font("Segoe UI", 9F);
            btnBrowsePublicKey.Location = new Point(429, 59);
            btnBrowsePublicKey.Margin = new Padding(3, 4, 3, 4);
            btnBrowsePublicKey.Name = "btnBrowsePublicKey";
            btnBrowsePublicKey.Size = new Size(80, 33);
            btnBrowsePublicKey.TabIndex = 3;
            btnBrowsePublicKey.Text = "Load...";
            btnBrowsePublicKey.Click += btnBrowsePublicKey_Click;
            // 
            // txtPublicKey
            // 
            txtPublicKey.Font = new Font("Consolas", 8F);
            txtPublicKey.Location = new Point(17, 60);
            txtPublicKey.Margin = new Padding(3, 4, 3, 4);
            txtPublicKey.Multiline = true;
            txtPublicKey.Name = "txtPublicKey";
            txtPublicKey.PlaceholderText = "MIIBCgKCAQEA...";
            txtPublicKey.ScrollBars = ScrollBars.Vertical;
            txtPublicKey.Size = new Size(399, 72);
            txtPublicKey.TabIndex = 2;
            // 
            // lblPublicKey
            // 
            lblPublicKey.AutoSize = true;
            lblPublicKey.Font = new Font("Segoe UI", 9F);
            lblPublicKey.Location = new Point(17, 33);
            lblPublicKey.Name = "lblPublicKey";
            lblPublicKey.Size = new Size(234, 20);
            lblPublicKey.TabIndex = 4;
            lblPublicKey.Text = "RSA Public Key (Base64 encoded):";
            // 
            // grpCredentials
            // 
            grpCredentials.Controls.Add(txtLicenseKey);
            grpCredentials.Controls.Add(lblLicenseKey);
            grpCredentials.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpCredentials.Location = new Point(23, 353);
            grpCredentials.Margin = new Padding(3, 4, 3, 4);
            grpCredentials.Name = "grpCredentials";
            grpCredentials.Padding = new Padding(3, 4, 3, 4);
            grpCredentials.Size = new Size(526, 73);
            grpCredentials.TabIndex = 2;
            grpCredentials.TabStop = false;
            grpCredentials.Text = "License Key";
            // 
            // txtLicenseKey
            // 
            txtLicenseKey.Font = new Font("Segoe UI", 9F);
            txtLicenseKey.Location = new Point(114, 29);
            txtLicenseKey.Margin = new Padding(3, 4, 3, 4);
            txtLicenseKey.Name = "txtLicenseKey";
            txtLicenseKey.PlaceholderText = "XXXX-XXXX-XXXX-XXXX";
            txtLicenseKey.Size = new Size(394, 27);
            txtLicenseKey.TabIndex = 4;
            // 
            // lblLicenseKey
            // 
            lblLicenseKey.AutoSize = true;
            lblLicenseKey.Font = new Font("Segoe UI", 9F);
            lblLicenseKey.Location = new Point(17, 33);
            lblLicenseKey.Name = "lblLicenseKey";
            lblLicenseKey.Size = new Size(88, 20);
            lblLicenseKey.TabIndex = 5;
            lblLicenseKey.Text = "License Key:";
            // 
            // grpUpdates
            // 
            grpUpdates.Controls.Add(btnOpenLicensedDownload);
            grpUpdates.Controls.Add(btnCheckUpdates);
            grpUpdates.Controls.Add(chkAutoCheckProductUpdates);
            grpUpdates.Controls.Add(lblEffectiveUpdateVersion);
            grpUpdates.Controls.Add(txtProductId);
            grpUpdates.Controls.Add(lblUpdProductId);
            grpUpdates.Controls.Add(txtAppVersion);
            grpUpdates.Controls.Add(lblUpdAppVer);
            grpUpdates.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpUpdates.Location = new Point(23, 437);
            grpUpdates.Margin = new Padding(3, 4, 3, 4);
            grpUpdates.Name = "grpUpdates";
            grpUpdates.Padding = new Padding(3, 4, 3, 4);
            grpUpdates.Size = new Size(526, 203);
            grpUpdates.TabIndex = 8;
            grpUpdates.TabStop = false;
            grpUpdates.Text = "License-aware updates";
            // 
            // btnCheckUpdates
            // 
            btnCheckUpdates.Font = new Font("Segoe UI", 9F);
            btnCheckUpdates.Location = new Point(17, 165);
            btnCheckUpdates.Margin = new Padding(3, 4, 3, 4);
            btnCheckUpdates.Name = "btnCheckUpdates";
            btnCheckUpdates.Size = new Size(235, 35);
            btnCheckUpdates.TabIndex = 11;
            btnCheckUpdates.Text = "Check for updates";
            btnCheckUpdates.UseVisualStyleBackColor = true;
            btnCheckUpdates.Click += btnCheckUpdates_Click;
            // 
            // btnOpenLicensedDownload
            // 
            btnOpenLicensedDownload.Font = new Font("Segoe UI", 9F);
            btnOpenLicensedDownload.Location = new Point(265, 165);
            btnOpenLicensedDownload.Margin = new Padding(3, 4, 3, 4);
            btnOpenLicensedDownload.Name = "btnOpenLicensedDownload";
            btnOpenLicensedDownload.Size = new Size(244, 35);
            btnOpenLicensedDownload.TabIndex = 16;
            btnOpenLicensedDownload.Text = "Open download (licensed version)";
            btnOpenLicensedDownload.UseVisualStyleBackColor = true;
            btnOpenLicensedDownload.Click += btnOpenLicensedDownload_Click;
            // 
            // chkAutoCheckProductUpdates
            // 
            chkAutoCheckProductUpdates.AutoSize = true;
            chkAutoCheckProductUpdates.Font = new Font("Segoe UI", 9F);
            chkAutoCheckProductUpdates.Location = new Point(17, 139);
            chkAutoCheckProductUpdates.Margin = new Padding(3, 4, 3, 4);
            chkAutoCheckProductUpdates.Name = "chkAutoCheckProductUpdates";
            chkAutoCheckProductUpdates.Size = new Size(409, 24);
            chkAutoCheckProductUpdates.TabIndex = 12;
            chkAutoCheckProductUpdates.Text = "Automatically check for product updates (every 24 hours)";
            chkAutoCheckProductUpdates.UseVisualStyleBackColor = true;
            // 
            // lblEffectiveUpdateVersion
            // 
            lblEffectiveUpdateVersion.Font = new Font("Segoe UI", 8.25F);
            lblEffectiveUpdateVersion.ForeColor = Color.FromArgb(75, 85, 99);
            lblEffectiveUpdateVersion.Location = new Point(17, 99);
            lblEffectiveUpdateVersion.Name = "lblEffectiveUpdateVersion";
            lblEffectiveUpdateVersion.Size = new Size(491, 37);
            lblEffectiveUpdateVersion.TabIndex = 13;
            lblEffectiveUpdateVersion.Text = "Effective version for update check (X-Current-Version): —";
            // 
            // txtProductId
            // 
            txtProductId.Font = new Font("Consolas", 8F);
            txtProductId.Location = new Point(183, 64);
            txtProductId.Margin = new Padding(3, 4, 3, 4);
            txtProductId.Name = "txtProductId";
            txtProductId.PlaceholderText = "From dashboard product settings";
            txtProductId.Size = new Size(325, 23);
            txtProductId.TabIndex = 10;
            // 
            // lblUpdProductId
            // 
            lblUpdProductId.AutoSize = true;
            lblUpdProductId.Font = new Font("Segoe UI", 9F);
            lblUpdProductId.Location = new Point(183, 37);
            lblUpdProductId.Name = "lblUpdProductId";
            lblUpdProductId.Size = new Size(191, 20);
            lblUpdProductId.TabIndex = 14;
            lblUpdProductId.Text = "Product ID (optional GUID):";
            // 
            // txtAppVersion
            // 
            txtAppVersion.Font = new Font("Segoe UI", 9F);
            txtAppVersion.Location = new Point(17, 64);
            txtAppVersion.Margin = new Padding(3, 4, 3, 4);
            txtAppVersion.Name = "txtAppVersion";
            txtAppVersion.PlaceholderText = "e.g. 1.0.0";
            txtAppVersion.Size = new Size(137, 27);
            txtAppVersion.TabIndex = 9;
            // 
            // lblUpdAppVer
            // 
            lblUpdAppVer.AutoSize = true;
            lblUpdAppVer.Font = new Font("Segoe UI", 9F);
            lblUpdAppVer.Location = new Point(17, 37);
            lblUpdAppVer.Name = "lblUpdAppVer";
            lblUpdAppVer.Size = new Size(157, 20);
            lblUpdAppVer.TabIndex = 15;
            lblUpdAppVer.Text = "App / product version:";
            // 
            // pnlStatus
            // 
            pnlStatus.BackColor = Color.FromArgb(243, 244, 246);
            pnlStatus.BorderStyle = BorderStyle.FixedSingle;
            pnlStatus.Controls.Add(lblValidationTime);
            pnlStatus.Controls.Add(lblStatus);
            pnlStatus.Controls.Add(lblStatusTitle);
            pnlStatus.Location = new Point(592, 266);
            pnlStatus.Margin = new Padding(3, 4, 3, 4);
            pnlStatus.Name = "pnlStatus";
            pnlStatus.Size = new Size(525, 93);
            pnlStatus.TabIndex = 3;
            // 
            // lblValidationTime
            // 
            lblValidationTime.AutoSize = true;
            lblValidationTime.Font = new Font("Segoe UI", 8F);
            lblValidationTime.ForeColor = Color.Gray;
            lblValidationTime.Location = new Point(343, 67);
            lblValidationTime.Name = "lblValidationTime";
            lblValidationTime.Size = new Size(0, 19);
            lblValidationTime.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatus.ForeColor = Color.Gray;
            lblStatus.Location = new Point(17, 40);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(142, 28);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Not Validated";
            // 
            // lblStatusTitle
            // 
            lblStatusTitle.AutoSize = true;
            lblStatusTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatusTitle.Location = new Point(17, 13);
            lblStatusTitle.Name = "lblStatusTitle";
            lblStatusTitle.Size = new Size(131, 20);
            lblStatusTitle.TabIndex = 2;
            lblStatusTitle.Text = "Validation Status:";
            // 
            // btnClearCachedLicense
            // 
            btnClearCachedLicense.BackColor = Color.FromArgb(239, 68, 68);
            btnClearCachedLicense.FlatAppearance.BorderSize = 0;
            btnClearCachedLicense.FlatStyle = FlatStyle.Flat;
            btnClearCachedLicense.Font = new Font("Segoe UI", 9F);
            btnClearCachedLicense.ForeColor = Color.White;
            btnClearCachedLicense.Location = new Point(866, 373);
            btnClearCachedLicense.Margin = new Padding(3, 4, 3, 4);
            btnClearCachedLicense.Name = "btnClearCachedLicense";
            btnClearCachedLicense.Size = new Size(251, 45);
            btnClearCachedLicense.TabIndex = 5;
            btnClearCachedLicense.Text = "Clear Cached License";
            btnClearCachedLicense.UseVisualStyleBackColor = false;
            btnClearCachedLicense.Click += btnClearCachedLicense_Click;
            // 
            // btnValidate
            // 
            btnValidate.BackColor = Color.FromArgb(34, 197, 94);
            btnValidate.FlatAppearance.BorderSize = 0;
            btnValidate.FlatStyle = FlatStyle.Flat;
            btnValidate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnValidate.ForeColor = Color.White;
            btnValidate.Location = new Point(592, 373);
            btnValidate.Margin = new Padding(3, 4, 3, 4);
            btnValidate.Name = "btnValidate";
            btnValidate.Size = new Size(251, 53);
            btnValidate.TabIndex = 6;
            btnValidate.Text = "✓ Validate License";
            btnValidate.UseVisualStyleBackColor = false;
            btnValidate.Click += btnValidate_Click;
            // 
            // btnValidateOffline
            // 
            btnValidateOffline.BackColor = Color.FromArgb(59, 130, 246);
            btnValidateOffline.FlatAppearance.BorderSize = 0;
            btnValidateOffline.FlatStyle = FlatStyle.Flat;
            btnValidateOffline.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnValidateOffline.ForeColor = Color.White;
            btnValidateOffline.Location = new Point(866, 426);
            btnValidateOffline.Margin = new Padding(3, 4, 3, 4);
            btnValidateOffline.Name = "btnValidateOffline";
            btnValidateOffline.Size = new Size(251, 53);
            btnValidateOffline.TabIndex = 7;
            btnValidateOffline.Text = "📁 Validate from Cache";
            btnValidateOffline.UseVisualStyleBackColor = false;
            btnValidateOffline.Click += btnValidateOffline_Click;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.FromArgb(107, 114, 128);
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 10F);
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(592, 434);
            btnClear.Margin = new Padding(3, 4, 3, 4);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(251, 53);
            btnClear.TabIndex = 7;
            btnClear.Text = "Clear All";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // lstFeatures
            // 
            lstFeatures.Font = new Font("Segoe UI", 9F);
            lstFeatures.FormattingEnabled = true;
            lstFeatures.HorizontalScrollbar = true;
            lstFeatures.Location = new Point(592, 106);
            lstFeatures.Margin = new Padding(3, 4, 3, 4);
            lstFeatures.Name = "lstFeatures";
            lstFeatures.Size = new Size(525, 124);
            lstFeatures.TabIndex = 8;
            // 
            // lblFeatures
            // 
            lblFeatures.AutoSize = true;
            lblFeatures.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFeatures.Location = new Point(592, 82);
            lblFeatures.Name = "lblFeatures";
            lblFeatures.Size = new Size(141, 20);
            lblFeatures.TabIndex = 9;
            lblFeatures.Text = "Assigned Features:";
            // 
            // toolTipUi
            // 
            toolTipUi.AutoPopDelay = 25000;
            toolTipUi.InitialDelay = 350;
            toolTipUi.ReshowDelay = 150;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1179, 671);
            Controls.Add(lblTitle);
            Controls.Add(grpLicenseFile);
            Controls.Add(grpPublicKey);
            Controls.Add(grpCredentials);
            Controls.Add(grpUpdates);
            Controls.Add(pnlStatus);
            Controls.Add(btnClearCachedLicense);
            Controls.Add(btnValidate);
            Controls.Add(btnValidateOffline);
            Controls.Add(btnClear);
            Controls.Add(lblFeatures);
            Controls.Add(lstFeatures);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LicenPro - License Validator";
            Load += Form1_Load;
            grpLicenseFile.ResumeLayout(false);
            grpLicenseFile.PerformLayout();
            grpPublicKey.ResumeLayout(false);
            grpPublicKey.PerformLayout();
            grpCredentials.ResumeLayout(false);
            grpCredentials.PerformLayout();
            grpUpdates.ResumeLayout(false);
            grpUpdates.PerformLayout();
            pnlStatus.ResumeLayout(false);
            pnlStatus.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private GroupBox grpLicenseFile;
        private Button btnBrowseLicense;
        private TextBox txtLicenseFile;
        private Label lblLicenseFile;
        private GroupBox grpPublicKey;
        private Button btnBrowsePublicKey;
        private TextBox txtPublicKey;
        private Label lblPublicKey;
        private GroupBox grpCredentials;
        private TextBox txtLicenseKey;
        private Label lblLicenseKey;
        private GroupBox grpUpdates;
        private Label lblEffectiveUpdateVersion;
        private Label lblUpdAppVer;
        private TextBox txtAppVersion;
        private Label lblUpdProductId;
        private TextBox txtProductId;
        private CheckBox chkAutoCheckProductUpdates;
        private Button btnOpenLicensedDownload;
        private Button btnCheckUpdates;
        private Panel pnlStatus;
        private Label lblValidationTime;
        private Label lblStatus;
        private Label lblStatusTitle;
        private Button btnClearCachedLicense;
        private Button btnValidate;
        private Button btnValidateOffline;
        private Button btnClear;
        private Label lblFeatures;
        private ListBox lstFeatures;
        private ToolTip toolTipUi;
    }
}
