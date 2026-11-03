using System;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Forms.Dialogs;
using Tonyo_sPizza.UI.Theme;
using Tonyo_sPizza.UI.Views.Admin;
using Tonyo_sPizza.UI.Views.Crew;
using Tonyo_sPizza.UI.Views.Customer;

namespace Tonyo_sPizza.UI.Forms
{
    public class MainForm : Form
    {
        private readonly AuthService _authService;

        // UI Layout Elements
        private Panel _pnlHeader;
        private Panel _pnlSidebar;
        private Panel _pnlContent;
        private TextBox _txtGlobalSearch;
        private Label _lblUserName;
        private Label _lblUserRole;
        private RoundedButton _btnDb;

        // Sidebar Navigation Buttons
        private FlowLayoutPanel _flpNavButtons;

        // View Instances
        private CustomerMenuControl _customerMenuControl;
        private CustomerOrdersControl _customerOrdersControl;
        private CrewQueueControl _crewQueueControl;
        private AdminStockControl _adminStockControl;
        private AdminMenuControl _adminMenuControl;
        private AdminCategoryControl _adminCategoryControl;
        private AdminUsersControl _adminUsersControl;
        private AdminAuditControl _adminAuditControl;
        private AdminPromoControl _adminPromoControl;
        private AdminSalesReportControl _adminSalesControl;

        private string _activeTab = "Menu";

        public MainForm()
        {
            _authService = AuthService.Instance;
            InitializeComponent();
            SetupRoleViews();
        }

        private void InitializeComponent()
        {
            Text = "Tonyo's Pizza - Restaurant Management System";
            Size = new Size(1300, 850);
            MinimumSize = new Size(1100, 720);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = AppColors.Background;

            // 1. Top Header
            BuildHeader();

            // 2. Left Sidebar
            BuildSidebar();

            // 3. Main Dynamic Content Container
            _pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = AppColors.Background
            };

            Controls.Add(_pnlContent);
            Controls.Add(_pnlSidebar);
            Controls.Add(_pnlHeader);
        }

