using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Forms.Dialogs;
using Tonyo_sPizza.UI.Theme;
using MenuItem = Tonyo_sPizza.Models.MenuItem;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminSalesReportControl : UserControl
    {
        private readonly OrderRepository _orderRepo;
        private readonly MenuItemRepository _menuRepo;
        private ItemBottleneckInfo _currentSpotlightItem;
        private ItemBottleneckInfo _topPizzaBottleneck;

        public class ItemBottleneckInfo
        {
            public MenuItem MenuItem { get; set; }
            public string ItemName { get; set; }
            public int UnitsSold { get; set; }
            public decimal Revenue { get; set; }
            public string MostPopularDoughSize { get; set; }
            public string BottleneckIngredientName { get; set; }
            public string BottleneckRole { get; set; }
            public decimal CurrentStock { get; set; }
            public string Unit { get; set; }
            public int PizzasCapacity { get; set; }
            public string StatusShort { get; set; }
            public string AlertMessage { get; set; }
            public Color AlertBgColor { get; set; }
            public Color AlertFgColor { get; set; }
            public Ingredient BottleneckIngredientObj { get; set; }
        }

        public AdminSalesReportControl()
        {
            _orderRepo = new OrderRepository();
            _menuRepo = new MenuItemRepository();
            InitializeComponent();
            SetupCustomUIAndEvents();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                LoadReportData();
            }
        }

        private void SetupCustomUIAndEvents()
        {
            UIHelper.EnableDoubleBuffering(this);

            if (_cboDatePreset != null)
            {
                _cboDatePreset.Items.Clear();
                _cboDatePreset.Items.AddRange(new object[]
                {
                    "All Time",
                    "Today",
                    "Yesterday",
                    "Last 7 Days",
                    "This Month"
                });
                _cboDatePreset.SelectedIndex = 0;
                _cboDatePreset.SelectedIndexChanged += CboDatePreset_SelectedIndexChanged;
            }

            if (_dtpFrom != null)
            {
                _dtpFrom.Value = new DateTime(2020, 1, 1);
                _dtpFrom.ValueChanged += (s, e) => LoadReportData();
            }
            if (_dtpTo != null)
            {
                _dtpTo.Value = DateTime.Today.AddDays(1);
                _dtpTo.ValueChanged += (s, e) => LoadReportData();
            }

            if (_cboStatusFilter != null)
            {
                _cboStatusFilter.Items.Clear();
                _cboStatusFilter.Items.AddRange(new object[]
                {
                    "Show All Transactions",
                    "Completed",
                    "Ready to Serve",
                    "Preparing",
                    "Pending",
                    "Cancelled"
                });
                _cboStatusFilter.SelectedIndex = 0;
                _cboStatusFilter.SelectedIndexChanged += CboStatusFilter_SelectedIndexChanged;
            }

            if (lblSearchIcon != null)
            {
                lblSearchIcon.Paint += (s, e) =>
                {
                    UIHelper.SetHighQuality(e.Graphics);
                    UIHelper.DrawVectorIcon(e.Graphics, "search", new Rectangle(0, 0, 15, 15), AppColors.TextLight);
                };
            }

            if (_txtSearch != null) _txtSearch.TextChanged += (s, e) => LoadReportData();
            if (_btnRefresh != null) _btnRefresh.Click += (s, e) => LoadReportData();
            if (_btnExport != null) _btnExport.Click += BtnExport_Click;
            if (_btnPrintReceipt != null) _btnPrintReceipt.Click += BtnPrintReceipt_Click;

            if (_dgvTopItems != null)
            {
                UIHelper.StyleDataGridView(_dgvTopItems);
                _dgvTopItems.Columns.Clear();
                _dgvTopItems.Columns.Add("Rank", "#");
                _dgvTopItems.Columns.Add("ItemName", "Item");
                _dgvTopItems.Columns.Add("UnitsSold", "Sold");
                _dgvTopItems.Columns.Add("StockNeed", "Stock Health");

                UIHelper.ConfigureGridColumn(_dgvTopItems, "Rank", 36);
                UIHelper.ConfigureGridColumn(_dgvTopItems, "ItemName", 110, fill: true);
                UIHelper.ConfigureGridColumn(_dgvTopItems, "UnitsSold", 54);
                UIHelper.ConfigureGridColumn(_dgvTopItems, "StockNeed", 136);

                UIHelper.AttachStatusPillPainter(_dgvTopItems, UIHelper.ResolveSemanticStatusColors, "StockNeed");
                _dgvTopItems.CellFormatting += DgvTopItems_CellFormatting;
                _dgvTopItems.SelectionChanged += DgvTopItems_SelectionChanged;
            }

            if (_btnSpotlightRestock != null) _btnSpotlightRestock.Click += BtnSpotlightRestock_Click;
            if (_btnSpotlightDetails != null) _btnSpotlightDetails.Click += BtnSpotlightDetails_Click;

            if (_dgvOrders != null)
            {
                UIHelper.StyleDataGridView(_dgvOrders);
                _dgvOrders.Columns.Clear();
                _dgvOrders.Columns.Add("OrderId", "Order #");
                _dgvOrders.Columns.Add("CreatedAt", "Date & Time");
                _dgvOrders.Columns.Add("Customer", "Customer");
                _dgvOrders.Columns.Add("Items", "Items");
                _dgvOrders.Columns.Add("Status", "Status");
                _dgvOrders.Columns.Add("TotalAmount", "Total (₱)");

                UIHelper.ConfigureGridColumn(_dgvOrders, "OrderId", 84);
                UIHelper.ConfigureGridColumn(_dgvOrders, "CreatedAt", 130);
                UIHelper.ConfigureGridColumn(_dgvOrders, "Customer", 102);
                UIHelper.ConfigureGridColumn(_dgvOrders, "Items", 105, fill: true);
                UIHelper.ConfigureGridColumn(_dgvOrders, "Status", 110);
                UIHelper.ConfigureGridColumn(_dgvOrders, "TotalAmount", 94);

                UIHelper.AttachStatusPillPainter(_dgvOrders, UIHelper.ResolveSemanticStatusColors, "Status");
                _dgvOrders.CellFormatting += DgvOrders_CellFormatting;
                _dgvOrders.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnPrintReceipt_Click(s, EventArgs.Empty); };
                _dgvOrders.CellMouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                    {
                        _dgvOrders.ClearSelection();
                        _dgvOrders.Rows[e.RowIndex].Selected = true;
                        if (_dgvOrders.Rows[e.RowIndex].Tag is Order ord)
                        {
                            var menu = UIHelper.CreateModernContextMenu();
                            var itemPrint = UIHelper.CreateModernMenuItem($"Print Receipt (#{ord.OrderId})", "print", (sender, args) =>
                            {
                                using (var dlg = new ReceiptPreviewDialog(ord))
                                    dlg.ShowDialog(this);
                            });
                            menu.Items.Add(itemPrint);
                            menu.Show(Cursor.Position);
                        }
                    }
                };
            }
        }

        private void BtnPrintReceipt_Click(object sender, EventArgs e)
        {
            if (_dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select an order from the ledger to preview or print its receipt slip.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_dgvOrders.SelectedRows[0].Tag is Order ord)
            {
                using (var dlg = new ReceiptPreviewDialog(ord))
                {
                    dlg.ShowDialog(this);
                }
            }
        }

        private void CboStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string status = _cboStatusFilter.SelectedItem?.ToString() ?? "";
            if (status == "Show All Transactions")
            {
                if (_cboDatePreset != null && _cboDatePreset.SelectedItem?.ToString() != "All Time")
                {
                    _cboDatePreset.SelectedItem = "All Time";
                    return;
                }
            }
            LoadReportData();
        }

        private void CboDatePreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sel = _cboDatePreset.SelectedItem?.ToString() ?? "All Time";
            DateTime today = DateTime.Today;

            switch (sel)
            {
                case "Today":
                    _dtpFrom.Value = today;
                    _dtpTo.Value = today;
                    break;
                case "Yesterday":
                    _dtpFrom.Value = today.AddDays(-1);
                    _dtpTo.Value = today.AddDays(-1);
                    break;
                case "Last 7 Days":
                    _dtpFrom.Value = today.AddDays(-7);
                    _dtpTo.Value = today;
                    break;
                case "This Month":
                    _dtpFrom.Value = new DateTime(today.Year, today.Month, 1);
                    _dtpTo.Value = today;
                    break;
                case "All Time":
                    _dtpFrom.Value = new DateTime(2020, 1, 1);
                    _dtpTo.Value = today.AddDays(1);
                    break;
            }

            LoadReportData();
        }

        private void DgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            if (_dgvOrders.Columns[e.ColumnIndex].Name == "TotalAmount")
            {
                e.CellStyle.Font = new Font("Segoe UI", 9.25f, FontStyle.Bold);
                e.CellStyle.ForeColor = AppColors.TextDark;
            }
        }

        private void DgvTopItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            if (_dgvTopItems.Columns[e.ColumnIndex].Name == "Rank")
            {
                e.CellStyle.Font = new Font("Segoe UI Semibold", 8.75f, FontStyle.Bold);
                e.CellStyle.ForeColor = AppColors.Primary;
            }
        }

        private void DgvTopItems_SelectionChanged(object sender, EventArgs e)
        {
            if (_dgvTopItems.SelectedRows.Count == 0) return;
            var info = _dgvTopItems.SelectedRows[0].Tag as ItemBottleneckInfo;
            if (info != null)
            {
                UpdateSpotlightCard(info, isOverallBestSeller: _dgvTopItems.SelectedRows[0].Index == 0);
            }
        }

        private void UpdateSpotlightCard(ItemBottleneckInfo info, bool isOverallBestSeller)
        {
            if (info == null) return;
            _currentSpotlightItem = info;

            if (lblSpotlightHeader != null)
            {
                lblSpotlightHeader.Text = isOverallBestSeller
                    ? "#1 BEST SELLER STOCK BOTTLENECK"
                    : "SELECTED ITEM STOCK BOTTLENECK";
            }

            if (_lblSpotlightPizza != null) _lblSpotlightPizza.Text = info.ItemName;

            if (_lblSpotlightStats != null)
            {
                string sizeNote = !string.IsNullOrEmpty(info.MostPopularDoughSize) && info.MostPopularDoughSize != "Regular"
                    ? $" • Top crust: {info.MostPopularDoughSize}"
                    : "";
                _lblSpotlightStats.Text = $"{info.UnitsSold} sold (₱{info.Revenue:N2}){sizeNote}";
            }

            if (_pnlSpotlightAlert != null && _lblSpotlightAlert != null)
            {
                _pnlSpotlightAlert.BackColor = info.AlertBgColor;
                _lblSpotlightAlert.ForeColor = info.AlertFgColor;
                _lblSpotlightAlert.Text = info.AlertMessage;
            }

            if (_btnSpotlightRestock != null) _btnSpotlightRestock.Visible = info.BottleneckIngredientObj != null;
            if (_btnSpotlightDetails != null) _btnSpotlightDetails.Visible = info.MenuItem != null;
        }

        private void BtnSpotlightRestock_Click(object sender, EventArgs e)
        {
            if (_currentSpotlightItem?.BottleneckIngredientObj == null)
            {
                MessageBox.Show(this, "No restockable ingredient identified for this item.", "No Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new RestockDialogForm(_currentSpotlightItem.BottleneckIngredientObj))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadReportData();
                }
            }
        }

        private void BtnSpotlightDetails_Click(object sender, EventArgs e)
        {
            if (_currentSpotlightItem?.MenuItem == null)
            {
                MessageBox.Show(this, "No menu item recipe linked.", "No Recipe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new PizzaIngredientsStockDialog(_currentSpotlightItem.MenuItem, () => LoadReportData()))
            {
                dlg.ShowDialog(this);
            }
        }

        private ItemBottleneckInfo AnalyzeItemStockBottleneck(MenuItem menuItem, string itemName, int unitsSold, decimal revenue, string popularSize, List<Ingredient> stockList)
        {
            var info = new ItemBottleneckInfo
            {
                MenuItem = menuItem,
                ItemName = !string.IsNullOrEmpty(itemName) ? itemName : (menuItem?.Name ?? "Unknown Item"),
                UnitsSold = unitsSold,
                Revenue = revenue,
                MostPopularDoughSize = !string.IsNullOrEmpty(popularSize) ? popularSize : "Regular",
                PizzasCapacity = int.MaxValue,
                StatusShort = "Healthy",
                AlertMessage = "All recipe ingredients are adequately stocked.",
                AlertBgColor = AppColors.StatusCompletedBg,
                AlertFgColor = AppColors.StatusCompleted
            };

            if (menuItem == null || stockList == null || stockList.Count == 0)
            {
                info.StatusShort = "No Recipe";
                info.AlertMessage = "No recipe inventory data linked to this item.";
                info.AlertBgColor = AppColors.StatusNeutralBg;
                info.AlertFgColor = AppColors.StatusNeutral;
                return info;
            }

            var candidateBottlenecks = new List<Tuple<Ingredient, string, decimal, int>>();

            if (menuItem.DoughSizes != null && menuItem.DoughSizes.Count > 0)
            {
                foreach (var ds in menuItem.DoughSizes)
                {
                    if (ds.IngredientId.HasValue)
                    {
                        var ing = stockList.FirstOrDefault(i => i.IngredientId == ds.IngredientId.Value);
                        if (ing != null)
                        {
                            decimal portion = ds.DoughPortionQty > 0 ? ds.DoughPortionQty : 1.0m;
                            int cap = (int)Math.Floor(ing.CurrentStock / portion);
                            candidateBottlenecks.Add(Tuple.Create(ing, $"Dough ({ds.SizeName})", portion, Math.Max(0, cap)));
                        }
                    }
                }
            }

            if (menuItem.RecipeIngredients != null && menuItem.RecipeIngredients.Count > 0)
            {
                var grouped = menuItem.RecipeIngredients.GroupBy(r => r.IngredientId);
                foreach (var g in grouped)
                {
                    var ing = stockList.FirstOrDefault(i => i.IngredientId == g.Key);
                    if (ing != null)
                    {
                        decimal portion = g.Max(x => x.QuantityRequired);
                        if (portion <= 0) portion = 1.0m;
                        int cap = (int)Math.Floor(ing.CurrentStock / portion);
                        candidateBottlenecks.Add(Tuple.Create(ing, "Recipe Topping", portion, Math.Max(0, cap)));
                    }
                }
            }

            if (candidateBottlenecks.Count == 0)
            {
                info.StatusShort = "No Recipe";
                info.AlertMessage = "No dough or recipe ingredients linked.";
                info.AlertBgColor = AppColors.StatusNeutralBg;
                info.AlertFgColor = AppColors.StatusNeutral;
                return info;
            }

            var sorted = candidateBottlenecks.OrderBy(b => b.Item4).ThenBy(b => b.Item1.CurrentStock).ToList();
            var bottleneck = sorted.First();

            info.BottleneckIngredientObj = bottleneck.Item1;
            info.BottleneckIngredientName = bottleneck.Item1.Name;
            info.BottleneckRole = bottleneck.Item2;
            info.CurrentStock = bottleneck.Item1.CurrentStock;
            info.Unit = bottleneck.Item1.Unit;
            info.PizzasCapacity = bottleneck.Item4;

            if (info.PizzasCapacity <= 0)
            {
                info.StatusShort = $"{bottleneck.Item1.Name} (0 left)";
                info.AlertMessage = $"Out of Stock: Restock '{bottleneck.Item1.Name}' immediately.";
                info.AlertBgColor = AppColors.StatusCancelledBg;
                info.AlertFgColor = AppColors.StatusCancelled;
            }
            else if (info.PizzasCapacity <= 15 || bottleneck.Item1.CurrentStock <= bottleneck.Item1.MinThreshold)
            {
                info.StatusShort = $"Low: {bottleneck.Item1.Name} ({info.PizzasCapacity})";
                info.AlertMessage = $"Low Stock: '{bottleneck.Item1.Name}' supports only {info.PizzasCapacity} more.";
                info.AlertBgColor = AppColors.StatusPendingBg;
                info.AlertFgColor = AppColors.StatusPending;
            }
            else
            {
                info.StatusShort = $"OK ({info.PizzasCapacity}+ cap)";
                info.AlertMessage = $"Healthy: Bottleneck '{bottleneck.Item1.Name}' ({info.PizzasCapacity} pizzas).";
                info.AlertBgColor = AppColors.StatusCompletedBg;
                info.AlertFgColor = AppColors.StatusCompleted;
            }

            return info;
        }

        public void LoadReportData()
        {
            try
            {
                DateTime fromDate = _dtpFrom.Value.Date;
                DateTime toDate = _dtpTo.Value.Date;
                string status = _cboStatusFilter?.SelectedItem?.ToString() ?? "Show All Transactions";
                string search = _txtSearch?.Text?.Trim() ?? "";

                List<Order> orders = _orderRepo.GetOrdersForSalesReport(fromDate, toDate, status, search);

                decimal grossRevenue = orders.Where(o => o.Status != "Cancelled").Sum(o => o.TotalAmount);
                int completedCount = orders.Count(o => o.Status == "Completed");
                int activeCount = orders.Count(o => o.Status == "Pending" || o.Status == "Preparing" || o.Status == "Ready to Serve");
                decimal avgOrderValue = completedCount > 0 ? grossRevenue / completedCount : (orders.Count > 0 ? grossRevenue / orders.Count : 0m);

                if (_lblGrossRevenue != null) _lblGrossRevenue.Text = $"₱{grossRevenue:N2}";
                if (_lblCompletedCount != null) _lblCompletedCount.Text = $"{completedCount}";
                if (_lblActiveCount != null) _lblActiveCount.Text = $"{activeCount}";
                if (_lblAverageOrderValue != null) _lblAverageOrderValue.Text = $"₱{avgOrderValue:N2}";

                List<MenuItem> allMenuItems = _menuRepo.GetAllMenuItems();
                var menuDict = new Dictionary<string, MenuItem>(StringComparer.OrdinalIgnoreCase);
                foreach (var m in allMenuItems)
                {
                    if (!string.IsNullOrEmpty(m.Name) && !menuDict.ContainsKey(m.Name))
                    {
                        menuDict[m.Name] = m;
                    }
                }

                StockService.Instance.ReloadFromDatabase();
                var stockList = StockService.Instance.WorkingStock ?? new List<Ingredient>();

                var itemSales = orders.Where(o => o.Status != "Cancelled")
                    .SelectMany(o => o.Items ?? new List<OrderItem>())
                    .GroupBy(i => i.ItemName)
                    .Select(g => new
                    {
                        ItemName = g.Key,
                        UnitsSold = g.Sum(x => x.Quantity),
                        TotalRevenue = g.Sum(x => x.Subtotal),
                        TopDoughSize = g.Where(x => !string.IsNullOrEmpty(x.SelectedDoughSize))
                                        .GroupBy(x => x.SelectedDoughSize)
                                        .OrderByDescending(s => s.Count())
                                        .Select(s => s.Key)
                                        .FirstOrDefault() ?? ""
                    })
                    .OrderByDescending(x => x.UnitsSold)
                    .ThenByDescending(x => x.TotalRevenue)
                    .ToList();

                ItemBottleneckInfo topPizzaInfo = null;
                var pizzaSales = itemSales.FirstOrDefault(s =>
                {
                    if (menuDict.TryGetValue(s.ItemName, out var m))
                    {
                        return (m.CategoryName != null && m.CategoryName.IndexOf("pizza", StringComparison.OrdinalIgnoreCase) >= 0)
                            || (m.DoughSizes != null && m.DoughSizes.Count > 0);
                    }
                    return false;
                });

                if (pizzaSales != null && menuDict.TryGetValue(pizzaSales.ItemName, out var topPizzaItem))
                {
                    topPizzaInfo = AnalyzeItemStockBottleneck(topPizzaItem, pizzaSales.ItemName, pizzaSales.UnitsSold, pizzaSales.TotalRevenue, pizzaSales.TopDoughSize, stockList);
                }
                else if (itemSales.Count > 0)
                {
                    var topItemSale = itemSales.First();
                    menuDict.TryGetValue(topItemSale.ItemName, out var topItemObj);
                    topPizzaInfo = AnalyzeItemStockBottleneck(topItemObj, topItemSale.ItemName, topItemSale.UnitsSold, topItemSale.TotalRevenue, topItemSale.TopDoughSize, stockList);
                }
                else
                {
                    var anyPizza = allMenuItems.FirstOrDefault(m =>
                        (m.CategoryName != null && m.CategoryName.IndexOf("pizza", StringComparison.OrdinalIgnoreCase) >= 0)
                        || (m.DoughSizes != null && m.DoughSizes.Count > 0));

                    if (anyPizza != null)
                    {
                        topPizzaInfo = AnalyzeItemStockBottleneck(anyPizza, anyPizza.Name, 0, 0m, anyPizza.DoughSizes?.FirstOrDefault()?.SizeName ?? "Regular", stockList);
                    }
                    else
                    {
                        topPizzaInfo = new ItemBottleneckInfo
                        {
                            ItemName = "No Pizza Orders",
                            UnitsSold = 0,
                            Revenue = 0,
                            StatusShort = "No Data",
                            AlertMessage = "No sales recorded for this period.",
                            AlertBgColor = AppColors.StatusNeutralBg,
                            AlertFgColor = AppColors.StatusNeutral
                        };
                    }
                }

                _topPizzaBottleneck = topPizzaInfo;

                if (_lblBestSellerKpi != null)
                {
                    string soldText = topPizzaInfo.UnitsSold > 0 ? $" ({topPizzaInfo.UnitsSold} sold)" : "";
                    _lblBestSellerKpi.Text = $"{topPizzaInfo.ItemName}{soldText}";
                }

                if (_lblBestSellerStockWarning != null)
                {
                    if (topPizzaInfo.BottleneckIngredientObj != null)
                    {
                        if (topPizzaInfo.PizzasCapacity <= 0)
                        {
                            _lblBestSellerStockWarning.ForeColor = AppColors.StatusCancelled;
                            _lblBestSellerStockWarning.Text = $"Out of Stock: {topPizzaInfo.BottleneckIngredientName}";
                        }
                        else if (topPizzaInfo.PizzasCapacity <= 15)
                        {
                            _lblBestSellerStockWarning.ForeColor = AppColors.StatusPending;
                            _lblBestSellerStockWarning.Text = $"Stock more: {topPizzaInfo.BottleneckIngredientName} ({topPizzaInfo.PizzasCapacity} left)";
                        }
                        else
                        {
                            _lblBestSellerStockWarning.ForeColor = AppColors.StatusCompleted;
                            _lblBestSellerStockWarning.Text = $"Healthy: {topPizzaInfo.BottleneckIngredientName} ({topPizzaInfo.PizzasCapacity} cap)";
                        }
                    }
                    else
                    {
                        _lblBestSellerStockWarning.ForeColor = AppColors.TextMuted;
                        _lblBestSellerStockWarning.Text = "No ingredient shortage";
                    }
                }

                UpdateSpotlightCard(topPizzaInfo, isOverallBestSeller: true);

                _dgvTopItems.Rows.Clear();
                int rank = 1;
                foreach (var item in itemSales.Take(15))
                {
                    menuDict.TryGetValue(item.ItemName, out var itemObj);
                    var bottleneckInfo = AnalyzeItemStockBottleneck(itemObj, item.ItemName, item.UnitsSold, item.TotalRevenue, item.TopDoughSize, stockList);

                    int rowIndex = _dgvTopItems.Rows.Add(
                        $"#{rank++}",
                        item.ItemName,
                        $"{item.UnitsSold}x",
                        bottleneckInfo.StatusShort
                    );
                    _dgvTopItems.Rows[rowIndex].Tag = bottleneckInfo;
                }

                _dgvOrders.Rows.Clear();
                foreach (var o in orders)
                {
                    string itemsSummary = o.Items != null && o.Items.Count > 0
                        ? string.Join(", ", o.Items.Select(i => $"{i.Quantity}x {i.ItemName}" + (!string.IsNullOrEmpty(i.SelectedDoughSize) ? $" [{i.SelectedDoughSize}]" : "")))
                        : "No items";

                    int rowIdx = _dgvOrders.Rows.Add(
                        $"#{o.OrderId}",
                        o.CreatedAt.ToString("yyyy-MM-dd hh:mm tt"),
                        o.CustomerName,
                        itemsSummary,
                        o.Status,
                        $"₱{o.TotalAmount:N2}"
                    );
                    _dgvOrders.Rows[rowIdx].Tag = o;
                }

                if (_lblOrdersCount != null)
                {
                    _lblOrdersCount.Text = $"{orders.Count} transaction(s)";
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Unable to load sales analytics data.", ex, "Report Error", this);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dgvOrders.Rows.Count == 0)
                {
                    MessageBox.Show(this, "There are no transactions shown in the table to export.", "No Transactions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DateTime fromDate = _dtpFrom.Value.Date;
                DateTime toDate = _dtpTo.Value.Date;
                string status = _cboStatusFilter?.SelectedItem?.ToString() ?? "Show All Transactions";
                string search = _txtSearch?.Text?.Trim() ?? "";

                List<Order> orders = _orderRepo.GetOrdersForSalesReport(fromDate, toDate, status, search);

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Title = "Export Transactions & Sales Report";
                    sfd.Filter = "CSV Spreadsheet (*.csv)|*.csv|Text Summary Report (*.txt)|*.txt|All Files (*.*)|*.*";
                    sfd.DefaultExt = "csv";
                    sfd.AddExtension = true;
                    sfd.FileName = $"TonyosPizza_Transactions_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        string ext = Path.GetExtension(sfd.FileName)?.ToLower();
                        if (ext == ".txt")
                        {
                            string reportContent = GenerateTextReport(orders, fromDate, toDate, status, search);
                            File.WriteAllText(sfd.FileName, reportContent, Encoding.UTF8);
                        }
                        else
                        {
                            string csvContent = GenerateCsvReportFromTable();
                            File.WriteAllText(sfd.FileName, csvContent, Encoding.UTF8);
                        }

                        AuditService.Instance.Log("Export Transactions", $"Exported {_dgvOrders.Rows.Count} transaction(s) to {Path.GetFileName(sfd.FileName)}.", "Orders");

                        if (MessageBox.Show(this, $"Transactions exported to:\n{sfd.FileName}\n\nWould you like to open it now?", "Export Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true }); }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Failed to export report file.", ex, "Export Error", this);
            }
        }

        private string GenerateCsvReportFromTable()
        {
            StringBuilder sb = new StringBuilder();

            if (_topPizzaBottleneck != null && _topPizzaBottleneck.BottleneckIngredientObj != null)
            {
                sb.AppendLine($"\"# Best-Selling Pizza:\",\"{EscapeCsv(_topPizzaBottleneck.ItemName)}\",\"Units Sold:\",\"{_topPizzaBottleneck.UnitsSold}\",\"Favorite Size:\",\"{EscapeCsv(_topPizzaBottleneck.MostPopularDoughSize ?? "Regular")}\"");
                sb.AppendLine($"\"# Ingredient Needed to Stock More:\",\"{EscapeCsv(_topPizzaBottleneck.BottleneckIngredientName)}\",\"Current Stock:\",\"{_topPizzaBottleneck.CurrentStock:N1} {_topPizzaBottleneck.Unit}\",\"Pizzas Capacity:\",\"{_topPizzaBottleneck.PizzasCapacity} pizzas remaining\"");
                sb.AppendLine();
            }

            sb.AppendLine("\"Order #\",\"Date & Time\",\"Customer\",\"Items Ordered\",\"Status\",\"Total (PHP)\"");

            foreach (DataGridViewRow row in _dgvOrders.Rows)
            {
                if (row.IsNewRow) continue;

                string orderId = EscapeCsv(row.Cells["OrderId"].Value?.ToString() ?? "");
                string date = EscapeCsv(row.Cells["CreatedAt"].Value?.ToString() ?? "");
                string customer = EscapeCsv(row.Cells["Customer"].Value?.ToString() ?? "");
                string items = EscapeCsv(row.Cells["Items"].Value?.ToString() ?? "");
                string status = EscapeCsv(row.Cells["Status"].Value?.ToString() ?? "");
                string total = EscapeCsv(row.Cells["TotalAmount"].Value?.ToString() ?? "");

                sb.AppendLine($"\"{orderId}\",\"{date}\",\"{customer}\",\"{items}\",\"{status}\",\"{total}\"");
            }

            return sb.ToString();
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Replace("\"", "\"\"");
        }

        private string GenerateTextReport(List<Order> orders, DateTime fromDate, DateTime toDate, string statusFilter, string searchKeyword)
        {
            StringBuilder sb = new StringBuilder();
            string divider = new string('=', 110);
            string subDivider = new string('-', 110);

            decimal grossRevenue = orders.Where(o => o.Status != "Cancelled").Sum(o => o.TotalAmount);
            int completedCount = orders.Count(o => o.Status == "Completed");
            int activeCount = orders.Count(o => o.Status == "Pending" || o.Status == "Preparing" || o.Status == "Ready to Serve");
            int cancelledCount = orders.Count(o => o.Status == "Cancelled");
            decimal avgOrderValue = completedCount > 0 ? grossRevenue / completedCount : (orders.Count > 0 ? grossRevenue / orders.Count : 0m);

            string currentUser = AuthService.Instance.CurrentUser?.FullName ?? "Admin";

            sb.AppendLine(divider);
            sb.AppendLine("                                TONYO'S PIZZA - SALES & REVENUE REPORT");
            sb.AppendLine(divider);
            sb.AppendLine($"Generated On:   {DateTime.Now:yyyy-MM-dd hh:mm tt}");
            sb.AppendLine($"Report Period:  {fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}");
            sb.AppendLine($"Status Filter:  {statusFilter}");
            sb.AppendLine($"Search Query:   {(!string.IsNullOrEmpty(searchKeyword) ? searchKeyword : "(None)")}");
            sb.AppendLine($"Exported By:    {currentUser}");
            sb.AppendLine(subDivider);
            sb.AppendLine();

            sb.AppendLine("[ 1. EXECUTIVE KPI SUMMARY ]");
            sb.AppendLine(subDivider);
            sb.AppendLine($"  • Gross Revenue:          ₱{grossRevenue:N2}");
            sb.AppendLine($"  • Completed Orders:       {completedCount}");
            sb.AppendLine($"  • Active Orders:          {activeCount}");
            sb.AppendLine($"  • Cancelled Orders:       {cancelledCount}");
            sb.AppendLine($"  • Total Transactions:     {orders.Count}");
            sb.AppendLine($"  • Average Order Value:    ₱{avgOrderValue:N2}");
            sb.AppendLine(subDivider);
            sb.AppendLine();

            sb.AppendLine("[ 2. BEST-SELLING PIZZA & INGREDIENT RESTOCK ADVISOR ]");
            sb.AppendLine(subDivider);
            if (_topPizzaBottleneck != null && _topPizzaBottleneck.BottleneckIngredientObj != null)
            {
                sb.AppendLine($"  • #1 Best-Selling Pizza:  {_topPizzaBottleneck.ItemName}");
                sb.AppendLine($"  • Units Sold:             {_topPizzaBottleneck.UnitsSold} units (₱{_topPizzaBottleneck.Revenue:N2})");
                sb.AppendLine($"  • Most Popular Size:      {_topPizzaBottleneck.MostPopularDoughSize ?? "Standard"}");
                sb.AppendLine($"  • Bottleneck Ingredient:  {_topPizzaBottleneck.BottleneckIngredientName} ({_topPizzaBottleneck.BottleneckRole})");
                sb.AppendLine($"  • Live Stock Available:   {_topPizzaBottleneck.CurrentStock:N1} {_topPizzaBottleneck.Unit}");
                sb.AppendLine($"  • Maximum Pizza Capacity: {_topPizzaBottleneck.PizzasCapacity} pizzas remaining");
                sb.AppendLine($"  • Restock Recommendation: {_topPizzaBottleneck.AlertMessage}");
            }
            else
            {
                sb.AppendLine("  (No pizza recipe bottleneck identified for this period)");
            }
            sb.AppendLine(subDivider);
            sb.AppendLine();

            var topItems = orders.Where(o => o.Status != "Cancelled")
                .SelectMany(o => o.Items ?? new List<OrderItem>())
                .GroupBy(i => i.ItemName)
                .Select(g => new
                {
                    ItemName = g.Key,
                    UnitsSold = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Subtotal)
                })
                .OrderByDescending(x => x.UnitsSold)
                .ThenByDescending(x => x.TotalRevenue)
                .ToList();

            sb.AppendLine("[ 3. BEST-SELLING MENU ITEMS ]");
            sb.AppendLine(subDivider);
            sb.AppendLine(string.Format("  {0,-6} {1,-45} {2,-15} {3,-20}", "Rank", "Item Name", "Units Sold", "Total Revenue (₱)"));
            sb.AppendLine("  " + new string('-', 100));

            if (topItems.Count == 0)
            {
                sb.AppendLine("  (No item sales recorded in this period)");
            }
            else
            {
                int rank = 1;
                foreach (var item in topItems)
                {
                    string name = item.ItemName.Length > 43 ? item.ItemName.Substring(0, 40) + "..." : item.ItemName;
                    sb.AppendLine(string.Format("  #{0,-5} {1,-45} {2,-15} ₱{3,-18:N2}", rank++, name, $"{item.UnitsSold}x", item.TotalRevenue));
                }
            }
            sb.AppendLine(subDivider);
            sb.AppendLine();

            sb.AppendLine("[ 4. ORDER TRANSACTIONS LEDGER ]");
            sb.AppendLine(subDivider);
            sb.AppendLine(string.Format("  {0,-9} {1,-20} {2,-18} {3,-14} {4,-13} {5}", "Order #", "Date & Time", "Customer", "Status", "Total (₱)", "Items Ordered"));
            sb.AppendLine("  " + new string('-', 105));

            if (orders.Count == 0)
            {
                sb.AppendLine("  (No order transactions found for the selected period)");
            }
            else
            {
                foreach (var o in orders)
                {
                    string itemsSummary = o.Items != null && o.Items.Count > 0
                        ? string.Join(", ", o.Items.Select(i => $"{i.Quantity}x {i.ItemName}" + (!string.IsNullOrEmpty(i.SelectedDoughSize) ? $" [{i.SelectedDoughSize}]" : "")))
                        : "(No items)";

                    string cust = o.CustomerName.Length > 16 ? o.CustomerName.Substring(0, 14) + ".." : o.CustomerName;
                    sb.AppendLine(string.Format("  #{0,-8} {1,-20} {2,-18} {3,-14} ₱{4,-11:N2} {5}",
                        o.OrderId,
                        o.CreatedAt.ToString("yyyy-MM-dd hh:mm tt"),
                        cust,
                        o.Status,
                        o.TotalAmount,
                        itemsSummary));
                }
            }

            sb.AppendLine(subDivider);
            sb.AppendLine();
            sb.AppendLine(divider);
            sb.AppendLine("                                     END OF REPORT - TONYO'S PIZZA");
            sb.AppendLine(divider);

            return sb.ToString();
        }
    }
}
