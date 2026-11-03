using System;
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

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK && AuthService.Instance.IsAuthenticated)
                {
                    Application.Run(new MainForm());
                }
            }
        }
    }
}
