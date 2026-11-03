using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Forms
{
    public class LoginForm : Form
    {
        private readonly AuthService _authService;

        // UI Containers
        private Panel _pnlHeader;
        private RoundedPanel _pnlCard;
        private Panel _pnlLoginContent;
        private Panel _pnlRegisterContent;

        // Login Controls
        private RoundedButton _btnTabSignIn;
        private RoundedButton _btnTabCreateAccount;
        private ModernInputField _txtLoginUsername;
        private ModernInputField _txtLoginPassword;
        private RoundedButton _btnLogin;

        // Register Controls
        private RoundedButton _btnBackToLogin;
        private ModernInputField _txtRegFullName;
        private ModernInputField _txtRegUsername;
        private ModernInputField _txtRegPassword;
        private ModernInputField _txtRegPhone;
        private RoundedButton _btnRegister;

        public string ActiveView { get; private set; } = "Login";

        public LoginForm()
        {
            _authService = AuthService.Instance;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Tonyo's Pizza - Authentication";
            ClientSize = new Size(460, 520);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(248, 250, 252);

            // 1. Top Brand Header Area
            _pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(460, 84),
                BackColor = Color.Transparent
            };
            _pnlHeader.Paint += PnlHeader_Paint;

            // 2. Center Card Container
            _pnlCard = new RoundedPanel
            {
                Location = new Point(28, 88),
                Size = new Size(404, 395),
                BorderRadius = 14,
                BackColor = Color.White,
                BorderColor = AppColors.Border,
                DrawBorder = true,
                BorderWidth = 1
            };

            BuildLoginContent();
            BuildRegisterContent();

            _pnlCard.Controls.Add(_pnlLoginContent);
            _pnlCard.Controls.Add(_pnlRegisterContent);

            Controls.Add(_pnlCard);
            Controls.Add(_pnlHeader);

            ShowLoginView();
        }

        private void PnlHeader_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            UIHelper.SetHighQuality(g);

            // Centered Brand Emblem + Title
            int brandW = 240;
            int startX = (_pnlHeader.Width - brandW) / 2;
            int startY = 16;

            // Circular Brand Badge
            using (SolidBrush b = new SolidBrush(AppColors.Primary))
            {
                g.FillEllipse(b, startX, startY, 36, 36);
            }
            UIHelper.DrawVectorIcon(g, "pizza", new Rectangle(startX + 8, startY + 8, 20, 20), Color.White);

            // Brand Name
            using (Font fn = new Font("Segoe UI Black", 16f, FontStyle.Bold))
            using (SolidBrush sb = new SolidBrush(AppColors.TextDark))
            {
                g.DrawString("TONYO'S PIZZA", fn, sb, startX + 44, startY + 5);
            }

            // Subtitle
            using (Font fnSub = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold))
            using (SolidBrush sbSub = new SolidBrush(AppColors.TextMuted))
            {
                string sub = "Artisan Wood-Fired Pizzeria • Store & Kitchen POS";
                SizeF subSize = g.MeasureString(sub, fnSub);
                g.DrawString(sub, fnSub, sbSub, (_pnlHeader.Width - subSize.Width) / 2, startY + 44);
            }
        }

        private void BuildLoginContent()
        {
            _pnlLoginContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            int y = 18;

            // Segmented Pill Tab Switcher
            RoundedPanel pnlTabs = new RoundedPanel
            {
                Location = new Point(24, y),
                Size = new Size(356, 38),
                BorderRadius = 8,
                BackColor = Color.FromArgb(241, 245, 249),
                BorderWidth = 0
            };

            _btnTabSignIn = new RoundedButton
            {
                Text = "Sign In",
                Location = new Point(3, 3),
                Size = new Size(173, 32),
                BorderRadius = 6,
                NormalColor = Color.White,
                ForeColor = AppColors.PrimaryDark,
                Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnTabSignIn.Click += (s, e) => ShowLoginView();

            _btnTabCreateAccount = new RoundedButton
            {
                Text = "Create Account",
                Location = new Point(180, 3),
                Size = new Size(173, 32),
                BorderRadius = 6,
                NormalColor = Color.Transparent,
                ForeColor = AppColors.TextMedium,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnTabCreateAccount.Click += (s, e) => ShowRegisterView();

            pnlTabs.Controls.Add(_btnTabSignIn);
            pnlTabs.Controls.Add(_btnTabCreateAccount);
            _pnlLoginContent.Controls.Add(pnlTabs);
            y += 50;

            // Welcome Text
            Label lblHeading = new Label
            {
                Text = "Welcome Back",
                Font = new Font("Segoe UI Bold", 15f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(24, y),
                AutoSize = true
            };
            y += 24;

            Label lblSub = new Label
            {
                Text = "Sign in to place orders, view kitchen queue, or manage store.",
                Font = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(24, y),
                AutoSize = true
            };
            y += 28;

            // Username Field
            Label lblUser = new Label
            {
                Text = "USERNAME",
                Font = new Font("Segoe UI Bold", 8f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium,
                Location = new Point(24, y),
                AutoSize = true
            };
            y += 18;

            _txtLoginUsername = new ModernInputField
            {
                Location = new Point(24, y),
                Size = new Size(356, 42),
                IconName = "users",
                Text = "admin"
            };
            y += 48;

            // Password Field
            Label lblPass = new Label
            {
                Text = "PASSWORD",
                Font = new Font("Segoe UI Bold", 8f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium,
                Location = new Point(24, y),
                AutoSize = true
            };
            y += 18;

            _txtLoginPassword = new ModernInputField
            {
                Location = new Point(24, y),
                Size = new Size(356, 42),
                IconName = "lock",
                IsPassword = true,
                Text = "admin"
            };
            y += 52;

            // Sign In Button
            _btnLogin = new RoundedButton
            {
                Text = "Sign In",
                IconName = "check",
                Location = new Point(24, y),
                Size = new Size(356, 44),
                BorderRadius = 8,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight,
                PressedColor = AppColors.PrimaryDark,
                Font = new Font("Segoe UI Bold", 10.5f, FontStyle.Bold)
            };
            _btnLogin.Click += BtnLogin_Click;
            y += 52;

            // Quick Demo Roles Helper Pill
            Panel pnlQuickRoles = new Panel
            {
                Location = new Point(24, y),
                Size = new Size(356, 28),
                BackColor = Color.Transparent
            };

            Label lblQuick = new Label
            {
                Text = "Quick demo:",
                Font = new Font("Segoe UI Semibold", 8f, FontStyle.Bold),
                ForeColor = AppColors.TextLight,
                Location = new Point(0, 6),
                AutoSize = true
            };

            RoundedButton btnDemoAdmin = CreateDemoChip("Admin", "admin", "admin", 80);
            RoundedButton btnDemoCrew = CreateDemoChip("Crew", "crew", "crew", 145);
            RoundedButton btnDemoCustomer = CreateDemoChip("Customer", "customer", "customer", 205);

            pnlQuickRoles.Controls.Add(lblQuick);
            pnlQuickRoles.Controls.Add(btnDemoAdmin);
            pnlQuickRoles.Controls.Add(btnDemoCrew);
            pnlQuickRoles.Controls.Add(btnDemoCustomer);

            _pnlLoginContent.Controls.Add(lblHeading);
            _pnlLoginContent.Controls.Add(lblSub);
            _pnlLoginContent.Controls.Add(lblUser);
            _pnlLoginContent.Controls.Add(_txtLoginUsername);
            _pnlLoginContent.Controls.Add(lblPass);
            _pnlLoginContent.Controls.Add(_txtLoginPassword);
            _pnlLoginContent.Controls.Add(_btnLogin);
            _pnlLoginContent.Controls.Add(pnlQuickRoles);
        }

        private RoundedButton CreateDemoChip(string text, string user, string pass, int x)
        {
            var btn = new RoundedButton
            {
                Text = text,
                Location = new Point(x, 2),
                Size = new Size(text.Length > 6 ? 68 : 58, 24),
                BorderRadius = 12,
                NormalColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI Semibold", 7.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.Click += (s, e) =>
            {
                _txtLoginUsername.Text = user;
                _txtLoginPassword.Text = pass;
            };
            return btn;
        }

        private void BuildRegisterContent()
        {
            _pnlRegisterContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            int y = 14;

            // Back Arrow Button to Return to Sign In (Task 3 Requirement)
            _btnBackToLogin = new RoundedButton
            {
                Text = "Back to Sign In",
                IconName = "back",
                Location = new Point(24, y),
                Size = new Size(140, 32),
                BorderRadius = 6,
                NormalColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnBackToLogin.Click += (s, e) => ShowLoginView();
            _pnlRegisterContent.Controls.Add(_btnBackToLogin);
            y += 40;

            // Heading
            Label lblHeading = new Label
            {
                Text = "Create Customer Account",
                Font = new Font("Segoe UI Bold", 13.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(24, y),
                AutoSize = true
            };
            y += 22;

            Label lblSub = new Label
            {
                Text = "Register to order delicious wood-fired pizza instantly.",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(24, y),
                AutoSize = true
            };
            y += 24;

            // Full Name
            Label lblFull = new Label
            {
                Text = "FULL NAME *",
                Font = new Font("Segoe UI Bold", 7.5f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium,
                Location = new Point(24, y),
                AutoSize = true
            };
            y += 16;

            _txtRegFullName = new ModernInputField
            {
                Location = new Point(24, y),
                Size = new Size(356, 38),
                IconName = "user"
            };
            y += 44;

            // Row for Username & Password
            Label lblU = new Label
            {
                Text = "USERNAME *",
                Font = new Font("Segoe UI Bold", 7.5f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium,
                Location = new Point(24, y),
                AutoSize = true
            };

            Label lblP = new Label
            {
                Text = "PASSWORD *",
                Font = new Font("Segoe UI Bold", 7.5f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium,
                Location = new Point(206, y),
                AutoSize = true
            };
            y += 16;

            _txtRegUsername = new ModernInputField
            {
                Location = new Point(24, y),
                Size = new Size(174, 38),
                IconName = "users"
            };

            _txtRegPassword = new ModernInputField
            {
                Location = new Point(206, y),
                Size = new Size(174, 38),
                IconName = "lock",
                IsPassword = true
            };
            y += 44;

            // Phone
            Label lblPhone = new Label
            {
                Text = "PHONE NUMBER (11 digits, e.g. 09123456789)",
                Font = new Font("Segoe UI Bold", 7.5f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium,
                Location = new Point(24, y),
                AutoSize = true
            };
            y += 16;

            _txtRegPhone = new ModernInputField
            {
                Location = new Point(24, y),
                Size = new Size(356, 38),
                IconName = "phone",
                MaxLength = 11
            };
            _txtRegPhone.InnerTextBox.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
            _txtRegPhone.InnerTextBox.TextChanged += (s, e) =>
            {
                string original = _txtRegPhone.InnerTextBox.Text;
                string digits = new string(original.Where(char.IsDigit).ToArray());
                if (digits.Length > 11)
                {
                    digits = digits.Substring(0, 11);
                }
                if (original != digits)
                {
                    int sel = _txtRegPhone.InnerTextBox.SelectionStart;
                    _txtRegPhone.InnerTextBox.Text = digits;
                    _txtRegPhone.InnerTextBox.SelectionStart = Math.Min(sel, digits.Length);
                }
            };
            y += 48;

            // Register Button
            _btnRegister = new RoundedButton
            {
                Text = "Complete Registration",
                IconName = "check",
                Location = new Point(24, y),
                Size = new Size(356, 42),
                BorderRadius = 8,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight,
                PressedColor = AppColors.PrimaryDark,
                Font = new Font("Segoe UI Bold", 10f, FontStyle.Bold)
            };
            _btnRegister.Click += BtnRegister_Click;

            _pnlRegisterContent.Controls.Add(lblHeading);
            _pnlRegisterContent.Controls.Add(lblSub);
            _pnlRegisterContent.Controls.Add(lblFull);
            _pnlRegisterContent.Controls.Add(_txtRegFullName);
            _pnlRegisterContent.Controls.Add(lblU);
            _pnlRegisterContent.Controls.Add(lblP);
            _pnlRegisterContent.Controls.Add(_txtRegUsername);
            _pnlRegisterContent.Controls.Add(_txtRegPassword);
            _pnlRegisterContent.Controls.Add(lblPhone);
            _pnlRegisterContent.Controls.Add(_txtRegPhone);
            _pnlRegisterContent.Controls.Add(_btnRegister);
        }

        private void ShowLoginView()
        {
            ActiveView = "Login";
            _pnlCard.Height = 395;
            ClientSize = new Size(460, 520);

            _pnlRegisterContent.Visible = false;
            _pnlLoginContent.Visible = true;
            _pnlLoginContent.BringToFront();

            _btnTabSignIn.NormalColor = Color.White;
            _btnTabSignIn.ForeColor = AppColors.PrimaryDark;
            _btnTabCreateAccount.NormalColor = Color.Transparent;
            _btnTabCreateAccount.ForeColor = AppColors.TextMedium;

            _pnlHeader.Invalidate();
        }

        private void ShowRegisterView()
        {
            ActiveView = "Register";
            _pnlCard.Height = 360;
            ClientSize = new Size(460, 485);

            _pnlLoginContent.Visible = false;
            _pnlRegisterContent.Visible = true;
            _pnlRegisterContent.BringToFront();

            _btnTabCreateAccount.NormalColor = Color.White;
            _btnTabCreateAccount.ForeColor = AppColors.PrimaryDark;
            _btnTabSignIn.NormalColor = Color.Transparent;
            _btnTabSignIn.ForeColor = AppColors.TextMedium;

            _pnlHeader.Invalidate();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = _txtLoginUsername.Text.Trim();
            string password = _txtLoginPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_authService.Login(username, password, out string err))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show($"Login Failed: {err}\n\nTip: Please check your credentials or select a quick demo account below.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string username = _txtRegUsername.Text.Trim();
            string password = _txtRegPassword.Text;
            string fullName = _txtRegFullName.Text.Trim();
            string role = "Customer";
            string phone = _txtRegPhone.Text.Trim();
            string address = string.Empty;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Full Name, Username, and Password are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Phone number is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtRegPhone.Focus();
                return;
            }

            if (!phone.StartsWith("09"))
            {
                MessageBox.Show("Phone number must start with '09' (e.g., 09123456789).", "Invalid Phone Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtRegPhone.Focus();
                return;
            }

            if (phone.Length != 11)
            {
                MessageBox.Show($"Phone number must be exactly 11 digits long (currently {phone.Length} digits).", "Invalid Phone Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtRegPhone.Focus();
                return;
            }

            if (_authService.Register(username, password, fullName, role, phone, address, out string err))
            {
                MessageBox.Show("Registration successful! You are now logged in as a Customer.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show($"Registration Failed: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
