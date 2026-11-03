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
        private string _activeCategoryFilter = "All"; // "All", "Staff", "Customers"

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
            if (lblSearchIcon != null)
            {
                lblSearchIcon.Paint += (s, e) =>
                {
                    UIHelper.SetHighQuality(e.Graphics);
                    UIHelper.DrawVectorIcon(e.Graphics, "search", new Rectangle(0, 0, 16, 16), AppColors.TextLight);
                };
            }

            if (_txtSearch != null) _txtSearch.TextChanged += (s, e) => RefreshGrid();

            if (_btnFilterAll != null)
            {
                _btnFilterAll.Click += (s, e) => { _activeCategoryFilter = "All"; UpdateFilterTabHighlights(); RefreshGrid(); };
            }
            if (_btnFilterStaff != null)
            {
                _btnFilterStaff.Click += (s, e) => { _activeCategoryFilter = "Staff"; UpdateFilterTabHighlights(); RefreshGrid(); };
            }
            if (_btnFilterCustomers != null)
            {
                _btnFilterCustomers.Click += (s, e) => { _activeCategoryFilter = "Customers"; UpdateFilterTabHighlights(); RefreshGrid(); };
            }

            if (_btnApplyRole != null) _btnApplyRole.Click += BtnApplyRole_Click;
            if (_btnDeleteUser != null) _btnDeleteUser.Click += BtnDeleteUser_Click;
            if (_btnSaveChanges != null) _btnSaveChanges.Click += BtnSaveChanges_Click;
            if (_btnCancelChanges != null) _btnCancelChanges.Click += BtnCancelChanges_Click;

            if (_dgvUsers != null)
            {
                UIHelper.StyleDataGridView(_dgvUsers);
                if (_dgvUsers.Columns.Count == 0)
                {
                    _dgvUsers.Columns.Add("UserId", "ID");
                    _dgvUsers.Columns.Add("Username", "Username");
                    _dgvUsers.Columns.Add("FullName", "Full Name");
                    _dgvUsers.Columns.Add("Role", "Current Role");
                    _dgvUsers.Columns.Add("Phone", "Phone Number");
                    _dgvUsers.Columns.Add("Address", "Address");
                    _dgvUsers.Columns.Add("CreatedAt", "Registered On");

                    _dgvUsers.Columns["UserId"].Width = 60;
                    _dgvUsers.Columns["Role"].Width = 120;
                    _dgvUsers.Columns["Phone"].Width = 120;
                    _dgvUsers.Columns["CreatedAt"].Width = 140;
                    _dgvUsers.Columns["FullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                _dgvUsers.CellFormatting += DgvUsers_CellFormatting;

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
                            var itemRole = UIHelper.CreateModernMenuItem("Change Role", "users", null);

                            var roleCust = UIHelper.CreateModernMenuItem("Customer", "cart", (sender, args) => ChangeUserRole(user, "Customer"));
                            if (user.Role == "Customer") roleCust.Checked = true;

                            var roleCrew = UIHelper.CreateModernMenuItem("Kitchen Crew", "kitchen", (sender, args) => ChangeUserRole(user, "Crew"));
                            if (user.Role == "Crew") roleCrew.Checked = true;

                            var roleAdmin = UIHelper.CreateModernMenuItem("Administrator", "star", (sender, args) => ChangeUserRole(user, "Admin"));
                            if (user.Role == "Admin") roleAdmin.Checked = true;

                            itemRole.DropDownItems.Add(roleCust);
                            itemRole.DropDownItems.Add(roleCrew);
                            itemRole.DropDownItems.Add(roleAdmin);

                            var itemDelete = UIHelper.CreateModernMenuItem("Delete User", "delete", (sender, args) => BtnDeleteUser_Click(this, EventArgs.Empty), Color.FromArgb(220, 38, 38));

                            menu.Items.Add(itemRole);
                            menu.Items.Add(new ToolStripSeparator());
                            menu.Items.Add(itemDelete);

                            menu.Show(Cursor.Position);
                        }
                    }
                };
            }

            UpdateFilterTabHighlights();
        }

        private RoundedButton CreateFilterButton(string text, string filterKey, int x)
        {
            var btn = new RoundedButton
            {
                Text = text,
                Location = new Point(x, 2),
                Height = 32,
                BorderRadius = 6,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold)
            };
            btn.Width = TextRenderer.MeasureText(text, btn.Font).Width + 24;

            btn.Click += (s, e) =>
            {
                _activeCategoryFilter = filterKey;
                UpdateFilterTabHighlights();
                RefreshGrid();
            };
            return btn;
        }

        private void UpdateFilterTabHighlights()
        {
            HighlightButton(_btnFilterAll, _activeCategoryFilter == "All");
            HighlightButton(_btnFilterStaff, _activeCategoryFilter == "Staff");
            HighlightButton(_btnFilterCustomers, _activeCategoryFilter == "Customers");
        }

        private void HighlightButton(RoundedButton btn, bool active)
        {
            if (active)
            {
                btn.NormalColor = AppColors.Primary;
                btn.ForeColor = Color.White;
            }
            else
            {
                btn.NormalColor = Color.White;
                btn.ForeColor = AppColors.TextMedium;
                btn.CustomBorderColor = AppColors.Border;
                btn.CustomBorderWidth = 1;
            }
            btn.Invalidate();
        }

        private void DgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_dgvUsers.Columns[e.ColumnIndex].Name == "Role" && e.Value != null)
            {
                string role = e.Value.ToString();
                if (role == "Admin")
                {
                    e.CellStyle.ForeColor = AppColors.PrimaryDark;
                    e.CellStyle.Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold);
                }
                else if (role == "Crew")
                {
                    e.CellStyle.ForeColor = AppColors.StatusPreparing;
                    e.CellStyle.Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = AppColors.StatusCompleted;
                    e.CellStyle.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
                }
            }
        }

        public void RefreshGrid()
        {
            _dgvUsers.Rows.Clear();
            var users = _userService.WorkingUsers;
            if (users != null)
            {
                var filtered = users.AsEnumerable();
                if (_activeCategoryFilter == "Staff")
                {
                    filtered = filtered.Where(u => u.Role == "Admin" || u.Role == "Crew");
                }
                else if (_activeCategoryFilter == "Customers")
                {
                    filtered = filtered.Where(u => u.Role == "Customer");
                }

                string q = _txtSearch?.Text?.Trim().ToLower() ?? "";
                if (!string.IsNullOrEmpty(q))
                {
                    filtered = filtered.Where(u => (u.Username ?? "").ToLower().Contains(q) ||
                                                   (u.FullName ?? "").ToLower().Contains(q) ||
                                                   (u.Role ?? "").ToLower().Contains(q) ||
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

        private User GetSelectedUser()
        {
            if (_dgvUsers.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(_dgvUsers.SelectedRows[0].Cells["UserId"].Value);
                return _userService.WorkingUsers.FirstOrDefault(u => u.UserId == id);
            }
            return null;
        }

        private void ChangeUserRole(User user, string newRole)
        {
            if (user == null)
            {
                MessageBox.Show("Please select a user from the table to change their role.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (user.Role == newRole)
            {
                MessageBox.Show($"User '{user.Username}' already has role '{newRole}'.", "No Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Security verification: If assigning to or modifying Kitchen Crew / Admin roles
            bool isPrivileged = (newRole == "Crew" || newRole == "Admin" || user.Role == "Crew" || user.Role == "Admin");
            if (isPrivileged)
            {
                using (var dlg = new ConfirmRoleChangeDialog(user, newRole))
                {
                    if (dlg.ShowDialog(this.FindForm()) != DialogResult.OK)
                    {
                        return; // Cancelled or verification failed
                    }
                }
            }

            _userService.UpdateUserRole(user.UserId, newRole);
            AuditService.Instance.Log("Role Change Authorized", $"Admin '{AuthService.Instance.CurrentUser?.Username}' changed role for '{user.Username}' ({user.FullName}) from '{user.Role}' to '{newRole}'.");
            RefreshGrid();
            MessageBox.Show($"Role for user '{user.FullName}' (@{user.Username}) has been successfully updated to '{newRole}'.", "Role Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnApplyRole_Click(object sender, EventArgs e)
        {
            var user = GetSelectedUser();
            if (user == null)
            {
                MessageBox.Show("Please select a user from the table to change their role.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Please select a user to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (user.UserId == AuthService.Instance.CurrentUser?.UserId)
            {
                MessageBox.Show("You cannot delete your own currently logged-in account.", "Action Forbidden", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete user '{user.Username}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_userService.DeleteUser(user.UserId, out string err))
                {
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show($"Failed to delete user: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancelChanges_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Discard all unsaved user role changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _userService.CancelChanges();
                RefreshGrid();
            }
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            if (_userService.SaveChanges(out string err))
            {
                MessageBox.Show("All user role updates saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshGrid();
            }
            else
            {
                MessageBox.Show($"Failed to save changes: {err}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
