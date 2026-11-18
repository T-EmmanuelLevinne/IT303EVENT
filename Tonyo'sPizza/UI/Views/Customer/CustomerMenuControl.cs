using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;
using MenuItem = Tonyo_sPizza.Models.MenuItem;

namespace Tonyo_sPizza.UI.Views.Customer
{
    public partial class CustomerMenuControl : UserControl
    {
        private readonly MenuItemRepository _menuRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly OrderService _orderService;
        private readonly AuthService _authService;

        private int? _selectedCategoryId = null;
        private string _searchKeyword = string.Empty;

        // Guest Kiosk Mode Overlays ("Order Now" Welcome & Queue Number Screen)
        private Panel _pnlKioskWelcome;
        private RoundedPanel _pnlWelcomeCenter;
        private Panel _pnlKioskQueue;
        private RoundedPanel _pnlQueueCenter;
        private Color _queueBandColor = AppColors.PrimarySoft;
        private Label _lblQueueStatus;
        private Label _lblQueueNumber;
        private Label _lblQueueDateTime;
        private Label _lblPrintNote;
        private Label _lblQueueCountdown;
        private RoundedButton _btnKioskOrderAgain;
        private Timer _kioskReturnTimer;
        private int _kioskSecondsRemaining;

        public event Action<bool> KioskBrowsingChanged;

        public bool IsGuestKioskMode => _authService?.CurrentUser?.IsGuestAccount == true;

        public CustomerMenuControl()
        {
            _menuRepo = new MenuItemRepository();
            _categoryRepo = new CategoryRepository();
            _orderService = OrderService.Instance;
            _authService = AuthService.Instance;

            InitializeComponent();
            SetupCustomUIAndEvents();
            BuildKioskOverlays();

            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                _orderService.CartUpdated += RefreshCartView;
                LoadCategories();
                LoadMenuItems();
                RefreshCartView();
                InitializeForCurrentUser();
            }
        }

        public void InitializeForCurrentUser()
        {
            StopKioskTimer();
            if (IsGuestKioskMode)
            {
                ShowKioskWelcome();
            }
            else
            {
                ShowKioskMenu();
            }
        }

