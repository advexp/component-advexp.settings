using System;
using System.Collections.Generic;
using System.Reflection;
using Gma.DataStructures;

namespace Advexp
{
    class SettingsObjectInfo
    {
        public object Settings { get; set; }
        public ISettingsSerializer ForegroundSerializer { get; set; }
        public MemberInfo MemberInfo { get; set; }
        public Type SettingAttributeType { get; set; }
    }

    class SettingInfo
    {
        public String SettingName { get; set; }
        public object SettingValue { get; set; }
        public Type SettingValueType { get; set; }
        public object SettingDefaultValue { get; set; }

        //------------------------------------------------------------------------------
        public SettingInfo()
        {
        }

        //------------------------------------------------------------------------------
        public SettingInfo(String settingName, DynamicSettingInfo dynamicSettingInfo)
        {
            this.SettingName = settingName;
            this.SettingValue = dynamicSettingInfo.SettingValue;
            this.SettingValueType = dynamicSettingInfo.SettingType;
            this.SettingDefaultValue = null;
        }
    }

    class SettingsHelperData
    {
        public Dictionary<Type, KeyValuePair<ISettingsSerializer, bool>> SerializersCache;
        public OrderedSet<ISettingsSerializer> SerializersToSync;
    }
}
