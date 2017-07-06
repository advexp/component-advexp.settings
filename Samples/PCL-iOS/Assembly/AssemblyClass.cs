using Advexp;
using Advexp.JSONSettings.Plugin;

namespace Sample.Assembly.PCL
{
    public static class AssemblyClass
    {
        //------------------------------------------------------------------------------
        public static void InitializeAndLoadSettings()
        {
            SettingsBaseConfiguration.SettingsNamePattern = "{ClassName}.{FieldName}";

            SettingsBaseConfiguration.RegisterSettingsPlugin<IJSONSettingsPlugin, JSONSettingsPlugin>();

            JSONSettingsConfiguration.JsonSerializerSettings.Formatting =
                Newtonsoft.Json.Formatting.Indented;

            JSONSettingsConfiguration.JsonSerializerSettings.Converters.
                                     Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            JSONSettingsConfiguration.PluginSettings.SkipSecureValues = false;

            AssemblyClass.LoadSettings();
        }

        //------------------------------------------------------------------------------
        public static string GetJSON()
        {
            return Settings.GetPlugin<IJSONSettingsPlugin>().Settings;
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