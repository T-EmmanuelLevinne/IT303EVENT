using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Forms.Dialogs;
using Tonyo_sPizza.UI.Theme;
using MenuItem = Tonyo_sPizza.Models.MenuItem;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminMenuControl : UserControl
    {
        private readonly MenuItemRepository _menuRepo;
        private readonly CategoryRepository _catRepo;
        private readonly Dictionary<int, MenuItem> _rowItemMap = new Dictionary<int, MenuItem>();

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
            UIHelper.EnableDoubleBuffering(this);

            if (lblSearchIcon != null)
            {
                lblSearchIcon.Paint += (s, e) =>
                {
                    UIHelper.SetHighQuality(e.Graphics);
                    UIHelper.DrawVectorIcon(e.Graphics, "search", new Rectangle(0, 0, 15, 15), AppColors.TextLight);
                };
            }

            if (_txtSearch != null) _txtSearch.TextChanged += (s, e) => LoadMenuGrid();
            if (_cboCategoryFilter != null) _cboCategoryFilter.SelectedIndexChanged += (s, e) => LoadMenuGrid();
            if (_chkShowArchived != null) _chkShowArchived.CheckedChanged += (s, e) => LoadMenuGrid();

            if (_btnAdd != null) _btnAdd.Click += BtnAdd_Click;
            if (_btnEdit != null) _btnEdit.Click += BtnEdit_Click;
            if (_btnToggleArchive != null) _btnToggleArchive.Click += BtnToggleArchive_Click;
            if (_btnDelete != null) _btnDelete.Click += BtnDelete_Click;
            if (_btnRefresh != null) _btnRefresh.Click += (s, e) => { LoadCategoryFilter(); LoadMenuGrid(); };

            if (_dgvMenu != null)
            {
                UIHelper.StyleDataGridView(_dgvMenu);
                _dgvMenu.Columns.Clear();
                _dgvMenu.Columns.Add("Id", "ID");
                _dgvMenu.Columns.Add("Name", "Item Name");
                _dgvMenu.Columns.Add("Category", "Category");
                _dgvMenu.Columns.Add("Price", "Price");
                _dgvMenu.Columns.Add("InStock", "Availability");

                UIHelper.ConfigureGridColumn(_dgvMenu, "Id", 56);
                UIHelper.ConfigureGridColumn(_dgvMenu, "Name", 160, fill: true);
                UIHelper.ConfigureGridColumn(_dgvMenu, "Category", 145);
                UIHelper.ConfigureGridColumn(_dgvMenu, "Price", 110);
                UIHelper.ConfigureGridColumn(_dgvMenu, "InStock", 130);

                UIHelper.AttachStatusPillPainter(_dgvMenu, UIHelper.ResolveSemanticStatusColors, "InStock");

                _dgvMenu.SelectionChanged += (s, e) => UpdateActionButtonStates();
                _dgvMenu.CellDoubleClick += (s, e) =>
                {
                    if (e.RowIndex >= 0) BtnEdit_Click(this, EventArgs.Empty);
                };

                _dgvMenu.CellMouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                    {
                        _dgvMenu.ClearSelection();
                        _dgvMenu.Rows[e.RowIndex].Selected = true;

                        var item = GetSelectedItem();
                        if (item != null)
                        {
                            var menu = UIHelper.CreateModernContextMenu();
                            menu.Items.Add(UIHelper.CreateModernMenuItem($"Edit '{item.Name}'", "edit", (sender, args) => BtnEdit_Click(this, EventArgs.Empty)));
                            menu.Items.Add(UIHelper.CreateModernMenuItem(
                                item.IsActive ? "Archive Item" : "Restore Item",
                                "archive",
                                (sender, args) => BtnToggleArchive_Click(this, EventArgs.Empty),
                                item.IsActive ? AppColors.StatusPending : AppColors.StatusCompleted));
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(UIHelper.CreateModernMenuItem("Delete Item", "delete", (sender, args) => BtnDelete_Click(this, EventArgs.Empty), AppColors.StatusCancelled));
                            menu.Show(Cursor.Position);
                        }
                    }
                };
            }
        }

        private void LoadCategoryFilter()
        {
            if (_cboCategoryFilter == null) return;
            try
            {
                int selectedCatId = (_cboCategoryFilter.SelectedItem as Category)?.CategoryId ?? 0;
                var cats = _catRepo.GetAllCategories(includeArchived: false);
                _cboCategoryFilter.Items.Clear();
                _cboCategoryFilter.Items.Add(new Category { CategoryId = 0, CategoryName = "All Categories" });
                int matchIdx = 0;
                for (int i = 0; i < cats.Count; i++)
                {
                    _cboCategoryFilter.Items.Add(cats[i]);
                    if (cats[i].CategoryId == selectedCatId) matchIdx = i + 1;
                }
                _cboCategoryFilter.DisplayMember = "CategoryName";
                _cboCategoryFilter.ValueMember = "CategoryId";
                _cboCategoryFilter.SelectedIndex = matchIdx;
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Unable to load category filters right now.", ex, "Category Filter Error", this);
            }
        }

        public void LoadMenuGrid()
        {
            _dgvMenu.Rows.Clear();
            _rowItemMap.Clear();
            try
            {
                string search = _txtSearch?.Text?.Trim();
                int? categoryId = null;
                if (_cboCategoryFilter?.SelectedItem is Category cat && cat.CategoryId > 0)
                {
                    categoryId = cat.CategoryId;
                }
                bool showArchived = _chkShowArchived != null && _chkShowArchived.Checked;

                var items = _menuRepo.GetMenuItemsFiltered(search, categoryId, showArchived);

                StockService.Instance.RefreshIfClean();
                foreach (var itm in items)
                {
                    _rowItemMap[itm.ItemId] = itm;
                    int portions = StockService.Instance.GetAvailableStockForMenuItem(itm);
                    string stockText;
                    if (!itm.IsActive)
                    {
                        stockText = "Archived";
                    }
                    else
                    {
                        bool inStock = itm.IsAvailable && portions > 0;
                        stockText = inStock ? "In Stock" : "Out of Stock";
                    }

                    int rowIdx = _dgvMenu.Rows.Add(
                        itm.ItemId,
                        itm.Name,
                        itm.CategoryName,
                        $"₱{itm.Price:N2}",
                        stockText
                    );

                    if (!itm.IsActive)
                    {
                        _dgvMenu.Rows[rowIdx].DefaultCellStyle.ForeColor = AppColors.TextMuted;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Could not load the menu catalog. Please try refreshing.", ex, "Menu Load Error", this);
            }
            UpdateActionButtonStates();
        }

        private void UpdateActionButtonStates()
        {
            var selected = GetSelectedItemFromCache();
            if (selected == null)
            {
                _btnToggleArchive.Text = "Archive";
                return;
            }

            if (selected.IsActive)
            {
                _btnToggleArchive.Text = "Archive";
                _btnToggleArchive.NormalColor = AppColors.StatusPendingBg;
                _btnToggleArchive.HoverColor = Color.FromArgb(253, 230, 138);
                _btnToggleArchive.ForeColor = AppColors.StatusPending;
            }
            else
            {
                _btnToggleArchive.Text = "Restore";
                _btnToggleArchive.NormalColor = AppColors.StatusCompletedBg;
                _btnToggleArchive.HoverColor = Color.FromArgb(187, 247, 208);
                _btnToggleArchive.ForeColor = AppColors.StatusCompleted;
            }
        }

        private MenuItem GetSelectedItemFromCache()
        {
            if (_dgvMenu.SelectedRows.Count > 0 && _dgvMenu.SelectedRows[0].Cells["Id"].Value != null)
            {
                int id = Convert.ToInt32(_dgvMenu.SelectedRows[0].Cells["Id"].Value);
                if (_rowItemMap.TryGetValue(id, out var cached)) return cached;
            }
            return null;
        }

        private MenuItem GetSelectedItem()
        {
            if (_dgvMenu.SelectedRows.Count > 0 && _dgvMenu.SelectedRows[0].Cells["Id"].Value != null)
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
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadMenuGrid();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var item = GetSelectedItem();
                if (item == null)
                {
                    MessageBox.Show(this, "Please select a menu item to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var dlg = new AddEditMenuItemForm(item))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadMenuGrid();
                    }
                }
            }
            catch (DBConcurrencyException cex)
            {
                MessageBox.Show(this, cex.Message, "Concurrent Edit Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadMenuGrid();
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Could not open menu item editor.", ex, "Edit Error", this);
            }
        }

        private void BtnToggleArchive_Click(object sender, EventArgs e)
        {
            try
            {
                var item = GetSelectedItem();
                if (item == null)
                {
                    MessageBox.Show(this, "Please select a menu item first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                bool targetActive = !item.IsActive;
                string verb = targetActive ? "Restore" : "Archive";
                string msg = targetActive
                    ? $"Restore '{item.Name}' back to the active menu?"
                    : $"Archive '{item.Name}' so customers can no longer order it while keeping past sales history?";

                if (MessageBox.Show(this, msg, $"Confirm {verb}", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_menuRepo.SetMenuItemActiveStatus(item.ItemId, targetActive))
                    {
                        AuditService.Instance.Log(targetActive ? "Restore" : "Archive", $"{verb}d menu item '{item.Name}'.", "MenuItems", item.ItemId.ToString());
                        LoadMenuGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Failed to update menu item archive status.", ex, "Archive Error", this);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var item = GetSelectedItem();
                if (item == null)
                {
                    MessageBox.Show(this, "Please select a menu item to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (_menuRepo.HasMenuItemOrders(item.ItemId))
                {
                    if (item.IsActive)
                    {
                        if (MessageBox.Show(
                            this,
                            $"'{item.Name}' cannot be permanently deleted because it appears in existing customer order history.\n\nWould you like to Archive it instead so it is hidden from ordering while keeping records intact?",
                            "Referenced by Orders",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            _menuRepo.SetMenuItemActiveStatus(item.ItemId, false);
                            AuditService.Instance.Log("Archive", $"Archived referenced menu item '{item.Name}'.", "MenuItems", item.ItemId.ToString());
                            LoadMenuGrid();
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, $"'{item.Name}' is already archived and has order history, so it cannot be permanently deleted.", "Referenced by Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                if (MessageBox.Show(this, $"Permanently delete '{item.Name}' from the catalog? This cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (_menuRepo.DeleteMenuItem(item.ItemId))
                    {
                        AuditService.Instance.Log("Delete", $"Permanently deleted menu item '{item.Name}'.", "MenuItems", item.ItemId.ToString());
                        LoadMenuGrid();
                    }
                    else
                    {
                        MessageBox.Show(this, "Failed to delete menu item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Could not delete this menu item. It may be referenced by existing records.", ex, "Delete Error", this);
            }
        }
    }
}
