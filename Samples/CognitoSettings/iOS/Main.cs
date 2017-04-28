using UIKit;
using Advexp.CognitoSyncSettings.Plugin;
using Amazon.CognitoSync;
using Amazon.CognitoIdentity;

namespace Sample.CognitoSyncSettings.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Advexp.SettingsBaseConfiguration.RegisterSettingsPlugin<ICognitoSyncSettingsPlugin, CognitoSyncSettingsPlugin>();

            CognitoSyncSettingsConfiguration.Config = new AmazonCognitoSyncConfig()
            {
                RegionEndpoint = Constants.CognitoSyncRegion,
            };

            CognitoSyncSettingsConfiguration.Credentials = new CognitoAWSCredentials(Constants.CognitoSyncIdentityPoolId, 
                                                                                     Constants.CognitoSyncIdentityRegion);

            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
