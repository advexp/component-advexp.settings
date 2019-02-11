using System;
using Advexp;

namespace Sample.LocalSettings.Mac
{
    class Settings : Advexp.Settings<Settings>
    {
        [Setting(Name = "CheckBox", Default = false)]
        public static bool CheckBox { get; set; }

        [Setting(Name = "DateTime", Default = "2018-04-01T13:45:30.0000000Z")]
        public static DateTime DateTime { get; set; }

        [Setting(Name = "Text", Default = "default text from code")]
        public static String Text { get; set; }
    }
}
