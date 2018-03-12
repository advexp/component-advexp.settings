using Advexp.CognitoSyncSettings.Plugin;

namespace TODOList.iOS
{
    [CognitoSyncDatasetInfo(Name = "DynamicSettings")]
    class DynamicSettings : Advexp.Settings<DynamicSettings>
    {
        // tasks are dynamic settings
    }
}
