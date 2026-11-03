using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tonyo_sPizza.UI.Theme
{
    public static class UIHelper
    {
        public static GraphicsPath GetRoundedRectanglePath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // Top Left
            path.AddArc(arc, 180, 90);

            // Top Right
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom Right
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom Left
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public static void SetHighQuality(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }

        public static Bitmap GeneratePlaceholderPizzaImage(int width, int height, string label = "Pizza")
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                SetHighQuality(g);
                g.Clear(Color.FromArgb(254, 242, 242));

                // Draw pizza circle
                int circleSize = Math.Min(width, height) - 20;
                Rectangle rect = new Rectangle((width - circleSize) / 2, (height - circleSize) / 2, circleSize, circleSize);
                
                using (SolidBrush crustBrush = new SolidBrush(Color.FromArgb(245, 158, 11)))
                {
                    g.FillEllipse(crustBrush, rect);
                }

                // Cheese
                rect.Inflate(-6, -6);
                using (SolidBrush cheeseBrush = new SolidBrush(Color.FromArgb(252, 211, 77)))
                {
                    g.FillEllipse(cheeseBrush, rect);
                }

                // Pepperoni dots
                using (SolidBrush pepBrush = new SolidBrush(Color.FromArgb(220, 38, 38)))
                {
                    int center = width / 2;
                    int centerY = height / 2;
                    g.FillEllipse(pepBrush, center - 18, centerY - 18, 12, 12);
                    g.FillEllipse(pepBrush, center + 6, centerY - 16, 12, 12);
                    g.FillEllipse(pepBrush, center - 12, centerY + 6, 12, 12);
                    g.FillEllipse(pepBrush, center + 8, centerY + 8, 10, 10);
                }

                // Text
                if (!string.IsNullOrEmpty(label))
                {
                    using (Font font = new Font("Segoe UI", 9, FontStyle.Bold))
                    using (SolidBrush textBrush = new SolidBrush(AppColors.PrimaryDark))
                    {
                        StringFormat sf = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Far
                        };
                        g.DrawString(label, font, textBrush, new Rectangle(0, height - 20, width, 18), sf);
                    }
                }
            }
            return bmp;
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = AppColors.Background;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = AppColors.TextDark;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 10, 12, 10);
            dgv.ColumnHeadersHeight = 44;

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = AppColors.TextMedium;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            dgv.DefaultCellStyle.SelectionBackColor = AppColors.PrimarySoft;
            dgv.DefaultCellStyle.SelectionForeColor = AppColors.PrimaryDark;
            dgv.DefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgv.RowTemplate.Height = 44;

            dgv.GridColor = AppColors.Border;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public static string GetCategoryIconName(string categoryName, string iconKey = null)
        {
            string key = (iconKey ?? "").ToLower();
            string name = (categoryName ?? "").ToLower();

            if (key.Contains("beverage") || name.Contains("beverage") || name.Contains("drink") || name.Contains("tea") || name.Contains("juice")) return "drink";
            if (key.Contains("pastry") || key.Contains("dessert") || name.Contains("dessert") || name.Contains("cake") || name.Contains("sweet")) return "dessert";
            if (key.Contains("chicken") || name.Contains("side") || name.Contains("appetizer") || name.Contains("wing") || name.Contains("bread")) return "side";
            if (name.Contains("signature")) return "star";
            if (name.Contains("classic")) return "pizza";
            return "pizza";
        }

        public static Bitmap CreateVectorIconBitmap(string iconName, Color color, int size = 18)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                SetHighQuality(g);
                DrawVectorIcon(g, iconName, new Rectangle(1, 1, size - 2, size - 2), color);
            }
            return bmp;
        }

        public static void DrawVectorIcon(Graphics g, string iconName, Rectangle rect, Color color)
        {
            if (rect.Width <= 0 || rect.Height <= 0) return;
            string key = (iconName ?? "").ToLower().Trim();

            using (Pen pen = new Pen(color, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            using (SolidBrush brush = new SolidBrush(color))
            {
                int x = rect.X;
                int y = rect.Y;
                int w = rect.Width;
                int h = rect.Height;

                if (key == "stock" || key == "box" || key == "package")
                {
                    int bx = x + 1, by = y + 2, bw = w - 2, bh = h - 3;
                    g.DrawRectangle(pen, bx, by, bw, bh);
                    g.DrawLine(pen, bx, by + bh / 3, bx + bw, by + bh / 3);
                    g.DrawLine(pen, bx + bw / 2, by, bx + bw / 2, by + bh);
                }
                else if (key == "menu" || key == "pizza" || key == "food")
                {
                    PointF top = new PointF(x + w / 2f, y + h - 1);
                    PointF left = new PointF(x + 1, y + 3);
                    PointF right = new PointF(x + w - 1, y + 3);

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddLine(top, left);
                        path.AddArc(x + 1, y + 1, w - 2, 5, 180, 180);
                        path.AddLine(right, top);
                        g.DrawPath(pen, path);
                    }
                    g.FillEllipse(brush, x + w / 2f - 1.5f, y + 5, 3, 3);
                    g.FillEllipse(brush, x + w * 0.35f, y + 8, 2.5f, 2.5f);
                }
                else if (key == "category" || key == "categories" || key == "grid")
                {
                    int tileW = (w - 4) / 2;
                    int tileH = (h - 4) / 2;
                    g.DrawRectangle(pen, x + 1, y + 1, tileW, tileH);
                    g.DrawRectangle(pen, x + tileW + 3, y + 1, tileW, tileH);
                    g.DrawRectangle(pen, x + 1, y + tileH + 3, tileW, tileH);
                    g.DrawRectangle(pen, x + tileW + 3, y + tileH + 3, tileW, tileH);
                }
                else if (key == "users" || key == "user" || key == "people")
                {
                    int headD = (int)(w * 0.38f);
                    int headX = x + (w - headD) / 2;
                    int headY = y + 1;
                    g.DrawEllipse(pen, headX, headY, headD, headD);
                    g.DrawArc(pen, x + 1, y + headD + 2, w - 2, h - headD - 1, 200, 140);
                }
                else if (key == "kitchen" || key == "queue" || key == "cook")
                {
                    int bw = w - 4, bh = h - 7;
                    int bx = x + 2, by = y + 5;
                    g.DrawRectangle(pen, bx, by, bw, bh);
                    g.DrawLine(pen, x, by + 3, bx, by + 3);
                    g.DrawLine(pen, bx + bw, by + 3, x + w, by + 3);
                    g.DrawLine(pen, x + w * 0.35f, y + 3, x + w * 0.35f, y + 1);
                    g.DrawLine(pen, x + w * 0.65f, y + 3, x + w * 0.65f, y + 1);
                }
                else if (key == "cart" || key == "shop" || key == "bag")
                {
                    int bagX = x + 2, bagY = y + 5, bagW = w - 4, bagH = h - 6;
                    g.DrawRectangle(pen, bagX, bagY, bagW, bagH);
                    g.DrawArc(pen, x + (w - 8) / 2, y + 1, 8, 8, 180, 180);
                }
                else if (key == "orders" || key == "list" || key == "receipt")
                {
                    int cx = x + 2, cy = y + 2, cw = w - 4, ch = h - 3;
                    g.DrawRectangle(pen, cx, cy, cw, ch);
                    g.DrawLine(pen, x + (w - 6) / 2, cy, x + (w + 6) / 2, cy);
                    g.DrawLine(pen, cx + 3, cy + 5, cx + cw - 3, cy + 5);
                    g.DrawLine(pen, cx + 3, cy + 8, cx + cw - 4, cy + 8);
                }
                else if (key == "edit" || key == "pencil")
                {
                    g.DrawLine(pen, x + 2, y + h - 2, x + 5, y + h - 5);
                    g.DrawLine(pen, x + 5, y + h - 5, x + w - 3, y + 3);
                    g.DrawLine(pen, x + 2, y + h - 2, x + 2, y + h - 4);
                }
                else if (key == "delete" || key == "trash" || key == "remove")
                {
                    int tx = x + 2, ty = y + 4, tw = w - 4, th = h - 5;
                    g.DrawLine(pen, x, ty, x + w, ty);
                    g.DrawRectangle(pen, tx + 1, ty, tw - 2, th);
                    g.DrawLine(pen, tx + tw / 2, ty + 2, tx + tw / 2, ty + th - 2);
                }
                else if (key == "restock" || key == "refresh" || key == "sync")
                {
                    g.DrawArc(pen, x + 1, y + 1, w - 2, h - 2, 45, 270);
                    g.DrawLine(pen, x + w - 2, y + h / 2, x + w - 5, y + h / 2 - 3);
                    g.DrawLine(pen, x + w - 2, y + h / 2, x + w - 5, y + h / 2 + 3);
                }
                else if (key == "back" || key == "arrow_left")
                {
                    g.DrawLine(pen, x + 2, y + h / 2, x + w - 2, y + h / 2);
                    g.DrawLine(pen, x + 2, y + h / 2, x + 6, y + 3);
                    g.DrawLine(pen, x + 2, y + h / 2, x + 6, y + h - 3);
                }
                else if (key == "drink" || key == "beverage")
                {
                    g.DrawLine(pen, x + 3, y + 4, x + 4, y + h - 1);
                    g.DrawLine(pen, x + 4, y + h - 1, x + w - 4, y + h - 1);
                    g.DrawLine(pen, x + w - 4, y + h - 1, x + w - 3, y + 4);
                    g.DrawLine(pen, x + 2, y + 4, x + w - 2, y + 4);
                    g.DrawLine(pen, x + w / 2, y + 4, x + w / 2 + 3, y);
                }
                else if (key == "dessert" || key == "cake")
                {
                    g.DrawRectangle(pen, x + 2, y + 5, w - 4, h - 6);
                    g.DrawLine(pen, x + 2, y + 9, x + w - 2, y + 9);
                    g.FillEllipse(brush, x + w / 2 - 2, y + 1, 4, 4);
                }
                else if (key == "side" || key == "chicken")
                {
                    g.DrawEllipse(pen, x + 3, y + 2, w - 6, (h - 4) * 0.7f);
                    g.DrawLine(pen, x + w / 2, y + (h - 4) * 0.7f, x + w / 2, y + h - 1);
                }
                else if (key == "star")
                {
                    g.DrawLine(pen, x + w / 2, y + 1, x + w / 2, y + h - 1);
                    g.DrawLine(pen, x + 1, y + h / 2, x + w - 1, y + h / 2);
                    g.DrawLine(pen, x + 3, y + 3, x + w - 3, y + h - 3);
                    g.DrawLine(pen, x + w - 3, y + 3, x + 3, y + h - 3);
                }
                else if (key == "promo" || key == "tag" || key == "voucher" || key == "discount")
                {
                    int vx = x + 1, vy = y + 3, vw = w - 2, vh = h - 6;
                    g.DrawRectangle(pen, vx, vy, vw, vh);
                    g.DrawLine(pen, vx + (int)(vw * 0.68f), vy, vx + (int)(vw * 0.68f), vy + vh);
                    g.FillEllipse(brush, vx + (int)(vw * 0.3f), vy + vh / 2 - 2, 4, 4);
                }
                else if (key == "search")
                {
                    int cd = (int)(w * 0.6f);
                    g.DrawEllipse(pen, x + 1, y + 1, cd, cd);
                    g.DrawLine(pen, x + 1 + cd * 85 / 100, y + 1 + cd * 85 / 100, x + w - 2, y + h - 2);
                }
                else if (key == "check")
                {
                    g.DrawLine(pen, x + 2, y + h / 2, x + w * 0.4f, y + h - 3);
                    g.DrawLine(pen, x + w * 0.4f, y + h - 3, x + w - 2, y + 2);
                }
                else if (key == "add" || key == "plus")
                {
                    g.DrawLine(pen, x + w / 2, y + 3, x + w / 2, y + h - 3);
                    g.DrawLine(pen, x + 3, y + h / 2, x + w - 3, y + h / 2);
                }
                else if (key == "lock")
                {
                    int bw = w - 4, bh = (int)(h * 0.52f);
                    int bx = x + 2, by = y + h - bh - 1;
                    g.DrawRectangle(pen, bx, by, bw, bh);
                    int sw = bw * 55 / 100;
                    int sx = bx + (bw - sw) / 2;
                    g.DrawArc(pen, sx, by - 6, sw, 8, 180, 180);
                    g.FillEllipse(brush, bx + bw / 2 - 1.5f, by + bh / 2 - 2, 3, 3);
                }
                else if (key == "phone")
                {
                    int pw = (int)(w * 0.55f), ph = h - 2;
                    int px = x + (w - pw) / 2, py = y + 1;
                    g.DrawRectangle(pen, px, py, pw, ph);
                    g.DrawLine(pen, px + 2, py + 3, px + pw - 2, py + 3);
                    g.FillEllipse(brush, px + pw / 2 - 1.5f, py + ph - 4, 3, 3);
                }
                else if (key == "pin" || key == "location")
                {
                    int pd = (int)(w * 0.55f);
                    int px = x + (w - pd) / 2;
                    g.DrawEllipse(pen, px, y + 1, pd, pd);
                    g.DrawLine(pen, px + pd / 2, y + 1 + pd, px + pd / 2, y + h - 1);
                }
                else if (key == "report" || key == "chart" || key == "analytics")
                {
                    int by = y + h - 2;
                    g.DrawLine(pen, x + 1, by, x + w - 1, by);
                    int barW = Math.Max(2, (w - 8) / 3);
                    g.DrawRectangle(pen, x + 2, by - (int)(h * 0.45f), barW, (int)(h * 0.45f));
                    g.DrawRectangle(pen, x + 3 + barW, by - (int)(h * 0.82f), barW, (int)(h * 0.82f));
                    g.DrawRectangle(pen, x + 4 + barW * 2, by - (int)(h * 0.62f), barW, (int)(h * 0.62f));
                }
                else
                {
                    g.FillEllipse(brush, x + w / 2 - 3, y + h / 2 - 3, 6, 6);
                }
            }
        }

        public static ContextMenuStrip CreateModernContextMenu()
        {
            var menu = new ContextMenuStrip
            {
                Renderer = new ModernMenuRenderer(),
                ShowImageMargin = true,
                ShowCheckMargin = false,
                ImageScalingSize = new Size(16, 16),
                BackColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Padding = new Padding(4),
                DropShadowEnabled = true
            };
            return menu;
        }

        public static ToolStripMenuItem CreateModernMenuItem(string text, string iconName, EventHandler onClick, Color? foreColor = null)
        {
            Color color = foreColor ?? AppColors.TextDark;
            var item = new ToolStripMenuItem(text)
            {
                ForeColor = color,
                Padding = new Padding(8, 6, 10, 6),
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            if (!string.IsNullOrEmpty(iconName))
            {
                item.Image = CreateVectorIconBitmap(iconName, color, 16);
            }
            if (onClick != null) item.Click += onClick;
            return item;
        }

        public static ToolStripMenuItem CreateModernMenuItem(string text, EventHandler onClick, Color? foreColor = null)
        {
            return CreateModernMenuItem(text, null, onClick, foreColor);
        }
    }

    public class ModernMenuRenderer : ToolStripProfessionalRenderer
    {
        public ModernMenuRenderer() : base(new ModernColorTable())
        {
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            if (e.Item.Selected || e.Item.Pressed)
            {
                using (var b = new SolidBrush(AppColors.PrimarySoft))
                using (var pen = new Pen(AppColors.PrimaryBorder, 1))
                {
                    Rectangle pill = new Rectangle(rc.Left + 4, rc.Top + 2, rc.Width - 8, rc.Height - 4);
                    using (var path = UIHelper.GetRoundedRectanglePath(pill, 6))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.FillPath(b, path);
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }
            else
            {
                using (var b = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRectangle(b, rc);
                }
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(8, e.Item.Height / 2, e.Item.Width - 16, 1);
            using (var pen = new Pen(AppColors.Border, 1))
            {
                e.Graphics.DrawLine(pen, rc.Left, rc.Top, rc.Right, rc.Top);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            using (var pen = new Pen(AppColors.Border, 1))
            {
                Rectangle rc = new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
                e.Graphics.DrawRectangle(pen, rc);
            }
        }
    }

    public class ModernColorTable : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground => Color.White;
        public override Color ImageMarginGradientBegin => Color.White;
        public override Color ImageMarginGradientMiddle => Color.White;
        public override Color ImageMarginGradientEnd => Color.White;
        public override Color MenuBorder => AppColors.Border;
        public override Color MenuItemBorder => Color.Transparent;
        public override Color MenuItemSelected => AppColors.PrimarySoft;
        public override Color MenuItemSelectedGradientBegin => AppColors.PrimarySoft;
        public override Color MenuItemSelectedGradientEnd => AppColors.PrimarySoft;
        public override Color MenuItemPressedGradientBegin => AppColors.PrimaryBorder;
        public override Color MenuItemPressedGradientEnd => AppColors.PrimaryBorder;
        public override Color SeparatorDark => AppColors.Border;
        public override Color SeparatorLight => Color.Transparent;
    }
}
