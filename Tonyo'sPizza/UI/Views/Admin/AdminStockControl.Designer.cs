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
            this.flpActions = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.pnlCatFilter = new System.Windows.Forms.Panel();
            this._cboCategoryFilter = new System.Windows.Forms.ComboBox();
            this.lblCat = new System.Windows.Forms.Label();
            this._btnAddIngredient = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnRestock = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnEdit = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnDelete = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._lblPendingChangesBanner = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this._dgvStock = new System.Windows.Forms.DataGridView();
            this._bottomPanel = new System.Windows.Forms.Panel();
            this._btnSaveChanges = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnCancelChanges = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.topPanel.SuspendLayout();
            this.flpActions.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlCatFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvStock)).BeginInit();
            this._bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.flpActions);
            this.topPanel.Controls.Add(this._lblPendingChangesBanner);
            this.topPanel.Controls.Add(this.title);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(24, 24);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1477, 86);
            this.topPanel.TabIndex = 0;
            // 
            // flpActions
            // 
            this.flpActions.Controls.Add(this.pnlSearch);
            this.flpActions.Controls.Add(this.pnlCatFilter);
            this.flpActions.Controls.Add(this._btnAddIngredient);
            this.flpActions.Controls.Add(this._btnRestock);
            this.flpActions.Controls.Add(this._btnEdit);
            this.flpActions.Controls.Add(this._btnDelete);
            this.flpActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpActions.Location = new System.Drawing.Point(0, 52);
            this.flpActions.Name = "flpActions";
            this.flpActions.Size = new System.Drawing.Size(1477, 34);
            this.flpActions.TabIndex = 2;
            this.flpActions.WrapContents = false;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlSearch.BorderRadius = 8;
            this.pnlSearch.BorderWidth = 1;
            this.pnlSearch.Controls.Add(this._txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearchIcon);
            this.pnlSearch.DrawBorder = true;
            this.pnlSearch.GradientAngle = 90F;
            this.pnlSearch.GradientEndColor = System.Drawing.Color.Empty;
            this.pnlSearch.GradientStartColor = System.Drawing.Color.Empty;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(220, 36);
            this.pnlSearch.TabIndex = 0;
            // 
            // _txtSearch
            // 
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtSearch.Location = new System.Drawing.Point(32, 9);
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(180, 17);
            this._txtSearch.TabIndex = 1;
            // 
            // lblSearchIcon
            // 
            this.lblSearchIcon.Location = new System.Drawing.Point(8, 9);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Size = new System.Drawing.Size(18, 18);
            this.lblSearchIcon.TabIndex = 0;
            // 
            // pnlCatFilter
            // 
            this.pnlCatFilter.Controls.Add(this._cboCategoryFilter);
            this.pnlCatFilter.Controls.Add(this.lblCat);
            this.pnlCatFilter.Location = new System.Drawing.Point(230, 0);
            this.pnlCatFilter.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlCatFilter.Name = "pnlCatFilter";
            this.pnlCatFilter.Size = new System.Drawing.Size(185, 36);
            this.pnlCatFilter.TabIndex = 1;
            // 
            // _cboCategoryFilter
            // 
            this._cboCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._cboCategoryFilter.FormattingEnabled = true;
            this._cboCategoryFilter.Location = new System.Drawing.Point(62, 6);
            this._cboCategoryFilter.Name = "_cboCategoryFilter";
            this._cboCategoryFilter.Size = new System.Drawing.Size(120, 25);
            this._cboCategoryFilter.TabIndex = 1;
            // 
            // lblCat
            // 
            this.lblCat.AutoSize = true;
            this.lblCat.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblCat.Location = new System.Drawing.Point(0, 9);
            this.lblCat.Name = "lblCat";
            this.lblCat.Size = new System.Drawing.Size(57, 15);
            this.lblCat.TabIndex = 0;
            this.lblCat.Text = "Category:";
            // 
            // _btnAddIngredient
            // 
            this._btnAddIngredient.BackColor = System.Drawing.Color.Transparent;
            this._btnAddIngredient.BorderRadius = 8;
            this._btnAddIngredient.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnAddIngredient.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnAddIngredient.CustomBorderWidth = 0;
            this._btnAddIngredient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnAddIngredient.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnAddIngredient.ForeColor = System.Drawing.Color.White;
            this._btnAddIngredient.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(115)))), ((int)(((byte)(22)))));
            this._btnAddIngredient.IconName = "add";
            this._btnAddIngredient.Location = new System.Drawing.Point(428, 3);
            this._btnAddIngredient.Name = "_btnAddIngredient";
            this._btnAddIngredient.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(88)))), ((int)(((byte)(12)))));
            this._btnAddIngredient.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnAddIngredient.Size = new System.Drawing.Size(145, 31);
            this._btnAddIngredient.TabIndex = 2;
            this._btnAddIngredient.Text = "Add Ingredient";
            this._btnAddIngredient.UseVisualStyleBackColor = false;
            // 
            // _btnRestock
            // 
            this._btnRestock.BackColor = System.Drawing.Color.Transparent;
            this._btnRestock.BorderRadius = 8;
            this._btnRestock.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnRestock.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnRestock.CustomBorderWidth = 0;
            this._btnRestock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnRestock.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnRestock.ForeColor = System.Drawing.Color.White;
            this._btnRestock.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this._btnRestock.IconName = "restock";
            this._btnRestock.Location = new System.Drawing.Point(579, 3);
            this._btnRestock.Name = "_btnRestock";
            this._btnRestock.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this._btnRestock.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnRestock.Size = new System.Drawing.Size(155, 31);
            this._btnRestock.TabIndex = 3;
            this._btnRestock.Text = "Restock Selected";
            this._btnRestock.UseVisualStyleBackColor = false;
            // 
            // _btnEdit
            // 
            this._btnEdit.BackColor = System.Drawing.Color.Transparent;
            this._btnEdit.BorderRadius = 8;
            this._btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnEdit.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnEdit.CustomBorderWidth = 0;
            this._btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnEdit.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnEdit.ForeColor = System.Drawing.Color.White;
            this._btnEdit.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this._btnEdit.IconName = "edit";
            this._btnEdit.Location = new System.Drawing.Point(740, 3);
            this._btnEdit.Name = "_btnEdit";
            this._btnEdit.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this._btnEdit.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnEdit.Size = new System.Drawing.Size(130, 31);
            this._btnEdit.TabIndex = 4;
            this._btnEdit.Text = "Edit Selected";
            this._btnEdit.UseVisualStyleBackColor = false;
            // 
            // _btnDelete
            // 
            this._btnDelete.BackColor = System.Drawing.Color.Transparent;
            this._btnDelete.BorderRadius = 8;
            this._btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnDelete.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnDelete.CustomBorderWidth = 0;
            this._btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnDelete.ForeColor = System.Drawing.Color.White;
            this._btnDelete.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this._btnDelete.IconName = "delete";
            this._btnDelete.Location = new System.Drawing.Point(876, 3);
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this._btnDelete.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnDelete.Size = new System.Drawing.Size(110, 31);
            this._btnDelete.TabIndex = 5;
            this._btnDelete.Text = "Delete";
            this._btnDelete.UseVisualStyleBackColor = false;
            // 
            // _lblPendingChangesBanner
            // 
            this._lblPendingChangesBanner.AutoSize = true;
            this._lblPendingChangesBanner.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._lblPendingChangesBanner.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this._lblPendingChangesBanner.Location = new System.Drawing.Point(0, 32);
            this._lblPendingChangesBanner.Name = "_lblPendingChangesBanner";
            this._lblPendingChangesBanner.Size = new System.Drawing.Size(478, 17);
            this._lblPendingChangesBanner.TabIndex = 1;
            this._lblPendingChangesBanner.Text = "You have unsaved changes in stock! Click \'Save Changes\' or \'Cancel Changes\'.";
            this._lblPendingChangesBanner.Visible = false;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.title.Location = new System.Drawing.Point(0, 4);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(284, 25);
            this.title.TabIndex = 0;
            this.title.Text = "Ingredient Stock Management";
            this.title.Click += new System.EventHandler(this.title_Click_1);
            // 
            // _dgvStock
            // 
            this._dgvStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvStock.Location = new System.Drawing.Point(24, 110);
            this._dgvStock.MultiSelect = false;
            this._dgvStock.Name = "_dgvStock";
            this._dgvStock.ReadOnly = true;
            this._dgvStock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dgvStock.Size = new System.Drawing.Size(1477, 517);
            this._dgvStock.TabIndex = 1;
            this._dgvStock.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._dgvStock_CellContentClick_1);
            // 
            // _bottomPanel
            // 
            this._bottomPanel.Controls.Add(this._btnSaveChanges);
            this._bottomPanel.Controls.Add(this._btnCancelChanges);
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Location = new System.Drawing.Point(24, 627);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this._bottomPanel.Size = new System.Drawing.Size(1477, 60);
            this._bottomPanel.TabIndex = 2;
            this._bottomPanel.Visible = false;
            // 
            // _btnSaveChanges
            // 
            this._btnSaveChanges.BackColor = System.Drawing.Color.Transparent;
            this._btnSaveChanges.BorderRadius = 8;
            this._btnSaveChanges.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnSaveChanges.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnSaveChanges.CustomBorderWidth = 0;
            this._btnSaveChanges.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnSaveChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnSaveChanges.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._btnSaveChanges.ForeColor = System.Drawing.Color.White;
            this._btnSaveChanges.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(115)))), ((int)(((byte)(22)))));
            this._btnSaveChanges.IconName = null;
            this._btnSaveChanges.Location = new System.Drawing.Point(1297, 12);
            this._btnSaveChanges.Name = "_btnSaveChanges";
            this._btnSaveChanges.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(88)))), ((int)(((byte)(12)))));
            this._btnSaveChanges.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnSaveChanges.Size = new System.Drawing.Size(180, 48);
            this._btnSaveChanges.TabIndex = 0;
            this._btnSaveChanges.Text = "Save Changes";
            this._btnSaveChanges.UseVisualStyleBackColor = false;
            // 
            // _btnCancelChanges
            // 
            this._btnCancelChanges.BackColor = System.Drawing.Color.Transparent;
            this._btnCancelChanges.BorderRadius = 8;
            this._btnCancelChanges.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnCancelChanges.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnCancelChanges.CustomBorderWidth = 0;
            this._btnCancelChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnCancelChanges.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnCancelChanges.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this._btnCancelChanges.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this._btnCancelChanges.IconName = null;
            this._btnCancelChanges.Location = new System.Drawing.Point(1120, 12);
            this._btnCancelChanges.Name = "_btnCancelChanges";
            this._btnCancelChanges.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this._btnCancelChanges.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnCancelChanges.Size = new System.Drawing.Size(160, 48);
            this._btnCancelChanges.TabIndex = 1;
            this._btnCancelChanges.Text = "Cancel Changes";
            this._btnCancelChanges.UseVisualStyleBackColor = false;
            // 
            // AdminStockControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.Controls.Add(this._dgvStock);
            this.Controls.Add(this._bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "AdminStockControl";
            this.Padding = new System.Windows.Forms.Padding(24);
            this.Size = new System.Drawing.Size(1525, 711);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.flpActions.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlCatFilter.ResumeLayout(false);
            this.pnlCatFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvStock)).EndInit();
            this._bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label _lblPendingChangesBanner;
        private System.Windows.Forms.FlowLayoutPanel flpActions;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.Panel pnlCatFilter;
        private System.Windows.Forms.Label lblCat;
        private System.Windows.Forms.ComboBox _cboCategoryFilter;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnAddIngredient;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRestock;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnEdit;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnDelete;
        private System.Windows.Forms.DataGridView _dgvStock;
        private System.Windows.Forms.Panel _bottomPanel;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnSaveChanges;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnCancelChanges;
    }
}
