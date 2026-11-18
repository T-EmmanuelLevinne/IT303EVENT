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
        private RoundedPanel _pnlSearch;
        private Label _lblUserName;
        private Label _lblUserRole;
        private RoundedButton _btnDb;

        // Sidebar Navigation Buttons
        private FlowLayoutPanel _flpNavButtons;

        // View Instances
        private AdminDashboardControl _adminDashboardControl;
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
        private AdminBackupControl _adminBackupControl;

        private string _activeTab = "Dashboard";

        public MainForm()
        {
            _authService = AuthService.Instance;
            InitializeComponent();
            SetupRoleViews();
        }

        private void InitializeComponent()
        {
            Text = "Tonyo's Pizza - Restaurant & Kitchen Management";
            Size = new Size(1340, 860);
            MinimumSize = new Size(1120, 720);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = AppColors.Background;
            DoubleBuffered = true;

            BuildHeader();
            BuildSidebar();

            _pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = AppColors.Background
            };
            UIHelper.EnableDoubleBuffering(_pnlContent);

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
                Padding = new Padding(20, 12, 24, 12)
            };

            _pnlHeader.Paint += (s, e) =>
            {
                UIHelper.SetHighQuality(e.Graphics);
                using (Pen pen = new Pen(AppColors.Border))
                {
                    e.Graphics.DrawLine(pen, 0, _pnlHeader.Height - 1, _pnlHeader.Width, _pnlHeader.Height - 1);
                }
            };

            // Artisanal Brand Mark
            Panel pnlBrand = new Panel
            {
                Location = new Point(16, 10),
                Size = new Size(256, 52),
                BackColor = Color.White
            };
            pnlBrand.Paint += (s, e) =>
            {
                UIHelper.SetHighQuality(e.Graphics);
                using (var b = new SolidBrush(AppColors.Primary))
                {
                    e.Graphics.FillEllipse(b, 0, 8, 34, 34);
                }
                UIHelper.DrawVectorIcon(e.Graphics, "pizza", new Rectangle(7, 15, 20, 20), Color.White);
            };

            Label lblBrand = new Label
            {
                Text = "Tonyo's Pizza",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = AppColors.TextDark,
                Location = new Point(42, 3),
                Size = new Size(206, 26),
                UseMnemonic = false,
                BackColor = Color.Transparent
            };

            Label lblSubBrand = new Label
            {
                Text = "Brick-Oven Kitchen & POS",
                Font = new Font("Segoe UI", 8.25f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(43, 30),
                Size = new Size(206, 18),
                UseMnemonic = false,
                BackColor = Color.Transparent
            };

            pnlBrand.Controls.Add(lblSubBrand);
            pnlBrand.Controls.Add(lblBrand);

            // Clean Search Bar (active for Customer Menu)
            _pnlSearch = new RoundedPanel
            {
                Location = new Point(275, 15),
                Size = new Size(340, 40),
                BorderRadius = 10,
                BackColor = AppColors.Background,
                BorderColor = AppColors.Border,
                DrawBorder = true
            };

            Label lblSearchIcon = new Label
            {
                Location = new Point(12, 12),
                Size = new Size(16, 16),
                BackColor = Color.Transparent
            };
            lblSearchIcon.Paint += (s, e) =>
            {
                UIHelper.SetHighQuality(e.Graphics);
                UIHelper.DrawVectorIcon(e.Graphics, "search", new Rectangle(0, 0, 15, 15), AppColors.TextLight);
            };

            _txtGlobalSearch = new TextBox
            {
                Location = new Point(36, 11),
                Width = 288,
                Font = new Font("Segoe UI", 9.75f),
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

            _pnlSearch.Controls.Add(_txtGlobalSearch);
            _pnlSearch.Controls.Add(lblSearchIcon);

            // Right User Profile & Actions
            Panel pnlRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 450,
                Height = 46,
                BackColor = Color.White
            };

            _btnDb = new RoundedButton
            {
                Text = "DB Settings",
                Location = new Point(42, 4),
                Size = new Size(102, 38),
                BorderRadius = 8,
                NormalColor = AppColors.SecondaryNormal,
                HoverColor = AppColors.SecondaryHover,
                PressedColor = AppColors.SecondaryPressed,
                ForeColor = AppColors.TextMedium,
                CustomBorderColor = AppColors.Border,
                CustomBorderWidth = 1,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold)
            };
            _btnDb.Click += (s, e) =>
            {
                try
                {
                    AuthService.Instance.RequireRole("Admin", "Crew");
                    using (var dlg = new DbConnectionDialog())
                    {
                        dlg.ShowDialog(this);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(this, ex.Message, "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            RoundedButton btnLogout = new RoundedButton
            {
                Text = "Sign Out",
                Location = new Point(152, 4),
                Size = new Size(88, 38),
                BorderRadius = 8,
                NormalColor = AppColors.StatusCancelledBg,
                HoverColor = Color.FromArgb(254, 205, 205),
                PressedColor = Color.FromArgb(252, 165, 165),
                ForeColor = AppColors.StatusCancelled,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold)
            };
            btnLogout.Click += (s, e) => Logout();

            RoundedPanel pnlUser = new RoundedPanel
            {
                Location = new Point(248, 2),
                Size = new Size(198, 42),
                BorderRadius = 10,
                BackColor = AppColors.PrimarySoft,
                BorderColor = AppColors.PrimaryBorder,
                DrawBorder = true
            };

            _lblUserName = new Label
            {
                Text = _authService.CurrentUser?.FullName ?? "Guest User",
                Font = new Font("Segoe UI", 9.25f, FontStyle.Bold),
                ForeColor = AppColors.PrimaryDark,
                Location = new Point(10, 4),
                Size = new Size(178, 18),
                AutoEllipsis = true,
                BackColor = Color.Transparent
            };

            _lblUserRole = new Label
            {
                Text = $"Role: {_authService.CurrentUser?.Role ?? "Customer"}",
                Font = new Font("Segoe UI", 8f),
                ForeColor = AppColors.TextMuted,
                Location = new Point(10, 22),
                Size = new Size(178, 16),
                BackColor = Color.Transparent
            };

            pnlUser.Controls.Add(_lblUserName);
            pnlUser.Controls.Add(_lblUserRole);

            pnlRight.Controls.Add(_btnDb);
            pnlRight.Controls.Add(btnLogout);
            pnlRight.Controls.Add(pnlUser);

            _pnlHeader.Controls.Add(pnlBrand);
            _pnlHeader.Controls.Add(_pnlSearch);
            _pnlHeader.Controls.Add(pnlRight);
        }

        private void BuildSidebar()
        {
            _pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 216,
                BackColor = Color.White,
                Padding = new Padding(12, 14, 12, 14)
            };

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
                AutoScroll = true,
                BackColor = Color.White
            };
            UIHelper.EnableDoubleBuffering(_flpNavButtons);

            _pnlSidebar.Controls.Add(_flpNavButtons);
        }

        private void SetupRoleViews()
        {
            _flpNavButtons.Controls.Clear();
            User user = _authService.CurrentUser;

            if (user == null) return;

            _lblUserName.Text = !string.IsNullOrEmpty(user.FullName) ? user.FullName : user.Username;
            _lblUserRole.Text = user.IsGuestAccount ? "Self-Order Kiosk" : $"Role: {user.Role}";

            if (_btnDb != null)
            {
                _btnDb.Visible = user.IsAdmin;
            }

            if (_pnlSearch != null)
            {
                _pnlSearch.Visible = !user.IsGuestAccount;
            }

            if (_pnlSidebar != null)
            {
                _pnlSidebar.Visible = !user.IsGuestAccount;
            }

            if (user.IsAdmin)
            {
                AddNavButton("Dashboard", "Dashboard", "dashboard", () => NavigateToTab("Dashboard"));
                AddNavButton("Stock Tracking", "Stock & Restock", "stock", () => NavigateToTab("Stock Tracking"));
                AddNavButton("Menu Management", "Food Menu", "menu", () => NavigateToTab("Menu Management"));
                AddNavButton("Categories", "Categories", "category", () => NavigateToTab("Categories"));
                AddNavButton("Promo Vouchers", "Promo Vouchers", "promo", () => NavigateToTab("Promo Vouchers"));
                AddNavButton("Sales Report", "Sales Report", "report", () => NavigateToTab("Sales Report"));
                AddNavButton("User Management", "User Accounts", "users", () => NavigateToTab("User Management"));
                AddNavButton("Kitchen Queue", "Kitchen Queue", "kitchen", () => NavigateToTab("Kitchen Queue"));
                AddNavButton("Audit Logs", "Audit Trail", "orders", () => NavigateToTab("Audit Logs"));
                AddNavButton("Backup & Restore", "Backup & Restore", "backup", () => NavigateToTab("Backup & Restore"));
                AddNavButton("Customer Menu", "Order Kiosk", "cart", () => NavigateToTab("Customer Menu"));

                NavigateToTab("Dashboard");
            }
            else if (user.IsCrew)
            {
                AddNavButton("Kitchen Queue", "Kitchen Queue", "kitchen", () => NavigateToTab("Kitchen Queue"));
                NavigateToTab("Kitchen Queue");
            }
            else if (user.IsGuestAccount)
            {
                AddNavButton("Menu", "Order Kiosk", "cart", () => NavigateToTab("Menu"));
                NavigateToTab("Menu");
            }
            else // Registered Customer
            {
                AddNavButton("Menu", "Menu & Order", "menu", () => NavigateToTab("Menu"));
                AddNavButton("Orders", "My Orders", "orders", () => NavigateToTab("Orders"));
                NavigateToTab("Menu");
            }
        }

        public void NavigateToTab(string tabKey)
        {
            try
            {
                User user = _authService.CurrentUser;
                if (user == null) return;

                Control targetView = null;
                switch (tabKey)
                {
                    case "Dashboard":
                        AuthService.Instance.RequireRole("Admin");
                        targetView = GetOrCreateAdminDashboardView();
                        break;
                    case "Stock Tracking":
                        AuthService.Instance.RequireRole("Admin");
                        targetView = GetOrCreateAdminStockView();
                        break;
                    case "Menu Management":
                        AuthService.Instance.RequireRole("Admin");
                        targetView = GetOrCreateAdminMenuView();
                        break;
                    case "Categories":
                        AuthService.Instance.RequireRole("Admin");
                        targetView = GetOrCreateAdminCategoryView();
                        break;
                    case "Promo Vouchers":
                        AuthService.Instance.RequireRole("Admin");
                        targetView = GetOrCreateAdminPromoView();
                        break;
                    case "Sales Report":
                        AuthService.Instance.RequireRole("Admin");
                        targetView = GetOrCreateAdminSalesView();
                        break;
                    case "User Management":
                        AuthService.Instance.RequireRole("Admin");
                        targetView = GetOrCreateAdminUsersView();
                        break;
                    case "Audit Logs":
                        AuthService.Instance.RequireRole("Admin");
                        targetView = GetOrCreateAdminAuditView();
                        break;
                    case "Backup & Restore":
                        AuthService.Instance.RequireRole("Admin");
                        targetView = GetOrCreateAdminBackupView();
                        break;
                    case "Kitchen Queue":
                        AuthService.Instance.RequireRole("Admin", "Crew");
                        targetView = GetOrCreateCrewQueueView();
                        break;
                    case "Customer Menu":
                    case "Menu":
                        targetView = GetOrCreateCustomerMenuView();
                        break;
                    case "Orders":
                        targetView = GetOrCreateCustomerOrdersView();
                        break;
                }

                if (targetView != null)
                {
                    _activeTab = tabKey;
                    HighlightActiveNavButton();
                    ShowView(targetView);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(this, ex.Message, "Role Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Unable to open the selected view.", ex, "Navigation Error", this);
            }
        }

        private void AddNavButton(string tabKey, string title, string iconName, Action onClick)
        {
            var btn = new RoundedButton
            {
                Text = title,
                Tag = tabKey,
                IconName = iconName,
                Size = new Size(190, 42),
                BorderRadius = 9,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 6),
                Cursor = Cursors.Hand,
                NormalColor = Color.White,
                HoverColor = AppColors.Background,
                PressedColor = AppColors.PrimarySoft,
                ForeColor = AppColors.TextMedium
            };

            btn.Click += (s, e) => onClick();

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
                        btn.HoverColor = AppColors.PrimarySoft;
                        btn.ForeColor = AppColors.PrimaryDark;
                        btn.CustomBorderColor = AppColors.PrimaryBorder;
                        btn.CustomBorderWidth = 1;
                        btn.ShowActiveAccentBar = true;
                    }
                    else
                    {
                        btn.NormalColor = Color.White;
                        btn.HoverColor = AppColors.Background;
                        btn.ForeColor = AppColors.TextMedium;
                        btn.CustomBorderColor = Color.Transparent;
                        btn.CustomBorderWidth = 0;
                        btn.ShowActiveAccentBar = false;
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

        private Control GetOrCreateAdminDashboardView()
        {
            if (_adminDashboardControl == null)
            {
                _adminDashboardControl = new AdminDashboardControl();
                _adminDashboardControl.NavigateRequested += (tabKey) => NavigateToTab(tabKey);
            }
            else
            {
                _adminDashboardControl.RefreshDashboard();
            }
            return _adminDashboardControl;
        }

        private Control GetOrCreateCustomerMenuView()
        {
            if (_customerMenuControl == null)
            {
                _customerMenuControl = new CustomerMenuControl();
                _customerMenuControl.KioskBrowsingChanged += (isBrowsing) =>
                {
                    if (_pnlSearch != null && _authService?.CurrentUser?.IsGuestAccount == true)
                    {
                        _pnlSearch.Visible = isBrowsing;
                        if (!isBrowsing && _txtGlobalSearch != null)
                        {
                            _txtGlobalSearch.Text = string.Empty;
                        }
                    }
                };
            }
            else
            {
                _customerMenuControl.LoadCategories();
                _customerMenuControl.LoadMenuItems();
                _customerMenuControl.RefreshCartView();
            }
            _customerMenuControl.InitializeForCurrentUser();
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

        private Control GetOrCreateAdminBackupView()
        {
            if (_adminBackupControl == null)
            {
                _adminBackupControl = new AdminBackupControl();
            }
            else
            {
                _adminBackupControl.LoadBackupData();
            }
            return _adminBackupControl;
        }

        private void Logout()
        {
            if (MessageBox.Show(this, "Are you sure you want to sign out?", "Sign Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _authService.Logout();
                OrderService.Instance.ClearCart();
                _adminDashboardControl = null;
                _customerMenuControl = null;
                _customerOrdersControl = null;
                _crewQueueControl = null;
                _adminStockControl = null;
                _adminMenuControl = null;
                _adminCategoryControl = null;
                _adminUsersControl = null;
                _adminAuditControl = null;
                _adminPromoControl = null;
                _adminSalesControl = null;
                _adminBackupControl = null;

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
