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

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class ConfirmOrderDialog : Form
    {
        private readonly List<OrderItem> _items;
        private readonly OrderService _orderService;
        private readonly MenuItemRepository _menuRepo;
        private readonly bool _isGuestKiosk;

        private FlowLayoutPanel _flpItems;
        private Label _lblSubtotal;
        private Label _lblServiceFee;
        private Label _lblDiscount;
        private Label _lblTotal;
        private Label _lblEmptyWarning;
        private RoundedButton _btnCancel;
        private RoundedButton _btnConfirm;

        public IReadOnlyList<OrderItem> ConfirmedItems => _items;

        public ConfirmOrderDialog(IEnumerable<OrderItem> selectedItems, bool isGuestKiosk)
        {
            _items = selectedItems?.Where(i => i != null).ToList() ?? new List<OrderItem>();
            _orderService = OrderService.Instance;
            _menuRepo = new MenuItemRepository();
            _isGuestKiosk = isGuestKiosk;

            InitializeUI();
            RenderOrderRows();
        }

        private void InitializeUI()
        {
            Text = "Confirm Order";
            Size = new Size(640, 566);
            MinimumSize = new Size(580, 460);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = AppColors.Background;
            Font = new Font("Segoe UI", 9.5f);

            // Header
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 68,
                BackColor = Color.White,
                Padding = new Padding(22, 12, 22, 10)
            };
            pnlHeader.Paint += (s, e) =>
            {
                using (var pen = new Pen(AppColors.Border))
                {
                    e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
                }
            };

            Label lblTitle = new Label
            {
                Text = "Confirm Order",
                Font = new Font("Segoe UI", 14.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(22, 10),
                AutoSize = true,
                UseMnemonic = false
            };

            Label lblSub = new Label
            {
                Text = "Review your selected items and make any corrections before placing your order.",
                Font = new Font("Segoe UI", 8.75f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(23, 38),
                Size = new Size(550, 20),
                UseMnemonic = false
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);

            // Footer Summary & Actions
            Panel pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Size = new Size(ClientSize.Width, 166),
                BackColor = Color.White,
                Padding = new Padding(22, 12, 22, 14)
            };
            pnlFooter.Paint += (s, e) =>
            {
                using (var pen = new Pen(AppColors.Border))
                {
                    e.Graphics.DrawLine(pen, 0, 0, pnlFooter.Width, 0);
                }
            };

            AddSummaryRow(pnlFooter, "Subtotal", 12, out _lblSubtotal, AppColors.TextDark, false);
            AddSummaryRow(pnlFooter, "Service Fee", 34, out _lblServiceFee, AppColors.TextMuted, false);
            AddSummaryRow(pnlFooter, "Discount", 56, out _lblDiscount, AppColors.Primary, false);

            Panel sep = new Panel
            {
                Location = new Point(22, 80),
                Size = new Size(560, 1),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = AppColors.BorderLight
            };
            pnlFooter.Controls.Add(sep);

            AddSummaryRow(pnlFooter, "Total", 86, out _lblTotal, AppColors.PrimaryDark, true);

            _lblEmptyWarning = new Label
            {
                Text = "Your selection is empty. Return to the menu to add items.",
                Font = new Font("Segoe UI Semibold", 8.75f, FontStyle.Bold),
                ForeColor = AppColors.StatusCancelled,
                Location = new Point(22, 120),
                Size = new Size(260, 32),
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = false,
                UseMnemonic = false
            };

            _btnCancel = new RoundedButton
            {
                Text = "Back to Menu",
                Size = new Size(138, 38),
                Location = new Point(296, 114),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderRadius = 8,
                BackColor = Color.White,
                NormalColor = AppColors.SecondaryNormal,
                HoverColor = AppColors.SecondaryHover,
                PressedColor = AppColors.SecondaryPressed,
                ForeColor = AppColors.TextDark,
                CustomBorderColor = AppColors.Border,
                CustomBorderWidth = 1,
                Font = new Font("Segoe UI Semibold", 9.25f, FontStyle.Bold)
            };
            _btnCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            _btnConfirm = new RoundedButton
            {
                Text = _isGuestKiosk ? "Confirm & Place Order" : "Place Order",
                IconName = "check",
                Size = new Size(192, 38),
                Location = new Point(400, 114),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderRadius = 8,
                BackColor = Color.White,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight,
                PressedColor = AppColors.PrimaryDark,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold)
            };
            _btnConfirm.Click += (s, e) =>
            {
                if (_items.Count == 0)
                {
                    MessageBox.Show(this, "Please keep at least one item in your order.", "Order Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DialogResult = DialogResult.OK;
                Close();
            };

            // Align right-edge action buttons cleanly to pnlFooter width
            int fw = pnlFooter.Width;
            _btnConfirm.Location = new Point(fw - 22 - _btnConfirm.Width, 114);
            _btnCancel.Location = new Point(_btnConfirm.Left - 10 - _btnCancel.Width, 114);
            sep.Width = fw - 44;

            pnlFooter.Controls.Add(_lblEmptyWarning);
            pnlFooter.Controls.Add(_btnCancel);
            pnlFooter.Controls.Add(_btnConfirm);

            // Scrollable Order Items List
            _flpItems = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = AppColors.Background,
                Padding = new Padding(18, 12, 18, 12)
            };
            UIHelper.EnableDoubleBuffering(_flpItems);

            Controls.Add(_flpItems);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
        }

        private void AddSummaryRow(Panel parent, string label, int y, out Label valueLabel, Color valueColor, bool isTotal)
        {
            Label lbl = new Label
            {
                Text = label,
                Location = new Point(22, y),
                Size = new Size(200, isTotal ? 24 : 20),
                Font = isTotal ? new Font("Segoe UI", 11.5f, FontStyle.Bold) : new Font("Segoe UI", 9f),
                ForeColor = isTotal ? AppColors.TextDark : AppColors.TextMuted,
                UseMnemonic = false
            };

            valueLabel = new Label
            {
                Text = "₱0.00",
                Location = new Point(parent.Width - 22 - 180, y),
                Size = new Size(180, isTotal ? 24 : 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleRight,
                Font = isTotal ? new Font("Segoe UI", 12.5f, FontStyle.Bold) : new Font("Segoe UI Semibold", 9.25f, FontStyle.Bold),
                ForeColor = valueColor,
                UseMnemonic = false
            };

            parent.Controls.Add(lbl);
            parent.Controls.Add(valueLabel);
        }

        private void RenderOrderRows()
        {
            _flpItems.SuspendLayout();
            try
            {
                _flpItems.Controls.Clear();
                int rowWidth = Math.Max(460, _flpItems.ClientSize.Width - 40);

                if (_items.Count == 0)
                {
                    Label empty = new Label
                    {
                        Text = "No items remaining in this order.",
                        Font = new Font("Segoe UI", 10f),
                        ForeColor = AppColors.TextMuted,
                        Size = new Size(rowWidth, 64),
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseMnemonic = false
                    };
                    _flpItems.Controls.Add(empty);
                }
                else
                {
                    foreach (var item in _items.ToList())
                    {
                        _flpItems.Controls.Add(CreateItemRow(item, rowWidth));
                    }
                }

                UpdateTotals();
            }
            finally
            {
                _flpItems.ResumeLayout(true);
            }
        }

        private Control CreateItemRow(OrderItem item, int rowWidth)
        {
            RoundedPanel row = new RoundedPanel
            {
                Size = new Size(rowWidth, 74),
                Margin = new Padding(0, 0, 0, 8),
                BorderRadius = 10,
                BackColor = Color.White,
                BorderColor = AppColors.Border,
                BorderWidth = 1,
                DrawBorder = true
            };

            PictureBox pic = new PictureBox
            {
                Location = new Point(12, 14),
                Size = new Size(46, 46),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            Image img = LoadItemImage(item, out string categoryName);
            pic.Image = img ?? UIHelper.GeneratePlaceholderFoodImage(46, 46, item.ItemName ?? "", categoryName, includeLabel: false);

            Label lblName = new Label
            {
                Text = item.DisplayTitle,
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(68, 12),
                Size = new Size(Math.Max(150, rowWidth - 360), 22),
                AutoEllipsis = true,
                BackColor = Color.White,
                UseMnemonic = false
            };

            string details = string.Empty;
            if (!string.IsNullOrEmpty(item.SelectedDoughSize)) details += item.SelectedDoughSize;
            if (!string.IsNullOrEmpty(item.SelectedAddOns))
                details += (string.IsNullOrEmpty(details) ? "" : " • ") + item.SelectedAddOns;
            if (string.IsNullOrEmpty(details))
                details = $"₱{item.UnitPrice:N2} each";
            else
                details += $"  (₱{item.UnitPrice:N2} ea)";

            Label lblOpts = new Label
            {
                Text = details,
                Font = new Font("Segoe UI", 8.25f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(68, 36),
                Size = new Size(Math.Max(150, rowWidth - 360), 22),
                AutoEllipsis = true,
                BackColor = Color.White,
                UseMnemonic = false
            };

            int rightX = rowWidth - 12;

            RoundedButton btnRemove = new RoundedButton
            {
                Text = "×",
                Size = new Size(28, 28),
                Location = new Point(rightX - 28, 23),
                BorderRadius = 7,
                BackColor = Color.White,
                NormalColor = AppColors.StatusCancelledBg,
                HoverColor = Color.FromArgb(254, 202, 202),
                PressedColor = Color.FromArgb(252, 165, 165),
                ForeColor = AppColors.StatusCancelled,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRemove.Click += (s, e) =>
            {
                _items.Remove(item);
                _orderService.RemoveFromCart(item);
                RenderOrderRows();
            };

            Label lblLineTotal = new Label
            {
                Text = $"₱{item.Subtotal:N2}",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(btnRemove.Left - 86, 23),
                Size = new Size(78, 28),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.White,
                UseMnemonic = false
            };

            RoundedButton btnEdit = new RoundedButton
            {
                Text = "Edit",
                Size = new Size(54, 28),
                Location = new Point(lblLineTotal.Left - 62, 23),
                BorderRadius = 7,
                BackColor = Color.White,
                NormalColor = AppColors.PrimarySoft,
                HoverColor = AppColors.PrimaryBorder,
                PressedColor = AppColors.PrimaryBorder,
                ForeColor = AppColors.PrimaryDark,
                Font = new Font("Segoe UI Semibold", 8.25f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEdit.Click += (s, e) => EditOrderItem(item);

            RoundedButton btnPlus = new RoundedButton
            {
                Text = "+",
                Size = new Size(26, 26),
                Location = new Point(btnEdit.Left - 36, 24),
                BorderRadius = 6,
                BackColor = Color.White,
                NormalColor = AppColors.SurfaceAlt,
                HoverColor = AppColors.Border,
                PressedColor = AppColors.Border,
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            Label lblQty = new Label
            {
                Text = item.Quantity.ToString(),
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(btnPlus.Left - 28, 24),
                Size = new Size(28, 26),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White,
                UseMnemonic = false
            };

            RoundedButton btnMinus = new RoundedButton
            {
                Text = "-",
                Size = new Size(26, 26),
                Location = new Point(lblQty.Left - 26, 24),
                BorderRadius = 6,
                BackColor = Color.White,
                NormalColor = AppColors.SurfaceAlt,
                HoverColor = AppColors.Border,
                PressedColor = AppColors.Border,
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnMinus.Click += (s, e) =>
            {
                if (item.Quantity <= 1)
                {
                    _items.Remove(item);
                    _orderService.RemoveFromCart(item);
                    RenderOrderRows();
                }
                else
                {
                    _orderService.UpdateQuantity(item, item.Quantity - 1);
                    lblQty.Text = item.Quantity.ToString();
                    lblLineTotal.Text = $"₱{item.Subtotal:N2}";
                    UpdateTotals();
                }
            };

            btnPlus.Click += (s, e) =>
            {
                int nextQty = item.Quantity + 1;
                if (item.ItemId.HasValue)
                {
                    try
                    {
                        var menuItem = _menuRepo.GetMenuItemById(item.ItemId.Value);
                        if (menuItem != null)
                        {
                            int avail = StockService.Instance.GetAvailableStockForMenuItem(menuItem, item.SelectedDoughSize);
                            if (nextQty > avail)
                            {
                                MessageBox.Show(this, $"Only {avail} portion(s) of '{item.DisplayTitle}' are available in stock right now.", "Stock Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                    catch { }
                }

                _orderService.UpdateQuantity(item, nextQty);
                lblQty.Text = item.Quantity.ToString();
                lblLineTotal.Text = $"₱{item.Subtotal:N2}";
                UpdateTotals();
            };

            row.Controls.Add(pic);
            row.Controls.Add(lblName);
            row.Controls.Add(lblOpts);
            row.Controls.Add(btnMinus);
            row.Controls.Add(lblQty);
            row.Controls.Add(btnPlus);
            row.Controls.Add(btnEdit);
            row.Controls.Add(lblLineTotal);
            row.Controls.Add(btnRemove);

            return row;
        }

        private void EditOrderItem(OrderItem item)
        {
            try
            {
                MenuItem menuItem = null;
                if (item.ItemId.HasValue)
                {
                    menuItem = _menuRepo.GetAllMenuItems().FirstOrDefault(m => m.ItemId == item.ItemId.Value);
                }
                if (menuItem == null)
                {
                    menuItem = new MenuItem
                    {
                        ItemId = item.ItemId ?? 0,
                        Name = item.ItemName,
                        Price = item.UnitPrice,
                        Description = "Customized item"
                    };
                }

                using (var dlg = new ItemCustomizationDialog(menuItem, item))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        if (dlg.WasRemoved)
                        {
                            _items.Remove(item);
                            _orderService.RemoveFromCart(item);
                        }
                        else if (dlg.ResultOrderItem != null)
                        {
                            int idx = _items.IndexOf(item);
                            int cartIdx = _orderService.Cart.IndexOf(item);
                            if (cartIdx >= 0) _orderService.Cart[cartIdx] = dlg.ResultOrderItem;
                            if (idx >= 0) _items[idx] = dlg.ResultOrderItem;
                            _orderService.NotifyCartUpdated();
                        }
                        RenderOrderRows();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Unable to open customization options for this item.", ex, "Edit Item", this);
            }
        }

        private Image LoadItemImage(OrderItem item, out string categoryName)
        {
            categoryName = null;
            if (!string.IsNullOrEmpty(item.ImageBase64))
            {
                try
                {
                    byte[] b = Convert.FromBase64String(item.ImageBase64);
                    using (var ms = new System.IO.MemoryStream(b))
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
                    var menuItem = _menuRepo.GetMenuItemById(item.ItemId.Value);
                    if (menuItem != null)
                    {
                        categoryName = menuItem.CategoryName;
                        return menuItem.GetImage();
                    }
                }
                catch { }
            }

            return null;
        }

        private void UpdateTotals()
        {
            decimal subtotal = _items.Sum(i => i.Subtotal);
            decimal fee = _items.Count > 0 ? _orderService.ServiceFee : 0m;
            decimal discount = _items.Count > 0 ? _orderService.DiscountAmount : 0m;
            decimal total = Math.Max(0m, subtotal + fee - discount);

            _lblSubtotal.Text = $"₱{subtotal:N2}";
            _lblServiceFee.Text = $"₱{fee:N2}";
            _lblDiscount.Text = $"-₱{discount:N2}";
            _lblTotal.Text = $"₱{total:N2}";

            bool hasItems = _items.Count > 0;
            _btnConfirm.Enabled = hasItems;
            _lblEmptyWarning.Visible = !hasItems;
        }
    }
}
