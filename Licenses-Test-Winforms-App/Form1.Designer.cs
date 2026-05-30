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
            lblStatusDetail = new Label();
            lblStatus = new Label();
            lblStatusTitle = new Label();
            btnClearCachedLicense = new Button();
            btnValidate = new Button();
            btnValidateOffline = new Button();
            btnClear = new Button();
            grpLicenseDetails = new GroupBox();
            lblDetailTypeValue = new Label();
            lblDetailType = new Label();
            lblDetailModeValue = new Label();
            lblDetailMode = new Label();
            lblDetailIssuerValue = new Label();
            lblDetailIssuer = new Label();
            lblDetailIssuedValue = new Label();
            lblDetailIssued = new Label();
            lblDetailNameValue = new Label();
            lblDetailName = new Label();
            lstFeatures = new ListBox();
            lblFeatures = new Label();
            toolTipUi = new ToolTip(components);
            grpLicenseFile.SuspendLayout();
            grpPublicKey.SuspendLayout();
            grpCredentials.SuspendLayout();
            grpUpdates.SuspendLayout();
            pnlStatus.SuspendLayout();
            grpLicenseDetails.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(283, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "LicenPro License Validator";
            // 
            // grpLicenseFile
            // 
            grpLicenseFile.Controls.Add(btnBrowseLicense);
            grpLicenseFile.Controls.Add(txtLicenseFile);
            grpLicenseFile.Controls.Add(lblLicenseFile);
            grpLicenseFile.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpLicenseFile.Location = new Point(20, 55);
            grpLicenseFile.Name = "grpLicenseFile";
            grpLicenseFile.Size = new Size(460, 80);
            grpLicenseFile.TabIndex = 0;
            grpLicenseFile.TabStop = false;
            grpLicenseFile.Text = "License File";
            // 
            // btnBrowseLicense
            // 
            btnBrowseLicense.Font = new Font("Segoe UI", 9F);
            btnBrowseLicense.Location = new Point(375, 44);
            btnBrowseLicense.Name = "btnBrowseLicense";
            btnBrowseLicense.Size = new Size(70, 25);
            btnBrowseLicense.TabIndex = 1;
            btnBrowseLicense.Text = "Browse...";
            btnBrowseLicense.Click += btnBrowseLicense_Click;
            // 
            // txtLicenseFile
            // 
            txtLicenseFile.Font = new Font("Segoe UI", 9F);
            txtLicenseFile.Location = new Point(15, 45);
            txtLicenseFile.Name = "txtLicenseFile";
            txtLicenseFile.PlaceholderText = "C:\\path\\to\\license.bin";
            txtLicenseFile.Size = new Size(350, 23);
            txtLicenseFile.TabIndex = 0;
            // 
            // lblLicenseFile
            // 
            lblLicenseFile.AutoSize = true;
            lblLicenseFile.Font = new Font("Segoe UI", 9F);
            lblLicenseFile.Location = new Point(15, 25);
            lblLicenseFile.Name = "lblLicenseFile";
            lblLicenseFile.Size = new Size(128, 15);
            lblLicenseFile.TabIndex = 2;
            lblLicenseFile.Text = "License File Path (.bin):";
            // 
            // grpPublicKey
            // 
            grpPublicKey.Controls.Add(btnBrowsePublicKey);
            grpPublicKey.Controls.Add(txtPublicKey);
            grpPublicKey.Controls.Add(lblPublicKey);
            grpPublicKey.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpPublicKey.Location = new Point(20, 145);
            grpPublicKey.Name = "grpPublicKey";
            grpPublicKey.Size = new Size(460, 122);
            grpPublicKey.TabIndex = 1;
            grpPublicKey.TabStop = false;
            grpPublicKey.Text = "Public Key";
            // 
            // btnBrowsePublicKey
            // 
            btnBrowsePublicKey.Font = new Font("Segoe UI", 9F);
            btnBrowsePublicKey.Location = new Point(375, 44);
            btnBrowsePublicKey.Name = "btnBrowsePublicKey";
            btnBrowsePublicKey.Size = new Size(70, 25);
            btnBrowsePublicKey.TabIndex = 3;
            btnBrowsePublicKey.Text = "Load...";
            btnBrowsePublicKey.Click += btnBrowsePublicKey_Click;
            // 
            // txtPublicKey
            // 
            txtPublicKey.Font = new Font("Consolas", 8F);
            txtPublicKey.Location = new Point(15, 45);
            txtPublicKey.Multiline = true;
            txtPublicKey.Name = "txtPublicKey";
            txtPublicKey.PlaceholderText = "MIIBCgKCAQEA...";
            txtPublicKey.ScrollBars = ScrollBars.Vertical;
            txtPublicKey.Size = new Size(350, 60);
            txtPublicKey.TabIndex = 2;
            // 
            // lblPublicKey
            // 
            lblPublicKey.AutoSize = true;
            lblPublicKey.Font = new Font("Segoe UI", 9F);
            lblPublicKey.Location = new Point(15, 25);
            lblPublicKey.Name = "lblPublicKey";
            lblPublicKey.Size = new Size(185, 15);
            lblPublicKey.TabIndex = 4;
            lblPublicKey.Text = "RSA Public Key (Base64 encoded):";
            // 
            // grpCredentials
            // 
            grpCredentials.Controls.Add(txtLicenseKey);
            grpCredentials.Controls.Add(lblLicenseKey);
            grpCredentials.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpCredentials.Location = new Point(20, 273);
            grpCredentials.Name = "grpCredentials";
            grpCredentials.Size = new Size(460, 55);
            grpCredentials.TabIndex = 2;
            grpCredentials.TabStop = false;
            grpCredentials.Text = "License Key";
            // 
            // txtLicenseKey
            // 
            txtLicenseKey.Font = new Font("Segoe UI", 9F);
            txtLicenseKey.Location = new Point(100, 22);
            txtLicenseKey.Name = "txtLicenseKey";
            txtLicenseKey.PlaceholderText = "XXXX-XXXX-XXXX-XXXX";
            txtLicenseKey.Size = new Size(345, 23);
            txtLicenseKey.TabIndex = 4;
            // 
            // lblLicenseKey
            // 
            lblLicenseKey.AutoSize = true;
            lblLicenseKey.Font = new Font("Segoe UI", 9F);
            lblLicenseKey.Location = new Point(15, 25);
            lblLicenseKey.Name = "lblLicenseKey";
            lblLicenseKey.Size = new Size(71, 15);
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
            grpUpdates.Location = new Point(20, 336);
            grpUpdates.Name = "grpUpdates";
            grpUpdates.Size = new Size(460, 199);
            grpUpdates.TabIndex = 8;
            grpUpdates.TabStop = false;
            grpUpdates.Text = "License-aware updates";
            // 
            // btnOpenLicensedDownload
            // 
            btnOpenLicensedDownload.Font = new Font("Segoe UI", 9F);
            btnOpenLicensedDownload.Location = new Point(232, 138);
            btnOpenLicensedDownload.Name = "btnOpenLicensedDownload";
            btnOpenLicensedDownload.Size = new Size(214, 26);
            btnOpenLicensedDownload.TabIndex = 16;
            btnOpenLicensedDownload.Text = "Open download (licensed version)";
            btnOpenLicensedDownload.UseVisualStyleBackColor = true;
            btnOpenLicensedDownload.Click += btnOpenLicensedDownload_Click;
            // 
            // btnCheckUpdates
            // 
            btnCheckUpdates.Font = new Font("Segoe UI", 9F);
            btnCheckUpdates.Location = new Point(15, 138);
            btnCheckUpdates.Name = "btnCheckUpdates";
            btnCheckUpdates.Size = new Size(206, 26);
            btnCheckUpdates.TabIndex = 11;
            btnCheckUpdates.Text = "Check for updates";
            btnCheckUpdates.UseVisualStyleBackColor = true;
            btnCheckUpdates.Click += btnCheckUpdates_Click;
            // 
            // chkAutoCheckProductUpdates
            // 
            chkAutoCheckProductUpdates.AutoSize = true;
            chkAutoCheckProductUpdates.Font = new Font("Segoe UI", 9F);
            chkAutoCheckProductUpdates.Location = new Point(15, 118);
            chkAutoCheckProductUpdates.Name = "chkAutoCheckProductUpdates";
            chkAutoCheckProductUpdates.Size = new Size(329, 19);
            chkAutoCheckProductUpdates.TabIndex = 12;
            chkAutoCheckProductUpdates.Text = "Automatically check for product updates (every 24 hours)";
            chkAutoCheckProductUpdates.UseVisualStyleBackColor = true;
            // 
            // lblEffectiveUpdateVersion
            // 
            lblEffectiveUpdateVersion.Font = new Font("Segoe UI", 8.25F);
            lblEffectiveUpdateVersion.ForeColor = Color.FromArgb(75, 85, 99);
            lblEffectiveUpdateVersion.Location = new Point(15, 74);
            lblEffectiveUpdateVersion.Name = "lblEffectiveUpdateVersion";
            lblEffectiveUpdateVersion.Size = new Size(430, 28);
            lblEffectiveUpdateVersion.TabIndex = 13;
            lblEffectiveUpdateVersion.Text = "Effective version for update check (X-Current-Version): —";
            // 
            // txtProductId
            // 
            txtProductId.Font = new Font("Consolas", 8F);
            txtProductId.Location = new Point(160, 48);
            txtProductId.Name = "txtProductId";
            txtProductId.PlaceholderText = "From dashboard product settings";
            txtProductId.Size = new Size(285, 20);
            txtProductId.TabIndex = 10;
            // 
            // lblUpdProductId
            // 
            lblUpdProductId.AutoSize = true;
            lblUpdProductId.Font = new Font("Segoe UI", 9F);
            lblUpdProductId.Location = new Point(160, 28);
            lblUpdProductId.Name = "lblUpdProductId";
            lblUpdProductId.Size = new Size(151, 15);
            lblUpdProductId.TabIndex = 14;
            lblUpdProductId.Text = "Product ID (optional GUID):";
            // 
            // txtAppVersion
            // 
            txtAppVersion.Font = new Font("Segoe UI", 9F);
            txtAppVersion.Location = new Point(15, 48);
            txtAppVersion.Name = "txtAppVersion";
            txtAppVersion.PlaceholderText = "e.g. 1.0.0";
            txtAppVersion.Size = new Size(120, 23);
            txtAppVersion.TabIndex = 9;
            // 
            // lblUpdAppVer
            // 
            lblUpdAppVer.AutoSize = true;
            lblUpdAppVer.Font = new Font("Segoe UI", 9F);
            lblUpdAppVer.Location = new Point(15, 28);
            lblUpdAppVer.Name = "lblUpdAppVer";
            lblUpdAppVer.Size = new Size(126, 15);
            lblUpdAppVer.TabIndex = 15;
            lblUpdAppVer.Text = "App / product version:";
            // 
            // pnlStatus
            // 
            pnlStatus.BackColor = Color.FromArgb(243, 244, 246);
            pnlStatus.BorderStyle = BorderStyle.FixedSingle;
            pnlStatus.Controls.Add(lblValidationTime);
            pnlStatus.Controls.Add(lblStatusDetail);
            pnlStatus.Controls.Add(lblStatus);
            pnlStatus.Controls.Add(lblStatusTitle);
            pnlStatus.Location = new Point(518, 342);
            pnlStatus.Name = "pnlStatus";
            pnlStatus.Size = new Size(460, 88);
            pnlStatus.TabIndex = 3;
            // 
            // lblValidationTime
            // 
            lblValidationTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblValidationTime.AutoSize = true;
            lblValidationTime.Font = new Font("Segoe UI", 8F);
            lblValidationTime.ForeColor = Color.Gray;
            lblValidationTime.Location = new Point(300, 68);
            lblValidationTime.Name = "lblValidationTime";
            lblValidationTime.Size = new Size(0, 13);
            lblValidationTime.TabIndex = 0;
            // 
            // lblStatusDetail
            // 
            lblStatusDetail.Font = new Font("Segoe UI", 9F);
            lblStatusDetail.ForeColor = Color.FromArgb(75, 85, 99);
            lblStatusDetail.Location = new Point(15, 52);
            lblStatusDetail.Name = "lblStatusDetail";
            lblStatusDetail.Size = new Size(430, 32);
            lblStatusDetail.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatus.ForeColor = Color.Gray;
            lblStatus.Location = new Point(15, 30);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(116, 21);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Not Validated";
            // 
            // lblStatusTitle
            // 
            lblStatusTitle.AutoSize = true;
            lblStatusTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatusTitle.Location = new Point(15, 10);
            lblStatusTitle.Name = "lblStatusTitle";
            lblStatusTitle.Size = new Size(102, 15);
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
            btnClearCachedLicense.Location = new Point(757, 453);
            btnClearCachedLicense.Name = "btnClearCachedLicense";
            btnClearCachedLicense.Size = new Size(220, 41);
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
            btnValidate.Location = new Point(518, 454);
            btnValidate.Name = "btnValidate";
            btnValidate.Size = new Size(220, 40);
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
            btnValidateOffline.Location = new Point(757, 500);
            btnValidateOffline.Name = "btnValidateOffline";
            btnValidateOffline.Size = new Size(220, 40);
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
            btnClear.Location = new Point(518, 499);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(220, 40);
            btnClear.TabIndex = 7;
            btnClear.Text = "Clear All";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // grpLicenseDetails
            // 
            grpLicenseDetails.Controls.Add(lblDetailTypeValue);
            grpLicenseDetails.Controls.Add(lblDetailType);
            grpLicenseDetails.Controls.Add(lblDetailModeValue);
            grpLicenseDetails.Controls.Add(lblDetailMode);
            grpLicenseDetails.Controls.Add(lblDetailIssuerValue);
            grpLicenseDetails.Controls.Add(lblDetailIssuer);
            grpLicenseDetails.Controls.Add(lblDetailIssuedValue);
            grpLicenseDetails.Controls.Add(lblDetailIssued);
            grpLicenseDetails.Controls.Add(lblDetailNameValue);
            grpLicenseDetails.Controls.Add(lblDetailName);
            grpLicenseDetails.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpLicenseDetails.Location = new Point(518, 55);
            grpLicenseDetails.Margin = new Padding(3, 2, 3, 2);
            grpLicenseDetails.Name = "grpLicenseDetails";
            grpLicenseDetails.Padding = new Padding(3, 2, 3, 2);
            grpLicenseDetails.Size = new Size(459, 130);
            grpLicenseDetails.TabIndex = 10;
            grpLicenseDetails.TabStop = false;
            grpLicenseDetails.Text = "License Details";
            // 
            // lblDetailTypeValue
            // 
            lblDetailTypeValue.AutoSize = true;
            lblDetailTypeValue.Font = new Font("Segoe UI", 9F);
            lblDetailTypeValue.Location = new Point(105, 101);
            lblDetailTypeValue.Name = "lblDetailTypeValue";
            lblDetailTypeValue.Size = new Size(19, 15);
            lblDetailTypeValue.TabIndex = 9;
            lblDetailTypeValue.Text = "—";
            // 
            // lblDetailType
            // 
            lblDetailType.AutoSize = true;
            lblDetailType.Font = new Font("Segoe UI", 9F);
            lblDetailType.Location = new Point(15, 101);
            lblDetailType.Name = "lblDetailType";
            lblDetailType.Size = new Size(34, 15);
            lblDetailType.TabIndex = 8;
            lblDetailType.Text = "Type:";
            // 
            // lblDetailModeValue
            // 
            lblDetailModeValue.AutoSize = true;
            lblDetailModeValue.Font = new Font("Segoe UI", 9F);
            lblDetailModeValue.Location = new Point(105, 86);
            lblDetailModeValue.Name = "lblDetailModeValue";
            lblDetailModeValue.Size = new Size(19, 15);
            lblDetailModeValue.TabIndex = 7;
            lblDetailModeValue.Text = "—";
            // 
            // lblDetailMode
            // 
            lblDetailMode.AutoSize = true;
            lblDetailMode.Font = new Font("Segoe UI", 9F);
            lblDetailMode.Location = new Point(15, 86);
            lblDetailMode.Name = "lblDetailMode";
            lblDetailMode.Size = new Size(41, 15);
            lblDetailMode.TabIndex = 6;
            lblDetailMode.Text = "Mode:";
            // 
            // lblDetailIssuerValue
            // 
            lblDetailIssuerValue.AutoEllipsis = true;
            lblDetailIssuerValue.Font = new Font("Segoe UI", 9F);
            lblDetailIssuerValue.Location = new Point(105, 65);
            lblDetailIssuerValue.Name = "lblDetailIssuerValue";
            lblDetailIssuerValue.Size = new Size(337, 15);
            lblDetailIssuerValue.TabIndex = 5;
            lblDetailIssuerValue.Text = "—";
            // 
            // lblDetailIssuer
            // 
            lblDetailIssuer.AutoSize = true;
            lblDetailIssuer.Font = new Font("Segoe UI", 9F);
            lblDetailIssuer.Location = new Point(15, 65);
            lblDetailIssuer.Name = "lblDetailIssuer";
            lblDetailIssuer.Size = new Size(40, 15);
            lblDetailIssuer.TabIndex = 4;
            lblDetailIssuer.Text = "Issuer:";
            // 
            // lblDetailIssuedValue
            // 
            lblDetailIssuedValue.AutoSize = true;
            lblDetailIssuedValue.Font = new Font("Segoe UI", 9F);
            lblDetailIssuedValue.Location = new Point(105, 45);
            lblDetailIssuedValue.Name = "lblDetailIssuedValue";
            lblDetailIssuedValue.Size = new Size(19, 15);
            lblDetailIssuedValue.TabIndex = 3;
            lblDetailIssuedValue.Text = "—";
            // 
            // lblDetailIssued
            // 
            lblDetailIssued.AutoSize = true;
            lblDetailIssued.Font = new Font("Segoe UI", 9F);
            lblDetailIssued.Location = new Point(15, 45);
            lblDetailIssued.Name = "lblDetailIssued";
            lblDetailIssued.Size = new Size(83, 15);
            lblDetailIssued.TabIndex = 2;
            lblDetailIssued.Text = "Created (UTC):";
            // 
            // lblDetailNameValue
            // 
            lblDetailNameValue.AutoEllipsis = true;
            lblDetailNameValue.Font = new Font("Segoe UI", 9F);
            lblDetailNameValue.Location = new Point(105, 25);
            lblDetailNameValue.Name = "lblDetailNameValue";
            lblDetailNameValue.Size = new Size(337, 15);
            lblDetailNameValue.TabIndex = 1;
            lblDetailNameValue.Text = "—";
            // 
            // lblDetailName
            // 
            lblDetailName.AutoSize = true;
            lblDetailName.Font = new Font("Segoe UI", 9F);
            lblDetailName.Location = new Point(15, 25);
            lblDetailName.Name = "lblDetailName";
            lblDetailName.Size = new Size(42, 15);
            lblDetailName.TabIndex = 0;
            lblDetailName.Text = "Name:";
            // 
            // lstFeatures
            // 
            lstFeatures.Font = new Font("Segoe UI", 9F);
            lstFeatures.FormattingEnabled = true;
            lstFeatures.HorizontalScrollbar = true;
            lstFeatures.ItemHeight = 15;
            lstFeatures.Location = new Point(518, 215);
            lstFeatures.Name = "lstFeatures";
            lstFeatures.Size = new Size(460, 109);
            lstFeatures.TabIndex = 8;
            // 
            // lblFeatures
            // 
            lblFeatures.AutoSize = true;
            lblFeatures.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFeatures.Location = new Point(518, 195);
            lblFeatures.Name = "lblFeatures";
            lblFeatures.Size = new Size(110, 15);
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
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1032, 578);
            Controls.Add(grpLicenseDetails);
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
            grpLicenseDetails.ResumeLayout(false);
            grpLicenseDetails.PerformLayout();
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
        private Label lblStatusDetail;
        private Label lblStatus;
        private Label lblStatusTitle;
        private Button btnClearCachedLicense;
        private Button btnValidate;
        private Button btnValidateOffline;
        private Button btnClear;
        private GroupBox grpLicenseDetails;
        private Label lblDetailName;
        private Label lblDetailNameValue;
        private Label lblDetailIssued;
        private Label lblDetailIssuedValue;
        private Label lblDetailIssuer;
        private Label lblDetailIssuerValue;
        private Label lblDetailMode;
        private Label lblDetailModeValue;
        private Label lblDetailType;
        private Label lblDetailTypeValue;
        private Label lblFeatures;
        private ListBox lstFeatures;
        private ToolTip toolTipUi;
    }
}
