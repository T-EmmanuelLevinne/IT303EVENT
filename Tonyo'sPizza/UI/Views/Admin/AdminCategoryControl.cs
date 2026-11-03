using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Forms.Dialogs;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminCategoryControl : UserControl
    {
        private readonly CategoryRepository _catRepo;

        public AdminCategoryControl()
        {
            _catRepo = new CategoryRepository();
            InitializeComponent();
            SetupCustomUIAndEvents();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                LoadCategoryGrid();
            }
        }

        private void SetupCustomUIAndEvents()
        {
            if (lblSearchIcon != null)
            {
                lblSearchIcon.Paint += (s, e) =>
                {
                    UIHelper.SetHighQuality(e.Graphics);
                    UIHelper.DrawVectorIcon(e.Graphics, "search", new Rectangle(0, 0, 16, 16), AppColors.TextLight);
                };
            }

            if (_txtSearch != null) _txtSearch.TextChanged += (s, e) => LoadCategoryGrid();
            if (_btnAdd != null) _btnAdd.Click += BtnAdd_Click;
            if (_btnEdit != null) _btnEdit.Click += BtnEdit_Click;
            if (_btnDelete != null) _btnDelete.Click += BtnDelete_Click;
            if (_btnRefresh != null) _btnRefresh.Click += (s, e) => LoadCategoryGrid();

            if (_dgvCategories != null)
            {
                UIHelper.StyleDataGridView(_dgvCategories);
                if (_dgvCategories.Columns.Count == 0)
                {
                    _dgvCategories.Columns.Add("Id", "ID");
                    _dgvCategories.Columns.Add("Name", "Category Name");
                    _dgvCategories.Columns.Add("Icon", "Icon / Type");
                    _dgvCategories.Columns.Add("Description", "Description");
                    _dgvCategories.Columns.Add("CreatedAt", "Created At");

                    _dgvCategories.Columns["Id"].Width = 60;
                    _dgvCategories.Columns["Icon"].Width = 120;
                    _dgvCategories.Columns["CreatedAt"].Width = 160;
                    _dgvCategories.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                _dgvCategories.CellMouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                    {
                        _dgvCategories.ClearSelection();
                        _dgvCategories.Rows[e.RowIndex].Selected = true;
                        var cat = GetSelectedCategory();
                        if (cat != null)
                        {
                            var menu = UIHelper.CreateModernContextMenu();
                            var itemEdit = UIHelper.CreateModernMenuItem("Edit Category", "edit", (sender, args) => BtnEdit_Click(this, EventArgs.Empty));
                            var itemDelete = UIHelper.CreateModernMenuItem("Delete Category", "delete", (sender, args) => BtnDelete_Click(this, EventArgs.Empty), Color.FromArgb(220, 38, 38));

                            menu.Items.Add(itemEdit);
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(itemDelete);

                            menu.Show(Cursor.Position);
                        }
                    }
                };
            }
        }

        public void LoadCategoryGrid()
        {
            _dgvCategories.Rows.Clear();
            try
            {
                string q = _txtSearch?.Text?.Trim().ToLower() ?? "";
                var categories = _catRepo.GetAllCategories();
                if (!string.IsNullOrEmpty(q))
                {
                    categories = categories.Where(c => (c.CategoryName ?? "").ToLower().Contains(q) || (c.Description ?? "").ToLower().Contains(q)).ToList();
                }
                foreach (var cat in categories)
                {
                    _dgvCategories.Rows.Add(
                        cat.CategoryId,
                        cat.CategoryName,
                        cat.IconKey ?? "-",
                        cat.Description,
                        cat.CreatedAt.ToString("g")
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Category GetSelectedCategory()
        {
            if (_dgvCategories.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(_dgvCategories.SelectedRows[0].Cells["Id"].Value);
                return _catRepo.GetCategoryById(id);
            }
            return null;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new AddEditCategoryForm())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadCategoryGrid();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var cat = GetSelectedCategory();
            if (cat == null)
            {
                MessageBox.Show("Please select a category to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new AddEditCategoryForm(cat))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadCategoryGrid();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var cat = GetSelectedCategory();
            if (cat == null)
            {
                MessageBox.Show("Please select a category to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete category '{cat.CategoryName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_catRepo.DeleteCategory(cat.CategoryId))
                {
                    LoadCategoryGrid();
                }
                else
                {
                    MessageBox.Show("Failed to delete category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
