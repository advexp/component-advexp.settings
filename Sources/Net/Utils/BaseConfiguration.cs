using System;
using System.Reflection;

namespace Advexp
{
#if __TDD__
    sealed class TDDData
    {
        public enum SerializerActions
        {
            Load,
            Save,
            Delete,
            LoadDefaults,
            Synchronize,
        }

        public Action<Type, SerializerActions, string, bool> SerializerAction = delegate(
            Type serializerType,
            SerializerActions action,
            string settingName,
            bool secure) { };
    }
#endif // __TDD__

    public class SettingsBaseConfiguration
    {
        //------------------------------------------------------------------------------
        static AdvancedConfiguration s_AdvancedConfiguration;
        internal static AdvancedConfiguration AdvancedConfigurationInternal
        {
            get
            {
                if (s_AdvancedConfiguration == null)
                {
                    s_AdvancedConfiguration = new AdvancedConfiguration();
                }

                return s_AdvancedConfiguration;
            }
        }

        //------------------------------------------------------------------------------
        public static AdvancedConfiguration AdvancedConfiguration
        {
            get
            {
                return SettingsBaseConfiguration.AdvancedConfigurationInternal;
            }
        }

        internal const string NamespacePatternName = "{NamespaceName}";
        internal const string ClassNamePatternName = "{ClassName}";
        internal const string FieldNamePatternName = "{FieldName}";
        internal const string DelimeterPatternName = "{Delimeter}";

        internal const char DefaultDelimeter = '.';

        static string s_DefaultSettingsNamePattern = null;

        static string s_SettingsNamePattern = null;

        //------------------------------------------------------------------------------
        internal static string SettingsNamePattern
        {
            get
            {
                if (s_SettingsNamePattern == null)
                {
                    if (s_DefaultSettingsNamePattern == null)
                    {
                        s_DefaultSettingsNamePattern =
                            SettingsBaseConfiguration.NamespacePatternName +
                                                     SettingsBaseConfiguration.DelimeterPatternName +
                            SettingsBaseConfiguration.ClassNamePatternName +
                                                     SettingsBaseConfiguration.DelimeterPatternName +
                            SettingsBaseConfiguration.FieldNamePatternName;
                    }

                    return s_DefaultSettingsNamePattern;
                }

                return s_SettingsNamePattern;
            }
        }

#if __TDD__
        internal static TDDData TDDData = new TDDData();
        internal static ITDDHandler s_TDDHandler = null;
#endif // __TDD__

#if !__WINDOWS__
        public static string EncryptionServiceID = "Advexp.Settings";
#endif // !__WINDOWS__

        public static ISettingsSerializer Serializer = null;

        /*
        public static bool EnableFormatMigration = false;
        */

        public static LogLevel LogLevel = LogLevel.Warning;

        //------------------------------------------------------------------------------
        public static void RegisterSettingsPlugin<PLUGIN_INTERFACE_TYPE, PLUGIN_OBJECT_TYPE>() 
            where PLUGIN_OBJECT_TYPE : SettingsBasePlugin
        {
            var interfaceType = typeof(PLUGIN_INTERFACE_TYPE);
            var pluginObjectType = typeof(PLUGIN_OBJECT_TYPE);

            Type foo = null;
            bool exist = InternalConfiguration.Plugins.TryGetValue(interfaceType, out foo);
            if (exist)
            {
                InternalConfiguration.Plugins[interfaceType] = pluginObjectType;
            }
            else
            {
                InternalConfiguration.Plugins.Add(interfaceType, pluginObjectType);
            }

            var setupMI = pluginObjectType.GetMethod("Setup");
            if (setupMI != null)
            {
                setupMI.Invoke(null, new object[]{});
            }
            else
            {
                string msg = String.Format("'Setup' method is not available in '{0}' type", pluginObjectType.ToString());
                throw new MissingMemberException(msg);
            }
        }

        //------------------------------------------------------------------------------
        public static void RegisterSettingsAttribute<ATTRIBUTE_TYPE, SERIALIZER_TYPE>()
            where ATTRIBUTE_TYPE : SettingBaseAttribute
            where SERIALIZER_TYPE : ISettingsSerializer
        {
            var attributeType = typeof(ATTRIBUTE_TYPE);
            var serializerType = typeof(SERIALIZER_TYPE);

            InternalConfiguration.SettingsAttributes[attributeType] = serializerType;
        }

#if __TDD__

        //------------------------------------------------------------------------------
        public static void SetTDDHandler(ITDDHandler handler)
        {
            s_TDDHandler = handler;
        }

#endif // __TDD__

    }
}

