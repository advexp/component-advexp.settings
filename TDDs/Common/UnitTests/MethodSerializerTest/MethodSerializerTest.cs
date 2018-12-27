using System;
using Advexp;
using NUnit.Framework;

namespace TDD
{
    [TestFixture]
    public class MethodSerializerTest
    {
        //------------------------------------------------------------------------------
        [SetUp]
        public void Setup()
        {
        }

        //------------------------------------------------------------------------------
        public void CleanUpSettingsParams(MethodSerializerSettings settings)
        {
            settings.m_SettingWasLoaded1 = false;
            settings.m_SettingWasSaved1 = false;
            settings.m_SettingWasDeleted1 = false;
            settings.m_SettingWasSynchronized1 = false;
            settings.m_SettingWasContains1 = false;

            settings.m_arNames1.Clear();

            settings.m_SettingWasLoaded2 = false;
            settings.m_SettingWasSaved2 = false;
            settings.m_SettingWasDeleted2 = false;
            settings.m_SettingWasSynchronized2 = false;
            settings.m_SettingWasContains2 = false;

            settings.m_arNames2.Clear();
        }

        //------------------------------------------------------------------------------
        void Compare(Boolean actionFlag1, Boolean actionFlag2, 
                     MethodSerializerSettings settings)
        {
            const String settingName1 = "v2.TDD.MethodSerializerSettings.Int32Value";
            const String settingName2 = "v2.TDD.MethodSerializerSettings.StringValue";

            Assert.IsTrue(actionFlag1);
            Assert.IsTrue(actionFlag2);

            if (settings != null)
            {
                var settingNames1 = settings.m_arNames1;
                var settingNames2 = settings.m_arNames2;

                Assert.AreEqual(settingNames1.Count, 1);
                Assert.AreEqual(settingNames2.Count, 1);
                Assert.AreEqual(settingNames1[0], settingName1);
                Assert.AreEqual(settingNames2[0], settingName2);
            }
        }

        //------------------------------------------------------------------------------
        [Test]
        public void Test()
        {
            var tddHandler = new TDDHandler();

            var settings = MethodSerializerSettings.Instance;

            CleanUpSettingsParams(settings);
            MethodSerializerSettings.LoadSettings();
            Compare(settings.m_SettingWasLoaded1, settings.m_SettingWasLoaded2,
                    settings);
            Compare(!settings.m_SettingWasSynchronized1, !settings.m_SettingWasSynchronized2,
                    settings);

            CleanUpSettingsParams(settings);
            MethodSerializerSettings.SaveSettings();
            Compare(settings.m_SettingWasSaved1, settings.m_SettingWasSaved2,
                   settings);
            Compare(settings.m_SettingWasSynchronized1, settings.m_SettingWasSynchronized2,
                    settings);

            CleanUpSettingsParams(settings);
            MethodSerializerSettings.DeleteSettings();
            Compare(settings.m_SettingWasDeleted1, settings.m_SettingWasDeleted2,
                    settings);
            Compare(settings.m_SettingWasSynchronized1, settings.m_SettingWasSynchronized2,
                                settings);

            tddHandler.CheckErrors();
        }
    }
}

