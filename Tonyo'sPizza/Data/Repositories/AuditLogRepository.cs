using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Tonyo_sPizza.Models;
using Tonyo_sPizza.Services;

namespace Tonyo_sPizza.Data.Repositories
{
    public class AuditLogRepository
    {
        public void LogAction(string username, string action, string details, int? userId = null, string tableName = null, string recordId = null)
        {
            try
            {
                DatabaseHelper.EnsureSchemaUpgrades();
                string sql = @"INSERT INTO [dbo].[AuditLogs] ([UserId], [Username], [Action], [TableName], [RecordId], [Details], [Timestamp])
                               VALUES (@UserId, @Username, @Action, @TableName, @RecordId, @Details, GETDATE())";

                SqlParameter[] parameters = {
                    new SqlParameter("@UserId", userId.HasValue ? (object)userId.Value : DBNull.Value),
                    new SqlParameter("@Username", string.IsNullOrEmpty(username) ? "System" : username),
                    new SqlParameter("@Action", action ?? "Action"),
                    new SqlParameter("@TableName", string.IsNullOrEmpty(tableName) ? DBNull.Value : (object)tableName),
                    new SqlParameter("@RecordId", string.IsNullOrEmpty(recordId) ? DBNull.Value : (object)recordId),
                    new SqlParameter("@Details", details ?? "")
                };

                DatabaseHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, $"AuditLogRepository.LogAction ({action})");
            }
        }

        public List<AuditLog> GetAllLogs(int limit = 250)
        {
            List<AuditLog> logs = new List<AuditLog>();
            try
            {
                DatabaseHelper.EnsureSchemaUpgrades();
                string sql = $@"SELECT TOP ({limit}) * FROM [dbo].[AuditLogs] ORDER BY [Timestamp] DESC";
                DataTable dt = DatabaseHelper.ExecuteQuery(sql);
                foreach (DataRow row in dt.Rows)
                {
                    logs.Add(MapRowToAuditLog(row));
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AuditLogRepository.GetAllLogs");
            }
            return logs;
        }

        public List<AuditLog> SearchLogs(string query, string actionFilter = "All", string userFilter = "All", DateTime? fromDate = null, DateTime? toDate = null, int limit = 300)
        {
            List<AuditLog> logs = new List<AuditLog>();
            try
            {
                DatabaseHelper.EnsureSchemaUpgrades();
                string sql = $@"SELECT TOP ({limit}) * FROM [dbo].[AuditLogs] WHERE (1=1) ";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(actionFilter) && actionFilter != "All" && actionFilter != "All Actions")
                {
                    sql += " AND [Action] = @Action ";
                    parameters.Add(new SqlParameter("@Action", actionFilter));
                }

                if (!string.IsNullOrEmpty(userFilter) && userFilter != "All" && userFilter != "All Users")
                {
                    sql += " AND [Username] = @Username ";
                    parameters.Add(new SqlParameter("@Username", userFilter));
                }

                if (fromDate.HasValue)
                {
                    sql += " AND [Timestamp] >= @FromDate ";
                    parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime) { Value = fromDate.Value.Date });
                }

                if (toDate.HasValue)
                {
                    sql += " AND [Timestamp] <= @ToDate ";
                    parameters.Add(new SqlParameter("@ToDate", SqlDbType.DateTime) { Value = toDate.Value.Date.AddDays(1).AddTicks(-1) });
                }

                if (!string.IsNullOrEmpty(query))
                {
                    sql += " AND ([Username] LIKE @Query OR [Action] LIKE @Query OR [Details] LIKE @Query OR ISNULL([TableName],'') LIKE @Query OR ISNULL([RecordId],'') LIKE @Query) ";
                    parameters.Add(new SqlParameter("@Query", "%" + query.Trim() + "%"));
                }

                sql += " ORDER BY [Timestamp] DESC";

                DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters.ToArray());
                foreach (DataRow row in dt.Rows)
                {
                    logs.Add(MapRowToAuditLog(row));
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AuditLogRepository.SearchLogs");
            }
            return logs;
        }

        public List<string> GetDistinctUsernames()
        {
            var list = new List<string>();
            try
            {
                DatabaseHelper.EnsureSchemaUpgrades();
                DataTable dt = DatabaseHelper.ExecuteQuery("SELECT DISTINCT [Username] FROM [dbo].[AuditLogs] ORDER BY [Username] ASC");
                foreach (DataRow row in dt.Rows)
                {
                    string u = row["Username"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(u)) list.Add(u);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AuditLogRepository.GetDistinctUsernames");
            }
            return list;
        }

        public List<string> GetDistinctActions()
        {
            var list = new List<string>();
            try
            {
                DatabaseHelper.EnsureSchemaUpgrades();
                DataTable dt = DatabaseHelper.ExecuteQuery("SELECT DISTINCT [Action] FROM [dbo].[AuditLogs] ORDER BY [Action] ASC");
                foreach (DataRow row in dt.Rows)
                {
                    string a = row["Action"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(a)) list.Add(a);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AuditLogRepository.GetDistinctActions");
            }
            return list;
        }

        private AuditLog MapRowToAuditLog(DataRow row)
        {
            return new AuditLog
            {
                LogId = Convert.ToInt32(row["LogId"]),
                UserId = row["UserId"] != DBNull.Value ? (int?)Convert.ToInt32(row["UserId"]) : null,
                Username = row["Username"]?.ToString() ?? "Unknown",
                Action = row["Action"]?.ToString() ?? "-",
                TableName = row.Table.Columns.Contains("TableName") && row["TableName"] != DBNull.Value ? row["TableName"].ToString() : null,
                RecordId = row.Table.Columns.Contains("RecordId") && row["RecordId"] != DBNull.Value ? row["RecordId"].ToString() : null,
                Details = row["Details"]?.ToString() ?? "",
                Timestamp = Convert.ToDateTime(row["Timestamp"])
            };
        }
    }
}
