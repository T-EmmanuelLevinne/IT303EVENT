using System;
using System.Collections.Generic;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Services
{
    public class AuditService
    {
        private static AuditService _instance;
        public static AuditService Instance => _instance ?? (_instance = new AuditService());

        private readonly AuditLogRepository _repo;

        private AuditService()
        {
            _repo = new AuditLogRepository();
        }

        public void Log(string action, string details, string tableName = null, string recordId = null)
        {
            var user = AuthService.Instance?.CurrentUser;
            string username = user != null ? user.Username : "System";
            int? userId = (user != null && user.UserId > 0) ? (int?)user.UserId : null;
            _repo.LogAction(username, action, details, userId, tableName, recordId);
        }

        public void LogForUser(string username, int? userId, string action, string details, string tableName = null, string recordId = null)
        {
            _repo.LogAction(username, action, details, userId, tableName, recordId);
        }

        public List<AuditLog> GetAllLogs(int limit = 250)
        {
            return _repo.GetAllLogs(limit);
        }

        public List<AuditLog> SearchLogs(string query, string actionFilter = "All", string userFilter = "All", DateTime? fromDate = null, DateTime? toDate = null, int limit = 300)
        {
            return _repo.SearchLogs(query, actionFilter, userFilter, fromDate, toDate, limit);
        }

        public List<string> GetDistinctUsernames() => _repo.GetDistinctUsernames();
        public List<string> GetDistinctActions() => _repo.GetDistinctActions();
    }
}
