using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Controls
{
    public class RoundedPanel : Panel
    {
        private int _borderRadius = 16;
        private Color _borderColor = AppColors.Border;
        private int _borderWidth = 1;
        private bool _drawBorder = true;
        private Color _gradientStartColor = Color.Empty;
        private Color _gradientEndColor = Color.Empty;
        private float _gradientAngle = 90f;

        [Category("Appearance")]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = Math.Max(0, value); UpdateRegion(); Invalidate(); }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public int BorderWidth
        {
            get => _borderWidth;
            set { _borderWidth = Math.Max(0, value); Invalidate(); }
        }

        [Category("Appearance")]
        public bool DrawBorder
        {
            get => _drawBorder;
            set { _drawBorder = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color GradientStartColor
        {
            get => _gradientStartColor;
            set { _gradientStartColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color GradientEndColor
        {
            get => _gradientEndColor;
            set { _gradientEndColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public float GradientAngle
        {
            get => _gradientAngle;
            set { _gradientAngle = value; Invalidate(); }
        }

        public RoundedPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            UpdateRegion();
        }

        private void UpdateRegion()
        {
            // We do smooth GDI+ anti-aliased drawing in OnPaint against the parent background,
            // so we don't apply a jagged 1-bit window Region which causes stair-stepped pixelated edges.
            Region = null;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            UIHelper.SetHighQuality(g);

            // Paint parent background first to ensure no black edges
            Color parentBg = Parent != null ? Parent.BackColor : AppColors.Background;
            using (SolidBrush parentBrush = new SolidBrush(parentBg))
            {
                g.FillRectangle(parentBrush, ClientRectangle);
            }

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using (GraphicsPath path = UIHelper.GetRoundedRectanglePath(rect, _borderRadius))
            {
                // Fill Background
                if (_gradientStartColor != Color.Empty && _gradientEndColor != Color.Empty)
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(rect, _gradientStartColor, _gradientEndColor, _gradientAngle))
                    {
                        g.FillPath(brush, path);
                    }
                }
                else
                {
                    using (SolidBrush brush = new SolidBrush(BackColor))
                    {
                        g.FillPath(brush, path);
                    }
                }

                // Draw Border
                if (_drawBorder && _borderWidth > 0)
                {
                    using (Pen pen = new Pen(_borderColor, _borderWidth))
                    {
                        pen.Alignment = PenAlignment.Inset;
                        g.DrawPath(pen, path);
                    }
                }
            }
        }
    }
}
