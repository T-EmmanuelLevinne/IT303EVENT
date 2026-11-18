namespace Tonyo_sPizza.UI.Views.Admin
{
    partial class AdminBackupControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSub = new System.Windows.Forms.Label();
            this._lblDbBadge = new System.Windows.Forms.Label();

            this.tlpSplit = new System.Windows.Forms.TableLayoutPanel();

            // Left: Backup Card
            this.pnlBackupCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblBackupTitle = new System.Windows.Forms.Label();
            this.lblBackupDesc = new System.Windows.Forms.Label();
            this.lblDestLabel = new System.Windows.Forms.Label();
            this._txtBackupPath = new System.Windows.Forms.TextBox();
            this._btnBrowseBackup = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnRunBackup = new Tonyo_sPizza.UI.Controls.RoundedButton();

            // Right: Restore Card
            this.pnlRestoreCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblRestoreTitle = new System.Windows.Forms.Label();
            this.lblRestoreDesc = new System.Windows.Forms.Label();
            this.pnlCautionBox = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblCautionText = new System.Windows.Forms.Label();
            this.lblSourceLabel = new System.Windows.Forms.Label();
            this._txtRestorePath = new System.Windows.Forms.TextBox();
            this._btnBrowseRestore = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.lblAdminPass = new System.Windows.Forms.Label();
            this._txtAdminPassword = new System.Windows.Forms.TextBox();
            this._btnRunRestore = new Tonyo_sPizza.UI.Controls.RoundedButton();

            // Bottom: Existing Backups Table
            this.pnlHistoryCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.pnlHistoryHeader = new System.Windows.Forms.Panel();
            this.lblHistoryTitle = new System.Windows.Forms.Label();
            this._btnRefreshList = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._dgvBackups = new System.Windows.Forms.DataGridView();

            this.SuspendLayout();

            // Header
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 68;

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.lblTitle.Location = new System.Drawing.Point(0, 2);
            this.lblTitle.Text = "Database Backup & Recovery";
            this.lblTitle.UseMnemonic = false;

            this.lblSub.AutoSize = true;
            this.lblSub.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.lblSub.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this.lblSub.Location = new System.Drawing.Point(2, 34);
            this.lblSub.Text = "Create SQL Server full backups (.bak) and restore historical snapshots securely (Admin only).";

            this._lblDbBadge.AutoSize = true;
            this._lblDbBadge.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._lblDbBadge.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCompleted;
            this._lblDbBadge.Dock = System.Windows.Forms.DockStyle.Right;
            this._lblDbBadge.Padding = new System.Windows.Forms.Padding(0, 14, 0, 0);
            this._lblDbBadge.Text = "Active Database: TonyosPizzaDB";

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSub);
            this.pnlHeader.Controls.Add(this._lblDbBadge);

            // Split Cards
            this.tlpSplit.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpSplit.Height = 278;
            this.tlpSplit.ColumnCount = 2;
            this.tlpSplit.RowCount = 1;
            this.tlpSplit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48F));
            this.tlpSplit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52F));

            // Left Card: Backup
            this.pnlBackupCard.Size = new System.Drawing.Size(480, 260);
            this.pnlBackupCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackupCard.Margin = new System.Windows.Forms.Padding(0, 6, 10, 12);
            this.pnlBackupCard.Padding = new System.Windows.Forms.Padding(20);
            this.pnlBackupCard.BorderRadius = 12;

            this.lblBackupTitle.Text = "Create Full Backup";
            this.lblBackupTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBackupTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.lblBackupTitle.Location = new System.Drawing.Point(20, 16);
            this.lblBackupTitle.AutoSize = true;

            this.lblBackupDesc.Text = "Saves a complete snapshot of all menu items, stock inventory, orders, user accounts, and audit trails.";
            this.lblBackupDesc.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lblBackupDesc.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this.lblBackupDesc.Location = new System.Drawing.Point(20, 44);
            this.lblBackupDesc.Size = new System.Drawing.Size(440, 34);
            this.lblBackupDesc.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            this.lblDestLabel.Text = "Backup File Destination (.bak)";
            this.lblDestLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblDestLabel.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this.lblDestLabel.Location = new System.Drawing.Point(20, 92);
            this.lblDestLabel.AutoSize = true;

            this._txtBackupPath.Location = new System.Drawing.Point(20, 114);
            this._txtBackupPath.Size = new System.Drawing.Size(340, 26);
            this._txtBackupPath.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtBackupPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            this._btnBrowseBackup.Text = "Browse";
            this._btnBrowseBackup.Size = new System.Drawing.Size(92, 30);
            this._btnBrowseBackup.Location = new System.Drawing.Point(368, 112);
            this._btnBrowseBackup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this._btnBrowseBackup.BorderRadius = 7;
            this._btnBrowseBackup.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnBrowseBackup.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnBrowseBackup.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;

            this._btnRunBackup.Text = "Create Backup Now";
            this._btnRunBackup.IconName = "backup";
            this._btnRunBackup.Size = new System.Drawing.Size(210, 40);
            this._btnRunBackup.Location = new System.Drawing.Point(20, 196);
            this._btnRunBackup.BorderRadius = 8;
            this._btnRunBackup.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.Primary;
            this._btnRunBackup.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryLight;
            this._btnRunBackup.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryDark;

            this.pnlBackupCard.Controls.Add(this.lblBackupTitle);
            this.pnlBackupCard.Controls.Add(this.lblBackupDesc);
            this.pnlBackupCard.Controls.Add(this.lblDestLabel);
            this.pnlBackupCard.Controls.Add(this._txtBackupPath);
            this.pnlBackupCard.Controls.Add(this._btnBrowseBackup);
            this.pnlBackupCard.Controls.Add(this._btnRunBackup);

            // Right Card: Restore
            this.pnlRestoreCard.Size = new System.Drawing.Size(510, 260);
            this.pnlRestoreCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRestoreCard.Margin = new System.Windows.Forms.Padding(6, 6, 0, 12);
            this.pnlRestoreCard.Padding = new System.Windows.Forms.Padding(20);
            this.pnlRestoreCard.BorderRadius = 12;

            this.lblRestoreTitle.Text = "Restore from Backup";
            this.lblRestoreTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRestoreTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.lblRestoreTitle.Location = new System.Drawing.Point(20, 16);
            this.lblRestoreTitle.AutoSize = true;

            this.pnlCautionBox.Location = new System.Drawing.Point(20, 44);
            this.pnlCautionBox.Size = new System.Drawing.Size(470, 42);
            this.pnlCautionBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.pnlCautionBox.BackColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPendingBg;
            this.pnlCautionBox.BorderColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPending;
            this.pnlCautionBox.BorderRadius = 8;

            this.lblCautionText.Text = "Caution: Restoring replaces all current records with the selected backup snapshot. Administrator authorization is required.";
            this.lblCautionText.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblCautionText.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPending;
            this.lblCautionText.Location = new System.Drawing.Point(10, 6);
            this.lblCautionText.Size = new System.Drawing.Size(450, 30);
            this.lblCautionText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.pnlCautionBox.Controls.Add(this.lblCautionText);

            this.lblSourceLabel.Text = "Backup File to Restore (.bak)";
            this.lblSourceLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblSourceLabel.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this.lblSourceLabel.Location = new System.Drawing.Point(20, 96);
            this.lblSourceLabel.AutoSize = true;

            this._txtRestorePath.Location = new System.Drawing.Point(20, 118);
            this._txtRestorePath.Size = new System.Drawing.Size(370, 26);
            this._txtRestorePath.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtRestorePath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            this._btnBrowseRestore.Text = "Browse";
            this._btnBrowseRestore.Size = new System.Drawing.Size(92, 30);
            this._btnBrowseRestore.Location = new System.Drawing.Point(398, 116);
            this._btnBrowseRestore.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this._btnBrowseRestore.BorderRadius = 7;
            this._btnBrowseRestore.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnBrowseRestore.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnBrowseRestore.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;

            this.lblAdminPass.Text = "Admin Password Confirmation *";
            this.lblAdminPass.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblAdminPass.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this.lblAdminPass.Location = new System.Drawing.Point(20, 156);
            this.lblAdminPass.AutoSize = true;

            this._txtAdminPassword.Location = new System.Drawing.Point(20, 178);
            this._txtAdminPassword.Size = new System.Drawing.Size(218, 26);
            this._txtAdminPassword.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtAdminPassword.UseSystemPasswordChar = true;

            this._btnRunRestore.Text = "Restore Database";
            this._btnRunRestore.IconName = "restore";
            this._btnRunRestore.Size = new System.Drawing.Size(185, 40);
            this._btnRunRestore.Location = new System.Drawing.Point(305, 170);
            this._btnRunRestore.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this._btnRunRestore.BorderRadius = 8;
            this._btnRunRestore.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCancelled;
            this._btnRunRestore.HoverColor = System.Drawing.Color.FromArgb(205, 45, 45);
            this._btnRunRestore.PressedColor = System.Drawing.Color.FromArgb(153, 27, 27);

            this.pnlRestoreCard.Controls.Add(this.lblRestoreTitle);
            this.pnlRestoreCard.Controls.Add(this.pnlCautionBox);
            this.pnlRestoreCard.Controls.Add(this.lblSourceLabel);
            this.pnlRestoreCard.Controls.Add(this._txtRestorePath);
            this.pnlRestoreCard.Controls.Add(this._btnBrowseRestore);
            this.pnlRestoreCard.Controls.Add(this.lblAdminPass);
            this.pnlRestoreCard.Controls.Add(this._txtAdminPassword);
            this.pnlRestoreCard.Controls.Add(this._btnRunRestore);

            this.tlpSplit.Controls.Add(this.pnlBackupCard, 0, 0);
            this.tlpSplit.Controls.Add(this.pnlRestoreCard, 1, 0);

            // Bottom Card: Available Backup Files
            this.pnlHistoryCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHistoryCard.Padding = new System.Windows.Forms.Padding(18);
            this.pnlHistoryCard.BorderRadius = 12;

            this.pnlHistoryHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHistoryHeader.Height = 38;
            this.pnlHistoryHeader.BackColor = System.Drawing.Color.White;

            this.lblHistoryTitle.Text = "Available Backup Files (Click any file to select for restore)";
            this.lblHistoryTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblHistoryTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.lblHistoryTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblHistoryTitle.AutoSize = true;

            this._btnRefreshList.Text = "Refresh List";
            this._btnRefreshList.IconName = "refresh";
            this._btnRefreshList.Size = new System.Drawing.Size(128, 32);
            this._btnRefreshList.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnRefreshList.BorderRadius = 7;
            this._btnRefreshList.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnRefreshList.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnRefreshList.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._btnRefreshList.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);

            this.pnlHistoryHeader.Controls.Add(this.lblHistoryTitle);
            this.pnlHistoryHeader.Controls.Add(this._btnRefreshList);

            this._dgvBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHistoryCard.Controls.Add(this._dgvBackups);
            this.pnlHistoryCard.Controls.Add(this.pnlHistoryHeader);

            this.BackColor = Tonyo_sPizza.UI.Theme.AppColors.Background;
            this.Controls.Add(this.pnlHistoryCard);
            this.Controls.Add(this.tlpSplit);
            this.Controls.Add(this.pnlHeader);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);
            this.Name = "AdminBackupControl";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Label _lblDbBadge;

        private System.Windows.Forms.TableLayoutPanel tlpSplit;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlBackupCard;
        private System.Windows.Forms.Label lblBackupTitle;
        private System.Windows.Forms.Label lblBackupDesc;
        private System.Windows.Forms.Label lblDestLabel;
        private System.Windows.Forms.TextBox _txtBackupPath;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnBrowseBackup;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRunBackup;

        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlRestoreCard;
        private System.Windows.Forms.Label lblRestoreTitle;
        private System.Windows.Forms.Label lblRestoreDesc;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlCautionBox;
        private System.Windows.Forms.Label lblCautionText;
        private System.Windows.Forms.Label lblSourceLabel;
        private System.Windows.Forms.TextBox _txtRestorePath;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnBrowseRestore;
        private System.Windows.Forms.Label lblAdminPass;
        private System.Windows.Forms.TextBox _txtAdminPassword;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRunRestore;

        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlHistoryCard;
        private System.Windows.Forms.Panel pnlHistoryHeader;
        private System.Windows.Forms.Label lblHistoryTitle;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefreshList;
        private System.Windows.Forms.DataGridView _dgvBackups;
    }
}
