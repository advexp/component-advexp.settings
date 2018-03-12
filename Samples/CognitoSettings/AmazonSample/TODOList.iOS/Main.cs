using Advexp;
using TODOListPortableLibrary;
using UIKit;

namespace TODOList.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            SettingsBaseConfiguration.LogLevel = LogLevel.Debug;

            CognitoSyncUtils.Initialize();

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}