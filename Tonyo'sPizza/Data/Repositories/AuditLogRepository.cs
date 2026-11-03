using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Data.Repositories
{
    public class AuditLogRepository
    {
        public void LogAction(string username, string action, string details, int? userId = null)
        {
            try
            {
                DatabaseHelper.EnsureSchemaUpgrades();
                string sql = @"INSERT INTO [dbo].[AuditLogs] ([UserId], [Username], [Action], [Details], [Timestamp])
                               VALUES (@UserId, @Username, @Action, @Details, GETDATE())";

                SqlParameter[] parameters = {
                    new SqlParameter("@UserId", userId.HasValue ? (object)userId.Value : DBNull.Value),
                    new SqlParameter("@Username", string.IsNullOrEmpty(username) ? "System" : username),
                    new SqlParameter("@Action", action ?? "Action"),
                    new SqlParameter("@Details", details ?? "")
                };

                DatabaseHelper.ExecuteNonQuery(sql, parameters);
            }
            catch
            {
               
            }
        }

        public List<AuditLog> GetAllLogs(int limit = 200)
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
            catch
            {
                // Fallback empty
            }
            return logs;
        }

        public List<AuditLog> SearchLogs(string query, string actionFilter = "All", int limit = 200)
        {
            List<AuditLog> logs = new List<AuditLog>();
            try
            {
                DatabaseHelper.EnsureSchemaUpgrades();
                string sql = $@"SELECT TOP ({limit}) * FROM [dbo].[AuditLogs] 
                                WHERE (1=1) ";

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(actionFilter) && actionFilter != "All")
                {
                    sql += " AND [Action] = @Action ";
                    parameters.Add(new SqlParameter("@Action", actionFilter));
                }

                if (!string.IsNullOrEmpty(query))
                {
                    sql += " AND ([Username] LIKE @Query OR [Action] LIKE @Query OR [Details] LIKE @Query) ";
                    parameters.Add(new SqlParameter("@Query", "%" + query.Trim() + "%"));
                }

                sql += " ORDER BY [Timestamp] DESC";

                DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters.ToArray());
                foreach (DataRow row in dt.Rows)
                {
                    logs.Add(MapRowToAuditLog(row));
                }
            }
            catch
            {
                // Fallback empty
            }
            return logs;
        }

        private AuditLog MapRowToAuditLog(DataRow row)
        {
            return new AuditLog
            {
                LogId = Convert.ToInt32(row["LogId"]),
                UserId = row["UserId"] != DBNull.Value ? (int?)Convert.ToInt32(row["UserId"]) : null,
                Username = row["Username"]?.ToString() ?? "Unknown",
                Action = row["Action"]?.ToString() ?? "-",
                Details = row["Details"]?.ToString() ?? "",
                Timestamp = row["Timestamp"] != DBNull.Value ? Convert.ToDateTime(row["Timestamp"]) : DateTime.Now
            };
        }
    }
}
