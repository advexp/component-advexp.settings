using Android.App;
using Android.OS;
using Android.Preferences;

namespace Sample.CognitoSyncSettings.Android
{
    [Activity (Label = "CognitoPreferencesActivity")]
    public class CognitoSyncSettingsActivity : PreferenceActivity
    {
        //------------------------------------------------------------------------------
        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            var transaction = FragmentManager.BeginTransaction ();
            var preferences = new CognitoPreferenceFragment ();
            transaction.Replace (global::Android.Resource.Id.Content, preferences);
            transaction.Commit ();
        }

        //------------------------------------------------------------------------------
        public override void OnBackPressed ()
        {
            base.OnBackPressed ();

            MainActivity.UpdatePropertiesText ();
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        class CognitoPreferenceFragment 
            : PreferenceFragment
            , Preference.IOnPreferenceChangeListener
        {
            //------------------------------------------------------------------------------
            public override void OnCreate (Bundle savedInstanceState)
            {
                // IDynamicSettingsPlugin ds = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>() as IDynamicSettingsPlugin;
                // You can use CognitoSync dynamic settings
                // Just cast plugin interface to IDynamicSettingsPlugin

                base.OnCreate (savedInstanceState);

                AddPreferencesFromResource (Resource.Xml.preferences);

                var switchPreference = (SwitchPreference)FindPreference ("switch");
                var textPrference = (EditTextPreference)FindPreference ("text");
                var enumPreference = (ListPreference)FindPreference ("enum");

                switchPreference.OnPreferenceChangeListener = this;
                textPrference.OnPreferenceChangeListener = this;
                enumPreference.OnPreferenceChangeListener = this;

                CognitoSyncSettings.LoadSettings ();

                switchPreference.Checked = CognitoSyncSettings.Boolean;
                textPrference.Text = CognitoSyncSettings.Text;
                enumPreference.Value = CognitoSyncSettings.Enum.ToString ();
            }

            //------------------------------------------------------------------------------
            public bool OnPreferenceChange (Preference preference, Java.Lang.Object newValue)
            {
                switch (preference.Key) {
                case "switch":
                    CognitoSyncSettings.Boolean = (bool)newValue;
                    CognitoSyncSettings.SaveSetting (s => CognitoSyncSettings.Boolean);
                    break;

                case "text":
                    CognitoSyncSettings.Text = (string)newValue;
                    CognitoSyncSettings.SaveSetting (s => CognitoSyncSettings.Text);
                    break;

                case "enum":
                    CognitoSyncSettings.Enum = (EEnumValues)System.Enum.Parse (typeof (EEnumValues), (string)newValue);
                    CognitoSyncSettings.SaveSetting (s => CognitoSyncSettings.Enum);
                    break;
                }

                return true;
            }
        }
    }
}

