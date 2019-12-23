using System;

namespace Advexp
{
    interface IPluginHolder
    {
        object GetPlugin(Type pluginType);
        T GetPlugin<T>();
    }
}

