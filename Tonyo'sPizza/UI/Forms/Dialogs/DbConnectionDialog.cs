using System;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Data;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class DbConnectionDialog : Form
    {
        private TextBox _txtServer;
        private TextBox _txtDatabase;
        private CheckBox _chkWindowsAuth;
        private TextBox _txtUser;
        private TextBox _txtPass;
        private Label _lblStatus;
        private RoundedButton _btnTest;
        private RoundedButton _btnSave;
        private RoundedButton _btnCancel;

        public DbConnectionDialog()
        {
            InitializeComponent();
            LoadCurrentSettings();
        }

        private void InitializeComponent()
        {
            Text = "Database Connection Settings";
            Size = new Size(460, 480);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = AppColors.Background;

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = AppColors.Primary
            };

            Label title = new Label
            {
                Text = "SQL Server Configuration",
                Font = new Font("Segoe UI Bold", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 16),
                AutoSize = true
            };
            header.Controls.Add(title);

            Panel content = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24, 20, 24, 20),
                BackColor = Color.White
            };

            int y = 16;

            Label lblServer = new Label { Text = "SQL Server Name / Instance:", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 24;
            _txtServer = new TextBox { Text = "localhost", Location = new Point(24, y), Width = 390, Font = new Font("Segoe UI", 10f) };
            y += 36;

            Label lblHint = new Label { Text = "Examples: localhost, .\\SQLEXPRESS, (localdb)\\MSSQLLocalDB", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = AppColors.TextMuted };
            y += 24;

            Label lblDb = new Label { Text = "Database Name:", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 24;
            _txtDatabase = new TextBox { Text = "TonyosPizzaDB", Location = new Point(24, y), Width = 390, Font = new Font("Segoe UI", 10f) };
            y += 36;

            _chkWindowsAuth = new CheckBox { Text = "Use Windows Authentication (Integrated Security)", Location = new Point(24, y), AutoSize = true, Checked = true, Font = new Font("Segoe UI", 9.5f) };
            _chkWindowsAuth.CheckedChanged += (s, e) =>
            {
                _txtUser.Enabled = !_chkWindowsAuth.Checked;
                _txtPass.Enabled = !_chkWindowsAuth.Checked;
            };
            y += 32;

            Label lblUser = new Label { Text = "Username (SQL Auth):", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = AppColors.TextMuted };
            y += 20;
            _txtUser = new TextBox { Location = new Point(24, y), Width = 185, Font = new Font("Segoe UI", 9.5f), Enabled = false };

            Label lblPass = new Label { Text = "Password (SQL Auth):", Location = new Point(220, y - 20), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = AppColors.TextMuted };
            _txtPass = new TextBox { Location = new Point(220, y), Width = 194, Font = new Font("Segoe UI", 9.5f), UseSystemPasswordChar = true, Enabled = false };
            y += 36;

            _lblStatus = new Label { Text = string.Empty, Location = new Point(24, y), Width = 390, Height = 34, Font = new Font("Segoe UI", 9f), ForeColor = AppColors.TextDark, AutoEllipsis = true };
            y += 40;

            _btnTest = new RoundedButton
            {
                Text = "Test Connection",
                Location = new Point(24, y),
                Size = new Size(135, 38),
                BorderRadius = 8,
                NormalColor = Color.FromArgb(71, 85, 105),
                HoverColor = Color.FromArgb(100, 116, 139)
            };
            _btnTest.Click += BtnTest_Click;

            _btnSave = new RoundedButton
            {
                Text = "Save & Apply",
                Location = new Point(170, y),
                Size = new Size(130, 38),
                BorderRadius = 8,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight
            };
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Location = new Point(310, y),
                Size = new Size(104, 38),
                BorderRadius = 8,
                NormalColor = Color.FromArgb(226, 232, 240),
                HoverColor = Color.FromArgb(203, 213, 225),
                ForeColor = AppColors.TextDark
            };
            _btnCancel.Click += (s, e) => Close();

            content.Controls.Add(lblServer);
            content.Controls.Add(_txtServer);
            content.Controls.Add(lblHint);
            content.Controls.Add(lblDb);
            content.Controls.Add(_txtDatabase);
            content.Controls.Add(_chkWindowsAuth);
            content.Controls.Add(lblUser);
            content.Controls.Add(_txtUser);
            content.Controls.Add(lblPass);
            content.Controls.Add(_txtPass);
            content.Controls.Add(_lblStatus);
            content.Controls.Add(_btnTest);
            content.Controls.Add(_btnSave);
            content.Controls.Add(_btnCancel);

            Controls.Add(content);
            Controls.Add(header);
        }

        private void LoadCurrentSettings()
        {
            string conn = DatabaseHelper.ConnectionString;
            if (conn.Contains("Server=") || conn.Contains("Data Source="))
            {
                var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
                _txtServer.Text = builder.DataSource;
                _txtDatabase.Text = builder.InitialCatalog;
                _chkWindowsAuth.Checked = builder.IntegratedSecurity;
                if (!builder.IntegratedSecurity)
                {
                    _txtUser.Text = builder.UserID;
                    _txtPass.Text = builder.Password;
                }
            }
        }

        private string BuildConnectionString()
        {
            var builder = new System.Data.SqlClient.SqlConnectionStringBuilder
            {
                DataSource = _txtServer.Text.Trim(),
                InitialCatalog = _txtDatabase.Text.Trim(),
                TrustServerCertificate = true
            };

            if (_chkWindowsAuth.Checked)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.IntegratedSecurity = false;
                builder.UserID = _txtUser.Text.Trim();
                builder.Password = _txtPass.Text;
            }
            return builder.ConnectionString;
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            _lblStatus.Text = "Testing connection...";
            _lblStatus.ForeColor = AppColors.TextDark;
            Application.DoEvents();

            string original = DatabaseHelper.ConnectionString;
            DatabaseHelper.ConnectionString = BuildConnectionString();

            if (DatabaseHelper.TestConnection(out string err))
            {
                _lblStatus.Text = "Connection successful!";
                _lblStatus.ForeColor = AppColors.StatusCompleted;
            }
            else
            {
                _lblStatus.Text = $"Failed: {err}";
                _lblStatus.ForeColor = AppColors.StatusCancelled;
                DatabaseHelper.ConnectionString = original;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DatabaseHelper.ConnectionString = BuildConnectionString();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
