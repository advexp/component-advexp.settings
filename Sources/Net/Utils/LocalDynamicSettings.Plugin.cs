namespace Advexp.LocalDynamicSettings.Plugin
{
    public class LocalDynamicSettingsPlugin : 
        DynamicSettingsBasePlugin<SettingAttribute>, 
        ILocalDynamicSettingsPlugin
    {
        //------------------------------------------------------------------------------
        // issue4 Default constructor not found for type
        // https://bitbucket.org/advexp/component-advexp.settings/issues/4/default-constructor-not-found-for-type
        [Advexp.Preserve]
        public LocalDynamicSettingsPlugin()
        {
        }

        //------------------------------------------------------------------------------
        public static void Setup()
        {
            // Setup will be called when EnablePlugin occure and Context will be null
            // Function will be called via reflection
        }

    }
}
