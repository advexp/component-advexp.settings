using System;
using Advexp;

namespace Sample.LocalSettings.Windows
{
    class Settings : Advexp.Settings<Settings>
    {
        [Setting(Name = "CheckBox", Default = true)]
        public static bool CheckBox { get; set; }

        [Setting(Name = "DateTimePicker", Default = "2018-04-01T13:45:30.0000000Z")]
        public static DateTime DateTimePicker { get; set; }

        [Setting(Name = "Text", Default = "default text from code")]
        public static String Text { get; set; }

        [Setting(Name = "Number", Default = 15)]
        public static Decimal Number { get; set; }

        [Setting(Name = "Password", Secure = true)]
        public static String Password { get; set; }
    }
}
