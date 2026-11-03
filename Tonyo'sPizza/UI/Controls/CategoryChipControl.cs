using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Controls
{
    public class CategoryChipControl : Control
    {
        private Category _category;
        private bool _isSelected = false;
        private bool _isHovered = false;
        private bool _isPressed = false;
        private readonly string _displayName;
        private int _borderRadius = 10;

        public Category Category => _category;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    Invalidate();
                }
            }
        }

        public event EventHandler CategoryClicked;

        public CategoryChipControl(Category category, bool isSelected = false)
        {
            _category = category;
            _isSelected = isSelected;

            string rawName = category != null ? category.CategoryName : "All Items";
            // Clean up multiple spaces (e.g., "Sides  Appetizers" -> "Sides Appetizers")
            _displayName = Regex.Replace(rawName ?? "All Items", @"\s+", " ").Trim();

            // Format all-caps (e.g., "LOVEY" -> "Lovey") for elegant typography
            if (_displayName.Length > 2 && _displayName == _displayName.ToUpperInvariant())
            {
                _displayName = char.ToUpper(_displayName[0]) + _displayName.Substring(1).ToLower();
            }

            Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);

            int textWidth = TextRenderer.MeasureText(_displayName, Font).Width;
            int calculatedWidth = Math.Max(84, textWidth + 28);

            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(calculatedWidth, 36);
            Margin = new Padding(0, 2, 8, 2);
            BackColor = Color.FromArgb(248, 250, 252);
            Cursor = Cursors.Hand;
            DoubleBuffered = true;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            _isPressed = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                _isPressed = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isPressed = false;
            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            CategoryClicked?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            UIHelper.SetHighQuality(g);

            // 1. Blend with parent background to eliminate jagged edges
            Color parentBg = Parent != null ? Parent.BackColor : AppColors.Background;
            using (SolidBrush parentBrush = new SolidBrush(parentBg))
            {
                g.FillRectangle(parentBrush, ClientRectangle);
            }

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            Color bgColor;
            Color borderColor;
            Color textColor;

            if (_isSelected)
            {
                bgColor = _isPressed ? AppColors.PrimaryDark : AppColors.Primary;
                borderColor = bgColor;
                textColor = Color.White;
            }
            else
            {
                if (_isPressed)
                {
                    bgColor = Color.FromArgb(241, 245, 249);
                    borderColor = Color.FromArgb(203, 213, 225);
                    textColor = AppColors.TextDark;
                }
                else if (_isHovered)
                {
                    bgColor = Color.FromArgb(248, 250, 252);
                    borderColor = Color.FromArgb(203, 213, 225);
                    textColor = AppColors.Primary;
                }
                else
                {
                    bgColor = Color.White;
                    borderColor = Color.FromArgb(226, 232, 240);
                    textColor = Color.FromArgb(51, 65, 85);
                }
            }

            using (GraphicsPath path = UIHelper.GetRoundedRectanglePath(rect, _borderRadius))
            {
                // Fill button body
                using (SolidBrush brush = new SolidBrush(bgColor))
                {
                    g.FillPath(brush, path);
                }

                // Draw Border
                using (Pen pen = new Pen(borderColor, 1.2f))
                {
                    pen.Alignment = PenAlignment.Inset;
                    g.DrawPath(pen, path);
                }
            }

            // Draw Centered Text
            TextRenderer.DrawText(
                g,
                _displayName,
                Font,
                rect,
                textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix
            );
        }
    }
}
