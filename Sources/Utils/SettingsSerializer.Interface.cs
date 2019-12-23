namespace Advexp
{
    public interface ISettingsSerializer
    {
        bool Load(string settingName, bool secure, SettingBaseAttribute attr, out object value);
        void Save(string settingName, bool secure, SettingBaseAttribute attr, object value);
        void Delete(string settingName, bool secure, SettingBaseAttribute attr);

        bool Contains(string settingName, bool secure, SettingBaseAttribute attr);

        void Synchronize();
    }
}