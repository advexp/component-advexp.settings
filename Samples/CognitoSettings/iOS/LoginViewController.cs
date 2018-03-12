using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Facebook.LoginKit;
using Advexp.CognitoSyncSettings.Plugin;

namespace Sample.CognitoSyncSettings.iOS
{
    public class LoginViewController : UIViewController
    {
        List<string> readPermissions = new List<string> { "public_profile" };

        //------------------------------------------------------------------------------
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            LoginManager login = new LoginManager();
            login.LoginBehavior = LoginBehavior.Web;
            login.LogOut();
            login.LogInWithReadPermissions(readPermissions.ToArray(), null, delegate(LoginManagerLoginResult result, NSError error)
            {
                if (error != null)
                {
                    UpdateCredentials(string.Empty);
                }
                else if (result.IsCancelled)
                {
                    UpdateCredentials(string.Empty);

                    ((AppDelegate)UIApplication.SharedApplication.Delegate).Continue(new SettingsViewController
                    { 
                        AddLoginButton = true,
                        AddSyncInProgressLabel = false,
                        EnableSettingsReload = false,
                        AddSyncDatasetButton = false,
                    });
                }
                else
                {
                    var accessToken = result.Token;
                    UpdateCredentials(accessToken.TokenString);

                    var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                    appDelegate.SynchronizeSettings();

                    appDelegate.Continue(new SettingsViewController
                    {
                        AddLoginButton = false,
                        AddLogoutButton = true,
                        AddSyncInProgressLabel = true,
                        EnableSettingsReload = true,
                        AddSyncDatasetButton = true,
                    });

                    // plugins are persistant and next call return the same object
                    var plugin = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();
                    plugin.SynchronizeDataset();
                }
            });
        }

        //------------------------------------------------------------------------------
        void UpdateCredentials(string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                CognitoSyncSettingsConfiguration.Credentials.Clear();
            }
            else
            {
                CognitoSyncSettingsConfiguration.Credentials.Clear();
                CognitoSyncSettingsConfiguration.Credentials.AddLogin(Constants.CognitoSyncProviderName, token);
            }
        }
    }
}