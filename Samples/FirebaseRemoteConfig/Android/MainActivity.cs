using Advexp.FirebaseRemoteConfig.Plugin;
using Android.App;
using Android.OS;
using Android.Widget;

namespace Sample.FirebaseRemoteConfig.Android
{
    [Activity(Label = "Sample.FirebaseRemoteConfig", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        //------------------------------------------------------------------------------
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            UpdateUi();

            var plugin = Settings.GetPlugin<IFirebaseRemoteConfigPlugin>();
            plugin.FetchCompletionHandler = (status) =>
            {
                switch (status)
                {
                    case FetchStatus.Success:
                        UpdateUi();
                        break;
                    case FetchStatus.Throttled:
                        break;
                    case FetchStatus.NoFetchYet:
                        break;
                    case FetchStatus.Failure:
                        break;
                }

                UpdateStatusText(status);
            };

            Button fetchButton = FindViewById<Button>(Resource.Id.fetchButton);
            fetchButton.Click += delegate
            {
                TextView statusTextView = FindViewById<TextView>(Resource.Id.status);
                statusTextView.Text = "Fetching...";

                plugin.Fetch(); 
            };
        }

        //------------------------------------------------------------------------------
        void UpdateUi()
        {
            RunOnUiThread(() =>
            {
                Settings.LoadSettings();

                TextView text = FindViewById<TextView>(Resource.Id.text);
                text.Text = Settings.String;

                CheckBox checkBox = FindViewById<CheckBox>(Resource.Id.checkBox);
                checkBox.Checked = Settings.Boolean;

                SeekBar seekBar = FindViewById<SeekBar>(Resource.Id.seekBar);
                seekBar.Progress = Settings.SliderValue;

                TextView date = FindViewById<TextView>(Resource.Id.date);
                date.Text = Settings.DateTime.ToString();

                TextView customObject = FindViewById<TextView>(Resource.Id.customObject);
                customObject.Text =
                    Settings.CustomObject == null ?
                    "null" :
                    Settings.CustomObject.ToString();
            });
        }

        //------------------------------------------------------------------------------
        void UpdateStatusText(FetchStatus status)
        {
            RunOnUiThread(() =>
            {
                TextView statusTextView = FindViewById<TextView>(Resource.Id.status);

                switch (status)
                {
                    case FetchStatus.Success:
                        statusTextView.Text = "Status - Success";
                        break;
                    case FetchStatus.Throttled:
                        statusTextView.Text = "Status - Throttled";
                        break;
                    case FetchStatus.NoFetchYet:
                        statusTextView.Text = "Status - NoFetchYet";
                        break;
                    case FetchStatus.Failure:
                        statusTextView.Text = "Status - Failure";
                        break;
                    default:
                        statusTextView.Text = "Status - Unknown status code";
                        break;
                }
            });
        }
    }
}

