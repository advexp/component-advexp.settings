using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;

namespace Advexp
{
    class InternalIsolatedStorageSerializer
        : IInternalSettingsSerializer, IDynamicSettingsInfo
    {
        IsolatedStorageFile m_Store = IsolatedStorageFile.GetUserStoreForDomain();

        //------------------------------------------------------------------------------
        public void SetIsolatedStorage(IsolatedStorageFile store)
        {
            m_Store = store;
        }

        #region IInternalSettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, SettingBaseAttribute attr, out object value)
        {
            value = null;

            // If the key exists, retrieve the value.
            if (m_Store.FileExists(settingName))
            {
                using (var stream = m_Store.OpenFile(settingName, FileMode.Open))
                {
                    using (var sr = new StreamReader(stream))
                    {
                        value = sr.ReadToEnd();
                    }
                }
            }
            else
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "IsolatedStorageSerializer.Load - no such setting",
                                                         settingName);

                return false;
            }

            if (value == null)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Warning,
                                                         "IsolatedStorageSerializer.Load - setting in storage is null",
                                                         settingName);

                return false;
            }

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                         "IsolatedStorageSerializer.Load - success",
                                         settingName);

            return true;
        }

        //------------------------------------------------------------------------------
        public void Save(string settingName, SettingBaseAttribute attr, object value)
        {
            var simpleValue = SettingsSerializerHelper.SimplifyObject(value);
            var stringValue = SettingsSerializerHelper.ConvertUsingTypeConverter<String>(simpleValue);

            using (var stream = m_Store.OpenFile(settingName, FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(stream))
                {
                    sw.Write(stringValue);
                }
            }

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                         "IsolatedStorageSerializer.Save - success",
                                         String.Format("setting is {0}, value is {1}",
                                                       settingName,
                                                       stringValue != null ? stringValue : "null"));
        }

        //------------------------------------------------------------------------------
        public void Delete(string settingName, SettingBaseAttribute attr)
        {
            if (m_Store.FileExists(settingName))
            {
                m_Store.DeleteFile(settingName);
            }

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                     "IsolatedStorageSerializer.Delete - success",
                                                     String.Format("setting is {0}", settingName));
        }

        //------------------------------------------------------------------------------
        public bool Contains(string settingName, SettingBaseAttribute attr)
        {
            bool contains = m_Store.FileExists(settingName);

            return contains;
        }

        //------------------------------------------------------------------------------
        public void Synchronize()
        {
        }

        #endregion

        #region IDynamicSettingsInfo implementation

        //------------------------------------------------------------------------------
        public IEnumerable<string> GetDynamicSettingsNames()
        {
            var delimeter = SettingNameFormatInfo.GetSettingNameDelimeter(this as ISettingsSerializerWishes);
            var dynamicSettingsNames = m_Store.GetFileNames(SettingNameFormatInfo.DynamicSettingNamePrefix + delimeter + "*");

            return dynamicSettingsNames;
        }

        #endregion
    }

    //------------------------------------------------------------------------------
    public class IsolatedStorageSerializer
        : ISettingsSerializer, IDynamicSettingsInfo
    {
        InternalIsolatedStorageSerializer m_serializer = new InternalIsolatedStorageSerializer();

        //------------------------------------------------------------------------------
        public void SetIsolatedStorage(IsolatedStorageFile store)
        {
            m_serializer.SetIsolatedStorage(store);
        }

        #region ISettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, bool secure, SettingBaseAttribute attr, out object value)
        {
            if (secure)
            {
                throw new ArgumentException(
                    String.Format("IsolatedStorageSerializer.Load: " +
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
                    String.Format("IsolatedStorageSerializer.Save: " +
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
                    String.Format("IsolatedStorageSerializer.Delete: " +
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
                    String.Format("IsolatedStorageSerializer.Contains: " +
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

