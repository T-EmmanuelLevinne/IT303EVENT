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
            Size = new Size(380, 320);
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
                Size = new Size(320, 44)
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
                Width = 310,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                DecimalPlaces = 2,
                Maximum = 999999,
                Minimum = 0.01m,
                Value = 10
            };
            y += 48;

            _btnConfirm = new RoundedButton
            {
                Text = "Confirm Restock",
                Location = new Point(24, y),
                Size = new Size(180, 40),
                BorderRadius = 8,
                NormalColor = AppColors.StatusCompleted,
                HoverColor = Color.FromArgb(16, 185, 129)
            };
            _btnConfirm.Click += BtnConfirm_Click;

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Location = new Point(214, y),
                Size = new Size(120, 40),
                BorderRadius = 8,
                NormalColor = Color.FromArgb(226, 232, 240),
                HoverColor = Color.FromArgb(203, 213, 225),
                ForeColor = AppColors.TextDark
            };
            _btnCancel.Click += (s, e) => Close();

            content.Controls.Add(lblInfo);
            content.Controls.Add(lblAdd);
            content.Controls.Add(_numAdd);
            content.Controls.Add(_btnConfirm);
            content.Controls.Add(_btnCancel);

            Controls.Add(content);
            Controls.Add(header);
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (_numAdd.Value <= 0)
            {
                MessageBox.Show("Please enter a quantity greater than zero.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            StockService.Instance.Restock(_ingredient.IngredientId, _numAdd.Value);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
