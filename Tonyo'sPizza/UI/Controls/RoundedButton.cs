using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Controls
{
    public class RoundedButton : Button
    {
        private int _borderRadius = 12;
        private Color _normalColor = AppColors.Primary;
        private Color _hoverColor = AppColors.PrimaryLight;
        private Color _pressedColor = AppColors.PrimaryDark;
        private Color _borderColor = Color.Transparent;
        private int _borderWidth = 0;
        private bool _isHovered = false;
        private bool _isPressed = false;
        private string _iconName = null;

        [Category("Appearance")]
        public string IconName
        {
            get => _iconName;
            set
            {
                _iconName = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color NormalColor
        {
            get => _normalColor;
            set
            {
                _normalColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color HoverColor
        {
            get => _hoverColor;
            set
            {
                _hoverColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color PressedColor
        {
            get => _pressedColor;
            set
            {
                _pressedColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color CustomBorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int CustomBorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = Math.Max(0, value);
                Invalidate();
            }
        }

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            BackColor = Color.Transparent;
            ForeColor = Color.White;
            Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            Cursor = Cursors.Hand;
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
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

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (mevent.Button == MouseButtons.Left)
            {
                _isPressed = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            _isPressed = false;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        private void UpdateRegion()
        {
            // We do smooth GDI+ anti-aliased drawing in OnPaint, so we don't apply a jagged 1px window Region
            Region = null;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            UIHelper.SetHighQuality(g);

            // 1. Resolve actual non-transparent parent background
            Color parentBg = AppColors.Background;
            Control p = Parent;
            while (p != null)
            {
                if (p.BackColor != Color.Transparent && p.BackColor.A == 255)
                {
                    parentBg = p.BackColor;
                    break;
                }
                p = p.Parent;
            }

            using (SolidBrush parentBrush = new SolidBrush(parentBg))
            {
                g.FillRectangle(parentBrush, ClientRectangle);
            }

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            Color currentBg = _normalColor;
            if (!Enabled)
            {
                currentBg = Color.FromArgb(226, 232, 240); // Disabled soft gray
            }
            else if (_isPressed)
            {
                currentBg = _pressedColor;
            }
            else if (_isHovered)
            {
                currentBg = _hoverColor;
            }

            using (GraphicsPath path = UIHelper.GetRoundedRectanglePath(rect, _borderRadius))
            {
                // Fill button body
                using (SolidBrush brush = new SolidBrush(currentBg))
                {
                    g.FillPath(brush, path);
                }

                // Draw border if configured
                if (_borderWidth > 0 && _borderColor != Color.Transparent)
                {
                    using (Pen pen = new Pen(_borderColor, _borderWidth))
                    {
                        pen.Alignment = PenAlignment.Inset;
                        g.DrawPath(pen, path);
                    }
                }
            }

            // Draw Text & Icon / Image
            Color textCol = Enabled ? ForeColor : Color.FromArgb(148, 163, 184);
            int textX = 0;
            int textWidth = Width;

            if (!string.IsNullOrEmpty(_iconName))
            {
                int iconSize = 16;
                int iconY = (Height - iconSize) / 2;
                int iconX = 12;
                UIHelper.DrawVectorIcon(g, _iconName, new Rectangle(iconX, iconY, iconSize, iconSize), textCol);
                textX = iconX + iconSize + 8;
                textWidth = Width - textX - 6;
            }
            else if (Image != null)
            {
                int imgY = (Height - Image.Height) / 2;
                int imgX = 12;
                g.DrawImage(Image, imgX, imgY, Image.Width, Image.Height);
                textX = imgX + Image.Width + 8;
                textWidth = Width - textX - 6;
            }

            Rectangle textRect = new Rectangle(textX, 0, Math.Max(0, textWidth), Height);

            using (SolidBrush textBrush = new SolidBrush(textCol))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = (!string.IsNullOrEmpty(_iconName) || Image != null) ? StringAlignment.Near : StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter,
                    FormatFlags = StringFormatFlags.NoWrap
                };
                g.DrawString(Text, Font, textBrush, textRect, sf);
            }
        }
    }
}
