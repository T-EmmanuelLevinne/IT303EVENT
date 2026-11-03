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
            this.title = new System.Windows.Forms.Label();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this._lblPendingBanner = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.Panel();
            this._btnFilterAll = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnFilterStaff = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnFilterCustomers = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.lblChange = new System.Windows.Forms.Label();
            this._cboChangeRole = new System.Windows.Forms.ComboBox();
            this._btnApplyRole = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnDeleteUser = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._dgvUsers = new System.Windows.Forms.DataGridView();
            this._bottomPanel = new System.Windows.Forms.Panel();
            this._btnSaveChanges = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnCancelChanges = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.topPanel.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvUsers)).BeginInit();
            this._bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.pnlActions);
            this.topPanel.Controls.Add(this._lblPendingBanner);
            this.topPanel.Controls.Add(this.pnlSearch);
            this.topPanel.Controls.Add(this.title);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Height = 90;
            this.topPanel.Location = new System.Drawing.Point(24, 24);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1200, 90);
            this.topPanel.TabIndex = 0;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.title.Location = new System.Drawing.Point(0, 4);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(378, 25);
            this.title.TabIndex = 0;
            this.title.Text = "User Management & Role Assignment";
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlSearch.BorderRadius = 8;
            this.pnlSearch.BorderWidth = 1;
            this.pnlSearch.Controls.Add(this._txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearchIcon);
            this.pnlSearch.DrawBorder = true;
            this.pnlSearch.Location = new System.Drawing.Point(410, 4);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(240, 32);
            this.pnlSearch.TabIndex = 1;
            // 
            // lblSearchIcon
            // 
            this.lblSearchIcon.Location = new System.Drawing.Point(8, 7);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Size = new System.Drawing.Size(18, 18);
            this.lblSearchIcon.TabIndex = 0;
            // 
            // _txtSearch
            // 
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtSearch.Location = new System.Drawing.Point(32, 7);
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(195, 17);
            this._txtSearch.TabIndex = 1;
            // 
            // _lblPendingBanner
            // 
            this._lblPendingBanner.AutoSize = true;
            this._lblPendingBanner.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._lblPendingBanner.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this._lblPendingBanner.Location = new System.Drawing.Point(0, 32);
            this._lblPendingBanner.Name = "_lblPendingBanner";
            this._lblPendingBanner.Size = new System.Drawing.Size(650, 17);
            this._lblPendingBanner.TabIndex = 2;
            this._lblPendingBanner.Text = "You have unsaved role changes! Click 'Save Changes' or 'Cancel Changes'.";
            this._lblPendingBanner.Visible = false;
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this._btnDeleteUser);
            this.pnlActions.Controls.Add(this._btnApplyRole);
            this.pnlActions.Controls.Add(this._cboChangeRole);
            this.pnlActions.Controls.Add(this.lblChange);
            this.pnlActions.Controls.Add(this._btnFilterCustomers);
            this.pnlActions.Controls.Add(this._btnFilterStaff);
            this.pnlActions.Controls.Add(this._btnFilterAll);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Height = 38;
            this.pnlActions.Location = new System.Drawing.Point(0, 52);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(1200, 38);
            this.pnlActions.TabIndex = 3;
            // 
            // _btnFilterAll
            // 
            this._btnFilterAll.BorderRadius = 6;
            this._btnFilterAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnFilterAll.Location = new System.Drawing.Point(0, 2);
            this._btnFilterAll.Name = "_btnFilterAll";
            this._btnFilterAll.Size = new System.Drawing.Size(90, 32);
            this._btnFilterAll.TabIndex = 0;
            this._btnFilterAll.Text = "All Users";
            // 
            // _btnFilterStaff
            // 
            this._btnFilterStaff.BorderRadius = 6;
            this._btnFilterStaff.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnFilterStaff.Location = new System.Drawing.Point(100, 2);
            this._btnFilterStaff.Name = "_btnFilterStaff";
            this._btnFilterStaff.Size = new System.Drawing.Size(165, 32);
            this._btnFilterStaff.TabIndex = 1;
            this._btnFilterStaff.Text = "Staff (Admin / Crew)";
            // 
            // _btnFilterCustomers
            // 
            this._btnFilterCustomers.BorderRadius = 6;
            this._btnFilterCustomers.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnFilterCustomers.Location = new System.Drawing.Point(275, 2);
            this._btnFilterCustomers.Name = "_btnFilterCustomers";
            this._btnFilterCustomers.Size = new System.Drawing.Size(110, 32);
            this._btnFilterCustomers.TabIndex = 2;
            this._btnFilterCustomers.Text = "Customers";
            // 
            // lblChange
            // 
            this.lblChange.AutoSize = true;
            this.lblChange.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblChange.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblChange.Location = new System.Drawing.Point(405, 10);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(34, 15);
            this.lblChange.TabIndex = 3;
            this.lblChange.Text = "Role:";
            // 
            // _cboChangeRole
            // 
            this._cboChangeRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboChangeRole.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._cboChangeRole.FormattingEnabled = true;
            this._cboChangeRole.Items.AddRange(new object[] {
            "Customer",
            "Crew",
            "Admin"});
            this._cboChangeRole.Location = new System.Drawing.Point(445, 6);
            this._cboChangeRole.Name = "_cboChangeRole";
            this._cboChangeRole.Size = new System.Drawing.Size(115, 25);
            this._cboChangeRole.TabIndex = 4;
            // 
            // _btnApplyRole
            // 
            this._btnApplyRole.BorderRadius = 6;
            this._btnApplyRole.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnApplyRole.ForeColor = System.Drawing.Color.White;
            this._btnApplyRole.IconName = "check";
            this._btnApplyRole.Location = new System.Drawing.Point(570, 4);
            this._btnApplyRole.Name = "_btnApplyRole";
            this._btnApplyRole.NormalColor = System.Drawing.Color.FromArgb(234, 88, 12);
            this._btnApplyRole.HoverColor = System.Drawing.Color.FromArgb(249, 115, 22);
            this._btnApplyRole.Size = new System.Drawing.Size(115, 32);
            this._btnApplyRole.TabIndex = 5;
            this._btnApplyRole.Text = "Apply Role";
            // 
            // _btnDeleteUser
            // 
            this._btnDeleteUser.BorderRadius = 6;
            this._btnDeleteUser.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnDeleteUser.ForeColor = System.Drawing.Color.White;
            this._btnDeleteUser.IconName = "delete";
            this._btnDeleteUser.Location = new System.Drawing.Point(695, 4);
            this._btnDeleteUser.Name = "_btnDeleteUser";
            this._btnDeleteUser.NormalColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this._btnDeleteUser.HoverColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this._btnDeleteUser.Size = new System.Drawing.Size(115, 32);
            this._btnDeleteUser.TabIndex = 6;
            this._btnDeleteUser.Text = "Delete User";
            // 
            // _dgvUsers
            // 
            this._dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvUsers.Location = new System.Drawing.Point(24, 114);
            this._dgvUsers.MultiSelect = false;
            this._dgvUsers.Name = "_dgvUsers";
            this._dgvUsers.ReadOnly = true;
            this._dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dgvUsers.Size = new System.Drawing.Size(1200, 418);
            this._dgvUsers.TabIndex = 1;
            // 
            // _bottomPanel
            // 
            this._bottomPanel.Controls.Add(this._btnSaveChanges);
            this._bottomPanel.Controls.Add(this._btnCancelChanges);
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Height = 60;
            this._bottomPanel.Location = new System.Drawing.Point(24, 532);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this._bottomPanel.Size = new System.Drawing.Size(1200, 60);
            this._bottomPanel.TabIndex = 2;
            this._bottomPanel.Visible = false;
            // 
            // _btnSaveChanges
            // 
            this._btnSaveChanges.BorderRadius = 8;
            this._btnSaveChanges.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnSaveChanges.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._btnSaveChanges.ForeColor = System.Drawing.Color.White;
            this._btnSaveChanges.Location = new System.Drawing.Point(970, 12);
            this._btnSaveChanges.Name = "_btnSaveChanges";
            this._btnSaveChanges.NormalColor = System.Drawing.Color.FromArgb(234, 88, 12);
            this._btnSaveChanges.HoverColor = System.Drawing.Color.FromArgb(249, 115, 22);
            this._btnSaveChanges.Size = new System.Drawing.Size(180, 48);
            this._btnSaveChanges.TabIndex = 0;
            this._btnSaveChanges.Text = "Save Changes";
            // 
            // _btnCancelChanges
            // 
            this._btnCancelChanges.BorderRadius = 8;
            this._btnCancelChanges.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnCancelChanges.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._btnCancelChanges.Location = new System.Drawing.Point(768, 12);
            this._btnCancelChanges.Name = "_btnCancelChanges";
            this._btnCancelChanges.NormalColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this._btnCancelChanges.HoverColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this._btnCancelChanges.Size = new System.Drawing.Size(160, 48);
            this._btnCancelChanges.TabIndex = 1;
            this._btnCancelChanges.Text = "Cancel Changes";
            // 
            // AdminUsersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this._dgvUsers);
            this.Controls.Add(this._bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "AdminUsersControl";
            this.Padding = new System.Windows.Forms.Padding(24);
            this.Size = new System.Drawing.Size(1248, 616);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvUsers)).EndInit();
            this._bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label title;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.Label _lblPendingBanner;
        private System.Windows.Forms.Panel pnlActions;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterAll;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterStaff;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnFilterCustomers;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.ComboBox _cboChangeRole;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnApplyRole;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnDeleteUser;
        private System.Windows.Forms.DataGridView _dgvUsers;
        private System.Windows.Forms.Panel _bottomPanel;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnSaveChanges;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnCancelChanges;
    }
}
