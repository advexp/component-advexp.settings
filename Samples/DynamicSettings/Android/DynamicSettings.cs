using System;
using Advexp;

namespace Sample.DynamicSettings.Android
{
    enum EnumPreference
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
    }

    class DynamicSettings : Advexp.Settings<DynamicSettings>
    {
        // all settings are dynamic, hold only names
        public const string CheckBoxSettingName = "checkbox";
        public const string SwitchSettingName = "switch";
        public const string EditTextSettingName = "edittext";
        public const string EnumSettingName = "enum";
    }
}

