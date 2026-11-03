using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class ConfirmRoleChangeDialog : Form
    {
        private readonly User _targetUser;
        private readonly string _newRole;
        private ModernInputField _txtAdminPassword;
        private Label _lblError;
        private RoundedButton _btnConfirm;
        private RoundedButton _btnCancel;

        public ConfirmRoleChangeDialog(User targetUser, string newRole)
        {
            _targetUser = targetUser ?? throw new ArgumentNullException(nameof(targetUser));
            _newRole = newRole ?? string.Empty;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Security Verification - Role Change";
            Size = new Size(490, 520);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            BackColor = AppColors.Background;

            // Header
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 62,
                BackColor = AppColors.PrimaryDark
            };

            Label lblTitle = new Label
            {
                Text = "Privileged Role Authorization",
                Font = new Font("Segoe UI Bold", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 10),
                AutoSize = true
            };

            Label lblSubtitle = new Label
            {
                Text = "Admin verification is required to confirm this role assignment.",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(254, 205, 211),
                Location = new Point(20, 34),
                AutoSize = true
            };

            header.Controls.Add(lblTitle);
            header.Controls.Add(lblSubtitle);
            Controls.Add(header);

            int y = 74;

            // Warning Box
            Panel pnlWarning = new Panel
            {
                Location = new Point(20, y),
                Size = new Size(434, 66),
                BackColor = AppColors.StatusPendingBg
            };
            pnlWarning.Paint += (s, e) =>
            {
                using (var pen = new Pen(AppColors.StatusPending, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlWarning.Width - 1, pnlWarning.Height - 1);
                }
            };

            Label lblWarnIcon = new Label
            {
                Text = "⚠️",
                Font = new Font("Segoe UI Emoji", 14f),
                Location = new Point(10, 12),
                Size = new Size(30, 30),
                BackColor = Color.Transparent
            };

            Label lblWarnText = new Label
            {
                Text = "Security Notice: Elevating a user to Kitchen Crew or Administrator grants operational and database access. Please verify this is the intended user.",
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(120, 53, 15),
                Location = new Point(44, 8),
                Size = new Size(378, 50),
                BackColor = Color.Transparent
            };

            pnlWarning.Controls.Add(lblWarnIcon);
            pnlWarning.Controls.Add(lblWarnText);
            Controls.Add(pnlWarning);
            y += 76;

            // Target User Card
            Panel pnlUserCard = new Panel
            {
                Location = new Point(20, y),
                Size = new Size(434, 118),
                BackColor = Color.White
            };
            pnlUserCard.Paint += (s, e) =>
            {
                using (var pen = new Pen(AppColors.Border, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlUserCard.Width - 1, pnlUserCard.Height - 1);
                }
            };

            Label lblCardHeader = new Label
            {
                Text = "TARGET USER DETAILS",
                Font = new Font("Segoe UI Bold", 7.5f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium,
                Location = new Point(14, 10),
                AutoSize = true
            };

            Label lblUserFull = new Label
            {
                Text = $"{_targetUser.FullName}",
                Font = new Font("Segoe UI Bold", 11.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(14, 28),
                AutoSize = true
            };

            Label lblUserInfo = new Label
            {
                Text = $"Username: @{_targetUser.Username}  •  ID: #{_targetUser.UserId}  •  Phone: {(!string.IsNullOrEmpty(_targetUser.Phone) ? _targetUser.Phone : "N/A")}",
                Font = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(14, 52),
                AutoSize = true
            };

            Label lblTransition = new Label
            {
                Text = $"Role Transition:  {_targetUser.Role}  ➔  ",
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(14, 82),
                AutoSize = true
            };

            Label lblNewRoleBadge = new Label
            {
                Text = $" {_newRole} ",
                Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = _newRole == "Admin" ? AppColors.PrimaryDark : AppColors.StatusPreparing,
                Location = new Point(14 + TextRenderer.MeasureText(lblTransition.Text, lblTransition.Font).Width, 80),
                Padding = new Padding(4, 2, 4, 2),
                AutoSize = true
            };

            pnlUserCard.Controls.Add(lblCardHeader);
            pnlUserCard.Controls.Add(lblUserFull);
            pnlUserCard.Controls.Add(lblUserInfo);
            pnlUserCard.Controls.Add(lblTransition);
            pnlUserCard.Controls.Add(lblNewRoleBadge);
            Controls.Add(pnlUserCard);
            y += 128;

            // Admin Authorization Section
            var currentAdmin = AuthService.Instance.CurrentUser;
            Label lblAdminPrompt = new Label
            {
                Text = "ENTER ADMIN PASSWORD TO CONFIRM",
                Font = new Font("Segoe UI Bold", 7.5f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium,
                Location = new Point(20, y),
                AutoSize = true
            };
            y += 16;

            Label lblAdminLogged = new Label
            {
                Text = $"Authorizing Administrator: {currentAdmin?.FullName ?? "Admin"} (@{currentAdmin?.Username ?? "admin"})",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(20, y),
                AutoSize = true
            };
            y += 20;

            _txtAdminPassword = new ModernInputField
            {
                Location = new Point(20, y),
                Size = new Size(434, 40),
                IsPassword = true,
                IconName = "lock"
            };
            _txtAdminPassword.InnerTextBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    BtnConfirm_Click(this, EventArgs.Empty);
                }
            };
            Controls.Add(lblAdminPrompt);
            Controls.Add(lblAdminLogged);
            Controls.Add(_txtAdminPassword);
            y += 44;

            _lblError = new Label
            {
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = AppColors.StatusCancelled,
                Location = new Point(20, y),
                Size = new Size(434, 18),
                Visible = false
            };
            Controls.Add(_lblError);
            y += 24;

            // Footer Buttons
            _btnConfirm = new RoundedButton
            {
                Text = "Confirm & Apply Role",
                IconName = "check",
                Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold),
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight,
                PressedColor = AppColors.PrimaryDark,
                ForeColor = Color.White,
                BorderRadius = 8,
                Location = new Point(244, y),
                Size = new Size(210, 40),
                Cursor = Cursors.Hand
            };
            _btnConfirm.Click += BtnConfirm_Click;

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                NormalColor = Color.White,
                HoverColor = AppColors.BorderLight,
                PressedColor = AppColors.Border,
                ForeColor = AppColors.TextDark,
                CustomBorderColor = AppColors.Border,
                CustomBorderWidth = 1,
                BorderRadius = 8,
                Location = new Point(130, y),
                Size = new Size(104, 40),
                Cursor = Cursors.Hand
            };
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(_btnCancel);
            Controls.Add(_btnConfirm);

            this.CancelButton = _btnCancel;
            this.Shown += (s, e) => _txtAdminPassword.InnerTextBox.Focus();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            string password = _txtAdminPassword.Text;
            if (string.IsNullOrWhiteSpace(password))
            {
                _lblError.Text = "❌ Please enter your administrator password.";
                _lblError.Visible = true;
                _txtAdminPassword.InnerTextBox.Focus();
                return;
            }

            if (!AuthService.Instance.VerifyCurrentAdminPassword(password))
            {
                _lblError.Text = "❌ Incorrect administrator password. Verification failed.";
                _lblError.Visible = true;
                _txtAdminPassword.Text = string.Empty;
                _txtAdminPassword.InnerTextBox.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