        private void BuildHeader()
        {
            _pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 24, 12)
            };

            // Bottom border line for header
            _pnlHeader.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(AppColors.Border))
                {
                    e.Graphics.DrawLine(pen, 0, _pnlHeader.Height - 1, _pnlHeader.Width, _pnlHeader.Height - 1);
                }
            };

            // Brand Logo (GoFood Style)
            Label lblBrand = new Label
            {
                Text = "GoFood.",
                Font = new Font("Segoe UI Black", 18f, FontStyle.Bold),
                ForeColor = AppColors.Primary,
                Location = new Point(20, 16),
                AutoSize = true
            };

            Label lblSubBrand = new Label
            {
                Text = "by Tonyo's Pizza",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = AppColors.TextMuted,
                Location = new Point(140, 24),
                AutoSize = true
            };

            // Search Bar (Center/Left)
            RoundedPanel pnlSearch = new RoundedPanel
            {
                Location = new Point(280, 14),
                Size = new Size(360, 42),
                BorderRadius = 10,
                BackColor = AppColors.Background,
                BorderColor = AppColors.Border,
                DrawBorder = true,
                Padding = new Padding(12, 8, 12, 8)
            };

            Label lblSearchIcon = new Label
            {
                Text = "Search:",
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = AppColors.TextMuted,
                Location = new Point(8, 10),
                Size = new Size(52, 20),
                BackColor = Color.Transparent
            };

            _txtGlobalSearch = new TextBox
            {
                Location = new Point(62, 10),
                Width = 280,
                Font = new Font("Segoe UI", 10f),
                BorderStyle = BorderStyle.None,
                BackColor = AppColors.Background,
                ForeColor = AppColors.TextDark
            };
            _txtGlobalSearch.TextChanged += (s, e) =>
            {
                if (_customerMenuControl != null)
                {
                    _customerMenuControl.SetSearchKeyword(_txtGlobalSearch.Text);
                }
            };

            pnlSearch.Controls.Add(_txtGlobalSearch);
            pnlSearch.Controls.Add(lblSearchIcon);

            // Right User Profile & Action Chip
            Panel pnlRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 470,
                Height = 46,
                BackColor = Color.Transparent
            };

            _btnDb = new RoundedButton
            {
                Text = "DB Settings",
                Location = new Point(70, 4),
                Size = new Size(95, 38),
                BorderRadius = 8,
                NormalColor = Color.FromArgb(241, 245, 249),
                HoverColor = Color.FromArgb(226, 232, 240),
                ForeColor = AppColors.TextMedium,
                CustomBorderColor = AppColors.Border,
                CustomBorderWidth = 1,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold)
            };
            _btnDb.Click += (s, e) =>
            {
                using (var dlg = new DbConnectionDialog())
                {
                    dlg.ShowDialog();
                }
            };

            RoundedButton btnLogout = new RoundedButton
            {
                Text = "Logout",
                Location = new Point(174, 4),
                Size = new Size(85, 38),
                BorderRadius = 8,
                NormalColor = Color.FromArgb(254, 226, 226),
                HoverColor = Color.FromArgb(254, 205, 205),
                ForeColor = AppColors.StatusCancelled,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold)
            };
            btnLogout.Click += (s, e) => Logout();

            // User Info Badge
            RoundedPanel pnlUser = new RoundedPanel
            {
                Location = new Point(268, 2),
                Size = new Size(195, 42),
                BorderRadius = 10,
                BackColor = AppColors.PrimarySoft,
                BorderColor = AppColors.PrimaryBorder,
                DrawBorder = true
            };

            _lblUserName = new Label
            {
                Text = _authService.CurrentUser?.FullName ?? "Guest User",
                Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryDark,
                Location = new Point(8, 4),
                Size = new Size(180, 18),
                AutoEllipsis = true,
                BackColor = Color.Transparent
            };

            _lblUserRole = new Label
            {
                Text = $"Role: {_authService.CurrentUser?.Role ?? "Customer"}",
                Font = new Font("Segoe UI", 8f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(8, 22),
                Size = new Size(180, 16),
                BackColor = Color.Transparent
            };

            pnlUser.Controls.Add(_lblUserName);
            pnlUser.Controls.Add(_lblUserRole);

            pnlRight.Controls.Add(_btnDb);
            pnlRight.Controls.Add(btnLogout);
            pnlRight.Controls.Add(pnlUser);

            _pnlHeader.Controls.Add(lblBrand);
            _pnlHeader.Controls.Add(lblSubBrand);
            _pnlHeader.Controls.Add(pnlSearch);
            _pnlHeader.Controls.Add(pnlRight);
        }

        private void BuildSidebar()
        {
            _pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.White,
                Padding = new Padding(12, 16, 12, 16)
            };

            // Divider border on right
            _pnlSidebar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(AppColors.Border))
                {
                    e.Graphics.DrawLine(pen, _pnlSidebar.Width - 1, 0, _pnlSidebar.Width - 1, _pnlSidebar.Height);
                }
            };

            _flpNavButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            _pnlSidebar.Controls.Add(_flpNavButtons);
        }

        private void SetupRoleViews()
        {
            _flpNavButtons.Controls.Clear();
            User user = _authService.CurrentUser;

            if (user == null) return;

            _lblUserName.Text = !string.IsNullOrEmpty(user.FullName) ? user.FullName : user.Username;
            _lblUserRole.Text = $"Role: {user.Role}";

            if (_btnDb != null)
            {
                _btnDb.Visible = !user.IsCustomer;
            }

            if (user.IsAdmin)
            {
                AddNavButton("Stock Tracking", "Stock Tracking", "stock", () => ShowView(GetOrCreateAdminStockView()));
                AddNavButton("Menu Management", "Food Menu", "menu", () => ShowView(GetOrCreateAdminMenuView()));
                AddNavButton("Categories", "Categories", "category", () => ShowView(GetOrCreateAdminCategoryView()));
                AddNavButton("Promo Vouchers", "Promo Vouchers", "promo", () => ShowView(GetOrCreateAdminPromoView()));
                AddNavButton("Sales Report", "Sales Report", "report", () => ShowView(GetOrCreateAdminSalesView()));
                AddNavButton("User Management", "User Management", "users", () => ShowView(GetOrCreateAdminUsersView()));
                AddNavButton("Kitchen Queue", "Kitchen Queue", "kitchen", () => ShowView(GetOrCreateCrewQueueView()));
                AddNavButton("Audit Logs", "Audit Logs", "orders", () => ShowView(GetOrCreateAdminAuditView()));
                AddNavButton("Customer Menu", "Customer View", "cart", () => ShowView(GetOrCreateCustomerMenuView()));

                ShowView(GetOrCreateAdminStockView());
            }
            else if (user.IsCrew)
            {
                AddNavButton("Kitchen Queue", "Kitchen Queue", "kitchen", () => ShowView(GetOrCreateCrewQueueView()));
                ShowView(GetOrCreateCrewQueueView());
            }
            else // Customer
            {
                AddNavButton("Menu", "Menu & Order", "menu", () => ShowView(GetOrCreateCustomerMenuView()));
                AddNavButton("Orders", "My Orders", "orders", () => ShowView(GetOrCreateCustomerOrdersView()));
                ShowView(GetOrCreateCustomerMenuView());
            }
        }

        private void AddNavButton(string tabKey, string title, string iconName, Action onClick)
        {
            var btn = new RoundedButton
            {
                Text = title,
                Tag = tabKey,
                IconName = iconName,
                Size = new Size(184, 46),
                BorderRadius = 10,
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 8),
                Cursor = Cursors.Hand
            };

            btn.Click += (s, e) =>
            {
                _activeTab = tabKey;
                HighlightActiveNavButton();
                onClick();
            };

            _flpNavButtons.Controls.Add(btn);
            HighlightActiveNavButton();
        }

        private void HighlightActiveNavButton()
        {
            foreach (Control c in _flpNavButtons.Controls)
            {
                if (c is RoundedButton btn)
                {
                    bool isActive = (btn.Tag?.ToString() == _activeTab);
                    if (isActive)
                    {
                        btn.NormalColor = AppColors.PrimarySoft;
                        btn.ForeColor = AppColors.PrimaryDark;
                        btn.CustomBorderColor = AppColors.PrimaryBorder;
                        btn.CustomBorderWidth = 1;
                    }
                    else
                    {
                        btn.NormalColor = Color.White;
                        btn.ForeColor = AppColors.TextMedium;
                        btn.CustomBorderColor = Color.Transparent;
                        btn.CustomBorderWidth = 0;
                    }
                    btn.Invalidate();
                }
            }
        }

        private void ShowView(Control view)
        {
            _pnlContent.SuspendLayout();
            _pnlContent.Controls.Clear();
            view.Dock = DockStyle.Fill;
            _pnlContent.Controls.Add(view);
            _pnlContent.ResumeLayout(true);
        }

        private Control GetOrCreateCustomerMenuView()
        {
            if (_customerMenuControl == null)
            {
                _customerMenuControl = new CustomerMenuControl();
            }
            else
            {
                _customerMenuControl.LoadCategories();
                _customerMenuControl.LoadMenuItems();
                _customerMenuControl.RefreshCartView();
            }
            return _customerMenuControl;
        }

        private Control GetOrCreateCustomerOrdersView()
        {
            if (_customerOrdersControl == null)
            {
                _customerOrdersControl = new CustomerOrdersControl();
            }
            else
            {
                _customerOrdersControl.LoadCustomerOrders();
            }
            return _customerOrdersControl;
        }

        private Control GetOrCreateCrewQueueView()
        {
            if (_crewQueueControl == null)
            {
                _crewQueueControl = new CrewQueueControl();
            }
            else
            {
                _crewQueueControl.LoadQueueOrders();
            }
            return _crewQueueControl;
        }

        private Control GetOrCreateAdminStockView()
        {
            if (_adminStockControl == null)
            {
                _adminStockControl = new AdminStockControl();
            }
            _adminStockControl.RefreshStockData();
            return _adminStockControl;
        }

        private Control GetOrCreateAdminMenuView()
        {
            if (_adminMenuControl == null)
            {
                _adminMenuControl = new AdminMenuControl();
            }
            else
            {
                _adminMenuControl.LoadMenuGrid();
            }
            return _adminMenuControl;
        }

        private Control GetOrCreateAdminCategoryView()
        {
            if (_adminCategoryControl == null)
            {
                _adminCategoryControl = new AdminCategoryControl();
            }
            else
            {
                _adminCategoryControl.LoadCategoryGrid();
            }
            return _adminCategoryControl;
        }

        private Control GetOrCreateAdminUsersView()
        {
            if (_adminUsersControl == null)
            {
                _adminUsersControl = new AdminUsersControl();
            }
            else
            {
                _adminUsersControl.RefreshGrid();
            }
            return _adminUsersControl;
        }

        private Control GetOrCreateAdminAuditView()
        {
            if (_adminAuditControl == null)
            {
                _adminAuditControl = new AdminAuditControl();
            }
            else
            {
                _adminAuditControl.LoadAuditLogs();
            }
            return _adminAuditControl;
        }

        private Control GetOrCreateAdminPromoView()
        {
            if (_adminPromoControl == null)
            {
                _adminPromoControl = new AdminPromoControl();
            }
            else
            {
                _adminPromoControl.LoadPromoGrid();
            }
            return _adminPromoControl;
        }

        private Control GetOrCreateAdminSalesView()
        {
            if (_adminSalesControl == null)
            {
                _adminSalesControl = new AdminSalesReportControl();
            }
            else
            {
                _adminSalesControl.LoadReportData();
            }
            return _adminSalesControl;
        }

        private void Logout()
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _authService.Logout();
                OrderService.Instance.ClearCart();
                _customerMenuControl = null;
                _customerOrdersControl = null;
                _crewQueueControl = null;
                _adminStockControl = null;
                _adminMenuControl = null;
                _adminCategoryControl = null;
                _adminUsersControl = null;

                Hide();
                using (var login = new LoginForm())
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {
                        SetupRoleViews();
                        Show();
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }
    }
}
