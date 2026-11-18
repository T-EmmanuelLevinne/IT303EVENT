using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class ReceiptPreviewDialog : Form
    {
        private readonly Order _order;
        private readonly ReceiptPrintService _printService;
        private Panel _pnlReceiptCanvas;
        private RoundedButton _btnPrint;
        private RoundedButton _btnCopy;
        private RoundedButton _btnClose;

        public ReceiptPreviewDialog(Order order)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _printService = new ReceiptPrintService(_order);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = $"Order Receipt Slip - #{_order.OrderId}";
            Size = new Size(460, 680);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = AppColors.SurfaceAlt;
            CancelButton = null;

            // Header Bar
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };
            header.Paint += (s, e) =>
            {
                using (var pen = new Pen(AppColors.Border, 1))
                    e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
            };

            Label lblTitle = new Label
            {
                Text = $"Order #{_order.OrderId} Receipt Preview",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(20, 15),
                AutoSize = true
            };
            header.Controls.Add(lblTitle);

            // Scrollable Preview Well
            Panel scrollWell = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = AppColors.SurfaceAlt,
                Padding = new Padding(24, 18, 24, 18)
            };
            UIHelper.EnableDoubleBuffering(scrollWell);

            int itemExtraHeight = Math.Max(0, (_order.Items?.Count ?? 1) * 46);
            int paperHeight = Math.Max(480, 410 + itemExtraHeight);

            _pnlReceiptCanvas = new Panel
            {
                Location = new Point(24, 18),
                Size = new Size(380, paperHeight),
                BackColor = Color.White
            };
            UIHelper.EnableDoubleBuffering(_pnlReceiptCanvas);
            _pnlReceiptCanvas.Paint += (s, e) =>
            {
                _printService.RenderReceiptToGraphics(e.Graphics, _pnlReceiptCanvas.Width);
                using (var pen = new Pen(AppColors.Border, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, _pnlReceiptCanvas.Width - 1, _pnlReceiptCanvas.Height - 1);
                }
            };
            scrollWell.Controls.Add(_pnlReceiptCanvas);

            // Footer Actions
            Panel bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 64,
                BackColor = Color.White,
                Padding = new Padding(18, 12, 18, 12)
            };
            bottom.Paint += (s, e) =>
            {
                using (var pen = new Pen(AppColors.Border, 1))
                    e.Graphics.DrawLine(pen, 0, 0, bottom.Width, 0);
            };

            _btnPrint = new RoundedButton
            {
                Text = "Print Receipt",
                IconName = "print",
                Width = 148,
                Height = 38,
                Dock = DockStyle.Left,
                BorderRadius = 8,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight,
                PressedColor = AppColors.PrimaryDark
            };
            _btnPrint.Click += BtnPrint_Click;

            _btnCopy = new RoundedButton
            {
                Text = "Copy Text",
                IconName = "orders",
                Width = 114,
                Height = 38,
                Location = new Point(176, 12),
                BorderRadius = 8,
                NormalColor = AppColors.SecondaryNormal,
                HoverColor = AppColors.SecondaryHover,
                ForeColor = AppColors.TextDark
            };
            _btnCopy.Click += BtnCopy_Click;

            _btnClose = new RoundedButton
            {
                Text = "Close",
                Width = 94,
                Height = 38,
                Dock = DockStyle.Right,
                BorderRadius = 8,
                NormalColor = AppColors.SecondaryNormal,
                HoverColor = AppColors.SecondaryHover,
                ForeColor = AppColors.TextDark
            };
            _btnClose.Click += (s, e) => Close();
            CancelButton = _btnClose;

            bottom.Controls.Add(_btnCopy);
            bottom.Controls.Add(_btnPrint);
            bottom.Controls.Add(_btnClose);

            Controls.Add(scrollWell);
            Controls.Add(bottom);
            Controls.Add(header);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                using (PrintDocument doc = _printService.CreatePrintDocument())
                using (PrintDialog pd = new PrintDialog { Document = doc, UseEXDialog = true })
                {
                    if (pd.ShowDialog(this) == DialogResult.OK)
                    {
                        doc.Print();
                        AuditService.Instance.Log("Print Receipt", $"Printed official receipt slip for Order #{_order.OrderId}.", "Orders", _order.OrderId.ToString());
                        MessageBox.Show(this, "Receipt sent to printer.", "Print Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Could not send the receipt to the selected printer. Please verify your printer or PDF printer configuration.", ex, "Print Error", this);
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                string slip = _printService.BuildPlainTextSlip();
                Clipboard.SetText(slip);
                MessageBox.Show(this, "Formatted receipt text copied to clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Unable to copy receipt text to clipboard.", ex, "Copy Notice", this);
            }
        }
    }
}
