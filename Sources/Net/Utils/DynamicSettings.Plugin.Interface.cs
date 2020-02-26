using System.Collections.Generic;

namespace Advexp.DynamicSettings.Plugin
{
    public interface IDynamicSettingsPlugin
    {
        void LoadSettings();
        void SaveSettings();
        void DeleteSettings();

        void LoadSetting(string settingName);
        void SaveSetting(string settingName);
        void DeleteSetting(string settingName);

        bool Contains(string settingName);

        void SetSettingsOrder(IEnumerable<string> settingsOrder);

        T GetSetting<T>(string settingName);
        T GetSetting<T>(string settingName, T defaultValue);
        void SetSetting<T>(string settingName, T settingValue);

        void SetDefaultSettings(IDictionary<string, object> defaultSettings);

        IEnumerator<string> GetEnumerator();
        int Count { get; }
    }
}
