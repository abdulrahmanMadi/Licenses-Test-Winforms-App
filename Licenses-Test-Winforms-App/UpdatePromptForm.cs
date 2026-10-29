using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LicenPro.SDK.Updates;

namespace Licenses_Test_Winforms_App
{
    /// <summary>
    /// "Update available" window: release notes, then Update &amp; restart (download with progress, verify,
    /// install, restart), Later, or Skip this version. Mandatory updates offer Update &amp; restart only.
    /// </summary>
    internal sealed class UpdatePromptForm : Form
    {
        private readonly UpdateInfo _update;
        private readonly Button _btnUpdate = new() { Text = "Update && restart", AutoSize = true };
        private readonly Button _btnLater = new() { Text = "Later", AutoSize = true, DialogResult = DialogResult.Cancel };
        private readonly Button _btnSkip = new() { Text = "Skip this version", AutoSize = true };
        private readonly ProgressBar _progress = new() { Dock = DockStyle.Fill, Visible = false };
        private readonly Label _status = new() { Dock = DockStyle.Fill, AutoSize = false, ForeColor = Color.DimGray };

        public UpdatePromptForm(UpdateInfo update)
        {
            _update = update;

            Text = $"{(string.IsNullOrWhiteSpace(update.ProductName) ? "Product" : update.ProductName)} update";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ClientSize = new Size(480, 340);
            Font = new Font("Segoe UI", 9F);

            var title = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = $"Version {update.Version} is available" + (update.IsMandatory ? " (required)" : string.Empty)
            };
            var size = update.DownloadSize > 0 ? $" · {update.DownloadSize / 1024d / 1024d:0.0} MB" : string.Empty;
            var meta = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.DimGray,
                Text = $"{update.ReleaseType}{size}" + (update.HasVerifiedPackage ? string.Empty : " · no installable package")
            };
            var notes = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Text = PlainText(string.IsNullOrWhiteSpace(update.ReleaseNotes) ? update.Changelog : update.ReleaseNotes)
            };

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            buttons.Controls.Add(_btnUpdate);
            if (!update.IsMandatory)
            {
                buttons.Controls.Add(_btnLater);
                buttons.Controls.Add(_btnSkip);
            }

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), RowCount = 6, ColumnCount = 1 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.Controls.Add(title, 0, 0);
            layout.Controls.Add(meta, 0, 1);
            layout.Controls.Add(notes, 0, 2);
            layout.Controls.Add(_progress, 0, 3);
            layout.Controls.Add(_status, 0, 4);
            layout.Controls.Add(buttons, 0, 5);
            Controls.Add(layout);

            AcceptButton = _btnUpdate;
            if (!update.IsMandatory)
                CancelButton = _btnLater;

            _btnUpdate.Enabled = update.HasVerifiedPackage;
            _btnUpdate.Click += async (_, _) => await UpdateAndRestartAsync();
            _btnSkip.Click += (_, _) =>
            {
                SdkUpdateManager.SkipVersion(update.Version);
                Close();
            };
        }

        private async Task UpdateAndRestartAsync()
        {
            _btnUpdate.Enabled = _btnLater.Enabled = _btnSkip.Enabled = false;
            _progress.Visible = true;
            _status.Text = "Downloading…";

            var progress = new Progress<DownloadProgress>(p =>
            {
                _progress.Value = (int)Math.Clamp(p.Percentage, 0, 100);
                _status.Text = $"Downloading… {p.BytesDownloaded / 1024d / 1024d:0.0} / {p.TotalBytes / 1024d / 1024d:0.0} MB";
            });

            var package = await SdkUpdateManager.DownloadUpdateAsync(_update, progress);
            if (package == null)
            {
                _status.ForeColor = Color.Firebrick;
                _status.Text = SdkUpdateManager.LastUpdateCheckMessage ?? "The update could not be downloaded.";
                _progress.Visible = false;
                _btnUpdate.Enabled = true;
                _btnLater.Enabled = _btnSkip.Enabled = true;
                return;
            }

            _status.Text = "Verified. Installing and restarting…";
            if (!SdkUpdateManager.ApplyUpdateAndRestart(() => BeginInvoke(Application.Exit)))
            {
                _status.ForeColor = Color.Firebrick;
                _status.Text = SdkUpdateManager.LastUpdateCheckMessage ?? "The update could not be installed.";
                _btnUpdate.Enabled = true;
            }
        }

        private static string PlainText(string? html)
        {
            var text = Form1.PlainTextFromHtmlChangelog(html);
            return string.IsNullOrWhiteSpace(text) ? "No release notes." : Regex.Replace(text, @"\r?\n", Environment.NewLine);
        }
    }
}
