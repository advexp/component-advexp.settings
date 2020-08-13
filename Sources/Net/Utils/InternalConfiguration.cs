using System;
using System.Reflection;
using System.Collections.Generic;

namespace Advexp
{
    static class InternalConfiguration
    {
        //------------------------------------------------------------------------------
        public static bool IsInitialized { get; set; }

        //------------------------------------------------------------------------------
        public static IPlatformHelper PlatformHelper { get; set; }

        //------------------------------------------------------------------------------
        public static Dictionary<Type/*plugin interface type*/, Type/*plugin object type*/> Plugins
        {
            get;
            set;
        }

        //------------------------------------------------------------------------------
        public static Dictionary<Type/*attribute type*/, Type/*serializer type*/> SettingsAttributes
        {
            get;
            set;
        }

        //------------------------------------------------------------------------------
        static InternalConfiguration()
        {
            Plugins = new Dictionary<Type, Type>();
            SettingsAttributes = new Dictionary<Type, Type>();
        }

        //------------------------------------------------------------------------------
        public static PLUGIN_INTERFACE_TYPE GetPluginHelper<PLUGIN_INTERFACE_TYPE>(IPluginContext context)
        {
            var pluginInterfaceType = typeof(PLUGIN_INTERFACE_TYPE);
            var pluginObjectType = InternalConfiguration.Plugins[pluginInterfaceType];

            SettingsBasePlugin pluginObject = (SettingsBasePlugin)Activator.CreateInstance(pluginObjectType);
            pluginObject.Context = context;

            return (PLUGIN_INTERFACE_TYPE)(object)pluginObject;
        }

        //------------------------------------------------------------------------------
        public static object GetPluginHelper(Type pluginInterfaceType, IPluginContext context)
        {
            var pluginObjectType = InternalConfiguration.Plugins[pluginInterfaceType];

            SettingsBasePlugin pluginObject = (SettingsBasePlugin)Activator.CreateInstance(pluginObjectType);
            pluginObject.Context = context;

            return pluginObject;
        }

        //------------------------------------------------------------------------------
        public static NotImplementedException GetNotImplementedForEvaluationException(string methodName)
        {
            var excMsg = String.Format("In the evaluation version of the library '{0}' was not implemented", methodName);
            return new NotImplementedException(excMsg);
        }

        //------------------------------------------------------------------------------
        public static void ThrowNotImplementedForEvaluation(string methodName)
        {
            var exc = InternalConfiguration.GetNotImplementedForEvaluationException(methodName);
            throw exc;
        }
    }
}
