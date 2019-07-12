using Advexp.iCloudSettings.Plugin;

namespace Advexp.Sample.iCloud
{
    class Settings : Advexp.Settings<Settings>
    {
        [iCloudSetting(Name = "Bool", Default = false)]
        public static bool Bool { get; set; }

        [iCloudSetting(Name = "Text", Default = "default text from code")]
        public static string Text { get; set; }
    }
}
