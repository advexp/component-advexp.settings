using Amazon.CognitoSync.SyncManager;

namespace Sample.CognitoSyncSettings.Android
{
    public interface ICognitoSyncEventsListerner
    {
        void OnSyncSuccess(object sender, SyncSuccessEventArgs e);
        void OnSyncFailure(object sender, SyncFailureEventArgs e);
    }
}

