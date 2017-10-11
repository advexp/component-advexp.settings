using System;
using Advexp;

namespace InAppSettingsKitSample
{
    enum AutoLock
    {
        e_never = 0,
        e_1minute = 1,
        e_2minutes = 2,
        e_3minutes = 3,
        e_4minutes = 4,
        e_5minutes = 5,
    }

    enum HomeButtonDoubleClick
    {
        e_home = 0,
        e_search = 1,
        e_phoneFavorite = 2,
        e_camera = 3,
        e_iPod = 4,
    }

    class AdvexpSettings : Advexp.Settings<AdvexpSettings>
    {
        [Setting(Name = "airplaneMode")]
        public static Boolean AirplaneMode {get; set;}

        [Setting(Name = "notifications")]
        public static Boolean Notifications {get; set;}

        [Setting(Name = "brightness")]
        public static int Brightness {get; set;}

        [Setting(Name = "autoBrightness")]
        public static bool AutoBrightness {get; set;}

        // In this case, the automatic setting name in storage will be
        // "InAppSettingsKitSample.AdvexpSettings.Bluetooth"
        // The name pattern can be changed using the SettingsConfiguration.SettingsNamePattern property
        // The default pattern name is: "{NamespaceName}.{ClassName}.{FieldName}"
        [Setting]
        public static bool Bluetooth {get; set;}

        [Setting(Name = "locationServices")]
        public static bool LocationServices {get; set;}

        [Setting(Name = "autoLock")]
        public static AutoLock AutoLock {get; set;}

        [Setting(Name = "homeButtonDoubleClick")]
        public static HomeButtonDoubleClick HomeButtonDoubleClick {get; set;}

        static AdvexpSettings()
        {
            AirplaneMode = false;
            Notifications = false;

            Brightness = 50;
            AutoBrightness = false;

            Bluetooth = false;

            LocationServices = true;

            AutoLock = AutoLock.e_never;

            HomeButtonDoubleClick = HomeButtonDoubleClick.e_home;
        }
    }
}
