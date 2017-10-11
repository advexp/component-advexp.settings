using System;
using Advexp;

namespace Sample.LocalSettings.iOS
{
    enum AutoLock
    {
        Never = 0,
        OneMinute = 1,
        TwoMinutes = 2,
        ThreeMinutes = 3,
        FourMminutes = 4,
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

    class LocalSettings : Advexp.Settings<LocalSettings>
    {
        [Setting(Name = "airplaneMode", Default = false)]
        public static Boolean AirplaneMode { get; set; }

        [Setting(Name = "notifications", Default = false)]
        public static Boolean Notifications { get; set; }

        [Setting(Name = "brightness", Default = 50)]
        public static int Brightness { get; set; }

        [Setting(Name = "autoBrightness", Default = false)]
        public static bool AutoBrightness { get; set; }

        [Setting(Name = "login", Default = "")]
        public static String Login { get; set; }

        // "Secure = true" mean, that setting value will be saved to the keychain
        [Setting(Name = "password", Secure = true, Default = "")]
        public static String Password { get; set; }

        [Setting (Name = "time", Default = "13:45:30.0000000Z")]
        public static DateTime Time { get; set; }

        [Setting (Name = "date", Default = "2009-06-15T13:45:30.0000000Z")]
        public static DateTime Date { get; set; }

		// In this case, the automatic setting name in storage will be
		// "Sample.LocalSettings.iOS.LocalSettings.Bluetooth"
		// The name pattern can be changed using the SettingsConfiguration.SettingsNamePattern property
		// The default pattern name is: "{NamespaceName}.{ClassName}.{FieldName}"
		[Setting (Default = false)]
        public static bool Bluetooth { get; set; }

        [Setting (Name = "locationServices", Default = true)]
        public static bool LocationServices { get; set; }

        [Setting (Name = "autoLock", Default = AutoLock.Never)]
        public static AutoLock AutoLock { get; set; }

        [Setting (Name = "homeButtonDoubleClick", Default = HomeButtonDoubleClick.Home)]
        public static HomeButtonDoubleClick HomeButtonDoubleClick { get; set; }
    }
}
