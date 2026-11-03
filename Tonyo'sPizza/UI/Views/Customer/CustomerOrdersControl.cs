using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Views.Customer
{
    public partial class CustomerOrdersControl : UserControl
    {
        private readonly OrderRepository _orderRepo;
        private readonly AuthService _authService;

        // Custom UI Controls
        private FlowLayoutPanel _pnlTabs;
        private RoundedButton _btnTabMyOrders;
        private RoundedButton _btnTabActive;
        private RoundedButton _btnTabPast;
        private RoundedButton _btnTabArchive;

        private Panel _pnlToolbar;
        private CheckBox _chkSelectAll;
        private Label _lblSelectedCount;
        private RoundedButton _btnArchiveSelected;
        private RoundedButton _btnRestoreSelected;
        private RoundedButton _btnDeleteSelected;

        private Panel _pnlArchiveNotice;
        private Label _lblArchiveNotice;

        private string _currentTab = "MyOrders"; // "MyOrders", "Active", "Past", "Archive"

        public CustomerOrdersControl()
        {
            _orderRepo = new OrderRepository();
            _authService = AuthService.Instance;

            InitializeComponent();
            SetupCustomUIAndEvents();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                LoadCustomerOrders();
            }
        }

        private void SetupCustomUIAndEvents()
        {
            if (_btnRefresh != null)
            {
                _btnRefresh.Click += (s, e) => LoadCustomerOrders();
            }

            // Adjust Header
            header.Height = 135;

            // 1. Tabs Panel: My Orders, Active Orders, Past Orders, Archive
            _pnlTabs = new FlowLayoutPanel
            {
                Location = new Point(0, 52),
                Size = new Size(600, 36),
                BackColor = Color.Transparent,
                WrapContents = false
            };

            _btnTabMyOrders = CreateTabButton("My Orders", "MyOrders", 105);
            _btnTabActive = CreateTabButton("Active Orders", "Active", 115);
            _btnTabPast = CreateTabButton("Past Orders", "Past", 105);
            _btnTabArchive = CreateTabButton("Archive", "Archive", 95);

            _pnlTabs.Controls.Add(_btnTabMyOrders);
            _pnlTabs.Controls.Add(_btnTabActive);
            _pnlTabs.Controls.Add(_btnTabPast);
            _pnlTabs.Controls.Add(_btnTabArchive);

            // 2. Toolbar Panel for Selection & Batch Actions
            _pnlToolbar = new Panel
            {
                Location = new Point(0, 94),
                Size = new Size(1100, 34),
                BackColor = Color.Transparent
            };

            _chkSelectAll = new CheckBox
            {
                Text = "Select All",
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(0, 6),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            _chkSelectAll.CheckedChanged += ChkSelectAll_CheckedChanged;

            _lblSelectedCount = new Label
            {
                Text = "0 selected",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(95, 8),
                AutoSize = true
            };

            _btnArchiveSelected = new RoundedButton
            {
                Text = "Archive Selected",
                IconName = "archive",
                Size = new Size(130, 28),
                Location = new Point(180, 2),
                BorderRadius = 6,
                NormalColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            _btnArchiveSelected.Click += BtnArchiveSelected_Click;

            _btnRestoreSelected = new RoundedButton
            {
                Text = "Restore Selected",
                IconName = "refresh",
                Size = new Size(130, 28),
                Location = new Point(180, 2),
                BorderRadius = 6,
                NormalColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false,
                Visible = false
            };
            _btnRestoreSelected.Click += BtnRestoreSelected_Click;

            _btnDeleteSelected = new RoundedButton
            {
                Text = "Delete Selected",
                IconName = "delete",
                Size = new Size(130, 28),
                Location = new Point(318, 2),
                BorderRadius = 6,
                NormalColor = Color.FromArgb(254, 226, 226),
                HoverColor = Color.FromArgb(254, 202, 202),
                ForeColor = Color.FromArgb(220, 38, 38),
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false,
                Visible = false
            };
            _btnDeleteSelected.Click += BtnDeleteSelected_Click;

            _pnlToolbar.Controls.Add(_chkSelectAll);
            _pnlToolbar.Controls.Add(_lblSelectedCount);
            _pnlToolbar.Controls.Add(_btnArchiveSelected);
            _pnlToolbar.Controls.Add(_btnRestoreSelected);
            _pnlToolbar.Controls.Add(_btnDeleteSelected);

            // 3. Notice Banner for 30-day Archive
            _pnlArchiveNotice = new Panel
            {
                Location = new Point(0, 134),
                Size = new Size(1100, 32),
                BackColor = Color.FromArgb(254, 243, 199),
                Visible = false
            };

            _lblArchiveNotice = new Label
            {
                Text = "📁 Personal Archive: Completed and cancelled orders are stored here. You can restore them anytime, or delete them permanently from your order history.",
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 83, 9),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            };
            _pnlArchiveNotice.Controls.Add(_lblArchiveNotice);

            header.Controls.Add(_pnlArchiveNotice);
            header.Controls.Add(_pnlToolbar);
            header.Controls.Add(_pnlTabs);

            UpdateTabHighlights();
        }

        private RoundedButton CreateTabButton(string text, string tabKey, int width)
        {
            var btn = new RoundedButton
            {
                Text = text,
                Tag = tabKey,
                Size = new Size(width, 32),
                BorderRadius = 6,
                NormalColor = Color.White,
                HoverColor = Color.FromArgb(241, 245, 249),
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 8, 0)
            };
            btn.Click += (s, e) =>
            {
                _currentTab = tabKey;
                if (_chkSelectAll != null) _chkSelectAll.Checked = false;
                UpdateTabHighlights();
                LoadCustomerOrders();
            };
            return btn;
        }

        private void UpdateTabHighlights()
        {
            bool isArchive = (_currentTab == "Archive");

            SetTabStyle(_btnTabMyOrders, _currentTab == "MyOrders");
            SetTabStyle(_btnTabActive, _currentTab == "Active");
            SetTabStyle(_btnTabPast, _currentTab == "Past");
            SetTabStyle(_btnTabArchive, _currentTab == "Archive");

            if (_pnlArchiveNotice != null)
            {
                _pnlArchiveNotice.Visible = isArchive;
            }

            if (_btnArchiveSelected != null)
            {
                _btnArchiveSelected.Visible = !isArchive && (_currentTab == "MyOrders" || _currentTab == "Past");
            }

            if (_btnRestoreSelected != null)
            {
                _btnRestoreSelected.Visible = isArchive;
            }

            if (_btnDeleteSelected != null)
            {
                _btnDeleteSelected.Visible = isArchive || _currentTab == "Past";
            }

            header.Height = isArchive ? 172 : 135;
        }

        private void SetTabStyle(RoundedButton btn, bool isActive)
        {
            if (btn == null) return;
            btn.NormalColor = isActive ? AppColors.Primary : Color.White;
            btn.HoverColor = isActive ? AppColors.PrimaryLight : Color.FromArgb(241, 245, 249);
            btn.ForeColor = isActive ? Color.White : AppColors.TextDark;
            btn.Invalidate();
        }

        public void LoadCustomerOrders()
        {
            _flpOrders.Controls.Clear();
            var user = _authService.CurrentUser;
            if (user == null) return;

            try
            {
                var orders = _orderRepo.GetOrdersByCustomerId(user.UserId, filter: _currentTab);
                if (orders.Count == 0)
                {
                    string emptyMsg;
                    switch (_currentTab)
                    {
                        case "Active":
                            emptyMsg = "No active orders in progress right now. Place an order from the menu to track it live!";
                            break;
                        case "Past":
                            emptyMsg = "No past completed orders found. Orders you've finished will appear here.";
                            break;
                        case "Archive":
                            emptyMsg = "Your archive is currently empty. You can archive completed or cancelled orders from Past Orders.";
                            break;
                        default:
                            emptyMsg = "You have not placed any orders yet. Head to the Menu to order your favorite wood-fired pizza!";
                            break;
                    }

                    Label lblEmpty = new Label
                    {
                        Text = emptyMsg,
                        Font = new Font("Segoe UI", 11f),
                        ForeColor = AppColors.TextMuted,
                        AutoSize = true,
                        Padding = new Padding(16)
                    };
                    _flpOrders.Controls.Add(lblEmpty);
                    UpdateSelectedSummary();
                    return;
                }

                foreach (var ord in orders)
                {
                    var card = new OrderQueueCardControl(ord, isCrewView: false);
                    card.CancelRequested += (orderId) =>
                    {
                        if (MessageBox.Show($"Are you sure you want to cancel Order #{orderId}?", "Cancel Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            if (OrderService.Instance.CancelOrder(orderId, out string err))
                            {
                                MessageBox.Show("Your order has been cancelled.", "Order Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadCustomerOrders();
                            }
                            else
                            {
                                MessageBox.Show($"Failed to cancel order: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };

                    card.ArchiveRequested += (orderId) =>
                    {
                        if (MessageBox.Show($"Move Order #{orderId} to your personal archive?", "Archive Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (OrderService.Instance.ArchiveOrderForCustomer(orderId, user.UserId, out string err))
                            {
                                LoadCustomerOrders();
                            }
                            else
                            {
                                MessageBox.Show($"Failed to archive order: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };

                    card.UnarchiveRequested += (orderId) =>
                    {
                        if (MessageBox.Show($"Restore Order #{orderId} back to your active/past orders?", "Restore Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (OrderService.Instance.UnarchiveOrderForCustomer(orderId, user.UserId, out string err))
                            {
                                LoadCustomerOrders();
                            }
                            else
                            {
                                MessageBox.Show($"Failed to restore order: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };

                    card.DeleteRequested += (orderId) =>
                    {
                        if (ord.Status != "Cancelled" && ord.Status != "Completed")
                        {
                            MessageBox.Show("You are not allowed to delete this order. Only completed or cancelled orders can be deleted.", "Action Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (MessageBox.Show($"Are you sure you want to delete Order #{orderId} from your order history?", "Delete Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            if (OrderService.Instance.DeleteOrderForCustomer(orderId, user.UserId, out string err))
                            {
                                MessageBox.Show($"Order #{orderId} has been deleted from your history.", "Order Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadCustomerOrders();
                            }
                            else
                            {
                                MessageBox.Show($"Failed to delete order: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };

                    card.SelectionChanged += (s, e) => UpdateSelectedSummary();

                    _flpOrders.Controls.Add(card);
                }

                UpdateSelectedSummary();
            }
            catch (Exception ex)
            {
                Label lblErr = new Label
                {
                    Text = $"Error loading orders: {ex.Message}",
                    Font = new Font("Segoe UI", 10f),
                    ForeColor = AppColors.StatusCancelled,
                    AutoSize = true
                };
                _flpOrders.Controls.Add(lblErr);
            }
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
            var selectedCards = _flpOrders.Controls.OfType<OrderQueueCardControl>().Where(c => c.IsSelected).ToList();
            int selectedCount = selectedCards.Count;
            _lblSelectedCount.Text = $"{selectedCount} selected";

            bool isArchive = (_currentTab == "Archive");
            if (!isArchive && _btnArchiveSelected != null)
            {
                _btnArchiveSelected.Enabled = selectedCards.Any(c => c.Order.Status == "Completed" || c.Order.Status == "Cancelled");
            }
            if (isArchive && _btnRestoreSelected != null)
            {
                _btnRestoreSelected.Enabled = selectedCount > 0;
            }
            if (_btnDeleteSelected != null && (isArchive || _currentTab == "Past"))
            {
                // Customer allowed to delete completed or cancelled orders
                _btnDeleteSelected.Enabled = selectedCards.Any(c => c.Order.Status == "Cancelled" || c.Order.Status == "Completed");
            }
        }

        private void BtnArchiveSelected_Click(object sender, EventArgs e)
        {
            var selectedCards = _flpOrders.Controls.OfType<OrderQueueCardControl>().Where(c => c.IsSelected).ToList();
            if (selectedCards.Count == 0) return;
            var user = _authService.CurrentUser;
            if (user == null) return;

            // Only completed or cancelled orders can be moved to archive
            var archivableCards = selectedCards.Where(c => c.Order.Status == "Completed" || c.Order.Status == "Cancelled").ToList();
            if (archivableCards.Count == 0)
            {
                MessageBox.Show("Only completed or cancelled orders can be moved to the archive. Active in-progress orders cannot be archived.", "Cannot Archive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ids = archivableCards.Select(c => c.Order.OrderId).ToList();
            if (MessageBox.Show($"Move {ids.Count} order(s) to your personal archive?", "Archive Orders", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (OrderService.Instance.ArchiveOrdersForCustomer(ids, user.UserId, out string err))
                {
                    _chkSelectAll.Checked = false;
                    LoadCustomerOrders();
                }
                else
                {
                    MessageBox.Show($"Failed to archive orders: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRestoreSelected_Click(object sender, EventArgs e)
        {
            var selectedCards = _flpOrders.Controls.OfType<OrderQueueCardControl>().Where(c => c.IsSelected).ToList();
            if (selectedCards.Count == 0) return;
            var user = _authService.CurrentUser;
            if (user == null) return;

            var ids = selectedCards.Select(c => c.Order.OrderId).ToList();
            if (MessageBox.Show($"Restore {ids.Count} order(s) back to your orders?", "Restore Orders", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (OrderService.Instance.UnarchiveOrdersForCustomer(ids, user.UserId, out string err))
                {
                    _chkSelectAll.Checked = false;
                    LoadCustomerOrders();
                }
                else
                {
                    MessageBox.Show($"Failed to restore orders: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteSelected_Click(object sender, EventArgs e)
        {
            var selectedCards = _flpOrders.Controls.OfType<OrderQueueCardControl>().Where(c => c.IsSelected).ToList();
            if (selectedCards.Count == 0) return;
            var user = _authService.CurrentUser;
            if (user == null) return;

            var deletableCards = selectedCards.Where(c => c.Order.Status == "Cancelled" || c.Order.Status == "Completed").ToList();
            if (deletableCards.Count == 0)
            {
                MessageBox.Show("You are not allowed to delete these orders. Only completed or cancelled orders can be deleted.", "Action Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int nonDeletableCount = selectedCards.Count - deletableCards.Count;
            string prompt = $"Are you sure you want to delete {deletableCards.Count} order(s) from your history?";
            if (nonDeletableCount > 0)
            {
                prompt += $"\n\nNote: {nonDeletableCount} active order(s) will not be deleted, as only completed or cancelled orders can be removed.";
            }

            if (MessageBox.Show(prompt, "Delete Orders", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var ids = deletableCards.Select(c => c.Order.OrderId).ToList();
                if (OrderService.Instance.DeleteOrdersForCustomer(ids, user.UserId, out int deleted, out string err))
                {
                    MessageBox.Show($"Successfully deleted {deleted} order(s) from your history.", "Orders Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _chkSelectAll.Checked = false;
                    LoadCustomerOrders();
                }
                else
                {
                    MessageBox.Show($"Failed to delete orders: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
