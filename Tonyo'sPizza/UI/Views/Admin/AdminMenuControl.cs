using System;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Forms.Dialogs;
using Tonyo_sPizza.UI.Theme;
using MenuItem = Tonyo_sPizza.Models.MenuItem;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminMenuControl : UserControl
    {
        private readonly MenuItemRepository _menuRepo;
        private readonly CategoryRepository _catRepo;

        public AdminMenuControl()
        {
            _menuRepo = new MenuItemRepository();
            _catRepo = new CategoryRepository();
            InitializeComponent();
            SetupCustomUIAndEvents();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                LoadCategoryFilter();
                LoadMenuGrid();
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

            if (_txtSearch != null)
            {
                _txtSearch.TextChanged += (s, e) => LoadMenuGrid();
            }

            if (_cboCategoryFilter != null)
            {
                _cboCategoryFilter.SelectedIndexChanged += (s, e) => LoadMenuGrid();
            }

            if (_btnAdd != null) _btnAdd.Click += BtnAdd_Click;
            if (_btnEdit != null) _btnEdit.Click += BtnEdit_Click;
            if (_btnDelete != null) _btnDelete.Click += BtnDelete_Click;
            if (_btnRefresh != null) _btnRefresh.Click += (s, e) => { LoadCategoryFilter(); LoadMenuGrid(); };

            if (_dgvMenu != null)
            {
                UIHelper.StyleDataGridView(_dgvMenu);
                if (_dgvMenu.Columns.Count == 0)
                {
                    _dgvMenu.Columns.Add("Id", "ID");
                    _dgvMenu.Columns.Add("Name", "Item Name");
                    _dgvMenu.Columns.Add("Category", "Category");
                    _dgvMenu.Columns.Add("Price", "Price");
                    _dgvMenu.Columns.Add("InStock", "In Stock");

                    _dgvMenu.Columns["Id"].Width = 60;
                    _dgvMenu.Columns["Category"].Width = 140;
                    _dgvMenu.Columns["Price"].Width = 120;
                    _dgvMenu.Columns["InStock"].Width = 130;
                    _dgvMenu.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                _dgvMenu.CellFormatting += DgvMenu_CellFormatting;

                _dgvMenu.CellDoubleClick += (s, e) =>
                {
                    if (e.RowIndex >= 0) BtnEdit_Click(this, EventArgs.Empty);
                };

                _dgvMenu.MouseClick += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        var hti = _dgvMenu.HitTest(e.X, e.Y);
                        if (hti.RowIndex >= 0)
                        {
                            _dgvMenu.ClearSelection();
                            _dgvMenu.Rows[hti.RowIndex].Selected = true;

                            var item = GetSelectedItem();
                            if (item != null)
                            {
                                var menu = UIHelper.CreateModernContextMenu();
                                var itemEdit = UIHelper.CreateModernMenuItem("Edit Menu Item", "edit", (sender, args) => BtnEdit_Click(this, EventArgs.Empty));
                                var itemRemove = UIHelper.CreateModernMenuItem("Remove Menu Item", "delete", (sender, args) => BtnDelete_Click(this, EventArgs.Empty), Color.FromArgb(220, 38, 38));

                                menu.Items.Add(itemEdit);
                                menu.Items.Add(new ToolStripSeparator());
                                menu.Items.Add(itemRemove);

                                menu.Show(Cursor.Position);
                            }
                        }
                    }
                };
            }
        }

        private void LoadCategoryFilter()
        {
            if (_cboCategoryFilter == null) return;
            var cats = _catRepo.GetAllCategories();
            _cboCategoryFilter.Items.Clear();
            _cboCategoryFilter.Items.Add(new Category { CategoryId = 0, CategoryName = "All Categories" });
            foreach (var c in cats) _cboCategoryFilter.Items.Add(c);
            _cboCategoryFilter.DisplayMember = "CategoryName";
            _cboCategoryFilter.ValueMember = "CategoryId";
            _cboCategoryFilter.SelectedIndex = 0;
        }

        public void LoadMenuGrid()
        {
            _dgvMenu.Rows.Clear();
            try
            {
                string search = _txtSearch.Text.Trim();
                int? categoryId = null;
                if (_cboCategoryFilter?.SelectedItem is Category cat && cat.CategoryId > 0)
                {
                    categoryId = cat.CategoryId;
                }

                var items = _menuRepo.GetMenuItemsFiltered(search, categoryId);

                StockService.Instance.RefreshIfClean();
                foreach (var itm in items)
                {
                    int portions = StockService.Instance.GetAvailableStockForMenuItem(itm);
                    bool inStock = itm.IsAvailable && portions > 0;
                    string stockText = inStock ? "In Stock" : "Out of Stock";

                    _dgvMenu.Rows.Add(
                        itm.ItemId,
                        itm.Name,
                        itm.CategoryName,
                        $"₱{itm.Price:N2}",
                        stockText
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading menu: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvMenu_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string col = _dgvMenu.Columns[e.ColumnIndex].Name;

            if (col == "InStock")
            {
                string status = e.Value.ToString();
                if (status == "Out of Stock")
                {
                    e.CellStyle.ForeColor = AppColors.StatusCancelled;
                    e.CellStyle.Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = AppColors.StatusCompleted;
                    e.CellStyle.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
                }
            }
        }

        private MenuItem GetSelectedItem()
        {
            if (_dgvMenu.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(_dgvMenu.SelectedRows[0].Cells["Id"].Value);
                return _menuRepo.GetMenuItemById(id);
            }
            return null;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new AddEditMenuItemForm())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadMenuGrid();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (item == null)
            {
                MessageBox.Show("Please select an item to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new AddEditMenuItemForm(item))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadMenuGrid();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (item == null)
            {
                MessageBox.Show("Please select an item to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete '{item.Name}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_menuRepo.DeleteMenuItem(item.ItemId))
                {
                    LoadMenuGrid();
                }
                else
                {
                    MessageBox.Show("Failed to delete menu item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
