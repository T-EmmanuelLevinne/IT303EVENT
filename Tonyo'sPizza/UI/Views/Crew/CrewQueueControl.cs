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
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Views.Crew
{
    public partial class CrewQueueControl : UserControl
    {
        private readonly OrderRepository _orderRepo;
        private string _activeFilter = "All";
        private List<Order> _allLoadedOrders = new List<Order>();

        public CrewQueueControl()
        {
            _orderRepo = new OrderRepository();
            InitializeComponent();
            SetupCustomUIAndEvents();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LoadQueueOrders();
            }
        }

        private void SetupCustomUIAndEvents()
        {
            if (lblSearchIcon != null)
            {
                lblSearchIcon.Paint += (s, e) =>
                {
                    UIHelper.SetHighQuality(e.Graphics);
                    UIHelper.DrawVectorIcon(e.Graphics, "search", new Rectangle(0, 0, 16, 16), AppColors.TextLight);
                };
            }

            if (_txtSearch != null)
            {
                _txtSearch.TextChanged += (s, e) => FilterAndRenderCards();
            }

            if (_btnRefresh != null)
            {
                _btnRefresh.Click += (s, e) => LoadQueueOrders();
            }

            // Bind filter tags and clicks
            BindTabButton(_btnFilterAll, "All");
            BindTabButton(_btnFilterPending, "Pending");
            BindTabButton(_btnFilterPreparing, "Preparing");
            BindTabButton(_btnFilterReady, "Ready to Serve");
            BindTabButton(_btnFilterCompleted, "Completed");
            BindTabButton(_btnFilterCancelled, "Cancelled");
            BindTabButton(_btnFilterArchived, "Archived");

            UpdateTabHighlights();

            if (_chkSelectAll != null)
            {
                _chkSelectAll.CheckedChanged += ChkSelectAll_CheckedChanged;
            }

            if (_btnArchiveSelected != null)
            {
                _btnArchiveSelected.Click += BtnArchiveSelected_Click;
            }

            if (_btnCancelSelected != null)
            {
                _btnCancelSelected.Click += BtnCancelSelected_Click;
            }
        }

        private void BindTabButton(RoundedButton btn, string filterKey)
        {
            if (btn == null) return;
            btn.Tag = filterKey;
            btn.Cursor = Cursors.Hand;
            btn.Click += (s, e) =>
            {
                _activeFilter = filterKey;
                if (_chkSelectAll != null) _chkSelectAll.Checked = false;
                if (_pnlArchiveNotice != null) _pnlArchiveNotice.Visible = (_activeFilter == "Archived");
                UpdateTabHighlights();
                LoadQueueOrders();
            };
        }

        private void UpdateTabHighlights()
        {
            foreach (Control c in _pnlTabs.Controls)
            {
                if (c is RoundedButton b)
                {
                    bool active = (b.Tag?.ToString() == _activeFilter);
                    if (active)
                    {
                        b.NormalColor = AppColors.Primary;
                        b.HoverColor = AppColors.PrimaryLight;
                        b.ForeColor = Color.White;
                    }
                    else
                    {
                        b.NormalColor = Color.White;
                        b.HoverColor = Color.FromArgb(241, 245, 249);
                        b.ForeColor = AppColors.TextDark;
                    }
                    b.Invalidate();
                }
            }
        }

        public void LoadQueueOrders()
        {
            try
            {
                // Auto prune archives older than 30 days
                _orderRepo.PruneExpiredArchives();

                _allLoadedOrders = _orderRepo.GetAllOrdersForCrew(_activeFilter);
                FilterAndRenderCards();
            }
            catch (Exception ex)
            {
                _flpOrders.Controls.Clear();
                Label lblErr = new Label
                {
                    Text = $"Error loading kitchen queue: {ex.Message}",
                    Font = new Font("Segoe UI", 10f),
                    ForeColor = AppColors.StatusCancelled,
                    AutoSize = true,
                    Padding = new Padding(16)
                };
                _flpOrders.Controls.Add(lblErr);
            }
        }

        private void FilterAndRenderCards()
        {
            _flpOrders.Controls.Clear();
            string query = _txtSearch?.Text?.Trim().ToLower() ?? "";

            var filtered = _allLoadedOrders.Where(o =>
                string.IsNullOrEmpty(query) ||
                o.OrderId.ToString().Contains(query) ||
                o.CustomerName.ToLower().Contains(query) ||
                o.Status.ToLower().Contains(query) ||
                (!string.IsNullOrEmpty(o.Notes) && o.Notes.ToLower().Contains(query)) ||
                (o.Items != null && o.Items.Any(i => i.ItemName.ToLower().Contains(query)))
            ).ToList();

            if (filtered.Count == 0)
            {
                Label lblEmpty = new Label
                {
                    Text = _activeFilter == "Archived" ? "No archived orders found in the last 30 days." : "No orders found in this queue category.",
                    Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold),
                    ForeColor = AppColors.TextMuted,
                    AutoSize = true,
                    Padding = new Padding(24)
                };
                _flpOrders.Controls.Add(lblEmpty);
                UpdateSelectedSummary();
                return;
            }

            foreach (var order in filtered)
            {
                var card = new OrderQueueCardControl(order, isCrewView: true);
                card.StatusChangeRequested += OnStatusChangeRequested;
                card.CancelRequested += OnSingleCancelRequested;
                card.ArchiveRequested += OnSingleArchiveRequested;
                card.SelectionChanged += (s, e) => UpdateSelectedSummary();

                _flpOrders.Controls.Add(card);
            }

            UpdateSelectedSummary();
            UpdateTabHighlights();
        }

        private void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool check = _chkSelectAll.Checked;
            foreach (Control c in _flpOrders.Controls)
            {
                if (c is OrderQueueCardControl card)
                {
                    card.IsSelected = check;
                }
            }
            UpdateSelectedSummary();
        }

        private void UpdateSelectedSummary()
        {
            int selectedCount = _flpOrders.Controls.OfType<OrderQueueCardControl>().Count(c => c.IsSelected);
            _lblSelectedCount.Text = $"{selectedCount} selected";
            _btnArchiveSelected.Enabled = selectedCount > 0;
            _btnCancelSelected.Enabled = selectedCount > 0;
        }

        private void BtnArchiveSelected_Click(object sender, EventArgs e)
        {
            var selectedCards = _flpOrders.Controls.OfType<OrderQueueCardControl>().Where(c => c.IsSelected).ToList();
            if (selectedCards.Count == 0) return;

            var ids = selectedCards.Select(c => c.Order.OrderId).ToList();
            if (MessageBox.Show($"Are you sure you want to move {ids.Count} order(s) to the 30-day archive?", "Archive Orders", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (OrderService.Instance.ArchiveOrders(ids, out string err))
                {
                    MessageBox.Show($"Successfully archived {ids.Count} order(s).", "Archive Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _chkSelectAll.Checked = false;
                    LoadQueueOrders();
                }
                else
                {
                    MessageBox.Show($"Failed to archive orders: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancelSelected_Click(object sender, EventArgs e)
        {
            var selectedCards = _flpOrders.Controls.OfType<OrderQueueCardControl>().Where(c => c.IsSelected).ToList();
            if (selectedCards.Count == 0) return;

            var ids = selectedCards.Select(c => c.Order.OrderId).ToList();
            if (MessageBox.Show($"Are you sure you want to cancel {ids.Count} order(s)?", "Cancel Orders", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (OrderService.Instance.CancelOrders(ids, out string err))
                {
                    MessageBox.Show($"Cancelled {ids.Count} order(s).", "Orders Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _chkSelectAll.Checked = false;
                    LoadQueueOrders();
                }
                else
                {
                    MessageBox.Show($"Failed to cancel orders: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OnSingleArchiveRequested(int orderId)
        {
            if (MessageBox.Show($"Move Order #{orderId} to the 30-day archive?", "Archive Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (OrderService.Instance.ArchiveOrder(orderId, out string err))
                {
                    LoadQueueOrders();
                }
                else
                {
                    MessageBox.Show($"Failed to archive order: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OnSingleCancelRequested(int orderId)
        {
            if (MessageBox.Show($"Are you sure you want to cancel Order #{orderId}?", "Cancel Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (OrderService.Instance.CancelOrder(orderId, out string err))
                {
                    LoadQueueOrders();
                }
                else
                {
                    MessageBox.Show($"Failed to cancel order: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OnStatusChangeRequested(int orderId, string newStatus)
        {
            try
            {
                if (_orderRepo.UpdateOrderStatus(orderId, newStatus))
                {
                    if (string.Equals(newStatus, "Completed", StringComparison.OrdinalIgnoreCase))
                    {
                        AuditService.Instance.Log("Order Completed & Archived", $"Order #{orderId} completed and automatically moved to 30-day archive.");
                    }
                    else
                    {
                        AuditService.Instance.Log("Update Order Status", $"Order #{orderId} changed status to '{newStatus}'");
                    }
                    LoadQueueOrders();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order status: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblArchiveNotice_Click(object sender, EventArgs e)
        {

        }

        private void _btnArchiveSelected_Click(object sender, EventArgs e)
        {

        }
    }
}
