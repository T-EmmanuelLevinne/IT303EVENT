using System;
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
    public partial class AdminUsersControl : UserControl
    {
        private readonly UserService _userService;
        private string _activeCategoryFilter = "All"; // "All", "Staff", "Customers", "Inactive"

        public AdminUsersControl()
        {
            _userService = UserService.Instance;
            InitializeComponent();
            SetupCustomUIAndEvents();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                _userService.UserDataChanged += RefreshGrid;
                RefreshGrid();
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

            if (_txtSearch != null) _txtSearch.TextChanged += (s, e) => RefreshGrid();

            _btnFilterAll.Click += (s, e) => { _activeCategoryFilter = "All"; UpdateFilterTabHighlights(); RefreshGrid(); };
            _btnFilterStaff.Click += (s, e) => { _activeCategoryFilter = "Staff"; UpdateFilterTabHighlights(); RefreshGrid(); };
            _btnFilterCustomers.Click += (s, e) => { _activeCategoryFilter = "Customers"; UpdateFilterTabHighlights(); RefreshGrid(); };
            _btnFilterInactive.Click += (s, e) => { _activeCategoryFilter = "Inactive"; UpdateFilterTabHighlights(); RefreshGrid(); };

            _btnAddUser.Click += BtnAddUser_Click;
            _btnEditUser.Click += BtnEditUser_Click;
            _btnResetPassword.Click += BtnResetPassword_Click;
            _btnToggleActive.Click += BtnToggleActive_Click;
            _btnUnlockUser.Click += BtnUnlockUser_Click;
            _btnApplyRole.Click += BtnApplyRole_Click;
            _btnDeleteUser.Click += BtnDeleteUser_Click;
            _btnSaveChanges.Click += BtnSaveChanges_Click;
            _btnCancelChanges.Click += BtnCancelChanges_Click;

            UIHelper.StyleDataGridView(_dgvUsers);
            _dgvUsers.Columns.Clear();
            _dgvUsers.Columns.Add("UserId", "ID");
            _dgvUsers.Columns.Add("Username", "Username");
            _dgvUsers.Columns.Add("FullName", "Full Name");
            _dgvUsers.Columns.Add("Role", "Role");
            _dgvUsers.Columns.Add("AccountStatus", "Status");
            _dgvUsers.Columns.Add("Phone", "Phone");
            _dgvUsers.Columns.Add("Address", "Address");
            _dgvUsers.Columns.Add("CreatedAt", "Registered On");

            UIHelper.ConfigureGridColumn(_dgvUsers, "UserId", 48);
            UIHelper.ConfigureGridColumn(_dgvUsers, "Username", 104);
            UIHelper.ConfigureGridColumn(_dgvUsers, "FullName", 110, fill: true);
            UIHelper.ConfigureGridColumn(_dgvUsers, "Role", 100);
            UIHelper.ConfigureGridColumn(_dgvUsers, "AccountStatus", 102);
            UIHelper.ConfigureGridColumn(_dgvUsers, "Phone", 108);
            UIHelper.ConfigureGridColumn(_dgvUsers, "Address", 110);
            UIHelper.ConfigureGridColumn(_dgvUsers, "CreatedAt", 136);

            UIHelper.AttachStatusPillPainter(_dgvUsers, UIHelper.ResolveSemanticStatusColors, "Role", "AccountStatus");
            _dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;
            _dgvUsers.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnEditUser_Click(s, EventArgs.Empty); };

            _dgvUsers.CellMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                {
                    _dgvUsers.ClearSelection();
                    _dgvUsers.Rows[e.RowIndex].Selected = true;
                    var user = GetSelectedUser();
                    if (user != null)
                    {
                        var menu = UIHelper.CreateModernContextMenu();
                        menu.Items.Add(UIHelper.CreateModernMenuItem($"Edit '{user.Username}'", "edit", (sender, args) => BtnEditUser_Click(this, EventArgs.Empty)));
                        menu.Items.Add(UIHelper.CreateModernMenuItem("Reset Password", "key", (sender, args) => BtnResetPassword_Click(this, EventArgs.Empty)));

                        if (user.IsLocked)
                        {
                            menu.Items.Add(UIHelper.CreateModernMenuItem("Unlock Account", "unlock", (sender, args) => BtnUnlockUser_Click(this, EventArgs.Empty), AppColors.StatusCompleted));
                        }

                        menu.Items.Add(UIHelper.CreateModernMenuItem(user.IsActive ? "Deactivate Account" : "Reactivate Account", "shield", (sender, args) => BtnToggleActive_Click(this, EventArgs.Empty), AppColors.StatusPending));

                        var itemRole = UIHelper.CreateModernMenuItem("Quick Role Change", "users", null);
                        var roleCust = UIHelper.CreateModernMenuItem("Customer", "cart", (sender, args) => ChangeUserRole(user, "Customer"));
                        if (user.Role == "Customer") roleCust.Checked = true;
                        var roleCrew = UIHelper.CreateModernMenuItem("Kitchen Crew", "kitchen", (sender, args) => ChangeUserRole(user, "Crew"));
                        if (user.Role == "Crew") roleCrew.Checked = true;
                        var roleAdmin = UIHelper.CreateModernMenuItem("Administrator", "star", (sender, args) => ChangeUserRole(user, "Admin"));
                        if (user.Role == "Admin") roleAdmin.Checked = true;
                        itemRole.DropDownItems.Add(roleCust);
                        itemRole.DropDownItems.Add(roleCrew);
                        itemRole.DropDownItems.Add(roleAdmin);
                        menu.Items.Add(itemRole);

                        menu.Items.Add(new ToolStripSeparator());
                        menu.Items.Add(UIHelper.CreateModernMenuItem("Delete User", "delete", (sender, args) => BtnDeleteUser_Click(this, EventArgs.Empty), AppColors.StatusCancelled));
                        menu.Show(Cursor.Position);
                    }
                }
            };

            UpdateFilterTabHighlights();
        }

        private void DgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            var user = GetSelectedUser();
            if (user != null)
            {
                _btnToggleActive.Text = user.IsActive ? "Deactivate" : "Reactivate";
                _btnUnlockUser.Enabled = user.IsLocked || user.FailedLoginAttempts > 0;
            }
        }

        private void UpdateFilterTabHighlights()
        {
            HighlightButton(_btnFilterAll, _activeCategoryFilter == "All");
            HighlightButton(_btnFilterStaff, _activeCategoryFilter == "Staff");
            HighlightButton(_btnFilterCustomers, _activeCategoryFilter == "Customers");
            HighlightButton(_btnFilterInactive, _activeCategoryFilter == "Inactive");
        }

        private void HighlightButton(RoundedButton btn, bool active)
        {
            if (active)
            {
                btn.NormalColor = AppColors.Primary;
                btn.HoverColor = AppColors.PrimaryLight;
                btn.ForeColor = Color.White;
                btn.CustomBorderWidth = 0;
            }
            else
            {
                btn.NormalColor = Color.White;
                btn.HoverColor = AppColors.SurfaceAlt;
                btn.ForeColor = AppColors.TextMedium;
                btn.CustomBorderColor = AppColors.Border;
                btn.CustomBorderWidth = 1;
            }
            btn.Invalidate();
        }

        public void RefreshGrid()
        {
            try
            {
                AuthService.Instance.RequireRole("Admin");
                _dgvUsers.Rows.Clear();
                var users = _userService.WorkingUsers;
                if (users != null)
                {
                    var filtered = users.AsEnumerable();
                    if (_activeCategoryFilter == "Staff")
                        filtered = filtered.Where(u => u.Role == "Admin" || u.Role == "Crew");
                    else if (_activeCategoryFilter == "Customers")
                        filtered = filtered.Where(u => u.Role == "Customer");
                    else if (_activeCategoryFilter == "Inactive")
                        filtered = filtered.Where(u => !u.IsActive || u.IsLocked);

                    string q = _txtSearch?.Text?.Trim().ToLower() ?? "";
                    if (!string.IsNullOrEmpty(q))
                    {
                        filtered = filtered.Where(u => (u.Username ?? "").ToLower().Contains(q) ||
                                                       (u.FullName ?? "").ToLower().Contains(q) ||
                                                       (u.Role ?? "").ToLower().Contains(q) ||
                                                       (u.AccountStatusDisplay ?? "").ToLower().Contains(q) ||
                                                       (u.Phone ?? "").ToLower().Contains(q) ||
                                                       (u.Address ?? "").ToLower().Contains(q));
                    }

                    foreach (var u in filtered)
                    {
                        _dgvUsers.Rows.Add(
                            u.UserId,
                            u.Username,
                            u.FullName,
                            u.Role,
                            u.AccountStatusDisplay,
                            !string.IsNullOrEmpty(u.Phone) ? u.Phone : "-",
                            !string.IsNullOrEmpty(u.Address) ? u.Address : "-",
                            u.CreatedAt.ToString("g")
                        );
                    }
                }

                bool dirty = _userService.HasPendingChanges;
                _lblPendingBanner.Visible = dirty;
                _bottomPanel.Visible = dirty;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AdminUsersControl.RefreshGrid");
            }
        }

        private User GetSelectedUser()
        {
            if (_dgvUsers.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(_dgvUsers.SelectedRows[0].Cells["UserId"].Value);
                return _userService.WorkingUsers.FirstOrDefault(u => u.UserId == id);
            }
            return null;
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            using (var dlg = new AddEditUserDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshGrid();
                }
            }
        }

        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            var user = GetSelectedUser();
            if (user == null)
            {
                MessageBox.Show(this, "Please select a user account to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new AddEditUserDialog(user))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshGrid();
                }
            }
        }

        private void BtnResetPassword_Click(object sender, EventArgs e)
        {
            var user = GetSelectedUser();
            if (user == null)
            {
                MessageBox.Show(this, "Please select a user account to reset its password.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new ResetPasswordDialog(user))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshGrid();
                }
            }
        }

        private void BtnToggleActive_Click(object sender, EventArgs e)
        {
            var user = GetSelectedUser();
            if (user == null)
            {
                MessageBox.Show(this, "Please select a user account.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool nextState = !user.IsActive;
            string verb = nextState ? "reactivate" : "deactivate";
            if (MessageBox.Show(this, $"Are you sure you want to {verb} account '@{user.Username}'?", "Confirm Account Status", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_userService.SetUserActiveStatus(user.UserId, nextState, out string err))
                {
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show(this, err, "Status Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BtnUnlockUser_Click(object sender, EventArgs e)
        {
            var user = GetSelectedUser();
            if (user == null)
            {
                MessageBox.Show(this, "Please select a user account to unlock.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!user.IsLocked && user.FailedLoginAttempts == 0)
            {
                MessageBox.Show(this, $"Account '@{user.Username}' is already unlocked.", "Not Locked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, $"Unlock account '@{user.Username}' and reset failed login attempts?", "Unlock Account", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_userService.UnlockUser(user.UserId, out string err))
                {
                    MessageBox.Show(this, $"Account '@{user.Username}' unlocked.", "Account Unlocked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show(this, err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ChangeUserRole(User user, string newRole)
        {
            if (user == null)
            {
                MessageBox.Show(this, "Please select a user from the table to change their role.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (user.Role == newRole)
            {
                MessageBox.Show(this, $"User '{user.Username}' already has role '{newRole}'.", "No Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool isPrivileged = (newRole == "Crew" || newRole == "Admin" || user.Role == "Crew" || user.Role == "Admin");
            if (isPrivileged)
            {
                using (var dlg = new ConfirmRoleChangeDialog(user, newRole))
                {
                    if (dlg.ShowDialog(this.FindForm()) != DialogResult.OK) return;
                }
            }

            _userService.UpdateUserRole(user.UserId, newRole);
            AuditService.Instance.Log("Role Change Authorized", $"Changed role for '{user.Username}' ({user.FullName}) from '{user.Role}' to '{newRole}'.", "Users", user.UserId.ToString());
            RefreshGrid();
            MessageBox.Show(this, $"Role for '{user.FullName}' queued to change to '{newRole}'. Click 'Save Role Changes' to persist.", "Role Queued", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnApplyRole_Click(object sender, EventArgs e)
        {
            var user = GetSelectedUser();
            if (user == null)
            {
                MessageBox.Show(this, "Please select a user from the table to change their role.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string newRole = _cboChangeRole.SelectedItem?.ToString() ?? "Customer";
            ChangeUserRole(user, newRole);
        }

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            var user = GetSelectedUser();
            if (user == null)
            {
                MessageBox.Show(this, "Please select a user to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (user.UserId == AuthService.Instance.CurrentUser?.UserId)
            {
                MessageBox.Show(this, "You cannot delete your own currently signed-in account.", "Action Forbidden", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(this, $"Permanently delete user '@{user.Username}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_userService.DeleteUser(user.UserId, out string err))
                {
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show(this, err, "Cannot Delete User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BtnCancelChanges_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Discard all unsaved user role changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _userService.CancelChanges();
                RefreshGrid();
            }
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            if (_userService.SaveChanges(out string err))
            {
                MessageBox.Show(this, "All user role updates saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshGrid();
            }
            else
            {
                MessageBox.Show(this, $"Failed to save changes: {err}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
