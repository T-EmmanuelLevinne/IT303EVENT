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
            this.pnlTopRow = new System.Windows.Forms.Panel();
            this.title = new System.Windows.Forms.Label();
            this.sub = new System.Windows.Forms.Label();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this.pnlControls = new System.Windows.Forms.FlowLayoutPanel();
            this._btnAdd = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnEdit = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnToggle = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnDelete = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnRefresh = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._lblCount = new System.Windows.Forms.Label();
            this.pnlGridCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._dgvPromos = new System.Windows.Forms.DataGridView();

            this.topPanel.SuspendLayout();
            this.pnlTopRow.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlControls.SuspendLayout();
            this.pnlGridCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvPromos)).BeginInit();
            this.SuspendLayout();

            // topPanel
            this.topPanel.Controls.Add(this.pnlControls);
            this.topPanel.Controls.Add(this.pnlTopRow);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Height = 104;
            this.topPanel.Name = "topPanel";

            // pnlTopRow
            this.pnlTopRow.Controls.Add(this.pnlSearch);
            this.pnlTopRow.Controls.Add(this.sub);
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
            this.title.Text = "Promo & Discount Vouchers";
            this.title.UseMnemonic = false;

            // sub
            this.sub.AutoSize = true;
            this.sub.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.sub.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this.sub.Location = new System.Drawing.Point(2, 30);
            this.sub.Name = "sub";
            this.sub.Text = "Configure coupon codes, minimum spend rules, and active campaign statuses.";

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

            // pnlControls
            this.pnlControls.AutoScroll = true;
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Height = 46;
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Padding = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.pnlControls.WrapContents = false;

            // _btnAdd
            this._btnAdd.BorderRadius = 7;
            this._btnAdd.ForeColor = System.Drawing.Color.White;
            this._btnAdd.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryLight;
            this._btnAdd.IconName = "add";
            this._btnAdd.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.Primary;
            this._btnAdd.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryDark;
            this._btnAdd.Size = new System.Drawing.Size(136, 34);
            this._btnAdd.Text = "Add Voucher";

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

            // _btnToggle
            this._btnToggle.BorderRadius = 7;
            this._btnToggle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPending;
            this._btnToggle.HoverColor = System.Drawing.Color.FromArgb(253, 230, 138);
            this._btnToggle.IconName = "check";
            this._btnToggle.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnToggle.Name = "_btnToggle";
            this._btnToggle.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPendingBg;
            this._btnToggle.PressedColor = System.Drawing.Color.FromArgb(252, 211, 77);
            this._btnToggle.Size = new System.Drawing.Size(136, 34);
            this._btnToggle.Text = "Toggle Status";

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
            this._btnRefresh.Margin = new System.Windows.Forms.Padding(0, 0, 14, 0);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnRefresh.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryPressed;
            this._btnRefresh.Size = new System.Drawing.Size(104, 34);
            this._btnRefresh.Text = "Refresh";

            // _lblCount
            this._lblCount.AutoSize = true;
            this._lblCount.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._lblCount.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this._lblCount.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this._lblCount.Name = "_lblCount";
            this._lblCount.Text = "0 voucher(s)";

            this.pnlControls.Controls.Add(this._btnAdd);
            this.pnlControls.Controls.Add(this._btnEdit);
            this.pnlControls.Controls.Add(this._btnToggle);
            this.pnlControls.Controls.Add(this._btnDelete);
            this.pnlControls.Controls.Add(this._btnRefresh);
            this.pnlControls.Controls.Add(this._lblCount);

            // pnlGridCard
            this.pnlGridCard.BackColor = System.Drawing.Color.White;
            this.pnlGridCard.BorderColor = Tonyo_sPizza.UI.Theme.AppColors.Border;
            this.pnlGridCard.BorderRadius = 12;
            this.pnlGridCard.BorderWidth = 1;
            this.pnlGridCard.Controls.Add(this._dgvPromos);
            this.pnlGridCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridCard.DrawBorder = true;
            this.pnlGridCard.Name = "pnlGridCard";
            this.pnlGridCard.Padding = new System.Windows.Forms.Padding(12);

            // _dgvPromos
            this._dgvPromos.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvPromos.MultiSelect = false;
            this._dgvPromos.Name = "_dgvPromos";
            this._dgvPromos.ReadOnly = true;
            this._dgvPromos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // AdminPromoControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Tonyo_sPizza.UI.Theme.AppColors.Background;
            this.Controls.Add(this.pnlGridCard);
            this.Controls.Add(this.topPanel);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "AdminPromoControl";
            this.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);
            this.Size = new System.Drawing.Size(1248, 606);

            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.pnlTopRow.ResumeLayout(false);
            this.pnlTopRow.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.pnlGridCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgvPromos)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel pnlTopRow;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label sub;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.FlowLayoutPanel pnlControls;
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
