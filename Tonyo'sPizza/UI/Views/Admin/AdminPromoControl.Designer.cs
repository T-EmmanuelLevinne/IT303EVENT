namespace Tonyo_sPizza.UI.Views.Admin
{
    partial class AdminPromoControl
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
            this.sub = new System.Windows.Forms.Label();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this._btnAdd = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnEdit = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnToggle = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnDelete = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnRefresh = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._lblCount = new System.Windows.Forms.Label();
            this.pnlGridCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._dgvPromos = new System.Windows.Forms.DataGridView();
            this.topPanel.SuspendLayout();
            this.pnlControls.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlGridCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvPromos)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.pnlControls);
            this.topPanel.Controls.Add(this.sub);
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
            this.title.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.title.Location = new System.Drawing.Point(0, 4);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(325, 28);
            this.title.TabIndex = 0;
            this.title.Text = "Promo & Voucher Management";
            // 
            // sub
            // 
            this.sub.AutoSize = true;
            this.sub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.sub.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.sub.Location = new System.Drawing.Point(0, 30);
            this.sub.Name = "sub";
            this.sub.Size = new System.Drawing.Size(595, 15);
            this.sub.TabIndex = 1;
            this.sub.Text = "Create discount vouchers, configure percentages, set minimum spend rules & manage promotional campaigns.";
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this._lblCount);
            this.pnlControls.Controls.Add(this._btnRefresh);
            this.pnlControls.Controls.Add(this._btnDelete);
            this.pnlControls.Controls.Add(this._btnToggle);
            this.pnlControls.Controls.Add(this._btnEdit);
            this.pnlControls.Controls.Add(this._btnAdd);
            this.pnlControls.Controls.Add(this.pnlSearch);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Height = 40;
            this.pnlControls.Location = new System.Drawing.Point(0, 50);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(1200, 40);
            this.pnlControls.TabIndex = 2;
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
            this.pnlSearch.Location = new System.Drawing.Point(0, 2);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(240, 36);
            this.pnlSearch.TabIndex = 0;
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
            this._btnAdd.Location = new System.Drawing.Point(255, 2);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.NormalColor = System.Drawing.Color.FromArgb(234, 88, 12);
            this._btnAdd.HoverColor = System.Drawing.Color.FromArgb(249, 115, 22);
            this._btnAdd.Size = new System.Drawing.Size(150, 36);
            this._btnAdd.TabIndex = 1;
            this._btnAdd.Text = "Add Promo";
            // 
            // _btnEdit
            // 
            this._btnEdit.BorderRadius = 8;
            this._btnEdit.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnEdit.ForeColor = System.Drawing.Color.White;
            this._btnEdit.IconName = "edit";
            this._btnEdit.Location = new System.Drawing.Point(415, 2);
            this._btnEdit.Name = "_btnEdit";
            this._btnEdit.NormalColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this._btnEdit.HoverColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this._btnEdit.Size = new System.Drawing.Size(150, 36);
            this._btnEdit.TabIndex = 2;
            this._btnEdit.Text = "Edit Selected";
            // 
            // _btnToggle
            // 
            this._btnToggle.BorderRadius = 8;
            this._btnToggle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnToggle.ForeColor = System.Drawing.Color.White;
            this._btnToggle.IconName = "check";
            this._btnToggle.Location = new System.Drawing.Point(575, 2);
            this._btnToggle.Name = "_btnToggle";
            this._btnToggle.NormalColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this._btnToggle.HoverColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this._btnToggle.Size = new System.Drawing.Size(145, 36);
            this._btnToggle.TabIndex = 3;
            this._btnToggle.Text = "Toggle Active";
            // 
            // _btnDelete
            // 
            this._btnDelete.BorderRadius = 8;
            this._btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnDelete.ForeColor = System.Drawing.Color.White;
            this._btnDelete.IconName = "delete";
            this._btnDelete.Location = new System.Drawing.Point(730, 2);
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
            this._btnRefresh.Location = new System.Drawing.Point(850, 2);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NormalColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this._btnRefresh.HoverColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this._btnRefresh.Size = new System.Drawing.Size(120, 36);
            this._btnRefresh.TabIndex = 5;
            this._btnRefresh.Text = "Refresh";
            // 
            // _lblCount
            // 
            this._lblCount.AutoSize = true;
            this._lblCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblCount.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this._lblCount.Location = new System.Drawing.Point(980, 10);
            this._lblCount.Name = "_lblCount";
            this._lblCount.Size = new System.Drawing.Size(65, 15);
            this._lblCount.TabIndex = 6;
            this._lblCount.Text = "0 vouchers";
            // 
            // pnlGridCard
            // 
            this.pnlGridCard.BackColor = System.Drawing.Color.White;
            this.pnlGridCard.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlGridCard.BorderRadius = 14;
            this.pnlGridCard.BorderWidth = 1;
            this.pnlGridCard.Controls.Add(this._dgvPromos);
            this.pnlGridCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridCard.DrawBorder = true;
            this.pnlGridCard.Location = new System.Drawing.Point(24, 114);
            this.pnlGridCard.Name = "pnlGridCard";
            this.pnlGridCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlGridCard.Size = new System.Drawing.Size(1200, 468);
            this.pnlGridCard.TabIndex = 1;
            // 
            // _dgvPromos
            // 
            this._dgvPromos.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvPromos.Location = new System.Drawing.Point(12, 12);
            this._dgvPromos.MultiSelect = false;
            this._dgvPromos.Name = "_dgvPromos";
            this._dgvPromos.ReadOnly = true;
            this._dgvPromos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dgvPromos.Size = new System.Drawing.Size(1176, 444);
            this._dgvPromos.TabIndex = 0;
            // 
            // AdminPromoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.pnlGridCard);
            this.Controls.Add(this.topPanel);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "AdminPromoControl";
            this.Padding = new System.Windows.Forms.Padding(24);
            this.Size = new System.Drawing.Size(1248, 606);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlGridCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgvPromos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label sub;
        private System.Windows.Forms.Panel pnlControls;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnAdd;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnEdit;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnToggle;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnDelete;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefresh;
        private System.Windows.Forms.Label _lblCount;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlGridCard;
        private System.Windows.Forms.DataGridView _dgvPromos;
    }
}
