using UIKit;
using Advexp.iCloudSettings.Plugin;

namespace Sample.iCloudSettings.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {

//#error Change bundle identifier in info.plist file.
//#error Update iCloud container name in Entitlements.plist file (same name for Mac and iOS samples).
//#error Apple Developer Program membership is requred for this sample.

            Advexp.SettingsBaseConfiguration.RegisterSettingsPlugin<IiCloudSettingsPlugin, iCloudSettingsPlugin>();

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
