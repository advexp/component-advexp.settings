using Advexp;
using Advexp.JSONSettings.Plugin;

namespace Sample.Assembly.Standard
{
    public static class AssemblyClass
    {
        //------------------------------------------------------------------------------
        static AssemblyClass()
        {
            SettingsBaseConfiguration.RegisterSettingsPlugin<IJSONSettingsPlugin, JSONSettingsPlugin>();

            JSONSettingsConfiguration.JsonSerializerSettings.Formatting = 
                Newtonsoft.Json.Formatting.Indented;

            JSONSettingsConfiguration.JsonSerializerSettings.Converters.
                                     Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }

        //------------------------------------------------------------------------------
        public static string GetJSON()
        {
            return Settings.GetPlugin<IJSONSettingsPlugin>().SaveSettingsToJSON();
        }

        //------------------------------------------------------------------------------
        public static string  StringValue
        {
            get
            {
                return Settings.String;
            }
            set
            {
                Settings.String = value;
            }
        }

        //------------------------------------------------------------------------------
        public static bool BoolValue 
        {
            get
            {
                return Settings.Bool;
            }
            set
            {
                Settings.Bool = value;
            }
        }

        //------------------------------------------------------------------------------
        public static SampleEnum EnumValue
        {
            get
            {
                return Settings.Enum;
            }
            set
            {
                Settings.Enum = value;
            }
        }

        //------------------------------------------------------------------------------
        public static void LoadSettings()
        {
            Settings.LoadSettings();
        }

        //------------------------------------------------------------------------------
        public static void SaveSettings()
        {
            Settings.SaveSettings();
        }
    }
}