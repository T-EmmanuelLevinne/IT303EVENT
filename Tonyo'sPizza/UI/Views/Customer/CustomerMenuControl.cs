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

        public CustomerMenuControl()
        {
            _menuRepo = new MenuItemRepository();
            _categoryRepo = new CategoryRepository();
            _orderService = OrderService.Instance;
            _authService = AuthService.Instance;

            InitializeComponent();
            SetupCustomUIAndEvents();

            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                _orderService.CartUpdated += RefreshCartView;
                LoadCategories();
                LoadMenuItems();
                RefreshCartView();
            }
        }

        private void SetupCustomUIAndEvents()
        {
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
            _flpFoodCards.Controls.Clear();

            try
            {
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
                Label lblErr = new Label
                {
                    Text = $"Error connecting to SQL Server: {ex.Message}\nPlease configure database settings.",
                    Font = new Font("Segoe UI", 9.5f),
                    ForeColor = AppColors.StatusCancelled,
                    AutoSize = true,
                    Padding = new Padding(12)
                };
                _flpFoodCards.Controls.Add(lblErr);
            }
        }

        public void RefreshCartView()
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
                Label lblEmptyIcon = new Label
                {
                    Text = "🛒",
                    Font = new Font("Segoe UI Emoji", 26f),
                    ForeColor = AppColors.TextLight,
                    Location = new Point(0, 20),
                    Size = new Size(targetWidth, 40),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblEmptyTitle = new Label
                {
                    Text = "Your Cart is Empty",
                    Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                    ForeColor = AppColors.TextMuted,
                    Location = new Point(0, 65),
                    Size = new Size(targetWidth, 22),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblEmptySub = new Label
                {
                    Text = "Select items from the menu to start your pizza order!",
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = AppColors.TextLight,
                    Location = new Point(0, 90),
                    Size = new Size(targetWidth, 34),
                    TextAlign = ContentAlignment.TopCenter
                };
                pnlEmpty.Controls.Add(lblEmptyIcon);
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

            // 1. Validate available stock for each selected item
            StockService.Instance.RefreshIfClean();
            var outOfStockErrors = new List<string>();
            foreach (var cartItem in selectedItems)
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
                return;
            }

            var user = _authService.CurrentUser;
            string address = !string.IsNullOrEmpty(user?.Address) ? user.Address : "Dine-In / Counter Pickup";
            string phone = user?.Phone ?? "";

            decimal itemsSubtotal = selectedItems.Sum(i => i.Subtotal);
            decimal fee = selectedItems.Count > 0 ? _orderService.ServiceFee : 0m;
            decimal total = Math.Max(0, itemsSubtotal + fee - _orderService.DiscountAmount);

            if (MessageBox.Show($"Confirm order placement for {selectedItems.Count} item(s) • Total: ₱{total:N2}?", "Confirm Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_orderService.Checkout(user, phone, address, "Placed via Tonyo's Pizza App", out string err, selectedItems))
                {
                    MessageBox.Show("Order placed successfully! The kitchen crew has received your order.", "Order Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshCartView();
                }
                else
                {
                    MessageBox.Show($"Failed to place order: {err}", "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
