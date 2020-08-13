using Advexp.LocalDynamicSettings.Plugin;

namespace Advexp
{
    class ModuleInitializer : ModuleBaseInitializer, IModuleInitializer
    {
        //------------------------------------------------------------------------------
        public static void CreateInstance()
        {
            if (s_ModuleInitializer == null)
            {
                s_ModuleInitializer = new ModuleInitializer();
            }
        }

        //------------------------------------------------------------------------------
        public void Initialize()
        {
            if (!InternalConfiguration.IsInitialized && s_ModuleInitializer != null)
            {
                InternalConfiguration.PlatformHelper = new PlatformHelpers();
                SettingsBaseConfiguration.RegisterSettingsAttribute<SettingAttribute, ISettingsSerializerImpl>();
                UserDefaultsHelper.LoadDefaultSettings();

                //SettingsBaseConfiguration.RegisterSettingsPlugin<IStaticSettingsPlugin, StaticSettingsPlugin>();
                SettingsBaseConfiguration.RegisterSettingsPlugin<ILocalDynamicSettingsPlugin, LocalDynamicSettingsPlugin>();

                s_ModuleInitializer = null;
                InternalConfiguration.IsInitialized = true;
            }
        }
    }
}

