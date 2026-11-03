using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class AddEditDoughSizeDialog : Form
    {
        private readonly MenuItemDoughSize _doughSize;
        private readonly List<Ingredient> _doughIngredients;

        private ComboBox _cboSizeName;
        private NumericUpDown _numPrice;
        private ComboBox _cboDoughIngredient;
        private NumericUpDown _numPortion;
        private Label _lblUnit;
        private Label _lblStockStatus;
        private Label _lblDeductionInfo;
        private CheckBox _chkDefault;
        private RoundedButton _btnSave;
        private RoundedButton _btnCancel;

        public MenuItemDoughSize ResultSize => _doughSize;

        public AddEditDoughSizeDialog(MenuItemDoughSize doughSize = null, List<Ingredient> allIngredients = null)
        {
            _doughSize = doughSize ?? new MenuItemDoughSize();
            var ingRepo = new IngredientRepository();
            var all = allIngredients ?? ingRepo.GetAllIngredients();
            _doughIngredients = all.Where(i => i.CategoryName == "Dough" || i.Name.ToLower().Contains("dough") || i.Name.ToLower().Contains("crust")).ToList();
            if (_doughIngredients.Count == 0) _doughIngredients = all;

            InitializeComponent();
            PopulateData();
        }

        private void InitializeComponent()
        {
            Text = "Configure Dough Size & Crust";
            Size = new Size(480, 530);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = AppColors.Background;

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = AppColors.Primary
            };

            Label title = new Label
            {
                Text = "Configure Dough Size & Crust",
                Font = new Font("Segoe UI Bold", 11.5f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(18, 14),
                AutoSize = true
            };
            header.Controls.Add(title);

            Panel content = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                BackColor = Color.White
            };

            int y = 14;

            Label lblName = new Label
            {
                Text = "Size / Crust Name * (Select preset or type custom):",
                Location = new Point(20, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = AppColors.TextDark
            };
            y += 22;

            _cboSizeName = new ComboBox
            {
                Location = new Point(20, y),
                Width = 415,
                Font = new Font("Segoe UI", 10f),
                DropDownStyle = ComboBoxStyle.DropDown
            };
            _cboSizeName.Items.AddRange(new object[]
            {
                "Small 10\"",
                "Medium 12\"",
                "Large 14\"",
                "Extra Large 16\"",
                "Personal 8\"",
                "Thin Crust 10\"",
                "Thin Crust 12\"",
                "Thin Crust 14\"",
                "Stuffed Crust 12\"",
                "Stuffed Crust 14\"",
                "Deep Dish 12\"",
                "Gluten-Free 10\"",
                "Regular Crust"
            });
            _cboSizeName.SelectedIndexChanged += CboSizeName_SelectedIndexChanged;
            y += 34;

            Label lblPrice = new Label { Text = "Selling Price (₱) *", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 22;

            _numPrice = new NumericUpDown { Location = new Point(20, y), Width = 415, Font = new Font("Segoe UI", 10f), DecimalPlaces = 2, Maximum = 9999, Minimum = 0, Value = 14.99m };
            y += 36;

            Label lblIng = new Label { Text = "Link Dough Stock Ingredient (For Live Stock & Deduction):", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 22;
            _cboDoughIngredient = new ComboBox { Location = new Point(20, y), Width = 415, Font = new Font("Segoe UI", 9.5f), DropDownStyle = ComboBoxStyle.DropDownList };
            _cboDoughIngredient.SelectedIndexChanged += (s, e) => UpdateStockAndDeduction();
            y += 34;

            Label lblPortion = new Label { Text = "Dough Portion Required:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 22;
            _numPortion = new NumericUpDown { Location = new Point(20, y), Width = 130, Font = new Font("Segoe UI", 10f), DecimalPlaces = 2, Maximum = 9999, Minimum = 0.01m, Value = 1.00m };
            _numPortion.ValueChanged += (s, e) => UpdateStockAndDeduction();

            _lblUnit = new Label { Text = "pcs", Location = new Point(158, y + 3), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };

            _lblStockStatus = new Label { Text = "Stock Available: -", Location = new Point(235, y + 3), AutoSize = true, Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold), ForeColor = AppColors.StatusCompleted };
            y += 32;

            _lblDeductionInfo = new Label
            {
                Text = "Stock Deduction: 1.00 pcs per item",
                Location = new Point(20, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryDark
            };
            y += 26;

            _chkDefault = new CheckBox { Text = "Set as Default Selected Size", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };

            content.Controls.Add(lblName);
            content.Controls.Add(_cboSizeName);
            content.Controls.Add(lblPrice);
            content.Controls.Add(_numPrice);
            content.Controls.Add(lblIng);
            content.Controls.Add(_cboDoughIngredient);
            content.Controls.Add(lblPortion);
            content.Controls.Add(_numPortion);
            content.Controls.Add(_lblUnit);
            content.Controls.Add(_lblStockStatus);
            content.Controls.Add(_lblDeductionInfo);
            content.Controls.Add(_chkDefault);

            Panel bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = AppColors.Background,
                Padding = new Padding(18, 10, 18, 10)
            };

            _btnSave = new RoundedButton
            {
                Text = "Apply Size",
                Dock = DockStyle.Right,
                Width = 140,
                BorderRadius = 8,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight
            };
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Dock = DockStyle.Right,
                Width = 100,
                BorderRadius = 8,
                NormalColor = Color.FromArgb(226, 232, 240),
                HoverColor = Color.FromArgb(203, 213, 225),
                ForeColor = AppColors.TextDark
            };
            _btnCancel.Click += (s, e) => Close();

            bottom.Controls.Add(_btnSave);
            bottom.Controls.Add(new Panel { Dock = DockStyle.Right, Width = 10 });
            bottom.Controls.Add(_btnCancel);

            Controls.Add(content);
            Controls.Add(bottom);
            Controls.Add(header);
        }

        private void PopulateData()
        {
            _cboDoughIngredient.Items.Clear();
            foreach (var ing in _doughIngredients)
            {
                _cboDoughIngredient.Items.Add(ing);
            }

            if (!string.IsNullOrEmpty(_doughSize.SizeName))
            {
                _cboSizeName.Text = _doughSize.SizeName;
                _numPrice.Value = _doughSize.Price > 0 ? _doughSize.Price : 14.99m;
                _numPortion.Value = _doughSize.DoughPortionQty > 0 ? _doughSize.DoughPortionQty : 1.00m;
                _chkDefault.Checked = _doughSize.IsDefault;

                if (_doughSize.IngredientId.HasValue)
                {
                    for (int i = 0; i < _cboDoughIngredient.Items.Count; i++)
                    {
                        if (_cboDoughIngredient.Items[i] is Ingredient ing && ing.IngredientId == _doughSize.IngredientId.Value)
                        {
                            _cboDoughIngredient.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                _cboSizeName.Text = "Medium 12\"";
            }

            if (_cboDoughIngredient.SelectedIndex == -1 && _cboDoughIngredient.Items.Count > 0)
            {
                _cboDoughIngredient.SelectedIndex = 0;
            }

            UpdateStockAndDeduction();
        }

        private void CboSizeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sel = _cboSizeName.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrEmpty(_doughSize.SizeName)) // Only auto-suggest if new size
            {
                if (sel.StartsWith("Small") || sel.StartsWith("Personal"))
                {
                    _numPortion.Value = 0.75m;
                    _numPrice.Value = 11.99m;
                }
                else if (sel.StartsWith("Medium"))
                {
                    _numPortion.Value = 1.00m;
                    _numPrice.Value = 14.99m;
                }
                else if (sel.StartsWith("Large"))
                {
                    _numPortion.Value = 1.50m;
                    _numPrice.Value = 18.99m;
                }
                else if (sel.StartsWith("Extra Large") || sel.StartsWith("XL"))
                {
                    _numPortion.Value = 2.00m;
                    _numPrice.Value = 22.99m;
                }
                UpdateStockAndDeduction();
            }
        }

        private void UpdateStockAndDeduction()
        {
            if (_cboDoughIngredient.SelectedItem is Ingredient ing)
            {
                _lblUnit.Text = ing.Unit;
                _lblStockStatus.Text = $"Stock Available: {ing.CurrentStock:N1} {ing.Unit}";
                _lblStockStatus.ForeColor = ing.CurrentStock > ing.MinThreshold ? AppColors.StatusCompleted : (ing.CurrentStock > 0 ? AppColors.StatusPending : AppColors.StatusCancelled);

                decimal portion = _numPortion.Value;
                _lblDeductionInfo.Text = $"Stock Deduction: {portion:N2} {ing.Unit} per item";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string sizeName = _cboSizeName.Text.Trim();
            if (string.IsNullOrEmpty(sizeName))
            {
                MessageBox.Show("Please select or enter a dough size / crust name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _cboSizeName.Focus();
                return;
            }

            if (_numPrice.Value <= 0)
            {
                MessageBox.Show("Please enter a valid selling price greater than ₱0.00.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _numPrice.Focus();
                return;
            }

            if (_cboDoughIngredient.SelectedItem == null)
            {
                MessageBox.Show("Please select a dough ingredient to link for live stock tracking.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _cboDoughIngredient.Focus();
                return;
            }

            if (_numPortion.Value <= 0)
            {
                MessageBox.Show("Dough portion required must be greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _numPortion.Focus();
                return;
            }

            var ing = _cboDoughIngredient.SelectedItem as Ingredient;
            if (ing != null && ing.CurrentStock <= 0)
            {
                var res = MessageBox.Show(
                    $"Warning: The selected dough ingredient '{ing.Name}' currently has 0 {ing.Unit} available in stock.\n\nThis size will appear as 'Out of Stock' to customers until inventory is restocked.\n\nDo you want to save this dough size anyway?",
                    "Dough Stock Depleted",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (res != DialogResult.Yes)
                {
                    return;
                }
            }

            _doughSize.SizeName = sizeName;
            _doughSize.Price = _numPrice.Value;
            _doughSize.Cost = ing != null ? Math.Round(_numPortion.Value * ing.CostPerUnit, 2) : 0m;
            _doughSize.DoughPortionQty = _numPortion.Value;
            _doughSize.IsDefault = _chkDefault.Checked;

            if (ing != null)
            {
                _doughSize.IngredientId = ing.IngredientId;
                _doughSize.DoughIngredientName = ing.Name;
                _doughSize.CurrentStockAvailable = ing.CurrentStock;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
