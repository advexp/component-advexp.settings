using UIKit;
using Amazon.CognitoSync;
using Amazon.CognitoIdentity;

namespace Sample.CognitoSyncSettings.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Advexp.
                SettingsBaseConfiguration.RegisterSettingsPlugin
                <
                    Advexp.CognitoSyncSettings.Plugin.ICognitoSyncSettingsPlugin, 
                    Advexp.CognitoSyncSettings.Plugin.CognitoSyncSettingsPlugin
                >();
    
            Advexp.CognitoSyncSettings.Plugin.
                CognitoSyncSettingsConfiguration.Config = 
                new AmazonCognitoSyncConfig()
                {
                    RegionEndpoint = Constants.CognitoSyncRegion,
                };

            Advexp.CognitoSyncSettings.Plugin.
                CognitoSyncSettingsConfiguration.Credentials = 
                new CognitoAWSCredentials
                    (Constants.CognitoSyncIdentityPoolId, 
                     Constants.CognitoSyncIdentityRegion);

            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
