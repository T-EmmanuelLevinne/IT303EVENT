using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Controls
{
    public class OrderQueueCardControl : UserControl
    {
        private readonly Order _order;
        private readonly RoundedPanel _card;
        private readonly CheckBox _chkSelect;
        private readonly Label _lblOrderHeader;
        private readonly Label _lblCustomerInfo;
        private readonly PictureBox _picItem;
        private readonly Label _lblItems;
        private readonly Label _lblNotes;
        private readonly Label _lblTotal;
        private readonly RoundedPanel _badgePanel;
        private readonly Label _lblBadgeText;
        private readonly RoundedButton _btnAction;
        private readonly RoundedButton _btnCancel;
        private readonly RoundedButton _btnArchive;

        public Order Order => _order;

        public bool IsSelected
        {
            get => _chkSelect.Checked;
            set => _chkSelect.Checked = value;
        }

        public event Action<int, string> StatusChangeRequested; // OrderId, newStatus
        public event Action<int> ArchiveRequested;
        public event Action<int> UnarchiveRequested;
        public event Action<int> DeleteRequested;
        public event Action<int> CancelRequested;
        public event EventHandler SelectionChanged;

        private readonly bool _isCrewView;

        public OrderQueueCardControl(Order order, bool isCrewView = true)
        {
            _order = order;
            _isCrewView = isCrewView;

            Width = 370;
            Height = 280;
            Margin = new Padding(0, 0, 16, 16);
            BackColor = AppColors.Background;

            _card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 14,
                BackColor = Color.White,
                BorderColor = AppColors.Border,
                DrawBorder = true,
                BorderWidth = 1,
                Padding = new Padding(14)
            };

            // Top Header (Select Checkbox + Order # & Time + Badge)
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = Color.White
            };

            _chkSelect = new CheckBox
            {
                Location = new Point(0, 5),
                Size = new Size(18, 18),
                Cursor = Cursors.Hand
            };
            _chkSelect.CheckedChanged += (s, e) =>
            {
                _card.BorderColor = _chkSelect.Checked ? AppColors.Primary : AppColors.Border;
                _card.Invalidate();
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            };

            _lblOrderHeader = new Label
            {
                Text = $"Order #{_order.OrderId} • {_order.CreatedAt:hh:mm tt}",
                Font = new Font("Segoe UI Bold", 10.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(24, 4),
                AutoSize = true,
                UseMnemonic = false
            };

            // Status Badge
            _badgePanel = new RoundedPanel
            {
                Dock = DockStyle.Right,
                Height = 26,
                Width = 105,
                BorderRadius = 6,
                DrawBorder = false
            };

            _lblBadgeText = new Label
            {
                Text = (_isCrewView && _order.IsArchived && _order.Status != "Completed") ? "Archived" : _order.Status,
                Font = new Font("Segoe UI Bold", 8.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                UseMnemonic = false
            };
            _badgePanel.Controls.Add(_lblBadgeText);

            UpdateStatusBadge();

            topPanel.Controls.Add(_chkSelect);
            topPanel.Controls.Add(_lblOrderHeader);
            topPanel.Controls.Add(_badgePanel);

            // Ensure items are loaded
            if (_order.Items == null || _order.Items.Count == 0)
            {
                try
                {
                    _order.Items = new Tonyo_sPizza.Data.Repositories.OrderRepository().GetOrderItems(_order.OrderId);
                }
                catch { }
            }

            // Customer details (address removed)
            _lblCustomerInfo = new Label
            {
                Text = $"Customer: {_order.CustomerName}",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = AppColors.TextMuted,
                Dock = DockStyle.Top,
                Height = 20,
                Padding = new Padding(0, 2, 0, 0)
            };

            // Items List
            StringBuilder sb = new StringBuilder();
            if (_order.Items != null && _order.Items.Count > 0)
            {
                foreach (var itm in _order.Items)
                {
                    string details = "";
                    if (!string.IsNullOrEmpty(itm.SelectedDoughSize)) details += $" [{itm.SelectedDoughSize}]";
                    if (!string.IsNullOrEmpty(itm.SelectedAddOns)) details += $" (+{itm.SelectedAddOns})";
                    sb.AppendLine($"• {itm.Quantity}x  {itm.ItemName}{details} (₱{itm.Subtotal:N2})");
                }
            }
            else
            {
                sb.AppendLine("(No item details)");
            }

            // Pizza photo thumbnail and item description section
            Panel pnlItemSection = new Panel
            {
                Dock = DockStyle.Top,
                Height = 84,
                BackColor = Color.White,
                Padding = new Padding(0, 4, 0, 4)
            };

            RoundedPanel picWrapper = new RoundedPanel
            {
                Dock = DockStyle.Left,
                Width = 76,
                BorderRadius = 10,
                BackColor = Color.FromArgb(254, 242, 242),
                BorderColor = Color.FromArgb(254, 202, 202),
                DrawBorder = true,
                BorderWidth = 1,
                Padding = new Padding(3),
                Margin = new Padding(0, 0, 8, 0)
            };

            _picItem = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(254, 242, 242),
                Image = GetOrderItemImage()
            };
            picWrapper.Controls.Add(_picItem);

            Panel pnlDetails = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(8, 0, 0, 0)
            };

            _lblItems = new Label
            {
                Text = sb.ToString(),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = AppColors.TextDark,
                Dock = DockStyle.Fill,
                AutoEllipsis = true
            };
            pnlDetails.Controls.Add(_lblItems);

            pnlItemSection.Controls.Add(pnlDetails);
            pnlItemSection.Controls.Add(picWrapper);

            // Notes if any
            _lblNotes = new Label
            {
                Text = !string.IsNullOrEmpty(_order.Notes) ? $"Note: {_order.Notes}" : string.Empty,
                Font = new Font("Segoe UI Italic", 8.5f, FontStyle.Italic),
                ForeColor = AppColors.PrimaryDark,
                Dock = DockStyle.Top,
                Height = 22,
                Visible = !string.IsNullOrEmpty(_order.Notes)
            };

            // Bottom Panel (Total + Action Buttons)
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 36,
                BackColor = Color.White
            };

            _lblTotal = new Label
            {
                Text = $"₱{_order.TotalAmount:N2}",
                Font = new Font("Segoe UI Bold", 11.5f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryDark,
                Location = new Point(0, 8),
                AutoSize = true
            };

            // Action Buttons Panel on Right
            FlowLayoutPanel flpActions = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Height = 36,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                BackColor = Color.White
            };

            _btnAction = new RoundedButton
            {
                Width = 115,
                Height = 32,
                BorderRadius = 6,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                Margin = new Padding(4, 0, 0, 0)
            };
            ConfigureActionButton();

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Width = 65,
                Height = 32,
                BorderRadius = 6,
                NormalColor = Color.FromArgb(254, 226, 226),
                HoverColor = Color.FromArgb(254, 202, 202),
                ForeColor = Color.FromArgb(220, 38, 38),
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                Margin = new Padding(4, 0, 0, 0)
            };
            _btnCancel.Click += (s, e) => CancelRequested?.Invoke(_order.OrderId);

            _btnArchive = new RoundedButton
            {
                Text = _order.IsArchived ? "Archived" : "Archive",
                Width = 68,
                Height = 32,
                BorderRadius = 6,
                NormalColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                Margin = new Padding(4, 0, 0, 0)
            };
            _btnArchive.Click += (s, e) => ArchiveRequested?.Invoke(_order.OrderId);

            // Visibility rules for buttons
            if (_order.IsArchived)
            {
                _btnAction.Visible = false;
                _btnCancel.Visible = false;
                _btnArchive.Enabled = false;
            }
            else if (_order.Status == "Cancelled" || _order.Status == "Completed")
            {
                _btnCancel.Visible = false;
                if (_order.Status == "Completed") _btnAction.Visible = false;
            }

            if (_isCrewView)
            {
                if (_btnAction.Visible) flpActions.Controls.Add(_btnAction);
                if (_btnCancel.Visible) flpActions.Controls.Add(_btnCancel);
                flpActions.Controls.Add(_btnArchive);
            }
            else
            {
                // Customer View
                if ((_order.Status == "Pending" || _order.Status == "Preparing") && !_order.IsCustomerArchived)
                {
                    flpActions.Controls.Add(_btnCancel);
                }

                if (_order.IsCustomerArchived)
                {
                    var btnRestore = new RoundedButton
                    {
                        Text = "Restore",
                        Width = 72,
                        Height = 32,
                        BorderRadius = 6,
                        NormalColor = Color.FromArgb(241, 245, 249),
                        HoverColor = Color.FromArgb(226, 232, 240),
                        ForeColor = AppColors.TextDark,
                        Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                        Margin = new Padding(4, 0, 0, 0)
                    };
                    btnRestore.Click += (s, e) => UnarchiveRequested?.Invoke(_order.OrderId);
                    flpActions.Controls.Add(btnRestore);

                    // Customers are allowed to delete if the order was cancelled or completed
                    if (_order.Status == "Cancelled" || _order.Status == "Completed")
                    {
                        var btnDelete = new RoundedButton
                        {
                            Text = "Delete",
                            IconName = "delete",
                            Width = 72,
                            Height = 32,
                            BorderRadius = 6,
                            NormalColor = Color.FromArgb(254, 226, 226),
                            HoverColor = Color.FromArgb(254, 202, 202),
                            ForeColor = Color.FromArgb(220, 38, 38),
                            Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                            Margin = new Padding(4, 0, 0, 0)
                        };
                        btnDelete.Click += (s, e) => DeleteRequested?.Invoke(_order.OrderId);
                        flpActions.Controls.Add(btnDelete);
                    }
                }
                else if (_order.Status == "Completed" || _order.Status == "Cancelled")
                {
                    var btnDelete = new RoundedButton
                    {
                        Text = "Delete",
                        IconName = "delete",
                        Width = 72,
                        Height = 32,
                        BorderRadius = 6,
                        NormalColor = Color.FromArgb(254, 226, 226),
                        HoverColor = Color.FromArgb(254, 202, 202),
                        ForeColor = Color.FromArgb(220, 38, 38),
                        Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                        Margin = new Padding(4, 0, 0, 0)
                    };
                    btnDelete.Click += (s, e) => DeleteRequested?.Invoke(_order.OrderId);
                    flpActions.Controls.Add(btnDelete);

                    var btnCustomerArchive = new RoundedButton
                    {
                        Text = "Archive",
                        Width = 72,
                        Height = 32,
                        BorderRadius = 6,
                        NormalColor = Color.FromArgb(241, 245, 249),
                        HoverColor = Color.FromArgb(226, 232, 240),
                        ForeColor = AppColors.TextDark,
                        Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                        Margin = new Padding(4, 0, 0, 0)
                    };
                    btnCustomerArchive.Click += (s, e) => ArchiveRequested?.Invoke(_order.OrderId);
                    flpActions.Controls.Add(btnCustomerArchive);
                }
            }

            bottomPanel.Controls.Add(_lblTotal);
            bottomPanel.Controls.Add(flpActions);

            _card.Controls.Add(bottomPanel);
            _card.Controls.Add(_lblNotes);
            _card.Controls.Add(pnlItemSection);
            _card.Controls.Add(_lblCustomerInfo);
            _card.Controls.Add(topPanel);

            Controls.Add(_card);
        }

        private void UpdateStatusBadge()
        {
            if (_isCrewView && _order.IsArchived && _order.Status != "Completed")
            {
                _badgePanel.BackColor = Color.FromArgb(254, 243, 199);
                _lblBadgeText.ForeColor = Color.FromArgb(180, 83, 9);
                _lblBadgeText.Text = "Archived";
                return;
            }

            switch (_order.Status)
            {
                case "Pending":
                    _badgePanel.BackColor = AppColors.StatusPendingBg;
                    _lblBadgeText.ForeColor = AppColors.StatusPending;
                    break;
                case "Preparing":
                    _badgePanel.BackColor = AppColors.StatusPreparingBg;
                    _lblBadgeText.ForeColor = AppColors.StatusPreparing;
                    break;
                case "Ready to Serve":
                    _badgePanel.BackColor = AppColors.StatusReadyBg;
                    _lblBadgeText.ForeColor = AppColors.StatusReady;
                    break;
                case "Completed":
                    _badgePanel.BackColor = AppColors.StatusCompletedBg;
                    _lblBadgeText.ForeColor = AppColors.StatusCompleted;
                    break;
                case "Cancelled":
                    _badgePanel.BackColor = Color.FromArgb(254, 226, 226);
                    _lblBadgeText.ForeColor = Color.FromArgb(220, 38, 38);
                    break;
                default:
                    _badgePanel.BackColor = AppColors.BorderLight;
                    _lblBadgeText.ForeColor = AppColors.TextMuted;
                    break;
            }
        }

        private void ConfigureActionButton()
        {
            if (!_isCrewView || _order.IsArchived)
            {
                _btnAction.Visible = false;
                return;
            }

            switch (_order.Status)
            {
                case "Pending":
                    _btnAction.Text = "Start Preparing";
                    _btnAction.NormalColor = AppColors.StatusPreparing;
                    _btnAction.HoverColor = Color.FromArgb(59, 130, 246);
                    _btnAction.Click += (s, e) => StatusChangeRequested?.Invoke(_order.OrderId, "Preparing");
                    break;
                case "Preparing":
                    _btnAction.Text = "Ready to Serve";
                    _btnAction.NormalColor = AppColors.StatusReady;
                    _btnAction.HoverColor = Color.FromArgb(139, 92, 246);
                    _btnAction.Click += (s, e) => StatusChangeRequested?.Invoke(_order.OrderId, "Ready to Serve");
                    break;
                case "Ready to Serve":
                    _btnAction.Text = "Complete Order";
                    _btnAction.NormalColor = AppColors.StatusCompleted;
                    _btnAction.HoverColor = Color.FromArgb(16, 185, 129);
                    _btnAction.Click += (s, e) => StatusChangeRequested?.Invoke(_order.OrderId, "Completed");
                    break;
                default:
                    _btnAction.Visible = false;
                    break;
            }
        }

        private Image GetOrderItemImage()
        {
            if (_order.Items != null && _order.Items.Count > 0)
            {
                foreach (var item in _order.Items)
                {
                    if (!string.IsNullOrEmpty(item.ImageBase64))
                    {
                        try
                        {
                            byte[] bytes = Convert.FromBase64String(item.ImageBase64);
                            using (var ms = new MemoryStream(bytes))
                            using (var temp = Image.FromStream(ms))
                            {
                                return new Bitmap(temp);
                            }
                        }
                        catch { }
                    }

                    if (item.ItemId.HasValue)
                    {
                        try
                        {
                            var repo = new Tonyo_sPizza.Data.Repositories.MenuItemRepository();
                            var menuItem = repo.GetMenuItemById(item.ItemId.Value);
                            if (menuItem != null)
                            {
                                var img = menuItem.GetImage();
                                if (img != null) return img;
                            }
                        }
                        catch { }
                    }
                }

                string firstName = _order.Items[0]?.ItemName ?? "Pizza";
                return UIHelper.GeneratePlaceholderPizzaImage(76, 76, firstName);
            }

            return UIHelper.GeneratePlaceholderPizzaImage(76, 76, "Pizza");
        }
    }
}
