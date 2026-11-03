using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Forms.Dialogs;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminPromoControl : UserControl
    {
        private readonly PromoVoucherRepository _promoRepo;

        public AdminPromoControl()
        {
            _promoRepo = new PromoVoucherRepository();
            InitializeComponent();
            SetupCustomUIAndEvents();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                LoadPromoGrid();
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

            if (_txtSearch != null) _txtSearch.TextChanged += (s, e) => LoadPromoGrid();
            if (_btnAdd != null) _btnAdd.Click += BtnAdd_Click;
            if (_btnEdit != null) _btnEdit.Click += BtnEdit_Click;
            if (_btnToggle != null) _btnToggle.Click += BtnToggle_Click;
            if (_btnDelete != null) _btnDelete.Click += BtnDelete_Click;
            if (_btnRefresh != null) _btnRefresh.Click += (s, e) => LoadPromoGrid();

            if (_dgvPromos != null)
            {
                UIHelper.StyleDataGridView(_dgvPromos);
                if (_dgvPromos.Columns.Count == 0)
                {
                    _dgvPromos.Columns.Add("PromoId", "ID");
                    _dgvPromos.Columns.Add("Code", "Voucher Code");
                    _dgvPromos.Columns.Add("Discount", "Discount");
                    _dgvPromos.Columns.Add("DiscountType", "Type");
                    _dgvPromos.Columns.Add("MinSpend", "Min Spend");
                    _dgvPromos.Columns.Add("Status", "Status");
                    _dgvPromos.Columns.Add("Description", "Description");
                    _dgvPromos.Columns.Add("CreatedAt", "Created");

                    _dgvPromos.Columns["PromoId"].Width = 60;
                    _dgvPromos.Columns["Code"].Width = 140;
                    _dgvPromos.Columns["Discount"].Width = 130;
                    _dgvPromos.Columns["DiscountType"].Width = 120;
                    _dgvPromos.Columns["MinSpend"].Width = 120;
                    _dgvPromos.Columns["Status"].Width = 110;
                    _dgvPromos.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    _dgvPromos.Columns["CreatedAt"].Width = 150;
                }

                _dgvPromos.CellFormatting += DgvPromos_CellFormatting;
                _dgvPromos.CellMouseDown += DgvPromos_CellMouseDown;
            }
        }

        private void DgvPromos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string col = _dgvPromos.Columns[e.ColumnIndex].Name;

            if (col == "Status")
            {
                string status = e.Value.ToString();
                if (status == "ACTIVE")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129);
                    e.CellStyle.Font = new Font("Segoe UI Bold", 9f, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68);
                    e.CellStyle.Font = new Font("Segoe UI Bold", 9f, FontStyle.Bold);
                }
            }
            else if (col == "Code")
            {
                e.CellStyle.ForeColor = AppColors.PrimaryDark;
                e.CellStyle.Font = new Font("Segoe UI Bold", 9.5f, FontStyle.Bold);
            }
            else if (col == "Discount")
            {
                e.CellStyle.ForeColor = Color.FromArgb(190, 24, 93);
                e.CellStyle.Font = new Font("Segoe UI Bold", 9f, FontStyle.Bold);
            }
        }

        private void DgvPromos_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                _dgvPromos.ClearSelection();
                _dgvPromos.Rows[e.RowIndex].Selected = true;

                var voucher = _dgvPromos.Rows[e.RowIndex].Tag as PromoVoucher;
                if (voucher != null)
                {
                    ContextMenuStrip menu = UIHelper.CreateModernContextMenu();
                    var itemEdit = UIHelper.CreateModernMenuItem($"Edit '{voucher.Code}'", "edit", (s, args) => EditVoucher(voucher));
                    var itemToggle = UIHelper.CreateModernMenuItem(voucher.IsActive ? "Deactivate Voucher" : "Activate Voucher", "check", (s, args) => ToggleVoucher(voucher));
                    var itemDelete = UIHelper.CreateModernMenuItem($"Delete '{voucher.Code}'", "delete", (s, args) => DeleteVoucher(voucher));

                    menu.Items.Add(itemEdit);
                    menu.Items.Add(itemToggle);
                    menu.Items.Add(new ToolStripSeparator());
                    menu.Items.Add(itemDelete);

                    menu.Show(Cursor.Position);
                }
            }
        }

        public void LoadPromoGrid()
        {
            string search = _txtSearch?.Text?.Trim() ?? "";
            List<PromoVoucher> vouchers = _promoRepo.GetAllVouchers(search);

            _dgvPromos.Rows.Clear();
            foreach (var v in vouchers)
            {
                int rowIndex = _dgvPromos.Rows.Add(
                    v.PromoId,
                    v.Code,
                    v.DisplayDiscount,
                    v.DiscountType,
                    v.MinOrderAmount > 0 ? $"₱{v.MinOrderAmount:N2}" : "No Min",
                    v.IsActive ? "ACTIVE" : "INACTIVE",
                    v.Description,
                    v.CreatedAt.ToString("yyyy-MM-dd hh:mm tt")
                );
                _dgvPromos.Rows[rowIndex].Tag = v;
            }

            if (_lblCount != null)
            {
                _lblCount.Text = $"{vouchers.Count} voucher(s)";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new AddEditPromoDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK && dlg.ResultVoucher != null)
                {
                    if (_promoRepo.InsertVoucher(dlg.ResultVoucher))
                    {
                        AuditService.Instance.Log("Add Promo Voucher", $"Created promo code '{dlg.ResultVoucher.Code}' ({dlg.ResultVoucher.DisplayDiscount})");
                        MessageBox.Show("Promo voucher created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPromoGrid();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save promo voucher. The code may already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_dgvPromos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a promo voucher to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var voucher = _dgvPromos.SelectedRows[0].Tag as PromoVoucher;
            if (voucher != null)
            {
                EditVoucher(voucher);
            }
        }

        private void EditVoucher(PromoVoucher voucher)
        {
            using (var dlg = new AddEditPromoDialog(voucher))
            {
                if (dlg.ShowDialog() == DialogResult.OK && dlg.ResultVoucher != null)
                {
                    if (_promoRepo.UpdateVoucher(dlg.ResultVoucher))
                    {
                        AuditService.Instance.Log("Edit Promo Voucher", $"Updated promo voucher '{dlg.ResultVoucher.Code}'");
                        MessageBox.Show("Promo voucher updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPromoGrid();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update promo voucher.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnToggle_Click(object sender, EventArgs e)
        {
            if (_dgvPromos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a promo voucher to toggle.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var voucher = _dgvPromos.SelectedRows[0].Tag as PromoVoucher;
            if (voucher != null)
            {
                ToggleVoucher(voucher);
            }
        }

        private void ToggleVoucher(PromoVoucher voucher)
        {
            bool newStatus = !voucher.IsActive;
            if (_promoRepo.ToggleActive(voucher.PromoId, newStatus))
            {
                AuditService.Instance.Log("Toggle Promo Status", $"Promo code '{voucher.Code}' set to {(newStatus ? "Active" : "Inactive")}");
                LoadPromoGrid();
            }
            else
            {
                MessageBox.Show("Failed to update voucher status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_dgvPromos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a promo voucher to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var voucher = _dgvPromos.SelectedRows[0].Tag as PromoVoucher;
            if (voucher != null)
            {
                DeleteVoucher(voucher);
            }
        }

        private void DeleteVoucher(PromoVoucher voucher)
        {
            if (MessageBox.Show($"Are you sure you want to delete promo voucher '{voucher.Code}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_promoRepo.DeleteVoucher(voucher.PromoId))
                {
                    AuditService.Instance.Log("Delete Promo Voucher", $"Deleted promo code '{voucher.Code}'");
                    MessageBox.Show("Promo voucher deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPromoGrid();
                }
                else
                {
                    MessageBox.Show("Failed to delete promo voucher.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
