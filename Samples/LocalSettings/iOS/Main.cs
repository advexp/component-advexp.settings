using UIKit;

namespace Sample.LocalSettings.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Advexp.SettingsBaseConfiguration.LogLevel = Advexp.LogLevel.Info;

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
