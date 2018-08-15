using System;
using Advexp.CognitoSyncSettings.Plugin;

namespace Sample.CognitoSyncSettings.iOS
{
    enum EEnumValues
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
    }

    [CognitoSyncDatasetInfo(Name = "CognitoSyncSettings")]
    class CognitoSyncSettings : Advexp.Settings<CognitoSyncSettings>
    {
        [CognitoSyncSetting(Name = "CognitoSyncSettings.Boolean")]
        public static Boolean Boolean;
        [CognitoSyncSetting(Name = "CognitoSyncSettings.Text")]
        public static String Text;
        [CognitoSyncSetting(Name = "CognitoSyncSettings.Enum")]
        public static EEnumValues Enum;
    }
}
