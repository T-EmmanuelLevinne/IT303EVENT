using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
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
            UIHelper.EnableDoubleBuffering(this);

            if (lblSearchIcon != null)
            {
                lblSearchIcon.Paint += (s, e) =>
                {
                    UIHelper.SetHighQuality(e.Graphics);
                    UIHelper.DrawVectorIcon(e.Graphics, "search", new Rectangle(0, 0, 15, 15), AppColors.TextLight);
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
                _dgvPromos.Columns.Clear();
                _dgvPromos.Columns.Add("PromoId", "ID");
                _dgvPromos.Columns.Add("Code", "Voucher Code");
                _dgvPromos.Columns.Add("Discount", "Discount");
                _dgvPromos.Columns.Add("DiscountType", "Type");
                _dgvPromos.Columns.Add("MinSpend", "Min Spend");
                _dgvPromos.Columns.Add("Status", "Status");
                _dgvPromos.Columns.Add("Description", "Description");
                _dgvPromos.Columns.Add("CreatedAt", "Created");

                UIHelper.ConfigureGridColumn(_dgvPromos, "PromoId", 52);
                UIHelper.ConfigureGridColumn(_dgvPromos, "Code", 120);
                UIHelper.ConfigureGridColumn(_dgvPromos, "Discount", 112);
                UIHelper.ConfigureGridColumn(_dgvPromos, "DiscountType", 96);
                UIHelper.ConfigureGridColumn(_dgvPromos, "MinSpend", 100);
                UIHelper.ConfigureGridColumn(_dgvPromos, "Status", 98);
                UIHelper.ConfigureGridColumn(_dgvPromos, "Description", 120, fill: true);
                UIHelper.ConfigureGridColumn(_dgvPromos, "CreatedAt", 148);

                UIHelper.AttachStatusPillPainter(_dgvPromos, UIHelper.ResolveSemanticStatusColors, "Status");
                _dgvPromos.CellFormatting += DgvPromos_CellFormatting;
                _dgvPromos.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnEdit_Click(s, EventArgs.Empty); };
                _dgvPromos.CellMouseDown += DgvPromos_CellMouseDown;
            }
        }

        private void DgvPromos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string col = _dgvPromos.Columns[e.ColumnIndex].Name;

            if (col == "Code")
            {
                e.CellStyle.ForeColor = AppColors.PrimaryDark;
                e.CellStyle.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            }
            else if (col == "Discount")
            {
                e.CellStyle.ForeColor = AppColors.Primary;
                e.CellStyle.Font = new Font("Segoe UI Semibold", 9.25f, FontStyle.Bold);
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
                    menu.Items.Add(UIHelper.CreateModernMenuItem($"Edit '{voucher.Code}'", "edit", (s, args) => EditVoucher(voucher)));
                    menu.Items.Add(UIHelper.CreateModernMenuItem(
                        voucher.IsActive ? "Deactivate Voucher" : "Activate Voucher",
                        "check",
                        (s, args) => ToggleVoucher(voucher),
                        voucher.IsActive ? AppColors.StatusPending : AppColors.StatusCompleted));
                    menu.Items.Add(new ToolStripSeparator());
                    menu.Items.Add(UIHelper.CreateModernMenuItem($"Delete '{voucher.Code}'", "delete", (s, args) => DeleteVoucher(voucher), AppColors.StatusCancelled));

                    menu.Show(Cursor.Position);
                }
            }
        }

        public void LoadPromoGrid()
        {
            try
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
                        v.IsActive ? "Active" : "Inactive",
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
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Unable to load promo vouchers right now.", ex, "Promo Load Error", this);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dlg = new AddEditPromoDialog())
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultVoucher != null)
                    {
                        if (_promoRepo.InsertVoucher(dlg.ResultVoucher))
                        {
                            AuditService.Instance.Log("Insert", $"Created promo code '{dlg.ResultVoucher.Code}' ({dlg.ResultVoucher.DisplayDiscount}).", "PromoVouchers", dlg.ResultVoucher.PromoId.ToString());
                            MessageBox.Show(this, "Promo voucher created successfully.", "Voucher Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadPromoGrid();
                        }
                        else
                        {
                            MessageBox.Show(this, "Failed to save promo voucher. That code may already exist.", "Duplicate or Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Could not create promo voucher. Ensure the voucher code is unique.", ex, "Save Error", this);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_dgvPromos.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select a promo voucher to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var voucher = _dgvPromos.SelectedRows[0].Tag as PromoVoucher;
            if (voucher != null) EditVoucher(voucher);
        }

        private void EditVoucher(PromoVoucher voucher)
        {
            try
            {
                using (var dlg = new AddEditPromoDialog(voucher))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultVoucher != null)
                    {
                        if (_promoRepo.UpdateVoucher(dlg.ResultVoucher))
                        {
                            AuditService.Instance.Log("Update", $"Updated promo voucher '{dlg.ResultVoucher.Code}'.", "PromoVouchers", dlg.ResultVoucher.PromoId.ToString());
                            MessageBox.Show(this, "Promo voucher updated successfully.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadPromoGrid();
                        }
                        else
                        {
                            MessageBox.Show(this, "Failed to update promo voucher.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (DBConcurrencyException cex)
            {
                MessageBox.Show(this, cex.Message, "Concurrent Edit Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadPromoGrid();
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Could not update promo voucher.", ex, "Update Error", this);
            }
        }

        private void BtnToggle_Click(object sender, EventArgs e)
        {
            if (_dgvPromos.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select a promo voucher to toggle.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var voucher = _dgvPromos.SelectedRows[0].Tag as PromoVoucher;
            if (voucher != null) ToggleVoucher(voucher);
        }

        private void ToggleVoucher(PromoVoucher voucher)
        {
            try
            {
                bool newStatus = !voucher.IsActive;
                string verb = newStatus ? "Activate" : "Deactivate";
                if (MessageBox.Show(this, $"{verb} promo voucher '{voucher.Code}'?", $"Confirm {verb}", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                if (_promoRepo.ToggleActive(voucher.PromoId, newStatus))
                {
                    AuditService.Instance.Log("Update", $"Promo code '{voucher.Code}' set to {(newStatus ? "Active" : "Inactive")}.", "PromoVouchers", voucher.PromoId.ToString());
                    LoadPromoGrid();
                }
                else
                {
                    MessageBox.Show(this, "Failed to update voucher status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Failed to toggle voucher status.", ex, "Toggle Error", this);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_dgvPromos.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select a promo voucher to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var voucher = _dgvPromos.SelectedRows[0].Tag as PromoVoucher;
            if (voucher != null) DeleteVoucher(voucher);
        }

        private void DeleteVoucher(PromoVoucher voucher)
        {
            try
            {
                if (MessageBox.Show(this, $"Are you sure you want to permanently delete promo voucher '{voucher.Code}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (_promoRepo.DeleteVoucher(voucher.PromoId))
                    {
                        AuditService.Instance.Log("Delete", $"Deleted promo code '{voucher.Code}'.", "PromoVouchers", voucher.PromoId.ToString());
                        LoadPromoGrid();
                    }
                    else
                    {
                        MessageBox.Show(this, "Failed to delete promo voucher.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Could not delete this promo voucher.", ex, "Delete Error", this);
            }
        }
    }
}
