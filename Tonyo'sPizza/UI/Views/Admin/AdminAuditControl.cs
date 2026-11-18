using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Theme;

namespace Tonyo_sPizza.UI.Views.Admin
{
    public partial class AdminAuditControl : UserControl
    {
        private readonly AuditService _auditService;
        private bool _isLoadingFilters = false;

        public AdminAuditControl()
        {
            _auditService = AuditService.Instance;
            InitializeComponent();
            SetupCustomUIAndEvents();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                RefreshFilterDropdowns();
                LoadAuditLogs();
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

            _dtpFrom.Value = DateTime.Today.AddDays(-30);
            _dtpTo.Value = DateTime.Today.AddDays(1);

            _txtSearch.TextChanged += (s, e) => { if (!_isLoadingFilters) LoadAuditLogs(); };
            _cboUserFilter.SelectedIndexChanged += (s, e) => { if (!_isLoadingFilters) LoadAuditLogs(); };
            _cboActionFilter.SelectedIndexChanged += (s, e) => { if (!_isLoadingFilters) LoadAuditLogs(); };
            _dtpFrom.ValueChanged += (s, e) => { if (!_isLoadingFilters) LoadAuditLogs(); };
            _dtpTo.ValueChanged += (s, e) => { if (!_isLoadingFilters) LoadAuditLogs(); };
            _btnRefresh.Click += (s, e) => { RefreshFilterDropdowns(); LoadAuditLogs(); };
            _btnExport.Click += BtnExport_Click;

            UIHelper.StyleDataGridView(_dgvLogs);
            _dgvLogs.ReadOnly = true;
            _dgvLogs.EditMode = DataGridViewEditMode.EditProgrammatically;
            _dgvLogs.CellBeginEdit += (s, e) => { e.Cancel = true; };

            _dgvLogs.Columns.Clear();
            _dgvLogs.Columns.Add("LogId", "ID");
            _dgvLogs.Columns.Add("Timestamp", "Date & Time");
            _dgvLogs.Columns.Add("Username", "User");
            _dgvLogs.Columns.Add("Action", "Action");
            _dgvLogs.Columns.Add("TableName", "Table");
            _dgvLogs.Columns.Add("RecordId", "Record ID");
            _dgvLogs.Columns.Add("Details", "Details");

            UIHelper.ConfigureGridColumn(_dgvLogs, "LogId", 54);
            UIHelper.ConfigureGridColumn(_dgvLogs, "Timestamp", 142);
            UIHelper.ConfigureGridColumn(_dgvLogs, "Username", 92);
            UIHelper.ConfigureGridColumn(_dgvLogs, "Action", 132);
            UIHelper.ConfigureGridColumn(_dgvLogs, "TableName", 96);
            UIHelper.ConfigureGridColumn(_dgvLogs, "RecordId", 92);
            UIHelper.ConfigureGridColumn(_dgvLogs, "Details", 140, fill: true);

            foreach (DataGridViewColumn col in _dgvLogs.Columns)
            {
                col.ReadOnly = true;
            }

            UIHelper.AttachStatusPillPainter(_dgvLogs, UIHelper.ResolveSemanticStatusColors, "Action");
        }

        private void RefreshFilterDropdowns()
        {
            try
            {
                _isLoadingFilters = true;

                string prevUser = _cboUserFilter.SelectedItem?.ToString() ?? "All Users";
                _cboUserFilter.Items.Clear();
                _cboUserFilter.Items.Add("All Users");
                foreach (var u in _auditService.GetDistinctUsernames())
                {
                    _cboUserFilter.Items.Add(u);
                }
                int uIdx = _cboUserFilter.FindStringExact(prevUser);
                _cboUserFilter.SelectedIndex = uIdx >= 0 ? uIdx : 0;

                string prevAction = _cboActionFilter.SelectedItem?.ToString() ?? "All Actions";
                _cboActionFilter.Items.Clear();
                _cboActionFilter.Items.Add("All Actions");
                foreach (var a in _auditService.GetDistinctActions())
                {
                    _cboActionFilter.Items.Add(a);
                }
                int aIdx = _cboActionFilter.FindStringExact(prevAction);
                _cboActionFilter.SelectedIndex = aIdx >= 0 ? aIdx : 0;
            }
            finally
            {
                _isLoadingFilters = false;
            }
        }

        public void LoadAuditLogs()
        {
            try
            {
                AuthService.Instance.RequireRole("Admin");
                string query = _txtSearch?.Text?.Trim() ?? "";
                string userFilter = _cboUserFilter?.SelectedItem?.ToString() ?? "All Users";
                string actionFilter = _cboActionFilter?.SelectedItem?.ToString() ?? "All Actions";
                DateTime from = _dtpFrom.Value.Date;
                DateTime to = _dtpTo.Value.Date;

                List<AuditLog> logs = _auditService.SearchLogs(query, actionFilter, userFilter, from, to, 400);

                _dgvLogs.Rows.Clear();
                foreach (var log in logs)
                {
                    _dgvLogs.Rows.Add(
                        log.LogId,
                        log.Timestamp.ToString("yyyy-MM-dd hh:mm tt"),
                        log.Username,
                        log.Action,
                        log.TableName ?? "-",
                        log.RecordId ?? "-",
                        log.Details
                    );
                }

                if (_lblCount != null)
                {
                    _lblCount.Text = $"{logs.Count} event(s)";
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(this, ex.Message, "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Unable to load audit log events.", ex, "Audit Error", this);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (_dgvLogs.Rows.Count == 0)
            {
                MessageBox.Show(this, "No audit events currently visible to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Title = "Export Audit Trail to CSV";
                    sfd.Filter = "CSV File (*.csv)|*.csv";
                    sfd.FileName = $"TonyosPizza_AuditTrail_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("\"Log ID\",\"Date & Time\",\"Username\",\"Action\",\"Table\",\"Record ID\",\"Details\"");
                        foreach (DataGridViewRow r in _dgvLogs.Rows)
                        {
                            sb.AppendLine(string.Join(",",
                                EscapeCsv(r.Cells["LogId"].Value?.ToString()),
                                EscapeCsv(r.Cells["Timestamp"].Value?.ToString()),
                                EscapeCsv(r.Cells["Username"].Value?.ToString()),
                                EscapeCsv(r.Cells["Action"].Value?.ToString()),
                                EscapeCsv(r.Cells["TableName"].Value?.ToString()),
                                EscapeCsv(r.Cells["RecordId"].Value?.ToString()),
                                EscapeCsv(r.Cells["Details"].Value?.ToString())
                            ));
                        }
                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        AuditService.Instance.Log("Export Audit Log", $"Exported {_dgvLogs.Rows.Count} audit event(s) to CSV.", "AuditLogs");
                        MessageBox.Show(this, $"Audit log exported successfully to:\n{sfd.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.ShowFriendlyError("Failed to export audit log.", ex, "Export Error", this);
            }
        }

        private static string EscapeCsv(string val)
        {
            string s = val ?? "";
            return $"\"{s.Replace("\"", "\"\"")}\"";
        }
    }
}
