namespace Advexp
{
    class ModuleBaseInitializer
    {
        protected static IModuleInitializer s_ModuleInitializer = null;

        //------------------------------------------------------------------------------
        public static IModuleInitializer Instance
        {
            get
            {
                return s_ModuleInitializer;
            }
        }
    }
}

