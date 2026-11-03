using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Forms.Dialogs;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminStockControl : UserControl
    {
        private readonly StockService _stockService;

        public AdminStockControl()
        {
            _stockService = StockService.Instance;
            InitializeComponent();
            SetupCustomUIAndEvents();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                _stockService.StockDataChanged += RefreshGrid;
                RefreshGrid();
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
                _txtSearch.TextChanged += (s, e) => RefreshGrid();
            }

            if (_cboCategoryFilter != null)
            {
                _cboCategoryFilter.SelectedIndexChanged += (s, e) => RefreshGrid();
            }

            if (_btnAddIngredient != null) _btnAddIngredient.Click += BtnAddIngredient_Click;
            if (_btnRestock != null) _btnRestock.Click += BtnRestock_Click;
            if (_btnEdit != null) _btnEdit.Click += BtnEdit_Click;
            if (_btnDelete != null) _btnDelete.Click += BtnDelete_Click;
            if (_btnSaveChanges != null) _btnSaveChanges.Click += BtnSaveChanges_Click;
            if (_btnCancelChanges != null) _btnCancelChanges.Click += BtnCancelChanges_Click;

            if (_dgvStock != null)
            {
                UIHelper.StyleDataGridView(_dgvStock);
                if (_dgvStock.Columns.Count == 0)
                {
                    _dgvStock.Columns.Add("Id", "ID");
                    _dgvStock.Columns.Add("Name", "Ingredient Name");
                    _dgvStock.Columns.Add("Category", "Category");
                    _dgvStock.Columns.Add("CurrentStock", "Current Stock");
                    _dgvStock.Columns.Add("Unit", "Unit");
                    _dgvStock.Columns.Add("MinThreshold", "Min Threshold");
                    _dgvStock.Columns.Add("Status", "Stock Status");

                    _dgvStock.Columns["Id"].Width = 50;
                    _dgvStock.Columns["Category"].Width = 120;
                    _dgvStock.Columns["CurrentStock"].Width = 120;
                    _dgvStock.Columns["Unit"].Width = 80;
                    _dgvStock.Columns["MinThreshold"].Width = 130;
                    _dgvStock.Columns["Status"].Width = 130;
                    _dgvStock.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                _dgvStock.CellFormatting += DgvStock_CellFormatting;

                _dgvStock.CellMouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                    {
                        _dgvStock.ClearSelection();
                        _dgvStock.Rows[e.RowIndex].Selected = true;
                        var ing = GetSelectedIngredient();
                        if (ing != null)
                        {
                            var menu = UIHelper.CreateModernContextMenu();
                            var itemRestock = UIHelper.CreateModernMenuItem($"Restock ({ing.Name})..", "restock", (sender, args) => BtnRestock_Click(this, EventArgs.Empty));
                            var itemEdit = UIHelper.CreateModernMenuItem("Edit Ingredient", "edit", (sender, args) => BtnEdit_Click(this, EventArgs.Empty));
                            var itemDelete = UIHelper.CreateModernMenuItem("Delete Ingredient", "delete", (sender, args) => BtnDelete_Click(this, EventArgs.Empty), Color.FromArgb(220, 38, 38));

                            menu.Items.Add(itemRestock);
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(itemEdit);
                            menu.Items.Add(itemDelete);

                            menu.Show(Cursor.Position);
                        }
                    }
                };
            }
        }

        private void DgvStock_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_dgvStock.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "Out of Stock")
                {
                    e.CellStyle.ForeColor = AppColors.StatusCancelled;
                    e.CellStyle.Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold);
                }
                else if (status == "Low Stock")
                {
                    e.CellStyle.ForeColor = AppColors.StatusPending;
                    e.CellStyle.Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = AppColors.StatusCompleted;
                    e.CellStyle.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
                }
            }
        }

        public void RefreshStockData()
        {
            _stockService.RefreshIfClean();
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            _dgvStock.Rows.Clear();
            var stockList = _stockService.WorkingStock;
            if (stockList != null)
            {
                // Dynamically populate category dropdown if needed
                UpdateCategoryDropdownOptions(stockList);

                string q = _txtSearch?.Text?.Trim().ToLower() ?? "";
                var filtered = string.IsNullOrEmpty(q)
                    ? stockList
                    : stockList.Where(i => (i.Name ?? "").ToLower().Contains(q) ||
                                           (i.CategoryName ?? "").ToLower().Contains(q) ||
                                           (i.Unit ?? "").ToLower().Contains(q) ||
                                           (i.StockStatusDisplay ?? "").ToLower().Contains(q)).ToList();

                if (_cboCategoryFilter != null && _cboCategoryFilter.SelectedIndex > 0)
                {
                    string sel = _cboCategoryFilter.SelectedItem.ToString();
                    filtered = filtered.Where(i => string.Equals(i.CategoryName, sel, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                foreach (var ing in filtered)
                {
                    _dgvStock.Rows.Add(
                        ing.IngredientId,
                        ing.Name,
                        ing.CategoryName,
                        $"{ing.CurrentStock:N2}",
                        ing.Unit,
                        $"{ing.MinThreshold:N2}",
                        ing.StockStatusDisplay
                    );
                }
            }

            bool dirty = _stockService.HasPendingChanges;
            _lblPendingChangesBanner.Visible = dirty;
            _bottomPanel.Visible = dirty;
        }

        private Ingredient GetSelectedIngredient()
        {
            if (_dgvStock.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(_dgvStock.SelectedRows[0].Cells["Id"].Value);
                return _stockService.WorkingStock.Find(i => i.IngredientId == id);
            }
            return null;
        }

        private void UpdateCategoryDropdownOptions(List<Ingredient> stockList)
        {
            if (_cboCategoryFilter == null) return;
            var cats = stockList.Select(i => i.CategoryName).Where(c => !string.IsNullOrEmpty(c)).Distinct().OrderBy(c => c).ToList();
            if (_cboCategoryFilter.Items.Count != cats.Count + 1)
            {
                string prevSel = _cboCategoryFilter.SelectedItem?.ToString() ?? "All Categories";
                _cboCategoryFilter.Items.Clear();
                _cboCategoryFilter.Items.Add("All Categories");
                foreach (var c in cats) _cboCategoryFilter.Items.Add(c);
                int idx = _cboCategoryFilter.FindStringExact(prevSel);
                _cboCategoryFilter.SelectedIndex = idx >= 0 ? idx : 0;
            }
        }

        private void BtnAddIngredient_Click(object sender, EventArgs e)
        {
            using (var dlg = new AddEditIngredientForm())
            {
                if (dlg.ShowDialog() == DialogResult.OK && dlg.Ingredient != null)
                {
                    if (_stockService.AddNewIngredient(dlg.Ingredient, out string err))
                    {
                        MessageBox.Show("Ingredient added to inventory successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshGrid();
                    }
                    else
                    {
                        MessageBox.Show($"Failed to add ingredient: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnRestock_Click(object sender, EventArgs e)
        {
            var ing = GetSelectedIngredient();
            if (ing == null)
            {
                MessageBox.Show("Please select an ingredient from the list to restock.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new RestockDialogForm(ing))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    RefreshGrid();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var ing = GetSelectedIngredient();
            if (ing == null)
            {
                MessageBox.Show("Please select an ingredient to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new AddEditIngredientForm(ing))
            {
                if (dlg.ShowDialog() == DialogResult.OK && dlg.Ingredient != null)
                {
                    if (_stockService.UpdateIngredient(dlg.Ingredient, out string err))
                    {
                        MessageBox.Show("Ingredient updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshGrid();
                    }
                    else
                    {
                        MessageBox.Show($"Failed to update ingredient: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var ing = GetSelectedIngredient();
            if (ing == null)
            {
                MessageBox.Show("Please select an ingredient to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete ingredient '{ing.Name}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_stockService.DeleteIngredient(ing.IngredientId, out string err))
                {
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show($"Failed to delete ingredient: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancelChanges_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Discard all unsaved stock modifications?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {   
                _stockService.CancelChanges();
                RefreshGrid();
            }
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            if (_stockService.SaveChanges(out string err))
            {
                MessageBox.Show("All stock changes saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshGrid();
            }
            else
            {
                MessageBox.Show($"Failed to save changes: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _dgvStock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void _btnCancelChanges_Click(object sender, EventArgs e)
        {

        }

        private void title_Click(object sender, EventArgs e)
        {

        }

        private void flpActions_Paint(object sender, PaintEventArgs e)
        {

        }

        private void _btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void _dgvStock_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void title_Click_1(object sender, EventArgs e)
        {

        }
    }
}
