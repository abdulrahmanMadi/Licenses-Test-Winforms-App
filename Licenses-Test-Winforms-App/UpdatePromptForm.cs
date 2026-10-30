using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LicenPro.SDK.Updates;

namespace Licenses_Test_Winforms_App
{
    /// <summary>
    /// "Update available" window: release notes, then Update &amp; restart (download with progress, verify,
    /// install, restart), Later, or Skip this version. A mandatory update offers Update &amp; restart only and the
    /// window cannot be dismissed; the SDK already downloads it in the background, so quitting the app installs it at
    /// the next start (owner decision 2026-10-30).
    /// </summary>
    internal sealed class UpdatePromptForm : Form
    {
        private readonly UpdateInfo _update;
        private readonly Button _btnUpdate = new() { Text = "Update && restart", AutoSize = true };
        private readonly Button _btnLater = new() { Text = "Later", AutoSize = true };
        private readonly Button _btnSkip = new() { Text = "Skip this version", AutoSize = true };
        private readonly ProgressBar _progress = new() { Dock = DockStyle.Fill, Visible = false };
        private readonly Label _status = new() { Dock = DockStyle.Fill, AutoSize = false, ForeColor = Color.DimGray };
        private bool _closeAllowed;

        public UpdatePromptForm(UpdateInfo update)
        {
            _update = update;

            Text = $"{(string.IsNullOrWhiteSpace(update.ProductName) ? "Product" : update.ProductName)} update";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ControlBox = !update.IsMandatory;
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
                TabStop = false,
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
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
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

            // Without the product public key the package can only be rejected after the download (BUG-44).
            _btnUpdate.Enabled = update.CanInstall;
            if (update.HasVerifiedPackage && !SdkUpdateManager.CanVerifyPackages)
                _status.Text = "This app has no product public key configured, so the update cannot be verified. Ask the vendor for a build that includes it.";

            _btnUpdate.Click += async (_, _) => await UpdateAndRestartAsync();
            _btnLater.Click += (_, _) => Close();
            _btnSkip.Click += (_, _) =>
            {
                SdkUpdateManager.SkipVersion(update.Version);
                Close();
            };
            FormClosing += (_, e) =>
            {
                // A mandatory update cannot be dismissed (Alt+F4). Something outside ending the window (Task Manager)
                // closes it, and the caller then quits the app: the downloaded update installs at the next start.
                if (_update.IsMandatory && !_closeAllowed && e.CloseReason is CloseReason.UserClosing or CloseReason.None)
                    e.Cancel = true;
            };
        }

        /// <summary>True while downloading or installing.</summary>
        public bool IsBusy { get; private set; }

        /// <summary>The window closed because the update is being installed (the app exits for that).</summary>
        public bool Installing { get; private set; }

        /// <summary>The window closed to make room for a newer offer.</summary>
        public bool Replaced { get; private set; }

        /// <summary>The window already shows this offer (same version, same mandatory flag).</summary>
        public bool ShowsSameOffer(UpdateInfo update) =>
            update.Version == _update.Version && update.IsMandatory == _update.IsMandatory;

        /// <summary>Closes this window so a newer offer can replace it.</summary>
        public void CloseForNewerOffer()
        {
            _closeAllowed = true;
            Replaced = true;
            Close();
        }

        private async Task UpdateAndRestartAsync()
        {
            IsBusy = true;
            _btnUpdate.Enabled = _btnLater.Enabled = _btnSkip.Enabled = false;
            _progress.Style = ProgressBarStyle.Marquee; // a mandatory update may already be downloading in the background
            _progress.Visible = true;
            _status.ForeColor = Color.DimGray;
            _status.Text = "Downloading…";

            var progress = new Progress<DownloadProgress>(p =>
            {
                _progress.Style = ProgressBarStyle.Continuous;
                _progress.Value = (int)Math.Clamp(p.Percentage, 0, 100);
                _status.Text = $"Downloading… {p.BytesDownloaded / 1024d / 1024d:0.0} / {p.TotalBytes / 1024d / 1024d:0.0} MB";
            });

            var package = await SdkUpdateManager.DownloadUpdateAsync(_update, progress);
            if (package == null)
            {
                _status.ForeColor = Color.Firebrick;
                _status.Text = SdkUpdateManager.LastUpdateCheckMessage ?? "The update could not be downloaded.";
                _progress.Visible = false;
                _btnUpdate.Enabled = _update.CanInstall;
                _btnLater.Enabled = _btnSkip.Enabled = true;
                IsBusy = false;
                return;
            }

            _status.Text = "Verified. Installing and restarting…";
            _closeAllowed = true;
            Installing = true;
            if (!SdkUpdateManager.ApplyUpdateAndRestart(() => BeginInvoke(Application.Exit)))
            {
                _closeAllowed = false;
                Installing = false;
                _status.ForeColor = Color.Firebrick;
                _status.Text = SdkUpdateManager.LastUpdateCheckMessage ?? "The update could not be installed.";
                _btnUpdate.Enabled = true;
                IsBusy = false;
            }
        }

        private static string PlainText(string? html)
        {
            var text = Form1.PlainTextFromHtmlChangelog(html);
            return string.IsNullOrWhiteSpace(text) ? "No release notes." : Regex.Replace(text, @"\r?\n", Environment.NewLine);
        }
    }
}
