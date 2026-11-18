using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Forms.Dialogs;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminCategoryControl : UserControl
    {
        private readonly CategoryRepository _catRepo;
        private readonly Dictionary<int, Category> _rowMap = new Dictionary<int, Category>();

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
            UIHelper.EnableDoubleBuffering(this);

            if (lblSearchIcon != null)
            {
                lblSearchIcon.Paint += (s, e) =>
                {
                    UIHelper.SetHighQuality(e.Graphics);
                    UIHelper.DrawVectorIcon(e.Graphics, "search", new Rectangle(0, 0, 15, 15), AppColors.TextLight);
                };
            }

            if (_txtSearch != null) _txtSearch.TextChanged += (s, e) => LoadCategoryGrid();
            if (_chkShowArchived != null) _chkShowArchived.CheckedChanged += (s, e) => LoadCategoryGrid();
            if (_btnAdd != null) _btnAdd.Click += BtnAdd_Click;
            if (_btnEdit != null) _btnEdit.Click += BtnEdit_Click;
            if (_btnToggleArchive != null) _btnToggleArchive.Click += BtnToggleArchive_Click;
            if (_btnDelete != null) _btnDelete.Click += BtnDelete_Click;
            if (_btnRefresh != null) _btnRefresh.Click += (s, e) => LoadCategoryGrid();

            if (_dgvCategories != null)
            {
                UIHelper.StyleDataGridView(_dgvCategories);
                _dgvCategories.Columns.Clear();
                _dgvCategories.Columns.Add("Id", "ID");
                _dgvCategories.Columns.Add("Name", "Category Name");
                _dgvCategories.Columns.Add("Icon", "Icon / Type");
                _dgvCategories.Columns.Add("Status", "Status");
                _dgvCategories.Columns.Add("Description", "Description");
                _dgvCategories.Columns.Add("CreatedAt", "Created At");

                UIHelper.ConfigureGridColumn(_dgvCategories, "Id", 52);
                UIHelper.ConfigureGridColumn(_dgvCategories, "Name", 160);
                UIHelper.ConfigureGridColumn(_dgvCategories, "Icon", 120);
                UIHelper.ConfigureGridColumn(_dgvCategories, "Status", 104);
                UIHelper.ConfigureGridColumn(_dgvCategories, "Description", 120, fill: true);
                UIHelper.ConfigureGridColumn(_dgvCategories, "CreatedAt", 150);

                UIHelper.AttachStatusPillPainter(_dgvCategories, UIHelper.ResolveSemanticStatusColors, "Status");
                _dgvCategories.SelectionChanged += (s, e) => UpdateActionButtonStates();
                _dgvCategories.CellDoubleClick += (s, e) =>
                {
                    if (e.RowIndex >= 0) BtnEdit_Click(this, EventArgs.Empty);
                };

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
                            menu.Items.Add(UIHelper.CreateModernMenuItem($"Edit '{cat.CategoryName}'", "edit", (sender, args) => BtnEdit_Click(this, EventArgs.Empty)));
                            menu.Items.Add(UIHelper.CreateModernMenuItem(
                                cat.IsActive ? "Archive Category" : "Restore Category",
                                "archive",
                                (sender, args) => BtnToggleArchive_Click(this, EventArgs.Empty),
                                cat.IsActive ? AppColors.StatusPending : AppColors.StatusCompleted));
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(UIHelper.CreateModernMenuItem("Delete Category", "delete", (sender, args) => BtnDelete_Click(this, EventArgs.Empty), AppColors.StatusCancelled));
                            menu.Show(Cursor.Position);
                        }
                    }
                };
            }
        }

        public void LoadCategoryGrid()
        {
            _dgvCategories.Rows.Clear();
            _rowMap.Clear();
            try
            {
                string q = _txtSearch?.Text?.Trim().ToLower() ?? "";
                bool includeArchived = _chkShowArchived != null && _chkShowArchived.Checked;

                var categories = _catRepo.GetAllCategories(includeArchived);
                if (!string.IsNullOrEmpty(q))
                {
                    categories = categories.Where(c =>
                        (c.CategoryName ?? "").ToLower().Contains(q) ||
                        (c.Description ?? "").ToLower().Contains(q) ||
                        (c.IconKey ?? "").ToLower().Contains(q)).ToList();
                }

                foreach (var cat in categories)
                {
                    _rowMap[cat.CategoryId] = cat;
                    int rowIdx = _dgvCategories.Rows.Add(
                        cat.CategoryId,
                        cat.CategoryName,
                        cat.IconKey ?? "-",
                        cat.IsActive ? "Active" : "Archived",
                        cat.Description,
                        cat.CreatedAt.ToString("g")
                    );

                    if (!cat.IsActive)
                    {
                        _dgvCategories.Rows[rowIdx].DefaultCellStyle.ForeColor = AppColors.TextMuted;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Unable to load categories right now.", ex, "Category Load Error", this);
            }
            UpdateActionButtonStates();
        }

        private void UpdateActionButtonStates()
        {
            if (_dgvCategories.SelectedRows.Count > 0 && _dgvCategories.SelectedRows[0].Cells["Id"].Value != null)
            {
                int id = Convert.ToInt32(_dgvCategories.SelectedRows[0].Cells["Id"].Value);
                if (_rowMap.TryGetValue(id, out var cat))
                {
                    if (cat.IsActive)
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
                    return;
                }
            }
            _btnToggleArchive.Text = "Archive";
        }

        private Category GetSelectedCategory()
        {
            if (_dgvCategories.SelectedRows.Count > 0 && _dgvCategories.SelectedRows[0].Cells["Id"].Value != null)
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
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadCategoryGrid();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var cat = GetSelectedCategory();
                if (cat == null)
                {
                    MessageBox.Show(this, "Please select a category to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var dlg = new AddEditCategoryForm(cat))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadCategoryGrid();
                    }
                }
            }
            catch (DBConcurrencyException cex)
            {
                MessageBox.Show(this, cex.Message, "Concurrent Edit Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadCategoryGrid();
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Could not edit the selected category.", ex, "Edit Error", this);
            }
        }

        private void BtnToggleArchive_Click(object sender, EventArgs e)
        {
            try
            {
                var cat = GetSelectedCategory();
                if (cat == null)
                {
                    MessageBox.Show(this, "Please select a category first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                bool targetActive = !cat.IsActive;
                string verb = targetActive ? "Restore" : "Archive";
                string prompt = targetActive
                    ? $"Restore category '{cat.CategoryName}' so it appears on the active menu again?"
                    : $"Archive category '{cat.CategoryName}'?";

                if (MessageBox.Show(this, prompt, $"Confirm {verb}", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_catRepo.SetCategoryActiveStatus(cat.CategoryId, targetActive))
                    {
                        AuditService.Instance.Log(targetActive ? "Restore" : "Archive", $"{verb}d category '{cat.CategoryName}'.", "Categories", cat.CategoryId.ToString());
                        LoadCategoryGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Could not update category archive status.", ex, "Archive Error", this);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var cat = GetSelectedCategory();
                if (cat == null)
                {
                    MessageBox.Show(this, "Please select a category to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (_catRepo.HasCategoryMenuItems(cat.CategoryId))
                {
                    if (cat.IsActive)
                    {
                        if (MessageBox.Show(
                            this,
                            $"Category '{cat.CategoryName}' still has menu items assigned to it and cannot be permanently deleted.\n\nWould you like to Archive this category instead?",
                            "Category In Use",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            _catRepo.SetCategoryActiveStatus(cat.CategoryId, false);
                            AuditService.Instance.Log("Archive", $"Archived referenced category '{cat.CategoryName}'.", "Categories", cat.CategoryId.ToString());
                            LoadCategoryGrid();
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, $"Category '{cat.CategoryName}' is linked to menu items and is already archived.", "Category In Use", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                if (MessageBox.Show(this, $"Permanently delete category '{cat.CategoryName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (_catRepo.DeleteCategory(cat.CategoryId))
                    {
                        AuditService.Instance.Log("Delete", $"Permanently deleted category '{cat.CategoryName}'.", "Categories", cat.CategoryId.ToString());
                        LoadCategoryGrid();
                    }
                    else
                    {
                        MessageBox.Show(this, "Failed to delete category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Unable to delete this category because it may be referenced by existing items.", ex, "Delete Error", this);
            }
        }
    }
}
