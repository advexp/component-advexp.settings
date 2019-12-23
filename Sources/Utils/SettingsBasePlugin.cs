using System;

namespace Advexp
{
    public abstract class SettingsBasePlugin
    {
        //------------------------------------------------------------------------------
        public IPluginContext Context
        {
            get;
            internal set;
        }

        //------------------------------------------------------------------------------
        public abstract string PluginName
        {
            get;
        }
    }
}