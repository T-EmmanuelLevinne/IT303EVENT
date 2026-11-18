using System;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class RestockDialogForm : Form
    {
        private readonly Ingredient _ingredient;
        private NumericUpDown _numAdd;
        private TextBox _txtNotes;
        private RoundedButton _btnConfirm;
        private RoundedButton _btnCancel;

        public RestockDialogForm(Ingredient ingredient)
        {
            _ingredient = ingredient ?? throw new ArgumentNullException(nameof(ingredient));
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Restock Ingredient";
            Size = new Size(400, 390);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = AppColors.Background;

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = AppColors.Primary
            };

            Label title = new Label
            {
                Text = "Restock Ingredient",
                Font = new Font("Segoe UI Bold", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 16),
                AutoSize = true
            };
            header.Controls.Add(title);

            Panel content = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                BackColor = Color.White
            };

            int y = 16;

            Label lblInfo = new Label
            {
                Text = $"Ingredient: {_ingredient.Name}\nCurrent Stock: {_ingredient.CurrentStock:N2} {_ingredient.Unit}",
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(24, y),
                Size = new Size(340, 44)
            };
            y += 54;

            Label lblAdd = new Label
            {
                Text = $"Quantity to Add ({_ingredient.Unit}) *",
                Location = new Point(24, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                ForeColor = AppColors.TextDark
            };
            y += 24;

            _numAdd = new NumericUpDown
            {
                Location = new Point(24, y),
                Width = 330,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                DecimalPlaces = 2,
                Maximum = 999999,
                Minimum = 0.01m,
                Value = 10
            };
            y += 44;

            Label lblNotes = new Label
            {
                Text = "Delivery / Supplier Reference (Optional)",
                Location = new Point(24, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = AppColors.TextMedium
            };
            y += 22;

            _txtNotes = new TextBox
            {
                Location = new Point(24, y),
                Width = 330,
                Font = new Font("Segoe UI", 9.5f),
                Text = "Manual Restock"
            };
            y += 44;

            _btnConfirm = new RoundedButton
            {
                Text = "Confirm Restock",
                IconName = "check",
                Location = new Point(24, y),
                Size = new Size(195, 40),
                BorderRadius = 8,
                NormalColor = AppColors.StatusCompleted,
                HoverColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold)
            };
            _btnConfirm.Click += BtnConfirm_Click;

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Location = new Point(229, y),
                Size = new Size(125, 40),
                BorderRadius = 8,
                NormalColor = AppColors.SecondaryNormal,
                HoverColor = AppColors.SecondaryHover,
                PressedColor = AppColors.SecondaryPressed,
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold)
            };
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            content.Controls.Add(lblInfo);
            content.Controls.Add(lblAdd);
            content.Controls.Add(_numAdd);
            content.Controls.Add(lblNotes);
            content.Controls.Add(_txtNotes);
            content.Controls.Add(_btnConfirm);
            content.Controls.Add(_btnCancel);

            Controls.Add(content);
            Controls.Add(header);

            AcceptButton = _btnConfirm;
            CancelButton = _btnCancel;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (_numAdd.Value <= 0)
            {
                MessageBox.Show(this, "Please enter a quantity greater than zero.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string notes = string.IsNullOrWhiteSpace(_txtNotes.Text) ? "Manual Restock" : _txtNotes.Text.Trim();
            if (StockService.Instance.RestockAtomic(_ingredient.IngredientId, _numAdd.Value, notes, out int txId, out string err))
            {
                MessageBox.Show(this, $"Restock recorded (Stock Transaction #{txId}).", "Restock Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(this, string.IsNullOrWhiteSpace(err) ? "Could not complete restock transaction." : err, "Restock Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
