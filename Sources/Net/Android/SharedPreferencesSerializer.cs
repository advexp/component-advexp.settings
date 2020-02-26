using System;
using System.Collections.Generic;
using Android.Content;
using Android.Preferences;

namespace Advexp
{
    class InternalSharedPreferencesSerializer
        : IInternalSettingsSerializer, IDynamicSettingsInfo
    {
        ISharedPreferences m_sharedPreferences = null;
        ISharedPreferencesEditor m_editor = null;

        //------------------------------------------------------------------------------
        public InternalSharedPreferencesSerializer()
        {
        }

        //------------------------------------------------------------------------------
        public ISharedPreferences GetSharedPreferences()
        {
            ISharedPreferences sharedPreferences = null;

            if (m_sharedPreferences != null)
            {
                sharedPreferences = m_sharedPreferences;
            }
            else
            {
                Context context = Android.App.Application.Context;
                sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(context);
            }

            Debug.Assert(sharedPreferences != null);

            return sharedPreferences;
        }

        //------------------------------------------------------------------------------
        public void SetSharedPreferences(ISharedPreferences sharedPreferences)
        {
            m_sharedPreferences = sharedPreferences;
        }

        #region IInternalSettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, SettingBaseAttribute attr, out object value)
        {
            bool loadSuccess = false;

            var prefs = GetSharedPreferences();
            if (!prefs.Contains(settingName))
            {
                value = null;
                loadSuccess = false;

                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "SharedPreferencesSerializer.Load - no such setting",
                                                         settingName);
            }
            else
            {
                // in this case simplification will be made in the SettingsSerializer.cs
                loadSuccess = prefs.All.TryGetValue(settingName, out value);

                if (loadSuccess)
                {
                    InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                             "SharedPreferencesSerializer.Load - success",
                                                             settingName);
                }
                else
                {
                    InternalConfiguration.PlatformHelper.Log(LogLevel.Error,
                                                             "SharedPreferencesSerializer.Load - error",
                                                             settingName);
                }
            }

            return loadSuccess;
        }

        //------------------------------------------------------------------------------
        public void Save(string settingName, SettingBaseAttribute attr, object value)
        {
            if (m_editor == null)
            {
                var prefs = GetSharedPreferences();
                m_editor = prefs.Edit();
            }

            object saveValue = value;

            if (value is bool)
            {
                m_editor.PutBoolean(settingName, (bool)value);
            }
            else if (value is int)
            {
                m_editor.PutInt(settingName, (int)value);
            }
            else if (value is long)
            {
                m_editor.PutLong(settingName, (long)value);
            }
            else if (value is float)
            {
                m_editor.PutFloat(settingName, (float)value);
            }
            else
            {
                var simpleValue = SettingsSerializerHelper.SimplifyObject(value);
                var stringValue = SettingsSerializerHelper.ConvertUsingTypeConverter<String>(simpleValue);

                saveValue = stringValue;

                m_editor.PutString(settingName, stringValue);
            }

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                     "SharedPreferencesSerializer.Save - success",
                                                     String.Format("Setting is {0}. Value is {1}", 
                                                                   settingName, 
                                                                   saveValue != null ? saveValue.ToString() : "null"));
        }

        //------------------------------------------------------------------------------
        public void Delete(string settingName, SettingBaseAttribute attr)
        {
            if (m_editor == null)
            {
                var prefs = GetSharedPreferences();
                m_editor = prefs.Edit();
            }

            m_editor.Remove(settingName);

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                     "SharedPreferencesSerializer.Delete - success",
                                                     settingName);
        }

        //------------------------------------------------------------------------------
        public bool Contains(string settingName, SettingBaseAttribute attr)
        {
            var prefs = GetSharedPreferences();
            if (prefs.Contains(settingName))
            {
                return true;
            }

            return false;
        }

        //------------------------------------------------------------------------------
        public void Synchronize()
        {
            if (m_editor != null)
            {
                m_editor.Commit();
                m_editor = null;
            }
        }

        #endregion

        #region IDynamicSettingsInfo implementation

        //------------------------------------------------------------------------------
        public IEnumerable<string> GetDynamicSettingsNames()
        {
            var prefs = GetSharedPreferences();

            List<string> dynamicSettingsNames = new List<string>();

            foreach (var key in prefs.All.Keys)
            {
                if (SettingNameFormatInfo.GetSettingNameMode(key) == SettingNameMode.Dynamic)
                {
                    dynamicSettingsNames.Add(key);
                }
            }

            return dynamicSettingsNames;
        }

        #endregion

    }

    //------------------------------------------------------------------------------
    public class SharedPreferencesSerializer
        : ISettingsSerializer, IDynamicSettingsInfo
    {
        InternalSharedPreferencesSerializer m_serializer = new InternalSharedPreferencesSerializer();

        //------------------------------------------------------------------------------
        public void SetSharedPreferences(ISharedPreferences sharedPreferences)
        {
            m_serializer.SetSharedPreferences(sharedPreferences);
        }

        #region ISettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, bool secure, SettingBaseAttribute attr, out object value)
        {
            if (secure)
            {
                throw new ArgumentException(
                    String.Format("SharedPreferencesSerializer.Load: " +
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
                    String.Format("SharedPreferencesSerializer.Save: " +
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
                    String.Format("SharedPreferencesSerializer.Delete: " +
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
                    String.Format("SharedPreferencesSerializer.Contains: " +
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

