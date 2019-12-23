using System;
using System.Collections.Generic;

namespace Advexp
{
    class KeyChainSerializer
        : IInternalSettingsSerializer
    {
        KeyChainUtils m_keyChainUtils;

        //------------------------------------------------------------------------------
        public KeyChainSerializer()
        {
            m_keyChainUtils = new KeyChainUtils(() => Android.App.Application.Context, 
                                  SettingsConfiguration.KeyStoreFileProtectionPassword, 
                                  SettingsConfiguration.KeyStoreFileName, 
                                  SettingsBaseConfiguration.EncryptionServiceID);
        }

        #region IInternalSettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, SettingBaseAttribute attr, out object value)
        {
            if (!m_keyChainUtils.Contains(settingName))
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "KeyChainSerializer.Load - no such setting",
                                                         settingName);

                value = null;
                return false;
            }

            string stringValue;
            bool status = m_keyChainUtils.GetKey(settingName, out stringValue);

            if (!status || stringValue == null)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error,
                                                         "KeyChainSerializer.Load - error",
                                                         settingName);
                value = null;
                return false;
            }
            else
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "KeyChainSerializer.Load - success",
                                                         settingName);
            }

            // in this case simplified object will be corrected in the SettingsSerializer.cs
            value = stringValue;

            return true;
        }

        //------------------------------------------------------------------------------
        public void Save(string settingName, SettingBaseAttribute attr, object value)
        {
            var simpleValue = SettingsSerializerHelper.SimplifyObject(value);
            var stringValue = SettingsSerializerHelper.ConvertUsingTypeConverter<String>(simpleValue);

            bool status = m_keyChainUtils.SaveKey(settingName, stringValue);
            
            if (status)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "KeyChainSerializer.Save - success",
                                                         settingName);
            }
            else
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error,
                                                         "KeyChainSerializer.Save - error",
                                                         settingName);
            }
        }

        //------------------------------------------------------------------------------
        public bool Contains(string settingName, SettingBaseAttribute attr)
        {
            bool contains = m_keyChainUtils.Contains(settingName);

            if (contains)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "KeyChainSerializer.Contains - setting exists",
                                                         settingName);
            }
            else
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "KeyChainSerializer.Contains - no such setting",
                                                         settingName);
            }

            return contains;
        }

        //------------------------------------------------------------------------------
        public void Delete(string settingName, SettingBaseAttribute attr)
        {
            bool status = m_keyChainUtils.DeleteKey(settingName);
            if (status)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "KeyChainSerializer.Delete - success",
                                                         settingName);
            }
            else
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error,
                                                         "KeyChainSerializer.Delete - error",
                                                         settingName);
            }
        }

        //------------------------------------------------------------------------------
        public void Synchronize()
        {
        }

        #endregion
    }
}