        private void SetupCustomUIAndEvents()
        {
            UIHelper.EnableDoubleBuffering(this);
            UIHelper.EnableDoubleBuffering(_flpFoodCards);
            UIHelper.EnableDoubleBuffering(_flpCartItems);
            UIHelper.EnableDoubleBuffering(_flpCategories);

            this.Disposed += (s, e) =>
            {
                if (_kioskReturnTimer != null)
                {
                    _kioskReturnTimer.Stop();
                    _kioskReturnTimer.Dispose();
                    _kioskReturnTimer = null;
                }
            };

            if (_pnlRightSidebar != null)
            {
                _pnlRightSidebar.Paint += (s, e) =>
                {
                    using (Pen pen = new Pen(AppColors.Border))
                    {
                        e.Graphics.DrawLine(pen, 0, 0, 0, _pnlRightSidebar.Height);
                    }
                };
            }

            if (pnlLogoCircle != null)
            {
                pnlLogoCircle.Paint += (s, e) =>
                {
                    UIHelper.SetHighQuality(e.Graphics);
                    using (var b = new SolidBrush(AppColors.Primary))
                    {
                        e.Graphics.FillEllipse(b, 0, 0, 38, 38);
                    }
                    UIHelper.DrawVectorIcon(e.Graphics, "pizza", new Rectangle(9, 9, 20, 20), Color.White);
                };
            }

            if (btnOrderNow != null)
            {
                btnOrderNow.Click += (s, e) => _flpFoodCards?.Focus();
            }

            if (pnlPromoBox != null)
            {
                pnlPromoBox.Cursor = Cursors.IBeam;
                pnlPromoBox.Click += (s, e) => _txtPromoCode?.Focus();
            }

            if (_chkCartSelectAll != null)
            {
                _chkCartSelectAll.CheckedChanged += (s, e) =>
                {
                    bool check = _chkCartSelectAll.Checked;
                    if (_flpCartItems != null)
                    {
                        foreach (Control c in _flpCartItems.Controls)
                        {
                            if (c is CartItemControl card)
                            {
                                card.IsSelected = check;
                            }
                        }
                    }
                    RecalculateSelectedTotals();
                };
            }

            if (_btnApplyPromo != null)
            {
                _btnApplyPromo.Click += (s, e) =>
                {
                    if (_orderService.ApplyPromoCode(_txtPromoCode?.Text, out string msg))
                    {
                        MessageBox.Show(msg, "Discount Applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RecalculateSelectedTotals();
                    }
                    else
                    {
                        MessageBox.Show(msg, "Promo Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
            }

            if (_btnCheckout != null)
            {
                _btnCheckout.Click += BtnCheckout_Click;
            }

            if (_flpCartItems != null)
            {
                _flpCartItems.SizeChanged += (s, e) =>
                {
                    int targetWidth = Math.Max(260, _flpCartItems.ClientSize.Width - 10);
                    foreach (Control c in _flpCartItems.Controls)
                    {
                        if (c is CartItemControl row)
                        {
                            row.Width = targetWidth;
                        }
                    }
                };
            }

            if (_lblCartAddress != null && _authService?.CurrentUser != null)
            {
                _lblCartAddress.Text = !string.IsNullOrEmpty(_authService.CurrentUser.Address) ? _authService.CurrentUser.Address : "Dine-In / Counter Pickup";
            }
        }

        public void LoadCategories()
        {
            _flpCategories.Controls.Clear();

            // "All" Category Chip
            var allChip = new CategoryChipControl(null, _selectedCategoryId == null);
            allChip.CategoryClicked += (s, e) =>
            {
                _selectedCategoryId = null;
                UpdateCategorySelection();
                LoadMenuItems();
            };
            _flpCategories.Controls.Add(allChip);

            try
            {
                var categories = _categoryRepo.GetAllCategories();
                foreach (var cat in categories)
                {
                    var chip = new CategoryChipControl(cat, _selectedCategoryId == cat.CategoryId);
                    chip.CategoryClicked += (s, e) =>
                    {
                        _selectedCategoryId = chip.Category.CategoryId;
                        UpdateCategorySelection();
                        LoadMenuItems();
                    };
                    _flpCategories.Controls.Add(chip);
                }
            }
            catch (Exception)
            {
                // Fallback if DB not yet connected
            }
        }

        private void UpdateCategorySelection()
        {
            foreach (Control c in _flpCategories.Controls)
            {
                if (c is CategoryChipControl chip)
                {
                    if (chip.Category == null)
                    {
                        chip.IsSelected = (_selectedCategoryId == null);
                    }
                    else
                    {
                        chip.IsSelected = (_selectedCategoryId == chip.Category.CategoryId);
                    }
                }
            }
        }

        public void SetSearchKeyword(string keyword)
        {
            _searchKeyword = keyword ?? string.Empty;
            LoadMenuItems();
        }

        public void LoadMenuItems()
        {
            _flpFoodCards.SuspendLayout();
            try
            {
                _flpFoodCards.Controls.Clear();

                List<MenuItem> items;
                if (!string.IsNullOrWhiteSpace(_searchKeyword))
                {
                    items = _menuRepo.SearchMenuItems(_searchKeyword);
                }
                else if (_selectedCategoryId.HasValue)
                {
                    items = _menuRepo.GetMenuItemsByCategory(_selectedCategoryId.Value);
                }
                else
                {
                    items = _menuRepo.GetAllMenuItems();
                }

                if (items.Count == 0)
                {
                    Label lblEmpty = new Label
                    {
                        Text = "No menu items found. Please check back later or add items in Admin!",
                        Font = new Font("Segoe UI", 10f),
                        ForeColor = AppColors.TextMuted,
                        AutoSize = true,
                        Padding = new Padding(12)
                    };
                    _flpFoodCards.Controls.Add(lblEmpty);
                    return;
                }

                foreach (var item in items)
                {
                    var card = new FoodCardControl(item);
                    card.OrderClicked += (selectedItem) =>
                    {
                        using (var dlg = new Tonyo_sPizza.UI.Forms.Dialogs.ItemCustomizationDialog(selectedItem))
                        {
                            if (dlg.ShowDialog() == DialogResult.OK && dlg.ResultOrderItem != null)
                            {
                                _orderService.AddToCart(dlg.ResultOrderItem);
                            }
                        }
                    };
                    _flpFoodCards.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CustomerMenuControl.LoadMenuItems");
                Label lblErr = new Label
                {
                    Text = "Unable to connect to the database. Please verify database settings.",
                    Font = new Font("Segoe UI", 9.5f),
                    ForeColor = AppColors.StatusCancelled,
                    AutoSize = true,
                    Padding = new Padding(12)
                };
                _flpFoodCards.Controls.Add(lblErr);
            }
            finally
            {
                _flpFoodCards.ResumeLayout(true);
            }
        }

        public void RefreshCartView()
        {
            _flpCartItems.SuspendLayout();
            try
            {
                _flpCartItems.Controls.Clear();

                var cart = _orderService.Cart;
                int targetWidth = Math.Max(260, _flpCartItems.ClientSize.Width - 10);

                if (cart.Count == 0)
                {
                    Panel pnlEmpty = new Panel
                    {
                        Width = targetWidth,
                        Height = 160,
                        BackColor = Color.Transparent
                    };
                    Panel pnlEmptyIcon = new Panel
                    {
                        Location = new Point((targetWidth - 48) / 2, 16),
                        Size = new Size(48, 48),
                        BackColor = Color.Transparent
                    };
                    pnlEmptyIcon.Paint += (s, e) =>
                    {
                        UIHelper.SetHighQuality(e.Graphics);
                        using (var b = new SolidBrush(AppColors.PrimarySoft))
                        {
                            e.Graphics.FillEllipse(b, 0, 0, 46, 46);
                        }
                        UIHelper.DrawVectorIcon(e.Graphics, "cart", new Rectangle(13, 13, 21, 21), AppColors.Primary);
                    };
                    Label lblEmptyTitle = new Label
                    {
                        Text = "Your Cart is Empty",
                        Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                        ForeColor = AppColors.TextMuted,
                        Location = new Point(0, 70),
                        Size = new Size(targetWidth, 22),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    Label lblEmptySub = new Label
                    {
                        Text = "Select items from the menu to start your pizza order!",
                        Font = new Font("Segoe UI", 8.5f),
                        ForeColor = AppColors.TextLight,
                        Location = new Point(0, 94),
                        Size = new Size(targetWidth, 34),
                        TextAlign = ContentAlignment.TopCenter
                    };
                    pnlEmpty.Controls.Add(pnlEmptyIcon);
                    pnlEmpty.Controls.Add(lblEmptyTitle);
                    pnlEmpty.Controls.Add(lblEmptySub);
                    _flpCartItems.Controls.Add(pnlEmpty);
                }
                else
                {
                    foreach (var itm in cart)
                    {
                        var row = new CartItemControl(itm)
                        {
                            Width = targetWidth
                        };
                        row.QuantityChanged += (orderItem, newQty) => _orderService.UpdateQuantity(orderItem, newQty);
                        row.ItemRemoved += (orderItem) => _orderService.RemoveFromCart(orderItem);
                        row.SelectionChanged += (s, e) => RecalculateSelectedTotals();
                        row.EditRequested += (orderItem) => OnEditCartItemRequested(orderItem);
                        _flpCartItems.Controls.Add(row);
                    }
                }

                RecalculateSelectedTotals();
            }
            finally
            {
                _flpCartItems.ResumeLayout(true);
            }
        }

        private void RecalculateSelectedTotals()
        {
            var selectedControls = _flpCartItems.Controls.OfType<CartItemControl>().Where(c => c.IsSelected).ToList();
            decimal selectedSubtotal = selectedControls.Sum(c => c.Item.Subtotal);
            decimal fee = selectedControls.Count > 0 ? _orderService.ServiceFee : 0m;
            decimal total = Math.Max(0, selectedSubtotal + fee - _orderService.DiscountAmount);

            _lblSubtotal.Text = $"₱{selectedSubtotal:N2}";
            _lblServiceFee.Text = $"₱{fee:N2}";
            _lblDiscount.Text = $"-₱{_orderService.DiscountAmount:N2}";
            _lblTotal.Text = $"₱{total:N2}";

            _btnCheckout.Enabled = selectedControls.Count > 0;
        }

        private void OnEditCartItemRequested(OrderItem cartItem)
        {
            try
            {
                MenuItem menuItem = null;
                if (cartItem.ItemId.HasValue)
                {
                    menuItem = _menuRepo.GetAllMenuItems().FirstOrDefault(m => m.ItemId == cartItem.ItemId.Value);
                }
                if (menuItem == null)
                {
                    menuItem = new MenuItem
                    {
                        ItemId = cartItem.ItemId ?? 0,
                        Name = cartItem.ItemName,
                        Price = cartItem.UnitPrice,
                        Description = "Customized artisan selection"
                    };
                }

                using (var dlg = new Tonyo_sPizza.UI.Forms.Dialogs.ItemCustomizationDialog(menuItem, cartItem))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (dlg.WasRemoved)
                        {
                            _orderService.RemoveFromCart(cartItem);
                        }
                        else if (dlg.ResultOrderItem != null)
                        {
                            int idx = _orderService.Cart.IndexOf(cartItem);
                            if (idx >= 0)
                            {
                                _orderService.Cart[idx] = dlg.ResultOrderItem;
                                _orderService.NotifyCartUpdated();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to edit item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateSelectedItemsStock(IEnumerable<OrderItem> items)
        {
            StockService.Instance.RefreshIfClean();
            var outOfStockErrors = new List<string>();
            foreach (var cartItem in items)
            {
                if (cartItem.ItemId.HasValue)
                {
                    try
                    {
                        var menuItem = _menuRepo.GetMenuItemById(cartItem.ItemId.Value);
                        if (menuItem != null)
                        {
                            int available = StockService.Instance.GetAvailableStockForMenuItem(menuItem, cartItem.SelectedDoughSize);
                            if (available <= 0)
                            {
                                outOfStockErrors.Add($"• '{cartItem.DisplayTitle}' is currently OUT OF STOCK.");
                            }
                            else if (cartItem.Quantity > available)
                            {
                                outOfStockErrors.Add($"• '{cartItem.DisplayTitle}' requested {cartItem.Quantity} portion(s), but only {available} available in stock.");
                            }
                        }
                    }
                    catch { }
                }
            }

            if (outOfStockErrors.Count > 0)
            {
                string msg = "Cannot complete order due to insufficient ingredient stock:\n\n" +
                             string.Join("\n", outOfStockErrors) +
                             "\n\nPlease adjust your order quantity and try again.";
                MessageBox.Show(msg, "Stock Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            var selectedItems = _flpCartItems.Controls.OfType<CartItemControl>()
                                            .Where(c => c.IsSelected)
                                            .Select(c => c.Item)
                                            .ToList();

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one item from your order to checkout.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateSelectedItemsStock(selectedItems)) return;

            using (var confirmDlg = new Tonyo_sPizza.UI.Forms.Dialogs.ConfirmOrderDialog(selectedItems, IsGuestKioskMode))
            {
                if (confirmDlg.ShowDialog(this.FindForm()) != DialogResult.OK)
                {
                    RefreshCartView();
                    return;
                }

                var confirmedItems = confirmDlg.ConfirmedItems.ToList();
                if (confirmedItems.Count == 0 || !ValidateSelectedItemsStock(confirmedItems))
                {
                    RefreshCartView();
                    return;
                }

                var user = _authService.CurrentUser;
                string address = !string.IsNullOrEmpty(user?.Address) ? user.Address : "Dine-In / Counter Pickup";
                string phone = user?.Phone ?? "";
                string orderNote = IsGuestKioskMode ? "Placed via Guest Self-Order Kiosk" : "Placed via Tonyo's Pizza App";

                if (_orderService.Checkout(user, phone, address, orderNote, out string err, confirmedItems))
                {
                    RefreshCartView();

                    if (IsGuestKioskMode && _orderService.LastPlacedOrder != null)
                    {
                        var placed = _orderService.LastPlacedOrder;
                        bool printed = new ReceiptPrintService(placed, "Self-Order Kiosk").TryAutoPrintSilently(out string printStatus);
                        ShowKioskQueueConfirmation(placed, printed, printStatus);
                    }
                    else
                    {
                        MessageBox.Show("Order placed successfully! The kitchen crew has received your order.", "Order Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show($"Failed to place order: {err}", "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==================== GUEST KIOSK WELCOME & QUEUE OVERLAYS ====================

        private void BuildKioskOverlays()
        {
            // 1. Welcome ("ORDER NOW") Kiosk Attract Screen
            _pnlKioskWelcome = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = AppColors.Background,
                Visible = false
            };
            UIHelper.EnableDoubleBuffering(_pnlKioskWelcome);
            _pnlKioskWelcome.Paint += (s, e) => PaintKioskCanvasBackdrop(e.Graphics, _pnlKioskWelcome.Size);

            _pnlWelcomeCenter = new RoundedPanel
            {
                Size = new Size(540, 232),
                BorderRadius = 16,
                BackColor = Color.White,
                BorderColor = AppColors.Border,
                BorderWidth = 1,
                DrawBorder = true
            };
            _pnlWelcomeCenter.Paint += (s, e) =>
            {
                UIHelper.SetHighQuality(e.Graphics);
                var innerRect = new Rectangle(1, 1, _pnlWelcomeCenter.Width - 3, _pnlWelcomeCenter.Height - 3);
                using (var cardPath = UIHelper.GetRoundedRectanglePath(innerRect, 15))
                {
                    var state = e.Graphics.Save();
                    e.Graphics.SetClip(cardPath);
                    using (var accentBrush = new SolidBrush(AppColors.Primary))
                    {
                        e.Graphics.FillRectangle(accentBrush, 0, 0, _pnlWelcomeCenter.Width, 5);
                    }
                    e.Graphics.Restore(state);
                }
            };

            RoundedButton btnMassiveOrderNow = new RoundedButton
            {
                Text = "ORDER NOW",
                Location = new Point((540 - 400) / 2, 46),
                Size = new Size(400, 92),
                BorderRadius = 14,
                BackColor = Color.White,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight,
                PressedColor = AppColors.PrimaryDark,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnMassiveOrderNow.Click += (s, e) =>
            {
                _selectedCategoryId = null;
                _searchKeyword = string.Empty;
                UpdateCategorySelection();
                LoadMenuItems();
                ShowKioskMenu();
            };

            Label lblTapHint = new Label
            {
                Text = "Tap Order Now to browse our full pizza & food menu",
                Font = new Font("Segoe UI", 10f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(30, 162),
                Size = new Size(480, 28),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White,
                UseMnemonic = false
            };

            _pnlWelcomeCenter.Controls.Add(btnMassiveOrderNow);
            _pnlWelcomeCenter.Controls.Add(lblTapHint);

            _pnlKioskWelcome.Controls.Add(_pnlWelcomeCenter);
            _pnlKioskWelcome.SizeChanged += (s, e) => CenterControlInParent(_pnlWelcomeCenter, _pnlKioskWelcome);

            // 2. Post-Checkout Queue Confirmation & Print Fallback Ticket Screen
            _pnlKioskQueue = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = AppColors.Background,
                Visible = false
            };
            UIHelper.EnableDoubleBuffering(_pnlKioskQueue);
            _pnlKioskQueue.Paint += (s, e) => PaintKioskCanvasBackdrop(e.Graphics, _pnlKioskQueue.Size);

            _pnlQueueCenter = new RoundedPanel
            {
                Size = new Size(540, 456),
                BorderRadius = 16,
                BackColor = Color.White,
                BorderColor = AppColors.Border,
                BorderWidth = 1,
                DrawBorder = true
            };
            _pnlQueueCenter.Paint += (s, e) =>
            {
                UIHelper.SetHighQuality(e.Graphics);
                var innerRect = new Rectangle(1, 1, _pnlQueueCenter.Width - 3, _pnlQueueCenter.Height - 3);
                using (var cardPath = UIHelper.GetRoundedRectanglePath(innerRect, 15))
                {
                    var state = e.Graphics.Save();
                    e.Graphics.SetClip(cardPath);
                    using (var bandBrush = new SolidBrush(_queueBandColor))
                    {
                        e.Graphics.FillRectangle(bandBrush, 0, 0, _pnlQueueCenter.Width, 54);
                    }
                    using (var accentBrush = new SolidBrush(AppColors.Primary))
                    {
                        e.Graphics.FillRectangle(accentBrush, 0, 0, _pnlQueueCenter.Width, 4);
                    }
                    using (var borderPen = new Pen(AppColors.Border))
                    {
                        e.Graphics.DrawLine(borderPen, 0, 53, _pnlQueueCenter.Width, 53);
                    }
                    e.Graphics.Restore(state);
                }

                // Perforated ticket rule separating queue number + date/time from CTA
                using (var dashPen = new Pen(Color.FromArgb(209, 200, 189), 1.4f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    e.Graphics.DrawLine(dashPen, 32, 294, _pnlQueueCenter.Width - 32, 294);
                }
            };

            _lblQueueStatus = new Label
            {
                Text = "ORDER CONFIRMED",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryDark,
                Location = new Point(16, 6),
                Size = new Size(508, 44),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = AppColors.PrimarySoft,
                UseMnemonic = false
            };

            Label lblQueueCaption = new Label
            {
                Text = "YOUR QUEUE NUMBER",
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                ForeColor = AppColors.TextMuted,
                Location = new Point(24, 76),
                Size = new Size(492, 24),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White,
                UseMnemonic = false
            };

            _lblQueueNumber = new Label
            {
                Text = "#001",
                Font = new Font("Segoe UI", 64f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryDark,
                Location = new Point(24, 102),
                Size = new Size(492, 106),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White,
                UseMnemonic = false
            };

            _lblQueueDateTime = new Label
            {
                Text = DateTime.Now.ToString("MMMM dd, yyyy  •  hh:mm:ss tt"),
                Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(24, 214),
                Size = new Size(492, 28),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White,
                UseMnemonic = false
            };

            _lblPrintNote = new Label
            {
                Text = "Please wait for your queue number to be called at the counter.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(24, 246),
                Size = new Size(492, 26),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White,
                UseMnemonic = false
            };

            _btnKioskOrderAgain = new RoundedButton
            {
                Text = "Order Again?",
                Location = new Point((540 - 290) / 2, 322),
                Size = new Size(290, 56),
                BorderRadius = 10,
                BackColor = Color.White,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight,
                PressedColor = AppColors.PrimaryDark,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnKioskOrderAgain.Click += (s, e) =>
            {
                StopKioskTimer();
                _selectedCategoryId = null;
                _searchKeyword = string.Empty;
                UpdateCategorySelection();
                LoadMenuItems();
                ShowKioskMenu();
            };

            _lblQueueCountdown = new Label
            {
                Text = "Returning to Order Now in 5 seconds...",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(24, 392),
                Size = new Size(492, 24),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White,
                UseMnemonic = false
            };

            _pnlQueueCenter.Controls.Add(_lblQueueStatus);
            _pnlQueueCenter.Controls.Add(lblQueueCaption);
            _pnlQueueCenter.Controls.Add(_lblQueueNumber);
            _pnlQueueCenter.Controls.Add(_lblQueueDateTime);
            _pnlQueueCenter.Controls.Add(_lblPrintNote);
            _pnlQueueCenter.Controls.Add(_btnKioskOrderAgain);
            _pnlQueueCenter.Controls.Add(_lblQueueCountdown);

            _pnlKioskQueue.Controls.Add(_pnlQueueCenter);
            _pnlKioskQueue.SizeChanged += (s, e) => CenterControlInParent(_pnlQueueCenter, _pnlKioskQueue);

            _kioskReturnTimer = new Timer { Interval = 1000 };
            _kioskReturnTimer.Tick += (s, e) =>
            {
                _kioskSecondsRemaining--;
                if (_kioskSecondsRemaining <= 0)
                {
                    StopKioskTimer();
                    ShowKioskWelcome();
                }
                else if (_lblQueueCountdown != null)
                {
                    _lblQueueCountdown.Text = $"Returning to Order Now in {_kioskSecondsRemaining} second{(_kioskSecondsRemaining == 1 ? "" : "s")}...";
                }
            };

            Controls.Add(_pnlKioskWelcome);
            Controls.Add(_pnlKioskQueue);
        }

        private static void PaintKioskCanvasBackdrop(Graphics g, Size size)
        {
            if (size.Width <= 0 || size.Height <= 0) return;
            g.Clear(AppColors.Background);
            using (var topBar = new SolidBrush(AppColors.Primary))
            {
                g.FillRectangle(topBar, 0, 0, size.Width, 4);
            }
        }

        private static void CenterControlInParent(Control child, Control parent)
        {
            if (child == null || parent == null) return;
            child.Location = new Point(
                Math.Max(0, (parent.ClientSize.Width - child.Width) / 2),
                Math.Max(0, (parent.ClientSize.Height - child.Height) / 2)
            );
        }

        private void ShowKioskWelcome()
        {
            StopKioskTimer();
            _pnlMainContent.Visible = false;
            _pnlRightSidebar.Visible = false;
            _pnlKioskQueue.Visible = false;
            _pnlKioskWelcome.Visible = true;
            _pnlKioskWelcome.BringToFront();
            CenterControlInParent(_pnlWelcomeCenter, _pnlKioskWelcome);
            KioskBrowsingChanged?.Invoke(false);
        }

        private void ShowKioskMenu()
        {
            StopKioskTimer();
            _pnlKioskWelcome.Visible = false;
            _pnlKioskQueue.Visible = false;
            _pnlRightSidebar.Visible = true;
            _pnlMainContent.Visible = true;
            _pnlMainContent.BringToFront();
            LoadCategories();
            LoadMenuItems();
            RefreshCartView();
            KioskBrowsingChanged?.Invoke(true);
        }

        private void ShowKioskQueueConfirmation(Order placedOrder, bool printed, string printStatus)
        {
            StopKioskTimer();
            _lblQueueNumber.Text = $"#{placedOrder.OrderId}";
            _lblQueueDateTime.Text = placedOrder.CreatedAt.ToString("MMMM dd, yyyy  •  hh:mm:ss tt");

            if (printed)
            {
                _queueBandColor = AppColors.StatusCompletedBg;
                _lblQueueStatus.BackColor = AppColors.StatusCompletedBg;
                _lblQueueStatus.ForeColor = AppColors.StatusCompleted;
                _lblQueueStatus.Text = "ORDER CONFIRMED  •  RECEIPT PRINTED";
                _lblPrintNote.Text = $"{printStatus} — please present your slip when called.";
            }
            else
            {
                _queueBandColor = AppColors.PrimarySoft;
                _lblQueueStatus.BackColor = AppColors.PrimarySoft;
                _lblQueueStatus.ForeColor = AppColors.PrimaryDark;
                _lblQueueStatus.Text = "ORDER CONFIRMED";
                _lblPrintNote.Text = $"{printStatus} — please keep your queue number above.";
            }
            _pnlQueueCenter.Invalidate();

            _kioskSecondsRemaining = 5;
            _lblQueueCountdown.Text = "Returning to Order Now in 5 seconds...";

            _pnlMainContent.Visible = false;
            _pnlRightSidebar.Visible = false;
            _pnlKioskWelcome.Visible = false;
            _pnlKioskQueue.Visible = true;
            _pnlKioskQueue.BringToFront();
            CenterControlInParent(_pnlQueueCenter, _pnlKioskQueue);
            KioskBrowsingChanged?.Invoke(false);

            _kioskReturnTimer.Start();
        }

        private void StopKioskTimer()
        {
            if (_kioskReturnTimer != null && _kioskReturnTimer.Enabled)
            {
                _kioskReturnTimer.Stop();
            }
        }
    }
}
