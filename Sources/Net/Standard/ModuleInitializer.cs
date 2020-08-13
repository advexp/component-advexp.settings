using System;

namespace Advexp
{
    class ModuleInitializer : ModuleBaseInitializer, IModuleInitializer
    {
        //------------------------------------------------------------------------------
        public static void CreateInstance()
        {
            s_ModuleInitializer = new ModuleInitializer();
        }

        //------------------------------------------------------------------------------
        public void Initialize()
        {
            throw new NotImplementedException("You should reference the Advexp.Settings library from your main application project in order to reference the platform-specific implementation");
        }
    }
}

