using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminBackupControl : UserControl
    {
        public AdminBackupControl()
        {
            InitializeComponent();
            SetupCustomUIAndEvents();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LoadBackupData();
            }
        }

        private void SetupCustomUIAndEvents()
        {
            UIHelper.EnableDoubleBuffering(this);

            UIHelper.StyleDataGridView(_dgvBackups);
            _dgvBackups.Columns.Add("FileName", "Backup File Name");
            _dgvBackups.Columns.Add("CreatedAt", "Created On");
            _dgvBackups.Columns.Add("Size", "File Size");
            _dgvBackups.Columns.Add("FullPath", "Location");

            _dgvBackups.Columns["FileName"].Width = 260;
            _dgvBackups.Columns["CreatedAt"].Width = 175;
            _dgvBackups.Columns["Size"].Width = 100;
            _dgvBackups.Columns["FullPath"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            _dgvBackups.SelectionChanged += (s, e) =>
            {
                if (_dgvBackups.SelectedRows.Count > 0)
                {
                    string path = _dgvBackups.SelectedRows[0].Cells["FullPath"].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        _txtRestorePath.Text = path;
                    }
                }
            };

            _btnBrowseBackup.Click += BtnBrowseBackup_Click;
            _btnRunBackup.Click += BtnRunBackup_Click;
            _btnBrowseRestore.Click += BtnBrowseRestore_Click;
            _btnRunRestore.Click += BtnRunRestore_Click;
            _btnRefreshList.Click += (s, e) => LoadBackupData();
        }

        public void LoadBackupData()
        {
            try
            {
                string dbName = BackupService.GetCurrentDatabaseName();
                _lblDbBadge.Text = $"Active Database: {dbName}";

                string defaultDir = BackupService.GetDefaultBackupDirectory();
                if (string.IsNullOrWhiteSpace(_txtBackupPath.Text))
                {
                    _txtBackupPath.Text = Path.Combine(defaultDir, $"{dbName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak");
                }

                _dgvBackups.Rows.Clear();
                var files = BackupService.GetAvailableBackups(defaultDir);
                foreach (var f in files)
                {
                    _dgvBackups.Rows.Add(
                        f.FileName,
                        f.CreatedAt.ToString("yyyy-MM-dd hh:mm tt"),
                        f.FormattedSize,
                        f.FullPath
                    );
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AdminBackupControl.LoadBackupData");
            }
        }

        private void BtnBrowseBackup_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Select Backup Destination File";
                sfd.Filter = "SQL Server Backup (*.bak)|*.bak";
                sfd.InitialDirectory = BackupService.GetDefaultBackupDirectory();
                sfd.FileName = $"{BackupService.GetCurrentDatabaseName()}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    _txtBackupPath.Text = sfd.FileName;
                }
            }
        }

        private void BtnRunBackup_Click(object sender, EventArgs e)
        {
            try
            {
                AuthService.Instance.RequireRole("Admin");
                string path = _txtBackupPath.Text.Trim();
                if (string.IsNullOrWhiteSpace(path))
                {
                    MessageBox.Show(this, "Please select a destination .bak file path.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show(this, $"Create a full database backup to:\n{path}?", "Confirm Database Backup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                Cursor = Cursors.WaitCursor;
                bool ok = BackupService.BackupDatabase(path, out string msg);
                Cursor = Cursors.Default;

                if (ok)
                {
                    MessageBox.Show(this, msg, "Backup Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _txtBackupPath.Text = Path.Combine(BackupService.GetDefaultBackupDirectory(), $"{BackupService.GetCurrentDatabaseName()}_{DateTime.Now:yyyyMMdd_HHmmss}.bak");
                    LoadBackupData();
                }
                else
                {
                    MessageBox.Show(this, msg, "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                ErrorLogger.ShowFriendlyError("Could not complete database backup.", ex, "Backup Error", this);
            }
        }

        private void BtnBrowseRestore_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Select SQL Server Backup (.bak) to Restore";
                ofd.Filter = "SQL Server Backup (*.bak)|*.bak|All Files (*.*)|*.*";
                ofd.InitialDirectory = BackupService.GetDefaultBackupDirectory();
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    _txtRestorePath.Text = ofd.FileName;
                }
            }
        }

        private void BtnRunRestore_Click(object sender, EventArgs e)
        {
            try
            {
                AuthService.Instance.RequireRole("Admin");

                string path = _txtRestorePath.Text.Trim();
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    MessageBox.Show(this, "Please select an existing .bak backup file to restore.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string pass = _txtAdminPassword.Text;
                if (string.IsNullOrWhiteSpace(pass) || !AuthService.Instance.VerifyCurrentAdminPassword(pass))
                {
                    MessageBox.Show(this, "Please enter your valid Administrator password to authorize a database restore.", "Authorization Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _txtAdminPassword.Focus();
                    return;
                }

                if (MessageBox.Show(
                    this,
                    $"WARNING: Restoring from '{Path.GetFileName(path)}' will overwrite ALL current data in the active database with the backup state.\n\nAre you certain you want to proceed?",
                    "Confirm Database Restore",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    return;
                }

                Cursor = Cursors.WaitCursor;
                bool ok = BackupService.RestoreDatabase(path, out string msg);
                Cursor = Cursors.Default;

                if (ok)
                {
                    _txtAdminPassword.Clear();
                    MessageBox.Show(this, msg, "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBackupData();
                }
                else
                {
                    MessageBox.Show(this, msg, "Restore Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                ErrorLogger.ShowFriendlyError("Database restore encountered an error.", ex, "Restore Error", this);
            }
        }
    }
}
