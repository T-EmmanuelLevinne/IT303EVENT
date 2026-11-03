using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Controls
{
    public class ModernInputField : UserControl
    {
        private TextBox _textBox;
        private bool _isFocused = false;
        private bool _isPassword = false;
        private bool _showPassword = false;
        private string _iconName = "user";
        private int _borderRadius = 8;
        private Rectangle _eyeRect;

        public string IconName
        {
            get => _iconName;
            set { _iconName = value; Invalidate(); }
        }

        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        public bool IsPassword
        {
            get => _isPassword;
            set
            {
                _isPassword = value;
                _textBox.UseSystemPasswordChar = _isPassword && !_showPassword;
                AdjustLayout();
                Invalidate();
            }
        }

        public override string Text
        {
            get => _textBox?.Text ?? string.Empty;
            set
            {
                if (_textBox != null)
                {
                    _textBox.Text = value;
                }
            }
        }

        public int MaxLength
        {
            get => _textBox?.MaxLength ?? 32767;
            set
            {
                if (_textBox != null)
                {
                    _textBox.MaxLength = value;
                }
            }
        }

        public TextBox InnerTextBox => _textBox;

        public event EventHandler FieldTextChanged;

        public ModernInputField()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            Size = new Size(380, 42);
            BackColor = Color.FromArgb(248, 250, 252);

            _textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10f),
                BackColor = Color.FromArgb(248, 250, 252),
                ForeColor = AppColors.TextDark
            };

            _textBox.GotFocus += (s, e) =>
            {
                _isFocused = true;
                BackColor = Color.White;
                _textBox.BackColor = Color.White;
                Invalidate();
            };

            _textBox.LostFocus += (s, e) =>
            {
                _isFocused = false;
                BackColor = Color.FromArgb(248, 250, 252);
                _textBox.BackColor = Color.FromArgb(248, 250, 252);
                Invalidate();
            };

            _textBox.TextChanged += (s, e) => FieldTextChanged?.Invoke(this, e);

            Controls.Add(_textBox);
            AdjustLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustLayout();
        }

        private void AdjustLayout()
        {
            if (_textBox == null) return;

            int leftPadding = string.IsNullOrEmpty(_iconName) ? 14 : 38;
            int rightPadding = _isPassword ? 36 : 14;

            int tbW = Math.Max(50, Width - leftPadding - rightPadding);
            int tbH = _textBox.PreferredHeight;
            int tbY = (Height - tbH) / 2;

            _textBox.Location = new Point(leftPadding, tbY);
            _textBox.Width = tbW;

            _eyeRect = new Rectangle(Width - 30, (Height - 16) / 2, 18, 16);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_isPassword && _eyeRect.Contains(e.Location))
            {
                _showPassword = !_showPassword;
                _textBox.UseSystemPasswordChar = _isPassword && !_showPassword;
                Invalidate();
            }
            else
            {
                _textBox.Focus();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            UIHelper.SetHighQuality(g);

            // 1. Draw parent background behind rounded corners
            Color parentBg = Parent != null ? Parent.BackColor : Color.White;
            using (SolidBrush parentBrush = new SolidBrush(parentBg))
            {
                g.FillRectangle(parentBrush, ClientRectangle);
            }

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using (GraphicsPath path = UIHelper.GetRoundedRectanglePath(rect, _borderRadius))
            {
                // 2. Fill background
                Color fillBg = _isFocused ? Color.White : Color.FromArgb(248, 250, 252);
                using (SolidBrush brush = new SolidBrush(fillBg))
                {
                    g.FillPath(brush, path);
                }

                // 3. Draw border
                Color borderColor = _isFocused ? AppColors.Primary : AppColors.Border;
                float borderWidth = _isFocused ? 1.5f : 1f;
                using (Pen pen = new Pen(borderColor, borderWidth))
                {
                    pen.Alignment = PenAlignment.Inset;
                    g.DrawPath(pen, path);
                }
            }

            // 4. Draw Vector Icon on Left
            if (!string.IsNullOrEmpty(_iconName))
            {
                Color iconColor = _isFocused ? AppColors.Primary : AppColors.TextLight;
                Rectangle iconRect = new Rectangle(12, (Height - 16) / 2, 16, 16);
                UIHelper.DrawVectorIcon(g, _iconName, iconRect, iconColor);
            }

            // 5. Draw Password Toggle Eye if applicable
            if (_isPassword)
            {
                Color eyeColor = _showPassword ? AppColors.Primary : AppColors.TextLight;
                using (Pen eyePen = new Pen(eyeColor, 1.4f))
                {
                    int ex = _eyeRect.X;
                    int ey = _eyeRect.Y;
                    int ew = _eyeRect.Width;
                    int eh = _eyeRect.Height;

                    g.DrawArc(eyePen, ex, ey, ew, eh, 20, 140);
                    g.DrawArc(eyePen, ex, ey - 2, ew, eh, 200, 140);
                    using (SolidBrush eyeDot = new SolidBrush(eyeColor))
                    {
                        g.FillEllipse(eyeDot, ex + ew / 2 - 2, ey + eh / 2 - 2, 4, 4);
                    }
                    if (!_showPassword)
                    {
                        g.DrawLine(eyePen, ex + 2, ey + eh - 2, ex + ew - 2, ey + 2);
                    }
                }
            }
        }
    }
}
