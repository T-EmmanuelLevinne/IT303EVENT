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
        private int _borderRadius = 8;
        private Color _normalColor = AppColors.Primary;
        private Color _hoverColor = AppColors.PrimaryLight;
        private Color _pressedColor = AppColors.PrimaryDark;
        private Color _borderColor = Color.Transparent;
        private int _borderWidth = 0;
        private bool _isPressed = false;
        private string _iconName = null;
        private bool _showActiveAccentBar = false;

        // Smooth 60fps hover lerp
        private float _hoverProgress = 0f;
        private float _targetHover = 0f;
        private Timer _animTimer;

        [Category("Appearance")]
        public string IconName
        {
            get => _iconName;
            set { _iconName = value; Invalidate(); }
        }

        [Category("Appearance")]
        public bool ShowActiveAccentBar
        {
            get => _showActiveAccentBar;
            set { _showActiveAccentBar = value; Invalidate(); }
        }

        [Category("Appearance")]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("Appearance")]
        public Color NormalColor
        {
            get => _normalColor;
            set { _normalColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color HoverColor
        {
            get => _hoverColor;
            set { _hoverColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color PressedColor
        {
            get => _pressedColor;
            set { _pressedColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color CustomBorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public int CustomBorderWidth
        {
            get => _borderWidth;
            set { _borderWidth = Math.Max(0, value); Invalidate(); }
        }

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            BackColor = Color.Transparent;
            ForeColor = Color.White;
            Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            Cursor = Cursors.Hand;
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);

            _animTimer = new Timer { Interval = 16 };
            _animTimer.Tick += AnimTimer_Tick;
        }

        protected override bool ShowFocusCues => false;

        public override void NotifyDefault(bool value)
        {
            // Prevent Win32 default-button thick black border on custom-painted RoundedButton
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            float step = 0.22f;
            if (_hoverProgress < _targetHover)
            {
                _hoverProgress = Math.Min(_targetHover, _hoverProgress + step);
            }
            else if (_hoverProgress > _targetHover)
            {
                _hoverProgress = Math.Max(_targetHover, _hoverProgress - step);
            }

            Invalidate();

            if (Math.Abs(_hoverProgress - _targetHover) < 0.01f)
            {
                _hoverProgress = _targetHover;
                _animTimer.Stop();
            }
        }

        private void StartHoverAnimation(float target)
        {
            _targetHover = target;
            if (!_animTimer.Enabled)
            {
                _animTimer.Start();
            }
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
            StartHoverAnimation(1f);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isPressed = false;
            StartHoverAnimation(0f);
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
            Region = null;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            UIHelper.SetHighQuality(g);

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

            if (BackColor == Color.Transparent && Parent != null)
            {
                bool needsGradientPaint = Parent.BackColor == Color.Transparent
                    || (Parent is RoundedPanel rp && rp.GradientStartColor != Color.Empty);
                if (needsGradientPaint)
                {
                    try
                    {
                        ButtonRenderer.DrawParentBackground(g, ClientRectangle, this);
                    }
                    catch { }
                }
            }

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            Color currentBg;
            if (!Enabled)
            {
                currentBg = Color.FromArgb(236, 233, 227);
            }
            else if (_isPressed)
            {
                currentBg = _pressedColor;
            }
            else
            {
                currentBg = AppColors.Lerp(_normalColor, _hoverColor, _hoverProgress);
            }

            using (GraphicsPath path = UIHelper.GetRoundedRectanglePath(rect, _borderRadius))
            {
                using (SolidBrush brush = new SolidBrush(currentBg))
                {
                    g.FillPath(brush, path);
                }

                if (_borderWidth > 0 && _borderColor != Color.Transparent)
                {
                    using (Pen pen = new Pen(_borderColor, _borderWidth))
                    {
                        pen.Alignment = PenAlignment.Inset;
                        g.DrawPath(pen, path);
                    }
                }
            }

            // Active sidebar accent strip
            if (_showActiveAccentBar && Height > 16)
            {
                Rectangle barRect = new Rectangle(4, 9, 3, Height - 18);
                using (GraphicsPath barPath = UIHelper.GetRoundedRectanglePath(barRect, 1))
                using (SolidBrush barBrush = new SolidBrush(AppColors.Primary))
                {
                    g.FillPath(barBrush, barPath);
                }
            }

            Color textCol = Enabled ? ForeColor : AppColors.TextLight;
            int textX = _showActiveAccentBar ? 14 : 8;
            int textWidth = Width - textX - 8;
            int yOffset = _isPressed && Enabled ? 1 : 0;

            if (!string.IsNullOrEmpty(_iconName))
            {
                int iconSize = 15;
                int iconY = (Height - iconSize) / 2 + yOffset;
                int iconX = _showActiveAccentBar ? 14 : 12;
                UIHelper.DrawVectorIcon(g, _iconName, new Rectangle(iconX, iconY, iconSize, iconSize), textCol);
                textX = iconX + iconSize + 8;
                textWidth = Width - textX - 6;
            }
            else if (Image != null)
            {
                int imgY = (Height - Image.Height) / 2 + yOffset;
                int imgX = 12;
                g.DrawImage(Image, imgX, imgY, Image.Width, Image.Height);
                textX = imgX + Image.Width + 8;
                textWidth = Width - textX - 6;
            }

            Rectangle textRect = new Rectangle(textX, yOffset, Math.Max(0, textWidth), Height);

            using (SolidBrush textBrush = new SolidBrush(textCol))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = (!string.IsNullOrEmpty(_iconName) || Image != null || _showActiveAccentBar) ? StringAlignment.Near : StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter,
                    FormatFlags = StringFormatFlags.NoWrap
                };
                g.DrawString(Text, Font, textBrush, textRect, sf);
            }
        }
    }
}
