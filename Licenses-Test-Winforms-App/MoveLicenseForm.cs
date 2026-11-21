using System.Drawing;
using System.Windows.Forms;
using LicenPro.SDK;

namespace Licenses_Test_Winforms_App
{
    /// <summary>
    /// "Move license to another device" window for a Node-Locked license (SDK 1.16, owner decision 2026-11-21).
    /// On the device the license is bound to, the customer gets a transfer code. On the new device they enter it: the move
    /// then waits for the product owner, who approves it on the license page; pressing the button again after that moves
    /// the license here. Same layout as the dashboard's license windows: header, fields, an "Important" box, buttons.
    /// </summary>
    internal sealed class MoveLicenseForm : Form
    {
        private static readonly Color Primary = Color.FromArgb(59, 130, 246);
        private static readonly Color Warning = Color.FromArgb(245, 158, 11);
        private static readonly Color Muted = Color.FromArgb(107, 114, 128);
        private static readonly Color Border = Color.FromArgb(229, 231, 235);
        private static readonly Color Success = Color.FromArgb(22, 163, 74);
        private static readonly Color Danger = Color.FromArgb(220, 38, 38);

        private readonly string? _licenseKey;
        private readonly TextBox _reason = new() { Multiline = true, Height = 46, Dock = DockStyle.Top, PlaceholderText = "e.g. New laptop" };
        private readonly Button _btnRequest = PrimaryButton("Get transfer code", Primary);
        private readonly TextBox _code = new()
        {
            ReadOnly = true, Dock = DockStyle.Fill, Font = new Font("Consolas", 13F, FontStyle.Bold), TextAlign = HorizontalAlignment.Center,
            BorderStyle = BorderStyle.None, BackColor = Color.FromArgb(243, 244, 246)
        };
        private readonly Button _btnCopy = OutlineButton("Copy");
        private readonly Label _codeHint = new() { AutoSize = true, ForeColor = Muted, Margin = new Padding(0, 4, 0, 0) };
        private readonly Panel _codePanel = new() { Dock = DockStyle.Top, Height = 64, Visible = false };
        private readonly TextBox _enterCode = new()
        {
            Dock = DockStyle.Top, Font = new Font("Consolas", 12F), CharacterCasing = CharacterCasing.Upper, MaxLength = 14,
            PlaceholderText = "XXXX-XXXX-XXXX"
        };
        private readonly Button _btnComplete = PrimaryButton("Move license here", Warning);
        private readonly Label _status = new() { Dock = DockStyle.Top, AutoSize = false, Height = 40, Padding = new Padding(0, 6, 0, 0) };

        /// <summary>The license key of the license that moved to this device, once the owner approved.</summary>
        public string? MovedLicenseKey { get; private set; }

        /// <param name="licenseKey">The license loaded on this device, or null when none is (a new device).</param>
        public MoveLicenseForm(string? licenseKey)
        {
            _licenseKey = string.IsNullOrWhiteSpace(licenseKey) ? null : licenseKey.Trim();

            Text = "Move license";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ClientSize = new Size(560, 640);
            Font = new Font("Segoe UI", 9F);
            BackColor = Color.White;

            var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 16, 24, 8), AutoScroll = true };
            // Docked to the top, so added bottom-up.
            body.Controls.Add(_status);
            body.Controls.Add(ImportantBox());
            body.Controls.Add(Spacer(14));
            body.Controls.Add(ButtonRow(_btnComplete));
            body.Controls.Add(Spacer(6));
            body.Controls.Add(_enterCode);
            body.Controls.Add(FieldLabel("Transfer code from the old device"));
            body.Controls.Add(SectionTitle("2. On the new device", "Enter the code. The move waits for the product owner's approval; press the button again after they approve."));
            body.Controls.Add(Spacer(12));
            body.Controls.Add(Divider());
            body.Controls.Add(Spacer(10));
            body.Controls.Add(_codePanel);
            body.Controls.Add(ButtonRow(_btnRequest));
            body.Controls.Add(Spacer(6));
            body.Controls.Add(_reason);
            body.Controls.Add(FieldLabel("Reason (optional)"));
            body.Controls.Add(SectionTitle("1. On the device the license is on now", _licenseKey == null
                ? "Validate the license on this device first, then get a code here."
                : "Get a code, then enter it on the new device. The code works for 24 hours."));

            var code = new TableLayoutPanel { Dock = DockStyle.Top, Height = 38, ColumnCount = 2 };
            code.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            code.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            code.Controls.Add(_code, 0, 0);
            code.Controls.Add(_btnCopy, 1, 0);
            _btnCopy.Dock = DockStyle.Fill;
            _codeHint.Dock = DockStyle.Top;
            _codePanel.Controls.Add(_codeHint);
            _codePanel.Controls.Add(code);

