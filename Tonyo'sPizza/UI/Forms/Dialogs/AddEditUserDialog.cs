using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class AddEditUserDialog : Form
    {
        private readonly User _existingUser;
        private readonly bool _isEdit;

        private TextBox _txtUsername;
        private TextBox _txtFullName;
        private TextBox _txtPassword;
        private TextBox _txtConfirmPassword;
        private ComboBox _cboRole;
        private TextBox _txtPhone;
        private TextBox _txtAddress;
        private CheckBox _chkIsActive;
        private RoundedButton _btnSave;
        private RoundedButton _btnCancel;

        public AddEditUserDialog(User user = null)
        {
            _existingUser = user;
            _isEdit = user != null;
            InitializeComponent();
            PopulateData();
        }

        private void InitializeComponent()
        {
            Text = _isEdit ? $"Edit Account - {_existingUser.Username}" : "Create New User Account";
            Size = new Size(460, _isEdit ? 490 : 590);
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
                Text = _isEdit ? "Update User Account" : "New Staff or Customer Account",
                Font = new Font("Segoe UI", 12.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(22, 15),
                AutoSize = true
            };
            header.Controls.Add(lblTitle);

            Panel content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(22, 16, 22, 16),
                AutoScroll = true
            };

            int y = 14;
            int fieldW = 396;

            Label lblUser = CreateLabel("Username *", 22, y);
            y += 20;
            _txtUsername = new TextBox
            {
                Location = new Point(22, y),
                Width = fieldW,
                Font = new Font("Segoe UI", 10f),
                ReadOnly = _isEdit,
                BackColor = _isEdit ? AppColors.SurfaceAlt : Color.White
            };
            y += 34;

            Label lblFull = CreateLabel("Full Name *", 22, y);
            y += 20;
            _txtFullName = new TextBox { Location = new Point(22, y), Width = fieldW, Font = new Font("Segoe UI", 10f) };
            y += 34;

            if (!_isEdit)
            {
                Label lblPass = CreateLabel("Password * (min 4 characters)", 22, y);
                y += 20;
                _txtPassword = new TextBox { Location = new Point(22, y), Width = 192, Font = new Font("Segoe UI", 10f), UseSystemPasswordChar = true };

                Label lblConf = CreateLabel("Confirm Password *", 226, y - 20);
                _txtConfirmPassword = new TextBox { Location = new Point(226, y), Width = 192, Font = new Font("Segoe UI", 10f), UseSystemPasswordChar = true };
                content.Controls.Add(lblPass);
                content.Controls.Add(_txtPassword);
                content.Controls.Add(lblConf);
                content.Controls.Add(_txtConfirmPassword);
                y += 36;
            }

            Label lblRole = CreateLabel("Assigned Role *", 22, y);
            Label lblPhone = CreateLabel("Phone Number (09XXXXXXXXX)", 226, y);
            y += 20;

            _cboRole = new ComboBox
            {
                Location = new Point(22, y),
                Width = 192,
                Font = new Font("Segoe UI", 10f),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cboRole.Items.AddRange(new object[] { "Customer", "Crew", "Admin" });
            _cboRole.SelectedIndex = 0;

            _txtPhone = new TextBox
            {
                Location = new Point(226, y),
                Width = 192,
                Font = new Font("Segoe UI", 10f),
                MaxLength = 11
            };
            _txtPhone.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
            y += 36;

            Label lblAddr = CreateLabel("Address (Optional)", 22, y);
            y += 20;
            _txtAddress = new TextBox { Location = new Point(22, y), Width = fieldW, Height = 54, Multiline = true, Font = new Font("Segoe UI", 9.5f) };
            y += 66;

            _chkIsActive = new CheckBox
            {
                Text = "Account is active and permitted to sign in",
                Location = new Point(22, y),
                AutoSize = true,
                Checked = true,
                Font = new Font("Segoe UI Semibold", 9.25f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Cursor = Cursors.Hand
            };

            content.Controls.Add(lblUser);
            content.Controls.Add(_txtUsername);
            content.Controls.Add(lblFull);
            content.Controls.Add(_txtFullName);
            content.Controls.Add(lblRole);
            content.Controls.Add(_cboRole);
            content.Controls.Add(lblPhone);
            content.Controls.Add(_txtPhone);
            content.Controls.Add(lblAddr);
            content.Controls.Add(_txtAddress);
            content.Controls.Add(_chkIsActive);

            Panel footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 62,
                BackColor = AppColors.Background,
                Padding = new Padding(22, 11, 22, 11)
            };
            footer.Paint += (s, e) =>
            {
                using (var pen = new Pen(AppColors.Border, 1))
                    e.Graphics.DrawLine(pen, 0, 0, footer.Width, 0);
            };

            _btnSave = new RoundedButton
            {
                Text = _isEdit ? "Save Changes" : "Create Account",
                IconName = "check",
                Width = 160,
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
                Width = 104,
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

        private static Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.75f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium
            };
        }

        private void PopulateData()
        {
            if (_isEdit)
            {
                _txtUsername.Text = _existingUser.Username;
                _txtFullName.Text = _existingUser.FullName;
                _txtPhone.Text = _existingUser.Phone;
                _txtAddress.Text = _existingUser.Address;
                _chkIsActive.Checked = _existingUser.IsActive;

                int roleIdx = _cboRole.FindStringExact(_existingUser.Role);
                if (roleIdx >= 0) _cboRole.SelectedIndex = roleIdx;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string username = _txtUsername.Text.Trim();
            string fullName = _txtFullName.Text.Trim();
            string role = _cboRole.SelectedItem?.ToString() ?? "Customer";
            string phone = _txtPhone.Text.Trim();
            string address = _txtAddress.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show(this, "Username and Full Name are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(phone) && (!phone.StartsWith("09") || phone.Length != 11 || !phone.All(char.IsDigit)))
            {
                MessageBox.Show(this, "Phone number must start with '09' and be exactly 11 digits.", "Invalid Phone Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtPhone.Focus();
                return;
            }

            if (!_isEdit)
            {
                string pass = _txtPassword.Text;
                string conf = _txtConfirmPassword.Text;

                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 4)
                {
                    MessageBox.Show(this, "Password must be at least 4 characters long.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _txtPassword.Focus();
                    return;
                }
                if (pass != conf)
                {
                    MessageBox.Show(this, "Password and Confirm Password do not match.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _txtConfirmPassword.Focus();
                    return;
                }

                if (UserService.Instance.CreateUser(username, pass, fullName, role, phone, address, _chkIsActive.Checked, out string err))
                {
                    MessageBox.Show(this, $"Account '@{username}' created successfully.", "Account Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(this, err, "Could Not Create User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (AuthService.Instance.CurrentUser?.UserId == _existingUser.UserId && !_chkIsActive.Checked)
                {
                    MessageBox.Show(this, "You cannot deactivate your own active account.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show(this, $"Save updates for user '@{_existingUser.Username}'?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                var updated = _existingUser.Clone();
                updated.FullName = fullName;
                updated.Role = role;
                updated.Phone = phone;
                updated.Address = address;
                updated.IsActive = _chkIsActive.Checked;

                if (UserService.Instance.UpdateUserProfile(updated, out string err))
                {
                    MessageBox.Show(this, $"User '@{_existingUser.Username}' updated successfully.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(this, err, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
