using UIKit;
using Firebase.RemoteConfig;
using Advexp;

namespace Sample.FirebaseRemoteConfig.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            SettingsBaseConfiguration.LogLevel = LogLevel.Debug;

            // Use Firebase library to configure APIs
            Firebase.Core.App.Configure();

            // Enabling developer mode, allows for frequent refreshes of the cache
            RemoteConfig.SharedInstance.ConfigSettings = new RemoteConfigSettings(true);

            Advexp.SettingsBaseConfiguration.LogLevel = Advexp.LogLevel.Debug;

            // implementations below are only in the paid version

            // by default expiration duration is 43200 seconds (12 hours)
            //Advexp.FirebaseRemoteConfig.Plugin.
            //      FirebaseRemoteConfigConfiguration.ExpirationDuration = 60;

            //Advexp.FirebaseRemoteConfig.Plugin.
            //      FirebaseRemoteConfigConfiguration.RemoteConfigDefaultsPlistFileName = 
            //      "RemoteConfigDefaults";

            Advexp.
                SettingsBaseConfiguration.RegisterSettingsPlugin
                <
                    Advexp.FirebaseRemoteConfig.Plugin.IFirebaseRemoteConfigPlugin,
                    Advexp.FirebaseRemoteConfig.Plugin.FirebaseRemoteConfigPlugin
                >();

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
