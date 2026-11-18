using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;
using Tonyo_sPizza.UI.Controls;

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

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            if (diameter > bounds.Width) diameter = bounds.Width;
            if (diameter > bounds.Height) diameter = bounds.Height;
            arc.Size = new Size(diameter, diameter);

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
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

        public static void EnableDoubleBuffering(Control control)
        {
            if (control == null) return;
            try
            {
                PropertyInfo prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                prop?.SetValue(control, true, null);
            }
            catch { }
        }

        public static Bitmap GeneratePlaceholderPizzaImage(int width, int height, string label = "Pizza")
        {
            return GeneratePlaceholderFoodImage(width, height, label, null, includeLabel: true);
        }

        public static Bitmap GeneratePlaceholderFoodImage(int width, int height, string itemName = "Item", string categoryName = null, bool includeLabel = false)
        {
            Bitmap bmp = new Bitmap(Math.Max(1, width), Math.Max(1, height));
            string combined = $"{categoryName ?? string.Empty} {itemName ?? string.Empty}".ToLowerInvariant();

            bool isDrink = combined.Contains("beverage") || combined.Contains("drink") || combined.Contains("coke")
                || combined.Contains("soda") || combined.Contains("cola") || combined.Contains("juice")
                || combined.Contains("tea") || combined.Contains("coffee") || combined.Contains("water")
                || combined.Contains("lemonade") || combined.Contains("shake");

            bool isDessert = !isDrink && (combined.Contains("dessert") || combined.Contains("cake")
                || combined.Contains("brownie") || combined.Contains("cookie") || combined.Contains("gelato")
                || combined.Contains("tiramisu") || combined.Contains("cannoli") || combined.Contains("sweet"));

            bool isSide = !isDrink && !isDessert && (combined.Contains("side") || combined.Contains("appetizer")
                || combined.Contains("wing") || combined.Contains("bread") || combined.Contains("garlic")
                || combined.Contains("fries") || combined.Contains("pasta") || combined.Contains("salad")
                || combined.Contains("nugget"));

            using (Graphics g = Graphics.FromImage(bmp))
            {
                SetHighQuality(g);

                Color backdrop = isDrink
                    ? Color.FromArgb(244, 247, 246)
                    : isDessert
                        ? Color.FromArgb(253, 244, 242)
                        : Color.FromArgb(252, 246, 238);

                g.Clear(backdrop);

                int cx = width / 2;
                int cy = includeLabel ? (height / 2) - 6 : height / 2;

                if (isDrink)
                {
                    DrawBeverageArtwork(g, cx, cy, Math.Min(width, height));
                }
                else if (isSide)
                {
                    DrawSideDishArtwork(g, cx, cy, Math.Min(width, height));
                }
                else if (isDessert)
                {
                    DrawDessertArtwork(g, cx, cy, Math.Min(width, height));
                }
                else
                {
                    DrawPizzaArtwork(g, cx, cy, Math.Min(width, height) - (includeLabel ? 26 : 18));
                }

                if (includeLabel && !string.IsNullOrEmpty(itemName))
                {
                    using (Font font = new Font("Segoe UI", 8.5f, FontStyle.Bold))
                    using (SolidBrush textBrush = new SolidBrush(AppColors.TextMedium))
                    {
                        StringFormat sf = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Far,
                            Trimming = StringTrimming.EllipsisCharacter
                        };
                        g.DrawString(itemName, font, textBrush, new Rectangle(4, height - 18, width - 8, 16), sf);
                    }
                }
            }
            return bmp;
        }

        private static void DrawPizzaArtwork(Graphics g, int cx, int cy, int circleSize)
        {
            circleSize = Math.Max(48, circleSize);
            Rectangle rect = new Rectangle(cx - circleSize / 2, cy - circleSize / 2, circleSize, circleSize);

            Rectangle plateRect = rect;
            plateRect.Inflate(5, 5);
            using (Pen platePen = new Pen(Color.FromArgb(232, 224, 214), 2f))
            {
                g.DrawEllipse(platePen, plateRect);
            }

            using (SolidBrush crustBrush = new SolidBrush(Color.FromArgb(215, 145, 75)))
            {
                g.FillEllipse(crustBrush, rect);
            }

            rect.Inflate(-6, -6);
            using (SolidBrush sauceBrush = new SolidBrush(Color.FromArgb(184, 50, 39)))
            {
                g.FillEllipse(sauceBrush, rect);
            }

            rect.Inflate(-3, -3);
            using (SolidBrush cheeseBrush = new SolidBrush(Color.FromArgb(247, 213, 120)))
            {
                g.FillEllipse(cheeseBrush, rect);
            }

            // Crisp slice score hairlines
            using (Pen slicePen = new Pen(Color.FromArgb(55, 165, 82, 42), 1.1f))
            {
                float r = rect.Width / 2f;
                for (int a = 0; a < 3; a++)
                {
                    double rad = a * Math.PI / 3.0;
                    float dx = (float)(Math.Cos(rad) * r);
                    float dy = (float)(Math.Sin(rad) * r);
                    g.DrawLine(slicePen, cx - dx, cy - dy, cx + dx, cy + dy);
                }
            }

            using (SolidBrush pepBrush = new SolidBrush(Color.FromArgb(182, 49, 38)))
            using (SolidBrush basilBrush = new SolidBrush(Color.FromArgb(46, 125, 50)))
            {
                g.FillEllipse(pepBrush, cx - 17, cy - 16, 11, 11);
                g.FillEllipse(pepBrush, cx + 6, cy - 15, 11, 11);
                g.FillEllipse(pepBrush, cx - 14, cy + 6, 11, 11);
                g.FillEllipse(pepBrush, cx + 7, cy + 7, 10, 10);
                g.FillEllipse(basilBrush, cx - 4, cy - 2, 7, 4);
                g.FillEllipse(basilBrush, cx + 12, cy - 2, 6, 4);
            }
        }

        private static void DrawBeverageArtwork(Graphics g, int cx, int cy, int scaleSize)
        {
            int cupW = Math.Max(36, scaleSize / 3);
            int cupH = Math.Max(54, (int)(scaleSize * 0.56));
            int topY = cy - cupH / 2 + 4;

            using (Pen strawPen = new Pen(AppColors.Primary, 3f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                g.DrawLine(strawPen, cx + 2, topY + 6, cx + 15, topY - 16);
                g.DrawLine(strawPen, cx + 15, topY - 16, cx + 24, topY - 14);
            }

            PointF[] glass = new PointF[]
            {
                new PointF(cx - cupW / 2f, topY),
                new PointF(cx + cupW / 2f, topY),
                new PointF(cx + cupW / 2.6f, topY + cupH),
                new PointF(cx - cupW / 2.6f, topY + cupH)
            };
            using (SolidBrush liquidBrush = new SolidBrush(Color.FromArgb(156, 39, 32)))
            {
                PointF[] liquid = new PointF[]
                {
                    new PointF(cx - cupW / 2.15f, topY + 12),
                    new PointF(cx + cupW / 2.15f, topY + 12),
                    new PointF(cx + cupW / 2.75f, topY + cupH - 4),
                    new PointF(cx - cupW / 2.75f, topY + cupH - 4)
                };
                g.FillPolygon(liquidBrush, liquid);
            }
            using (SolidBrush iceBrush = new SolidBrush(Color.FromArgb(135, 255, 255, 255)))
            {
                g.FillRectangle(iceBrush, cx - 9, topY + 18, 9, 9);
                g.FillRectangle(iceBrush, cx + 1, topY + 26, 8, 8);
            }
            using (Pen glassPen = new Pen(Color.FromArgb(180, 192, 190), 2f))
            {
                g.DrawPolygon(glassPen, glass);
                g.DrawLine(glassPen, cx - cupW / 2f - 2, topY, cx + cupW / 2f + 2, topY);
            }
        }

        private static void DrawSideDishArtwork(Graphics g, int cx, int cy, int scaleSize)
        {
            int size = Math.Max(64, scaleSize - 24);
            Rectangle plate = new Rectangle(cx - size / 2, cy - size / 2, size, size);
            using (SolidBrush plateFill = new SolidBrush(Color.White))
            using (Pen plateRim = new Pen(Color.FromArgb(224, 215, 203), 2f))
            {
                g.FillEllipse(plateFill, plate);
                g.DrawEllipse(plateRim, plate);
            }

            using (SolidBrush goldBrush = new SolidBrush(Color.FromArgb(214, 146, 68)))
            using (SolidBrush glazeBrush = new SolidBrush(Color.FromArgb(184, 62, 42)))
            using (SolidBrush herbBrush = new SolidBrush(Color.FromArgb(56, 132, 60)))
            {
                g.FillEllipse(glazeBrush, cx - 20, cy - 15, 22, 13);
                g.FillEllipse(goldBrush, cx - 2, cy - 16, 22, 13);
                g.FillEllipse(goldBrush, cx - 18, cy + 2, 21, 13);
                g.FillEllipse(glazeBrush, cx + 2, cy + 3, 20, 12);
                g.FillEllipse(herbBrush, cx - 3, cy - 4, 6, 4);
                g.FillEllipse(herbBrush, cx + 8, cy + 1, 5, 4);
            }
        }

        private static void DrawDessertArtwork(Graphics g, int cx, int cy, int scaleSize)
        {
            int w = Math.Max(54, (int)(scaleSize * 0.56));
            int h = Math.Max(40, (int)(scaleSize * 0.38));
            int x = cx - w / 2;
            int y = cy - h / 2 + 4;

            using (SolidBrush spongeBrush = new SolidBrush(Color.FromArgb(228, 178, 124)))
            using (SolidBrush frostingBrush = new SolidBrush(Color.FromArgb(252, 240, 236)))
            using (SolidBrush berryBrush = new SolidBrush(AppColors.Primary))
            using (Pen outlinePen = new Pen(Color.FromArgb(196, 148, 102), 1.5f))
            {
                g.FillRectangle(spongeBrush, x, y + h / 3, w, (h * 2) / 3);
                g.FillRectangle(frostingBrush, x - 2, y, w + 4, h / 3 + 4);
                g.DrawRectangle(outlinePen, x, y + h / 3, w, (h * 2) / 3);
                g.FillEllipse(berryBrush, cx - 7, y - 9, 14, 14);
            }
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            if (dgv == null) return;
            EnableDoubleBuffering(dgv);

            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(247, 244, 239);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = AppColors.TextDark;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 6, 8, 6);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(247, 244, 239);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = AppColors.TextDark;
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = AppColors.TextMedium;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.25f, FontStyle.Regular);
            dgv.DefaultCellStyle.SelectionBackColor = AppColors.PrimarySoft;
            dgv.DefaultCellStyle.SelectionForeColor = AppColors.PrimaryDark;
            dgv.DefaultCellStyle.Padding = new Padding(8, 5, 8, 5);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(253, 252, 249);
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = AppColors.PrimarySoft;
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = AppColors.PrimaryDark;

            dgv.RowTemplate.Height = 44;
            dgv.GridColor = AppColors.BorderLight;
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

        public static void ConfigureGridColumn(DataGridView dgv, string columnName, int width, bool fill = false, int minWidth = 48)
        {
            if (dgv == null || !dgv.Columns.Contains(columnName)) return;
            DataGridViewColumn col = dgv.Columns[columnName];
            if (fill)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.MinimumWidth = Math.Max(minWidth, width);
            }
            else
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                col.MinimumWidth = Math.Min(width, Math.Max(36, minWidth));
                col.Width = width;
            }
        }

        /// <summary>
        /// Renders a clean, anti-aliased pill badge inside a DataGridView cell instead of raw emoji text.
        /// </summary>
        public static void AttachStatusPillPainter(DataGridView dgv, Func<string, (Color Fg, Color Bg)> styleResolver, params string[] columnNames)
        {
            if (dgv == null || styleResolver == null || columnNames == null || columnNames.Length == 0) return;
            var targetCols = new HashSet<string>(columnNames, StringComparer.OrdinalIgnoreCase);

            dgv.CellPainting += (s, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                string colName = dgv.Columns[e.ColumnIndex].Name;
                if (!targetCols.Contains(colName)) return;

                string text = e.FormattedValue?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(text)) return;

                e.PaintBackground(e.ClipBounds, true);
                SetHighQuality(e.Graphics);

                var (fg, bg) = styleResolver(text);

                using (Font pillFont = new Font("Segoe UI Semibold", 8.25f, FontStyle.Bold))
                {
                    Size measured = TextRenderer.MeasureText(text, pillFont);
                    int pillW = Math.Min(e.CellBounds.Width - 14, measured.Width + 22);
                    int pillH = Math.Min(e.CellBounds.Height - 12, 24);
                    int px = e.CellBounds.X + 10;
                    int py = e.CellBounds.Y + (e.CellBounds.Height - pillH) / 2;

                    Rectangle pillRect = new Rectangle(px, py, Math.Max(40, pillW), pillH);

                    using (GraphicsPath path = GetRoundedRectanglePath(pillRect, 11))
                    using (SolidBrush fillBrush = new SolidBrush(bg))
                    {
                        e.Graphics.FillPath(fillBrush, path);
                    }

                    // 6px dot inside left edge of pill
                    using (SolidBrush dotBrush = new SolidBrush(fg))
                    {
                        e.Graphics.FillEllipse(dotBrush, pillRect.X + 8, pillRect.Y + (pillRect.Height - 6) / 2, 6, 6);
                    }

                    Rectangle txtRect = new Rectangle(pillRect.X + 17, pillRect.Y, Math.Max(10, pillRect.Width - 20), pillRect.Height);
                    TextRenderer.DrawText(
                        e.Graphics,
                        text,
                        pillFont,
                        txtRect,
                        fg,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix
                    );
                }

                // Preserve bottom row border
                using (Pen gridPen = new Pen(dgv.GridColor, 1))
                {
                    e.Graphics.DrawLine(gridPen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                }

                e.Handled = true;
            };
        }

        public static (Color Fg, Color Bg) ResolveSemanticStatusColors(string value)
        {
            string s = (value ?? "").Trim().ToLowerInvariant();
            if (s.Contains("out of stock") || s.Contains("critical") || s.Contains("cancelled") || s.Contains("void") || s.Contains("delete") || s.Contains("failed") || s.Contains("locked") || s.Contains("0 left"))
                return (AppColors.StatusCancelled, AppColors.StatusCancelledBg);

            if (s.Contains("low") || s.Contains("pending") || s.Contains("stock more") || s.Contains("archive") || s.Contains("warning") || s.Contains("inactive"))
                return (AppColors.StatusPending, AppColors.StatusPendingBg);

            if (s.Contains("preparing") || s.Contains("restock") || s.Contains("edit") || s.Contains("update") || s.Contains("reset"))
                return (AppColors.StatusPreparing, AppColors.StatusPreparingBg);

            if (s.Contains("ready") || s.Contains("admin"))
                return (AppColors.StatusReady, AppColors.StatusReadyBg);

            if (s.Contains("completed") || s.Contains("in stock") || s.Contains("active") || s.Contains("posted") || s.Contains("ok") || s.Contains("adequate") || s.Contains("healthy") || s.Contains("login"))
                return (AppColors.StatusCompleted, AppColors.StatusCompletedBg);

            return (AppColors.StatusNeutral, AppColors.StatusNeutralBg);
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

            using (Pen pen = new Pen(color, 1.7f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            using (SolidBrush brush = new SolidBrush(color))
            {
                int x = rect.X;
                int y = rect.Y;
                int w = rect.Width;
                int h = rect.Height;

                if (key == "dashboard" || key == "home")
                {
                    int bw = (w - 5) / 2;
                    int bh1 = (int)(h * 0.52f);
                    int bh2 = h - bh1 - 5;
                    g.DrawRectangle(pen, x + 1, y + 1, bw, bh1);
                    g.DrawRectangle(pen, x + bw + 4, y + 1, bw, bh2);
                    g.DrawRectangle(pen, x + 1, y + bh1 + 4, bw, bh2);
                    g.DrawRectangle(pen, x + bw + 4, y + bh2 + 4, bw, bh1);
                }
                else if (key == "stock" || key == "box" || key == "package")
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
                else if (key == "user_add")
                {
                    int headD = (int)(w * 0.34f);
                    g.DrawEllipse(pen, x + 2, y + 1, headD, headD);
                    g.DrawArc(pen, x, y + headD + 2, w - 6, h - headD - 1, 200, 140);
                    g.DrawLine(pen, x + w - 4, y + 4, x + w - 4, y + 10);
                    g.DrawLine(pen, x + w - 7, y + 7, x + w - 1, y + 7);
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
                else if (key == "print" || key == "printer")
                {
                    int px = x + 3, pw = w - 6;
                    g.DrawRectangle(pen, px, y + 1, pw, 4);
                    g.DrawRectangle(pen, x + 1, y + 5, w - 2, h - 8);
                    g.DrawRectangle(pen, px, y + h - 5, pw, 4);
                }
                else if (key == "backup" || key == "database")
                {
                    g.DrawEllipse(pen, x + 2, y + 1, w - 4, 5);
                    g.DrawLine(pen, x + 2, y + 3, x + 2, y + h - 3);
                    g.DrawLine(pen, x + w - 2, y + 3, x + w - 2, y + h - 3);
                    g.DrawArc(pen, x + 2, y + h - 6, w - 4, 5, 0, 180);
                    g.DrawArc(pen, x + 2, y + h / 2 - 2, w - 4, 5, 0, 180);
                }
                else if (key == "key")
                {
                    g.DrawEllipse(pen, x + 1, y + 4, 6, 6);
                    g.DrawLine(pen, x + 7, y + 7, x + w - 1, y + 7);
                    g.DrawLine(pen, x + w - 3, y + 7, x + w - 3, y + 10);
                }
                else if (key == "shield")
                {
                    using (GraphicsPath p = new GraphicsPath())
                    {
                        p.AddLine(x + w / 2, y + 1, x + w - 2, y + 3);
                        p.AddLine(x + w - 2, y + 3, x + w - 2, y + h * 0.6f);
                        p.AddLine(x + w - 2, y + h * 0.6f, x + w / 2, y + h - 1);
                        p.AddLine(x + w / 2, y + h - 1, x + 2, y + h * 0.6f);
                        p.AddLine(x + 2, y + h * 0.6f, x + 2, y + 3);
                        p.CloseFigure();
                        g.DrawPath(pen, p);
                    }
                }
                else if (key == "archive")
                {
                    g.DrawRectangle(pen, x + 1, y + 2, w - 2, 4);
                    g.DrawRectangle(pen, x + 2, y + 6, w - 4, h - 7);
                    g.DrawLine(pen, x + w / 2 - 2, y + 9, x + w / 2 + 2, y + 9);
                }
                else if (key == "export")
                {
                    g.DrawRectangle(pen, x + 2, y + h / 2, w - 4, h / 2 - 1);
                    g.DrawLine(pen, x + w / 2, y + 1, x + w / 2, y + h / 2 + 2);
                    g.DrawLine(pen, x + w / 2, y + 1, x + w / 2 - 3, y + 4);
                    g.DrawLine(pen, x + w / 2, y + 1, x + w / 2 + 3, y + 4);
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
                else if (key == "restock" || key == "refresh" || key == "sync" || key == "restore")
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
                else if (key == "unlock")
                {
                    int bw = w - 4, bh = (int)(h * 0.52f);
                    int bx = x + 2, by = y + h - bh - 1;
                    g.DrawRectangle(pen, bx, by, bw, bh);
                    int sw = bw * 55 / 100;
                    int sx = bx + (bw - sw) / 2;
                    g.DrawArc(pen, sx, by - 7, sw, 8, 180, 150);
                }
                else if (key == "warning" || key == "alert")
                {
                    PointF top = new PointF(x + w / 2f, y + 1);
                    PointF left = new PointF(x + 1, y + h - 1);
                    PointF right = new PointF(x + w - 1, y + h - 1);
                    g.DrawPolygon(pen, new[] { top, right, left });
                    g.DrawLine(pen, x + w / 2f, y + 5, x + w / 2f, y + h - 5);
                    g.FillEllipse(brush, x + w / 2f - 1, y + h - 3.5f, 2, 2);
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

        public static Bitmap CreateDualToneMenuIconBitmap(string iconName, Color baseColor, Color accentColor, int size = 18)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                SetHighQuality(g);
                DrawDualToneMenuIcon(g, iconName, new Rectangle(1, 1, size - 2, size - 2), baseColor, accentColor);
            }
            return bmp;
        }

        private static void DrawDualToneMenuIcon(Graphics g, string iconName, Rectangle rect, Color baseColor, Color accentColor)
        {
            if (rect.Width <= 0 || rect.Height <= 0) return;
            string key = (iconName ?? "").ToLower().Trim();

            using (Pen inkPen = new Pen(baseColor, 1.45f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            using (Pen accentPen = new Pen(accentColor, 1.55f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            using (SolidBrush accentBrush = new SolidBrush(accentColor))
            {
                int x = rect.X, y = rect.Y, w = rect.Width, h = rect.Height;

                if (key == "edit" || key == "pencil")
                {
                    g.DrawLine(inkPen, x + 2, y + h - 2, x + 4, y + h - 6);
                    g.DrawLine(inkPen, x + 4, y + h - 6, x + w - 4, y + 2);
                    g.DrawLine(inkPen, x + w - 4, y + 2, x + w - 1, y + 5);
                    g.DrawLine(inkPen, x + w - 1, y + 5, x + 7, y + h - 3);
                    g.DrawLine(inkPen, x + 7, y + h - 3, x + 2, y + h - 2);
                    g.DrawLine(accentPen, x + 2, y + h - 1, x + w - 2, y + h - 1);
                }
                else if (key == "delete" || key == "trash" || key == "remove")
                {
                    int tx = x + 3, ty = y + 4, tw = w - 6, th = h - 5;
                    g.DrawLine(inkPen, x + 1, ty, x + w - 1, ty);
                    g.DrawArc(inkPen, x + w / 2 - 2, y + 1, 4, 4, 180, 180);
                    g.DrawRectangle(inkPen, tx, ty, tw, th);
                    g.DrawLine(accentPen, tx + tw / 2f - 1.5f, ty + 3, tx + tw / 2f - 1.5f, ty + th - 3);
                    g.DrawLine(accentPen, tx + tw / 2f + 1.5f, ty + 3, tx + tw / 2f + 1.5f, ty + th - 3);
                }
                else if (key == "restock" || key == "refresh" || key == "sync" || key == "restore")
                {
                    g.DrawArc(inkPen, x + 2, y + 2, w - 4, h - 4, 40, 275);
                    g.DrawLine(accentPen, x + w - 2, y + 3, x + w - 2, y + 7);
                    g.DrawLine(accentPen, x + w - 2, y + 7, x + w - 6, y + 7);
                }
                else if (key == "add" || key == "plus")
                {
                    g.DrawEllipse(inkPen, x + 1, y + 1, w - 2, h - 2);
                    g.DrawLine(accentPen, x + w / 2f, y + 4.5f, x + w / 2f, y + h - 4.5f);
                    g.DrawLine(accentPen, x + 4.5f, y + h / 2f, x + w - 4.5f, y + h / 2f);
                }
                else if (key == "minus")
                {
                    g.DrawEllipse(inkPen, x + 1, y + 1, w - 2, h - 2);
                    g.DrawLine(accentPen, x + 4.5f, y + h / 2f, x + w - 4.5f, y + h / 2f);
                }
                else if (key == "archive")
                {
                    g.DrawRectangle(inkPen, x + 1, y + 2, w - 2, 4);
                    g.DrawRectangle(inkPen, x + 2, y + 6, w - 4, h - 7);
                    g.DrawLine(accentPen, x + w / 2f - 2.5f, y + 9.5f, x + w / 2f + 2.5f, y + 9.5f);
                }
                else if (key == "print" || key == "printer")
                {
                    int px = x + 3, pw = w - 6;
                    g.DrawRectangle(inkPen, px, y + 1, pw, 4);
                    g.DrawRectangle(inkPen, x + 1, y + 5, w - 2, h - 8);
                    g.DrawRectangle(accentPen, px, y + h - 5, pw, 4);
                }
                else if (key == "key")
                {
                    g.DrawEllipse(inkPen, x + 1, y + 4, 6, 6);
                    g.DrawLine(inkPen, x + 7, y + 7, x + w - 1, y + 7);
                    g.DrawLine(accentPen, x + w - 3, y + 7, x + w - 3, y + 11);
                    g.DrawLine(accentPen, x + w - 5.5f, y + 7, x + w - 5.5f, y + 10);
                }
                else if (key == "unlock")
                {
                    int bw = w - 4, bh = (int)(h * 0.5f);
                    int bx = x + 2, by = y + h - bh - 1;
                    g.DrawRectangle(inkPen, bx, by, bw, bh);
                    int sw = bw * 58 / 100;
                    int sx = bx + (bw - sw) / 2;
                    g.DrawArc(accentPen, sx, by - 7, sw, 8, 180, 145);
                }
                else if (key == "shield")
                {
                    using (GraphicsPath p = new GraphicsPath())
                    {
                        p.AddLine(x + w / 2f, y + 1, x + w - 2, y + 3);
                        p.AddLine(x + w - 2, y + 3, x + w - 2, y + h * 0.6f);
                        p.AddLine(x + w - 2, y + h * 0.6f, x + w / 2f, y + h - 1);
                        p.AddLine(x + w / 2f, y + h - 1, x + 2, y + h * 0.6f);
                        p.AddLine(x + 2, y + h * 0.6f, x + 2, y + 3);
                        p.CloseFigure();
                        g.DrawPath(inkPen, p);
                    }
                    g.FillEllipse(accentBrush, x + w / 2f - 2f, y + h * 0.42f, 4f, 4f);
                }
                else if (key == "users" || key == "user")
                {
                    int headD = (int)(w * 0.36f);
                    g.DrawEllipse(inkPen, x + 2, y + 2, headD, headD);
                    g.DrawArc(inkPen, x + 1, y + headD + 3, w - 6, h - headD - 2, 200, 140);
                    g.DrawEllipse(accentPen, x + w - headD - 1, y + 3, headD - 1, headD - 1);
                }
                else if (key == "kitchen" || key == "cook")
                {
                    int bw = w - 4, bh = h - 7;
                    int bx = x + 2, by = y + 5;
                    g.DrawRectangle(inkPen, bx, by, bw, bh);
                    g.DrawLine(inkPen, x, by + 3, bx, by + 3);
                    g.DrawLine(inkPen, bx + bw, by + 3, x + w, by + 3);
                    g.DrawLine(accentPen, x + w * 0.35f, y + 3, x + w * 0.65f, y + 3);
                }
                else if (key == "cart" || key == "bag" || key == "shop")
                {
                    g.DrawRectangle(inkPen, x + 2, y + 5, w - 4, h - 6);
                    g.DrawArc(accentPen, x + (w - 8) / 2f, y + 1, 8, 8, 180, 180);
                }
                else if (key == "star")
                {
                    PointF[] pts = new PointF[10];
                    float cx = x + w / 2f;
                    float cy = y + h / 2f;
                    float outerR = Math.Min(w, h) * 0.46f;
                    float innerR = outerR * 0.44f;
                    for (int i = 0; i < 10; i++)
                    {
                        double angle = -Math.PI / 2.0 + i * Math.PI / 5.0;
                        float r = (i % 2 == 0) ? outerR : innerR;
                        pts[i] = new PointF(cx + (float)(Math.Cos(angle) * r), cy + (float)(Math.Sin(angle) * r));
                    }
                    g.DrawPolygon(inkPen, pts);
                    g.FillEllipse(accentBrush, cx - 2f, cy - 2f, 4f, 4f);
                }
                else if (key == "check")
                {
                    g.DrawLine(accentPen, x + 2, y + h * 0.55f, x + w * 0.42f, y + h - 3);
                    g.DrawLine(accentPen, x + w * 0.42f, y + h - 3, x + w - 2, y + 2);
                }
                else if (key == "stock" || key == "box" || key == "package")
                {
                    g.DrawRectangle(inkPen, x + 2, y + 2, w - 4, h - 4);
                    g.DrawLine(inkPen, x + 2, y + h / 3f + 1, x + w - 2, y + h / 3f + 1);
                    g.DrawLine(accentPen, x + w / 2f, y + 2, x + w / 2f, y + h / 3f + 1);
                }
                else
                {
                    DrawVectorIcon(g, key, rect, accentColor);
                }
            }
        }

        public static ContextMenuStrip CreateModernContextMenu()
        {
            return new ModernContextMenu();
        }

        public static ToolStripMenuItem CreateModernMenuItem(string text, string iconName, EventHandler onClick, Color? foreColor = null)
        {
            bool hasCustomColor = foreColor.HasValue;
            bool isDestructive = hasCustomColor && foreColor.Value.R >= 165 && foreColor.Value.G < 80;
            Color textColor = isDestructive ? foreColor.Value : AppColors.TextDark;
            Color iconInk = isDestructive ? Color.FromArgb(62, 30, 30) : Color.FromArgb(44, 39, 35);
            Color iconAccent = hasCustomColor ? foreColor.Value : AppColors.Primary;

            var item = new ModernMenuItem(text)
            {
                ForeColor = textColor,
                Padding = new Padding(8, 7, 16, 7),
                Margin = new Padding(0, 1, 0, 1),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft
            };
            if (!string.IsNullOrEmpty(iconName))
            {
                item.Image = CreateDualToneMenuIconBitmap(iconName, iconInk, iconAccent, 18);
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
        private static readonly Color MenuBg = Color.FromArgb(252, 250, 248);
        private static readonly Color HoverFill = Color.FromArgb(241, 237, 231);
        private static readonly Color PressedFill = Color.FromArgb(232, 227, 219);
        private static readonly Color DestructiveHoverFill = Color.FromArgb(254, 232, 232);
        private static readonly Color DestructivePressedFill = Color.FromArgb(254, 214, 214);
        private static readonly Color DividerColor = Color.FromArgb(234, 229, 221);

        public ModernMenuRenderer() : base(new ModernColorTable()) { }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using (var b = new SolidBrush(MenuBg))
            {
                e.Graphics.FillRectangle(b, e.AffectedBounds);
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            using (var b = new SolidBrush(MenuBg))
            {
                e.Graphics.FillRectangle(b, e.AffectedBounds);
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            int w = e.ToolStrip != null && e.ToolStrip.ClientSize.Width > 0 ? e.ToolStrip.ClientSize.Width : e.Item.Width;
            Rectangle rc = new Rectangle(0, 0, w, e.Item.Height);
            using (var bgBrush = new SolidBrush(MenuBg))
            {
                e.Graphics.FillRectangle(bgBrush, rc);
            }

            if (!e.Item.Enabled) return;

            bool isChecked = e.Item is ToolStripMenuItem mi && mi.Checked;
            bool isDestructive = e.Item.ForeColor.R >= 165 && e.Item.ForeColor.G < 80;

            Color? pillFill = null;
            if (e.Item.Pressed)
            {
                pillFill = isDestructive ? DestructivePressedFill : PressedFill;
            }
            else if (e.Item.Selected)
            {
                pillFill = isDestructive ? DestructiveHoverFill : HoverFill;
            }
            else if (isChecked)
            {
                pillFill = AppColors.PrimarySoft;
            }

            if (pillFill.HasValue)
            {
                Rectangle pill = new Rectangle(6, 1, Math.Max(8, w - 12), Math.Max(8, e.Item.Height - 2));
                using (var b = new SolidBrush(pillFill.Value))
                using (var path = UIHelper.GetRoundedRectanglePath(pill, 6))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(b, path);
                }
            }
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            // Suppressed intentionally so left dual-tone icons remain clean; active check is drawn right-aligned in OnRenderItemText
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            if (e.Image == null) return;

            int size = 18;
            int ix = 16;
            int iy = (e.Item.Height - size) / 2;
            Rectangle dest = new Rectangle(ix, iy, size, size);

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (!e.Item.Enabled)
            {
                using (var ia = new ImageAttributes())
                {
                    var cm = new ColorMatrix { Matrix33 = 0.36f };
                    ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    e.Graphics.DrawImage(e.Image, dest, 0, 0, e.Image.Width, e.Image.Height, GraphicsUnit.Pixel, ia);
                }
            }
            else
            {
                e.Graphics.DrawImage(e.Image, dest);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            int w = e.ToolStrip != null && e.ToolStrip.ClientSize.Width > 0 ? e.ToolStrip.ClientSize.Width : e.Item.Width;
            int textLeft = 44;
            int rightReserve = (e.Item is ToolStripMenuItem menuItem && (menuItem.HasDropDownItems || menuItem.Checked)) ? 32 : 16;
            Rectangle rect = new Rectangle(textLeft, 0, Math.Max(10, w - textLeft - rightReserve), e.Item.Height);

            Color fore = !e.Item.Enabled ? AppColors.TextLight : e.Item.ForeColor;
            TextRenderer.DrawText(
                e.Graphics,
                e.Text,
                e.TextFont,
                rect,
                fore,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis
            );

            if (e.Item is ToolStripMenuItem mi && mi.Checked && !mi.HasDropDownItems && e.Item.Enabled)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                int cx = w - 24;
                int cy = e.Item.Height / 2;
                using (var checkPen = new Pen(AppColors.Primary, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                {
                    e.Graphics.DrawLine(checkPen, cx, cy, cx + 3, cy + 3);
                    e.Graphics.DrawLine(checkPen, cx + 3, cy + 3, cx + 8, cy - 3);
                }
            }
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            int w = e.Item?.Owner != null && e.Item.Owner.ClientSize.Width > 0
                ? e.Item.Owner.ClientSize.Width
                : (e.Item?.Width ?? e.ArrowRectangle.Right);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Color chevronColor = (e.Item == null || e.Item.Enabled) ? AppColors.TextMuted : AppColors.TextLight;
            int cx = w - 18;
            int cy = (e.Item?.Height ?? e.ArrowRectangle.Height) / 2;

            using (var pen = new Pen(chevronColor, 1.45f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            {
                e.Graphics.DrawLine(pen, cx - 2, cy - 4, cx + 2, cy);
                e.Graphics.DrawLine(pen, cx + 2, cy, cx - 2, cy + 4);
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            int w = e.ToolStrip != null && e.ToolStrip.ClientSize.Width > 0 ? e.ToolStrip.ClientSize.Width : e.Item.Width;
            int y = e.Item.Height / 2;
            using (var pen = new Pen(DividerColor, 1))
            {
                e.Graphics.DrawLine(pen, 0, y, w, y);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (ModernContextMenu.IsDropDownUsingNativeRounding(e.ToolStrip))
            {
                return;
            }

            Rectangle rc = new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
            using (var path = UIHelper.GetRoundedRectanglePath(rc, 8))
            using (var pen = new Pen(AppColors.Border, 1))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }
    }

    public class ModernColorTable : ProfessionalColorTable
    {
        private static readonly Color MenuSurface = Color.FromArgb(252, 250, 248);
        private static readonly Color HoverSoft = Color.FromArgb(241, 237, 231);
        private static readonly Color PressedSoft = Color.FromArgb(232, 227, 219);

        public override Color ToolStripDropDownBackground => MenuSurface;
        public override Color ImageMarginGradientBegin => MenuSurface;
        public override Color ImageMarginGradientMiddle => MenuSurface;
        public override Color ImageMarginGradientEnd => MenuSurface;
        public override Color MenuBorder => AppColors.Border;
        public override Color MenuItemBorder => Color.Transparent;
        public override Color MenuItemSelected => HoverSoft;
        public override Color MenuItemSelectedGradientBegin => HoverSoft;
        public override Color MenuItemSelectedGradientEnd => HoverSoft;
        public override Color MenuItemPressedGradientBegin => PressedSoft;
        public override Color MenuItemPressedGradientEnd => PressedSoft;
        public override Color SeparatorDark => Color.FromArgb(234, 229, 221);
        public override Color SeparatorLight => Color.Transparent;
    }
}
