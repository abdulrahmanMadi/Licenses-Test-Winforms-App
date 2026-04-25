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
            SdkBootstrap.OnApplicationStartup();
            Application.Run(new Form1());
        }
    }
}
