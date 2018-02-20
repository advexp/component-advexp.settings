using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Content;
using Advexp.JSONSettings.Plugin;
using Advexp.LocalDynamicSettings.Plugin;

namespace Sample.JSONSettings.Android
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        static WeakReference s_TextView = new WeakReference(null);

        //------------------------------------------------------------------------------
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Context context = global::Android.App.Application.Context;
            PreferenceManager.SetDefaultValues(context, Resource.Xml.preferences, false);

            SetContentView(Resource.Layout.Main);

            s_TextView.Target = FindViewById(Resource.Id.textView);

            UpdatePropertiesText();

            Button button = FindViewById<Button>(Resource.Id.changePrefsButton);
            button.Click += delegate
            {
                StartActivity(typeof(LocalPreferencesActivity));
            };
        }

        //------------------------------------------------------------------------------
        public static void UpdatePropertiesText()
        {
            LocalSettings.LoadSettings();

            var plugin = LocalSettings.GetPlugin<IJSONSettingsPlugin>();

            TextView textView = (TextView)s_TextView.Target;

            String jsonText = plugin.SaveSettingsToJSON();
            textView.Text = jsonText;
        }
    }
}


