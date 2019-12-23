using System;
using System.IO;
using System.Collections.Generic;
using Windows.Storage;

namespace Advexp
{
    class InternalApplicationDataContainerSerializer
        : IInternalSettingsSerializer, IDynamicSettingsInfo
    {
        string m_DataContainerFileName = null;

        //------------------------------------------------------------------------------
        ApplicationDataContainer GetAppSettings()
        {
            if (string.IsNullOrEmpty(m_DataContainerFileName))
            {
                return ApplicationData.Current.LocalSettings;
            }

            if (!ApplicationData.Current.LocalSettings.Containers.ContainsKey(m_DataContainerFileName))
            {
                ApplicationData.Current.LocalSettings.CreateContainer(m_DataContainerFileName, ApplicationDataCreateDisposition.Always);
            }

            return ApplicationData.Current.LocalSettings.Containers[m_DataContainerFileName];
        }

        //------------------------------------------------------------------------------
        public void SetDataContainerFileName(string fileName)
        {
            m_DataContainerFileName = fileName;
        }

        #region IInternalSettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, SettingBaseAttribute attr, out object value)
        {
            value = null;

            var settings = GetAppSettings();

            // If the key exists, retrieve the value.
            if (settings.Values.ContainsKey(settingName))
            {
                value = settings.Values[settingName];
            }
            else
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "ApplicationDataContainerSerializer.Load - no such setting",
                                                         settingName);
            }

            if (value == null)
            {
                return false;
            }

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                         "ApplicationDataContainerSerializer.Load - success",
                                         settingName);

            return true;
        }

        //------------------------------------------------------------------------------
        public void Save(string settingName, SettingBaseAttribute attr, object value)
        {
            var settings = GetAppSettings();

            settings.Values[settingName] = value;

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                         "ApplicationDataContainerSerializer.Save - success",
                                         String.Format("setting is {0}, value is {1}",
                                                       settingName,
                                                       value != null ? value : "null"));
        }

        //------------------------------------------------------------------------------
        public void Delete(string settingName, SettingBaseAttribute attr)
        {
            var settings = GetAppSettings();

            if (settings.Values.ContainsKey(settingName))
            {
                settings.Values.Remove(settingName);
            }

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                     "ApplicationDataContainerSerializer.Delete - success",
                                                     String.Format("setting is {0}", settingName));
        }

        //------------------------------------------------------------------------------
        public bool Contains(string settingName, SettingBaseAttribute attr)
        {
            var settings = GetAppSettings();

            bool contains = settings.Values.ContainsKey(settingName);

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
            var dynamicSettingsNames = new List<string>();

            var settings = GetAppSettings();

            foreach (var item in settings.Values)
            {
                var settingName = item.Key;
                if (SettingNameFormatInfo.GetSettingNameMode(settingName) == SettingNameMode.Dynamic)
                {
                    dynamicSettingsNames.Add(settingName);
                }
            }

            return dynamicSettingsNames;
        }

        #endregion
    }

    //------------------------------------------------------------------------------
    public class ApplicationDataContainerSerializer
        : ISettingsSerializer, IDynamicSettingsInfo
    {
        InternalApplicationDataContainerSerializer m_serializer = new InternalApplicationDataContainerSerializer();

        //------------------------------------------------------------------------------
        public void SetDataContainerFileName(string fileName)
        {
            m_serializer.SetDataContainerFileName(fileName);
        }

        #region ISettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, bool secure, SettingBaseAttribute attr, out object value)
        {
            if (secure)
            {
                throw new ArgumentException(
                    String.Format("ApplicationDataContainerSerializer.Load: " +
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
                    String.Format("ApplicationDataContainerSerializer.Save: " +
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
                    String.Format("ApplicationDataContainerSerializer.Delete: " +
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
                    String.Format("ApplicationDataContainerSerializer.Contains: " +
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

