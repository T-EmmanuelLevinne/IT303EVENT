using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;
using MenuItem = Tonyo_sPizza.Models.MenuItem;

namespace Tonyo_sPizza.UI.Forms.Dialogs
{
    public class AddEditMenuItemForm : Form
    {
        private readonly MenuItemRepository _menuRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly IngredientRepository _ingredientRepo;
        private readonly MenuItem _item;
        private readonly bool _isEdit;

        // Left Panel (Item Details & Image)
        private TextBox _txtName;
        private ComboBox _cboCategory;
        private NumericUpDown _numPrice;
        private TextBox _txtDescription;
        private CheckBox _chkAvailable;
        private PictureBox _picPreview;
        private RoundedButton _btnBrowseImage;
        private RoundedButton _btnClearImage;

        // Dough Sizes (4 Slots)
        private FlowLayoutPanel _flpDoughSizes;
        private List<MenuItemDoughSize> _doughSizes;

        // Recipe & Add-on Tabs
        private TabControl _tabDetails;
        private TabPage _tabRecipe;
        private TabPage _tabAddOns;
        private Panel _pnlActiveSizeBanner;
        private Label _lblRecipeBannerTitle;
        private Label _lblRecipeBannerSub;

        // Recipe Checkbox Controls (Categorized)
        private FlowLayoutPanel _flpRecipeCategories;
        private List<Ingredient> _allIngredients;
        private Dictionary<int, (CheckBox Chk, NumericUpDown NumQty, Label LblStock, Ingredient Ing)> _recipeIngredientControls;
        private Dictionary<string, Dictionary<int, decimal>> _recipesBySize;
        private string _currentActiveDoughSize = "";

        // Add-ons Controls (Categorized)
        private FlowLayoutPanel _flpAddOns;
        private Dictionary<int, (CheckBox Chk, NumericUpDown NumPrice, NumericUpDown NumPortion, ComboBox CboCategory, Ingredient Ing)> _addOnControls;

        private RoundedButton _btnSave;
        private RoundedButton _btnCancel;

        public MenuItem ResultItem => _item;

        public AddEditMenuItemForm(MenuItem item = null)
        {
            _menuRepo = new MenuItemRepository();
            _categoryRepo = new CategoryRepository();
            _ingredientRepo = new IngredientRepository();
            _item = item ?? new MenuItem();
            _isEdit = item != null;

            if (_isEdit && _item.ItemId > 0)
            {
                if (_item.RecipeIngredients == null || _item.RecipeIngredients.Count == 0)
                {
                    _item.RecipeIngredients = _menuRepo.GetRecipeIngredients(_item.ItemId);
                }
                if (_item.DoughSizes == null || _item.DoughSizes.Count == 0)
                {
                    _item.DoughSizes = _menuRepo.GetMenuItemDoughSizes(_item.ItemId);
                }
                if (_item.AvailableAddOns == null || _item.AvailableAddOns.Count == 0)
                {
                    _item.AvailableAddOns = _menuRepo.GetMenuItemAddOns(_item.ItemId);
                }
            }

            _doughSizes = _item.DoughSizes != null ? new List<MenuItemDoughSize>(_item.DoughSizes) : new List<MenuItemDoughSize>();
            _allIngredients = _ingredientRepo.GetAllIngredients();
            _recipeIngredientControls = new Dictionary<int, (CheckBox, NumericUpDown, Label, Ingredient)>();
            _addOnControls = new Dictionary<int, (CheckBox, NumericUpDown, NumericUpDown, ComboBox, Ingredient)>();

            _recipesBySize = new Dictionary<string, Dictionary<int, decimal>>(StringComparer.OrdinalIgnoreCase);
            if (_item.RecipeIngredients != null && _item.RecipeIngredients.Count > 0)
            {
                foreach (var r in _item.RecipeIngredients)
                {
                    string sKey = r.SizeName ?? "";
                    if (!_recipesBySize.ContainsKey(sKey))
                    {
                        _recipesBySize[sKey] = new Dictionary<int, decimal>();
                    }
                    _recipesBySize[sKey][r.IngredientId] = r.QuantityRequired;
                }
            }

            if (_doughSizes.Count > 0)
            {
                _currentActiveDoughSize = (_doughSizes.FirstOrDefault(d => d.IsDefault) ?? _doughSizes[0]).SizeName;
                if (_recipesBySize.ContainsKey("") && _recipesBySize[""].Count > 0)
                {
                    foreach (var ds in _doughSizes)
                    {
                        if (!_recipesBySize.ContainsKey(ds.SizeName))
                        {
                            _recipesBySize[ds.SizeName] = new Dictionary<int, decimal>(_recipesBySize[""]);
                        }
                    }
                }
            }
            else
            {
                _currentActiveDoughSize = "";
            }

            InitializeComponent();
            LoadCategories();
            PopulateData();
            BuildDoughSizeCards();
            BuildCategorizedRecipeCheckboxes();
            BuildCategorizedAddOns();
            RecalculateRecipeCost();
        }

