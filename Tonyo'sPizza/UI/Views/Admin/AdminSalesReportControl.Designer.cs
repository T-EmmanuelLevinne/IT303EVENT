namespace Tonyo_sPizza.UI.Views.Admin
{
    partial class AdminSalesReportControl
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
            this.title = new System.Windows.Forms.Label();
            this.sub = new System.Windows.Forms.Label();
            this.pnlKpis = new System.Windows.Forms.Panel();
            this.cardRevenue = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblTitleRevenue = new System.Windows.Forms.Label();
            this._lblGrossRevenue = new System.Windows.Forms.Label();
            this.cardCompleted = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblTitleCompleted = new System.Windows.Forms.Label();
            this._lblCompletedCount = new System.Windows.Forms.Label();
            this.cardActive = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblTitleActive = new System.Windows.Forms.Label();
            this._lblActiveCount = new System.Windows.Forms.Label();
            this.cardAvg = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblTitleAvg = new System.Windows.Forms.Label();
            this._lblAverageOrderValue = new System.Windows.Forms.Label();
            this.pnlFilterBar = new System.Windows.Forms.Panel();
            this.lblPreset = new System.Windows.Forms.Label();
            this._cboDatePreset = new System.Windows.Forms.ComboBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this._dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this._dtpTo = new System.Windows.Forms.DateTimePicker();
            this._cboStatusFilter = new System.Windows.Forms.ComboBox();
            this.pnlSearch = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblSearchIcon = new System.Windows.Forms.Label();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this._btnRefresh = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnExport = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.pnlMainBody = new System.Windows.Forms.Panel();
            this.pnlRightCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._dgvOrders = new System.Windows.Forms.DataGridView();
            this.pnlLedgerHeader = new System.Windows.Forms.Panel();
            this.lblLedgerTitle = new System.Windows.Forms.Label();
            this._lblOrdersCount = new System.Windows.Forms.Label();
            this.pnlSpacer = new System.Windows.Forms.Panel();
            this.pnlLeftCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._dgvTopItems = new System.Windows.Forms.DataGridView();
            this.lblTopTitle = new System.Windows.Forms.Label();
            this.topHeader.SuspendLayout();
            this.pnlKpis.SuspendLayout();
            this.cardRevenue.SuspendLayout();
            this.cardCompleted.SuspendLayout();
            this.cardActive.SuspendLayout();
            this.cardAvg.SuspendLayout();
            this.pnlFilterBar.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlMainBody.SuspendLayout();
            this.pnlRightCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvOrders)).BeginInit();
            this.pnlLedgerHeader.SuspendLayout();
            this.pnlLeftCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvTopItems)).BeginInit();
            this.SuspendLayout();
            // 
            // topHeader
            // 
            this.topHeader.Controls.Add(this.sub);
            this.topHeader.Controls.Add(this.title);
            this.topHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.topHeader.Height = 62;
            this.topHeader.Location = new System.Drawing.Point(24, 24);
            this.topHeader.Name = "topHeader";
            this.topHeader.Size = new System.Drawing.Size(1200, 62);
            this.topHeader.TabIndex = 0;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(325, 28);
            this.title.TabIndex = 0;
            this.title.Text = "Sales Report & Revenue Analytics";
            // 
            // sub
            // 
            this.sub.AutoSize = true;
            this.sub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.sub.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.sub.Location = new System.Drawing.Point(0, 28);
            this.sub.Name = "sub";
            this.sub.Size = new System.Drawing.Size(520, 15);
            this.sub.TabIndex = 1;
            this.sub.Text = "Monitor store turnover, customer orders, average ticket metrics, and best-selling pizza items.";
            // 
            // pnlKpis
            // 
            this.pnlKpis.Controls.Add(this.cardAvg);
            this.pnlKpis.Controls.Add(this.cardActive);
            this.pnlKpis.Controls.Add(this.cardCompleted);
            this.pnlKpis.Controls.Add(this.cardRevenue);
            this.pnlKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKpis.Height = 84;
            this.pnlKpis.Location = new System.Drawing.Point(24, 86);
            this.pnlKpis.Name = "pnlKpis";
            this.pnlKpis.Size = new System.Drawing.Size(1200, 84);
            this.pnlKpis.TabIndex = 1;
            // 
            // cardRevenue
            // 
            this.cardRevenue.BackColor = System.Drawing.Color.White;
            this.cardRevenue.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.cardRevenue.BorderRadius = 10;
            this.cardRevenue.BorderWidth = 1;
            this.cardRevenue.Controls.Add(this._lblGrossRevenue);
            this.cardRevenue.Controls.Add(this.lblTitleRevenue);
            this.cardRevenue.DrawBorder = true;
            this.cardRevenue.Location = new System.Drawing.Point(0, 4);
            this.cardRevenue.Name = "cardRevenue";
            this.cardRevenue.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardRevenue.Size = new System.Drawing.Size(215, 72);
            this.cardRevenue.TabIndex = 0;
            // 
            // lblTitleRevenue
            // 
            this.lblTitleRevenue.AutoSize = true;
            this.lblTitleRevenue.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblTitleRevenue.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblTitleRevenue.Location = new System.Drawing.Point(14, 10);
            this.lblTitleRevenue.Name = "lblTitleRevenue";
            this.lblTitleRevenue.Size = new System.Drawing.Size(83, 12);
            this.lblTitleRevenue.TabIndex = 0;
            this.lblTitleRevenue.Text = "TOTAL REVENUE";
            // 
            // _lblGrossRevenue
            // 
            this._lblGrossRevenue.AutoSize = true;
            this._lblGrossRevenue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this._lblGrossRevenue.ForeColor = System.Drawing.Color.FromArgb(194, 65, 12);
            this._lblGrossRevenue.Location = new System.Drawing.Point(14, 28);
            this._lblGrossRevenue.Name = "_lblGrossRevenue";
            this._lblGrossRevenue.Size = new System.Drawing.Size(61, 25);
            this._lblGrossRevenue.TabIndex = 1;
            this._lblGrossRevenue.Text = "₱0.00";
            // 
            // cardCompleted
            // 
            this.cardCompleted.BackColor = System.Drawing.Color.White;
            this.cardCompleted.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.cardCompleted.BorderRadius = 10;
            this.cardCompleted.BorderWidth = 1;
            this.cardCompleted.Controls.Add(this._lblCompletedCount);
            this.cardCompleted.Controls.Add(this.lblTitleCompleted);
            this.cardCompleted.DrawBorder = true;
            this.cardCompleted.Location = new System.Drawing.Point(231, 4);
            this.cardCompleted.Name = "cardCompleted";
            this.cardCompleted.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardCompleted.Size = new System.Drawing.Size(215, 72);
            this.cardCompleted.TabIndex = 1;
            // 
            // lblTitleCompleted
            // 
            this.lblTitleCompleted.AutoSize = true;
            this.lblTitleCompleted.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblTitleCompleted.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblTitleCompleted.Location = new System.Drawing.Point(14, 10);
            this.lblTitleCompleted.Name = "lblTitleCompleted";
            this.lblTitleCompleted.Size = new System.Drawing.Size(107, 12);
            this.lblTitleCompleted.TabIndex = 0;
            this.lblTitleCompleted.Text = "COMPLETED ORDERS";
            // 
            // _lblCompletedCount
            // 
            this._lblCompletedCount.AutoSize = true;
            this._lblCompletedCount.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this._lblCompletedCount.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this._lblCompletedCount.Location = new System.Drawing.Point(14, 28);
            this._lblCompletedCount.Name = "_lblCompletedCount";
            this._lblCompletedCount.Size = new System.Drawing.Size(23, 25);
            this._lblCompletedCount.TabIndex = 1;
            this._lblCompletedCount.Text = "0";
            // 
            // cardActive
            // 
            this.cardActive.BackColor = System.Drawing.Color.White;
            this.cardActive.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.cardActive.BorderRadius = 10;
            this.cardActive.BorderWidth = 1;
            this.cardActive.Controls.Add(this._lblActiveCount);
            this.cardActive.Controls.Add(this.lblTitleActive);
            this.cardActive.DrawBorder = true;
            this.cardActive.Location = new System.Drawing.Point(462, 4);
            this.cardActive.Name = "cardActive";
            this.cardActive.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardActive.Size = new System.Drawing.Size(215, 72);
            this.cardActive.TabIndex = 2;
            // 
            // lblTitleActive
            // 
            this.lblTitleActive.AutoSize = true;
            this.lblTitleActive.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblTitleActive.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblTitleActive.Location = new System.Drawing.Point(14, 10);
            this.lblTitleActive.Name = "lblTitleActive";
            this.lblTitleActive.Size = new System.Drawing.Size(82, 12);
            this.lblTitleActive.TabIndex = 0;
            this.lblTitleActive.Text = "ACTIVE ORDERS";
            // 
            // _lblActiveCount
            // 
            this._lblActiveCount.AutoSize = true;
            this._lblActiveCount.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this._lblActiveCount.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this._lblActiveCount.Location = new System.Drawing.Point(14, 28);
            this._lblActiveCount.Name = "_lblActiveCount";
            this._lblActiveCount.Size = new System.Drawing.Size(23, 25);
            this._lblActiveCount.TabIndex = 1;
            this._lblActiveCount.Text = "0";
            // 
            // cardAvg
            // 
            this.cardAvg.BackColor = System.Drawing.Color.White;
            this.cardAvg.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.cardAvg.BorderRadius = 10;
            this.cardAvg.BorderWidth = 1;
            this.cardAvg.Controls.Add(this._lblAverageOrderValue);
            this.cardAvg.Controls.Add(this.lblTitleAvg);
            this.cardAvg.DrawBorder = true;
            this.cardAvg.Location = new System.Drawing.Point(693, 4);
            this.cardAvg.Name = "cardAvg";
            this.cardAvg.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardAvg.Size = new System.Drawing.Size(215, 72);
            this.cardAvg.TabIndex = 3;
            // 
            // lblTitleAvg
            // 
            this.lblTitleAvg.AutoSize = true;
            this.lblTitleAvg.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblTitleAvg.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblTitleAvg.Location = new System.Drawing.Point(14, 10);
            this.lblTitleAvg.Name = "lblTitleAvg";
            this.lblTitleAvg.Size = new System.Drawing.Size(100, 12);
            this.lblTitleAvg.TabIndex = 0;
            this.lblTitleAvg.Text = "AVG. ORDER VALUE";
            // 
            // _lblAverageOrderValue
            // 
            this._lblAverageOrderValue.AutoSize = true;
            this._lblAverageOrderValue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this._lblAverageOrderValue.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this._lblAverageOrderValue.Location = new System.Drawing.Point(14, 28);
            this._lblAverageOrderValue.Name = "_lblAverageOrderValue";
            this._lblAverageOrderValue.Size = new System.Drawing.Size(61, 25);
            this._lblAverageOrderValue.TabIndex = 1;
            this._lblAverageOrderValue.Text = "₱0.00";
            // 
            // pnlFilterBar
            // 
            this.pnlFilterBar.Controls.Add(this._btnExport);
            this.pnlFilterBar.Controls.Add(this._btnRefresh);
            this.pnlFilterBar.Controls.Add(this.pnlSearch);
            this.pnlFilterBar.Controls.Add(this._cboStatusFilter);
            this.pnlFilterBar.Controls.Add(this._dtpTo);
            this.pnlFilterBar.Controls.Add(this.lblTo);
            this.pnlFilterBar.Controls.Add(this._dtpFrom);
            this.pnlFilterBar.Controls.Add(this.lblFrom);
            this.pnlFilterBar.Controls.Add(this._cboDatePreset);
            this.pnlFilterBar.Controls.Add(this.lblPreset);
            this.pnlFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterBar.Height = 44;
            this.pnlFilterBar.Location = new System.Drawing.Point(24, 170);
            this.pnlFilterBar.Name = "pnlFilterBar";
            this.pnlFilterBar.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.pnlFilterBar.Size = new System.Drawing.Size(1200, 44);
            this.pnlFilterBar.TabIndex = 2;
            // 
            // lblPreset
            // 
            this.lblPreset.AutoSize = true;
            this.lblPreset.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPreset.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblPreset.Location = new System.Drawing.Point(0, 12);
            this.lblPreset.Name = "lblPreset";
            this.lblPreset.Size = new System.Drawing.Size(43, 15);
            this.lblPreset.TabIndex = 0;
            this.lblPreset.Text = "Period:";
            // 
            // _cboDatePreset
            // 
            this._cboDatePreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboDatePreset.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._cboDatePreset.FormattingEnabled = true;
            this._cboDatePreset.Location = new System.Drawing.Point(48, 8);
            this._cboDatePreset.Name = "_cboDatePreset";
            this._cboDatePreset.Size = new System.Drawing.Size(115, 25);
            this._cboDatePreset.TabIndex = 1;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblFrom.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblFrom.Location = new System.Drawing.Point(172, 12);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(37, 15);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "From:";
            // 
            // _dtpFrom
            // 
            this._dtpFrom.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtpFrom.Location = new System.Drawing.Point(212, 8);
            this._dtpFrom.Name = "_dtpFrom";
            this._dtpFrom.Size = new System.Drawing.Size(110, 24);
            this._dtpFrom.TabIndex = 3;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTo.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTo.Location = new System.Drawing.Point(330, 12);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(23, 15);
            this.lblTo.TabIndex = 4;
            this.lblTo.Text = "To:";
            // 
            // _dtpTo
            // 
            this._dtpTo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtpTo.Location = new System.Drawing.Point(356, 8);
            this._dtpTo.Name = "_dtpTo";
            this._dtpTo.Size = new System.Drawing.Size(110, 24);
            this._dtpTo.TabIndex = 5;
            // 
            // _cboStatusFilter
            // 
            this._cboStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboStatusFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._cboStatusFilter.FormattingEnabled = true;
            this._cboStatusFilter.Location = new System.Drawing.Point(474, 8);
            this._cboStatusFilter.Name = "_cboStatusFilter";
            this._cboStatusFilter.Size = new System.Drawing.Size(165, 25);
            this._cboStatusFilter.TabIndex = 6;
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
            this.pnlSearch.Location = new System.Drawing.Point(645, 6);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(155, 34);
            this.pnlSearch.TabIndex = 7;
            // 
            // lblSearchIcon
            // 
            this.lblSearchIcon.Location = new System.Drawing.Point(8, 8);
            this.lblSearchIcon.Name = "lblSearchIcon";
            this.lblSearchIcon.Size = new System.Drawing.Size(16, 16);
            this.lblSearchIcon.TabIndex = 0;
            // 
            // _txtSearch
            // 
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._txtSearch.Location = new System.Drawing.Point(28, 8);
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(120, 16);
            this._txtSearch.TabIndex = 1;
            // 
            // _btnRefresh
            // 
            this._btnRefresh.BorderRadius = 8;
            this._btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnRefresh.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._btnRefresh.IconName = "refresh";
            this._btnRefresh.Location = new System.Drawing.Point(806, 6);
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.NormalColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this._btnRefresh.HoverColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this._btnRefresh.Size = new System.Drawing.Size(95, 34);
            this._btnRefresh.TabIndex = 8;
            this._btnRefresh.Text = "Refresh";
            // 
            // _btnExport
            // 
            this._btnExport.BorderRadius = 8;
            this._btnExport.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this._btnExport.ForeColor = System.Drawing.Color.White;
            this._btnExport.IconName = "orders";
            this._btnExport.Location = new System.Drawing.Point(908, 6);
            this._btnExport.Name = "_btnExport";
            this._btnExport.NormalColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this._btnExport.HoverColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this._btnExport.Size = new System.Drawing.Size(130, 34);
            this._btnExport.TabIndex = 9;
            this._btnExport.Text = "Export Report";
            // 
            // pnlMainBody
            // 
            this.pnlMainBody.Controls.Add(this.pnlRightCard);
            this.pnlMainBody.Controls.Add(this.pnlSpacer);
            this.pnlMainBody.Controls.Add(this.pnlLeftCard);
            this.pnlMainBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainBody.Location = new System.Drawing.Point(24, 214);
            this.pnlMainBody.Name = "pnlMainBody";
            this.pnlMainBody.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlMainBody.Size = new System.Drawing.Size(1200, 378);
            this.pnlMainBody.TabIndex = 3;
            // 
            // pnlRightCard
            // 
            this.pnlRightCard.BackColor = System.Drawing.Color.White;
            this.pnlRightCard.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlRightCard.BorderRadius = 12;
            this.pnlRightCard.BorderWidth = 1;
            this.pnlRightCard.Controls.Add(this._dgvOrders);
            this.pnlRightCard.Controls.Add(this.pnlLedgerHeader);
            this.pnlRightCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRightCard.DrawBorder = true;
            this.pnlRightCard.Location = new System.Drawing.Point(386, 10);
            this.pnlRightCard.Name = "pnlRightCard";
            this.pnlRightCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlRightCard.Size = new System.Drawing.Size(814, 368);
            this.pnlRightCard.TabIndex = 2;
            // 
            // _dgvOrders
            // 
            this._dgvOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvOrders.Location = new System.Drawing.Point(12, 42);
            this._dgvOrders.MultiSelect = false;
            this._dgvOrders.Name = "_dgvOrders";
            this._dgvOrders.ReadOnly = true;
            this._dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dgvOrders.Size = new System.Drawing.Size(790, 314);
            this._dgvOrders.TabIndex = 1;
            // 
            // pnlLedgerHeader
            // 
            this.pnlLedgerHeader.Controls.Add(this._lblOrdersCount);
            this.pnlLedgerHeader.Controls.Add(this.lblLedgerTitle);
            this.pnlLedgerHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLedgerHeader.Height = 30;
            this.pnlLedgerHeader.Location = new System.Drawing.Point(12, 12);
            this.pnlLedgerHeader.Name = "pnlLedgerHeader";
            this.pnlLedgerHeader.Size = new System.Drawing.Size(790, 30);
            this.pnlLedgerHeader.TabIndex = 0;
            // 
            // lblLedgerTitle
            // 
            this.lblLedgerTitle.AutoSize = true;
            this.lblLedgerTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblLedgerTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLedgerTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblLedgerTitle.Location = new System.Drawing.Point(0, 0);
            this.lblLedgerTitle.Name = "lblLedgerTitle";
            this.lblLedgerTitle.Size = new System.Drawing.Size(232, 20);
            this.lblLedgerTitle.TabIndex = 0;
            this.lblLedgerTitle.Text = "📋 Order Transactions Ledger";
            // 
            // _lblOrdersCount
            // 
            this._lblOrdersCount.AutoSize = true;
            this._lblOrdersCount.Dock = System.Windows.Forms.DockStyle.Right;
            this._lblOrdersCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblOrdersCount.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this._lblOrdersCount.Location = new System.Drawing.Point(741, 0);
            this._lblOrdersCount.Name = "_lblOrdersCount";
            this._lblOrdersCount.Size = new System.Drawing.Size(49, 15);
            this._lblOrdersCount.TabIndex = 1;
            this._lblOrdersCount.Text = "0 orders";
            // 
            // pnlSpacer
            // 
            this.pnlSpacer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSpacer.Location = new System.Drawing.Point(370, 10);
            this.pnlSpacer.Name = "pnlSpacer";
            this.pnlSpacer.Size = new System.Drawing.Size(16, 368);
            this.pnlSpacer.TabIndex = 1;
            // 
            // pnlLeftCard
            // 
            this.pnlLeftCard.BackColor = System.Drawing.Color.White;
            this.pnlLeftCard.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlLeftCard.BorderRadius = 12;
            this.pnlLeftCard.BorderWidth = 1;
            this.pnlLeftCard.Controls.Add(this._dgvTopItems);
            this.pnlLeftCard.Controls.Add(this.lblTopTitle);
            this.pnlLeftCard.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeftCard.DrawBorder = true;
            this.pnlLeftCard.Location = new System.Drawing.Point(0, 10);
            this.pnlLeftCard.Name = "pnlLeftCard";
            this.pnlLeftCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlLeftCard.Size = new System.Drawing.Size(370, 368);
            this.pnlLeftCard.TabIndex = 0;
            // 
            // _dgvTopItems
            // 
            this._dgvTopItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dgvTopItems.Location = new System.Drawing.Point(12, 42);
            this._dgvTopItems.MultiSelect = false;
            this._dgvTopItems.Name = "_dgvTopItems";
            this._dgvTopItems.ReadOnly = true;
            this._dgvTopItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dgvTopItems.Size = new System.Drawing.Size(346, 314);
            this._dgvTopItems.TabIndex = 1;
            // 
            // lblTopTitle
            // 
            this.lblTopTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTopTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTopTitle.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblTopTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTopTitle.Name = "lblTopTitle";
            this.lblTopTitle.Size = new System.Drawing.Size(346, 30);
            this.lblTopTitle.TabIndex = 0;
            this.lblTopTitle.Text = "🏆 Best Selling Menu Items";
            // 
            // AdminSalesReportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.pnlMainBody);
            this.Controls.Add(this.pnlFilterBar);
            this.Controls.Add(this.pnlKpis);
            this.Controls.Add(this.topHeader);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "AdminSalesReportControl";
            this.Padding = new System.Windows.Forms.Padding(24);
            this.Size = new System.Drawing.Size(1248, 616);
            this.topHeader.ResumeLayout(false);
            this.topHeader.PerformLayout();
            this.pnlKpis.ResumeLayout(false);
            this.cardRevenue.ResumeLayout(false);
            this.cardRevenue.PerformLayout();
            this.cardCompleted.ResumeLayout(false);
            this.cardCompleted.PerformLayout();
            this.cardActive.ResumeLayout(false);
            this.cardActive.PerformLayout();
            this.cardAvg.ResumeLayout(false);
            this.cardAvg.PerformLayout();
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlFilterBar.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlMainBody.ResumeLayout(false);
            this.pnlRightCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgvOrders)).EndInit();
            this.pnlLedgerHeader.ResumeLayout(false);
            this.pnlLedgerHeader.PerformLayout();
            this.pnlLeftCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgvTopItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topHeader;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label sub;
        private System.Windows.Forms.Panel pnlKpis;
        private Tonyo_sPizza.UI.Controls.RoundedPanel cardRevenue;
        private System.Windows.Forms.Label lblTitleRevenue;
        private System.Windows.Forms.Label _lblGrossRevenue;
        private Tonyo_sPizza.UI.Controls.RoundedPanel cardCompleted;
        private System.Windows.Forms.Label lblTitleCompleted;
        private System.Windows.Forms.Label _lblCompletedCount;
        private Tonyo_sPizza.UI.Controls.RoundedPanel cardActive;
        private System.Windows.Forms.Label lblTitleActive;
        private System.Windows.Forms.Label _lblActiveCount;
        private Tonyo_sPizza.UI.Controls.RoundedPanel cardAvg;
        private System.Windows.Forms.Label lblTitleAvg;
        private System.Windows.Forms.Label _lblAverageOrderValue;
        private System.Windows.Forms.Panel pnlFilterBar;
        private System.Windows.Forms.Label lblPreset;
        private System.Windows.Forms.ComboBox _cboDatePreset;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker _dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker _dtpTo;
        private System.Windows.Forms.ComboBox _cboStatusFilter;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlSearch;
        private System.Windows.Forms.Label lblSearchIcon;
        private System.Windows.Forms.TextBox _txtSearch;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefresh;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnExport;
        private System.Windows.Forms.Panel pnlMainBody;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlLeftCard;
        private System.Windows.Forms.Label lblTopTitle;
        private System.Windows.Forms.DataGridView _dgvTopItems;
        private System.Windows.Forms.Panel pnlSpacer;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlRightCard;
        private System.Windows.Forms.Panel pnlLedgerHeader;
        private System.Windows.Forms.Label lblLedgerTitle;
        private System.Windows.Forms.Label _lblOrdersCount;
        private System.Windows.Forms.DataGridView _dgvOrders;
    }
}
