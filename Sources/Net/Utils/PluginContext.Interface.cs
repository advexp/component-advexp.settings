using System;

namespace Advexp
{
    public interface IPluginContext
    {
        void SaveSettings(ISettingsSerializer pluginForegroundSerializer, SettingsBasePlugin sourcePlugin);
        void LoadSettings(ISettingsSerializer pluginForegroundSerializer, SettingsBasePlugin sourcePlugin);
        void DeleteSettings(ISettingsSerializer pluginForegroundSerializer, SettingsBasePlugin sourcePlugin);

        bool TryGetSettingMetadata(string settingName, string metadataName, out object metadataValue);
        bool TryGetClassMetadata(string metadataName, out object metadataValue);

        void SetSettingMetadata(string settingName, string metadataName, object metadataValue);
        void SetClassMetadata(string metadataName, object metadataValue);

        ISettingsSerializer GetSettingsForegroundSerializer();
    }
}

