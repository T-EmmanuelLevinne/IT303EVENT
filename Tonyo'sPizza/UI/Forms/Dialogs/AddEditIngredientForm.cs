using System;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class AddEditIngredientForm : Form
    {
        private readonly Ingredient _ingredient;
        private readonly bool _isEdit;
        private readonly IngredientCategoryRepository _catRepo;

        private TextBox _txtName;
        private ComboBox _cboCategory;
        private RoundedButton _btnNewCategory;
        private NumericUpDown _numStock;
        private ComboBox _cboUnit;
        private NumericUpDown _numMinThreshold;
        private RoundedButton _btnSave;
        private RoundedButton _btnCancel;

        public Ingredient ResultIngredient => _ingredient;
        public Ingredient Ingredient => _ingredient;

        public AddEditIngredientForm(Ingredient ingredient = null)
        {
            _catRepo = new IngredientCategoryRepository();
            _ingredient = ingredient ?? new Ingredient();
            _isEdit = ingredient != null;

            InitializeComponent();
            LoadCategories();
            PopulateData();
        }

        private void InitializeComponent()
        {
            Text = _isEdit ? "Edit Ingredient" : "Add New Ingredient";
            Size = new Size(460, 510);
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
                Text = _isEdit ? "Edit Ingredient" : "Add New Stock Ingredient",
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

            int y = 14;

            Label lblName = new Label { Text = "Ingredient Name *", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 24;
            _txtName = new TextBox { Location = new Point(24, y), Width = 390, Font = new Font("Segoe UI", 10f) };
            y += 34;

            Label lblCat = new Label { Text = "Ingredient Category * (e.g. Dough, Cheese, Veggie, Meat, Sauce)", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 22;
            _cboCategory = new ComboBox { Location = new Point(24, y), Width = 250, Font = new Font("Segoe UI", 9.5f), DropDownStyle = ComboBoxStyle.DropDownList };
            
            _btnNewCategory = new RoundedButton
            {
                Text = "+ New Category",
                Location = new Point(282, y - 2),
                Size = new Size(132, 32),
                BorderRadius = 6,
                NormalColor = Color.FromArgb(71, 85, 105),
                HoverColor = Color.FromArgb(100, 116, 139),
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold)
            };
            _btnNewCategory.Click += BtnNewCategory_Click;
            y += 36;

            Label lblStock = new Label { Text = "Current Stock *", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            Label lblUnit = new Label { Text = "Unit *", Location = new Point(230, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 24;
            _numStock = new NumericUpDown { Location = new Point(24, y), Width = 190, Font = new Font("Segoe UI", 10f), DecimalPlaces = 2, Maximum = 999999, Minimum = 0 };
            _cboUnit = new ComboBox { Location = new Point(230, y), Width = 184, Font = new Font("Segoe UI", 10f), DropDownStyle = ComboBoxStyle.DropDownList };
            _cboUnit.Items.AddRange(new object[] { "g", "kg", "pcs", "ml", "L", "slices", "packs" });
            _cboUnit.SelectedIndex = 0;
            y += 36;

            Label lblMin = new Label { Text = "Low Stock Alert Threshold *", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 24;
            _numMinThreshold = new NumericUpDown { Location = new Point(24, y), Width = 390, Font = new Font("Segoe UI", 10f), DecimalPlaces = 2, Maximum = 999999, Minimum = 0, Value = 10 };
            y += 46;

            _btnSave = new RoundedButton
            {
                Text = _isEdit ? "Apply Changes" : "Add Ingredient",
                Location = new Point(24, y),
                Size = new Size(220, 42),
                BorderRadius = 8,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight
            };
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Location = new Point(254, y),
                Size = new Size(160, 42),
                BorderRadius = 8,
                NormalColor = Color.FromArgb(226, 232, 240),
                HoverColor = Color.FromArgb(203, 213, 225),
                ForeColor = AppColors.TextDark
            };
            _btnCancel.Click += (s, e) => Close();

            content.Controls.Add(lblName);
            content.Controls.Add(_txtName);
            content.Controls.Add(lblCat);
            content.Controls.Add(_cboCategory);
            content.Controls.Add(_btnNewCategory);
            content.Controls.Add(lblStock);
            content.Controls.Add(_numStock);
            content.Controls.Add(lblUnit);
            content.Controls.Add(_cboUnit);
            content.Controls.Add(lblMin);
            content.Controls.Add(_numMinThreshold);
            content.Controls.Add(_btnSave);
            content.Controls.Add(_btnCancel);

            Controls.Add(content);
            Controls.Add(header);
        }

        private void LoadCategories()
        {
            _cboCategory.Items.Clear();
            var cats = _catRepo.GetAllCategories();
            if (cats.Count == 0)
            {
                string[] defaults = { "Dough", "Cheese", "Veggie", "Meat", "Sauce" };
                foreach (var d in defaults)
                {
                    var c = new IngredientCategory { CategoryName = d };
                    _catRepo.InsertCategory(c);
                    cats.Add(c);
                }
            }

            foreach (var c in cats)
            {
                _cboCategory.Items.Add(c);
            }

            if (_cboCategory.Items.Count > 0 && _cboCategory.SelectedIndex == -1)
            {
                _cboCategory.SelectedIndex = 0;
            }
        }

        private void BtnNewCategory_Click(object sender, EventArgs e)
        {
            string newCatName = PromptInput("Enter New Ingredient Category Name:", "New Ingredient Category");
            if (!string.IsNullOrWhiteSpace(newCatName))
            {
                newCatName = newCatName.Trim();
                var newCat = new IngredientCategory { CategoryName = newCatName };
                if (_catRepo.InsertCategory(newCat))
                {
                    LoadCategories();
                    for (int i = 0; i < _cboCategory.Items.Count; i++)
                    {
                        if (_cboCategory.Items[i] is IngredientCategory ic && ic.CategoryName.Equals(newCatName, StringComparison.OrdinalIgnoreCase))
                        {
                            _cboCategory.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private string PromptInput(string prompt, string title)
        {
            using (Form promptForm = new Form())
            {
                promptForm.Width = 380;
                promptForm.Height = 180;
                promptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                promptForm.Text = title;
                promptForm.StartPosition = FormStartPosition.CenterParent;
                promptForm.MaximizeBox = false;
                promptForm.MinimizeBox = false;
                promptForm.BackColor = Color.White;

                Label textLabel = new Label() { Left = 20, Top = 16, Text = prompt, AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold) };
                TextBox textBox = new TextBox() { Left = 20, Top = 44, Width = 320, Font = new Font("Segoe UI", 10f) };
                
                RoundedButton confirmation = new RoundedButton() { Text = "Add Category", Left = 180, Width = 160, Top = 82, Height = 36, BorderRadius = 6, NormalColor = AppColors.Primary, HoverColor = AppColors.PrimaryLight };
                confirmation.Click += (s, e) => { promptForm.DialogResult = DialogResult.OK; promptForm.Close(); };

                promptForm.Controls.Add(textLabel);
                promptForm.Controls.Add(textBox);
                promptForm.Controls.Add(confirmation);
                promptForm.AcceptButton = confirmation;

                return promptForm.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }

        private void PopulateData()
        {
            if (_isEdit)
            {
                _txtName.Text = _ingredient.Name;
                _numStock.Value = _ingredient.CurrentStock;
                _numMinThreshold.Value = _ingredient.MinThreshold;

                int idx = _cboUnit.FindStringExact(_ingredient.Unit);
                if (idx >= 0) _cboUnit.SelectedIndex = idx;

                if (!string.IsNullOrEmpty(_ingredient.CategoryName))
                {
                    for (int i = 0; i < _cboCategory.Items.Count; i++)
                    {
                        if (_cboCategory.Items[i] is IngredientCategory ic && ic.CategoryName.Equals(_ingredient.CategoryName, StringComparison.OrdinalIgnoreCase))
                        {
                            _cboCategory.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string name = _txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter an ingredient name. Ingredient Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtName.Focus();
                return;
            }

            if (_numStock.Value <= 0)
            {
                MessageBox.Show("Please enter a valid stock quantity greater than 0. Current Stock is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _numStock.Focus();
                return;
            }

            var ingRepo = new IngredientRepository();
            if (ingRepo.IngredientNameExists(name, _isEdit ? (int?)_ingredient.IngredientId : null))
            {
                MessageBox.Show($"An ingredient with the name '{name}' already exists. Please choose a different name.", "Duplicate Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtName.Focus();
                return;
            }

            _ingredient.Name = name;
            _ingredient.CurrentStock = _numStock.Value;
            _ingredient.Unit = _cboUnit.SelectedItem?.ToString() ?? "g";
            _ingredient.MinThreshold = _numMinThreshold.Value;

            if (_cboCategory.SelectedItem is IngredientCategory cat)
            {
                _ingredient.CategoryId = cat.IngredientCategoryId;
                _ingredient.CategoryName = cat.CategoryName;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
