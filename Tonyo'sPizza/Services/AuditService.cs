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

        public void Log(string action, string details)
        {
            var user = AuthService.Instance?.CurrentUser;
            string username = user != null ? user.Username : "System";
            int? userId = user != null ? (int?)user.UserId : null;
            _repo.LogAction(username, action, details, userId);
        }

        public List<AuditLog> GetAllLogs(int limit = 200)
        {
            return _repo.GetAllLogs(limit);
        }

        public List<AuditLog> SearchLogs(string query, string actionFilter = "All", int limit = 200)
        {
            return _repo.SearchLogs(query, actionFilter, limit);
        }
    }
}