            var footer = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom, Height = 56, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(20, 12, 20, 12),
                BackColor = Color.FromArgb(249, 250, 251)
            };
            var close = OutlineButton("Close");
            close.Width = 100;
            close.Click += (_, _) => Close();
            footer.Controls.Add(close);

            Controls.Add(body);
            Controls.Add(Divider(DockStyle.Bottom));
            Controls.Add(footer);
            Controls.Add(Divider());
            Controls.Add(Header());

            CancelButton = close;
            ClientSize = new Size(560, 680);
            _btnRequest.Enabled = _licenseKey != null;
            _reason.Enabled = _licenseKey != null;
            _btnRequest.Click += async (_, _) => await RequestAsync();
            _btnComplete.Click += async (_, _) => await CompleteAsync();
            _btnCopy.Click += (_, _) =>
            {
                if (!string.IsNullOrEmpty(_code.Text))
                    Clipboard.SetText(_code.Text);
            };
        }

        private async Task RequestAsync()
        {
            SetBusy(true, "Getting a transfer code...");
            try
            {
                var result = await NodeLockedDeviceManager.RequestTransferAsync(_licenseKey!, _reason.Text.Trim());
                if (!result.IsSuccess)
                {
                    ShowStatus(result.ErrorMessage ?? "The transfer code could not be created.", Danger);
                    return;
                }

                _code.Text = result.TransferCode;
                _codeHint.Text = $"Enter this code on the new device before {result.ExpiresAt.ToLocalTime():g} (local time).";
                _codePanel.Visible = true;
                ShowStatus("Code ready. This device keeps the license until the product owner approves the move.", Success);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async Task CompleteAsync()
        {
            var code = _enterCode.Text.Trim();
            if (code.Length == 0)
            {
                ShowStatus("Enter the transfer code from the old device.", Danger);
                return;
            }

            SetBusy(true, "Sending the code...");
            try
            {
                var result = await NodeLockedDeviceManager.CompleteTransferAsync(code);
                if (result.IsSuccess)
                {
                    MovedLicenseKey = result.LicenseKey;
                    ShowStatus("The license moved to this device. Close this window to validate it here.", Success);
                    _btnComplete.Enabled = false;
                    return;
                }

                if (result.AwaitingApproval)
                {
                    ShowStatus(result.ErrorMessage ?? "Waiting for the product owner's approval.", Warning);
                    _btnComplete.Text = "Check again";
                    return;
                }

                ShowStatus(result.ErrorMessage ?? "The license could not be moved.", Danger);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void SetBusy(bool busy, string? message = null)
        {
            UseWaitCursor = busy;
            _btnRequest.Enabled = !busy && _licenseKey != null;
            _btnComplete.Enabled = !busy && MovedLicenseKey == null;
            if (busy && message != null)
                ShowStatus(message, Muted);
        }

        private void ShowStatus(string message, Color color)
        {
            _status.ForeColor = color;
            _status.Text = message;
        }

        // ---- layout pieces (dashboard modal look) -----------------------------------------------------------

        private static Control Header()
        {
            var header = new Panel { Dock = DockStyle.Top, Height = 72, Padding = new Padding(24, 14, 24, 10) };
            var icon = new Label
            {
                Text = "⇄", Font = new Font("Segoe UI Symbol", 16F, FontStyle.Bold), ForeColor = Primary, BackColor = Color.FromArgb(239, 246, 255),
                Size = new Size(44, 44), Location = new Point(24, 14), TextAlign = ContentAlignment.MiddleCenter
            };
            var title = new Label
            {
                Text = "Move license to another device", Font = new Font("Segoe UI", 12F, FontStyle.Bold), AutoSize = true,
                Location = new Point(80, 14)
            };
            var subtitle = new Label
            {
                Text = "Move this Node-Locked license to a new computer", ForeColor = Muted, AutoSize = true, Location = new Point(81, 40)
            };
            header.Controls.Add(icon);
            header.Controls.Add(title);
            header.Controls.Add(subtitle);
            return header;
        }

        private static Control SectionTitle(string title, string description)
        {
            var panel = new Panel { Dock = DockStyle.Top, Height = 58 };
            panel.Controls.Add(new Label { Text = description, ForeColor = Muted, Dock = DockStyle.Top, Height = 34, AutoSize = false });
            panel.Controls.Add(new Label { Text = title, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Dock = DockStyle.Top, Height = 22 });
            return panel;
        }

        private static Control FieldLabel(string text) =>
            new Label { Text = text, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Dock = DockStyle.Top, Height = 22, Padding = new Padding(0, 4, 0, 0) };

        private static Control ButtonRow(Button button)
        {
            var row = new Panel { Dock = DockStyle.Top, Height = 36 };
            button.Dock = DockStyle.Right;
            row.Controls.Add(button);
            return row;
        }

        private static Control ImportantBox()
        {
            var box = new Panel
            {
                Dock = DockStyle.Top, Height = 92, Padding = new Padding(14, 10, 14, 10), BackColor = Color.FromArgb(255, 251, 235),
                BorderStyle = BorderStyle.FixedSingle
            };
            box.Controls.Add(new Label
            {
                Dock = DockStyle.Fill, ForeColor = Color.FromArgb(180, 83, 9),
                Text = "•  The product owner approves every move; until then the license stays on the old device.\n" +
                       "•  After the move, the old device can no longer use this license.\n" +
                       "•  Moves are limited (a few per year, with a waiting time between them)."
            });
            box.Controls.Add(new Label
            {
                Dock = DockStyle.Top, Height = 20, Text = "Important", Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(146, 64, 14)
            });
            return box;
        }

        private static Control Divider(DockStyle dock = DockStyle.Top) =>
            new Panel { Dock = dock, Height = 1, BackColor = Border };

        private static Control Spacer(int height) => new Panel { Dock = DockStyle.Top, Height = height };

        private static Button PrimaryButton(string text, Color color)
        {
            var button = new Button
            {
                Text = text, Width = 170, BackColor = color, ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            // A flat button keeps its colour when disabled; grey it so it does not look clickable.
            button.EnabledChanged += (_, _) =>
            {
                button.BackColor = button.Enabled ? color : Color.FromArgb(209, 213, 219);
                button.ForeColor = button.Enabled ? Color.White : Color.FromArgb(107, 114, 128);
            };
            return button;
        }

        private static Button OutlineButton(string text)
        {
            var button = new Button { Text = text, Height = 32, FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };
            button.FlatAppearance.BorderColor = Border;
            return button;
        }
    }
}
