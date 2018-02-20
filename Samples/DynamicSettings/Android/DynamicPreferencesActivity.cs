using Android.App;
using Android.OS;
using Android.Preferences;

namespace Sample.DynamicSettings.Android
{
    [Activity(Label = "DynamicPreferencesActivity")]
    public class DynamicPreferencesActivity : PreferenceActivity
    {
        //------------------------------------------------------------------------------
        protected override void OnCreate(Bundle savedInstanceState) 
        {
            base.OnCreate(savedInstanceState);

            var transaction = FragmentManager.BeginTransaction();
            var preferences = new DynamicPreferenceFragment();
            transaction.Replace(global::Android.Resource.Id.Content, preferences);
            transaction.Commit();
        }

        //------------------------------------------------------------------------------
        public override void OnBackPressed()
        {
            base.OnBackPressed();

            MainActivity.UpdatePropertiesText();
        }

        //------------------------------------------------------------------------------
        class DynamicPreferenceFragment : PreferenceFragment
        {
            //------------------------------------------------------------------------------
            public override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                AddPreferencesFromResource(Resource.Xml.preferences);
            }
        }
    }
}

