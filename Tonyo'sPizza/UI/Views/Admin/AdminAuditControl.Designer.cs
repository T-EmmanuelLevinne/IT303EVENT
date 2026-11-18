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
            this.pnlControls = new System.Windows.Forms.FlowLayoutPanel();
            this._lblCount = new System.Windows.Forms.Label();
            this._btnExport = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnRefresh = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this._dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this._cboUserFilter = new System.Windows.Forms.ComboBox();
            this._cboActionFilter = new System.Windows.Forms.ComboBox();
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

            // topHeader
            this.topHeader.Controls.Add(this.pnlControls);
            this.topHeader.Controls.Add(this.sub);
            this.topHeader.Controls.Add(this.title);
            this.topHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.topHeader.Height = 140;

            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Text = "System Audit Trail";

            this.sub.AutoSize = true;
            this.sub.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.sub.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this.sub.Location = new System.Drawing.Point(2, 32);
            this.sub.Text = "Trace sign-ins, failed attempts, record modifications, stock transactions, and database backups.";

            // Fluid Filter Bar
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Height = 80;
            this.pnlControls.WrapContents = true;
            this.pnlControls.AutoScroll = false;
            this.pnlControls.Padding = new System.Windows.Forms.Padding(0, 4, 0, 2);

            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.BorderRadius = 8;
            this.pnlSearch.Size = new System.Drawing.Size(210, 34);
            this.pnlSearch.Margin = new System.Windows.Forms.Padding(0, 0, 8, 6);

            this.lblSearchIcon.Location = new System.Drawing.Point(9, 9);
            this.lblSearchIcon.Size = new System.Drawing.Size(16, 16);
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._txtSearch.Location = new System.Drawing.Point(30, 8);
            this._txtSearch.Size = new System.Drawing.Size(170, 17);
            this.pnlSearch.Controls.Add(this._txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearchIcon);

            this._cboUserFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboUserFilter.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._cboUserFilter.Size = new System.Drawing.Size(134, 25);
            this._cboUserFilter.Margin = new System.Windows.Forms.Padding(0, 5, 8, 0);

            this._cboActionFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboActionFilter.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._cboActionFilter.Size = new System.Drawing.Size(158, 25);
            this._cboActionFilter.Margin = new System.Windows.Forms.Padding(0, 5, 10, 0);

            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblFrom.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this.lblFrom.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblFrom.Text = "From:";

            this._dtpFrom.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtpFrom.Size = new System.Drawing.Size(102, 24);
            this._dtpFrom.Margin = new System.Windows.Forms.Padding(0, 5, 8, 0);

            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblTo.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this.lblTo.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblTo.Text = "To:";

            this._dtpTo.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtpTo.Size = new System.Drawing.Size(102, 24);
            this._dtpTo.Margin = new System.Windows.Forms.Padding(0, 5, 10, 0);

            this._btnRefresh.Text = "Refresh";
            this._btnRefresh.IconName = "refresh";
            this._btnRefresh.Size = new System.Drawing.Size(98, 34);
            this._btnRefresh.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this._btnRefresh.BorderRadius = 8;
            this._btnRefresh.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnRefresh.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnRefresh.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;

            this._btnExport.Text = "Export CSV";
            this._btnExport.IconName = "export";
            this._btnExport.Size = new System.Drawing.Size(112, 34);
            this._btnExport.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this._btnExport.BorderRadius = 8;
            this._btnExport.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.Primary;
            this._btnExport.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryLight;
            this._btnExport.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryDark;

            this._lblCount.AutoSize = true;
            this._lblCount.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this._lblCount.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this._lblCount.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this._lblCount.Text = "0 events";

            this.pnlControls.Controls.Add(this.pnlSearch);
            this.pnlControls.Controls.Add(this._cboUserFilter);
            this.pnlControls.Controls.Add(this._cboActionFilter);
            this.pnlControls.Controls.Add(this.lblFrom);
            this.pnlControls.Controls.Add(this._dtpFrom);
            this.pnlControls.Controls.Add(this.lblTo);
            this.pnlControls.Controls.Add(this._dtpTo);
            this.pnlControls.Controls.Add(this._btnRefresh);
            this.pnlControls.Controls.Add(this._btnExport);
            this.pnlControls.Controls.Add(this._lblCount);

            // Grid Container Card
            this.pnlGridCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridCard.BorderRadius = 12;
            this.pnlGridCard.Padding = new System.Windows.Forms.Padding(14);
            this.pnlGridCard.Controls.Add(this._dgvLogs);

            this._dgvLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvLogs.ReadOnly = true;

            this.BackColor = Tonyo_sPizza.UI.Theme.AppColors.Background;
            this.Controls.Add(this.pnlGridCard);
            this.Controls.Add(this.topHeader);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "AdminAuditControl";
            this.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);
            this.Size = new System.Drawing.Size(1400, 711);

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
        private System.Windows.Forms.FlowLayoutPanel pnlControls;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private System.Windows.Forms.ComboBox _cboUserFilter;
        private System.Windows.Forms.ComboBox _cboActionFilter;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker _dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker _dtpTo;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefresh;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnExport;
        private System.Windows.Forms.Label _lblCount;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlGridCard;
        private System.Windows.Forms.DataGridView _dgvLogs;
    }
}
