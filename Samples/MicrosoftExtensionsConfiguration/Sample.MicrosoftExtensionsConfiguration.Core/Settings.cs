using System;
using Advexp.MicrosoftExtensionsConfiguration.Plugin;

namespace Sample.MicrosoftExtensionsConfiguration.Core
{
    enum EnumValue
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

    class Person
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime DateTimeValue { get; set; }
    }

    class Settings : Advexp.Settings<Settings>
    {
        [MicrosoftExtensionsConfiguration(Name = "boolValue", Default = true)]
        public static bool BoolValue { get; set; }

        [MicrosoftExtensionsConfiguration(Name = "intValue", Default = 15)]
        public static int IntValue { get; set; }

        [MicrosoftExtensionsConfiguration(Name = "stringValue", Default = "default string from code")]
        public static string StringValue { get; set; }

        [MicrosoftExtensionsConfiguration(Name = "enumValueValue", Default = EnumValue.Four)]
        public static EnumValue EnumValue { get; set; }

        [MicrosoftExtensionsConfiguration(Name = "personValue", Default = null, BindTo=typeof(Person))]
        public static Person PersonValue { get; set; }

        [MicrosoftExtensionsConfiguration(Name = "mySection:dateTimeValue", Default = "2000-01-01T0:0:1.0000000Z")]
        public static DateTime DateTimeValue { get; set; }
    }
}
