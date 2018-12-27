using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample.LocalSettings.Windows
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Advexp.SettingsConfiguration.LogLevel = Advexp.LogLevel.Debug;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Settings.LoadSettings();

            Application.Run(new Form1());

            Settings.SaveSettings();
        }
    }
}
