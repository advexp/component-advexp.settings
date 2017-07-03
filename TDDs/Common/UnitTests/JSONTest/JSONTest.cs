using NUnit.Framework;
using Advexp.JSONSettings.Plugin;
using Advexp;

namespace TDD
{
    [TestFixture]
    public class JSONTest
    {
        //------------------------------------------------------------------------------
        [TestFixtureSetUp]
        public void Setup()
        {
            SettingsConfiguration.EnableFormatMigration = false;
        }

        //------------------------------------------------------------------------------
        [Test]
        public void Test()
        {
            var refSettings = new DifferentTypesLocalSettings();

            var refSettingsPlugin = refSettings.GetObjectPlugin<IJSONSettingsPlugin>();

            var refJsonSettings = refSettingsPlugin.Settings;

            var loadedSettings = new DifferentTypesLocalSettings();

            var loadedSettingsPlugin = loadedSettings.GetObjectPlugin<IJSONSettingsPlugin>();
            loadedSettingsPlugin.Settings = refJsonSettings;

            DifferentTypesTest.CompareSettings(loadedSettings, refSettings);
        }

        //------------------------------------------------------------------------------
        [Test]
        public void SecureTest()
        {
            JSONSettingsConfiguration.PluginSettings.SkipSecureValues = false;

            var refSettings = new DifferentTypesSecureSettings();

            var refSettingsPlugin = refSettings.GetObjectPlugin<IJSONSettingsPlugin>();
            var refJsonSettings = refSettingsPlugin.Settings;

            var loadedSettings = new DifferentTypesSecureSettings();

            var loadedSettingsPlugin = loadedSettings.GetObjectPlugin<IJSONSettingsPlugin>();
            loadedSettingsPlugin.Settings = refJsonSettings;

            DifferentTypesTest.CompareSettings(loadedSettings, refSettings);
        }
    }
}

