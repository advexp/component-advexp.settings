using Advexp;
using Advexp.JSONSettings.Plugin;

namespace TDD
{
    public static class OneTimeSetUpImpl
    {
        public static void SetUp()
        {
            SettingsBaseConfiguration.LogLevel = LogLevel.Info;

            SettingsBaseConfiguration.RegisterSettingsPlugin<IJSONSettingsPlugin, JSONSettingsPlugin>();
        }
    }
}