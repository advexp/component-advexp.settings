using System;
using System.Collections.Generic;
using Advexp.CognitoSyncSettings.Plugin;
using Amazon.CognitoSync.SyncManager;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Widget;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using AndroidHUD;
using System.Threading;


[assembly: Permission (Name = Android.Manifest.Permission.Internet)]
[assembly: Permission (Name = Android.Manifest.Permission.WriteExternalStorage)]
[assembly: MetaData ("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: MetaData ("com.facebook.sdk.ApplicationName", Value = "@string/app_name")]

namespace Sample.CognitoSyncSettings.Android
{
	[Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity :  FragmentActivity, ICognitoSyncEventsListerner
    {
        static WeakReference s_TextView = new WeakReference(null);
		ICallbackManager m_CallbackManager;

        //------------------------------------------------------------------------------
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FacebookSdk.SdkInitialize(this.ApplicationContext);

            Context context = global::Android.App.Application.Context;
            PreferenceManager.SetDefaultValues(context, Resource.Xml.preferences, false);

            SetContentView(Resource.Layout.Main);

            s_TextView.Target = FindViewById(Resource.Id.textView);

            UpdatePropertiesText();

            ((MainApplication)Application).SetCognitoSyncListerner(this);

            m_CallbackManager = CallbackManagerFactory.Create();

            LoginManager.Instance.RegisterCallback(m_CallbackManager, new FacebookCallback<LoginResult>()
            {
                HandleSuccess = loginResult =>
                {
                    var accessToken = loginResult.AccessToken;
                    UpdateCredentials(accessToken.Token);
                    SynchronizeSettings();
                },
                HandleCancel = () =>
                {
                    UpdateCredentials(string.Empty);
                },
                HandleError = loginError =>
                {
                    UpdateCredentials(string.Empty);
                }
            });
            LoginManager.Instance.LogInWithReadPermissions(this, new List<string> { "public_profile" });

            Button changesPrefsButton = FindViewById<Button>(Resource.Id.changePrefsButton);
            changesPrefsButton.Click += delegate
            {
                StartActivity(typeof(CognitoSyncSettingsActivity));
            };

            Button synchronizeSettingsButton = FindViewById<Button>(Resource.Id.synchronize_settings);
            synchronizeSettingsButton.Click += delegate
            {
                SynchronizeSettings();
            };
        }

        //------------------------------------------------------------------------------
        void UpdateCredentials(string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                CognitoSyncSettingsConfiguration.Credentials.RemoveLogin(Constants.CognitoSyncProviderName);
            }
            else
            {
                CognitoSyncSettingsConfiguration.Credentials.Clear();
                CognitoSyncSettingsConfiguration.Credentials.AddLogin(Constants.CognitoSyncProviderName, token);
            }
        }

        //------------------------------------------------------------------------------
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            m_CallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }

        //------------------------------------------------------------------------------
        public void SynchronizeSettings()
        {
            AndHUD.Shared.Show(this, "Synchronizing dataset...");

            var plugin = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();
            plugin.SynchronizeDataset();
        }

        //------------------------------------------------------------------------------
        public static void UpdatePropertiesText()
        {
            CognitoSyncSettings.LoadSettings();

            TextView textView = (TextView)s_TextView.Target;

            String propertiesText = String.Format(
                "Bool value is '{0}'\n" +
                "TextPreference value is '{1}'\n" +
                "EnumPreference value is '{2}'", 
                CognitoSyncSettings.Boolean,
                CognitoSyncSettings.Text,
                CognitoSyncSettings.Enum
            );

            textView.Text = propertiesText;
        }

        //------------------------------------------------------------------------------
        public void OnSyncSuccess (object sender, SyncSuccessEventArgs e)
        {
            RunOnUiThread(() =>
            {
                AndHUD.Shared.Dismiss(this);
                UpdatePropertiesText();
            });
        }

        //------------------------------------------------------------------------------
        public void OnSyncFailure (object sender, SyncFailureEventArgs e)
        {
            RunOnUiThread(() =>
            {
                AndHUD.Shared.ShowErrorWithStatus(this, "Synchronization error: " + e.Exception.Message, MaskType.Black, TimeSpan.FromSeconds(5));
            });
        }
    }

	internal class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
	{
		public Action HandleCancel { get; set; }
		public Action<FacebookException> HandleError { get; set; }
		public Action<TResult> HandleSuccess { get; set; }

        //------------------------------------------------------------------------------
		public void OnCancel()
		{
			var c = HandleCancel;
			if (c != null)
				c();
		}

        //------------------------------------------------------------------------------
		public void OnError(FacebookException error)
		{
			var c = HandleError;
			if (c != null)
				c(error);
		}

        //------------------------------------------------------------------------------
		public void OnSuccess(Java.Lang.Object result)
		{
			var c = HandleSuccess;
			if (c != null)
				c(result.JavaCast<TResult>());
		}
	}
}