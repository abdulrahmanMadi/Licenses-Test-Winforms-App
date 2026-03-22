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
            txtUserName = new TextBox();
            lblUserName = new Label();
            chkForceOnline = new CheckBox();
            pnlStatus = new Panel();
            lblValidationTime = new Label();
            lblStatus = new Label();
            lblStatusTitle = new Label();
            btnValidate = new Button();
            btnClear = new Button();
            lstFeatures = new ListBox();
            lblFeatures = new Label();
            grpLicenseFile.SuspendLayout();
            grpPublicKey.SuspendLayout();
            grpCredentials.SuspendLayout();
            pnlStatus.SuspendLayout();
            SuspendLayout();

            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(280, 30);
            lblTitle.Text = "LicenPro License Validator";

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

            lblLicenseFile.AutoSize = true;
            lblLicenseFile.Font = new Font("Segoe UI", 9F);
            lblLicenseFile.Location = new Point(15, 25);
            lblLicenseFile.Name = "lblLicenseFile";
            lblLicenseFile.Size = new Size(130, 15);
            lblLicenseFile.Text = "License File Path (.bin):";

            txtLicenseFile.Font = new Font("Segoe UI", 9F);
            txtLicenseFile.Location = new Point(15, 45);
            txtLicenseFile.Name = "txtLicenseFile";
            txtLicenseFile.PlaceholderText = "C:\\path\\to\\license.bin";
            txtLicenseFile.Size = new Size(350, 23);
            txtLicenseFile.TabIndex = 0;

            btnBrowseLicense.Font = new Font("Segoe UI", 9F);
            btnBrowseLicense.Location = new Point(375, 44);
            btnBrowseLicense.Name = "btnBrowseLicense";
            btnBrowseLicense.Size = new Size(70, 25);
            btnBrowseLicense.TabIndex = 1;
            btnBrowseLicense.Text = "Browse...";
            btnBrowseLicense.Click += new EventHandler(btnBrowseLicense_Click);

            grpPublicKey.Controls.Add(btnBrowsePublicKey);
            grpPublicKey.Controls.Add(txtPublicKey);
            grpPublicKey.Controls.Add(lblPublicKey);
            grpPublicKey.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpPublicKey.Location = new Point(20, 145);
            grpPublicKey.Name = "grpPublicKey";
            grpPublicKey.Size = new Size(460, 110);
            grpPublicKey.TabIndex = 1;
            grpPublicKey.TabStop = false;
            grpPublicKey.Text = "Public Key";

            lblPublicKey.AutoSize = true;
            lblPublicKey.Font = new Font("Segoe UI", 9F);
            lblPublicKey.Location = new Point(15, 25);
            lblPublicKey.Name = "lblPublicKey";
            lblPublicKey.Size = new Size(200, 15);
            lblPublicKey.Text = "RSA Public Key (Base64 encoded):";

            txtPublicKey.Font = new Font("Consolas", 8F);
            txtPublicKey.Location = new Point(15, 45);
            txtPublicKey.Multiline = true;
            txtPublicKey.Name = "txtPublicKey";
            txtPublicKey.PlaceholderText = "MIIBCgKCAQEA...";
            txtPublicKey.ScrollBars = ScrollBars.Vertical;
            txtPublicKey.Size = new Size(350, 55);
            txtPublicKey.TabIndex = 2;

            btnBrowsePublicKey.Font = new Font("Segoe UI", 9F);
            btnBrowsePublicKey.Location = new Point(375, 44);
            btnBrowsePublicKey.Name = "btnBrowsePublicKey";
            btnBrowsePublicKey.Size = new Size(70, 25);
            btnBrowsePublicKey.TabIndex = 3;
            btnBrowsePublicKey.Text = "Load...";
            btnBrowsePublicKey.Click += new EventHandler(btnBrowsePublicKey_Click);

            grpCredentials.Controls.Add(txtLicenseKey);
            grpCredentials.Controls.Add(lblLicenseKey);
            grpCredentials.Controls.Add(txtUserName);
            grpCredentials.Controls.Add(lblUserName);
            grpCredentials.Controls.Add(chkForceOnline);
            grpCredentials.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpCredentials.Location = new Point(20, 265);
            grpCredentials.Name = "grpCredentials";
            grpCredentials.Size = new Size(460, 100);
            grpCredentials.TabIndex = 2;
            grpCredentials.TabStop = false;
            grpCredentials.Text = "License Credentials";

            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI", 9F);
            lblUserName.Location = new Point(15, 25);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(68, 15);
            lblUserName.Text = "User Name:";

            txtUserName.Font = new Font("Segoe UI", 9F);
            txtUserName.Location = new Point(15, 45);
            txtUserName.Name = "txtUserName";
            txtUserName.PlaceholderText = "john.doe@example.com";
            txtUserName.Size = new Size(200, 23);
            txtUserName.TabIndex = 4;

            lblLicenseKey.AutoSize = true;
            lblLicenseKey.Font = new Font("Segoe UI", 9F);
            lblLicenseKey.Location = new Point(230, 25);
            lblLicenseKey.Name = "lblLicenseKey";
            lblLicenseKey.Size = new Size(71, 15);
            lblLicenseKey.Text = "License Key:";

            txtLicenseKey.Font = new Font("Segoe UI", 9F);
            txtLicenseKey.Location = new Point(90, 68);
            txtLicenseKey.Name = "txtLicenseKey";
            txtLicenseKey.PlaceholderText = "XXXX-XXXX-XXXX-XXXX";
            txtLicenseKey.Size = new Size(245, 23);
            txtLicenseKey.TabIndex = 5;

            chkForceOnline.AutoSize = true;
            chkForceOnline.Font = new Font("Segoe UI", 9F);
            chkForceOnline.Location = new Point(350, 45);
            chkForceOnline.Name = "chkForceOnline";
            chkForceOnline.Size = new Size(100, 19);
            chkForceOnline.TabIndex = 6;
            chkForceOnline.Text = "Force Online";
            chkForceOnline.UseVisualStyleBackColor = true;

            pnlStatus.BackColor = Color.FromArgb(243, 244, 246);
            pnlStatus.BorderStyle = BorderStyle.FixedSingle;
            pnlStatus.Controls.Add(lblValidationTime);
            pnlStatus.Controls.Add(lblStatus);
            pnlStatus.Controls.Add(lblStatusTitle);
            pnlStatus.Location = new Point(20, 375);
            pnlStatus.Name = "pnlStatus";
            pnlStatus.Size = new Size(460, 70);
            pnlStatus.TabIndex = 3;

            lblStatusTitle.AutoSize = true;
            lblStatusTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatusTitle.Location = new Point(15, 10);
            lblStatusTitle.Name = "lblStatusTitle";
            lblStatusTitle.Size = new Size(110, 15);
            lblStatusTitle.Text = "Validation Status:";

            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatus.ForeColor = Color.Gray;
            lblStatus.Location = new Point(15, 30);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(110, 21);
            lblStatus.Text = "Not Validated";

            lblValidationTime.AutoSize = true;
            lblValidationTime.Font = new Font("Segoe UI", 8F);
            lblValidationTime.ForeColor = Color.Gray;
            lblValidationTime.Location = new Point(300, 50);
            lblValidationTime.Name = "lblValidationTime";
            lblValidationTime.Size = new Size(0, 13);
            lblValidationTime.Text = "";

            btnValidate.BackColor = Color.FromArgb(34, 197, 94);
            btnValidate.FlatAppearance.BorderSize = 0;
            btnValidate.FlatStyle = FlatStyle.Flat;
            btnValidate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnValidate.ForeColor = Color.White;
            btnValidate.Location = new Point(20, 455);
            btnValidate.Name = "btnValidate";
            btnValidate.Size = new Size(220, 40);
            btnValidate.TabIndex = 4;
            btnValidate.Text = "✓ Validate License";
            btnValidate.UseVisualStyleBackColor = false;
            btnValidate.Click += btnValidate_Click;

            btnClear.BackColor = Color.FromArgb(107, 114, 128);
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 10F);
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(260, 455);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(220, 40);
            btnClear.TabIndex = 5;
            btnClear.Text = "Clear All";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;

            lblFeatures.AutoSize = true;
            lblFeatures.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFeatures.Location = new Point(20, 510);
            lblFeatures.Name = "lblFeatures";
            lblFeatures.Size = new Size(100, 15);
            lblFeatures.Text = "Assigned Features:";

            lstFeatures.Font = new Font("Segoe UI", 9F);
            lstFeatures.FormattingEnabled = true;
            lstFeatures.HorizontalScrollbar = true;
            lstFeatures.Location = new Point(20, 530);
            lstFeatures.Name = "lstFeatures";
            lstFeatures.Size = new Size(460, 95);
            lstFeatures.TabIndex = 6;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(500, 640);
            Controls.Add(lblTitle);
            Controls.Add(grpLicenseFile);
            Controls.Add(grpPublicKey);
            Controls.Add(grpCredentials);
            Controls.Add(pnlStatus);
            Controls.Add(btnValidate);
            Controls.Add(btnClear);
            Controls.Add(lblFeatures);
            Controls.Add(lstFeatures);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LicenPro - License Validator";
            grpLicenseFile.ResumeLayout(false);
            grpLicenseFile.PerformLayout();
            grpPublicKey.ResumeLayout(false);
            grpPublicKey.PerformLayout();
            grpCredentials.ResumeLayout(false);
            grpCredentials.PerformLayout();
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
        private TextBox txtUserName;
        private Label lblUserName;
        private CheckBox chkForceOnline;
        private Panel pnlStatus;
        private Label lblValidationTime;
        private Label lblStatus;
        private Label lblStatusTitle;
        private Button btnValidate;
        private Button btnClear;
        private Label lblFeatures;
        private ListBox lstFeatures;
    }
}
