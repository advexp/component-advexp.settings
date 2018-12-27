using NUnit.Framework;
using Advexp.JSONSettings.Plugin;
using Advexp.LocalDynamicSettings.Plugin;
using Advexp;
using System;

namespace TDD
{
    [TestFixture]
    public class JSONTest
    {
        //------------------------------------------------------------------------------
        static JSONTest()
        {
            //SettingsBaseConfiguration.RegisterSettingsPlugin<IJSONSettingsPlugin, JSONSettingsPlugin>();
        }

        //------------------------------------------------------------------------------
        [Test]
        public void Test()
        {
            var tddHandler = new TDDHandler();

            var refSettings = new DifferentTypesLocalSettings();

            var refSettingsPlugin = refSettings.GetObjectPlugin<IJSONSettingsPlugin>();
            var refSettingsDSPlugin = refSettings.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            for (int i = 0, iCount = 10; i < iCount; i++)
            {
                var settingName = String.Format("item{0}", i);
                refSettingsDSPlugin.SetSetting(settingName, i);
            }

            var refJsonSettings = refSettingsPlugin.SaveSettingsToJSON();

            var loadedSettings = new DifferentTypesLocalSettings();

            var loadedSettingsPlugin = loadedSettings.GetObjectPlugin<IJSONSettingsPlugin>();
            loadedSettingsPlugin.LoadSettingsFromJSON(refJsonSettings);

            DifferentTypesTest.CompareSettings(loadedSettings, refSettings);
            CompareDynamicSettings(loadedSettings, refSettings);

            tddHandler.CheckErrors();
        }

        //------------------------------------------------------------------------------
        void CompareDynamicSettings(DifferentTypesLocalSettings loadedSettings, DifferentTypesLocalSettings refSettings)
        {
            var refSettingsDSPlugin = refSettings.GetObjectPlugin<ILocalDynamicSettingsPlugin>();
            var loadedSettingsDSPlugin = loadedSettings.GetObjectPlugin<ILocalDynamicSettingsPlugin>();

            int whileIndex = 0;

            using (var refEnumerator = refSettingsDSPlugin.GetEnumerator())
            using (var loadedEnumerator = loadedSettingsDSPlugin.GetEnumerator())
            {
                while (refEnumerator.MoveNext() && loadedEnumerator.MoveNext())
                {
                    // check names
                    Assert.AreEqual(refEnumerator.Current, loadedEnumerator.Current);

                    var refDynamicValue = refSettingsDSPlugin.GetSetting<Int32>(refEnumerator.Current);
                    var loadedDynamicValue = loadedSettingsDSPlugin.GetSetting<Int32>(loadedEnumerator.Current);

                    // check values
                    Assert.AreEqual(refDynamicValue, loadedDynamicValue);

                    whileIndex++;
                }
            }

            // check settings count
            Assert.AreEqual(refSettingsDSPlugin.Count, whileIndex);
            Assert.AreEqual(loadedSettingsDSPlugin.Count, whileIndex);
        }
    }
}
