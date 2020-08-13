using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Advexp
{
    public partial class SettingsT<ST> : IPluginHolder, IPluginContext where ST : new()
    {
        //------------------------------------------------------------------------------
        public static ST Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new ST();
                }

                return (ST)s_instance;
            }
        }

        //------------------------------------------------------------------------------
        public static void LoadSettings()
        {
            LoadSettings(Instance);
        }

        //------------------------------------------------------------------------------
        public void LoadObjectSettings()
        {
            LoadSettings(this);
        }

        //------------------------------------------------------------------------------
        public static void LoadSettings(object settings)
        {
            ProcessClass(settings, SettingsEnumerationMode.Load);
            LoadAllDynamicSettings(settings);
        }

        //------------------------------------------------------------------------------
        public static void SaveSettings()
        {
            SaveSettings(Instance);
        }

        //------------------------------------------------------------------------------
        public void SaveObjectSettings()
        {
            SaveSettings(this);
        }

        //------------------------------------------------------------------------------
        public static void SaveSettings(object settings)
        {
            ProcessClass(settings, SettingsEnumerationMode.Save);
            SaveAllDynamicSettings(settings);
        }

        //------------------------------------------------------------------------------
        public static void DeleteSettings()
        {
            DeleteSettings(Instance);
        }

        //------------------------------------------------------------------------------
        public void DeleteObjectSettings()
        {
            DeleteSettings(this);
        }

        //------------------------------------------------------------------------------
        public static void DeleteSettings(object settings)
        {
            ProcessClass(settings, SettingsEnumerationMode.Delete);
            DeleteAllDynamicSettings(settings);
        }

        //------------------------------------------------------------------------------
        public static void LoadDefaultSettings()
        {
            LoadDefaultSettings(Instance);
        }

        //------------------------------------------------------------------------------
        public void LoadObjectDefaultSettings()
        {
            LoadDefaultSettings(this);
        }

        //------------------------------------------------------------------------------
        public static void LoadDefaultSettings(object settings)
        {
            ProcessClass(settings, SettingsEnumerationMode.LoadDefaults);
        }

        //------------------------------------------------------------------------------
        public static PLUGIN_INTERFACE_TYPE GetPlugin<PLUGIN_INTERFACE_TYPE>()
        {
            var plugin = ((IPluginHolder)Instance).GetPlugin<PLUGIN_INTERFACE_TYPE>();

            return plugin;
        }

        //------------------------------------------------------------------------------
        public PLUGIN_INTERFACE_TYPE GetObjectPlugin<PLUGIN_INTERFACE_TYPE>()
        {
            var plugin = ((IPluginHolder)this).GetPlugin<PLUGIN_INTERFACE_TYPE>();

            return plugin;
        }
        //------------------------------------------------------------------------------
        public static void LoadObjectSetting<T>(object settings, Expression<Func<ST, T>> value)
        {
            ProcessExpression(settings, value, SettingsEnumerationMode.Load);
        }

        //------------------------------------------------------------------------------
        public static void SaveObjectSetting<T>(object settings, Expression<Func<ST, T>> value)
        {
            ProcessExpression(settings, value, SettingsEnumerationMode.Save);
        }

        //------------------------------------------------------------------------------
        public static void DeleteObjectSetting<T>(object settings, Expression<Func<ST, T>> value)
        {
            ProcessExpression(settings, value, SettingsEnumerationMode.Delete);
        }

        //------------------------------------------------------------------------------
        public static void LoadObjectDefaultSetting<T>(object settings, Expression<Func<ST, T>> value)
        {
            ProcessExpression(settings, value, SettingsEnumerationMode.LoadDefaults);
        }

        //------------------------------------------------------------------------------
        public static void LoadSetting<T>(Expression<Func<ST, T>> value)
        {
            LoadObjectSetting(Instance, value);
        }

        //------------------------------------------------------------------------------
        public void LoadObjectSetting<T>(Expression<Func<ST, T>> value)
        {
            LoadObjectSetting(this, value);
        }

        //------------------------------------------------------------------------------
        public static void SaveSetting<T>(Expression<Func<ST, T>> value)
        {
            SaveObjectSetting(Instance, value);
        }

        //------------------------------------------------------------------------------
        public void SaveObjectSetting<T>(Expression<Func<ST, T>> value)
        {
            SaveObjectSetting(this, value);
        }

        //------------------------------------------------------------------------------
        public static void DeleteSetting<T>(Expression<Func<ST, T>> value)
        {
            DeleteObjectSetting(Instance, value);
        }

        //------------------------------------------------------------------------------
        public void DeleteObjectSetting<T>(Expression<Func<ST, T>> value)
        {
            DeleteObjectSetting(this, value);
        }

        //------------------------------------------------------------------------------
        public static void LoadDefaultSetting<T>(Expression<Func<ST, T>> value)
        {
            LoadObjectDefaultSetting(Instance, value);
        }

        //------------------------------------------------------------------------------
        public void LoadObjectDefaultSetting<T>(Expression<Func<ST, T>> value)
        {
            LoadObjectDefaultSetting(this, value);
        }

        //------------------------------------------------------------------------------
        public static bool ContainsSetting<T>(Expression<Func<ST, T>> value)
        {
            return ContainsObjectSetting(Instance, value);
        }

        //------------------------------------------------------------------------------
        public bool ContainsObjectSetting<T>(Expression<Func<ST, T>> value)
        {
            return ContainsObjectSetting(this, value);
        }

        //------------------------------------------------------------------------------
        public static bool ContainsObjectSetting<T>(object settings, Expression<Func<ST, T>> value)
        {
            return ContainsObjectSettingImpl(settings, value);
        }

        // Settings metadata
        //------------------------------------------------------------------------------
        public static void SetObjectSettingMetadata(object settings, string settingName, string metadataName, object metadataValue)
        {
            SetObjectSettingMetadataImpl(settings, settingName, metadataName, metadataValue);
        }

        //------------------------------------------------------------------------------
        public void SetObjectSettingMetadata<T>(Expression<Func<ST, T>> value, string metadataName, object metadataValue)
        {
            SettingBaseAttribute settingBaseAttribute = GetSettingBaseAttribute(value);
            SetObjectSettingMetadata(this, settingBaseAttribute.Name, metadataName, metadataValue);
        }

        //------------------------------------------------------------------------------
        public void SetObjectSettingMetadata(string settingName, string metadataName, object metadataValue)
        {
            SetObjectSettingMetadata(this, settingName, metadataName, metadataValue);
        }

        //------------------------------------------------------------------------------
        public static void SetSettingMetadata<T>(Expression<Func<ST, T>> value, string metadataName, object metadataValue)
        {
            SettingBaseAttribute settingBaseAttribute = GetSettingBaseAttribute(value);
            SetObjectSettingMetadata(Instance, settingBaseAttribute.Name, metadataName, metadataValue);
        }

        //------------------------------------------------------------------------------
        public static void SetSettingMetadata(string settingName, string metadataName, object metadataValue)
        {
            SetObjectSettingMetadata(Instance, settingName, metadataName, metadataValue);
        }

        //------------------------------------------------------------------------------
        public static void SetClassMetadata(string metadataName, object metadataValue)
        {
            SetClassMetadataImpl(metadataName, metadataValue);
        }

        //------------------------------------------------------------------------------
        public static bool TryGetObjectSettingMetadata(object settings, string settingName, string metadataName, out object metadataValue)
        {
            return TryGetObjectSettingMetadataImpl(settings, settingName, metadataName, out metadataValue);
        }

        //------------------------------------------------------------------------------
        public bool TryGetObjectSettingMetadata<T>(Expression<Func<ST, T>> value, string metadataName, out object metadataValue)
        {
            SettingBaseAttribute settingBaseAttribute = GetSettingBaseAttribute(value);
            return TryGetObjectSettingMetadata(this, settingBaseAttribute.Name, metadataName, out metadataValue);
        }

        //------------------------------------------------------------------------------
        public bool TryGetObjectSettingMetadata(string settingName, string metadataName, out object metadataValue)
        {
            return TryGetObjectSettingMetadata(this, settingName, metadataName, out metadataValue);
        }

        //------------------------------------------------------------------------------
        public static bool TryGetSettingMetadata<T>(Expression<Func<ST, T>> value, string metadataName, out object metadataValue)
        {
            SettingBaseAttribute settingBaseAttribute = GetSettingBaseAttribute(value);
            return TryGetObjectSettingMetadata(Instance, settingBaseAttribute.Name, metadataName, out metadataValue);
        }

        //------------------------------------------------------------------------------
        public static bool TryGetSettingMetadata(string settingName, string metadataName, out object metadataValue)
        {
            return TryGetObjectSettingMetadata(Instance, settingName, metadataName, out metadataValue);
        }

        //------------------------------------------------------------------------------
        public static bool TryGetClassMetadata(string metadataName, out object metadataValue)
        {
            return TryGetClassMetadataImpl(metadataName, out metadataValue);
        }
    }
}
