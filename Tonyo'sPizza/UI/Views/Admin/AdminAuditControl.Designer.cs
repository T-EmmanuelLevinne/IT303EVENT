namespace Tonyo_sPizza.UI.Views.Admin
{
    partial class AdminAuditControl
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
            this.topHeader = new System.Windows.Forms.Panel();
            this.pnlControls = new System.Windows.Forms.Panel();
            this._lblCount = new System.Windows.Forms.Label();
            this._btnRefresh = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._cboActionFilter = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this.sub = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.pnlGridCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._dgvLogs = new System.Windows.Forms.DataGridView();
            this.topHeader.SuspendLayout();
            this.pnlControls.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlGridCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // topHeader
            // 
            this.topHeader.Controls.Add(this.pnlControls);
            this.topHeader.Controls.Add(this.sub);
            this.topHeader.Controls.Add(this.title);
            this.topHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.topHeader.Location = new System.Drawing.Point(24, 24);
            this.topHeader.Name = "topHeader";
            this.topHeader.Size = new System.Drawing.Size(1477, 90);
            this.topHeader.TabIndex = 0;
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this._lblCount);
            this.pnlControls.Controls.Add(this._btnRefresh);
            this.pnlControls.Controls.Add(this._cboActionFilter);
            this.pnlControls.Controls.Add(this.lblFilter);
            this.pnlControls.Controls.Add(this.pnlSearch);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(0, 50);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(1477, 40);
            this.pnlControls.TabIndex = 2;
            // 
            // _lblCount
            // 
            this._lblCount.AutoSize = true;
            this._lblCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this._lblCount.Location = new System.Drawing.Point(680, 10);
            this._lblCount.Name = "_lblCount";
            this._lblCount.Size = new System.Drawing.Size(99, 15);
            this._lblCount.TabIndex = 4;
            this._lblCount.Text = "Showing 0 events";
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
            this._btnRefresh.ForeColor = System.Drawing.Color.White;
            this._btnRefresh.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this._btnRefresh.IconName = "refresh";
            this._btnRefresh.Location = new System.Drawing.Point(510, 4);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this._btnRefresh.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(18)))), ((int)(((byte)(57)))));
            this._btnRefresh.Size = new System.Drawing.Size(150, 34);
            this._btnRefresh.TabIndex = 3;
            this._btnRefresh.Text = "Refresh Logs";
            this._btnRefresh.UseVisualStyleBackColor = false;
            // 
            // _cboActionFilter
            // 
            this._cboActionFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboActionFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._cboActionFilter.FormattingEnabled = true;
            this._cboActionFilter.Items.AddRange(new object[] {
            "All Actions",
            "Create Order",
            "Cancel Order",
            "Batch Cancel Orders",
            "Archive Order",
            "Batch Archive Orders",
            "User Management",
            "Delete User",
            "Stock Update",
            "Add Ingredient",
            "Delete Ingredient"});
            this._cboActionFilter.Location = new System.Drawing.Point(325, 6);
            this._cboActionFilter.Name = "_cboActionFilter";
            this._cboActionFilter.Size = new System.Drawing.Size(170, 25);
            this._cboActionFilter.TabIndex = 2;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblFilter.Location = new System.Drawing.Point(280, 10);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(36, 15);
            this.lblFilter.TabIndex = 1;
            this.lblFilter.Text = "Filter:";
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
            this.pnlSearch.Location = new System.Drawing.Point(0, 2);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(260, 34);
            this.pnlSearch.TabIndex = 0;
            // 
            // _txtSearch
            // 
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtSearch.Location = new System.Drawing.Point(32, 7);
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(215, 17);
            this._txtSearch.TabIndex = 1;
            // 
            // lblSearchIcon
            // 
            this.lblSearchIcon.Location = new System.Drawing.Point(8, 7);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Size = new System.Drawing.Size(18, 18);
            this.lblSearchIcon.TabIndex = 0;
            // 
            // sub
            // 
            this.sub.AutoSize = true;
            this.sub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.sub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.sub.Location = new System.Drawing.Point(0, 30);
            this.sub.Name = "sub";
            this.sub.Size = new System.Drawing.Size(477, 15);
            this.sub.TabIndex = 1;
            this.sub.Text = "Comprehensive trace of user activities, inventory adjustments, and order state transitions.";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.title.Location = new System.Drawing.Point(0, 4);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(259, 28);
            this.title.TabIndex = 0;
            this.title.Text = "Administrative Audit Logs";
            // 
            // pnlGridCard
            // 
            this.pnlGridCard.BackColor = System.Drawing.Color.White;
            this.pnlGridCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlGridCard.BorderRadius = 14;
            this.pnlGridCard.BorderWidth = 1;
            this.pnlGridCard.Controls.Add(this._dgvLogs);
            this.pnlGridCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridCard.DrawBorder = true;
            this.pnlGridCard.GradientAngle = 90F;
            this.pnlGridCard.GradientEndColor = System.Drawing.Color.Empty;
            this.pnlGridCard.GradientStartColor = System.Drawing.Color.Empty;
            this.pnlGridCard.Location = new System.Drawing.Point(24, 114);
            this.pnlGridCard.Name = "pnlGridCard";
            this.pnlGridCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlGridCard.Size = new System.Drawing.Size(1477, 573);
            this.pnlGridCard.TabIndex = 1;
            // 
            // _dgvLogs
            // 
            this._dgvLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvLogs.Location = new System.Drawing.Point(12, 12);
            this._dgvLogs.Name = "_dgvLogs";
            this._dgvLogs.ReadOnly = true;
            this._dgvLogs.Size = new System.Drawing.Size(1453, 549);
            this._dgvLogs.TabIndex = 0;
            // 
            // AdminAuditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.pnlGridCard);
            this.Controls.Add(this.topHeader);
            this.Name = "AdminAuditControl";
            this.Padding = new System.Windows.Forms.Padding(24);
            this.Size = new System.Drawing.Size(1525, 711);
            this.topHeader.ResumeLayout(false);
            this.topHeader.PerformLayout();
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlGridCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgvLogs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topHeader;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label sub;
        private System.Windows.Forms.Panel pnlControls;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox _cboActionFilter;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefresh;
        private System.Windows.Forms.Label _lblCount;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlGridCard;
        private System.Windows.Forms.DataGridView _dgvLogs;
    }
}
