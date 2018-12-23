using Advexp;
using Advexp.CognitoSyncSettings.Plugin;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync;
using Amazon.CognitoSync.SyncManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Sample.CognitoSyncSettings.Windows
{
    public static class ControlExtensions
    {
        /// <summary>
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="code"></param>
        public static void UIThread(this Control @this, Action code)
        {
            if (@this.InvokeRequired)
            {
                @this.BeginInvoke(code);
            }
            else
            {
                code.Invoke();
            }
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetupAdvexpSettings();

            CognitoSyncSettings.LoadSettings();

            Application.Run(new Form1());
        }

        static void SetupAdvexpSettings()
        {
            SettingsConfiguration.EncryptionServiceID = "Advexp.Settings.Sample";

            Advexp.SettingsBaseConfiguration.RegisterSettingsPlugin<ICognitoSyncSettingsPlugin, CognitoSyncSettingsPlugin>();
  
            Amazon.AWSConfigs.ApplicationName = Constants.FacebookAppName;

            CognitoSyncSettingsConfiguration.Config = new AmazonCognitoSyncConfig()
            {
                RegionEndpoint = Constants.CognitoSyncRegion,
            };

            CognitoSyncSettingsConfiguration.Credentials = new CognitoAWSCredentials(Constants.CognitoSyncIdentityPoolId,
                                                        Constants.CognitoSyncIdentityRegion);

            // plugins are persistant and next call will return the same object
            var plugin = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();

            plugin.OnSyncSuccess += (object sender, SyncSuccessEventArgs e) =>
            {
            };

            plugin.OnSyncFailure += (object sender, SyncFailureEventArgs e) =>
            {
            };

            plugin.OnDatasetDeleted = delegate (Dataset ds)
            {
                // Do clean up if necessary
                // returning true informs the corresponding dataset can be purged in the local storage and return false retains the local dataset
                return true;
            };

            plugin.OnDatasetMerged = delegate (Dataset dataset, List<string> datasetNames)
            {
                // returning true allows the Synchronize to continue and false stops it
                return true;
            };

            plugin.OnSyncConflict = delegate (Dataset dataset, List<SyncConflict> conflicts)
            {
                var resolvedRecords = new List<Amazon.CognitoSync.SyncManager.Record>();
                foreach (SyncConflict conflictRecord in conflicts)
                {
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

        }
    }
}
