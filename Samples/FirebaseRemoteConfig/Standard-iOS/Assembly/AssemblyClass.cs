using Advexp;
using System;
using Advexp.FirebaseRemoteConfig.Plugin;
using Advexp.DynamicSettings.Plugin;

namespace Sample.FirebaseRemoteConfig.Assembly.Standard
{
    public static class AssemblyClass
    {
        //------------------------------------------------------------------------------
        public static string GetCustomObjectValue()
        {
            if (Settings.CustomObject == null)
            {
                return "custom object - null";
            }

            return Settings.CustomObject.ToString();
        }

        //------------------------------------------------------------------------------
        public static string GetStringValue()
        {
            if (Settings.String == null)
            {
                return "string object - null";
            }

            return Settings.String;
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