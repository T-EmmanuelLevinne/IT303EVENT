namespace Tonyo_sPizza.UI.Views.Admin
{
    partial class AdminMenuControl
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
            this._btnRefresh = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnDelete = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnEdit = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnAdd = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.pnlCat = new System.Windows.Forms.Panel();
            this._cboCategoryFilter = new System.Windows.Forms.ComboBox();
            this.lblCat = new System.Windows.Forms.Label();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this._dgvMenu = new System.Windows.Forms.DataGridView();
            this.topPanel.SuspendLayout();
            this.pnlCat.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this._btnRefresh);
            this.topPanel.Controls.Add(this._btnDelete);
            this.topPanel.Controls.Add(this._btnEdit);
            this.topPanel.Controls.Add(this._btnAdd);
            this.topPanel.Controls.Add(this.pnlCat);
            this.topPanel.Controls.Add(this.pnlSearch);
            this.topPanel.Controls.Add(this.title);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(24, 24);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1477, 84);
            this.topPanel.TabIndex = 0;
            // 
            // _btnRefresh
            // 
            this._btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this._btnRefresh.BorderRadius = 8;
            this._btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnRefresh.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnRefresh.CustomBorderWidth = 0;
            this._btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this._btnRefresh.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this._btnRefresh.IconName = "refresh";
            this._btnRefresh.Location = new System.Drawing.Point(865, 40);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this._btnRefresh.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnRefresh.Size = new System.Drawing.Size(115, 36);
            this._btnRefresh.TabIndex = 6;
            this._btnRefresh.Text = "Refresh";
            this._btnRefresh.UseVisualStyleBackColor = false;
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
            this._btnDelete.Location = new System.Drawing.Point(745, 40);
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this._btnDelete.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnDelete.Size = new System.Drawing.Size(110, 36);
            this._btnDelete.TabIndex = 5;
            this._btnDelete.Text = "Remove";
            this._btnDelete.UseVisualStyleBackColor = false;
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
            this._btnEdit.Location = new System.Drawing.Point(590, 40);
            this._btnEdit.Name = "_btnEdit";
            this._btnEdit.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this._btnEdit.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnEdit.Size = new System.Drawing.Size(145, 36);
            this._btnEdit.TabIndex = 4;
            this._btnEdit.Text = "Edit Selected";
            this._btnEdit.UseVisualStyleBackColor = false;
            // 
            // _btnAdd
            // 
            this._btnAdd.BackColor = System.Drawing.Color.Transparent;
            this._btnAdd.BorderRadius = 8;
            this._btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnAdd.CustomBorderColor = System.Drawing.Color.Transparent;
            this._btnAdd.CustomBorderWidth = 0;
            this._btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnAdd.ForeColor = System.Drawing.Color.White;
            this._btnAdd.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(115)))), ((int)(((byte)(22)))));
            this._btnAdd.IconName = "add";
            this._btnAdd.Location = new System.Drawing.Point(420, 40);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(88)))), ((int)(((byte)(12)))));
            this._btnAdd.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnAdd.Size = new System.Drawing.Size(160, 36);
            this._btnAdd.TabIndex = 3;
            this._btnAdd.Text = "Add Menu Item";
            this._btnAdd.UseVisualStyleBackColor = false;
            // 
            // pnlCat
            // 
            this.pnlCat.Controls.Add(this._cboCategoryFilter);
            this.pnlCat.Controls.Add(this.lblCat);
            this.pnlCat.Location = new System.Drawing.Point(220, 40);
            this.pnlCat.Name = "pnlCat";
            this.pnlCat.Size = new System.Drawing.Size(190, 36);
            this.pnlCat.TabIndex = 2;
            // 
            // _cboCategoryFilter
            // 
            this._cboCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._cboCategoryFilter.FormattingEnabled = true;
            this._cboCategoryFilter.Location = new System.Drawing.Point(62, 6);
            this._cboCategoryFilter.Name = "_cboCategoryFilter";
            this._cboCategoryFilter.Size = new System.Drawing.Size(125, 25);
            this._cboCategoryFilter.TabIndex = 1;
            // 
            // lblCat
            // 
            this.lblCat.AutoSize = true;
            this.lblCat.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblCat.Location = new System.Drawing.Point(0, 10);
            this.lblCat.Name = "lblCat";
            this.lblCat.Size = new System.Drawing.Size(57, 15);
            this.lblCat.TabIndex = 0;
            this.lblCat.Text = "Category:";
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
            this.pnlSearch.Location = new System.Drawing.Point(0, 40);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(210, 36);
            this.pnlSearch.TabIndex = 1;
            // 
            // _txtSearch
            // 
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtSearch.Location = new System.Drawing.Point(30, 9);
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(172, 17);
            this._txtSearch.TabIndex = 1;
            // 
            // lblSearchIcon
            // 
            this.lblSearchIcon.Location = new System.Drawing.Point(8, 9);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Size = new System.Drawing.Size(18, 18);
            this.lblSearchIcon.TabIndex = 0;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.title.Location = new System.Drawing.Point(0, 4);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(240, 25);
            this.title.TabIndex = 0;
            this.title.Text = "Menu Items Management";
            // 
            // _dgvMenu
            // 
            this._dgvMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvMenu.Location = new System.Drawing.Point(24, 108);
            this._dgvMenu.MultiSelect = false;
            this._dgvMenu.Name = "_dgvMenu";
            this._dgvMenu.ReadOnly = true;
            this._dgvMenu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dgvMenu.Size = new System.Drawing.Size(1477, 579);
            this._dgvMenu.TabIndex = 1;
            // 
            // AdminMenuControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.Controls.Add(this._dgvMenu);
            this.Controls.Add(this.topPanel);
            this.Name = "AdminMenuControl";
            this.Padding = new System.Windows.Forms.Padding(24);
            this.Size = new System.Drawing.Size(1525, 711);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.pnlCat.ResumeLayout(false);
            this.pnlCat.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label title;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.Panel pnlCat;
        private System.Windows.Forms.Label lblCat;
        private System.Windows.Forms.ComboBox _cboCategoryFilter;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnAdd;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnEdit;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnDelete;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefresh;
        private System.Windows.Forms.DataGridView _dgvMenu;
    }
}
