using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Tonyo_sPizza.Services
{
    /// <summary>
    /// Centralized thread-safe error logger that records technical details to disk
    /// while allowing the UI to surface calm, non-technical messages to users.
    /// </summary>
    public static class ErrorLogger
    {
        private static readonly object _syncLock = new object();

        public static string GetLogDirectory()
        {
            try
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string dir = Path.Combine(localAppData, "TonyosPizza", "Logs");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return dir;
            }
            catch
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string GetLogFilePath()
        {
            return Path.Combine(GetLogDirectory(), "error.log");
        }

        public static void Log(Exception ex, string context = "")
        {
            if (ex == null) return;
            try
            {
                string username = "Anonymous";
                try
                {
                    var u = AuthService.Instance?.CurrentUser;
                    if (u != null) username = $"{u.Username} ({u.Role})";
                }
                catch { }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("--------------------------------------------------------------------------------");
                sb.AppendLine($"Timestamp : {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                sb.AppendLine($"User      : {username}");
                if (!string.IsNullOrWhiteSpace(context))
                    sb.AppendLine($"Context   : {context}");
                sb.AppendLine($"Exception : {ex.GetType().FullName}");
                sb.AppendLine($"Message   : {ex.Message}");
                if (ex.InnerException != null)
                {
                    sb.AppendLine($"Inner Ex  : {ex.InnerException.GetType().FullName}: {ex.InnerException.Message}");
                }
                sb.AppendLine("StackTrace:");
                sb.AppendLine(ex.StackTrace ?? "(none)");

                lock (_syncLock)
                {
                    File.AppendAllText(GetLogFilePath(), sb.ToString(), Encoding.UTF8);
                }
            }
            catch
            {
                // Never allow error logging itself to throw into caller flow
            }
        }

        public static void LogMessage(string context, string message)
        {
            try
            {
                string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{context}] {message}{Environment.NewLine}";
                lock (_syncLock)
                {
                    File.AppendAllText(GetLogFilePath(), entry, Encoding.UTF8);
                }
            }
            catch { }
        }

        public static void ShowFriendlyError(string userMessage, Exception ex = null, string title = "Notice", IWin32Window owner = null)
        {
            if (ex != null)
            {
                Log(ex, title);
            }

            string display = string.IsNullOrWhiteSpace(userMessage)
                ? "We ran into an unexpected issue while completing your request. Details have been recorded for support."
                : userMessage;

            if (owner != null)
                MessageBox.Show(owner, display, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show(display, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
