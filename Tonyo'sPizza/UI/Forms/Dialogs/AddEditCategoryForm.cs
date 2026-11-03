using System;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class AddEditCategoryForm : Form
    {
        private readonly CategoryRepository _repo;
        private readonly Category _category;
        private readonly bool _isEdit;

        private TextBox _txtName;
        private ComboBox _cboIcon;
        private TextBox _txtDescription;
        private RoundedButton _btnSave;
        private RoundedButton _btnCancel;

        public Category ResultCategory => _category;

        public AddEditCategoryForm(Category category = null)
        {
            _repo = new CategoryRepository();
            _category = category ?? new Category();
            _isEdit = category != null;

            InitializeComponent();
            PopulateData();
        }

        private void InitializeComponent()
        {
            Text = _isEdit ? "Edit Category" : "Add New Category";
            Size = new Size(420, 380);
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
                Text = _isEdit ? "Edit Category" : "Add New Category",
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

            Label lblName = new Label { Text = "Category Name *", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 24;
            _txtName = new TextBox { Location = new Point(24, y), Width = 350, Font = new Font("Segoe UI", 10f) };
            y += 36;

            Label lblIcon = new Label { Text = "Icon / Type *", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 24;
            _cboIcon = new ComboBox { Location = new Point(24, y), Width = 350, Font = new Font("Segoe UI", 10f), DropDownStyle = ComboBoxStyle.DropDownList };
            _cboIcon.Items.AddRange(new object[] { "Pizza", "Burger", "Chinese", "Fast Food", "Pastry", "Chicken", "Beverage", "Salad", "Dessert" });
            _cboIcon.SelectedIndex = 0;
            y += 36;

            Label lblDesc = new Label { Text = "Description (Optional)", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 24;
            _txtDescription = new TextBox { Location = new Point(24, y), Width = 350, Height = 60, Multiline = true, Font = new Font("Segoe UI", 9.5f) };
            y += 72;

            _btnSave = new RoundedButton
            {
                Text = _isEdit ? "Update Category" : "Save Category",
                Location = new Point(24, y),
                Size = new Size(200, 40),
                BorderRadius = 8,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight
            };
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Location = new Point(234, y),
                Size = new Size(140, 40),
                BorderRadius = 8,
                NormalColor = Color.FromArgb(226, 232, 240),
                HoverColor = Color.FromArgb(203, 213, 225),
                ForeColor = AppColors.TextDark
            };
            _btnCancel.Click += (s, e) => Close();

            content.Controls.Add(lblName);
            content.Controls.Add(_txtName);
            content.Controls.Add(lblIcon);
            content.Controls.Add(_cboIcon);
            content.Controls.Add(lblDesc);
            content.Controls.Add(_txtDescription);
            content.Controls.Add(_btnSave);
            content.Controls.Add(_btnCancel);

            Controls.Add(content);
            Controls.Add(header);
        }

        private void PopulateData()
        {
            if (_isEdit)
            {
                _txtName.Text = _category.CategoryName;
                _txtDescription.Text = _category.Description;
                if (!string.IsNullOrEmpty(_category.IconKey))
                {
                    int idx = _cboIcon.FindStringExact(_category.IconKey);
                    if (idx >= 0) _cboIcon.SelectedIndex = idx;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string name = _txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a category name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _category.CategoryName = name;
                _category.IconKey = _cboIcon.SelectedItem?.ToString() ?? "Pizza";
                _category.Description = _txtDescription.Text.Trim();

                if (_isEdit)
                {
                    _repo.UpdateCategory(_category);
                }
                else
                {
                    _repo.InsertCategory(_category);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving category: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
