namespace Tonyo_sPizza.UI.Views.Admin
{
    partial class AdminDashboardControl
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

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this._lblGreeting = new System.Windows.Forms.Label();
            this._lblDateSub = new System.Windows.Forms.Label();
            this._btnRefresh = new Tonyo_sPizza.UI.Controls.RoundedButton();

            this.tlpKpis = new System.Windows.Forms.TableLayoutPanel();
            this.cardSales = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblKpiSalesTitle = new System.Windows.Forms.Label();
            this._lblKpiSalesValue = new System.Windows.Forms.Label();
            this._lblKpiSalesSub = new System.Windows.Forms.Label();

            this.cardOrders = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblKpiOrdersTitle = new System.Windows.Forms.Label();
            this._lblKpiOrdersValue = new System.Windows.Forms.Label();
            this._lblKpiOrdersSub = new System.Windows.Forms.Label();

            this.cardLowStock = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblKpiStockTitle = new System.Windows.Forms.Label();
            this._lblKpiStockValue = new System.Windows.Forms.Label();
            this._lblKpiStockSub = new System.Windows.Forms.Label();

            this.cardMenuHealth = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblKpiMenuTitle = new System.Windows.Forms.Label();
            this._lblKpiMenuValue = new System.Windows.Forms.Label();
            this._lblKpiMenuSub = new System.Windows.Forms.Label();

            this.tlpMainSplit = new System.Windows.Forms.TableLayoutPanel();
            this.pnlWatchlistCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.pnlWatchHeader = new System.Windows.Forms.Panel();
            this.lblWatchTitle = new System.Windows.Forms.Label();
            this._btnQuickRestock = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._dgvWatchlist = new System.Windows.Forms.DataGridView();

            this.pnlRightStack = new System.Windows.Forms.Panel();
            this.pnlShortcutsCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.lblShortcutsTitle = new System.Windows.Forms.Label();
            this._flpShortcuts = new System.Windows.Forms.FlowLayoutPanel();

            this.pnlRecentOrdersCard = new Tonyo_sPizza.UI.Controls.RoundedPanel();
            this.pnlRecentHeader = new System.Windows.Forms.Panel();
            this.lblRecentTitle = new System.Windows.Forms.Label();
            this._btnPrintLatestSlip = new Tonyo_sPizza.UI.Controls.RoundedButton();
            this._dgvRecentOrders = new System.Windows.Forms.DataGridView();

            this.SuspendLayout();

            // Header
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 64;

            this._lblGreeting.AutoSize = true;
            this._lblGreeting.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this._lblGreeting.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._lblGreeting.Location = new System.Drawing.Point(0, 2);
            this._lblGreeting.Text = "Kitchen & Store Overview";

            this._lblDateSub.AutoSize = true;
            this._lblDateSub.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this._lblDateSub.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            this._lblDateSub.Location = new System.Drawing.Point(2, 34);
            this._lblDateSub.Text = "Live operational metrics and quick module launch";
            this._lblDateSub.UseMnemonic = false;

            this._btnRefresh.Text = "Refresh Data";
            this._btnRefresh.IconName = "refresh";
            this._btnRefresh.Size = new System.Drawing.Size(134, 36);
            this._btnRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnRefresh.BorderRadius = 8;
            this._btnRefresh.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnRefresh.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnRefresh.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;

