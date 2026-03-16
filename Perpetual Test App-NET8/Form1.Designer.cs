namespace Perpetual_Test_App_NET8
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
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
            pnlStatus = new Panel();
            lblValidationTime = new Label();
            lblStatus = new Label();
            lblStatusTitle = new Label();
            btnValidate = new Button();
            btnClear = new Button();
            btnPagesEdit = new Button();
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
            lblTitle.Size = new Size(320, 30);
            lblTitle.Text = "Perpetual License Validator";

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
            grpCredentials.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpCredentials.Location = new Point(20, 265);
            grpCredentials.Name = "grpCredentials";
            grpCredentials.Size = new Size(460, 90);
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
            txtLicenseKey.Location = new Point(230, 45);
            txtLicenseKey.Name = "txtLicenseKey";
            txtLicenseKey.PlaceholderText = "XXXX-XXXX-XXXX-XXXX";
            txtLicenseKey.Size = new Size(215, 23);
            txtLicenseKey.TabIndex = 5;

            pnlStatus.BackColor = Color.FromArgb(243, 244, 246);
            pnlStatus.BorderStyle = BorderStyle.FixedSingle;
            pnlStatus.Controls.Add(lblValidationTime);
            pnlStatus.Controls.Add(lblStatus);
            pnlStatus.Controls.Add(lblStatusTitle);
            pnlStatus.Location = new Point(20, 365);
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
            btnValidate.Location = new Point(20, 445);
            btnValidate.Name = "btnValidate";
            btnValidate.Size = new Size(220, 40);
            btnValidate.TabIndex = 6;
            btnValidate.Text = "✓ Validate License";
            btnValidate.UseVisualStyleBackColor = false;
            btnValidate.Click += btnValidate_Click;

            btnClear.BackColor = Color.FromArgb(107, 114, 128);
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 10F);
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(260, 445);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(220, 40);
            btnClear.TabIndex = 7;
            btnClear.Text = "Clear All";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;

            btnPagesEdit.BackColor = Color.FromArgb(59, 130, 246);
            btnPagesEdit.FlatAppearance.BorderSize = 0;
            btnPagesEdit.FlatStyle = FlatStyle.Flat;
            btnPagesEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPagesEdit.ForeColor = Color.White;
            btnPagesEdit.Location = new Point(20, 495);
            btnPagesEdit.Name = "btnPagesEdit";
            btnPagesEdit.Size = new Size(460, 40);
            btnPagesEdit.TabIndex = 8;
            btnPagesEdit.Text = "Open Pages Editor (Pages Edit)";
            btnPagesEdit.UseVisualStyleBackColor = false;
            btnPagesEdit.Click += btnPagesEdit_Click;

            lblFeatures.AutoSize = true;
            lblFeatures.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFeatures.Location = new Point(20, 545);
            lblFeatures.Name = "lblFeatures";
            lblFeatures.Size = new Size(100, 15);
            lblFeatures.Text = "Assigned Features:";

            lstFeatures.Font = new Font("Segoe UI", 9F);
            lstFeatures.FormattingEnabled = true;
            lstFeatures.HorizontalScrollbar = true;
            lstFeatures.Location = new Point(20, 565);
            lstFeatures.Name = "lstFeatures";
            lstFeatures.Size = new Size(460, 95);
            lstFeatures.TabIndex = 9;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(500, 670);
            Controls.Add(lblTitle);
            Controls.Add(grpLicenseFile);
            Controls.Add(grpPublicKey);
            Controls.Add(grpCredentials);
            Controls.Add(pnlStatus);
            Controls.Add(btnValidate);
            Controls.Add(btnClear);
            Controls.Add(btnPagesEdit);
            Controls.Add(lblFeatures);
            Controls.Add(lstFeatures);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LicenPro - Perpetual License Test";
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
        private Panel pnlStatus;
        private Label lblValidationTime;
        private Label lblStatus;
        private Label lblStatusTitle;
        private Button btnValidate;
        private Button btnClear;
        private Button btnPagesEdit;
        private Label lblFeatures;
        private ListBox lstFeatures;
    }
}
