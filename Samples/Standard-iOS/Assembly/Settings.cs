using Advexp;

namespace Sample.Assembly.Standard
{
    public enum SampleEnum
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

    class Settings : Advexp.Settings<Settings>
    {
        [Setting]
        public static bool Bool {get; set;}

        [Setting]
        public static string String {get; set;}

        [Setting]
        public static SampleEnum Enum {get; set;}
    }
}
