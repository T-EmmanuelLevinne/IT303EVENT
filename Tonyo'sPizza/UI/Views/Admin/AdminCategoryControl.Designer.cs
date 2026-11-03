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
            this.title = new System.Windows.Forms.Label();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this._btnAdd = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnEdit = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnDelete = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnRefresh = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._dgvCategories = new System.Windows.Forms.DataGridView();
            this.topPanel.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvCategories)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this._btnRefresh);
            this.topPanel.Controls.Add(this._btnDelete);
            this.topPanel.Controls.Add(this._btnEdit);
            this.topPanel.Controls.Add(this._btnAdd);
            this.topPanel.Controls.Add(this.pnlSearch);
            this.topPanel.Controls.Add(this.title);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Height = 84;
            this.topPanel.Location = new System.Drawing.Point(24, 24);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1200, 84);
            this.topPanel.TabIndex = 0;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.title.Location = new System.Drawing.Point(0, 4);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(217, 25);
            this.title.TabIndex = 0;
            this.title.Text = "Category Management";
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
            this.pnlSearch.Location = new System.Drawing.Point(0, 40);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(240, 36);
            this.pnlSearch.TabIndex = 1;
            // 
            // lblSearchIcon
            // 
            this.lblSearchIcon.Location = new System.Drawing.Point(8, 9);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Size = new System.Drawing.Size(18, 18);
            this.lblSearchIcon.TabIndex = 0;
            // 
            // _txtSearch
            // 
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtSearch.Location = new System.Drawing.Point(32, 9);
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(195, 17);
            this._txtSearch.TabIndex = 1;
            // 
            // _btnAdd
            // 
            this._btnAdd.BorderRadius = 8;
            this._btnAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnAdd.ForeColor = System.Drawing.Color.White;
            this._btnAdd.IconName = "add";
            this._btnAdd.Location = new System.Drawing.Point(255, 40);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.NormalColor = System.Drawing.Color.FromArgb(234, 88, 12);
            this._btnAdd.HoverColor = System.Drawing.Color.FromArgb(249, 115, 22);
            this._btnAdd.Size = new System.Drawing.Size(160, 36);
            this._btnAdd.TabIndex = 2;
            this._btnAdd.Text = "Add Category";
            // 
            // _btnEdit
            // 
            this._btnEdit.BorderRadius = 8;
            this._btnEdit.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnEdit.ForeColor = System.Drawing.Color.White;
            this._btnEdit.IconName = "edit";
            this._btnEdit.Location = new System.Drawing.Point(425, 40);
            this._btnEdit.Name = "_btnEdit";
            this._btnEdit.NormalColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this._btnEdit.HoverColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this._btnEdit.Size = new System.Drawing.Size(150, 36);
            this._btnEdit.TabIndex = 3;
            this._btnEdit.Text = "Edit Selected";
            // 
            // _btnDelete
            // 
            this._btnDelete.BorderRadius = 8;
            this._btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnDelete.ForeColor = System.Drawing.Color.White;
            this._btnDelete.IconName = "delete";
            this._btnDelete.Location = new System.Drawing.Point(585, 40);
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.NormalColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this._btnDelete.HoverColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this._btnDelete.Size = new System.Drawing.Size(110, 36);
            this._btnDelete.TabIndex = 4;
            this._btnDelete.Text = "Delete";
            // 
            // _btnRefresh
            // 
            this._btnRefresh.BorderRadius = 8;
            this._btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnRefresh.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._btnRefresh.IconName = "refresh";
            this._btnRefresh.Location = new System.Drawing.Point(705, 40);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NormalColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this._btnRefresh.HoverColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this._btnRefresh.Size = new System.Drawing.Size(120, 36);
            this._btnRefresh.TabIndex = 5;
            this._btnRefresh.Text = "Refresh";
            // 
            // _dgvCategories
            // 
            this._dgvCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvCategories.Location = new System.Drawing.Point(24, 108);
            this._dgvCategories.MultiSelect = false;
            this._dgvCategories.Name = "_dgvCategories";
            this._dgvCategories.ReadOnly = true;
            this._dgvCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dgvCategories.Size = new System.Drawing.Size(1200, 460);
            this._dgvCategories.TabIndex = 1;
            // 
            // AdminCategoryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this._dgvCategories);
            this.Controls.Add(this.topPanel);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "AdminCategoryControl";
            this.Padding = new System.Windows.Forms.Padding(24);
            this.Size = new System.Drawing.Size(1248, 592);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvCategories)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label title;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnAdd;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnEdit;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnDelete;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefresh;
        private System.Windows.Forms.DataGridView _dgvCategories;
    }
}
