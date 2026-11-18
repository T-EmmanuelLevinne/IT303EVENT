namespace Tonyo_sPizza.UI.Views.Admin
{
    partial class AdminCategoryControl
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
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this._chkShowArchived = new System.Windows.Forms.CheckBox();
            this._btnAdd = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnEdit = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnToggleArchive = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnDelete = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnRefresh = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.pnlGridCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._dgvCategories = new System.Windows.Forms.DataGridView();

            this.topPanel.SuspendLayout();
            this.pnlTopRow.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.pnlGridCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvCategories)).BeginInit();
            this.SuspendLayout();

            // topPanel
            this.topPanel.Controls.Add(this.pnlActions);
            this.topPanel.Controls.Add(this.pnlTopRow);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Height = 104;
            this.topPanel.Name = "topPanel";

            // pnlTopRow
            this.pnlTopRow.Controls.Add(this.pnlSearch);
            this.pnlTopRow.Controls.Add(this.lblSubtitle);
            this.pnlTopRow.Controls.Add(this.title);
            this.pnlTopRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopRow.Height = 52;
            this.pnlTopRow.Name = "pnlTopRow";

            // title
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Name = "title";
            this.title.Text = "Menu Categories";

            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.lblSubtitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this.lblSubtitle.Location = new System.Drawing.Point(2, 30);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Text = "Organize customer menu sections, category icons, and archived groups.";

            // pnlSearch
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.BorderColor = Tonyo_sPizza.UI.Theme.AppColors.Border;
            this.pnlSearch.BorderRadius = 8;
            this.pnlSearch.BorderWidth = 1;
            this.pnlSearch.Controls.Add(this._txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearchIcon);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearch.DrawBorder = true;
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(270, 36);

            // lblSearchIcon
            this.lblSearchIcon.Location = new System.Drawing.Point(10, 10);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Size = new System.Drawing.Size(16, 16);

            // _txtSearch
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtSearch.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._txtSearch.Location = new System.Drawing.Point(32, 9);
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(226, 17);

            // pnlActions
            this.pnlActions.AutoScroll = true;
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Height = 46;
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.pnlActions.WrapContents = false;

            // _chkShowArchived
            this._chkShowArchived.AutoSize = true;
            this._chkShowArchived.Cursor = System.Windows.Forms.Cursors.Hand;
            this._chkShowArchived.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._chkShowArchived.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this._chkShowArchived.Margin = new System.Windows.Forms.Padding(0, 8, 16, 0);
            this._chkShowArchived.Name = "_chkShowArchived";
            this._chkShowArchived.Text = "Show Archived";
            this._chkShowArchived.UseVisualStyleBackColor = true;

            // _btnAdd
            this._btnAdd.BorderRadius = 7;
            this._btnAdd.ForeColor = System.Drawing.Color.White;
            this._btnAdd.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryLight;
            this._btnAdd.IconName = "add";
            this._btnAdd.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.Primary;
            this._btnAdd.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryDark;
            this._btnAdd.Size = new System.Drawing.Size(142, 34);
            this._btnAdd.Text = "Add Category";

            // _btnEdit
            this._btnEdit.BorderRadius = 7;
            this._btnEdit.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._btnEdit.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnEdit.IconName = "edit";
            this._btnEdit.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnEdit.Name = "_btnEdit";
            this._btnEdit.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnEdit.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryPressed;
            this._btnEdit.Size = new System.Drawing.Size(96, 34);
            this._btnEdit.Text = "Edit";

            // _btnToggleArchive
            this._btnToggleArchive.BorderRadius = 7;
            this._btnToggleArchive.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPending;
            this._btnToggleArchive.HoverColor = System.Drawing.Color.FromArgb(253, 230, 138);
            this._btnToggleArchive.IconName = "archive";
            this._btnToggleArchive.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnToggleArchive.Name = "_btnToggleArchive";
            this._btnToggleArchive.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPendingBg;
            this._btnToggleArchive.PressedColor = System.Drawing.Color.FromArgb(252, 211, 77);
            this._btnToggleArchive.Size = new System.Drawing.Size(108, 34);
            this._btnToggleArchive.Text = "Archive";

            // _btnDelete
            this._btnDelete.BorderRadius = 7;
            this._btnDelete.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCancelled;
            this._btnDelete.HoverColor = System.Drawing.Color.FromArgb(254, 202, 202);
            this._btnDelete.IconName = "delete";
            this._btnDelete.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCancelledBg;
            this._btnDelete.PressedColor = System.Drawing.Color.FromArgb(252, 165, 165);
            this._btnDelete.Size = new System.Drawing.Size(96, 34);
            this._btnDelete.Text = "Delete";

            // _btnRefresh
            this._btnRefresh.BorderRadius = 7;
            this._btnRefresh.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._btnRefresh.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnRefresh.IconName = "refresh";
            this._btnRefresh.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnRefresh.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryPressed;
            this._btnRefresh.Size = new System.Drawing.Size(104, 34);
            this._btnRefresh.Text = "Refresh";

            this.pnlActions.Controls.Add(this._chkShowArchived);
            this.pnlActions.Controls.Add(this._btnAdd);
            this.pnlActions.Controls.Add(this._btnEdit);
            this.pnlActions.Controls.Add(this._btnToggleArchive);
            this.pnlActions.Controls.Add(this._btnDelete);
            this.pnlActions.Controls.Add(this._btnRefresh);

            // pnlGridCard
            this.pnlGridCard.BackColor = System.Drawing.Color.White;
            this.pnlGridCard.BorderColor = Tonyo_sPizza.UI.Theme.AppColors.Border;
            this.pnlGridCard.BorderRadius = 12;
            this.pnlGridCard.BorderWidth = 1;
            this.pnlGridCard.Controls.Add(this._dgvCategories);
            this.pnlGridCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridCard.DrawBorder = true;
            this.pnlGridCard.Name = "pnlGridCard";
            this.pnlGridCard.Padding = new System.Windows.Forms.Padding(12);

            // _dgvCategories
            this._dgvCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvCategories.MultiSelect = false;
            this._dgvCategories.Name = "_dgvCategories";
            this._dgvCategories.ReadOnly = true;
            this._dgvCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // AdminCategoryControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Tonyo_sPizza.UI.Theme.AppColors.Background;
            this.Controls.Add(this.pnlGridCard);
            this.Controls.Add(this.topPanel);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "AdminCategoryControl";
            this.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);
            this.Size = new System.Drawing.Size(1248, 592);

            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.pnlTopRow.ResumeLayout(false);
            this.pnlTopRow.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.pnlGridCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgvCategories)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel pnlTopRow;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label lblSubtitle;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private System.Windows.Forms.CheckBox _chkShowArchived;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnAdd;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnEdit;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnToggleArchive;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnDelete;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefresh;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlGridCard;
        private System.Windows.Forms.DataGridView _dgvCategories;
    }
}
