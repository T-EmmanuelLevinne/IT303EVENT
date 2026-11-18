using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Forms.Dialogs;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminDashboardControl : UserControl
    {
        private readonly OrderRepository _orderRepo;
        private readonly MenuItemRepository _menuRepo;
        private readonly PromoVoucherRepository _promoRepo;

        public event Action<string> NavigateRequested;

        public AdminDashboardControl()
        {
            _orderRepo = new OrderRepository();
            _menuRepo = new MenuItemRepository();
            _promoRepo = new PromoVoucherRepository();

            InitializeComponent();
            SetupCustomUIAndEvents();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                RefreshDashboard();
            }
        }

        private void SetupCustomUIAndEvents()
        {
            UIHelper.EnableDoubleBuffering(this);
            UIHelper.EnableDoubleBuffering(_flpShortcuts);

            _btnRefresh.Click += (s, e) => RefreshDashboard();
            _btnQuickRestock.Click += BtnQuickRestock_Click;
            _btnPrintLatestSlip.Click += BtnPrintLatestSlip_Click;

            // Setup Watchlist Grid
            UIHelper.StyleDataGridView(_dgvWatchlist);
            _dgvWatchlist.Columns.Add("IngredientId", "ID");
            _dgvWatchlist.Columns.Add("Name", "Ingredient");
            _dgvWatchlist.Columns.Add("Stock", "Stock");
            _dgvWatchlist.Columns.Add("Status", "Status");
            _dgvWatchlist.Columns["IngredientId"].Visible = false;
            UIHelper.ConfigureGridColumn(_dgvWatchlist, "Name", 110, fill: true);
            UIHelper.ConfigureGridColumn(_dgvWatchlist, "Stock", 90);
            UIHelper.ConfigureGridColumn(_dgvWatchlist, "Status", 114);
            UIHelper.AttachStatusPillPainter(_dgvWatchlist, UIHelper.ResolveSemanticStatusColors, "Status");
            _dgvWatchlist.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnQuickRestock_Click(s, EventArgs.Empty); };
            _dgvWatchlist.CellMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                {
                    _dgvWatchlist.ClearSelection();
                    _dgvWatchlist.Rows[e.RowIndex].Selected = true;
                    if (_dgvWatchlist.Rows[e.RowIndex].Tag is Ingredient ing)
                    {
                        var menu = UIHelper.CreateModernContextMenu();
                        menu.Items.Add(UIHelper.CreateModernMenuItem($"Restock '{ing.Name}'", "restock", (sender, args) => BtnQuickRestock_Click(this, EventArgs.Empty)));
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(UIHelper.CreateModernMenuItem("Open Stock Tracking", "stock", (sender, args) => NavigateRequested?.Invoke("Stock Tracking")));
                        menu.Show(Cursor.Position);
                    }
                }
            };

            // Setup Recent Orders Grid
            UIHelper.StyleDataGridView(_dgvRecentOrders);
            _dgvRecentOrders.Columns.Add("OrderId", "Order #");
            _dgvRecentOrders.Columns.Add("Customer", "Customer");
            _dgvRecentOrders.Columns.Add("Time", "Time");
            _dgvRecentOrders.Columns.Add("Total", "Total");
            _dgvRecentOrders.Columns.Add("Status", "Status");
            UIHelper.ConfigureGridColumn(_dgvRecentOrders, "OrderId", 86);
            UIHelper.ConfigureGridColumn(_dgvRecentOrders, "Customer", 100, fill: true);
            UIHelper.ConfigureGridColumn(_dgvRecentOrders, "Time", 84);
            UIHelper.ConfigureGridColumn(_dgvRecentOrders, "Total", 86);
            UIHelper.ConfigureGridColumn(_dgvRecentOrders, "Status", 112);
            UIHelper.AttachStatusPillPainter(_dgvRecentOrders, UIHelper.ResolveSemanticStatusColors, "Status");
            _dgvRecentOrders.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnPrintLatestSlip_Click(s, EventArgs.Empty); };
            _dgvRecentOrders.CellMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                {
                    _dgvRecentOrders.ClearSelection();
                    _dgvRecentOrders.Rows[e.RowIndex].Selected = true;
                    if (_dgvRecentOrders.Rows[e.RowIndex].Tag is Order ord)
                    {
                        var menu = UIHelper.CreateModernContextMenu();
                        menu.Items.Add(UIHelper.CreateModernMenuItem($"Print Receipt (#{ord.OrderId})", "print", (sender, args) => BtnPrintLatestSlip_Click(this, EventArgs.Empty)));
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(UIHelper.CreateModernMenuItem("Open Kitchen Queue", "kitchen", (sender, args) => NavigateRequested?.Invoke("Kitchen Queue")));
                        menu.Show(Cursor.Position);
                    }
                }
            };

            BuildModuleShortcuts();
        }

        private void BuildModuleShortcuts()
        {
            _flpShortcuts.Controls.Clear();
            var shortcuts = new[]
            {
                ("Stock & Restock", "Stock Tracking", "stock"),
                ("Food Menu", "Menu Management", "menu"),
                ("Categories", "Categories", "category"),
                ("Kitchen Queue", "Kitchen Queue", "kitchen"),
                ("Sales Report", "Sales Report", "report"),
                ("User Accounts", "User Management", "users"),
                ("Audit Trail", "Audit Logs", "orders"),
                ("Backup & Restore", "Backup & Restore", "backup")
            };

            foreach (var (label, tabKey, icon) in shortcuts)
            {
                var btn = new RoundedButton
                {
                    Text = label,
                    IconName = icon,
                    Size = new Size(154, 40),
                    Margin = new Padding(0, 0, 10, 10),
                    BorderRadius = 8,
                    NormalColor = AppColors.Background,
                    HoverColor = AppColors.PrimarySoft,
                    PressedColor = AppColors.PrimaryBorder,
                    CustomBorderColor = AppColors.Border,
                    CustomBorderWidth = 1,
                    ForeColor = AppColors.TextDark,
                    Font = new Font("Segoe UI Semibold", 8.75f, FontStyle.Bold)
                };
                string target = tabKey;
                btn.Click += (s, e) => NavigateRequested?.Invoke(target);
                _flpShortcuts.Controls.Add(btn);
            }
        }

        public void RefreshDashboard()
        {
            try
            {
                var currentUser = AuthService.Instance?.CurrentUser;
                int hr = DateTime.Now.Hour;
                string timeOfDay = hr < 12 ? "Good morning" : (hr < 18 ? "Good afternoon" : "Good evening");
                string displayName = !string.IsNullOrWhiteSpace(currentUser?.FullName) ? currentUser.FullName : (currentUser?.Username ?? "Admin");
                _lblGreeting.Text = $"{timeOfDay}, {displayName}";
                _lblDateSub.Text = $"{DateTime.Now:dddd, MMMM dd, yyyy}  •  Live Kitchen & Store Summary";

                // 1. Live Orders Metrics
                DateTime today = DateTime.Today;
                List<Order> todayOrders = _orderRepo.GetOrdersForSalesReport(today, today, "All", "");
                decimal todaySales = todayOrders.Where(o => o.Status != "Cancelled").Sum(o => o.TotalAmount);
                int todayCompleted = todayOrders.Count(o => o.Status == "Completed");

                List<Order> allActive = _orderRepo.GetAllOrdersForCrew("All")
                    .Where(o => o.Status == "Pending" || o.Status == "Preparing" || o.Status == "Ready to Serve")
                    .ToList();
                int pendingCnt = allActive.Count(o => o.Status == "Pending");
                int prepCnt = allActive.Count(o => o.Status == "Preparing");
                int readyCnt = allActive.Count(o => o.Status == "Ready to Serve");

                _lblKpiSalesValue.Text = $"₱{todaySales:N2}";
                _lblKpiSalesSub.Text = $"{todayCompleted} completed order(s) today";

                _lblKpiOrdersValue.Text = $"{allActive.Count} active";
                _lblKpiOrdersSub.Text = $"{pendingCnt} pending • {prepCnt} preparing • {readyCnt} ready";

                // 2. Live Stock Metrics
                StockService.Instance.ReloadFromDatabase();
                var stock = StockService.Instance.WorkingStock ?? new List<Ingredient>();
                var outItems = stock.Where(i => i.IsActive && i.IsOutOfStock).ToList();
                var lowItems = stock.Where(i => i.IsActive && !i.IsOutOfStock && i.IsLowStock).ToList();
                var alerts = outItems.Concat(lowItems).ToList();

                _lblKpiStockValue.Text = alerts.Count == 0 ? "All Healthy" : $"{alerts.Count} alert(s)";
                _lblKpiStockValue.ForeColor = outItems.Count > 0 ? AppColors.StatusCancelled : (lowItems.Count > 0 ? AppColors.StatusPending : AppColors.StatusCompleted);
                _lblKpiStockSub.Text = $"{outItems.Count} out of stock • {lowItems.Count} low threshold";

                // 3. Menu & Vouchers
                var menuItems = _menuRepo.GetAllMenuItems();
                int inStockCount = menuItems.Count(m => m.IsAvailable && StockService.Instance.GetAvailableStockForMenuItem(m) > 0);
                var vouchers = _promoRepo.GetAllVouchers().Where(v => v.IsActive).ToList();

                _lblKpiMenuValue.Text = $"{inStockCount} / {menuItems.Count} ready";
                _lblKpiMenuSub.Text = $"{vouchers.Count} active promo voucher(s)";

                // Populate Watchlist Grid (show alerts first, fallback to lowest 8 items)
                _dgvWatchlist.Rows.Clear();
                var displayStock = alerts.Count > 0
                    ? alerts
                    : stock.Where(i => i.IsActive).OrderBy(i => i.CurrentStock).Take(8).ToList();

                foreach (var ing in displayStock)
                {
                    int rowIdx = _dgvWatchlist.Rows.Add(
                        ing.IngredientId,
                        ing.Name,
                        $"{ing.CurrentStock:N1} {ing.Unit}",
                        ing.StockStatusDisplay
                    );
                    _dgvWatchlist.Rows[rowIdx].Tag = ing;
                }

                // Populate Recent Orders Grid
                _dgvRecentOrders.Rows.Clear();
                var recentOrders = _orderRepo.GetOrdersForSalesReport(null, null, "All", "").Take(10).ToList();
                foreach (var ord in recentOrders)
                {
                    int rowIdx = _dgvRecentOrders.Rows.Add(
                        $"#{ord.OrderId}",
                        ord.CustomerName,
                        ord.CreatedAt.ToString("hh:mm tt"),
                        $"₱{ord.TotalAmount:N2}",
                        ord.Status
                    );
                    _dgvRecentOrders.Rows[rowIdx].Tag = ord;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AdminDashboardControl.RefreshDashboard");
                _lblDateSub.Text = "Unable to refresh live metrics. Check database connection.";
            }
        }

        private void BtnQuickRestock_Click(object sender, EventArgs e)
        {
            if (_dgvWatchlist.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Select an ingredient from the watchlist to restock.", "Select Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_dgvWatchlist.SelectedRows[0].Tag is Ingredient ing)
            {
                using (var dlg = new RestockDialogForm(ing))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        RefreshDashboard();
                    }
                }
            }
        }

        private void BtnPrintLatestSlip_Click(object sender, EventArgs e)
        {
            Order target = null;
            if (_dgvRecentOrders.SelectedRows.Count > 0)
            {
                target = _dgvRecentOrders.SelectedRows[0].Tag as Order;
            }
            else if (_dgvRecentOrders.Rows.Count > 0)
            {
                target = _dgvRecentOrders.Rows[0].Tag as Order;
            }

            if (target == null)
            {
                MessageBox.Show(this, "No orders available to preview or print.", "No Order Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new ReceiptPreviewDialog(target))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
