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

            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            Size = new Size(224, 256);
            Margin = new Padding(0, 0, 16, 16);
            BackColor = AppColors.Background;

            _cardPanel = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 12,
                BackColor = AppColors.CardBackground,
                BorderColor = AppColors.Border,
                DrawBorder = true,
                BorderWidth = 1,
                Padding = new Padding(10)
            };

            _picFood = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 128,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(253, 246, 238)
            };

            Image img = _item.GetImage();
            _picFood.Image = img ?? UIHelper.GeneratePlaceholderFoodImage(200, 128, _item.Name, _item.CategoryName, includeLabel: false);

            _lblTitle = new Label
            {
                Text = _item.Name,
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Dock = DockStyle.Top,
                Height = 38,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                UseMnemonic = false,
                Padding = new Padding(2, 4, 0, 0)
            };

            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                BackColor = Color.White
            };

            Panel pricePanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 110,
                BackColor = Color.White
            };

            _lblPrice = new Label
            {
                Text = $"₱{_item.Price:N2}",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblOldPrice = new Label
            {
                Text = _item.OldPrice.HasValue ? $"₱{_item.OldPrice.Value:N2}" : string.Empty,
                Font = new Font("Segoe UI", 8.25f, FontStyle.Strikeout),
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
                    Text = "Out of Stock",
                    Font = new Font("Segoe UI Semibold", 7.75f, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = AppColors.StatusCancelled,
                    AutoSize = true,
                    Padding = new Padding(6, 2, 6, 2),
                    Location = new Point(8, 8)
                };
                _picFood.Controls.Add(lblOutOfStockBadge);
            }
            else if (availableStock <= 5)
            {
                Label lblLowStockBadge = new Label
                {
                    Text = $"Only {availableStock} left",
                    Font = new Font("Segoe UI Semibold", 7.75f, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = AppColors.StatusPending,
                    AutoSize = true,
                    Padding = new Padding(6, 2, 6, 2),
                    Location = new Point(8, 8)
                };
                _picFood.Controls.Add(lblLowStockBadge);
            }

            _btnOrder = new RoundedButton
            {
                Text = isOutOfStock ? "Unavailable" : "Order",
                Dock = DockStyle.Right,
                Width = isOutOfStock ? 86 : 74,
                Height = 36,
                BorderRadius = 8,
                NormalColor = isOutOfStock ? AppColors.SurfaceAlt : AppColors.Primary,
                HoverColor = isOutOfStock ? AppColors.Border : AppColors.PrimaryLight,
                PressedColor = isOutOfStock ? AppColors.Border : AppColors.PrimaryDark,
                ForeColor = isOutOfStock ? AppColors.TextLight : Color.White,
                Font = new Font("Segoe UI Semibold", isOutOfStock ? 8f : 9f, FontStyle.Bold)
            };

            Action trigger;
            if (isOutOfStock)
            {
                trigger = () => MessageBox.Show($"'{_item.Name}' is currently out of stock due to unavailable ingredients.", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            EventHandler hoverEnter = (s, e) =>
            {
                if (!isOutOfStock)
                {
                    _cardPanel.BorderColor = AppColors.PrimaryBorder;
                    _cardPanel.Invalidate();
                }
            };
            EventHandler hoverLeave = (s, e) =>
            {
                _cardPanel.BorderColor = AppColors.Border;
                _cardPanel.Invalidate();
            };

            foreach (Control c in new Control[] { _cardPanel, _picFood, _lblTitle, _lblPrice, bottomPanel, this })
            {
                c.MouseEnter += hoverEnter;
                c.MouseLeave += hoverLeave;
            }

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
