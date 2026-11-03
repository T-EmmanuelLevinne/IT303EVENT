using System;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Controls
{
    public class CartItemControl : UserControl
    {
        private readonly OrderItem _item;
        private readonly CheckBox _chkSelect;
        private readonly PictureBox _picThumb;
        private readonly Label _lblName;
        private readonly Label _lblDetails;
        private readonly Label _lblQtyDisplay;
        private readonly Label _lblPrice;
        private readonly Button _btnMinus;
        private readonly Button _btnPlus;
        private readonly RoundedButton _btnEdit;
        private readonly Button _btnRemove;
        private bool _isHovered = false;

        public OrderItem Item => _item;

        public bool IsSelected
        {
            get => _chkSelect.Checked;
            set => _chkSelect.Checked = value;
        }

        public event Action<OrderItem, int> QuantityChanged;
        public event Action<OrderItem> ItemRemoved;
        public event Action<OrderItem> EditRequested;
        public event EventHandler SelectionChanged;

        public CartItemControl(OrderItem item)
        {
            _item = item;

            Size = new Size(275, 84);
            BackColor = Color.White;
            Margin = new Padding(0, 0, 0, 8);
            Cursor = Cursors.Hand;

            // Selection Checkbox
            _chkSelect = new CheckBox
            {
                Location = new Point(6, 32),
                Size = new Size(18, 18),
                Checked = true,
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            _chkSelect.CheckedChanged += (s, e) => SelectionChanged?.Invoke(this, EventArgs.Empty);

            // Thumbnail
            _picThumb = new PictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(28, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(254, 242, 242),
                Cursor = Cursors.Hand
            };

            Image thumbImg = null;
            if (!string.IsNullOrEmpty(item.ImageBase64))
            {
                try
                {
                    byte[] b = Convert.FromBase64String(item.ImageBase64);
                    using (var ms = new System.IO.MemoryStream(b))
                    using (var temp = Image.FromStream(ms))
                    {
                        thumbImg = new Bitmap(temp);
                    }
                }
                catch { }
            }

            if (thumbImg == null && item.ItemId.HasValue)
            {
                try
                {
                    var repo = new Tonyo_sPizza.Data.Repositories.MenuItemRepository();
                    var menuItem = repo.GetMenuItemById(item.ItemId.Value);
                    thumbImg = menuItem?.GetImage();
                }
                catch { }
            }

            _picThumb.Image = thumbImg ?? UIHelper.GeneratePlaceholderPizzaImage(40, 40, item.ItemName ?? "");

            // Food Name
            _lblName = new Label
            {
                Text = item.DisplayTitle,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(74, 8),
                Size = new Size(115, 18),
                AutoEllipsis = true,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            // Dough / Addons Details line
            string detailText = "";
            if (!string.IsNullOrEmpty(item.SelectedDoughSize)) detailText += item.SelectedDoughSize;
            if (!string.IsNullOrEmpty(item.SelectedAddOns))
            {
                if (detailText.Length > 0) detailText += " • ";
                detailText += item.SelectedAddOns;
            }
            if (string.IsNullOrEmpty(detailText)) detailText = "Standard Option";

            _lblDetails = new Label
            {
                Text = detailText,
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(74, 28),
                Size = new Size(115, 16),
                AutoEllipsis = true,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            // Price
            _lblPrice = new Label
            {
                Text = $"₱{item.Subtotal:N2}",
                Font = new Font("Segoe UI Bold", 10f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryDark,
                Location = new Point(175, 8),
                Size = new Size(70, 18),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            // Remove Button
            _btnRemove = new Button
            {
                Text = "✕",
                Size = new Size(22, 22),
                Location = new Point(248, 7),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Bold", 8f, FontStyle.Bold),
                ForeColor = Color.FromArgb(239, 68, 68),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            _btnRemove.FlatAppearance.BorderSize = 0;
            _btnRemove.Click += (s, e) => ItemRemoved?.Invoke(_item);

            // Bottom controls row (Qty & Edit Button)
            int rowY = 50;

            _lblQtyDisplay = new Label
            {
                Text = $"Qty: {item.Quantity}",
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(74, rowY + 3),
                Size = new Size(44, 18),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            _btnMinus = new Button
            {
                Text = "-",
                Size = new Size(20, 20),
                Location = new Point(122, rowY + 1),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Bold", 8, FontStyle.Bold),
                ForeColor = AppColors.TextMedium,
                BackColor = AppColors.Background,
                Cursor = Cursors.Hand
            };
            _btnMinus.FlatAppearance.BorderSize = 0;
            _btnMinus.Click += (s, e) => QuantityChanged?.Invoke(_item, _item.Quantity - 1);

            _btnPlus = new Button
            {
                Text = "+",
                Size = new Size(20, 20),
                Location = new Point(146, rowY + 1),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Bold", 8, FontStyle.Bold),
                ForeColor = AppColors.PrimaryDark,
                BackColor = AppColors.PrimarySoft,
                Cursor = Cursors.Hand
            };
            _btnPlus.FlatAppearance.BorderSize = 0;
            _btnPlus.Click += (s, e) =>
            {
                int maxStock = 99;
                if (_item.ItemId.HasValue)
                {
                    try
                    {
                        var repo = new Tonyo_sPizza.Data.Repositories.MenuItemRepository();
                        var menuItem = repo.GetMenuItemById(_item.ItemId.Value);
                        if (menuItem != null)
                        {
                            maxStock = Tonyo_sPizza.Services.StockService.Instance.GetAvailableStockForMenuItem(menuItem, _item.SelectedDoughSize);
                        }
                    }
                    catch { }
                }

                if (_item.Quantity < maxStock)
                {
                    QuantityChanged?.Invoke(_item, _item.Quantity + 1);
                }
                else
                {
                    MessageBox.Show($"Cannot increase quantity. Maximum available stock is {maxStock}.", "Stock Limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            _btnEdit = new RoundedButton
            {
                Text = "Edit",
                IconName = "edit",
                Size = new Size(68, 24),
                Location = new Point(198, rowY - 1),
                BorderRadius = 6,
                NormalColor = AppColors.PrimarySoft,
                HoverColor = AppColors.PrimaryBorder,
                ForeColor = AppColors.PrimaryDark,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnEdit.Click += (s, e) => EditRequested?.Invoke(_item);

            // Click anywhere on card (except +/- and remove) to trigger Edit
            Action triggerEdit = () => EditRequested?.Invoke(_item);
            this.Click += (s, e) => triggerEdit();
            _picThumb.Click += (s, e) => triggerEdit();
            _lblName.Click += (s, e) => triggerEdit();
            _lblDetails.Click += (s, e) => triggerEdit();
            _lblPrice.Click += (s, e) => triggerEdit();
            _lblQtyDisplay.Click += (s, e) => triggerEdit();

            // Right-click context menu
            var menu = UIHelper.CreateModernContextMenu();
            var mnuEdit = UIHelper.CreateModernMenuItem("Edit Customization", "edit", (s, e) => EditRequested?.Invoke(_item));
            var mnuRemove = UIHelper.CreateModernMenuItem("Remove from Cart", "delete", (s, e) => ItemRemoved?.Invoke(_item), Color.FromArgb(220, 38, 38));
            menu.Items.Add(mnuEdit);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(mnuRemove);
            this.ContextMenuStrip = menu;
            _picThumb.ContextMenuStrip = menu;
            _lblName.ContextMenuStrip = menu;
            _lblDetails.ContextMenuStrip = menu;

            // Hover effects
            EventHandler onEnter = (s, e) => { _isHovered = true; Invalidate(); };
            EventHandler onLeave = (s, e) => { _isHovered = false; Invalidate(); };
            this.MouseEnter += onEnter;
            this.MouseLeave += onLeave;
            _picThumb.MouseEnter += onEnter;
            _picThumb.MouseLeave += onLeave;
            _lblName.MouseEnter += onEnter;
            _lblName.MouseLeave += onLeave;
            _lblDetails.MouseEnter += onEnter;
            _lblDetails.MouseLeave += onLeave;

            ToolTip tip = new ToolTip();
            tip.SetToolTip(this, "Click to customize pizza size, add-ons or quantity");
            tip.SetToolTip(_lblName, "Click to customize pizza size, add-ons or quantity");
            tip.SetToolTip(_picThumb, "Click to customize pizza size, add-ons or quantity");
            tip.SetToolTip(_btnEdit, "Edit pizza size, add-ons or options");
            tip.SetToolTip(_btnRemove, "Remove this pizza from your cart");

            Controls.Add(_chkSelect);
            Controls.Add(_picThumb);
            Controls.Add(_lblName);
            Controls.Add(_lblDetails);
            Controls.Add(_lblPrice);
            Controls.Add(_btnRemove);
            Controls.Add(_lblQtyDisplay);
            Controls.Add(_btnMinus);
            Controls.Add(_btnPlus);
            Controls.Add(_btnEdit);

            UpdateLayout();
            Resize += (s, e) => UpdateLayout();
        }

        private void UpdateLayout()
        {
            _btnRemove.Left = Width - 28;
            _lblPrice.Left = Width - 98;
            _btnEdit.Left = Width - 76;
            int textW = Math.Max(60, Width - 170);
            _lblName.Width = textW;
            _lblDetails.Width = textW;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            UIHelper.SetHighQuality(e.Graphics);

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            Color borderColor = _isHovered ? AppColors.Primary : AppColors.Border;
            Color bgColor = _isHovered ? Color.FromArgb(255, 250, 245) : Color.White;

            using (var path = UIHelper.GetRoundedRectanglePath(rect, 8))
            {
                using (var brush = new SolidBrush(bgColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                using (var pen = new Pen(borderColor, _isHovered ? 1.5f : 1f))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
    }
}
