using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Controls;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminAuditControl : UserControl
    {
        private readonly AuditService _auditService;

        public AdminAuditControl()
        {
            _auditService = AuditService.Instance;
            InitializeComponent();
            SetupCustomUIAndEvents();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                LoadAuditLogs();
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

            if (_txtSearch != null) _txtSearch.TextChanged += (s, e) => LoadAuditLogs();
            if (_cboActionFilter != null) _cboActionFilter.SelectedIndexChanged += (s, e) => LoadAuditLogs();
            if (_btnRefresh != null) _btnRefresh.Click += (s, e) => LoadAuditLogs();

            if (_dgvLogs != null)
            {
                UIHelper.StyleDataGridView(_dgvLogs);
                _dgvLogs.ReadOnly = true;
                _dgvLogs.EditMode = DataGridViewEditMode.EditProgrammatically;
                _dgvLogs.AllowUserToAddRows = false;
                _dgvLogs.AllowUserToDeleteRows = false;
                _dgvLogs.CellBeginEdit += (s, e) => { e.Cancel = true; };

                if (_dgvLogs.Columns.Count == 0)
                {
                    _dgvLogs.Columns.Add("LogId", "ID");
                    _dgvLogs.Columns.Add("Timestamp", "Date & Time");
                    _dgvLogs.Columns.Add("Username", "User");
                    _dgvLogs.Columns.Add("Action", "Action");
                    _dgvLogs.Columns.Add("Details", "Details");

                    _dgvLogs.Columns["LogId"].Width = 60;
                    _dgvLogs.Columns["Timestamp"].Width = 160;
                    _dgvLogs.Columns["Username"].Width = 120;
                    _dgvLogs.Columns["Action"].Width = 180;
                    _dgvLogs.Columns["Details"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    foreach (DataGridViewColumn col in _dgvLogs.Columns)
                    {
                        col.ReadOnly = true;
                    }
                }

                _dgvLogs.CellFormatting += DgvLogs_CellFormatting;
            }
        }

        private void DgvLogs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string col = _dgvLogs.Columns[e.ColumnIndex].Name;

            if (col == "Action")
            {
                string act = e.Value.ToString();
                if (act.Contains("Delete") || act.Contains("Cancel"))
                {
                    e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                    e.CellStyle.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
                }
                else if (act.Contains("Create") || act.Contains("Add"))
                {
                    e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129);
                    e.CellStyle.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
                }
                else if (act.Contains("Archive"))
                {
                    e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6);
                    e.CellStyle.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = AppColors.PrimaryDark;
                    e.CellStyle.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
                }
            }
        }

        public void LoadAuditLogs()
        {
            string query = _txtSearch?.Text?.Trim() ?? "";
            string filter = _cboActionFilter?.SelectedItem?.ToString() ?? "All Actions";
            if (filter == "All Actions") filter = "All";

            List<AuditLog> logs = _auditService.SearchLogs(query, filter);

            _dgvLogs.Rows.Clear();
            foreach (var log in logs)
            {
                _dgvLogs.Rows.Add(
                    log.LogId,
                    log.Timestamp.ToString("yyyy-MM-dd hh:mm:ss tt"),
                    log.Username,
                    log.Action,
                    log.Details
                );
            }

            if (_lblCount != null)
            {
                _lblCount.Text = $"Showing {logs.Count} event(s)";
            }
        }
    }
}
