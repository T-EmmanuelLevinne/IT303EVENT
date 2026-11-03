using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;
using MenuItem = Tonyo_sPizza.Models.MenuItem;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class ItemCustomizationDialog : Form
    {
        private readonly MenuItem _item;
        private readonly OrderItem _existingItem;
        private MenuItemDoughSize _selectedDoughSize;
        private List<MenuItemAddOn> _selectedAddOns;
        private int _quantity = 1;

        private Label _lblItemTitle;
        private Label _lblItemDesc;
        private PictureBox _picItem;
        private FlowLayoutPanel _flpDoughSizes;
        private FlowLayoutPanel _flpAddOns;
        private Label _lblTotalSummary;
        private Label _lblQtyDisplay;
        private Label _lblStockNotice;
        private RoundedButton _btnAddToCart;
        private RoundedButton _btnCancel;
        private int _availableStock = 0;

        public OrderItem ResultOrderItem { get; private set; }
        public bool WasRemoved { get; private set; } = false;

        public ItemCustomizationDialog(MenuItem item, OrderItem existingItem = null)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
            _existingItem = existingItem;
            _selectedAddOns = new List<MenuItemAddOn>();

            // Ensure dough sizes and add-ons are loaded from repository if needed
            if ((_item.DoughSizes == null || _item.DoughSizes.Count == 0) && _item.ItemId > 0)
            {
                try
                {
                    var repo = new Data.Repositories.MenuItemRepository();
                    _item.DoughSizes = repo.GetMenuItemDoughSizes(_item.ItemId);
                }
                catch { }
            }
            if (_item.DoughSizes == null) _item.DoughSizes = new List<MenuItemDoughSize>();

            if ((_item.AvailableAddOns == null || _item.AvailableAddOns.Count == 0) && _item.ItemId > 0)
            {
                try
                {
                    var repo = new Data.Repositories.MenuItemRepository();
                    _item.AvailableAddOns = repo.GetMenuItemAddOns(_item.ItemId);
                }
                catch { }
            }
            if (_item.AvailableAddOns == null) _item.AvailableAddOns = new List<MenuItemAddOn>();

            // Handle pre-selection from existing cart item or defaults
            if (_item.DoughSizes.Count > 0)
            {
                if (_existingItem != null && !string.IsNullOrEmpty(_existingItem.SelectedDoughSize))
                {
                    _selectedDoughSize = _item.DoughSizes.FirstOrDefault(ds => ds.SizeName == _existingItem.SelectedDoughSize)
                                         ?? _item.DoughSizes.FirstOrDefault(ds => ds.IsDefault)
                                         ?? _item.DoughSizes[0];
                }
                else
                {
                    _selectedDoughSize = _item.DoughSizes.FirstOrDefault(ds => ds.IsDefault) ?? _item.DoughSizes[0];
                }
            }
            else
            {
                _selectedDoughSize = null;
            }

            if (_existingItem != null)
            {
                _quantity = Math.Max(1, _existingItem.Quantity);

                if (!string.IsNullOrEmpty(_existingItem.SelectedAddOns))
                {
                    var names = _existingItem.SelectedAddOns.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                          .Select(s => s.Trim().ToLower())
                                                          .ToList();

                    foreach (var ao in _item.AvailableAddOns)
                    {
                        if (names.Any(n => ao.AddOnName.ToLower().Contains(n)))
                        {
                            _selectedAddOns.Add(ao);
                        }
                    }
                }
            }

            InitializeComponent();
            PopulateData();
            UpdateAvailableStock();
            RecalculateTotal();
        }

        private void InitializeComponent()
        {
            Text = _existingItem != null ? "Edit Menu Item - Tonyo's Pizza" : "Customize Pizza - Tonyo's Pizza";
            Size = new Size(600, 710);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = AppColors.Background;

            // 1. Header (Food image + Name + Description)
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.White,
                Padding = new Padding(16, 12, 16, 12)
            };

            _picItem = new PictureBox
            {
                Location = new Point(16, 12),
                Size = new Size(125, 95),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(248, 250, 252),
                BorderStyle = BorderStyle.FixedSingle
            };

            _lblItemTitle = new Label
            {
                Location = new Point(155, 14),
                AutoSize = true,
                Font = new Font("Segoe UI Bold", 13.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark
            };

            _lblItemDesc = new Label
            {
                Location = new Point(155, 44),
                Size = new Size(405, 58),
                Font = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextMuted
            };

            header.Controls.Add(_picItem);
            header.Controls.Add(_lblItemTitle);
            header.Controls.Add(_lblItemDesc);

            // 2. Body Container
            Panel body = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(18, 10, 18, 10),
                BackColor = Color.White
            };

            int y = 6;

            // Dough Size Section (only shown if item has dough sizes)
            bool hasDoughSizes = _item.DoughSizes != null && _item.DoughSizes.Count > 0;
            if (hasDoughSizes)
            {
                Label lblDoughTitle = new Label
                {
                    Text = _item.DoughSizes.Count > 1 
                        ? $"Choose Dough Size ({_item.DoughSizes.Count} Sizes Available):" 
                        : "Dough Size / Crust:",
                    Location = new Point(18, y),
                    AutoSize = true,
                    Font = new Font("Segoe UI Bold", 10.5f, FontStyle.Bold),
                    ForeColor = AppColors.TextDark
                };
                y += 26;

                int doughHeight = _item.DoughSizes.Count <= 2 ? 46 : 88;
                _flpDoughSizes = new FlowLayoutPanel
                {
                    Location = new Point(18, y),
                    Width = 545,
                    Height = doughHeight,
                    AutoScroll = true,
                    WrapContents = true
                };
                y += doughHeight + 10;

                body.Controls.Add(lblDoughTitle);
                body.Controls.Add(_flpDoughSizes);
            }
            else
            {
                _flpDoughSizes = new FlowLayoutPanel { Visible = false, Size = Size.Empty };
            }

            // Add-ons Section
            bool hasAddOns = _item.AvailableAddOns != null && _item.AvailableAddOns.Count > 0;
            Label lblAddOnTitle = new Label
            {
                Text = hasAddOns 
                    ? "Optional Add-ons & Extra Toppings (Checkbox to select):" 
                    : "Optional Add-ons:",
                Location = new Point(18, y),
                AutoSize = true,
                Font = new Font("Segoe UI Bold", 10.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark
            };
            y += 26;

            int addOnsHeight = hasDoughSizes ? (_item.DoughSizes.Count <= 2 ? 285 : 245) : 340;
            _flpAddOns = new FlowLayoutPanel
            {
                Location = new Point(18, y),
                Width = 545,
                Height = addOnsHeight,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            body.Controls.Add(lblAddOnTitle);
            body.Controls.Add(_flpAddOns);

            // 3. Bottom Action Bar
            Panel bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 72,
                BackColor = Color.White,
                Padding = new Padding(18, 12, 18, 14)
            };

            // Quantity selector: [-] [ 1 ] [+]
            Button btnMinus = new Button
            {
                Text = "-",
                Location = new Point(18, 18),
                Size = new Size(34, 34),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Bold", 11f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _lblQtyDisplay = new Label
            {
                Text = _quantity.ToString(),
                Location = new Point(56, 22),
                Width = 32,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Bold", 11f, FontStyle.Bold)
            };
            Button btnPlus = new Button
            {
                Text = "+",
                Location = new Point(92, 18),
                Size = new Size(34, 34),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Bold", 11f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnMinus.Click += (s, e) =>
            {
                if (_quantity > 1)
                {
                    _quantity--;
                    _lblQtyDisplay.Text = _quantity.ToString();
                    RecalculateTotal();
                }
            };
            btnPlus.Click += (s, e) =>
            {
                if (_quantity < _availableStock)
                {
                    _quantity++;
                    _lblQtyDisplay.Text = _quantity.ToString();
                    RecalculateTotal();
                }
                else
                {
                    MessageBox.Show($"Only {_availableStock} portion(s) available in stock.", "Stock Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            _lblTotalSummary = new Label
            {
                Location = new Point(138, 14),
                AutoSize = true,
                Font = new Font("Segoe UI Bold", 12f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryDark
            };

            _lblStockNotice = new Label
            {
                Location = new Point(138, 38),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = AppColors.TextMuted
            };

            // Right buttons panel (Remove + Cancel + Save/Add)
            Panel pnlActionBtns = new Panel
            {
                Dock = DockStyle.Right,
                Width = _existingItem != null ? 350 : 260,
                Height = 44,
                BackColor = Color.Transparent
            };

            if (_existingItem != null)
            {
                var btnRemove = new RoundedButton
                {
                    Text = "Remove",
                    IconName = "delete",
                    Location = new Point(0, 4),
                    Size = new Size(95, 40),
                    BorderRadius = 8,
                    NormalColor = Color.FromArgb(254, 242, 242),
                    HoverColor = Color.FromArgb(254, 226, 226),
                    ForeColor = Color.FromArgb(220, 38, 38),
                    Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnRemove.Click += (s, e) =>
                {
                    if (MessageBox.Show($"Remove '{_item.Name}' from your cart?", "Remove Pizza", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        WasRemoved = true;
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                };
                pnlActionBtns.Controls.Add(btnRemove);
            }

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Location = new Point(_existingItem != null ? 102 : 0, 4),
                Size = new Size(90, 40),
                BorderRadius = 8,
                NormalColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold)
            };
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            _btnAddToCart = new RoundedButton
            {
                Text = _existingItem != null ? "Save Changes" : "Add to Cart",
                IconName = "check",
                Location = new Point(_existingItem != null ? 198 : 98, 4),
                Size = new Size(_existingItem != null ? 145 : 150, 40),
                BorderRadius = 8,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight,
                Font = new Font("Segoe UI Bold", 10.5f, FontStyle.Bold)
            };
            _btnAddToCart.Click += BtnAddToCart_Click;

            pnlActionBtns.Controls.Add(_btnCancel);
            pnlActionBtns.Controls.Add(_btnAddToCart);

            bottom.Controls.Add(btnMinus);
            bottom.Controls.Add(_lblQtyDisplay);
            bottom.Controls.Add(btnPlus);
            bottom.Controls.Add(_lblTotalSummary);
            bottom.Controls.Add(_lblStockNotice);
            bottom.Controls.Add(pnlActionBtns);

            Controls.Add(body);
            Controls.Add(bottom);
            Controls.Add(header);
        }

        private void PopulateData()
        {
            _lblItemTitle.Text = _item.Name;
            _lblItemDesc.Text = _item.Description;
            _picItem.Image = _item.GetImage() ?? UIHelper.GeneratePlaceholderPizzaImage(125, 95, _item.Name);

            // 1. Render Dough Sizes (Radio cards) - Only display what the admin actually added!
            if (_flpDoughSizes != null && _item.DoughSizes != null && _item.DoughSizes.Count > 0)
            {
                _flpDoughSizes.SuspendLayout();
                _flpDoughSizes.Controls.Clear();

                foreach (var ds in _item.DoughSizes)
                {
                    bool isThisSelected = (_selectedDoughSize != null && _selectedDoughSize.SizeName == ds.SizeName);
                    var btnSize = new RadioButton
                    {
                        Text = $"{ds.SizeName} (₱{ds.Price:N2})",
                        Checked = isThisSelected,
                        AutoSize = true,
                        Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                        Margin = new Padding(0, 0, 16, 8),
                        Cursor = Cursors.Hand
                    };
                    btnSize.CheckedChanged += (s, e) =>
                    {
                        if (btnSize.Checked)
                        {
                            _selectedDoughSize = ds;
                            UpdateAvailableStock();
                            RecalculateTotal();
                        }
                    };
                    _flpDoughSizes.Controls.Add(btnSize);
                }
                _flpDoughSizes.ResumeLayout();
            }

            // 2. Render Add-ons Grouped by Category - Only display what the admin actually added!
            if (_flpAddOns != null)
            {
                _flpAddOns.SuspendLayout();
                _flpAddOns.Controls.Clear();

                if (_item.AvailableAddOns != null && _item.AvailableAddOns.Count > 0)
                {
                    var groups = _item.AvailableAddOns.GroupBy(a => a.CategoryName ?? "Toppings");
                    foreach (var grp in groups)
                    {
                        Label lblGrp = new Label
                        {
                            Text = grp.Key.ToUpper(),
                            Font = new Font("Segoe UI Bold", 8.5f, FontStyle.Bold),
                            ForeColor = AppColors.TextDark,
                            Margin = new Padding(0, 6, 0, 2),
                            AutoSize = true
                        };
                        _flpAddOns.Controls.Add(lblGrp);

                        foreach (var ao in grp)
                        {
                            bool isPreChecked = _selectedAddOns.Any(a => a.AddOnName.Equals(ao.AddOnName, StringComparison.OrdinalIgnoreCase));
                            CheckBox chk = new CheckBox
                            {
                                Text = $"{ao.AddOnName} (+₱{ao.Price:N2})",
                                Checked = isPreChecked,
                                AutoSize = true,
                                Font = new Font("Segoe UI", 9f),
                                Margin = new Padding(12, 1, 0, 1),
                                Cursor = Cursors.Hand
                            };
                            chk.CheckedChanged += (s, e) =>
                            {
                                if (chk.Checked)
                                {
                                    if (!_selectedAddOns.Any(a => a.AddOnName.Equals(ao.AddOnName, StringComparison.OrdinalIgnoreCase)))
                                        _selectedAddOns.Add(ao);
                                }
                                else
                                {
                                    _selectedAddOns.RemoveAll(a => a.AddOnName.Equals(ao.AddOnName, StringComparison.OrdinalIgnoreCase));
                                }
                                RecalculateTotal();
                            };
                            _flpAddOns.Controls.Add(chk);
                        }
                    }
                }
                else
                {
                    Label lblNone = new Label
                    {
                        Text = "No optional add-on toppings configured for this item.",
                        Font = new Font("Segoe UI", 9.5f, FontStyle.Italic),
                        ForeColor = AppColors.TextMuted,
                        AutoSize = true,
                        Padding = new Padding(12)
                    };
                    _flpAddOns.Controls.Add(lblNone);
                }

                _flpAddOns.ResumeLayout();
            }
        }

        private void UpdateAvailableStock()
        {
            _availableStock = Services.StockService.Instance.GetAvailableStockForMenuItem(_item, _selectedDoughSize?.SizeName);
            if (_availableStock <= 0)
            {
                _quantity = 0;
                if (_lblQtyDisplay != null) _lblQtyDisplay.Text = "0";
                if (_btnAddToCart != null)
                {
                    _btnAddToCart.Enabled = false;
                    _btnAddToCart.Text = "Out of Stock";
                    _btnAddToCart.NormalColor = Color.FromArgb(241, 245, 249);
                    _btnAddToCart.ForeColor = Color.FromArgb(148, 163, 184);
                }
                if (_lblStockNotice != null)
                {
                    _lblStockNotice.Text = "Out of Stock (Ingredients depleted)";
                    _lblStockNotice.ForeColor = Color.FromArgb(220, 38, 38);
                }
            }
            else
            {
                if (_quantity <= 0) _quantity = 1;
                if (_quantity > _availableStock) _quantity = _availableStock;
                if (_lblQtyDisplay != null) _lblQtyDisplay.Text = _quantity.ToString();
                if (_btnAddToCart != null)
                {
                    _btnAddToCart.Enabled = true;
                    _btnAddToCart.Text = _existingItem != null ? "Save Changes" : "Add to Cart";
                    _btnAddToCart.NormalColor = AppColors.Primary;
                    _btnAddToCart.ForeColor = Color.White;
                }
                if (_lblStockNotice != null)
                {
                    _lblStockNotice.Text = $"Stock Available: {_availableStock} portion{(_availableStock > 1 ? "s" : "")}";
                    _lblStockNotice.ForeColor = _availableStock <= 5 ? Color.FromArgb(217, 119, 6) : AppColors.TextMuted;
                }
            }
        }

        private void RecalculateTotal()
        {
            decimal basePrice = _selectedDoughSize != null ? _selectedDoughSize.Price : _item.Price;
            decimal addOnsTotal = _selectedAddOns.Sum(a => a.Price);
            decimal unitPrice = basePrice + addOnsTotal;
            decimal total = unitPrice * _quantity;

            _lblTotalSummary.Text = $"Total: ₱{total:N2}";
        }

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            if (_availableStock <= 0 || _quantity <= 0)
            {
                MessageBox.Show($"Sorry, '{_item.Name}' is currently out of stock.", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_quantity > _availableStock)
            {
                MessageBox.Show($"Cannot order {_quantity} portion(s). Only {_availableStock} available in stock.", "Stock Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal basePrice = _selectedDoughSize != null ? _selectedDoughSize.Price : _item.Price;
            decimal addOnsTotal = _selectedAddOns.Sum(a => a.Price);
            decimal finalUnitPrice = basePrice + addOnsTotal;

            string doughName = _selectedDoughSize != null ? _selectedDoughSize.SizeName : null;
            string addOnsStr = _selectedAddOns.Count > 0 ? string.Join(", ", _selectedAddOns.Select(a => a.AddOnName)) : null;

            ResultOrderItem = new OrderItem(_item, _quantity, doughName, finalUnitPrice, addOnsStr);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
