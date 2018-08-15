using Foundation;
using UIKit;
using Advexp.CognitoSyncSettings.Plugin;
using Amazon.CognitoSync.SyncManager;
using System.Collections.Generic;
using Facebook.CoreKit;

namespace Sample.CognitoSyncSettings.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        UINavigationController m_NavigationController;
        UIViewController m_ViewController;

        //------------------------------------------------------------------------------
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Profile.EnableUpdatesOnAccessTokenChange (true);
            Settings.AppID = Constants.FacebookAppId;
            Settings.DisplayName = Constants.FacebookAppName;

            CognitoSyncSettings.LoadSettings();

            // plugins are persistant per instance (static settings processed by separate, internal instance) 
            // and next call will return the same object
            var plugin = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();

            plugin.OnSyncSuccess += (sender, e) => 
            {
                var el = m_ViewController as ICognitoSyncEventsListerner;
                InvokeOnMainThread(() => 
                {
                    if (el != null)
                    {
                        el.OnSyncSuccess(sender, e);
                    }
                });
            };

            plugin.OnSyncFailure += (sender, e) => 
            {
                var el = m_ViewController as ICognitoSyncEventsListerner;
                InvokeOnMainThread(() => 
                {
                    if (el != null)
                    {
                        el.OnSyncFailure(sender, e);
                    }
                });
            };

            plugin.OnDatasetDeleted = delegate(Dataset ds)
            {
                // Do clean up if necessary
                // returning true informs the corresponding dataset can be purged in the local storage and return false retains the local dataset
                return true;
            };

            plugin.OnDatasetMerged = delegate(Dataset dataset, List<string> datasetNames)
            {
                // returning true allows the Synchronize to continue and false stops it
                return true;
            };

            plugin.OnSyncConflict = delegate(Dataset dataset, List<SyncConflict> conflicts)
            {
                var resolvedRecords = new List<Amazon.CognitoSync.SyncManager.Record>();
                foreach(SyncConflict conflictRecord in conflicts) {
                    // SyncManager provides the following default conflict resolution methods:
                    //      ResolveWithRemoteRecord - overwrites the local with remote records
                    //      ResolveWithLocalRecord - overwrites the remote with local records
                    //      ResolveWithValue - to implement your own logic
                    resolvedRecords.Add(conflictRecord.ResolveWithRemoteRecord());
                }
                // resolves the conflicts in local storage
                dataset.Resolve(resolvedRecords);

                // on return true the synchronize operation continues where it left,
                // returning false cancels the synchronize operation
                return true;
            };

            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            Continue(new SettingsViewController(){ AddLoginButton = true });
            Window.MakeKeyAndVisible();

            // This method verifies if you have been logged into the app before, and keep you logged in after you reopen or kill your app.
            return ApplicationDelegate.SharedInstance.FinishedLaunching (application, launchOptions);
        }

        //------------------------------------------------------------------------------
        public void SynchronizeSettings()
        {
            
        }

        //------------------------------------------------------------------------------
        public void Continue(UIViewController vc)
        {
            if (vc != null)
            {
                InvokeOnMainThread(() => 
                {
                    m_ViewController = vc;
                    m_NavigationController = new UINavigationController(m_ViewController);
                    Window.RootViewController = m_NavigationController;
                });
            }
        }

        //------------------------------------------------------------------------------
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            // We need to handle URLs by passing them to their own OpenUrl in order to make the SSO authentication works.
            return ApplicationDelegate.SharedInstance.OpenUrl (application, url, sourceApplication, annotation);
        }

    }
}