            System.Windows.Forms.Panel pnlBtnWrap = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Right,
                Width = 140,
                Padding = new System.Windows.Forms.Padding(0, 10, 0, 14)
            };
            pnlBtnWrap.Controls.Add(this._btnRefresh);

            this.pnlHeader.Controls.Add(this._lblGreeting);
            this.pnlHeader.Controls.Add(this._lblDateSub);
            this.pnlHeader.Controls.Add(pnlBtnWrap);

            // Responsive 4-Column KPI Grid
            this.tlpKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpKpis.Height = 104;
            this.tlpKpis.ColumnCount = 4;
            this.tlpKpis.RowCount = 1;
            this.tlpKpis.Margin = new System.Windows.Forms.Padding(0, 8, 0, 12);
            this.tlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));

            ConfigureKpiCard(this.cardSales, this.lblKpiSalesTitle, this._lblKpiSalesValue, this._lblKpiSalesSub,
                "Today's Gross Sales", "₱0.00", "0 completed orders today", Tonyo_sPizza.UI.Theme.AppColors.Primary, new System.Windows.Forms.Padding(0, 0, 10, 8));

            ConfigureKpiCard(this.cardOrders, this.lblKpiOrdersTitle, this._lblKpiOrdersValue, this._lblKpiOrdersSub,
                "Active Kitchen Queue", "0 orders", "0 pending • 0 preparing", Tonyo_sPizza.UI.Theme.AppColors.StatusPreparing, new System.Windows.Forms.Padding(4, 0, 8, 8));

            ConfigureKpiCard(this.cardLowStock, this.lblKpiStockTitle, this._lblKpiStockValue, this._lblKpiStockSub,
                "Inventory Alerts", "0 items", "All stock levels healthy", Tonyo_sPizza.UI.Theme.AppColors.StatusPending, new System.Windows.Forms.Padding(4, 0, 8, 8));

            ConfigureKpiCard(this.cardMenuHealth, this.lblKpiMenuTitle, this._lblKpiMenuValue, this._lblKpiMenuSub,
                "Menu & Active Vouchers", "0 items", "0 active promos available", Tonyo_sPizza.UI.Theme.AppColors.StatusCompleted, new System.Windows.Forms.Padding(4, 0, 0, 8));

            this.tlpKpis.Controls.Add(this.cardSales, 0, 0);
            this.tlpKpis.Controls.Add(this.cardOrders, 1, 0);
            this.tlpKpis.Controls.Add(this.cardLowStock, 2, 0);
            this.tlpKpis.Controls.Add(this.cardMenuHealth, 3, 0);

            // Responsive 2-Column Main Split
            this.tlpMainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMainSplit.ColumnCount = 2;
            this.tlpMainSplit.RowCount = 1;
            this.tlpMainSplit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46F));
            this.tlpMainSplit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54F));

            // Left: Low Stock Watchlist Card
            this.pnlWatchlistCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWatchlistCard.Margin = new System.Windows.Forms.Padding(0, 4, 10, 0);
            this.pnlWatchlistCard.Padding = new System.Windows.Forms.Padding(16);
            this.pnlWatchlistCard.BorderRadius = 12;

            this.pnlWatchHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWatchHeader.Height = 38;
            this.pnlWatchHeader.BackColor = System.Drawing.Color.White;

            this.lblWatchTitle.Text = "Stock Attention Watchlist";
            this.lblWatchTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblWatchTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.lblWatchTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblWatchTitle.AutoSize = true;

            this._btnQuickRestock.Text = "Restock Selected";
            this._btnQuickRestock.IconName = "restock";
            this._btnQuickRestock.Size = new System.Drawing.Size(148, 32);
            this._btnQuickRestock.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnQuickRestock.BorderRadius = 7;
            this._btnQuickRestock.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.Primary;
            this._btnQuickRestock.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.PrimaryLight;
            this._btnQuickRestock.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);

            this.pnlWatchHeader.Controls.Add(this.lblWatchTitle);
            this.pnlWatchHeader.Controls.Add(this._btnQuickRestock);

            this._dgvWatchlist.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlWatchlistCard.Controls.Add(this._dgvWatchlist);
            this.pnlWatchlistCard.Controls.Add(this.pnlWatchHeader);

            // Right Stack: Module Launchers + Recent Orders
            this.pnlRightStack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRightStack.Margin = new System.Windows.Forms.Padding(4, 4, 0, 0);

            this.pnlShortcutsCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlShortcutsCard.Height = 148;
            this.pnlShortcutsCard.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.pnlShortcutsCard.BorderRadius = 12;

            this.lblShortcutsTitle.Text = "Quick Module Launch";
            this.lblShortcutsTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblShortcutsTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.lblShortcutsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblShortcutsTitle.Height = 26;

            this._flpShortcuts.Dock = System.Windows.Forms.DockStyle.Fill;
            this._flpShortcuts.BackColor = System.Drawing.Color.White;
            this._flpShortcuts.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this._flpShortcuts.AutoScroll = true;

            this.pnlShortcutsCard.Controls.Add(this._flpShortcuts);
            this.pnlShortcutsCard.Controls.Add(this.lblShortcutsTitle);

            System.Windows.Forms.Panel pnlGap = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Top, Height = 12 };

            this.pnlRecentOrdersCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecentOrdersCard.Padding = new System.Windows.Forms.Padding(16);
            this.pnlRecentOrdersCard.BorderRadius = 12;

            this.pnlRecentHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRecentHeader.Height = 38;
            this.pnlRecentHeader.BackColor = System.Drawing.Color.White;

            this.lblRecentTitle.Text = "Recent Store Orders";
            this.lblRecentTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblRecentTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this.lblRecentTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblRecentTitle.AutoSize = true;

            this._btnPrintLatestSlip.Text = "Print Receipt";
            this._btnPrintLatestSlip.IconName = "print";
            this._btnPrintLatestSlip.Size = new System.Drawing.Size(128, 32);
            this._btnPrintLatestSlip.Dock = System.Windows.Forms.DockStyle.Right;
            this._btnPrintLatestSlip.BorderRadius = 7;
            this._btnPrintLatestSlip.NormalColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryNormal;
            this._btnPrintLatestSlip.HoverColor = Tonyo_sPizza.UI.Theme.AppColors.SecondaryHover;
            this._btnPrintLatestSlip.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextDark;
            this._btnPrintLatestSlip.Font = new System.Drawing.Font("Segoe UI Semibold", 8.75F, System.Drawing.FontStyle.Bold);

            this.pnlRecentHeader.Controls.Add(this.lblRecentTitle);
            this.pnlRecentHeader.Controls.Add(this._btnPrintLatestSlip);

            this._dgvRecentOrders.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlRecentOrdersCard.Controls.Add(this._dgvRecentOrders);
            this.pnlRecentOrdersCard.Controls.Add(this.pnlRecentHeader);

            this.pnlRightStack.Controls.Add(this.pnlRecentOrdersCard);
            this.pnlRightStack.Controls.Add(pnlGap);
            this.pnlRightStack.Controls.Add(this.pnlShortcutsCard);

            this.tlpMainSplit.Controls.Add(this.pnlWatchlistCard, 0, 0);
            this.tlpMainSplit.Controls.Add(this.pnlRightStack, 1, 0);

            this.BackColor = Tonyo_sPizza.UI.Theme.AppColors.Background;
            this.Controls.Add(this.tlpMainSplit);
            this.Controls.Add(this.tlpKpis);
            this.Controls.Add(this.pnlHeader);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);
            this.Name = "AdminDashboardControl";
            this.ResumeLayout(false);
        }

        private void ConfigureKpiCard(
            Tonyo_sPizza.UI.Controls.RoundedPanel card,
            System.Windows.Forms.Label lblTitle,
            System.Windows.Forms.Label lblVal,
            System.Windows.Forms.Label lblSub,
            string title,
            string defaultVal,
            string defaultSub,
            System.Drawing.Color valColor,
            System.Windows.Forms.Padding margin)
        {
            card.Dock = System.Windows.Forms.DockStyle.Fill;
            card.Margin = margin;
            card.Padding = new System.Windows.Forms.Padding(16, 12, 14, 10);
            card.BorderRadius = 10;
            card.BackColor = System.Drawing.Color.White;

            lblTitle.Text = title.ToUpperInvariant();
            lblTitle.UseMnemonic = false;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 7.5F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextMuted;
            lblTitle.Location = new System.Drawing.Point(16, 12);
            lblTitle.AutoSize = true;

            lblVal.Text = defaultVal;
            lblVal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblVal.ForeColor = valColor;
            lblVal.Location = new System.Drawing.Point(16, 31);
            lblVal.Size = new System.Drawing.Size(220, 30);
            lblVal.AutoEllipsis = true;
            lblVal.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            lblSub.Text = defaultSub;
            lblSub.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            lblSub.ForeColor = Tonyo_sPizza.UI.Theme.AppColors.TextLight;
            lblSub.Location = new System.Drawing.Point(16, 65);
            lblSub.Size = new System.Drawing.Size(220, 18);
            lblSub.AutoEllipsis = true;
            lblSub.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            card.Controls.Add(lblSub);
            card.Controls.Add(lblVal);
            card.Controls.Add(lblTitle);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label _lblGreeting;
        private System.Windows.Forms.Label _lblDateSub;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnRefresh;

        private System.Windows.Forms.TableLayoutPanel tlpKpis;
        private Tonyo_sPizza.UI.Controls.RoundedPanel cardSales;
        private System.Windows.Forms.Label lblKpiSalesTitle;
        private System.Windows.Forms.Label _lblKpiSalesValue;
        private System.Windows.Forms.Label _lblKpiSalesSub;

        private Tonyo_sPizza.UI.Controls.RoundedPanel cardOrders;
        private System.Windows.Forms.Label lblKpiOrdersTitle;
        private System.Windows.Forms.Label _lblKpiOrdersValue;
        private System.Windows.Forms.Label _lblKpiOrdersSub;

        private Tonyo_sPizza.UI.Controls.RoundedPanel cardLowStock;
        private System.Windows.Forms.Label lblKpiStockTitle;
        private System.Windows.Forms.Label _lblKpiStockValue;
        private System.Windows.Forms.Label _lblKpiStockSub;

        private Tonyo_sPizza.UI.Controls.RoundedPanel cardMenuHealth;
        private System.Windows.Forms.Label lblKpiMenuTitle;
        private System.Windows.Forms.Label _lblKpiMenuValue;
        private System.Windows.Forms.Label _lblKpiMenuSub;

        private System.Windows.Forms.TableLayoutPanel tlpMainSplit;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlWatchlistCard;
        private System.Windows.Forms.Panel pnlWatchHeader;
        private System.Windows.Forms.Label lblWatchTitle;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnQuickRestock;
        private System.Windows.Forms.DataGridView _dgvWatchlist;

        private System.Windows.Forms.Panel pnlRightStack;
        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlShortcutsCard;
        private System.Windows.Forms.Label lblShortcutsTitle;
        private System.Windows.Forms.FlowLayoutPanel _flpShortcuts;

        private Tonyo_sPizza.UI.Controls.RoundedPanel pnlRecentOrdersCard;
        private System.Windows.Forms.Panel pnlRecentHeader;
        private System.Windows.Forms.Label lblRecentTitle;
        private Tonyo_sPizza.UI.Controls.RoundedButton _btnPrintLatestSlip;
        private System.Windows.Forms.DataGridView _dgvRecentOrders;
    }
}
