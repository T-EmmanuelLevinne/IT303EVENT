using System;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.UI.Theme;
using MenuItem = Tonyo_sPizza.Models.MenuItem;

namespace Tonyo_sPizza.UI.Controls
{
    public class FoodCardControl : UserControl
    {
        private readonly MenuItem _item;
        private readonly RoundedPanel _cardPanel;
        private readonly PictureBox _picFood;
        private readonly Label _lblTitle;
        private readonly Label _lblPrice;
        private readonly Label _lblOldPrice;
        private readonly RoundedButton _btnOrder;

        public MenuItem Item => _item;

        public event Action<MenuItem> OrderClicked;

        public FoodCardControl(MenuItem item)
        {
            _item = item;

            Size = new Size(220, 250);
            Margin = new Padding(0, 0, 16, 16);
            BackColor = AppColors.Background;

            _cardPanel = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 14,
                BackColor = AppColors.CardBackground,
                BorderColor = AppColors.Border,
                DrawBorder = true,
                BorderWidth = 1,
                Padding = new Padding(10)
            };

            // Food Picture
            _picFood = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 125,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(254, 242, 242)
            };

            Image img = _item.GetImage();
            if (img != null)
            {
                _picFood.Image = img;
            }
            else
            {
                _picFood.Image = UIHelper.GeneratePlaceholderPizzaImage(200, 125, _item.Name);
            }

            // Title
            _lblTitle = new Label
            {
                Text = _item.Name,
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Dock = DockStyle.Top,
                Height = 36,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Padding = new Padding(2, 4, 0, 0)
            };

            // Bottom Action Panel (Price + Order Button)
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                BackColor = Color.Transparent
            };

            // Price Labels container
            Panel pricePanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 110,
                BackColor = Color.Transparent
            };

            _lblPrice = new Label
            {
                Text = $"₱{_item.Price:N2}",
                Font = new Font("Segoe UI Bold", 10.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblOldPrice = new Label
            {
                Text = _item.OldPrice.HasValue ? $"₱{_item.OldPrice.Value:N2}" : string.Empty,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Strikeout),
                ForeColor = AppColors.TextLight,
                Dock = DockStyle.Bottom,
                Height = 18,
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = _item.OldPrice.HasValue && _item.OldPrice.Value > _item.Price
            };

            pricePanel.Controls.Add(_lblPrice);
            pricePanel.Controls.Add(_lblOldPrice);

            int availableStock = Services.StockService.Instance.GetAvailableStockForMenuItem(_item);
            bool isOutOfStock = availableStock <= 0;

            if (isOutOfStock)
            {
                Label lblOutOfStockBadge = new Label
                {
                    Text = "OUT OF STOCK",
                    Font = new Font("Segoe UI Bold", 8f, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(220, 38, 38),
                    AutoSize = true,
                    Padding = new Padding(6, 3, 6, 3),
                    Location = new Point(6, 6)
                };
                _picFood.Controls.Add(lblOutOfStockBadge);
            }
            else if (availableStock <= 5)
            {
                Label lblLowStockBadge = new Label
                {
                    Text = $"Only {availableStock} left!",
                    Font = new Font("Segoe UI Bold", 7.5f, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(217, 119, 6),
                    AutoSize = true,
                    Padding = new Padding(5, 2, 5, 2),
                    Location = new Point(6, 6)
                };
                _picFood.Controls.Add(lblLowStockBadge);
            }

            // Order Button
            _btnOrder = new RoundedButton
            {
                Text = isOutOfStock ? "Out of Stock" : "Order",
                Dock = DockStyle.Right,
                Width = isOutOfStock ? 84 : 72,
                Height = 36,
                BorderRadius = 8,
                NormalColor = isOutOfStock ? Color.FromArgb(241, 245, 249) : AppColors.Primary,
                HoverColor = isOutOfStock ? Color.FromArgb(226, 232, 240) : AppColors.PrimaryLight,
                PressedColor = isOutOfStock ? Color.FromArgb(203, 213, 225) : AppColors.PrimaryDark,
                ForeColor = isOutOfStock ? Color.FromArgb(148, 163, 184) : Color.White,
                Font = new Font("Segoe UI Semibold", isOutOfStock ? 8f : 9f, FontStyle.Bold)
            };

            Action trigger;
            if (isOutOfStock)
            {
                trigger = () => MessageBox.Show($"Sorry, '{_item.Name}' is currently out of stock due to unavailable ingredients.", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                trigger = () => OrderClicked?.Invoke(_item);
            }

            _btnOrder.Click += (s, e) => trigger();
            _picFood.Click += (s, e) => trigger();
            _lblTitle.Click += (s, e) => trigger();
            _lblPrice.Click += (s, e) => trigger();
            _cardPanel.Click += (s, e) => trigger();
            this.Click += (s, e) => trigger();

            _picFood.Cursor = Cursors.Hand;
            _lblTitle.Cursor = Cursors.Hand;
            _cardPanel.Cursor = Cursors.Hand;
            this.Cursor = Cursors.Hand;

            bottomPanel.Controls.Add(pricePanel);
            bottomPanel.Controls.Add(_btnOrder);

            _cardPanel.Controls.Add(bottomPanel);
            _cardPanel.Controls.Add(_lblTitle);
            _cardPanel.Controls.Add(_picFood);

            Controls.Add(_cardPanel);
        }
    }
}
