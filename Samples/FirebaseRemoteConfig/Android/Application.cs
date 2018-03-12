using System;
using Android.App;
using Android.Runtime;

namespace Sample.FirebaseRemoteConfig.Android
{
    [Application]
    public class MainApplication : global::Android.App.Application
    {
        //------------------------------------------------------------------------------
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        //------------------------------------------------------------------------------
        public override void OnCreate()
        {
            base.OnCreate();

            Advexp.SettingsBaseConfiguration.LogLevel = Advexp.LogLevel.Debug;

            Firebase.RemoteConfig.FirebaseRemoteConfigSettings configSettings = 
                new Firebase.RemoteConfig.FirebaseRemoteConfigSettings.Builder().SetDeveloperModeEnabled(true).Build();
            Firebase.RemoteConfig.FirebaseRemoteConfig.Instance.SetConfigSettings(configSettings);

            // implementations below are only in the paid version

            // by default expiration duration is 43200 seconds (12 hours)
            //Advexp.FirebaseRemoteConfig.Plugin.
            //      FirebaseRemoteConfigConfiguration.ExpirationDuration = 60;

            Advexp.FirebaseRemoteConfig.Plugin.
                  FirebaseRemoteConfigConfiguration.RemoteConfigDefaultsId = Resource.Xml.remote_config_defaults;

            Advexp.
                SettingsBaseConfiguration.RegisterSettingsPlugin
                <
                    Advexp.FirebaseRemoteConfig.Plugin.IFirebaseRemoteConfigPlugin,
                    Advexp.FirebaseRemoteConfig.Plugin.FirebaseRemoteConfigPlugin
                >();
        }
    }
}

