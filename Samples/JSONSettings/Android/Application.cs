using System;
using Android.App;
using Android.Runtime;
using Advexp.JSONSettings.Plugin;
using Advexp;

namespace Sample.JSONSettings.Android
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

            SettingsConfiguration.KeyStoreFileProtectionPassword = "password";
            SettingsConfiguration.KeyStoreFileName = "keystore";
            SettingsConfiguration.EncryptionServiceID = "Advexp.Settings.Sample";

            Advexp.SettingsBaseConfiguration.RegisterSettingsPlugin<IJSONSettingsPlugin, JSONSettingsPlugin>();

            JSONSettingsConfiguration.JsonSerializerSettings.Formatting = 
                Newtonsoft.Json.Formatting.Indented;
            JSONSettingsConfiguration.JsonSerializerSettings.Converters.
                                     Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }
    }
}