        private void InitializeComponent()
        {
            Text = _isEdit ? "Edit Menu Item" : "Add New Menu Item";
            Size = new Size(1050, 780);
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
                Text = _isEdit ? "Edit Menu Item & Recipe" : "Create New Menu Item & Recipe",
                Font = new Font("Segoe UI Bold", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 16),
                AutoSize = true
            };
            header.Controls.Add(title);

            // Main 2-column container
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.White,
                Padding = new Padding(12)
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 430)); // Left col
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));  // Right col

            // ==================== LEFT COLUMN ====================
            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            int y = 10;

            Label lblName = new Label { Text = "Item Name *", Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 22;
            _txtName = new TextBox { Location = new Point(10, y), Width = 390, Font = new Font("Segoe UI", 10f) };
            _txtName.TextChanged += (s, e) => RecalculateRecipeCost();
            y += 34;

            Label lblCat = new Label { Text = "Category *", Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 22;
            _cboCategory = new ComboBox { Location = new Point(10, y), Width = 390, Font = new Font("Segoe UI", 10f), DropDownStyle = ComboBoxStyle.DropDownList };
            y += 34;

            Label lblPrice = new Label { Text = "Price (₱) *", Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 22;
            _numPrice = new NumericUpDown { Location = new Point(10, y), Width = 390, Font = new Font("Segoe UI", 10f), DecimalPlaces = 2, Maximum = 9999, Minimum = 0, Value = 14.99m };
            y += 34;

            Label lblDesc = new Label { Text = "Description", Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 22;
            _txtDescription = new TextBox { Location = new Point(10, y), Width = 390, Height = 48, Multiline = true, Font = new Font("Segoe UI", 9f) };
            y += 56;

            _chkAvailable = new CheckBox { Text = "Is Available for Ordering", Location = new Point(10, y), Checked = true, AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 30;

            // Image Preview & Upload
            Label lblImage = new Label { Text = "Pizza / Food Image Preview", Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold), ForeColor = AppColors.TextDark };
            y += 22;

            _picPreview = new PictureBox
            {
                Location = new Point(10, y),
                Size = new Size(185, 120),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(248, 250, 252),
                BorderStyle = BorderStyle.FixedSingle,
                AllowDrop = true
            };
            _picPreview.DragEnter += (s, e) => { if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy; };
            _picPreview.DragDrop += PicPreview_DragDrop;

            _btnBrowseImage = new RoundedButton
            {
                Text = "Browse Image...",
                Location = new Point(205, y + 15),
                Size = new Size(195, 38),
                BorderRadius = 8,
                NormalColor = Color.FromArgb(71, 85, 105),
                HoverColor = Color.FromArgb(100, 116, 139)
            };
            _btnBrowseImage.Click += BtnBrowseImage_Click;

            _btnClearImage = new RoundedButton
            {
                Text = "Clear Image",
                Location = new Point(205, y + 62),
                Size = new Size(195, 38),
                BorderRadius = 8,
                NormalColor = Color.FromArgb(226, 232, 240),
                HoverColor = Color.FromArgb(203, 213, 225),
                ForeColor = AppColors.TextDark
            };
            _btnClearImage.Click += (s, e) => { _picPreview.Image = null; _item.ImageBase64 = null; };
            y += 130;

            // 4 Dough Sizes Header & Flow
            Label lblDoughSection = new Label
            {
                Text = "Dough Sizes & Crust Types (Up to 4)",
                Location = new Point(10, y),
                AutoSize = true,
                Font = new Font("Segoe UI Bold", 10f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryDark
            };
            y += 24;

            _flpDoughSizes = new FlowLayoutPanel
            {
                Location = new Point(10, y),
                Size = new Size(395, 130),
                AutoScroll = true,
                WrapContents = true,
                BackColor = Color.FromArgb(248, 250, 252),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(6)
            };

            leftPanel.Controls.Add(lblName);
            leftPanel.Controls.Add(_txtName);
            leftPanel.Controls.Add(lblCat);
            leftPanel.Controls.Add(_cboCategory);
            leftPanel.Controls.Add(lblPrice);
            leftPanel.Controls.Add(_numPrice);
            leftPanel.Controls.Add(lblDesc);
            leftPanel.Controls.Add(_txtDescription);
            leftPanel.Controls.Add(_chkAvailable);
            leftPanel.Controls.Add(lblImage);
            leftPanel.Controls.Add(_picPreview);
            leftPanel.Controls.Add(_btnBrowseImage);
            leftPanel.Controls.Add(_btnClearImage);
            leftPanel.Controls.Add(lblDoughSection);
            leftPanel.Controls.Add(_flpDoughSizes);

            // ==================== RIGHT COLUMN (TABS) ====================
            _tabDetails = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold)
            };

            // TAB 1: Recipe Ingredients (Categorized Checkboxes)
            _tabRecipe = new TabPage("Recipe Ingredients (Stock-Aware)");
            _tabRecipe.BackColor = Color.White;
            _tabRecipe.Padding = new Padding(12);

            _pnlActiveSizeBanner = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.FromArgb(254, 242, 242),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10, 6, 10, 6),
                Margin = new Padding(0, 0, 0, 8)
            };

            _lblRecipeBannerTitle = new Label
            {
                Text = "Deduction Ingredients for: ...",
                Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryDark,
                Location = new Point(10, 7),
                AutoSize = true
            };

            _lblRecipeBannerSub = new Label
            {
                Text = "Configure deduction quantities for this dough size. Click other sizes on the left to customize.",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(10, 29),
                AutoSize = true
            };

            _pnlActiveSizeBanner.Controls.Add(_lblRecipeBannerTitle);
            _pnlActiveSizeBanner.Controls.Add(_lblRecipeBannerSub);

            _flpRecipeCategories = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(4)
            };

            _tabRecipe.Controls.Add(_flpRecipeCategories);
            _tabRecipe.Controls.Add(_pnlActiveSizeBanner);
            _pnlActiveSizeBanner.BringToFront();

            // TAB 2: Available Add-ons by Category
            _tabAddOns = new TabPage("Available Add-ons (Customer Options)");
            _tabAddOns.BackColor = Color.White;
            _tabAddOns.Padding = new Padding(12);

            Label lblAddOnNote = new Label
            {
                Text = "Check in-stock ingredients to offer as optional customer Add-ons with custom prices & portion sizes:",
                Dock = DockStyle.Top,
                Height = 32,
                Font = new Font("Segoe UI", 9f),
                ForeColor = AppColors.TextMuted
            };

            _flpAddOns = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(4)
            };

            _tabAddOns.Controls.Add(_flpAddOns);
            _tabAddOns.Controls.Add(lblAddOnNote);

            _tabDetails.TabPages.Add(_tabRecipe);
            _tabDetails.TabPages.Add(_tabAddOns);

            mainLayout.Controls.Add(leftPanel, 0, 0);
            mainLayout.Controls.Add(_tabDetails, 1, 0);

            // ==================== BOTTOM BAR ====================
            Panel bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 64,
                BackColor = AppColors.Background,
                Padding = new Padding(20, 12, 20, 12)
            };

            _btnSave = new RoundedButton
            {
                Text = _isEdit ? "Save Changes" : "Create Menu Item",
                Dock = DockStyle.Right,
                Width = 170,
                BorderRadius = 8,
                NormalColor = AppColors.Primary,
                HoverColor = AppColors.PrimaryLight
            };
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new RoundedButton
            {
                Text = "Cancel",
                Dock = DockStyle.Right,
                Width = 120,
                BorderRadius = 8,
                NormalColor = Color.FromArgb(226, 232, 240),
                HoverColor = Color.FromArgb(203, 213, 225),
                ForeColor = AppColors.TextDark
            };
            _btnCancel.Click += (s, e) => Close();

            bottom.Controls.Add(_btnSave);
            bottom.Controls.Add(new Panel { Dock = DockStyle.Right, Width = 12 });
            bottom.Controls.Add(_btnCancel);

            Controls.Add(mainLayout);
            Controls.Add(bottom);
            Controls.Add(header);
        }

        private void LoadCategories()
        {
            _cboCategory.Items.Clear();
            var categories = _categoryRepo.GetAllCategories();
            foreach (var cat in categories)
            {
                _cboCategory.Items.Add(cat);
            }
            if (_cboCategory.Items.Count > 0)
            {
                _cboCategory.SelectedIndex = 0;
            }
        }

        private void PopulateData()
        {
            if (_isEdit)
            {
                _txtName.Text = _item.Name;
                _numPrice.Value = _item.Price;
                _txtDescription.Text = _item.Description;
                _chkAvailable.Checked = _item.IsAvailable;

                if (_item.CategoryId.HasValue)
                {
                    for (int i = 0; i < _cboCategory.Items.Count; i++)
                    {
                        if (_cboCategory.Items[i] is Category cat && cat.CategoryId == _item.CategoryId.Value)
                        {
                            _cboCategory.SelectedIndex = i;
                            break;
                        }
                    }
                }

                _picPreview.Image = _item.GetImage();
            }
        }

        // ==================== DOUGH SIZES SLOTS ====================
        private void BuildDoughSizeCards()
        {
            _flpDoughSizes.SuspendLayout();
            _flpDoughSizes.Controls.Clear();

            // Render existing dough sizes
            for (int i = 0; i < _doughSizes.Count; i++)
            {
                var ds = _doughSizes[i];
                int index = i;
                bool isSelected = string.Equals(_currentActiveDoughSize, ds.SizeName, StringComparison.OrdinalIgnoreCase);

                Panel card = new Panel
                {
                    Width = 180,
                    Height = 52,
                    BackColor = isSelected ? Color.FromArgb(254, 242, 242) : Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(3),
                    Cursor = Cursors.Hand
                };

                if (isSelected)
                {
                    card.Paint += (s, e) =>
                    {
                        using (var pen = new Pen(AppColors.Primary, 2f))
                        {
                            e.Graphics.DrawRectangle(pen, 1, 1, card.Width - 3, card.Height - 3);
                        }
                    };
                }

                Label lblName = new Label
                {
                    Text = (isSelected ? "▶ " : "") + $"{ds.SizeName} - ₱{ds.Price:N2}",
                    Font = new Font("Segoe UI Bold", 8.5f, FontStyle.Bold),
                    ForeColor = isSelected ? AppColors.PrimaryDark : AppColors.TextDark,
                    Location = new Point(6, 6),
                    AutoSize = true,
                    Cursor = Cursors.Hand
                };

                Label lblSub = new Label
                {
                    Text = $"Portion: {ds.DoughPortionQty:N1} {(string.IsNullOrEmpty(ds.DoughIngredientName) ? "portion" : ds.DoughIngredientName)}",
                    Font = new Font("Segoe UI", 7.5f),
                    ForeColor = isSelected ? AppColors.Primary : AppColors.TextMuted,
                    Location = new Point(6, 26),
                    AutoSize = true,
                    Cursor = Cursors.Hand
                };

                Button btnDel = new Button
                {
                    Text = "✕",
                    Width = 24,
                    Height = 24,
                    Location = new Point(150, 4),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.Red,
                    Cursor = Cursors.Hand,
                    Font = new Font("Segoe UI Bold", 8f, FontStyle.Bold),
                    BackColor = Color.Transparent
                };
                btnDel.FlatAppearance.BorderSize = 0;
                btnDel.Click += (s, e) =>
                {
                    RemoveDoughSizeAt(index);
                };

                Action editAction = () =>
                {
                    SaveCurrentRecipeControlsToMemory();
                    string oldName = ds.SizeName;
                    using (var dlg = new AddEditDoughSizeDialog(ds, _allIngredients))
                    {
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            if (!string.Equals(oldName, ds.SizeName, StringComparison.OrdinalIgnoreCase))
                            {
                                if (_recipesBySize.ContainsKey(oldName))
                                {
                                    _recipesBySize[ds.SizeName] = _recipesBySize[oldName];
                                    _recipesBySize.Remove(oldName);
                                }
                                if (string.Equals(_currentActiveDoughSize, oldName, StringComparison.OrdinalIgnoreCase))
                                {
                                    _currentActiveDoughSize = ds.SizeName;
                                }
                            }
                            BuildDoughSizeCards();
                            LoadRecipeControlsForActiveSize();
                        }
                    }
                };

                MouseEventHandler clickHandler = (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        SelectActiveDoughSize(ds.SizeName);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        var menu = UIHelper.CreateModernContextMenu();
                        var itemEdit = UIHelper.CreateModernMenuItem("Edit Dough Size", "edit", (sender, args) => editAction());
                        var itemRemove = UIHelper.CreateModernMenuItem("Remove Dough Size", "delete", (sender, args) =>
                        {
                            RemoveDoughSizeAt(index);
                        }, Color.FromArgb(220, 38, 38));

                        menu.Items.Add(itemEdit);
                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(itemRemove);

                        menu.Show(Cursor.Position);
                    }
                };

                card.MouseClick += clickHandler;
                lblName.MouseClick += clickHandler;
                lblSub.MouseClick += clickHandler;

                card.DoubleClick += (s, e) => editAction();
                lblName.DoubleClick += (s, e) => editAction();
                lblSub.DoubleClick += (s, e) => editAction();

                card.Controls.Add(lblName);
                card.Controls.Add(lblSub);
                card.Controls.Add(btnDel);
                _flpDoughSizes.Controls.Add(card);
            }

            // If less than 4, show [+] Add Dough Size card
            if (_doughSizes.Count < 4)
            {
                Button btnAdd = new Button
                {
                    Text = "+ Add Dough Size",
                    Width = 180,
                    Height = 52,
                    Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                    BackColor = Color.FromArgb(241, 245, 249),
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(3),
                    Cursor = Cursors.Hand
                };
                btnAdd.Click += (s, e) =>
                {
                    SaveCurrentRecipeControlsToMemory();
                    using (var dlg = new AddEditDoughSizeDialog(null, _allIngredients))
                    {
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            _doughSizes.Add(dlg.ResultSize);
                            _currentActiveDoughSize = dlg.ResultSize.SizeName;

                            if (!_recipesBySize.ContainsKey(_currentActiveDoughSize))
                            {
                                var seed = _recipesBySize.Values.FirstOrDefault(v => v.Count > 0);
                                _recipesBySize[_currentActiveDoughSize] = seed != null 
                                    ? new Dictionary<int, decimal>(seed) 
                                    : new Dictionary<int, decimal>();
                            }

                            BuildDoughSizeCards();
                            LoadRecipeControlsForActiveSize();
                            _tabDetails.SelectedTab = _tabRecipe;
                        }
                    }
                };
                _flpDoughSizes.Controls.Add(btnAdd);
            }

            _flpDoughSizes.ResumeLayout();
        }

        private void RemoveDoughSizeAt(int index)
        {
            if (index < 0 || index >= _doughSizes.Count) return;
            SaveCurrentRecipeControlsToMemory();
            string removedSize = _doughSizes[index].SizeName;
            _doughSizes.RemoveAt(index);
            if (_recipesBySize.ContainsKey(removedSize))
            {
                _recipesBySize.Remove(removedSize);
            }

            if (string.Equals(_currentActiveDoughSize, removedSize, StringComparison.OrdinalIgnoreCase))
            {
                _currentActiveDoughSize = _doughSizes.Count > 0 ? _doughSizes[0].SizeName : "";
            }

            BuildDoughSizeCards();
            LoadRecipeControlsForActiveSize();
        }

        private void SelectActiveDoughSize(string sizeName)
        {
            if (string.Equals(_currentActiveDoughSize, sizeName, StringComparison.OrdinalIgnoreCase))
            {
                _tabDetails.SelectedTab = _tabRecipe;
                return;
            }

            SaveCurrentRecipeControlsToMemory();
            _currentActiveDoughSize = sizeName;
            BuildDoughSizeCards();
            LoadRecipeControlsForActiveSize();
            _tabDetails.SelectedTab = _tabRecipe;
        }

        private void SaveCurrentRecipeControlsToMemory()
        {
            if (_recipeIngredientControls == null || _recipeIngredientControls.Count == 0) return;

            string key = _currentActiveDoughSize ?? "";
            var dict = new Dictionary<int, decimal>();
            foreach (var kvp in _recipeIngredientControls)
            {
                if (kvp.Value.Chk.Checked)
                {
                    dict[kvp.Key] = kvp.Value.NumQty.Value;
                }
            }
            _recipesBySize[key] = dict;
        }

        private void LoadRecipeControlsForActiveSize()
        {
            if (_recipeIngredientControls == null || _recipeIngredientControls.Count == 0) return;

            string key = _currentActiveDoughSize ?? "";
            Dictionary<int, decimal> dict = null;
            if (!_recipesBySize.TryGetValue(key, out dict))
            {
                var seed = _recipesBySize.Values.FirstOrDefault(v => v.Count > 0);
                if (seed != null)
                {
                    dict = new Dictionary<int, decimal>(seed);
                    _recipesBySize[key] = dict;
                }
                else
                {
                    dict = new Dictionary<int, decimal>();
                    _recipesBySize[key] = dict;
                }
            }

            foreach (var kvp in _recipeIngredientControls)
            {
                int ingId = kvp.Key;
                var (chk, numQty, lblStock, ing) = kvp.Value;
                if (dict != null && dict.TryGetValue(ingId, out decimal qty))
                {
                    chk.Checked = true;
                    numQty.Value = Math.Max(numQty.Minimum, Math.Min(numQty.Maximum, qty));
                    numQty.Enabled = true;
                }
                else
                {
                    chk.Checked = false;
                    numQty.Value = 1.00m;
                    numQty.Enabled = false;
                }
            }

            UpdateRecipeBanner();
        }

        private void UpdateRecipeBanner()
        {
            if (_lblRecipeBannerTitle == null) return;

            if (!string.IsNullOrEmpty(_currentActiveDoughSize))
            {
                _lblRecipeBannerTitle.Text = $"Deduction Ingredients for: {_currentActiveDoughSize}";
                _lblRecipeBannerSub.Text = "Configure deduction quantities for this dough size. Click other sizes on the left to customize.";
                _pnlActiveSizeBanner.BackColor = Color.FromArgb(254, 242, 242);
                _lblRecipeBannerTitle.ForeColor = AppColors.PrimaryDark;
            }
            else
            {
                _lblRecipeBannerTitle.Text = "Standard Recipe Ingredients";
                _lblRecipeBannerSub.Text = "Configure standard deduction quantities for this item.";
                _pnlActiveSizeBanner.BackColor = Color.FromArgb(241, 245, 249);
                _lblRecipeBannerTitle.ForeColor = AppColors.TextDark;
            }
        }

        // ==================== CATEGORIZED RECIPE CHECKBOXES ====================
        private void BuildCategorizedRecipeCheckboxes()
        {
            _flpRecipeCategories.SuspendLayout();
            _flpRecipeCategories.Controls.Clear();
            _recipeIngredientControls.Clear();

            var nonDoughIngredients = _allIngredients.Where(i => 
                !string.Equals(i.CategoryName, "Dough", StringComparison.OrdinalIgnoreCase) && 
                !i.Name.ToLower().Contains("dough") &&
                !i.Name.ToLower().Contains("crust")
            ).ToList();

            var groups = nonDoughIngredients.GroupBy(i => !string.IsNullOrEmpty(i.CategoryName) ? i.CategoryName : "General").OrderBy(g => g.Key);

            foreach (var group in groups)
            {
                Panel groupHeader = new Panel
                {
                    Width = 530,
                    Height = 28,
                    BackColor = Color.FromArgb(241, 245, 249),
                    Margin = new Padding(0, 8, 0, 4)
                };
                Label lblGroup = new Label
                {
                    Text = group.Key.ToUpper(),
                    Font = new Font("Segoe UI Bold", 9f, FontStyle.Bold),
                    ForeColor = AppColors.TextDark,
                    Location = new Point(6, 5),
                    AutoSize = true
                };
                groupHeader.Controls.Add(lblGroup);
                _flpRecipeCategories.Controls.Add(groupHeader);

                foreach (var ing in group)
                {
                    Panel row = new Panel
                    {
                        Width = 530,
                        Height = 34,
                        Margin = new Padding(0, 1, 0, 1),
                        BackColor = Color.White
                    };

                    CheckBox chk = new CheckBox
                    {
                        Text = ing.Name,
                        Checked = false,
                        Location = new Point(6, 6),
                        Width = 200,
                        Font = new Font("Segoe UI", 9f)
                    };

                    Label lblStock = new Label
                    {
                        Text = $"Stock: {ing.CurrentStock:N0} {ing.Unit}",
                        Location = new Point(210, 8),
                        Width = 110,
                        Font = new Font("Segoe UI", 8.5f),
                        ForeColor = ing.CurrentStock > ing.MinThreshold ? AppColors.StatusCompleted : (ing.CurrentStock > 0 ? AppColors.StatusPending : AppColors.StatusCancelled)
                    };

                    Label lblQtyTitle = new Label
                    {
                        Text = "Qty:",
                        Location = new Point(325, 8),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 8.5f),
                        ForeColor = AppColors.TextMuted
                    };

                    NumericUpDown numQty = new NumericUpDown
                    {
                        Location = new Point(355, 5),
                        Width = 90,
                        DecimalPlaces = 2,
                        Maximum = 9999,
                        Minimum = 0.01m,
                        Value = 1.00m,
                        Enabled = false,
                        Font = new Font("Segoe UI", 9f)
                    };

                    Label lblUnit = new Label
                    {
                        Text = ing.Unit,
                        Location = new Point(450, 8),
                        AutoSize = true,
                        Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                        ForeColor = AppColors.TextDark
                    };

                    chk.CheckedChanged += (s, e) =>
                    {
                        numQty.Enabled = chk.Checked;
                        RecalculateRecipeCost();
                    };
                    numQty.ValueChanged += (s, e) => RecalculateRecipeCost();

                    row.Controls.Add(chk);
                    row.Controls.Add(lblStock);
                    row.Controls.Add(lblQtyTitle);
                    row.Controls.Add(numQty);
                    row.Controls.Add(lblUnit);

                    _flpRecipeCategories.Controls.Add(row);
                    _recipeIngredientControls[ing.IngredientId] = (chk, numQty, lblStock, ing);
                }
            }

            _flpRecipeCategories.ResumeLayout();
            LoadRecipeControlsForActiveSize();
        }

        private void RecalculateRecipeCost()
        {
        }

        // ==================== CATEGORIZED AVAILABLE ADD-ONS ====================
        private void BuildCategorizedAddOns()
        {
            _flpAddOns.SuspendLayout();
            _flpAddOns.Controls.Clear();
            _addOnControls.Clear();

            var nonDoughIngredients = _allIngredients.Where(i => 
                !string.Equals(i.CategoryName, "Dough", StringComparison.OrdinalIgnoreCase) && 
                !i.Name.ToLower().Contains("dough") &&
                !i.Name.ToLower().Contains("crust")
            ).ToList();

            var groups = nonDoughIngredients.GroupBy(i => !string.IsNullOrEmpty(i.CategoryName) ? i.CategoryName : "General").OrderBy(g => g.Key);

            foreach (var group in groups)
            {
                Panel groupHeader = new Panel
                {
                    Width = 530,
                    Height = 28,
                    BackColor = Color.FromArgb(241, 245, 249),
                    Margin = new Padding(0, 8, 0, 4)
                };
                Label lblGroup = new Label
                {
                    Text = $"{group.Key.ToUpper()} ADD-ONS",
                    Font = new Font("Segoe UI Bold", 9f, FontStyle.Bold),
                    ForeColor = AppColors.TextDark,
                    Location = new Point(6, 5),
                    AutoSize = true
                };
                groupHeader.Controls.Add(lblGroup);
                _flpAddOns.Controls.Add(groupHeader);

                foreach (var ing in group)
                {
                    var existingAddOn = _item.AvailableAddOns?.FirstOrDefault(a => a.IngredientId == ing.IngredientId);
                    bool isChecked = existingAddOn != null;
                    decimal price = existingAddOn?.Price ?? 1.50m;
                    decimal portion = existingAddOn?.PortionQty ?? (ing.Unit == "g" ? 40m : 1m);

                    Panel row = new Panel
                    {
                        Width = 530,
                        Height = 34,
                        Margin = new Padding(0, 1, 0, 1),
                        BackColor = Color.White
                    };

                    CheckBox chk = new CheckBox
                    {
                        Text = ing.Name,
                        Checked = isChecked,
                        Location = new Point(6, 6),
                        Width = 175,
                        Font = new Font("Segoe UI", 9f)
                    };

                    Label lblPriceTitle = new Label { Text = "+₱:", Location = new Point(185, 8), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = AppColors.TextMuted };
                    NumericUpDown numPrice = new NumericUpDown
                    {
                        Location = new Point(208, 5),
                        Width = 70,
                        DecimalPlaces = 2,
                        Maximum = 999,
                        Minimum = 0,
                        Value = price,
                        Enabled = isChecked,
                        Font = new Font("Segoe UI", 9f)
                    };

                    Label lblPortionTitle = new Label { Text = "Portion:", Location = new Point(285, 8), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = AppColors.TextMuted };
                    NumericUpDown numPortion = new NumericUpDown
                    {
                        Location = new Point(335, 5),
                        Width = 70,
                        DecimalPlaces = 2,
                        Maximum = 9999,
                        Minimum = 0.1m,
                        Value = portion,
                        Enabled = isChecked,
                        Font = new Font("Segoe UI", 9f)
                    };

                    Label lblUnit = new Label
                    {
                        Text = ing.Unit,
                        Location = new Point(410, 8),
                        Width = 40,
                        Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                        ForeColor = AppColors.TextDark
                    };

                    chk.CheckedChanged += (s, e) =>
                    {
                        numPrice.Enabled = chk.Checked;
                        numPortion.Enabled = chk.Checked;
                    };

                    row.Controls.Add(chk);
                    row.Controls.Add(lblPriceTitle);
                    row.Controls.Add(numPrice);
                    row.Controls.Add(lblPortionTitle);
                    row.Controls.Add(numPortion);
                    row.Controls.Add(lblUnit);

                    _flpAddOns.Controls.Add(row);
                    _addOnControls[ing.IngredientId] = (chk, numPrice, numPortion, null, ing);
                }
            }

            _flpAddOns.ResumeLayout();
        }

        // ==================== IMAGE HANDLING ====================
        private void BtnBrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadImageFromFile(ofd.FileName);
                }
            }
        }

        private void PicPreview_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                LoadImageFromFile(files[0]);
            }
        }

        private void LoadImageFromFile(string path)
        {
            try
            {
                using (var img = Image.FromFile(path))
                {
                    _picPreview.Image = new Bitmap(img);
                    _item.SetImage(_picPreview.Image);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load image: {ex.Message}", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== SAVE MENU ITEM ====================
        private void BtnSave_Click(object sender, EventArgs e)
        {
            string name = _txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a food item name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_numPrice.Value <= 0)
            {
                MessageBox.Show("Please enter a valid price greater than zero.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _item.Name = name;
                _item.Price = _numPrice.Value;
                _item.OldPrice = null;
                _item.Description = _txtDescription.Text.Trim();
                _item.IsAvailable = _chkAvailable.Checked;

                if (_cboCategory.SelectedItem is Category cat)
                {
                    _item.CategoryId = cat.CategoryId;
                    _item.CategoryName = cat.CategoryName;
                }

                if (_picPreview.Image != null)
                {
                    _item.SetImage(_picPreview.Image);
                }

                // 1. Gather Recipe Ingredients
                SaveCurrentRecipeControlsToMemory();

                List<MenuItemIngredient> recipes = new List<MenuItemIngredient>();
                if (_doughSizes.Count > 0)
                {
                    // Check if any dough size has no ingredients selected
                    var emptySizes = _doughSizes.Where(ds => !_recipesBySize.ContainsKey(ds.SizeName) || _recipesBySize[ds.SizeName].Count == 0).ToList();
                    if (emptySizes.Count > 0)
                    {
                        MessageBox.Show($"Please select recipe ingredients for dough size \"{emptySizes[0].SizeName}\". All dough sizes must have ingredient deductions configured.", "Recipe Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        SelectActiveDoughSize(emptySizes[0].SizeName);
                        return;
                    }

                    foreach (var ds in _doughSizes)
                    {
                        if (_recipesBySize.TryGetValue(ds.SizeName, out var dict))
                        {
                            foreach (var kvp in dict)
                            {
                                var ing = _allIngredients.FirstOrDefault(i => i.IngredientId == kvp.Key);
                                recipes.Add(new MenuItemIngredient(_item.ItemId, kvp.Key, kvp.Value, ing?.Name, ing?.Unit, ds.SizeName));
                            }
                        }
                    }
                }
                else
                {
                    if (_recipesBySize.TryGetValue("", out var dict))
                    {
                        foreach (var kvp in dict)
                        {
                            var ing = _allIngredients.FirstOrDefault(i => i.IngredientId == kvp.Key);
                            recipes.Add(new MenuItemIngredient(_item.ItemId, kvp.Key, kvp.Value, ing?.Name, ing?.Unit, ""));
                        }
                    }
                }

                if (recipes.Count == 0)
                {
                    MessageBox.Show("Please select at least one recipe ingredient for this menu item. A recipe is required for inventory stock tracking.", "Recipe Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _tabDetails.SelectedTab = _tabRecipe;
                    return;
                }

                _item.RecipeIngredients = recipes;

                // 2. Gather Dough Sizes
                _item.DoughSizes = _doughSizes;

                // 3. Gather Available Add-ons
                List<MenuItemAddOn> addOns = new List<MenuItemAddOn>();
                foreach (var kvp in _addOnControls)
                {
                    if (kvp.Value.Chk.Checked)
                    {
                        addOns.Add(new MenuItemAddOn(
                            0,
                            _item.ItemId,
                            kvp.Key,
                            $"Extra {kvp.Value.Ing.Name}",
                            kvp.Value.NumPrice.Value,
                            kvp.Value.NumPortion.Value,
                            kvp.Value.Ing.CategoryName,
                            kvp.Value.Ing.Unit,
                            kvp.Value.Ing.CurrentStock
                        ));
                    }
                }
                _item.AvailableAddOns = addOns;

                // 4. Duplicate Check: Same Name, Same Category, Same Recipe Ingredients, and Same Add-ons
                var recipeIds = recipes.Select(r => r.IngredientId).Distinct();
                var addOnIds = addOns.Select(a => a.IngredientId);
                var duplicate = _menuRepo.FindDuplicateMenuItem(_item.ItemId, name, _item.CategoryId, recipeIds, addOnIds);
                if (duplicate != null)
                {
                    string catDisplay = !string.IsNullOrEmpty(_item.CategoryName) ? _item.CategoryName : "Selected Category";
                    MessageBox.Show(
                        $"A product with the same name (\"{name}\"), same category (\"{catDisplay}\"), same recipe ingredients, and same add-ons already exists in the system (Item #{duplicate.ItemId}).\n\nDuplicate products are not allowed. Please modify the product name, category, recipe, or add-ons.",
                        "Duplicate Product Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // Persist
                if (_isEdit)
                {
                    _menuRepo.UpdateMenuItem(_item);
                }
                else
                {
                    _menuRepo.InsertMenuItem(_item);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving menu item: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
