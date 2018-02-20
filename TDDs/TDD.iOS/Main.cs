using UIKit;
using Advexp;
using Advexp.JSONSettings.Plugin;

namespace TDD
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            SettingsBaseConfiguration.LogLevel = LogLevel.Error;

            SettingsBaseConfiguration.RegisterSettingsPlugin<IJSONSettingsPlugin, JSONSettingsPlugin>();

            // if you want to use a different Application Delegate class from "UnitTestAppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "UnitTestAppDelegate");
        }
    }
}
