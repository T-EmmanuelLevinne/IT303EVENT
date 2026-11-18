using System;
using System.Threading;
using System.Windows.Forms;
using Tonyo_sPizza.Services;
using Tonyo_sPizza.UI.Forms;

namespace Tonyo_sPizza
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Global non-crashing error handling routed to central error log
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += OnApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK && AuthService.Instance.IsAuthenticated)
                {
                    Application.Run(new MainForm());
                }
            }
        }

        private static void OnApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ErrorLogger.Log(e.Exception, "Application.ThreadException");
            ErrorLogger.ShowFriendlyError(
                "Something went wrong while processing that action. Technical details have been recorded to the error log so you can safely continue working.",
                null,
                "Unexpected Error"
            );
        }

        private static void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                ErrorLogger.Log(ex, "AppDomain.UnhandledException");
            }
        }
    }
}
