using System;
using System.Collections.Generic;
using Advexp.CognitoSyncSettings.Plugin;
using Amazon.CognitoSync.SyncManager;
using Android.App;
using Android.Runtime;
using Amazon.CognitoSync;
using Amazon.CognitoIdentity;
using Advexp;

namespace Sample.CognitoSyncSettings.Android
{
    [Application]
    public class MainApplication : global::Android.App.Application
    {
        ICognitoSyncEventsListerner m_CognitoSyncListerner;

        //------------------------------------------------------------------------------
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        //------------------------------------------------------------------------------
        public override void OnCreate()
        {
            base.OnCreate();

            SettingsConfiguration.KeyStoreFileProtectionPassword = "password";
            SettingsConfiguration.KeyStoreFileName = "keystore";
            SettingsConfiguration.EncryptionServiceID = "Advexp.Settings.Sample";

            Advexp.SettingsBaseConfiguration.RegisterSettingsPlugin<ICognitoSyncSettingsPlugin, CognitoSyncSettingsPlugin>();

            CognitoSyncSettingsConfiguration.Config = new AmazonCognitoSyncConfig()
            {
                RegionEndpoint = Constants.CognitoSyncRegion,
            };

            CognitoSyncSettingsConfiguration.Credentials = new CognitoAWSCredentials(Constants.CognitoSyncIdentityPoolId, 
                                                        Constants.CognitoSyncIdentityRegion);

            // plugins are persistant and next call return the same object
            var plugin = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();

            plugin.OnSyncSuccess += (object sender, SyncSuccessEventArgs e) => 
            {
                ICognitoSyncEventsListerner listerner;
                //lock(m_CognitoSyncListerner)
                {
                    listerner = m_CognitoSyncListerner;
                }
                if (listerner != null)
                {
                    listerner.OnSyncSuccess(sender, e);
                }
            };

            plugin.OnSyncFailure += (object sender, SyncFailureEventArgs e) => 
            {
                ICognitoSyncEventsListerner listerner;
                //lock(m_CognitoSyncListerner)
                {
                    listerner = m_CognitoSyncListerner;
                }
                if (listerner != null)
                {
                    listerner.OnSyncFailure(sender, e);
                }
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
        }

        //------------------------------------------------------------------------------
        public void SetCognitoSyncListerner(ICognitoSyncEventsListerner listerner)
        {
            m_CognitoSyncListerner = listerner;
        }
    }
}

