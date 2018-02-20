using System;
using Advexp;

namespace Sample.DynamicSettings.iOS
{
    enum AutoLock
    {
        Never = 0,
        OneMinute = 1,
        TwoMinutes = 2,
        ThreeMinutes = 3,
        FourMinutes = 4,
        FiveMinutes = 5,
    }

    enum HomeButtonDoubleClick
    {
        Home = 0,
        Search = 1,
        PhoneFavorite = 2,
        Camera = 3,
        iPod = 4,
    }

    class DynamicSettings : Advexp.Settings<DynamicSettings>
    {
        // all settings are dynamic, hold only names
        public const string AirplaneModeSettingName = "airplaneMode";
        public const string NotificationsSettingName = "notifications";
        public const string BrightnessSettingName = "brightness";
        public const string AutoBrightnessSettingName = "autoBrightness";
        public const string LoginSettingName = "login";
        public const string TimeSettingName = "time";
        public const string DateSettingName = "date";
        public const string BluetoothSettingName = "bluetooth";
        public const string LocationServicesSettingName = "locationServices";
        public const string AutoLockSettingName = "autoLock";
        public const string HomeButtonDoubleClickSettingName = "homeButtonDoubleClick";
    }
}
