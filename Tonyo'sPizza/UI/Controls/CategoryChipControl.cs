using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Controls
{
    public class CategoryChipControl : Control
    {
        private readonly Category _category;
        private bool _isSelected = false;
        private bool _isHovered = false;
        private bool _isPressed = false;
        private readonly string _displayName;
        private readonly int _borderRadius = 10;
        private float _hoverProgress = 0f;
        private float _targetHover = 0f;
        private Timer _animTimer;

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
            _displayName = System.Text.RegularExpressions.Regex.Replace(rawName ?? "All Items", @"\s+", " ").Trim();

            if (_displayName.Length > 2 && _displayName == _displayName.ToUpperInvariant())
            {
                _displayName = char.ToUpper(_displayName[0]) + _displayName.Substring(1).ToLower();
            }

            Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            int textWidth = TextRenderer.MeasureText(_displayName, Font).Width;
            int calculatedWidth = Math.Max(88, textWidth + 30);

            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(calculatedWidth, 36);
            Margin = new Padding(0, 2, 8, 2);
            BackColor = AppColors.Background;
            Cursor = Cursors.Hand;
            DoubleBuffered = true;

            _animTimer = new Timer { Interval = 16 };
            _animTimer.Tick += (s, e) =>
            {
                float step = 0.24f;
                if (_hoverProgress < _targetHover) _hoverProgress = Math.Min(_targetHover, _hoverProgress + step);
                else if (_hoverProgress > _targetHover) _hoverProgress = Math.Max(_targetHover, _hoverProgress - step);
                Invalidate();
                if (Math.Abs(_hoverProgress - _targetHover) < 0.01f)
                {
                    _hoverProgress = _targetHover;
                    _animTimer.Stop();
                }
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _animTimer != null)
            {
                _animTimer.Stop();
                _animTimer.Dispose();
                _animTimer = null;
            }
            base.Dispose(disposing);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            _targetHover = 1f;
            if (!_animTimer.Enabled) _animTimer.Start();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            _isPressed = false;
            _targetHover = 0f;
            if (!_animTimer.Enabled) _animTimer.Start();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left) { _isPressed = true; Invalidate(); }
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
            else if (_isPressed)
            {
                bgColor = AppColors.SecondaryPressed;
                borderColor = AppColors.PrimaryBorder;
                textColor = AppColors.TextDark;
            }
            else
            {
                bgColor = AppColors.Lerp(Color.White, AppColors.PrimarySoft, _hoverProgress);
                borderColor = AppColors.Lerp(AppColors.Border, AppColors.PrimaryBorder, _hoverProgress);
                textColor = _isHovered ? AppColors.PrimaryDark : AppColors.TextMedium;
            }

            using (GraphicsPath path = UIHelper.GetRoundedRectanglePath(rect, _borderRadius))
            {
                using (SolidBrush brush = new SolidBrush(bgColor)) g.FillPath(brush, path);
                using (Pen pen = new Pen(borderColor, 1.2f) { Alignment = PenAlignment.Inset }) g.DrawPath(pen, path);
            }

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
