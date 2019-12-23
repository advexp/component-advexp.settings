namespace Advexp
{
    public class Settings<T> : SettingsT<T> where T : new()
    {
        //------------------------------------------------------------------------------
        public Settings()
        {
            // create module initializer in context of platform
            ModuleInitializer.CreateInstance();
        }
    }
}
