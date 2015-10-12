using System;
using System.Collections.Generic;
using Advexp;
using NUnit.Framework;

namespace TDD
{
    [MethodSerializer("ClassSerializerLoad", "ClassSerializerSave", "ClassSerializerDelete", "ClassSerializerSynchronize")]
    public class MethodSerializerSettings : Advexp.Settings<TDD.MethodSerializerSettings>
    {
        public Boolean m_SettingWasLoaded1 = false;
        public Boolean m_SettingWasSaved1 = false;
        public Boolean m_SettingWasDeleted1 = false;
        public Boolean m_SettingWasSynchronized1 = false;
        public List<String> m_arNames1 = new List<String>();

        public Boolean m_SettingWasLoaded2 = false;
        public Boolean m_SettingWasSaved2 = false;
        public Boolean m_SettingWasDeleted2 = false;
        public Boolean m_SettingWasSynchronized2 = false;
        public List<String> m_arNames2 = new List<String>();

        [Setting]
        public static Int32 Int32Value {get; set;}

        [Setting]
        [MethodSerializer("SettingSerializerLoad", "SettingSerializerSave", "SettingSerializerDelete", "SettingSerializerSynchronize")]
        public static String StringValue {get; set;}

        //------------------------------------------------------------------------------
        public object ClassSerializerLoad(string settingName, bool secure)
        {
            Assert.IsFalse(m_SettingWasLoaded1);
            m_SettingWasLoaded1 = true;

            m_arNames1.Add(settingName);

            return null;
        }

        //------------------------------------------------------------------------------
        public void ClassSerializerSave(string settingName, object value, bool secure)
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
        public object SettingSerializerLoad(string settingName, bool secure)
        {
            Assert.IsFalse(m_SettingWasLoaded2);
            m_SettingWasLoaded2 = true;

            m_arNames2.Add(settingName);

            return null;
        }

        //------------------------------------------------------------------------------
        public void SettingSerializerSave(string settingName, object value, bool secure)
        {
            Assert.IsFalse(m_SettingWasSaved2);
            m_SettingWasSaved2 = true;

            m_arNames2.Add(settingName);
        }

        //------------------------------------------------------------------------------
        public void SettingSerializerDelete(string settingName, bool secure)
        {
            Assert.IsFalse(m_SettingWasDeleted2);
            m_SettingWasDeleted2 = true;

            m_arNames2.Add(settingName);
        }

        //------------------------------------------------------------------------------
        public void SettingSerializerSynchronize()
        {
            Assert.IsFalse(m_SettingWasSynchronized2);
            m_SettingWasSynchronized2 = true;
        }
    }
}

