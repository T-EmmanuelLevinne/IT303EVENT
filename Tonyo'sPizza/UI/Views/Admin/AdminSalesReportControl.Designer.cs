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

            this.tlpKpis = new System.Windows.Forms.TableLayoutPanel();
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
            this.cardBestSeller = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblTitleBestSeller = new System.Windows.Forms.Label();
            this._lblBestSellerKpi = new System.Windows.Forms.Label();
            this._lblBestSellerStockWarning = new System.Windows.Forms.Label();

            this.pnlFilterBar = new System.Windows.Forms.FlowLayoutPanel();
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

            this.tlpMainBody = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeftCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.pnlBestSellerSpotlight = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblSpotlightHeader = new System.Windows.Forms.Label();
            this._lblSpotlightPizza = new System.Windows.Forms.Label();
            this._lblSpotlightStats = new System.Windows.Forms.Label();
            this._pnlSpotlightAlert = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this._lblSpotlightAlert = new System.Windows.Forms.Label();
            this._btnSpotlightRestock = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._btnSpotlightDetails = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this.lblTopTitle = new System.Windows.Forms.Label();
            this._dgvTopItems = new System.Windows.Forms.DataGridView();

            this.pnlRightCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.pnlLedgerHeader = new System.Windows.Forms.Panel();
            this.lblLedgerTitle = new System.Windows.Forms.Label();
            this._btnPrintReceipt = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._lblOrdersCount = new System.Windows.Forms.Label();
            this._dgvOrders = new System.Windows.Forms.DataGridView();

            this.topHeader.SuspendLayout();
            this.tlpKpis.SuspendLayout();
            this.cardRevenue.SuspendLayout();
            this.cardCompleted.SuspendLayout();
            this.cardActive.SuspendLayout();
            this.cardAvg.SuspendLayout();
            this.cardBestSeller.SuspendLayout();
            this.pnlFilterBar.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.tlpMainBody.SuspendLayout();
            this.pnlLeftCard.SuspendLayout();
            this.pnlBestSellerSpotlight.SuspendLayout();
            this._pnlSpotlightAlert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvTopItems)).BeginInit();
            this.pnlRightCard.SuspendLayout();
            this.pnlLedgerHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvOrders)).BeginInit();
            this.SuspendLayout();

            // topHeader
            this.topHeader.Controls.Add(this.sub);
            this.topHeader.Controls.Add(this.title);
            this.topHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.topHeader.Height = 56;
            this.topHeader.TabIndex = 0;

            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Text = "Sales Report & Inventory Intelligence";
            this.title.UseMnemonic = false;

            this.sub.AutoSize = true;
            this.sub.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.sub.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this.sub.Location = new System.Drawing.Point(2, 32);
            this.sub.Text = "Review store turnover, transaction ledger, top sellers, and critical ingredient restock needs.";

            // Responsive 5-Column KPI Grid
            this.tlpKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpKpis.Height = 86;
            this.tlpKpis.ColumnCount = 5;
            this.tlpKpis.RowCount = 1;
            this.tlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19F));
            this.tlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19F));
            this.tlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26F));

            ConfigureSimpleKpi(this.cardRevenue, this.lblTitleRevenue, this._lblGrossRevenue, "TOTAL REVENUE", "₱0.00", Tonyo_sPizza.UI.Theme.AppColors.Primary, new System.Windows.Forms.Padding(0, 4, 8, 6));
            ConfigureSimpleKpi(this.cardCompleted, this.lblTitleCompleted, this._lblCompletedCount, "COMPLETED ORDERS", "0", Tonyo_sPizza.UI.Theme.AppColors.StatusCompleted, new System.Windows.Forms.Padding(2, 4, 8, 6));
            ConfigureSimpleKpi(this.cardActive, this.lblTitleActive, this._lblActiveCount, "ACTIVE ORDERS", "0", Tonyo_sPizza.UI.Theme.AppColors.StatusPending, new System.Windows.Forms.Padding(2, 4, 8, 6));
            ConfigureSimpleKpi(this.cardAvg, this.lblTitleAvg, this._lblAverageOrderValue, "AVG. ORDER VALUE", "₱0.00", Tonyo_sPizza.UI.Theme.AppColors.TextDark, new System.Windows.Forms.Padding(2, 4, 8, 6));

            this.cardBestSeller.Size = new System.Drawing.Size(270, 82);
            this.cardBestSeller.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardBestSeller.Margin = new System.Windows.Forms.Padding(2, 4, 0, 6);
            this.cardBestSeller.BorderRadius = 10;
            this.cardBestSeller.BackColor = System.Drawing.Color.White;

            this.lblTitleBestSeller.AutoSize = true;
            this.lblTitleBestSeller.Font = new System.Drawing.Font("Segoe UI Semibold", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblTitleBestSeller.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this.lblTitleBestSeller.Location = new System.Drawing.Point(14, 10);
            this.lblTitleBestSeller.Text = "BEST SELLER & STOCK NEED";
            this.lblTitleBestSeller.UseMnemonic = false;

            this._lblBestSellerKpi.AutoEllipsis = true;
            this._lblBestSellerKpi.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this._lblBestSellerKpi.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.Primary;
            this._lblBestSellerKpi.Location = new System.Drawing.Point(14, 26);
            this._lblBestSellerKpi.Size = new System.Drawing.Size(240, 22);
            this._lblBestSellerKpi.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this._lblBestSellerKpi.Text = "None";

            this._lblBestSellerStockWarning.AutoEllipsis = true;
            this._lblBestSellerStockWarning.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this._lblBestSellerStockWarning.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this._lblBestSellerStockWarning.Location = new System.Drawing.Point(14, 50);
            this._lblBestSellerStockWarning.Size = new System.Drawing.Size(240, 18);
            this._lblBestSellerStockWarning.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this._lblBestSellerStockWarning.Text = "Checking inventory...";

            this.cardBestSeller.Controls.Add(this._lblBestSellerStockWarning);
            this.cardBestSeller.Controls.Add(this._lblBestSellerKpi);
            this.cardBestSeller.Controls.Add(this.lblTitleBestSeller);

            this.tlpKpis.Controls.Add(this.cardRevenue, 0, 0);
            this.tlpKpis.Controls.Add(this.cardCompleted, 1, 0);
            this.tlpKpis.Controls.Add(this.cardActive, 2, 0);
            this.tlpKpis.Controls.Add(this.cardAvg, 3, 0);
            this.tlpKpis.Controls.Add(this.cardBestSeller, 4, 0);

            // Fluid Filter Bar
            this.pnlFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterBar.AutoSize = true;
            this.pnlFilterBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlFilterBar.WrapContents = true;
            this.pnlFilterBar.AutoScroll = false;
            this.pnlFilterBar.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);

            this.lblPreset.AutoSize = true;
            this.lblPreset.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblPreset.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this.lblPreset.Margin = new System.Windows.Forms.Padding(0, 9, 4, 6);
            this.lblPreset.Text = "Period:";

            this._cboDatePreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboDatePreset.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._cboDatePreset.Size = new System.Drawing.Size(112, 25);
            this._cboDatePreset.Margin = new System.Windows.Forms.Padding(0, 5, 10, 6);

            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblFrom.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this.lblFrom.Margin = new System.Windows.Forms.Padding(0, 9, 4, 0);
            this.lblFrom.Text = "From:";

            this._dtpFrom.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtpFrom.Size = new System.Drawing.Size(106, 24);
            this._dtpFrom.Margin = new System.Windows.Forms.Padding(0, 5, 8, 0);

            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);
            this.lblTo.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMedium;
            this.lblTo.Margin = new System.Windows.Forms.Padding(0, 9, 4, 0);
            this.lblTo.Text = "To:";

            this._dtpTo.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtpTo.Size = new System.Drawing.Size(106, 24);
            this._dtpTo.Margin = new System.Windows.Forms.Padding(0, 5, 10, 0);

            this._cboStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboStatusFilter.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._cboStatusFilter.Size = new System.Drawing.Size(164, 25);
            this._cboStatusFilter.Margin = new System.Windows.Forms.Padding(0, 5, 10, 0);

            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.BorderRadius = 8;
            this.pnlSearch.Size = new System.Drawing.Size(176, 34);
            this.pnlSearch.Margin = new System.Windows.Forms.Padding(0, 1, 10, 0);
            this.lblSearchIcon.Location = new System.Drawing.Point(9, 9);
            this.lblSearchIcon.Size = new System.Drawing.Size(16, 16);
            this._txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._txtSearch.Location = new System.Drawing.Point(30, 8);
            this._txtSearch.Size = new System.Drawing.Size(136, 17);
            this.pnlSearch.Controls.Add(this._txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearchIcon);

            this._btnRefresh.Text = "Refresh";
            this._btnRefresh.IconName = "refresh";
            this._btnRefresh.Size = new System.Drawing.Size(96, 34);
            this._btnRefresh.Margin = new System.Windows.Forms.Padding(0, 1, 8, 0);
            this._btnRefresh.BorderRadius = 8;
            this._btnRefresh.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnRefresh.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnRefresh.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;

            this._btnExport.Text = "Export Report";
            this._btnExport.IconName = "export";
            this._btnExport.Size = new System.Drawing.Size(134, 34);
            this._btnExport.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this._btnExport.BorderRadius = 8;
            this._btnExport.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.Primary;
            this._btnExport.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryLight;
            this._btnExport.PressedColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryDark;

            this.pnlFilterBar.Controls.Add(this.lblPreset);
            this.pnlFilterBar.Controls.Add(this._cboDatePreset);
            this.pnlFilterBar.Controls.Add(this.lblFrom);
            this.pnlFilterBar.Controls.Add(this._dtpFrom);
            this.pnlFilterBar.Controls.Add(this.lblTo);
            this.pnlFilterBar.Controls.Add(this._dtpTo);
            this.pnlFilterBar.Controls.Add(this._cboStatusFilter);
            this.pnlFilterBar.Controls.Add(this.pnlSearch);
            this.pnlFilterBar.Controls.Add(this._btnRefresh);
            this.pnlFilterBar.Controls.Add(this._btnExport);

            // Responsive Main Split Body (36% Left / 64% Right)
            this.tlpMainBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMainBody.ColumnCount = 2;
            this.tlpMainBody.RowCount = 1;
            this.tlpMainBody.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.tlpMainBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.tlpMainBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64F));

            // Left Card
            this.pnlLeftCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeftCard.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlLeftCard.Padding = new System.Windows.Forms.Padding(14);
            this.pnlLeftCard.BorderRadius = 12;

            this.pnlBestSellerSpotlight.BackColor = Tonyo_sPizza.UI.Theme.AppColors.PrimarySoft;
            this.pnlBestSellerSpotlight.BorderColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryBorder;
            this.pnlBestSellerSpotlight.BorderRadius = 10;
            this.pnlBestSellerSpotlight.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBestSellerSpotlight.Padding = new System.Windows.Forms.Padding(12, 8, 12, 10);
            this.pnlBestSellerSpotlight.Height = 124;

            this.lblSpotlightHeader.AutoSize = true;
            this.lblSpotlightHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblSpotlightHeader.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryDark;
            this.lblSpotlightHeader.Location = new System.Drawing.Point(12, 8);
            this.lblSpotlightHeader.Text = "BEST SELLER STOCK BOTTLENECK";

            this._lblSpotlightPizza.AutoEllipsis = true;
            this._lblSpotlightPizza.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this._lblSpotlightPizza.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._lblSpotlightPizza.Location = new System.Drawing.Point(12, 24);
            this._lblSpotlightPizza.Size = new System.Drawing.Size(400, 22);
            this._lblSpotlightPizza.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this._lblSpotlightPizza.Text = "Loading best selling pizza...";

            this._lblSpotlightStats.AutoEllipsis = true;
            this._lblSpotlightStats.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._lblSpotlightStats.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this._lblSpotlightStats.Location = new System.Drawing.Point(12, 48);
            this._lblSpotlightStats.Size = new System.Drawing.Size(400, 18);
            this._lblSpotlightStats.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this._lblSpotlightStats.Text = "Calculating sales metrics...";

            this._pnlSpotlightAlert.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnlSpotlightAlert.Height = 38;
            this._pnlSpotlightAlert.BorderRadius = 8;
            this._pnlSpotlightAlert.DrawBorder = false;
            this._pnlSpotlightAlert.BackColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPendingBg;
            this._pnlSpotlightAlert.Padding = new System.Windows.Forms.Padding(8, 4, 6, 4);

            this._lblSpotlightAlert.AutoEllipsis = true;
            this._lblSpotlightAlert.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this._lblSpotlightAlert.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.StatusPending;
            this._lblSpotlightAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblSpotlightAlert.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._lblSpotlightAlert.Text = "Analyzing ingredient stock...";

            System.Windows.Forms.Panel pnlSpotBtns = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Right,
                Width = 148,
                BackColor = System.Drawing.Color.Transparent
            };

            this._btnSpotlightRestock.Text = "+ Restock";
            this._btnSpotlightRestock.Size = new System.Drawing.Size(80, 28);
            this._btnSpotlightRestock.Location = new System.Drawing.Point(4, 1);
            this._btnSpotlightRestock.BorderRadius = 6;
            this._btnSpotlightRestock.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.StatusCompleted;
            this._btnSpotlightRestock.HoverColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this._btnSpotlightRestock.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);

            this._btnSpotlightDetails.Text = "Details";
            this._btnSpotlightDetails.Size = new System.Drawing.Size(58, 28);
            this._btnSpotlightDetails.Location = new System.Drawing.Point(88, 1);
            this._btnSpotlightDetails.BorderRadius = 6;
            this._btnSpotlightDetails.NormalColor = System.Drawing.Color.White;
            this._btnSpotlightDetails.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SurfaceAlt;
            this._btnSpotlightDetails.CustomBorderColor = Tonyo_sPizza.UI.Theme.AppColors.Border;
            this._btnSpotlightDetails.CustomBorderWidth = 1;
            this._btnSpotlightDetails.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._btnSpotlightDetails.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);

            pnlSpotBtns.Controls.Add(this._btnSpotlightRestock);
            pnlSpotBtns.Controls.Add(this._btnSpotlightDetails);
            this._pnlSpotlightAlert.Controls.Add(this._lblSpotlightAlert);
            this._pnlSpotlightAlert.Controls.Add(pnlSpotBtns);

            this.pnlBestSellerSpotlight.Controls.Add(this._pnlSpotlightAlert);
            this.pnlBestSellerSpotlight.Controls.Add(this._lblSpotlightStats);
            this.pnlBestSellerSpotlight.Controls.Add(this._lblSpotlightPizza);
            this.pnlBestSellerSpotlight.Controls.Add(this.lblSpotlightHeader);

            this.lblTopTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTopTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblTopTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.lblTopTitle.Height = 32;
            this.lblTopTitle.Text = "Top Selling Items & Stock Health";
            this.lblTopTitle.UseMnemonic = false;
            this.lblTopTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblTopTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);

            this._dgvTopItems.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlLeftCard.Controls.Add(this._dgvTopItems);
            this.pnlLeftCard.Controls.Add(this.lblTopTitle);
            this.pnlLeftCard.Controls.Add(this.pnlBestSellerSpotlight);

            // Right Card: Ledger
            this.pnlRightCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRightCard.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.pnlRightCard.Padding = new System.Windows.Forms.Padding(14);
            this.pnlRightCard.BorderRadius = 12;

            this.pnlLedgerHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLedgerHeader.Height = 36;
            this.pnlLedgerHeader.BackColor = System.Drawing.Color.White;

            this.lblLedgerTitle.AutoSize = true;
            this.lblLedgerTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblLedgerTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLedgerTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.lblLedgerTitle.Text = "Order Transactions Ledger";

            this._btnPrintReceipt.Text = "Print Receipt";
            this._btnPrintReceipt.IconName = "print";
            this._btnPrintReceipt.Size = new System.Drawing.Size(124, 30);
            this._btnPrintReceipt.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnPrintReceipt.BorderRadius = 7;
            this._btnPrintReceipt.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnPrintReceipt.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnPrintReceipt.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._btnPrintReceipt.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);

            this._lblOrdersCount.AutoSize = true;
            this._lblOrdersCount.Dock = System.Windows.Forms.DockStyle.Right;
            this._lblOrdersCount.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this._lblOrdersCount.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this._lblOrdersCount.Padding = new System.Windows.Forms.Padding(0, 7, 14, 0);
            this._lblOrdersCount.Text = "0 orders";

            this.pnlLedgerHeader.Controls.Add(this.lblLedgerTitle);
            this.pnlLedgerHeader.Controls.Add(this._lblOrdersCount);
            this.pnlLedgerHeader.Controls.Add(this._btnPrintReceipt);

            this._dgvOrders.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlRightCard.Controls.Add(this._dgvOrders);
            this.pnlRightCard.Controls.Add(this.pnlLedgerHeader);

            this.tlpMainBody.Controls.Add(this.pnlLeftCard, 0, 0);
            this.tlpMainBody.Controls.Add(this.pnlRightCard, 1, 0);

            this.BackColor = Tonyo_sPizza.UI.Theme.AppColors.Background;
            this.Controls.Add(this.tlpMainBody);
            this.Controls.Add(this.pnlFilterBar);
            this.Controls.Add(this.tlpKpis);
            this.Controls.Add(this.topHeader);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "AdminSalesReportControl";
            this.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);
            this.Size = new System.Drawing.Size(1248, 616);

            this.topHeader.ResumeLayout(false);
            this.topHeader.PerformLayout();
            this.tlpKpis.ResumeLayout(false);
            this.cardRevenue.ResumeLayout(false);
            this.cardRevenue.PerformLayout();
            this.cardCompleted.ResumeLayout(false);
            this.cardCompleted.PerformLayout();
            this.cardActive.ResumeLayout(false);
            this.cardActive.PerformLayout();
            this.cardAvg.ResumeLayout(false);
            this.cardAvg.PerformLayout();
            this.cardBestSeller.ResumeLayout(false);
            this.cardBestSeller.PerformLayout();
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlFilterBar.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.tlpMainBody.ResumeLayout(false);
            this.pnlLeftCard.ResumeLayout(false);
            this.pnlBestSellerSpotlight.ResumeLayout(false);
            this.pnlBestSellerSpotlight.PerformLayout();
            this._pnlSpotlightAlert.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._dgvTopItems)).EndInit();
            this.pnlRightCard.ResumeLayout(false);
            this.pnlLedgerHeader.ResumeLayout(false);
            this.pnlLedgerHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dgvOrders)).EndInit();
            this.ResumeLayout(false);
        }

        private void ConfigureSimpleKpi(Tonyo_sPizza.UI.Controls.RoundedPanel card, System.Windows.Forms.Label lblTitle, System.Windows.Forms.Label lblVal, string titleText, string defaultVal, System.Drawing.Color valColor, System.Windows.Forms.Padding margin)
        {
            card.Dock = System.Windows.Forms.DockStyle.Fill;
            card.Margin = margin;
            card.BorderRadius = 10;
            card.BackColor = System.Drawing.Color.White;

            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 7.5F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            lblTitle.Location = new System.Drawing.Point(14, 10);
            lblTitle.Text = titleText;

            lblVal.AutoEllipsis = true;
            lblVal.Font = new System.Drawing.Font("Segoe UI", 14.5F, System.Drawing.FontStyle.Bold);
            lblVal.ForeColor = valColor;
            lblVal.Location = new System.Drawing.Point(14, 30);
            lblVal.Size = new System.Drawing.Size(180, 28);
            lblVal.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblVal.Text = defaultVal;

            card.Controls.Add(lblVal);
            card.Controls.Add(lblTitle);
        }

        #endregion

        private System.Windows.Forms.Panel topHeader;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label sub;
        private System.Windows.Forms.TableLayoutPanel tlpKpis;
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
        private Tonyo_sPizza.UI.Controls.RoundedPanel cardBestSeller;
        private System.Windows.Forms.Label lblTitleBestSeller;
        private System.Windows.Forms.Label _lblBestSellerKpi;
        private System.Windows.Forms.Label _lblBestSellerStockWarning;
        private System.Windows.Forms.FlowLayoutPanel pnlFilterBar;
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
        private System.Windows.Forms.TableLayoutPanel tlpMainBody;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlLeftCard;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlBestSellerSpotlight;
        private System.Windows.Forms.Label lblSpotlightHeader;
        private System.Windows.Forms.Label _lblSpotlightPizza;
        private System.Windows.Forms.Label _lblSpotlightStats;
        private Tonyo_sPizza.UI.Controls.RoundedPanel _pnlSpotlightAlert;
        private System.Windows.Forms.Label _lblSpotlightAlert;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnSpotlightRestock;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnSpotlightDetails;
        private System.Windows.Forms.Label lblTopTitle;
        private System.Windows.Forms.DataGridView _dgvTopItems;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlRightCard;
        private System.Windows.Forms.Panel pnlLedgerHeader;
        private System.Windows.Forms.Label lblLedgerTitle;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnPrintReceipt;
        private System.Windows.Forms.Label _lblOrdersCount;
        private System.Windows.Forms.DataGridView _dgvOrders;
    }
}
