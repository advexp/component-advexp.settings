using System;
using System.Collections.Generic;
using Advexp.CognitoSyncSettings.Plugin;
using Advexp.DynamicSettings.Plugin;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync;
using Amazon.CognitoSync.SyncManager;

namespace TODOListPortableLibrary
{
    public class CognitoSyncUtils
    {
        public delegate void SyncResultAction(Exception exception);

        static SyncResultAction s_Action = null;

        //------------------------------------------------------------------------------
        public static void Initialize()
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
    
            // plugins are persistant per instance (static settings processed by separate, internal instance) 
            // and next call will return the same object
            var cognitoSyncPlugin = AdvexpTasks.GetPlugin<ICognitoSyncSettingsPlugin>();

            cognitoSyncPlugin.OnSyncSuccess += (object sender, SyncSuccessEventArgs e) =>
            {
                var action = s_Action;
                if (action != null)
                {
                    action(null);
                }
            };
            cognitoSyncPlugin.OnDatasetDeleted = delegate
            {
                //basically use what ever we got in remote
                return true;
            };
            cognitoSyncPlugin.OnSyncConflict = delegate (Dataset ds, List<SyncConflict> conflicts)
            {
                //trust the remote
                List<Record> resolved = new List<Record>();
                foreach (SyncConflict sc in conflicts)
                {
                    resolved.Add(sc.ResolveWithRemoteRecord());
                }
                ds.Resolve(resolved);
                return true;
            };
            cognitoSyncPlugin.OnDatasetMerged = delegate (Dataset dataset, List<string> datasetNames)
            {
                // returning true allows the Synchronize to continue and false stops it
                return true;
            };
            cognitoSyncPlugin.OnSyncFailure += (object sender, SyncFailureEventArgs e) =>
            {
                var action = s_Action;
                if (action != null)
                {
                    action(e.Exception);
                }
            };
        }

        //------------------------------------------------------------------------------
        public static void SetSyncAction(SyncResultAction action)
        {
            s_Action = action;
        }

        //------------------------------------------------------------------------------
        public static void UpdateCredentials(string token)
        {
            var cred = CognitoSyncSettingsConfiguration.Credentials;

            if (String.IsNullOrEmpty(token))
            {
                cred.RemoveLogin(Constants.CognitoSyncProviderName);
            }
            else
            {
                cred.AddLogin(Constants.CognitoSyncProviderName, token);
            }
        }

        //------------------------------------------------------------------------------
        public static void LoadTasks()
        {
            var cognitoSyncDynamicSettingsPlugin = GetCognitoSyncDynamicSettings();
            cognitoSyncDynamicSettingsPlugin.LoadSettings();
        }

        //------------------------------------------------------------------------------
        static IDynamicSettingsPlugin GetCognitoSyncDynamicSettings()
        {
            var cognitoSyncPlugin = AdvexpTasks.GetPlugin<ICognitoSyncSettingsPlugin>();
            var cognitoSyncDynamicSettingsPlugin = (IDynamicSettingsPlugin)cognitoSyncPlugin;

            return cognitoSyncDynamicSettingsPlugin;
        }

        //------------------------------------------------------------------------------
        public static void SaveTask(Task task)
        {
            var cognitoSyncDynamicSettingsPlugin = GetCognitoSyncDynamicSettings();

            cognitoSyncDynamicSettingsPlugin.SetSetting(task.Id, task);

            cognitoSyncDynamicSettingsPlugin.SaveSetting(task.Id);
        }

        //------------------------------------------------------------------------------
        public static void DeleteTask(string id)
        {
            var cognitoSyncDynamicSettingsPlugin = GetCognitoSyncDynamicSettings();

            cognitoSyncDynamicSettingsPlugin.DeleteSetting(id);
        }

        //------------------------------------------------------------------------------
        public static void SaveTask(string title, string description, bool completed)
        {
            SaveTask(new Task
            {
                Id = Guid.NewGuid().ToString(),
                Title = title,
                Completed = completed,
                Description = description
            });
        }

        //------------------------------------------------------------------------------
        public static List<Task> GetTasks()
        {
            var cognitoSyncDynamicSettingsPlugin = GetCognitoSyncDynamicSettings();

            List<Task> tasks = new List<Task>();
            foreach (var taskId in cognitoSyncDynamicSettingsPlugin)
            {
                var taskValue = cognitoSyncDynamicSettingsPlugin.GetSetting<Task>(taskId);
                tasks.Add(taskValue);
            }

            return tasks;
        }

        //------------------------------------------------------------------------------
        public static Task GetTask(string id)
        {
            var cognitoSyncDynamicSettingsPlugin = GetCognitoSyncDynamicSettings();

            Task task = null;

            if (cognitoSyncDynamicSettingsPlugin.Contains(id))
            {
                task = cognitoSyncDynamicSettingsPlugin.GetSetting<Task>(id);
            }

            return task;
        }

        //------------------------------------------------------------------------------
        public static async void Synchronize()
        {
            var cognitoSyncPlugin = AdvexpTasks.GetPlugin<ICognitoSyncSettingsPlugin>();
            await cognitoSyncPlugin.SynchronizeDataset();
        }
    }
}
