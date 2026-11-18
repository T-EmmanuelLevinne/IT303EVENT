namespace Tonyo_sPizza.UI.Views.Admin
{
    partial class AdminUsersControl
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.topPanel = new System.Windows.Forms.Panel();
            this.pnlTopRow = new System.Windows.Forms.Panel();
            this.title = new System.Windows.Forms.Label();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this._lblPendingBanner = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this._btnFilterAll = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnFilterStaff = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnFilterCustomers = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnFilterInactive = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnAddUser = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnEditUser = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnResetPassword = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnToggleActive = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnUnlockUser = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnDeleteUser = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.lblChange = new System.Windows.Forms.Label();
            this._cboChangeRole = new System.Windows.Forms.ComboBox();
            this._btnApplyRole = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.pnlGridCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._dgvUsers = new System.Windows.Forms.DataGridView();
            this._bottomPanel = new System.Windows.Forms.Panel();
            this._btnSaveChanges = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnCancelChanges = new Tonyo_sPizza.UI.Controls.RoundedButton();

            this.topPanel.SuspendLayout();
            this.pnlTopRow.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.pnlGridCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvUsers)).BeginInit();
            this._bottomPanel.SuspendLayout();
            this.SuspendLayout();

            // topPanel
            this.topPanel.Controls.Add(this.pnlActions);
            this.topPanel.Controls.Add(this._lblPendingBanner);
            this.topPanel.Controls.Add(this.pnlTopRow);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Height = 132;
            this.topPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);

            // Top row: Title + Search
            this.pnlTopRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopRow.Height = 40;
            this.pnlTopRow.Controls.Add(this.pnlSearch);
            this.pnlTopRow.Controls.Add(this.title);

            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.title.Location = new System.Drawing.Point(0, 2);
            this.title.Text = "User Accounts & Security";
            this.title.UseMnemonic = false;

            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.BorderRadius = 8;
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearch.Size = new System.Drawing.Size(260, 34);
            this.lblSearchIcon.Location = new System.Drawing.Point(10, 9);
            this.lblSearchIcon.Size = new System.Drawing.Size(16, 16);
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtSearch.Location = new System.Drawing.Point(32, 8);
            this._txtSearch.Size = new System.Drawing.Size(218, 17);
            this.pnlSearch.Controls.Add(this._txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearchIcon);

            // Pending Banner
            this._lblPendingBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this._lblPendingBanner.Height = 22;
            this._lblPendingBanner.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._lblPendingBanner.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPending;
            this._lblPendingBanner.Text = "Unsaved role updates pending. Click 'Save Changes' or 'Cancel Changes' below.";
            this._lblPendingBanner.Visible = false;

            // Actions Bar
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActions.AutoSize = true;
            this.pnlActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlActions.WrapContents = true;
            this.pnlActions.AutoScroll = false;
            this.pnlActions.Padding = new System.Windows.Forms.Padding(0, 6, 0, 2);

            this._btnFilterAll.Text = "All";
            this._btnFilterAll.Size = new System.Drawing.Size(64, 34);
            this._btnFilterAll.Margin = new System.Windows.Forms.Padding(0, 0, 6, 6);
            this._btnFilterAll.BorderRadius = 7;

            this._btnFilterStaff.Text = "Staff";
            this._btnFilterStaff.Size = new System.Drawing.Size(72, 34);
            this._btnFilterStaff.Margin = new System.Windows.Forms.Padding(0, 0, 6, 6);
            this._btnFilterStaff.BorderRadius = 7;

            this._btnFilterCustomers.Text = "Customers";
            this._btnFilterCustomers.Size = new System.Drawing.Size(96, 34);
            this._btnFilterCustomers.Margin = new System.Windows.Forms.Padding(0, 0, 6, 6);
            this._btnFilterCustomers.BorderRadius = 7;

            this._btnFilterInactive.Text = "Inactive / Locked";
            this._btnFilterInactive.Size = new System.Drawing.Size(128, 34);
            this._btnFilterInactive.Margin = new System.Windows.Forms.Padding(0, 0, 14, 6);
            this._btnFilterInactive.BorderRadius = 7;

            this._btnAddUser.Text = "Add User";
            this._btnAddUser.IconName = "user_add";
            this._btnAddUser.Size = new System.Drawing.Size(112, 34);
            this._btnAddUser.Margin = new System.Windows.Forms.Padding(0, 0, 6, 6);
            this._btnAddUser.BorderRadius = 7;
            this._btnAddUser.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.Primary;
            this._btnAddUser.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryLight;

            this._btnEditUser.Text = "Edit";
            this._btnEditUser.IconName = "edit";
            this._btnEditUser.Size = new System.Drawing.Size(80, 34);
            this._btnEditUser.Margin = new System.Windows.Forms.Padding(0, 0, 6, 6);
            this._btnEditUser.BorderRadius = 7;
            this._btnEditUser.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnEditUser.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnEditUser.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;

            this._btnResetPassword.Text = "Reset Password";
            this._btnResetPassword.IconName = "key";
            this._btnResetPassword.Size = new System.Drawing.Size(138, 34);
            this._btnResetPassword.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._btnResetPassword.BorderRadius = 7;
            this._btnResetPassword.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnResetPassword.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnResetPassword.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;

            this._btnToggleActive.Text = "Deactivate";
            this._btnToggleActive.IconName = "shield";
            this._btnToggleActive.Size = new System.Drawing.Size(108, 34);
            this._btnToggleActive.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._btnToggleActive.BorderRadius = 7;
            this._btnToggleActive.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPendingBg;
            this._btnToggleActive.HoverColor = System.Drawing.Color.FromArgb(253, 230, 138);
            this._btnToggleActive.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPending;

            this._btnUnlockUser.Text = "Unlock";
            this._btnUnlockUser.IconName = "unlock";
            this._btnUnlockUser.Size = new System.Drawing.Size(88, 34);
            this._btnUnlockUser.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._btnUnlockUser.BorderRadius = 7;
            this._btnUnlockUser.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCompletedBg;
            this._btnUnlockUser.HoverColor = System.Drawing.Color.FromArgb(187, 247, 208);
            this._btnUnlockUser.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCompleted;

            this._btnDeleteUser.Text = "Delete";
            this._btnDeleteUser.IconName = "delete";
            this._btnDeleteUser.Size = new System.Drawing.Size(88, 34);
            this._btnDeleteUser.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this._btnDeleteUser.BorderRadius = 7;
            this._btnDeleteUser.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCancelledBg;
            this._btnDeleteUser.HoverColor = System.Drawing.Color.FromArgb(254, 202, 202);
            this._btnDeleteUser.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCancelled;

            this.lblChange.AutoSize = true;
            this.lblChange.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblChange.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this.lblChange.Margin = new System.Windows.Forms.Padding(0, 9, 4, 0);
            this.lblChange.Text = "Role:";

            this._cboChangeRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboChangeRole.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._cboChangeRole.Items.AddRange(new object[] { "Customer", "Crew", "Admin" });
            this._cboChangeRole.Size = new System.Drawing.Size(96, 25);
            this._cboChangeRole.Margin = new System.Windows.Forms.Padding(0, 5, 6, 0);
            this._cboChangeRole.SelectedIndex = 0;

            this._btnApplyRole.Text = "Apply";
            this._btnApplyRole.Size = new System.Drawing.Size(72, 34);
            this._btnApplyRole.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._btnApplyRole.BorderRadius = 7;
            this._btnApplyRole.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this._btnApplyRole.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;

            this.pnlActions.Controls.Add(this._btnFilterAll);
            this.pnlActions.Controls.Add(this._btnFilterStaff);
            this.pnlActions.Controls.Add(this._btnFilterCustomers);
            this.pnlActions.Controls.Add(this._btnFilterInactive);
            this.pnlActions.Controls.Add(this._btnAddUser);
            this.pnlActions.Controls.Add(this._btnEditUser);
            this.pnlActions.Controls.Add(this._btnResetPassword);
            this.pnlActions.Controls.Add(this._btnToggleActive);
            this.pnlActions.Controls.Add(this._btnUnlockUser);
            this.pnlActions.Controls.Add(this._btnDeleteUser);
            this.pnlActions.Controls.Add(this.lblChange);
            this.pnlActions.Controls.Add(this._cboChangeRole);
            this.pnlActions.Controls.Add(this._btnApplyRole);

            // Grid Card
            this.pnlGridCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridCard.BorderRadius = 12;
            this.pnlGridCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlGridCard.Controls.Add(this._dgvUsers);
            this._dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;

            // Bottom Save Bar
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Height = 56;
            this._bottomPanel.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this._bottomPanel.Visible = false;

            this._btnSaveChanges.Text = "Save Role Changes";
            this._btnSaveChanges.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnSaveChanges.Size = new System.Drawing.Size(170, 42);
            this._btnSaveChanges.BorderRadius = 8;
            this._btnSaveChanges.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.Primary;
            this._btnSaveChanges.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryLight;

            this._btnCancelChanges.Text = "Cancel Changes";
            this._btnCancelChanges.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnCancelChanges.Size = new System.Drawing.Size(140, 42);
            this._btnCancelChanges.BorderRadius = 8;
            this._btnCancelChanges.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnCancelChanges.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnCancelChanges.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;

            this._bottomPanel.Controls.Add(this._btnSaveChanges);
            this._bottomPanel.Controls.Add(new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Right, Width = 10 });
            this._bottomPanel.Controls.Add(this._btnCancelChanges);

            this.BackColor = Tonyo_sPizza.UI.Theme.AppColors.Background;
            this.Controls.Add(this.pnlGridCard);
            this.Controls.Add(this._bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "AdminUsersControl";
            this.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);
            this.Size = new System.Drawing.Size(1248, 616);

            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.pnlTopRow.ResumeLayout(false);
            this.pnlTopRow.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.pnlGridCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgvUsers)).EndInit();
            this._bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel pnlTopRow;
        private System.Windows.Forms.Label title;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.Label _lblPendingBanner;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterAll;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterStaff;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterCustomers;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterInactive;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnAddUser;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnEditUser;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnResetPassword;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnToggleActive;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnUnlockUser;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnDeleteUser;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.ComboBox _cboChangeRole;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnApplyRole;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlGridCard;
        private System.Windows.Forms.DataGridView _dgvUsers;
        private System.Windows.Forms.Panel _bottomPanel;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnSaveChanges;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnCancelChanges;
    }
}
