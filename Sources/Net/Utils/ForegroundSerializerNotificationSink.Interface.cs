namespace Advexp
{
    internal interface IForegroundSerializerNotificationSink
    {
        void OnStartStaticSettingsSerialization(object settings);
        void OnEndStaticSettingsSerialization();

        void OnStartDynamicSettingsSerialization(SettingsBasePlugin localDynamicSettings);
        void OnEndDynamicSettingsSerialization();

        void OnStartDynamicSettingsPluginSerialization(SettingsBasePlugin sourcePlugin);
        void OnEndDynamicSettingsPluginSerialization();
    }
}
