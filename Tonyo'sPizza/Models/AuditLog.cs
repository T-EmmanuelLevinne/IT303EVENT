using System;

namespace Tonyo_sPizza.Models
{
    public class AuditLog
    {
        public int LogId { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }

        public AuditLog()
        {
            Timestamp = DateTime.Now;
        }

        public AuditLog(string username, string action, string details, int? userId = null)
        {
            Username = username;
            Action = action;
            Details = details;
            UserId = userId;
            Timestamp = DateTime.Now;
        }
    }
}
