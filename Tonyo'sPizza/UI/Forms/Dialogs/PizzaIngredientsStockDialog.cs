using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;
using MenuItem = Tonyo_sPizza.Models.MenuItem;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class PizzaIngredientsStockDialog : Form
    {
        private readonly MenuItem _pizzaItem;
        private readonly Action _onRestocked;
        private DataGridView _dgvIngredients;
        private Label _lblSummary;
        private Label _lblBottleneck;
        private RoundedButton _btnRestockSelected;
        private RoundedButton _btnClose;

        public class IngredientNeedRow
        {
            public int IngredientId { get; set; }
            public string Role { get; set; }
            public string IngredientName { get; set; }
            public decimal CurrentStock { get; set; }
            public string Unit { get; set; }
            public decimal MinThreshold { get; set; }
            public decimal PortionRequired { get; set; }
            public int PizzasCapacity { get; set; }
            public string HealthStatus { get; set; }
            public Ingredient IngredientObj { get; set; }
        }

        public PizzaIngredientsStockDialog(MenuItem pizzaItem, Action onRestocked = null)
        {
            _pizzaItem = pizzaItem ?? throw new ArgumentNullException(nameof(pizzaItem));
            _onRestocked = onRestocked;

            InitializeComponent();
            LoadIngredientBreakdown();
        }

        private void InitializeComponent()
        {
            Text = $"Ingredient Inventory & Stock Capacity - {_pizzaItem.Name}";
            Size = new Size(720, 520);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = AppColors.Background;

            // Header
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 84,
                BackColor = AppColors.Primary,
                Padding = new Padding(20, 12, 20, 10)
            };

            Label lblTitle = new Label
            {
                Text = $"{_pizzaItem.Name} - Ingredient Stock & Restock Advisor",
                Font = new Font("Segoe UI Bold", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(18, 12),
                AutoSize = true
            };

            _lblSummary = new Label
            {
                Text = "Analyzing recipe and dough ingredients...",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(254, 226, 226),
                Location = new Point(20, 36),
                AutoSize = true
            };

            _lblBottleneck = new Label
            {
                Text = "",
                Font = new Font("Segoe UI Bold", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(254, 240, 138),
                Location = new Point(20, 56),
                AutoSize = true
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(_lblSummary);
            pnlHeader.Controls.Add(_lblBottleneck);

            // Grid
            _dgvIngredients = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            UIHelper.StyleDataGridView(_dgvIngredients);

            _dgvIngredients.Columns.Add("Role", "Type");
            _dgvIngredients.Columns.Add("IngredientName", "Ingredient");
            _dgvIngredients.Columns.Add("CurrentStock", "Current Stock");
            _dgvIngredients.Columns.Add("PortionRequired", "Portion / Pizza");
            _dgvIngredients.Columns.Add("PizzasCapacity", "Pizzas Capacity");
            _dgvIngredients.Columns.Add("HealthStatus", "Stock Status");

            _dgvIngredients.Columns["Role"].Width = 85;
            _dgvIngredients.Columns["IngredientName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            _dgvIngredients.Columns["CurrentStock"].Width = 110;
            _dgvIngredients.Columns["PortionRequired"].Width = 100;
            _dgvIngredients.Columns["PizzasCapacity"].Width = 105;
            _dgvIngredients.Columns["HealthStatus"].Width = 122;

            UIHelper.AttachStatusPillPainter(_dgvIngredients, UIHelper.ResolveSemanticStatusColors, "HealthStatus");
            _dgvIngredients.CellFormatting += DgvIngredients_CellFormatting;

            Panel pnlGridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 12, 16, 12),
                BackColor = Color.White
            };
            pnlGridContainer.Controls.Add(_dgvIngredients);

            // Bottom
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = AppColors.Background,
                Padding = new Padding(18, 10, 18, 10)
            };

            _btnRestockSelected = new RoundedButton
            {
                Text = "+ Restock Selected Ingredient",
                Width = 220,
                Height = 38,
                Dock = DockStyle.Left,
                BorderRadius = 8,
                NormalColor = AppColors.StatusCompleted,
                HoverColor = Color.FromArgb(16, 185, 129),
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold)
            };
            _btnRestockSelected.Click += BtnRestockSelected_Click;

            _btnClose = new RoundedButton
            {
                Text = "Close",
                Width = 100,
                Height = 38,
                Dock = DockStyle.Right,
                BorderRadius = 8,
                NormalColor = AppColors.SecondaryNormal,
                HoverColor = AppColors.SecondaryHover,
                PressedColor = AppColors.SecondaryPressed,
                ForeColor = AppColors.TextDark,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold)
            };
            _btnClose.Click += (s, e) => Close();

            pnlBottom.Controls.Add(_btnRestockSelected);
            pnlBottom.Controls.Add(_btnClose);

            Controls.Add(pnlGridContainer);
            Controls.Add(pnlBottom);
            Controls.Add(pnlHeader);
            CancelButton = _btnClose;
        }

        private void DgvIngredients_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string col = _dgvIngredients.Columns[e.ColumnIndex].Name;

            if (col == "PizzasCapacity")
            {
                e.CellStyle.Font = new Font("Segoe UI Bold", 9f, FontStyle.Bold);
            }
        }

        public void LoadIngredientBreakdown()
        {
            _dgvIngredients.Rows.Clear();
            var stockList = StockService.Instance.WorkingStock;
            if (stockList == null || stockList.Count == 0)
            {
                StockService.Instance.ReloadFromDatabase();
                stockList = StockService.Instance.WorkingStock;
            }

            var rows = new List<IngredientNeedRow>();

            // 1. Dough Ingredients from DoughSizes
            if (_pizzaItem.DoughSizes != null && _pizzaItem.DoughSizes.Count > 0)
            {
                foreach (var ds in _pizzaItem.DoughSizes)
                {
                    if (ds.IngredientId.HasValue)
                    {
                        var ing = stockList.FirstOrDefault(i => i.IngredientId == ds.IngredientId.Value);
                        if (ing != null && !rows.Any(r => r.IngredientId == ing.IngredientId && r.Role.StartsWith("Dough")))
                        {
                            decimal portion = ds.DoughPortionQty > 0 ? ds.DoughPortionQty : 1.0m;
                            int cap = (int)Math.Floor(ing.CurrentStock / portion);
                            string status = ing.CurrentStock <= 0 ? "Out of Stock" : (cap <= 15 || ing.CurrentStock <= ing.MinThreshold ? "Stock More" : "Adequate");

                            rows.Add(new IngredientNeedRow
                            {
                                IngredientId = ing.IngredientId,
                                Role = $"Dough ({ds.SizeName})",
                                IngredientName = ing.Name,
                                CurrentStock = ing.CurrentStock,
                                Unit = ing.Unit,
                                MinThreshold = ing.MinThreshold,
                                PortionRequired = portion,
                                PizzasCapacity = Math.Max(0, cap),
                                HealthStatus = status,
                                IngredientObj = ing
                            });
                        }
                    }
                }
            }

            // 2. Recipe Ingredients
            if (_pizzaItem.RecipeIngredients != null && _pizzaItem.RecipeIngredients.Count > 0)
            {
                var grouped = _pizzaItem.RecipeIngredients
                    .GroupBy(r => r.IngredientId)
                    .ToList();

                foreach (var g in grouped)
                {
                    int ingId = g.Key;
                    var ing = stockList.FirstOrDefault(i => i.IngredientId == ingId);
                    if (ing != null)
                    {
                        // Use max portion requirement across sizes as conservative capacity calculation
                        decimal portion = g.Max(x => x.QuantityRequired);
                        if (portion <= 0) portion = 1.0m;
                        int cap = (int)Math.Floor(ing.CurrentStock / portion);
                        string status = ing.CurrentStock <= 0 ? "Out of Stock" : (cap <= 15 || ing.CurrentStock <= ing.MinThreshold ? "Stock More" : "Adequate");

                        rows.Add(new IngredientNeedRow
                        {
                            IngredientId = ing.IngredientId,
                            Role = "Recipe / Topping",
                            IngredientName = ing.Name,
                            CurrentStock = ing.CurrentStock,
                            Unit = ing.Unit,
                            MinThreshold = ing.MinThreshold,
                            PortionRequired = portion,
                            PizzasCapacity = Math.Max(0, cap),
                            HealthStatus = status,
                            IngredientObj = ing
                        });
                    }
                }
            }

            // Sort by PizzasCapacity ASCENDING (lowest first = highest restock urgency)
            var sorted = rows.OrderBy(r => r.PizzasCapacity).ThenBy(r => r.CurrentStock).ToList();

            foreach (var r in sorted)
            {
                int rowIndex = _dgvIngredients.Rows.Add(
                    r.Role,
                    r.IngredientName,
                    $"{r.CurrentStock:N1} {r.Unit}",
                    $"{r.PortionRequired:N2} {r.Unit}",
                    $"{r.PizzasCapacity} pizzas",
                    r.HealthStatus
                );
                _dgvIngredients.Rows[rowIndex].Tag = r;
            }

            if (sorted.Count > 0)
            {
                var bottleneck = sorted.First();
                int totalCap = bottleneck.PizzasCapacity;

                _lblSummary.Text = $"Total Pizza Capacity: Can make {totalCap} more {_pizzaItem.Name} pizzas with current inventory.";
                if (bottleneck.PizzasCapacity <= 0)
                {
                    _lblBottleneck.Text = $"CRITICAL: '{bottleneck.IngredientName}' is OUT OF STOCK. Immediate restock required.";
                }
                else if (bottleneck.PizzasCapacity <= 15 || bottleneck.CurrentStock <= bottleneck.MinThreshold)
                {
                    _lblBottleneck.Text = $"STOCK MORE: '{bottleneck.IngredientName}' is the bottleneck (Only {bottleneck.PizzasCapacity} pizzas remaining).";
                }
                else
                {
                    _lblBottleneck.Text = $"Health: Next ingredient to deplete is '{bottleneck.IngredientName}' (Supports {bottleneck.PizzasCapacity} pizzas).";
                }
            }
            else
            {
                _lblSummary.Text = "No ingredients linked to this food item yet.";
                _lblBottleneck.Text = "";
            }
        }

        private void BtnRestockSelected_Click(object sender, EventArgs e)
        {
            if (_dgvIngredients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an ingredient from the table to restock.", "Select Ingredient", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = _dgvIngredients.SelectedRows[0].Tag as IngredientNeedRow;
            if (row?.IngredientObj != null)
            {
                using (var dlg = new RestockDialogForm(row.IngredientObj))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        LoadIngredientBreakdown();
                        _onRestocked?.Invoke();
                    }
                }
            }
        }
    }
}
