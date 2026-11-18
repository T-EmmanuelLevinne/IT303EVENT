using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.Services
{
    public class ReceiptPrintService
    {
        private readonly Order _order;
        private readonly string _servedBy;

        public ReceiptPrintService(Order order, string servedBy = null)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
            if (_order.Items == null || _order.Items.Count == 0)
            {
                try
                {
                    _order.Items = new OrderRepository().GetOrderItems(_order.OrderId);
                }
                catch { }
            }
            _servedBy = string.IsNullOrWhiteSpace(servedBy)
                ? (AuthService.Instance?.CurrentUser?.FullName ?? "Counter Staff")
                : servedBy;
        }

        private static readonly string[] VirtualPrinterKeywords =
        {
            "microsoft print to pdf",
            "microsoft xps",
            "onenote",
            "fax",
            "send to onenote",
            "adobe pdf",
            "foxit",
            "cutepdf",
            "nitro"
        };

        public static bool TryDetectPhysicalPrinter(out string printerName)
        {
            printerName = null;
            try
            {
                foreach (string installed in PrinterSettings.InstalledPrinters)
                {
                    if (string.IsNullOrWhiteSpace(installed)) continue;
                    string lower = installed.ToLowerInvariant();
                    if (VirtualPrinterKeywords.Any(k => lower.Contains(k))) continue;

                    var ps = new PrinterSettings { PrinterName = installed };
                    if (ps.IsValid)
                    {
                        printerName = installed;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ReceiptPrintService.TryDetectPhysicalPrinter");
            }
            return false;
        }

        public bool TryAutoPrintSilently(out string statusMessage)
        {
            statusMessage = string.Empty;
            try
            {
                if (!TryDetectPhysicalPrinter(out string printerName))
                {
                    statusMessage = "No physical printer detected";
                    return false;
                }

                using (PrintDocument doc = CreatePrintDocument())
                {
                    doc.PrinterSettings.PrinterName = printerName;
                    doc.PrintController = new StandardPrintController();
                    doc.Print();
                }

                statusMessage = $"Receipt printed on '{printerName}'";
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ReceiptPrintService.TryAutoPrintSilently");
                statusMessage = "Printer unavailable right now";
                return false;
            }
        }

        public PrintDocument CreatePrintDocument()
        {
            var doc = new PrintDocument
            {
                DocumentName = $"TonyosPizza_Receipt_Order_{_order.OrderId}"
            };
            doc.DefaultPageSettings.PaperSize = new PaperSize("ReceiptSlip", 380, Math.Max(560, 420 + (_order.Items?.Count ?? 1) * 44));
            doc.DefaultPageSettings.Margins = new Margins(16, 16, 16, 16);
            doc.PrintPage += OnPrintPage;
            return doc;
        }

        public void RenderReceiptToGraphics(Graphics g, int width)
        {
            UIHelper.SetHighQuality(g);
            g.Clear(Color.White);

            int pad = 18;
            int contentW = width - pad * 2;
            float y = 18f;

            using (var fontBrand = new Font("Segoe UI", 15f, FontStyle.Bold))
            using (var fontSub = new Font("Segoe UI", 8.5f))
            using (var fontSection = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold))
            using (var fontBody = new Font("Segoe UI", 9f))
            using (var fontBodyBold = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (var fontDetail = new Font("Segoe UI", 8f))
            using (var fontTotal = new Font("Segoe UI", 12.5f, FontStyle.Bold))
            using (var brushInk = new SolidBrush(Color.FromArgb(31, 27, 24)))
            using (var brushMuted = new SolidBrush(Color.FromArgb(110, 102, 95)))
            using (var brushAccent = new SolidBrush(AppColors.Primary))
            using (var penDashed = new Pen(Color.FromArgb(200, 194, 186), 1f) { DashStyle = DashStyle.Dash })
            using (var penSolid = new Pen(Color.FromArgb(225, 220, 212), 1f))
            {
                var centerFormat = new StringFormat { Alignment = StringAlignment.Center };
                var rightFormat = new StringFormat { Alignment = StringAlignment.Far };

                // Brand Emblem + Header
                g.DrawString("TONYO'S PIZZA", fontBrand, brushAccent, new RectangleF(pad, y, contentW, 26), centerFormat);
                y += 24;
                g.DrawString("Wood-Fired Brick Oven Kitchen", fontSub, brushMuted, new RectangleF(pad, y, contentW, 16), centerFormat);
                y += 15;
                g.DrawString("123 Hearthstone Ave.  |  Tel: (02) 8555-0192", fontDetail, brushMuted, new RectangleF(pad, y, contentW, 14), centerFormat);
                y += 20;

                g.DrawLine(penDashed, pad, y, pad + contentW, y);
                y += 10;

                // Receipt Title & Order Metadata
                g.DrawString("OFFICIAL ORDER SLIP", fontSection, brushInk, new RectangleF(pad, y, contentW, 16), centerFormat);
                y += 20;

                DrawKeyValueLine(g, "Order No.:", $"#{_order.OrderId}", pad, ref y, contentW, fontBody, fontBodyBold, brushMuted, brushInk);
                DrawKeyValueLine(g, "Date & Time:", _order.CreatedAt.ToString("MMM dd, yyyy  hh:mm tt"), pad, ref y, contentW, fontBody, fontBody, brushMuted, brushInk);
                DrawKeyValueLine(g, "Customer:", string.IsNullOrWhiteSpace(_order.CustomerName) ? "Walk-in Guest" : _order.CustomerName, pad, ref y, contentW, fontBody, fontBodyBold, brushMuted, brushInk);
                if (!string.IsNullOrWhiteSpace(_order.Phone))
                    DrawKeyValueLine(g, "Contact:", _order.Phone, pad, ref y, contentW, fontBody, fontBody, brushMuted, brushInk);
                DrawKeyValueLine(g, "Processed By:", _servedBy, pad, ref y, contentW, fontBody, fontBody, brushMuted, brushInk);
                DrawKeyValueLine(g, "Status:", _order.Status, pad, ref y, contentW, fontBody, fontBodyBold, brushMuted, brushInk);

                y += 6;
                g.DrawLine(penDashed, pad, y, pad + contentW, y);
                y += 10;

                // Column Headers
                g.DrawString("ITEM", fontSection, brushMuted, pad, y);
                g.DrawString("AMOUNT", fontSection, brushMuted, new RectangleF(pad, y, contentW, 16), rightFormat);
                y += 18;
                g.DrawLine(penSolid, pad, y, pad + contentW, y);
                y += 8;

                decimal itemsSubtotal = 0m;
                if (_order.Items != null)
                {
                    foreach (var item in _order.Items)
                    {
                        itemsSubtotal += item.Subtotal;
                        string lineTitle = $"{item.Quantity}x  {item.ItemName}";
                        string priceStr = $"₱{item.Subtotal:N2}";

                        g.DrawString(priceStr, fontBodyBold, brushInk, new RectangleF(pad + contentW - 85, y, 85, 18), rightFormat);
                        SizeF titleSz = g.MeasureString(lineTitle, fontBodyBold, contentW - 90);
                        g.DrawString(lineTitle, fontBodyBold, brushInk, new RectangleF(pad, y, contentW - 90, titleSz.Height + 2));
                        y += Math.Max(18, titleSz.Height + 2);

                        if (!string.IsNullOrWhiteSpace(item.SelectedDoughSize))
                        {
                            g.DrawString($"    Crust: {item.SelectedDoughSize}", fontDetail, brushMuted, pad, y);
                            y += 15;
                        }
                        if (!string.IsNullOrWhiteSpace(item.SelectedAddOns))
                        {
                            string addOnText = $"    Add-ons: {item.SelectedAddOns}";
                            SizeF aoSz = g.MeasureString(addOnText, fontDetail, contentW - 10);
                            g.DrawString(addOnText, fontDetail, brushMuted, new RectangleF(pad, y, contentW - 10, aoSz.Height + 2));
                            y += aoSz.Height + 2;
                        }
                        y += 4;
                    }
                }

                y += 4;
                g.DrawLine(penDashed, pad, y, pad + contentW, y);
                y += 10;

                // Summary Totals
                DrawKeyValueLine(g, "Items Subtotal:", $"₱{itemsSubtotal:N2}", pad, ref y, contentW, fontBody, fontBody, brushMuted, brushInk);

                decimal diff = _order.TotalAmount - itemsSubtotal;
                if (diff > 0)
                {
                    DrawKeyValueLine(g, "Service Fee:", $"₱{diff:N2}", pad, ref y, contentW, fontBody, fontBody, brushMuted, brushInk);
                }
                else if (diff < 0)
                {
                    decimal serviceFee = 2.00m;
                    decimal discount = Math.Abs(diff - serviceFee);
                    DrawKeyValueLine(g, "Service Fee:", $"₱{serviceFee:N2}", pad, ref y, contentW, fontBody, fontBody, brushMuted, brushInk);
                    DrawKeyValueLine(g, "Promo Discount:", $"-₱{discount:N2}", pad, ref y, contentW, fontBody, fontBodyBold, brushMuted, brushAccent);
                }

                y += 4;
                g.DrawLine(penSolid, pad, y, pad + contentW, y);
                y += 8;

                g.DrawString("TOTAL DUE", fontSection, brushInk, pad, y + 4);
                g.DrawString($"₱{_order.TotalAmount:N2}", fontTotal, brushAccent, new RectangleF(pad, y, contentW, 24), rightFormat);
                y += 30;

                if (!string.IsNullOrWhiteSpace(_order.Notes))
                {
                    g.DrawLine(penDashed, pad, y, pad + contentW, y);
                    y += 8;
                    string noteTxt = $"Kitchen Note: {_order.Notes}";
                    SizeF nSz = g.MeasureString(noteTxt, fontDetail, contentW);
                    g.DrawString(noteTxt, fontDetail, brushMuted, new RectangleF(pad, y, contentW, nSz.Height + 4));
                    y += nSz.Height + 8;
                }

                y += 8;
                g.DrawLine(penDashed, pad, y, pad + contentW, y);
                y += 12;

                g.DrawString("Thank you for dining with Tonyo's Pizza!", fontBodyBold, brushInk, new RectangleF(pad, y, contentW, 18), centerFormat);
                y += 18;
                g.DrawString($"Printed: {DateTime.Now:yyyy-MM-dd hh:mm tt}", fontDetail, brushMuted, new RectangleF(pad, y, contentW, 14), centerFormat);
            }
        }

        public string BuildPlainTextSlip()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==========================================");
            sb.AppendLine("              TONYO'S PIZZA               ");
            sb.AppendLine("      Wood-Fired Brick Oven Kitchen       ");
            sb.AppendLine("==========================================");
            sb.AppendLine($"Order No.   : #{_order.OrderId}");
            sb.AppendLine($"Date        : {_order.CreatedAt:yyyy-MM-dd hh:mm tt}");
            sb.AppendLine($"Customer    : {_order.CustomerName}");
            if (!string.IsNullOrWhiteSpace(_order.Phone)) sb.AppendLine($"Contact     : {_order.Phone}");
            sb.AppendLine($"Status      : {_order.Status}");
            sb.AppendLine($"Processed By: {_servedBy}");
            sb.AppendLine("------------------------------------------");
            if (_order.Items != null)
            {
                foreach (var item in _order.Items)
                {
                    sb.AppendLine($"{item.Quantity}x {item.ItemName,-24} ₱{item.Subtotal,8:N2}");
                    if (!string.IsNullOrWhiteSpace(item.SelectedDoughSize))
                        sb.AppendLine($"   Crust: {item.SelectedDoughSize}");
                    if (!string.IsNullOrWhiteSpace(item.SelectedAddOns))
                        sb.AppendLine($"   Add-ons: {item.SelectedAddOns}");
                }
            }
            sb.AppendLine("------------------------------------------");
            sb.AppendLine($"TOTAL AMOUNT DUE:               ₱{_order.TotalAmount,8:N2}");
            if (!string.IsNullOrWhiteSpace(_order.Notes))
                sb.AppendLine($"Note: {_order.Notes}");
            sb.AppendLine("==========================================");
            sb.AppendLine("   Thank you for your order at Tonyo's!   ");
            sb.AppendLine($"   Printed: {DateTime.Now:yyyy-MM-dd hh:mm tt}");
            sb.AppendLine("==========================================");
            return sb.ToString();
        }

        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            RenderReceiptToGraphics(e.Graphics, e.PageBounds.Width);
            e.HasMorePages = false;
        }

        private static void DrawKeyValueLine(Graphics g, string key, string value, int x, ref float y, int width, Font keyFont, Font valFont, Brush keyBrush, Brush valBrush)
        {
            var rightFormat = new StringFormat { Alignment = StringAlignment.Far };
            g.DrawString(key, keyFont, keyBrush, x, y);
            g.DrawString(value, valFont, valBrush, new RectangleF(x, y, width, 18), rightFormat);
            y += 17;
        }
    }
}
