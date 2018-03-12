using System.Collections.Generic;

using Foundation;
using UIKit;
using Facebook.LoginKit;
using TODOListPortableLibrary;

namespace TODOList.iOS
{
    public class LoginViewController : UIViewController
    {
        List<string> readPermissions = new List<string> { "public_profile" };

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            LoginManager login = new LoginManager();
            login.LogInWithReadPermissions(readPermissions.ToArray(), this, delegate(LoginManagerLoginResult result, NSError error)
            {
                if (error != null)
                {
                    CognitoSyncUtils.UpdateCredentials(string.Empty);
                }
                else if (result.IsCancelled)
                {
                    CognitoSyncUtils.UpdateCredentials(string.Empty);
                }
                else
                {
                    var accessToken = result.Token;
                    CognitoSyncUtils.UpdateCredentials(accessToken.TokenString);
                    InvokeOnMainThread(() =>
                    {
                        ((AppDelegate)UIApplication.SharedApplication.Delegate).UpdateRootViewController(new TODOViewController());
                    });
                }
            });

        }
    }
}