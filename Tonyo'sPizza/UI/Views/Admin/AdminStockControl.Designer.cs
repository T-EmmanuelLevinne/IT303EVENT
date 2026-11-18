namespace Tonyo_sPizza.UI.Views.Admin
{
    partial class AdminStockControl
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
            this.pnlTopHeaderRow = new System.Windows.Forms.Panel();
            this.title = new System.Windows.Forms.Label();
            this._btnTabInventory = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnTabTransactions = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._lblPendingChangesBanner = new System.Windows.Forms.Label();
            this.flpActions = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.pnlCatFilter = new System.Windows.Forms.Panel();
            this._cboCategoryFilter = new System.Windows.Forms.ComboBox();
            this.lblCat = new System.Windows.Forms.Label();
            this._chkShowArchived = new System.Windows.Forms.CheckBox();
            this._btnAddIngredient = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnRestock = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnEdit = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnArchiveToggle = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnDelete = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnVoidTransaction = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnRefreshTx = new Tonyo_sPizza.UI.Controls.RoundedButton();

            this.pnlGridContainer = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._dgvStock = new System.Windows.Forms.DataGridView();
            this._dgvTransactions = new System.Windows.Forms.DataGridView();
            this._bottomPanel = new System.Windows.Forms.Panel();
            this._btnSaveChanges = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnCancelChanges = new Tonyo_sPizza.UI.Controls.RoundedButton();

            this.topPanel.SuspendLayout();
            this.pnlTopHeaderRow.SuspendLayout();
            this.flpActions.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlCatFilter.SuspendLayout();
            this.pnlGridContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dgvTransactions)).BeginInit();
            this._bottomPanel.SuspendLayout();
            this.SuspendLayout();

            // topPanel
            this.topPanel.Controls.Add(this.flpActions);
            this.topPanel.Controls.Add(this._lblPendingChangesBanner);
            this.topPanel.Controls.Add(this.pnlTopHeaderRow);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Height = 132;
            this.topPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);

            this.pnlTopHeaderRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopHeaderRow.Height = 42;

            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.title.Location = new System.Drawing.Point(0, 2);
            this.title.Text = "Ingredient Inventory & Stock Ledger";
            this.title.UseMnemonic = false;

            this._btnTabTransactions.Text = "Stock Transactions";
            this._btnTabTransactions.IconName = "orders";
            this._btnTabTransactions.Size = new System.Drawing.Size(162, 34);
            this._btnTabTransactions.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnTabTransactions.BorderRadius = 8;

            this._btnTabInventory.Text = "Current Inventory";
            this._btnTabInventory.IconName = "stock";
            this._btnTabInventory.Size = new System.Drawing.Size(156, 34);
            this._btnTabInventory.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnTabInventory.BorderRadius = 8;

            this.pnlTopHeaderRow.Controls.Add(this.title);
            this.pnlTopHeaderRow.Controls.Add(this._btnTabTransactions);
            this.pnlTopHeaderRow.Controls.Add(new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Right, Width = 8 });
            this.pnlTopHeaderRow.Controls.Add(this._btnTabInventory);

            // Pending Banner
            this._lblPendingChangesBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this._lblPendingChangesBanner.Height = 20;
            this._lblPendingChangesBanner.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this._lblPendingChangesBanner.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPending;
            this._lblPendingChangesBanner.Text = "Unsaved stock changes pending. Click 'Save Changes' or 'Cancel Changes' below.";
            this._lblPendingChangesBanner.Visible = false;

            // Actions Bar
            this.flpActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpActions.AutoSize = true;
            this.flpActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpActions.WrapContents = true;
            this.flpActions.AutoScroll = false;
            this.flpActions.Padding = new System.Windows.Forms.Padding(0, 6, 0, 2);

            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.BorderRadius = 8;
            this.pnlSearch.Margin = new System.Windows.Forms.Padding(0, 0, 8, 6);
            this.pnlSearch.Size = new System.Drawing.Size(210, 34);
            this.lblSearchIcon.Location = new System.Drawing.Point(9, 9);
            this.lblSearchIcon.Size = new System.Drawing.Size(16, 16);
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._txtSearch.Location = new System.Drawing.Point(30, 8);
            this._txtSearch.Size = new System.Drawing.Size(170, 17);
            this.pnlSearch.Controls.Add(this._txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearchIcon);

            this.pnlCatFilter.Size = new System.Drawing.Size(178, 34);
            this.pnlCatFilter.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.lblCat.AutoSize = true;
            this.lblCat.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCat.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this.lblCat.Location = new System.Drawing.Point(0, 9);
            this.lblCat.Text = "Category:";
            this._cboCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._cboCategoryFilter.Location = new System.Drawing.Point(62, 5);
            this._cboCategoryFilter.Size = new System.Drawing.Size(114, 25);
            this.pnlCatFilter.Controls.Add(this._cboCategoryFilter);
            this.pnlCatFilter.Controls.Add(this.lblCat);

            this._chkShowArchived.Text = "Archived";
            this._chkShowArchived.AutoSize = true;
            this._chkShowArchived.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this._chkShowArchived.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this._chkShowArchived.Margin = new System.Windows.Forms.Padding(2, 8, 10, 0);

            this._btnAddIngredient.Text = "Add Ingredient";
            this._btnAddIngredient.IconName = "add";
            this._btnAddIngredient.Size = new System.Drawing.Size(134, 34);
            this._btnAddIngredient.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._btnAddIngredient.BorderRadius = 7;
            this._btnAddIngredient.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.Primary;
            this._btnAddIngredient.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryLight;

            this._btnRestock.Text = "Restock";
            this._btnRestock.IconName = "restock";
            this._btnRestock.Size = new System.Drawing.Size(102, 34);
            this._btnRestock.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._btnRestock.BorderRadius = 7;
            this._btnRestock.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCompleted;
            this._btnRestock.HoverColor = System.Drawing.Color.FromArgb(22, 163, 74);

            this._btnEdit.Text = "Edit";
            this._btnEdit.IconName = "edit";
            this._btnEdit.Size = new System.Drawing.Size(80, 34);
            this._btnEdit.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._btnEdit.BorderRadius = 7;
            this._btnEdit.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnEdit.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnEdit.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;

            this._btnArchiveToggle.Text = "Archive";
            this._btnArchiveToggle.IconName = "archive";
            this._btnArchiveToggle.Size = new System.Drawing.Size(96, 34);
            this._btnArchiveToggle.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._btnArchiveToggle.BorderRadius = 7;
            this._btnArchiveToggle.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPendingBg;
            this._btnArchiveToggle.HoverColor = System.Drawing.Color.FromArgb(253, 230, 138);
            this._btnArchiveToggle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPending;

            this._btnDelete.Text = "Delete";
            this._btnDelete.IconName = "delete";
            this._btnDelete.Size = new System.Drawing.Size(88, 34);
            this._btnDelete.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._btnDelete.BorderRadius = 7;
            this._btnDelete.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCancelledBg;
            this._btnDelete.HoverColor = System.Drawing.Color.FromArgb(254, 202, 202);
            this._btnDelete.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCancelled;

            this._btnVoidTransaction.Text = "Void Transaction";
            this._btnVoidTransaction.IconName = "delete";
            this._btnVoidTransaction.Size = new System.Drawing.Size(148, 34);
            this._btnVoidTransaction.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._btnVoidTransaction.BorderRadius = 7;
            this._btnVoidTransaction.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCancelledBg;
            this._btnVoidTransaction.HoverColor = System.Drawing.Color.FromArgb(254, 202, 202);
            this._btnVoidTransaction.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCancelled;
            this._btnVoidTransaction.Visible = false;

            this._btnRefreshTx.Text = "Refresh";
            this._btnRefreshTx.IconName = "refresh";
            this._btnRefreshTx.Size = new System.Drawing.Size(98, 34);
            this._btnRefreshTx.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._btnRefreshTx.BorderRadius = 7;
            this._btnRefreshTx.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnRefreshTx.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnRefreshTx.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._btnRefreshTx.Visible = false;

            this.flpActions.Controls.Add(this.pnlSearch);
            this.flpActions.Controls.Add(this.pnlCatFilter);
            this.flpActions.Controls.Add(this._chkShowArchived);
            this.flpActions.Controls.Add(this._btnAddIngredient);
            this.flpActions.Controls.Add(this._btnRestock);
            this.flpActions.Controls.Add(this._btnEdit);
            this.flpActions.Controls.Add(this._btnArchiveToggle);
            this.flpActions.Controls.Add(this._btnDelete);
            this.flpActions.Controls.Add(this._btnVoidTransaction);
            this.flpActions.Controls.Add(this._btnRefreshTx);

            // Grid Container
            this.pnlGridContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridContainer.BorderRadius = 12;
            this.pnlGridContainer.Padding = new System.Windows.Forms.Padding(12);
            this._dgvStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvTransactions.Visible = false;

            this.pnlGridContainer.Controls.Add(this._dgvStock);
            this.pnlGridContainer.Controls.Add(this._dgvTransactions);

            // Bottom Save Panel
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Height = 56;
            this._bottomPanel.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this._bottomPanel.Visible = false;

            this._btnSaveChanges.Text = "Save Changes";
            this._btnSaveChanges.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnSaveChanges.Size = new System.Drawing.Size(160, 42);
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
            this.Controls.Add(this.pnlGridContainer);
            this.Controls.Add(this._bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "AdminStockControl";
            this.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);
            this.Size = new System.Drawing.Size(1400, 711);

            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.pnlTopHeaderRow.ResumeLayout(false);
            this.pnlTopHeaderRow.PerformLayout();
            this.flpActions.ResumeLayout(false);
            this.flpActions.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlCatFilter.ResumeLayout(false);
            this.pnlCatFilter.PerformLayout();
            this.pnlGridContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgvStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dgvTransactions)).EndInit();
            this._bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel pnlTopHeaderRow;
        private System.Windows.Forms.Label title;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnTabInventory;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnTabTransactions;
        private System.Windows.Forms.Label _lblPendingChangesBanner;
        private System.Windows.Forms.FlowLayoutPanel flpActions;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.Panel pnlCatFilter;
        private System.Windows.Forms.Label lblCat;
        private System.Windows.Forms.ComboBox _cboCategoryFilter;
        private System.Windows.Forms.CheckBox _chkShowArchived;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnAddIngredient;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRestock;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnEdit;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnArchiveToggle;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnDelete;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnVoidTransaction;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefreshTx;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlGridContainer;
        private System.Windows.Forms.DataGridView _dgvStock;
        private System.Windows.Forms.DataGridView _dgvTransactions;
        private System.Windows.Forms.Panel _bottomPanel;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnSaveChanges;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnCancelChanges;
    }
}
