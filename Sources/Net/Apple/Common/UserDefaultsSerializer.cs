using System;
using System.Collections.Generic;
using Foundation;

namespace Advexp
{
    class InternalUserDefaultsSerializer
        : IInternalSettingsSerializer, IDynamicSettingsInfo
    {
        NSUserDefaults m_userDefaults = null;

        //------------------------------------------------------------------------------
        NSUserDefaults GetUserDefaults()
        {
            NSUserDefaults userDefaults = null;

            if (m_userDefaults != null)
            {
                userDefaults = m_userDefaults;
            }
            else
            {
                userDefaults = NSUserDefaults.StandardUserDefaults;
            }

            Debug.Assert(userDefaults != null);

            return userDefaults;
        }

        //------------------------------------------------------------------------------
        public void SetUserDefaults(NSUserDefaults userDefaults)
        {
            m_userDefaults = userDefaults;
        }

        #region IInternalSettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, SettingBaseAttribute attr, out object value)
        {
            var prefs = GetUserDefaults();
            var nsValue = prefs[settingName];

            if (nsValue == null)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "UserDefaultsSerializer.Load - no such setting",
                                                         settingName);

                value = null;
                return false;
            }

            // in this case simplified object will be corrected in the SettingsSerializer.cs
            value = nsValue.ToUnderlyingObject();

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                     "UserDefaultsSerializer.Load - success",
                                                     settingName);

            return true;
        }

        //------------------------------------------------------------------------------
        public void Save(string settingName, SettingBaseAttribute attr, object value)
        {
            var simpleObj = SettingsSerializerHelper.SimplifyObject(value);

            var prefs = GetUserDefaults();
            var nsValue = simpleObj.ToNSObject();

            prefs[settingName] = nsValue;

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                     "UserDefaultsSerializer.Save - success",
                                                     String.Format("Setting name is {0}; Value is {1}",
                                                                   settingName,
                                                                   nsValue != null ? nsValue.ToString() : "null"));
        }

        //------------------------------------------------------------------------------
        public void Delete(string settingName, SettingBaseAttribute attr)
        {
            var prefs = GetUserDefaults();

            prefs.RemoveObject(settingName);

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                     "UserDefaultsSerializer.Delete - success",
                                                     settingName);
        }

        //------------------------------------------------------------------------------
        public bool Contains(string settingName, SettingBaseAttribute attr)
        {
            var prefs = GetUserDefaults();

            bool contains = prefs[settingName] != null;

            return contains;
        }

        //------------------------------------------------------------------------------
        public void Synchronize()
        {
            var prefs = GetUserDefaults();

            prefs.Synchronize();
        }

        #endregion

        #region IDynamicSettingsInfo implementation

        //------------------------------------------------------------------------------
        public IEnumerable<string> GetDynamicSettingsNames()
        {
            var prefs = GetUserDefaults();

            List<string> dynamicSettingsNames = new List<string>();

            foreach(var key in prefs.ToDictionary().Keys)
            {
                var strKey = key.ToString();
                if (SettingNameFormatInfo.GetSettingNameMode(strKey) == SettingNameMode.Dynamic)
                {
                    dynamicSettingsNames.Add(strKey);
                }
            }

            return dynamicSettingsNames;
        }

        #endregion

    }

    //------------------------------------------------------------------------------
    public class UserDefaultsSerializer
        : ISettingsSerializer, IDynamicSettingsInfo
    {
        InternalUserDefaultsSerializer m_serializer = new InternalUserDefaultsSerializer();

        //------------------------------------------------------------------------------
        public void SetUserDefaults(NSUserDefaults userDefaults)
        {
            m_serializer.SetUserDefaults(userDefaults);
        }

        #region ISettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, bool secure, SettingBaseAttribute attr, out object value)
        {
            if (secure)
            {
                throw new ArgumentException(
                    String.Format("UserDefaultsSerializer.Load: " +
                                  "Secure=true canot be applied to the '{0}' setting", settingName));
            }

            return m_serializer.Load(settingName, attr, out value);
        }

        //------------------------------------------------------------------------------
        public void Save(string settingName, bool secure, SettingBaseAttribute attr, object value)
        {
            if (secure)
            {
                throw new ArgumentException(
                    String.Format("UserDefaultsSerializer.Save: " +
                                  "Secure=true canot be applied to the '{0}' setting", settingName));
            }

            m_serializer.Save(settingName, attr, value);
        }

        //------------------------------------------------------------------------------
        public void Delete(string settingName, bool secure, SettingBaseAttribute attr)
        {
            if (secure)
            {
                throw new ArgumentException(
                    String.Format("UserDefaultsSerializer.Delete: " +
                                  "Secure=true canot be applied to the '{0}' setting", settingName));
            }

            m_serializer.Delete(settingName, attr);
        }

        //------------------------------------------------------------------------------
        public bool Contains(string settingName, bool secure, SettingBaseAttribute attr)
        {
            if (secure)
            {
                throw new ArgumentException(
                    String.Format("UserDefaultsSerializer.Contains: " +
                                  "Secure=true canot be applied to the '{0}' setting", settingName));
            }

            return m_serializer.Contains(settingName, attr);
        }

        //------------------------------------------------------------------------------
        public void Synchronize()
        {
            m_serializer.Synchronize();
        }

        #endregion

        #region IDynamicSettingsInfo implementation

        //------------------------------------------------------------------------------
        public IEnumerable<string> GetDynamicSettingsNames()
        {
            return m_serializer.GetDynamicSettingsNames();
        }

        #endregion
    }
}

