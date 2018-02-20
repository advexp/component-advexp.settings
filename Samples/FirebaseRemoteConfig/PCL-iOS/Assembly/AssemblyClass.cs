using Advexp;
using System;
using Advexp.FirebaseRemoteConfig.Plugin;
using Advexp.DynamicSettings.Plugin;

namespace Sample.FirebaseRemoteConfig.Assembly.PCL
{
    public static class AssemblyClass
    {
        //------------------------------------------------------------------------------
        public static string GetValues()
        {
            var ds = Settings.GetPlugin<IFirebaseRemoteConfigPlugin>() as IDynamicSettingsPlugin;
            ds.LoadSettings();
            foreach(var dsi in ds)
            {
                var val = ds.GetSetting<string>(dsi);
                var lval = val.ToLower();
            }

            return Settings.CustomObject.ToString();
        }

        //------------------------------------------------------------------------------
        public static void Fetch()
        {
            var plugin = Settings.GetPlugin<IFirebaseRemoteConfigPlugin>();
            plugin.Fetch();
        }

        //------------------------------------------------------------------------------
        public static void SetCompletionHandler(FetchCompletionHandler handler)
        {
            var plugin = Settings.GetPlugin<IFirebaseRemoteConfigPlugin>();

            plugin.FetchCompletionHandler = handler;
        }

        //------------------------------------------------------------------------------
        public static void LoadSettings()
        {
            Settings.LoadSettings();
        }
    }
}