using System.Windows.Forms;
using LicenPro.SDK.AppHosting;

namespace Licenses_Test_Winforms_App
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            // Installs a staged update when this process is the installer (it then exits here), applies a
            // downloaded mandatory update, and reports the outcome after an update restart.
            SdkBootstrap.OnApplicationStartup(onRestarted: info => Form1.UpdateRestartInfo = info);
            Application.Run(new Form1());
        }
    }
}
