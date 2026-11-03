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
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminSalesReportControl : UserControl
    {
        private readonly OrderRepository _orderRepo;

        public AdminSalesReportControl()
        {
            _orderRepo = new OrderRepository();
            InitializeComponent();
            SetupCustomUIAndEvents();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                LoadReportData();
            }
        }

        private void SetupCustomUIAndEvents()
        {
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
                    UIHelper.DrawVectorIcon(e.Graphics, "search", new Rectangle(0, 0, 16, 16), AppColors.TextLight);
                };
            }

            if (_txtSearch != null) _txtSearch.TextChanged += (s, e) => LoadReportData();
            if (_btnRefresh != null) _btnRefresh.Click += (s, e) => LoadReportData();
            if (_btnExport != null) _btnExport.Click += BtnExport_Click;

            if (_dgvTopItems != null)
            {
                UIHelper.StyleDataGridView(_dgvTopItems);
                if (_dgvTopItems.Columns.Count == 0)
                {
                    _dgvTopItems.Columns.Add("Rank", "#");
                    _dgvTopItems.Columns.Add("ItemName", "Item");
                    _dgvTopItems.Columns.Add("UnitsSold", "Sold");
                    _dgvTopItems.Columns.Add("Revenue", "Revenue");

                    _dgvTopItems.Columns["Rank"].Width = 40;
                    _dgvTopItems.Columns["ItemName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    _dgvTopItems.Columns["UnitsSold"].Width = 60;
                    _dgvTopItems.Columns["Revenue"].Width = 90;
                }
            }

            if (_dgvOrders != null)
            {
                UIHelper.StyleDataGridView(_dgvOrders);
                if (_dgvOrders.Columns.Count == 0)
                {
                    _dgvOrders.Columns.Add("OrderId", "Order #");
                    _dgvOrders.Columns.Add("CreatedAt", "Date & Time");
                    _dgvOrders.Columns.Add("Customer", "Customer");
                    _dgvOrders.Columns.Add("Items", "Items Ordered");
                    _dgvOrders.Columns.Add("Status", "Status");
                    _dgvOrders.Columns.Add("TotalAmount", "Total (₱)");

                    _dgvOrders.Columns["OrderId"].Width = 75;
                    _dgvOrders.Columns["CreatedAt"].Width = 135;
                    _dgvOrders.Columns["Customer"].Width = 120;
                    _dgvOrders.Columns["Items"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    _dgvOrders.Columns["Status"].Width = 100;
                    _dgvOrders.Columns["TotalAmount"].Width = 95;
                }

                _dgvOrders.CellFormatting += DgvOrders_CellFormatting;
            }
        }

        private void CboStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string status = _cboStatusFilter.SelectedItem?.ToString() ?? "";
            if (status == "Show All Transactions")
            {
                // When selecting Show All Transactions, ensure period is set to All Time to display every transaction made
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
            string col = _dgvOrders.Columns[e.ColumnIndex].Name;

            if (col == "Status")
            {
                string status = e.Value.ToString();
                if (status == "Completed")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129);
                    e.CellStyle.Font = new Font("Segoe UI Bold", 9f, FontStyle.Bold);
                }
                else if (status == "Cancelled")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68);
                    e.CellStyle.Font = new Font("Segoe UI Bold", 9f, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(245, 158, 11);
                    e.CellStyle.Font = new Font("Segoe UI Bold", 9f, FontStyle.Bold);
                }
            }
            else if (col == "TotalAmount")
            {
                e.CellStyle.Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold);
                e.CellStyle.ForeColor = AppColors.TextDark;
            }
        }

        public void LoadReportData()
        {
            DateTime fromDate = _dtpFrom.Value.Date;
            DateTime toDate = _dtpTo.Value.Date;
            string status = _cboStatusFilter?.SelectedItem?.ToString() ?? "Show All Transactions";
            string search = _txtSearch?.Text?.Trim() ?? "";

            List<Order> orders = _orderRepo.GetOrdersForSalesReport(fromDate, toDate, status, search);

            // 1. Calculate KPI Metrics
            decimal grossRevenue = orders.Where(o => o.Status != "Cancelled").Sum(o => o.TotalAmount);
            int completedCount = orders.Count(o => o.Status == "Completed");
            int activeCount = orders.Count(o => o.Status == "Pending" || o.Status == "Preparing" || o.Status == "Ready to Serve");
            decimal avgOrderValue = completedCount > 0 ? grossRevenue / completedCount : (orders.Count > 0 ? grossRevenue / orders.Count : 0m);

            if (_lblGrossRevenue != null) _lblGrossRevenue.Text = $"₱{grossRevenue:N2}";
            if (_lblCompletedCount != null) _lblCompletedCount.Text = $"{completedCount}";
            if (_lblActiveCount != null) _lblActiveCount.Text = $"{activeCount}";
            if (_lblAverageOrderValue != null) _lblAverageOrderValue.Text = $"₱{avgOrderValue:N2}";

            // 2. Populate Top Selling Items
            _dgvTopItems.Rows.Clear();
            var topItems = orders.Where(o => o.Status != "Cancelled")
                .SelectMany(o => o.Items ?? new List<OrderItem>())
                .GroupBy(i => i.ItemName)
                .Select(g => new
                {
                    ItemName = g.Key,
                    UnitsSold = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Subtotal)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(15)
                .ToList();

            int rank = 1;
            foreach (var item in topItems)
            {
                _dgvTopItems.Rows.Add(
                    $"#{rank++}",
                    item.ItemName,
                    $"{item.UnitsSold}x",
                    $"₱{item.TotalRevenue:N2}"
                );
            }

            // 3. Populate Order Ledger
            _dgvOrders.Rows.Clear();
            foreach (var o in orders)
            {
                string itemsSummary = o.Items != null && o.Items.Count > 0
                    ? string.Join(", ", o.Items.Select(i => $"{i.Quantity}x {i.ItemName}"))
                    : "No items";

                _dgvOrders.Rows.Add(
                    $"#{o.OrderId}",
                    o.CreatedAt.ToString("yyyy-MM-dd hh:mm tt"),
                    o.CustomerName,
                    itemsSummary,
                    o.Status,
                    $"₱{o.TotalAmount:N2}"
                );
            }

            if (_lblOrdersCount != null)
            {
                _lblOrdersCount.Text = $"{orders.Count} transaction(s)";
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dgvOrders.Rows.Count == 0)
                {
                    MessageBox.Show("There are no transactions shown in the table to export.", "No Transactions", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                        try
                        {
                            AuditService.Instance.Log("Export Transactions", $"Exported {_dgvOrders.Rows.Count} transaction(s) to {Path.GetFileName(sfd.FileName)}.");
                        }
                        catch { }

                        if (MessageBox.Show($"Transactions successfully exported to:\n{sfd.FileName}\n\nWould you like to open it now?", "Export Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export report: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateCsvReportFromTable()
        {
            StringBuilder sb = new StringBuilder();
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

            // 1. Executive Summary
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

            // 2. Best-Selling Menu Items
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

            sb.AppendLine("[ 2. BEST-SELLING MENU ITEMS ]");
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

            // 3. Order Transactions Ledger
            sb.AppendLine("[ 3. ORDER TRANSACTIONS LEDGER ]");
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
