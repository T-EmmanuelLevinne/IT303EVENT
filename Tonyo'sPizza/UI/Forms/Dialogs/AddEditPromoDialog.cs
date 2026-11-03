using System;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class AddEditPromoDialog : Form
    {
        private readonly PromoVoucher _voucher;
        private TextBox _txtCode;
        private TextBox _txtDescription;
        private ComboBox _cboDiscountType;
        private NumericUpDown _numDiscountValue;
        private NumericUpDown _numMinSpend;
        private CheckBox _chkActive;
        private RoundedButton _btnSave;
        private RoundedButton _btnCancel;

        public PromoVoucher ResultVoucher { get; private set; }

        public AddEditPromoDialog(PromoVoucher voucher = null)
        {
            _voucher = voucher;
            InitializeComponent();
            PopulateData();
        }

        private void InitializeComponent()
        {
            Text = _voucher == null ? "Add Promo Voucher" : "Edit Promo Voucher";
            Size = new Size(460, 500);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = AppColors.Background;

            Panel pnlCard = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                BackColor = Color.White
            };

            int y = 14;

            // Title
            Label lblTitle = new Label
            {
                Text = _voucher == null ? "Create New Promo Voucher" : "Modify Promo Voucher",
                Font = new Font("Segoe UI Bold", 13f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(20, y),
                AutoSize = true
            };
            y += 34;

            // Voucher Code
            Label lblCode = new Label
            {
                Text = "VOUCHER CODE *",
                Font = new Font("Segoe UI Bold", 8.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(22, y),
                AutoSize = true
            };
            y += 20;

            _txtCode = new TextBox
            {
                Location = new Point(22, y),
                Width = 390,
                Font = new Font("Segoe UI Bold", 11f, FontStyle.Bold),
                CharacterCasing = CharacterCasing.Upper
            };
            y += 38;

            // Description
            Label lblDesc = new Label
            {
                Text = "DESCRIPTION / BANNER TEXT",
                Font = new Font("Segoe UI Bold", 8.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(22, y),
                AutoSize = true
            };
            y += 20;

            _txtDescription = new TextBox
            {
                Location = new Point(22, y),
                Width = 390,
                Font = new Font("Segoe UI", 9.5f)
            };
            y += 38;

            // Discount Type & Value Row
            Label lblType = new Label
            {
                Text = "DISCOUNT TYPE",
                Font = new Font("Segoe UI Bold", 8.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(22, y),
                AutoSize = true
            };

            Label lblVal = new Label
            {
                Text = "DISCOUNT VALUE",
                Font = new Font("Segoe UI Bold", 8.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(225, y),
                AutoSize = true
            };
            y += 20;

            _cboDiscountType = new ComboBox
            {
                Location = new Point(22, y),
                Width = 185,
                Font = new Font("Segoe UI", 9.5f),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cboDiscountType.Items.AddRange(new object[] { "Percentage (%)", "Fixed Amount (₱)" });
            _cboDiscountType.SelectedIndex = 0;

            _numDiscountValue = new NumericUpDown
            {
                Location = new Point(225, y),
                Width = 187,
                Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold),
                DecimalPlaces = 2,
                Maximum = 10000m,
                Value = 10m
            };
            y += 38;

            // Minimum Spend
            Label lblSpend = new Label
            {
                Text = "MINIMUM ORDER SPEND (₱)",
                Font = new Font("Segoe UI Bold", 8.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(22, y),
                AutoSize = true
            };
            y += 20;

            _numMinSpend = new NumericUpDown
            {
                Location = new Point(22, y),
                Width = 390,
                Font = new Font("Segoe UI", 9.5f),
                DecimalPlaces = 2,
                Maximum = 50000m,
                Value = 0m
            };
            y += 38;

            // Active Checkbox
            _chkActive = new CheckBox
            {
                Text = "Voucher is active and ready to be redeemed at checkout",
                Location = new Point(22, y),
                Width = 390,
                Checked = true,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Cursor = Cursors.Hand
            };
            y += 45;

            // Action Buttons
            Panel pnlButtons = new Panel
            {
                Location = new Point(22, y),
                Width = 390,
                Height = 44
            };

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Location = new Point(140, 2),
                Size = new Size(110, 38),
                BorderRadius = 8,
                NormalColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold)
            };
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            _btnSave = new RoundedButton
            {
                Text = "Save Voucher",
                IconName = "check",
                Location = new Point(260, 2),
                Size = new Size(130, 38),
                BorderRadius = 8,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold)
            };
            _btnSave.Click += BtnSave_Click;

            pnlButtons.Controls.Add(_btnCancel);
            pnlButtons.Controls.Add(_btnSave);

            pnlCard.Controls.Add(lblTitle);
            pnlCard.Controls.Add(lblCode);
            pnlCard.Controls.Add(_txtCode);
            pnlCard.Controls.Add(lblDesc);
            pnlCard.Controls.Add(_txtDescription);
            pnlCard.Controls.Add(lblType);
            pnlCard.Controls.Add(lblVal);
            pnlCard.Controls.Add(_cboDiscountType);
            pnlCard.Controls.Add(_numDiscountValue);
            pnlCard.Controls.Add(lblSpend);
            pnlCard.Controls.Add(_numMinSpend);
            pnlCard.Controls.Add(_chkActive);
            pnlCard.Controls.Add(pnlButtons);

            Controls.Add(pnlCard);
        }

        private void PopulateData()
        {
            if (_voucher != null)
            {
                _txtCode.Text = _voucher.Code;
                _txtDescription.Text = _voucher.Description;
                _cboDiscountType.SelectedIndex = _voucher.DiscountType == "Fixed" ? 1 : 0;
                _numDiscountValue.Value = Math.Min(_numDiscountValue.Maximum, _voucher.DiscountValue);
                _numMinSpend.Value = Math.Min(_numMinSpend.Maximum, _voucher.MinOrderAmount);
                _chkActive.Checked = _voucher.IsActive;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string code = _txtCode.Text.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("Please enter a voucher code (e.g., TONYO25).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtCode.Focus();
                return;
            }

            if (_numDiscountValue.Value <= 0)
            {
                MessageBox.Show("Discount value must be greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _numDiscountValue.Focus();
                return;
            }

            string discType = _cboDiscountType.SelectedIndex == 1 ? "Fixed" : "Percentage";
            if (discType == "Percentage" && _numDiscountValue.Value > 100m)
            {
                MessageBox.Show("Percentage discount cannot exceed 100%.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _numDiscountValue.Focus();
                return;
            }

            ResultVoucher = new PromoVoucher
            {
                PromoId = _voucher?.PromoId ?? 0,
                Code = code,
                Description = _txtDescription.Text.Trim(),
                DiscountType = discType,
                DiscountValue = _numDiscountValue.Value,
                MinOrderAmount = _numMinSpend.Value,
                IsActive = _chkActive.Checked,
                CreatedAt = _voucher?.CreatedAt ?? DateTime.Now
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
