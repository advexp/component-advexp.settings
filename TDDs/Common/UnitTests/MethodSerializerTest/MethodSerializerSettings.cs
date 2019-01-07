using System;
using System.Collections.Generic;
using Advexp;
using NUnit.Framework;

namespace TDD
{
    [Advexp.Preserve(AllMembers = true)]
    [MethodSerializer("ClassSerializerLoad", "ClassSerializerSave", "ClassSerializerDelete", "ClassSerializerSynchronize", "ClassSerializerContains")]
    public class MethodSerializerSettings : Advexp.Settings<TDD.MethodSerializerSettings>
    {
        public Boolean m_SettingWasLoaded1 = false;
        public Boolean m_SettingWasSaved1 = false;
        public Boolean m_SettingWasDeleted1 = false;
        public Boolean m_SettingWasSynchronized1 = false;
        public Boolean m_SettingWasContains1 = false;
        public List<String> m_arNames1 = new List<String>();

        public Boolean m_SettingWasLoaded2 = false;
        public Boolean m_SettingWasSaved2 = false;
        public Boolean m_SettingWasDeleted2 = false;
        public Boolean m_SettingWasSynchronized2 = false;
        public Boolean m_SettingWasContains2 = false;
        public List<String> m_arNames2 = new List<String>();

        [Setting]
        public static Int32 Int32Value {get; set;}

        [Setting]
        [MethodSerializer("SettingsSerializerLoad", "SettingsSerializerSave", "SettingsSerializerDelete", "SettingsSerializerSynchronize", "SettingsSetializerContains")]
        public static String StringValue {get; set;}

        //------------------------------------------------------------------------------
        public bool ClassSerializerLoad(string settingName, bool secure, out object value)
        {
            Assert.IsFalse(m_SettingWasLoaded1);
            m_SettingWasLoaded1 = true;

            m_arNames1.Add(settingName);

            value = null;

            return true;
        }

        //------------------------------------------------------------------------------
        public void ClassSerializerSave(string settingName, bool secure, object value)
        {
            Assert.IsFalse(m_SettingWasSaved1);
            m_SettingWasSaved1 = true;

            m_arNames1.Add(settingName);
        }

        //------------------------------------------------------------------------------
        public void ClassSerializerDelete(string settingName, bool secure)
        {
            Assert.IsFalse(m_SettingWasDeleted1);
            m_SettingWasDeleted1 = true;

            m_arNames1.Add(settingName);
        }

        //------------------------------------------------------------------------------
        public void ClassSerializerSynchronize()
        {
            Assert.IsFalse(m_SettingWasSynchronized1);
            m_SettingWasSynchronized1 = true;
        }

        //------------------------------------------------------------------------------
        public bool ClassSerializerContains(string settingName)
        {
            return true;
        }

        //------------------------------------------------------------------------------
        public bool SettingsSerializerLoad(string settingName, bool secure, out object value)
        {
            Assert.IsFalse(m_SettingWasLoaded2);
            m_SettingWasLoaded2 = true;

            m_arNames2.Add(settingName);

            value = null;

            return true;
        }

        //------------------------------------------------------------------------------
        public void SettingsSerializerSave(string settingName, bool secure, object value)
        {
            Assert.IsFalse(m_SettingWasSaved2);
            m_SettingWasSaved2 = true;

            m_arNames2.Add(settingName);
        }

        //------------------------------------------------------------------------------
        public void SettingsSerializerDelete(string settingName, bool secure)
        {
            Assert.IsFalse(m_SettingWasDeleted2);
            m_SettingWasDeleted2 = true;

            m_arNames2.Add(settingName);
        }

        //------------------------------------------------------------------------------
        public void SettingsSerializerSynchronize()
        {
            Assert.IsFalse(m_SettingWasSynchronized2);
            m_SettingWasSynchronized2 = true;
        }

        //------------------------------------------------------------------------------
        public bool SettingsSetializerContains(string settingName)
        {
            return true;
        }
    }
}

