using System;
using Advexp;

namespace Sample.JSONSettings.Android
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

    class LocalSettings : Advexp.Settings<LocalSettings>
    {
        [Setting(Name = "checkbox")]
        public static Boolean Checkbox {get; set;}

        [Setting(Name = "switch")]
        public static Boolean Switch {get; set;}

        [Setting(Name = "edittext", Default = "default text")]
        public static String TextPreference{get; set;}

        [Setting(Name = "enum")]
        public static EnumPreference EnumPreference{get; set;}
    }
}

