using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Content;
using Advexp.LocalDynamicSettings.Plugin;

namespace Sample.DynamicSettings.Android
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
                StartActivity(typeof(DynamicPreferencesActivity));
            };
        }

        //------------------------------------------------------------------------------
        public static void UpdatePropertiesText()
        {
            var lds = DynamicSettings.GetPlugin<ILocalDynamicSettingsPlugin>();
            lds.LoadSettings();

            TextView textView = (TextView)s_TextView.Target;

            String propertiesText = String.Format(
                "Checkbox value is '{0}'\n" +
                "Switch value is '{1}'\n" +
                "TextPreference value is '{2}'\n" +
                "EnumPreference value is '{3}'", 
                lds.GetSetting<string>(DynamicSettings.CheckBoxSettingName),
                lds.GetSetting<string>(DynamicSettings.SwitchSettingName),
                lds.GetSetting<string>(DynamicSettings.EditTextSettingName),
                lds.GetSetting<string>(DynamicSettings.EnumSettingName)
            );

            textView.Text = propertiesText;
        }
    }
}


