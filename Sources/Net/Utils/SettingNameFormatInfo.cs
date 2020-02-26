using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Advexp
{
    internal enum SettingNameMode
    {
        Unknown,
        Static,
        Dynamic,
        Service,
    }

    internal class SettingNameFormatInfo
    {
        public const String CurrentVersionPrefixStartPart = "v";
        public const String CurrentVersionPrefix = CurrentVersionPrefixStartPart + "2";
        public const String DynamicSettingNamePrefix = "D";
        public const String ServiceSettingNamePrefix = "S";

        String m_FullSettingName;
        char m_Delimeter;
        SettingNameMode m_SettingNameMode;

        //------------------------------------------------------------------------------
        String m_RawSettingName = null;
        public String RawSettingName
        {
            get
            {
                if (String.IsNullOrEmpty(m_RawSettingName))
                {
                    string settingsTypeInfo;
                    string formatVersionPrefix;
                    Parse(m_FullSettingName, m_Delimeter, out settingsTypeInfo, out formatVersionPrefix, out m_RawSettingName, m_SettingNameMode);
                }

                return m_RawSettingName;
            }
        }

        //------------------------------------------------------------------------------
        String m_V1SettingName = null;
        public String V1SettingName 
        {
            get
            {
                if (m_V1SettingName == null)
                {
                    m_V1SettingName = RawSettingName;
                }

                return m_V1SettingName;
            }
        }

        //------------------------------------------------------------------------------
        static SettingNameFormatInfo()
        {
        }

        //------------------------------------------------------------------------------
        public SettingNameFormatInfo(String fullSettingName, char delimeter, SettingNameMode nameMode)
        {
            m_FullSettingName = fullSettingName;
            m_Delimeter = delimeter;
            if (nameMode != SettingNameMode.Unknown)
            {
                m_SettingNameMode = nameMode;
            }
            else
            {
                m_SettingNameMode = GetSettingNameMode(fullSettingName);
            }
        }

        //------------------------------------------------------------------------------
        static Match GetStaticMatch(string fullSettingName, char delimeter)
        {
            Match match = Regex.Match(fullSettingName, 
                                      String.Format("^({1}\\d+)({0})(.*)", 
                                                    Regex.Escape(delimeter.ToString()), 
                                                    Regex.Escape(CurrentVersionPrefixStartPart)), 
                                      RegexOptions.None);

            return match;
        }

        //------------------------------------------------------------------------------
        static Match GetDynamicMatch(string fullSettingName, char delimeter)
        {
            var strMatch = String.Format("^({0})({1})({2}\\d+)({1})(.*?)({1})(.*?)({1})(.*)",
                                         Regex.Escape(DynamicSettingNamePrefix),
                                         Regex.Escape(delimeter.ToString()),
                                         Regex.Escape(CurrentVersionPrefixStartPart));
            Match match = Regex.Match(fullSettingName, strMatch, RegexOptions.None);

            return match;
        }

        //------------------------------------------------------------------------------
        static Match GetServiceMatch(string fullSettingName, char delimeter)
        {
            var strMatch = String.Format("^({0})({1})({2}\\d+)({1})(.*?)({1})(.*?)({1})(.*)",
                                         Regex.Escape(ServiceSettingNamePrefix),
                                         Regex.Escape(delimeter.ToString()),
                                         Regex.Escape(CurrentVersionPrefixStartPart));
            Match match = Regex.Match(fullSettingName, strMatch, RegexOptions.None);

            return match;
        }

        //------------------------------------------------------------------------------
        public static bool Parse(string settingNameWithPrefix, 
                                 char delimeter, 
                                 out string formatVersionPrefix, 
                                 out string settingsTypeInfo, 
                                 out string settingName, 
                                 SettingNameMode nameMode)
        {
            formatVersionPrefix = String.Empty;
            settingsTypeInfo = String.Empty;

            settingName = settingNameWithPrefix;

            switch (nameMode)
            {
                case SettingNameMode.Static:
                    {
                        Match match = GetStaticMatch(settingNameWithPrefix, delimeter);
                        if (match.Success)
                        {
                            formatVersionPrefix = match.Groups[1].Value;
                            settingName = match.Groups[3].Value;

                            return true;
                        }
                        break;
                    }
                case SettingNameMode.Dynamic:
                    {
                        Match match = GetDynamicMatch(settingNameWithPrefix, delimeter);
                        if (match.Success)
                        {
                            formatVersionPrefix = match.Groups[3].Value;
                            settingsTypeInfo = match.Groups[5].Value + match.Groups[6].Value + match.Groups[7].Value;
                            settingName = match.Groups[9].Value;

                            return true;
                        }
                        break;
                    }
                case SettingNameMode.Service:
                    {
                        Match match = GetServiceMatch(settingNameWithPrefix, delimeter);
                        if (match.Success)
                        {
                            formatVersionPrefix = match.Groups[3].Value;
                            settingsTypeInfo = match.Groups[5].Value + match.Groups[6].Value + match.Groups[7].Value;
                            settingName = match.Groups[9].Value;

                            return true;
                        }
                        break;
                    }
            }

            return false;
        }

        //------------------------------------------------------------------------------
        public static SettingNameMode GetSettingNameMode(string fullSettingName)
        {
            var staticSetting = fullSettingName.StartsWith(CurrentVersionPrefixStartPart, StringComparison.OrdinalIgnoreCase);
            if (staticSetting)
            {
                return SettingNameMode.Static;
            }

            var dynamicSetting = fullSettingName.StartsWith(DynamicSettingNamePrefix, StringComparison.OrdinalIgnoreCase);
            if (dynamicSetting)
            {
                return SettingNameMode.Dynamic;
            }

            var serviceSetting = fullSettingName.StartsWith(ServiceSettingNamePrefix, StringComparison.OrdinalIgnoreCase);
            if (serviceSetting)
            {
                return SettingNameMode.Service;
            }

            return SettingNameMode.Unknown;
        }

        //------------------------------------------------------------------------------
        public static char GetSettingNameDelimeter(ISettingsSerializerWishes wishes)
        {
            char delimeter = SettingsBaseConfiguration.DefaultDelimeter;

            if (wishes != null)
            {
                delimeter = wishes.Delimeter();
            }

            if (delimeter != '.' && delimeter != '_')
            {
                Debug.Assert(false, String.Format("Unknown delimeter '{0}'. Set to default '{1}'", delimeter, SettingsBaseConfiguration.DefaultDelimeter));
                delimeter = SettingsBaseConfiguration.DefaultDelimeter;
            }

            return delimeter;
        }

        //------------------------------------------------------------------------------
        public static string GetSettingsTypeInfo(Type settingsType, ISettingsSerializerWishes wishes)
        {
            var delimeter = SettingNameFormatInfo.GetSettingNameDelimeter(wishes);

            string settingsTypeInfo;

            if (wishes != null)
            {
                var updatedNamespaceName = wishes.RemoveInappropriateSymbols(settingsType.Namespace);
                var updatedClassName = wishes.RemoveInappropriateSymbols(settingsType.Name);

                settingsTypeInfo = updatedNamespaceName + delimeter + updatedClassName;
            }
            else
            {
                settingsTypeInfo = settingsType.Namespace + delimeter + settingsType.Name;
            }

            return settingsTypeInfo;
        }

        //------------------------------------------------------------------------------
        static string GetFullSettingNameByPatternAndAddPrefix(string settingName, ISettingsSerializerWishes wishes, Type settingsType,
                                                  SettingNameMode nameMode)
        {
            if (settingsType != null)
            {
                var settingNamePattern = SettingsBaseConfiguration.SettingsNamePattern;

                string updatedSettingName;
                string updatedClassName;
                string updatedNamespaceName;

                if (wishes != null)
                {
                    updatedSettingName = wishes.RemoveInappropriateSymbols(settingName);
                    updatedClassName = wishes.RemoveInappropriateSymbols(settingsType.Name);
                    updatedNamespaceName = wishes.RemoveInappropriateSymbols(settingsType.Namespace);
                }
                else
                {
                    updatedSettingName = settingName;
                    updatedClassName = settingsType.Name;
                    updatedNamespaceName = settingsType.Namespace;
                }

                settingNamePattern = settingNamePattern.Replace(SettingsBaseConfiguration.FieldNamePatternName, updatedSettingName);

                var delimeter = GetSettingNameDelimeter(wishes);
                settingNamePattern = settingNamePattern.Replace(SettingsBaseConfiguration.DelimeterPatternName, delimeter.ToString());

                if (String.IsNullOrEmpty(settingsType.Namespace))
                {
                    // remove empty namespace if it doesn't exist
                    settingNamePattern = settingNamePattern.Replace(SettingsBaseConfiguration.NamespacePatternName, "");
                }
                else
                {
                    settingNamePattern = settingNamePattern.Replace(SettingsBaseConfiguration.NamespacePatternName,
                                                                    updatedNamespaceName);
                }

                settingNamePattern = settingNamePattern.Replace(SettingsBaseConfiguration.ClassNamePatternName,
                                                                updatedClassName);

                settingName = settingNamePattern;
            }

            var fullSettingName = SettingNameFormatInfo.AddSettingNameModeAndVersion(settingName, wishes, nameMode);
                                 
            return fullSettingName;
        }

        //------------------------------------------------------------------------------
        public static string GetFullSettingName(string settingName, ISettingsSerializerWishes wishes, Type settingsType,
                                            SettingNameMode nameMode)
        {
            return GetFullSettingNameByPatternAndAddPrefix(settingName, wishes, settingsType, nameMode);
        }

        //------------------------------------------------------------------------------
        public static string GetFullSettingName(Type settingsType,
                                                MemberInfo mi, SettingBaseAttribute baseSettingAttr,
                                                ISettingsSerializerWishes wishes,
                                                SettingNameMode nameMode)
        {
            var delimeter = GetSettingNameDelimeter(wishes);

            Debug.Assert(mi != null);
            Debug.Assert(baseSettingAttr != null);

            string settingName;
            if (string.IsNullOrEmpty(baseSettingAttr.Name))
            {
                settingName = mi.Name;
            }
            else
            {
                settingName = baseSettingAttr.Name;
                settingsType = null;
            }

            settingName = GetFullSettingNameByPatternAndAddPrefix(settingName, wishes, settingsType, nameMode);

            return settingName;
        }
    
        //------------------------------------------------------------------------------
        public static string GetFullDynamicSettingsDefaultOrderSettingName(ISettingsSerializerWishes wishes, Type settingsClassType)
        {
            const string settingName = "DynamicSettingsDefaultOrder";

            return GetFullDynamicSettingsOrderSettingNameImpl(settingName, wishes, settingsClassType);
        }

        //------------------------------------------------------------------------------
        public static string GetFullDynamicSettingsCustomOrderSettingName(ISettingsSerializerWishes wishes, Type settingsClassType)
        {
            const string settingName = "DynamicSettingsCustomOrder";

            return GetFullDynamicSettingsOrderSettingNameImpl(settingName, wishes, settingsClassType);
        }

        //------------------------------------------------------------------------------
        static string GetFullDynamicSettingsOrderSettingNameImpl(string settingName, ISettingsSerializerWishes wishes, Type settingsClassType)
        {
            var fullSettingName = GetFullSettingName(settingName, wishes, settingsClassType, SettingNameMode.Service);

            return fullSettingName;
        }

        //------------------------------------------------------------------------------
        public static string AddSettingNameModeAndVersion(string settingName, ISettingsSerializerWishes wishes, SettingNameMode nameMode)
        {
            var delimeter = GetSettingNameDelimeter(wishes);

            settingName = SettingNameFormatInfo.CurrentVersionPrefix +
                                   delimeter +
                                   settingName;

            if (nameMode == SettingNameMode.Dynamic)
            {
                settingName = SettingNameFormatInfo.DynamicSettingNamePrefix + delimeter + settingName;
            }
            else if (nameMode == SettingNameMode.Service)
            {
                settingName = SettingNameFormatInfo.ServiceSettingNamePrefix + delimeter + settingName;
            }

            return settingName;
        }

        //------------------------------------------------------------------------------
        public static string InsertSettingNamePrefix(string fullSettingName, string prefix, ISettingsSerializerWishes wishes)
        {
            var delimeter = GetSettingNameDelimeter(wishes);

            // ^(([DS]_)?v\d+_)(.*)
            var strMatch = String.Format("^(([{0}{1}]{2})?{3}\\d+{2})(.*)", 
                                         Regex.Escape(SettingNameFormatInfo.DynamicSettingNamePrefix),
                                         Regex.Escape(SettingNameFormatInfo.ServiceSettingNamePrefix),
                                         Regex.Escape(delimeter.ToString()),
                                         Regex.Escape(SettingNameFormatInfo.CurrentVersionPrefixStartPart));

            Match match = Regex.Match(fullSettingName, strMatch, RegexOptions.None);
            if (match.Success)
            {
                fullSettingName = match.Groups[1] + prefix + delimeter + match.Groups[3];

                return fullSettingName;
            }

            throw new ArgumentException(String.Format("incorrect setting name: {0}", fullSettingName));
        }
    }
}
