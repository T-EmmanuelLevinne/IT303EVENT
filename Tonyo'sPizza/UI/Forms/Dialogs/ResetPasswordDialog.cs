using System;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class ResetPasswordDialog : Form
    {
        private readonly User _targetUser;
        private TextBox _txtNewPassword;
        private TextBox _txtConfirmPassword;
        private CheckBox _chkUnlock;
        private RoundedButton _btnSave;
        private RoundedButton _btnCancel;

        public ResetPasswordDialog(User targetUser)
        {
            _targetUser = targetUser ?? throw new ArgumentNullException(nameof(targetUser));
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = $"Reset Password - @{_targetUser.Username}";
            Size = new Size(420, 340);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = AppColors.Background;

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 58,
                BackColor = Color.White,
                Padding = new Padding(22, 10, 22, 10)
            };
            header.Paint += (s, e) =>
            {
                using (var pen = new Pen(AppColors.Border, 1))
                    e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
            };

            Label lblTitle = new Label
            {
                Text = $"Reset Password for {_targetUser.FullName}",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(22, 16),
                AutoSize = true
            };
            header.Controls.Add(lblTitle);

            Panel content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(22, 16, 22, 16)
            };

            int y = 14;
            Label lblSub = new Label
            {
                Text = $"Username: @{_targetUser.Username}  ({ _targetUser.Role })",
                Location = new Point(22, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextMuted
            };
            y += 28;

            Label lblNew = new Label
            {
                Text = "New Password * (minimum 4 characters)",
                Location = new Point(22, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.75f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium
            };
            y += 20;

            _txtNewPassword = new TextBox
            {
                Location = new Point(22, y),
                Width = 356,
                Font = new Font("Segoe UI", 10f),
                UseSystemPasswordChar = true
            };
            y += 34;

            Label lblConf = new Label
            {
                Text = "Confirm New Password *",
                Location = new Point(22, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.75f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium
            };
            y += 20;

            _txtConfirmPassword = new TextBox
            {
                Location = new Point(22, y),
                Width = 356,
                Font = new Font("Segoe UI", 10f),
                UseSystemPasswordChar = true
            };
            y += 36;

            _chkUnlock = new CheckBox
            {
                Text = "Unlock account & reset failed login attempts",
                Location = new Point(22, y),
                AutoSize = true,
                Checked = _targetUser.IsLocked || _targetUser.FailedLoginAttempts > 0,
                Font = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextDark
            };

            content.Controls.Add(lblSub);
            content.Controls.Add(lblNew);
            content.Controls.Add(_txtNewPassword);
            content.Controls.Add(lblConf);
            content.Controls.Add(_txtConfirmPassword);
            content.Controls.Add(_chkUnlock);

            Panel footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = AppColors.Background,
                Padding = new Padding(22, 10, 22, 10)
            };
            footer.Paint += (s, e) =>
            {
                using (var pen = new Pen(AppColors.Border, 1))
                    e.Graphics.DrawLine(pen, 0, 0, footer.Width, 0);
            };

            _btnSave = new RoundedButton
            {
                Text = "Reset Password",
                IconName = "key",
                Width = 152,
                Height = 38,
                Dock = DockStyle.Right,
                BorderRadius = 8,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight,
                PressedColor = AppColors.PrimaryDark
            };
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Width = 96,
                Height = 38,
                Dock = DockStyle.Right,
                BorderRadius = 8,
                NormalColor = Color.White,
                HoverColor = AppColors.SurfaceAlt,
                CustomBorderColor = AppColors.Border,
                CustomBorderWidth = 1,
                ForeColor = AppColors.TextDark
            };
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            footer.Controls.Add(_btnSave);
            footer.Controls.Add(new Panel { Dock = DockStyle.Right, Width = 10, BackColor = Color.Transparent });
            footer.Controls.Add(_btnCancel);

            Controls.Add(content);
            Controls.Add(footer);
            Controls.Add(header);

            AcceptButton = _btnSave;
            CancelButton = _btnCancel;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string p1 = _txtNewPassword.Text;
            string p2 = _txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(p1) || p1.Length < 4)
            {
                MessageBox.Show(this, "Password must be at least 4 characters long.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtNewPassword.Focus();
                return;
            }
            if (p1 != p2)
            {
                MessageBox.Show(this, "Passwords do not match.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtConfirmPassword.Focus();
                return;
            }

            if (MessageBox.Show(this, $"Confirm resetting the password for '@{_targetUser.Username}'?", "Confirm Password Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            if (UserService.Instance.ResetUserPassword(_targetUser.UserId, p1, _chkUnlock.Checked, out string err))
            {
                MessageBox.Show(this, $"Password has been reset cleanly with a new cryptographic salt for '@{_targetUser.Username}'.", "Password Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(this, err, "Reset Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
