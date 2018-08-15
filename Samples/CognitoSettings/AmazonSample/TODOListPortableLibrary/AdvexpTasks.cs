using Advexp.CognitoSyncSettings.Plugin;

namespace TODOListPortableLibrary
{
    [CognitoSyncDatasetInfo(Name = "TASK_DATASET")]
    class AdvexpTasks : Advexp.Settings<AdvexpTasks>    
    {
        // tasks are dynamic settings
    }
}
