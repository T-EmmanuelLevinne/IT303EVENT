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
        private string _activeSubView = "Inventory"; // "Inventory" or "Transactions"

        public AdminStockControl()
        {
            _stockService = StockService.Instance;
            InitializeComponent();
            SetupCustomUIAndEvents();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                _stockService.StockDataChanged += RefreshActiveView;
                SwitchSubView("Inventory");
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

            _txtSearch.TextChanged += (s, e) => RefreshActiveView();
            _cboCategoryFilter.SelectedIndexChanged += (s, e) => RefreshGrid();
            _chkShowArchived.CheckedChanged += (s, e) =>
            {
                _stockService.IncludeArchived = _chkShowArchived.Checked;
                RefreshGrid();
            };

            _btnTabInventory.Click += (s, e) => SwitchSubView("Inventory");
            _btnTabTransactions.Click += (s, e) => SwitchSubView("Transactions");

            _btnAddIngredient.Click += BtnAddIngredient_Click;
            _btnRestock.Click += BtnRestock_Click;
            _btnEdit.Click += BtnEdit_Click;
            _btnArchiveToggle.Click += BtnArchiveToggle_Click;
            _btnDelete.Click += BtnDelete_Click;
            _btnSaveChanges.Click += BtnSaveChanges_Click;
            _btnCancelChanges.Click += BtnCancelChanges_Click;
            _btnVoidTransaction.Click += BtnVoidTransaction_Click;
            _btnRefreshTx.Click += (s, e) => LoadStockTransactions();

            // Inventory Grid
            UIHelper.StyleDataGridView(_dgvStock);
            _dgvStock.Columns.Add("Id", "ID");
            _dgvStock.Columns.Add("Name", "Ingredient Name");
            _dgvStock.Columns.Add("Category", "Category");
            _dgvStock.Columns.Add("CurrentStock", "Current Stock");
            _dgvStock.Columns.Add("Unit", "Unit");
            _dgvStock.Columns.Add("MinThreshold", "Min Stock");
            _dgvStock.Columns.Add("Status", "Stock Status");

            UIHelper.ConfigureGridColumn(_dgvStock, "Id", 48);
            UIHelper.ConfigureGridColumn(_dgvStock, "Name", 130, fill: true);
            UIHelper.ConfigureGridColumn(_dgvStock, "Category", 100);
            UIHelper.ConfigureGridColumn(_dgvStock, "CurrentStock", 122);
            UIHelper.ConfigureGridColumn(_dgvStock, "Unit", 64);
            UIHelper.ConfigureGridColumn(_dgvStock, "MinThreshold", 102);
            UIHelper.ConfigureGridColumn(_dgvStock, "Status", 120);

            UIHelper.AttachStatusPillPainter(_dgvStock, UIHelper.ResolveSemanticStatusColors, "Status");
            _dgvStock.SelectionChanged += (s, e) =>
            {
                var ing = GetSelectedIngredient();
                if (ing != null) _btnArchiveToggle.Text = ing.IsActive ? "Archive" : "Restore";
            };
            _dgvStock.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnRestock_Click(s, EventArgs.Empty); };

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
                        menu.Items.Add(UIHelper.CreateModernMenuItem($"Restock '{ing.Name}'", "restock", (sender, args) => BtnRestock_Click(this, EventArgs.Empty)));
                        menu.Items.Add(UIHelper.CreateModernMenuItem("Edit Ingredient", "edit", (sender, args) => BtnEdit_Click(this, EventArgs.Empty)));
                        menu.Items.Add(UIHelper.CreateModernMenuItem(ing.IsActive ? "Archive Ingredient" : "Restore Ingredient", "archive", (sender, args) => BtnArchiveToggle_Click(this, EventArgs.Empty), AppColors.StatusPending));
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(UIHelper.CreateModernMenuItem("Delete Permanently", "delete", (sender, args) => BtnDelete_Click(this, EventArgs.Empty), AppColors.StatusCancelled));
                        menu.Show(Cursor.Position);
                    }
                }
            };

            // Transactions Grid
            UIHelper.StyleDataGridView(_dgvTransactions);
            _dgvTransactions.Columns.Add("StockTxId", "Tx #");
            _dgvTransactions.Columns.Add("CreatedAt", "Date & Time");
            _dgvTransactions.Columns.Add("TransactionType", "Type");
            _dgvTransactions.Columns.Add("Username", "Staff");
            _dgvTransactions.Columns.Add("Items", "Ingredient Changes");
            _dgvTransactions.Columns.Add("Notes", "Notes");
            _dgvTransactions.Columns.Add("Status", "Status");

            UIHelper.ConfigureGridColumn(_dgvTransactions, "StockTxId", 60);
            UIHelper.ConfigureGridColumn(_dgvTransactions, "CreatedAt", 144);
            UIHelper.ConfigureGridColumn(_dgvTransactions, "TransactionType", 104);
            UIHelper.ConfigureGridColumn(_dgvTransactions, "Username", 96);
            UIHelper.ConfigureGridColumn(_dgvTransactions, "Items", 140, fill: true);
            UIHelper.ConfigureGridColumn(_dgvTransactions, "Notes", 140);
            UIHelper.ConfigureGridColumn(_dgvTransactions, "Status", 102);

            UIHelper.AttachStatusPillPainter(_dgvTransactions, UIHelper.ResolveSemanticStatusColors, "Status", "TransactionType");
            _dgvTransactions.CellMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                {
                    _dgvTransactions.ClearSelection();
                    _dgvTransactions.Rows[e.RowIndex].Selected = true;
                    if (_dgvTransactions.Rows[e.RowIndex].Tag is StockTransaction tx)
                    {
                        var menu = UIHelper.CreateModernContextMenu();
                        var itemRefresh = UIHelper.CreateModernMenuItem("Refresh Ledger", "refresh", (sender, args) => LoadStockTransactions());
                        menu.Items.Add(itemRefresh);

                        if (!string.Equals(tx.Status, "Voided", StringComparison.OrdinalIgnoreCase))
                        {
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(UIHelper.CreateModernMenuItem($"Void Transaction #{tx.StockTxId}", "delete", (sender, args) => BtnVoidTransaction_Click(this, EventArgs.Empty), AppColors.StatusCancelled));
                        }
                        menu.Show(Cursor.Position);
                    }
                }
            };
        }

        private void SwitchSubView(string view)
        {
            _activeSubView = view;
            bool isInv = view == "Inventory";

            _btnTabInventory.NormalColor = isInv ? AppColors.Primary : Color.White;
            _btnTabInventory.HoverColor = isInv ? AppColors.PrimaryLight : AppColors.SurfaceAlt;
            _btnTabInventory.ForeColor = isInv ? Color.White : AppColors.TextMedium;
            _btnTabInventory.CustomBorderWidth = isInv ? 0 : 1;
            _btnTabInventory.CustomBorderColor = AppColors.Border;

            _btnTabTransactions.NormalColor = !isInv ? AppColors.Primary : Color.White;
            _btnTabTransactions.HoverColor = !isInv ? AppColors.PrimaryLight : AppColors.SurfaceAlt;
            _btnTabTransactions.ForeColor = !isInv ? Color.White : AppColors.TextMedium;
            _btnTabTransactions.CustomBorderWidth = !isInv ? 0 : 1;
            _btnTabTransactions.CustomBorderColor = AppColors.Border;

            pnlCatFilter.Visible = isInv;
            _chkShowArchived.Visible = isInv;
            _btnAddIngredient.Visible = isInv;
            _btnRestock.Visible = isInv;
            _btnEdit.Visible = isInv;
            _btnArchiveToggle.Visible = isInv;
            _btnDelete.Visible = isInv;

            _btnVoidTransaction.Visible = !isInv;
            _btnRefreshTx.Visible = !isInv;

            _dgvStock.Visible = isInv;
            _dgvTransactions.Visible = !isInv;

            if (isInv)
                RefreshGrid();
            else
                LoadStockTransactions();
        }

        private void RefreshActiveView()
        {
            if (_activeSubView == "Inventory")
                RefreshGrid();
            else
                LoadStockTransactions();
        }

        public void RefreshStockData()
        {
            _stockService.RefreshIfClean();
            RefreshActiveView();
        }

        public void RefreshGrid()
        {
            _dgvStock.Rows.Clear();
            var stockList = _stockService.WorkingStock;
            if (stockList != null)
            {
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
            _lblPendingChangesBanner.Visible = dirty && _activeSubView == "Inventory";
            _bottomPanel.Visible = dirty && _activeSubView == "Inventory";
        }

        private void LoadStockTransactions()
        {
            try
            {
                string search = _txtSearch?.Text?.Trim();
                var list = _stockService.GetStockTransactions(null, null, "All", search);
                _dgvTransactions.Rows.Clear();
                foreach (var tx in list)
                {
                    string summary = tx.Items != null && tx.Items.Count > 0
                        ? string.Join(", ", tx.Items.Select(d => $"{(d.QuantityChange >= 0 ? "+" : "")}{d.QuantityChange:N2} {d.Unit} {d.IngredientName} ({d.StockBefore:N1} -> {d.StockAfter:N1})"))
                        : "-";

                    int rowIdx = _dgvTransactions.Rows.Add(
                        $"#{tx.StockTxId}",
                        tx.CreatedAt.ToString("yyyy-MM-dd hh:mm tt"),
                        tx.TransactionType,
                        tx.Username,
                        summary,
                        string.IsNullOrWhiteSpace(tx.Notes) ? "-" : tx.Notes,
                        tx.Status
                    );
                    _dgvTransactions.Rows[rowIdx].Tag = tx;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Unable to load stock transaction history.", ex, "Inventory Ledger Error", this);
            }
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
                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Ingredient != null)
                {
                    if (_stockService.AddNewIngredient(dlg.Ingredient, out string err))
                    {
                        MessageBox.Show(this, "Ingredient added to inventory successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshGrid();
                    }
                    else
                    {
                        MessageBox.Show(this, err, "Could Not Add Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void BtnRestock_Click(object sender, EventArgs e)
        {
            var ing = GetSelectedIngredient();
            if (ing == null)
            {
                MessageBox.Show(this, "Please select an ingredient from the list to restock.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new RestockDialogForm(ing))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
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
                MessageBox.Show(this, "Please select an ingredient to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new AddEditIngredientForm(ing))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Ingredient != null)
                {
                    if (_stockService.UpdateIngredient(dlg.Ingredient, out string err))
                    {
                        MessageBox.Show(this, "Ingredient updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshGrid();
                    }
                    else
                    {
                        MessageBox.Show(this, err, "Update Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void BtnArchiveToggle_Click(object sender, EventArgs e)
        {
            var ing = GetSelectedIngredient();
            if (ing == null)
            {
                MessageBox.Show(this, "Please select an ingredient first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool nextState = !ing.IsActive;
            string verb = nextState ? "restore" : "archive";
            if (MessageBox.Show(this, $"Are you sure you want to {verb} '{ing.Name}'?", "Confirm Status", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_stockService.SetIngredientActiveStatus(ing.IngredientId, nextState, out string err))
                {
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show(this, err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var ing = GetSelectedIngredient();
            if (ing == null)
            {
                MessageBox.Show(this, "Please select an ingredient to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, $"Are you sure you want to permanently delete ingredient '{ing.Name}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_stockService.DeleteIngredient(ing.IngredientId, out string err))
                {
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show(this, err, "Cannot Delete Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BtnVoidTransaction_Click(object sender, EventArgs e)
        {
            if (_dgvTransactions.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Select a stock transaction to void.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_dgvTransactions.SelectedRows[0].Tag is StockTransaction tx)
            {
                if (string.Equals(tx.Status, "Voided", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(this, $"Transaction #{tx.StockTxId} is already voided.", "Already Voided", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show(this, $"Void Stock Transaction #{tx.StockTxId} and reverse all quantity adjustments atomically?", "Confirm Void Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (_stockService.VoidStockTransaction(tx.StockTxId, tx.RowVersion, out string err))
                    {
                        MessageBox.Show(this, $"Transaction #{tx.StockTxId} voided and stock quantities reversed.", "Voided", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadStockTransactions();
                    }
                    else
                    {
                        MessageBox.Show(this, err, "Void Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnCancelChanges_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Discard all unsaved stock modifications?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _stockService.CancelChanges();
                RefreshGrid();
            }
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            if (_stockService.SaveChanges(out string err))
            {
                MessageBox.Show(this, "All stock changes saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshGrid();
            }
            else
            {
                MessageBox.Show(this, err, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
