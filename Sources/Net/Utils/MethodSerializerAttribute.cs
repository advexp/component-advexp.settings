using System;

namespace Advexp
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class MethodSerializerAttribute : Attribute
    {
        // bool Load(string settingName, bool secure, SettingBaseAttribute attr, out object value)
        //------------------------------------------------------------------------------
        public string LoadMethod { get; set; }

        // void Save(string settingName, bool secure, SettingBaseAttribute attr, object value)
        //------------------------------------------------------------------------------
        public string SaveMethod { get; set; }

        // void Delete(string settingName, bool secure, SettingBaseAttribute attr)
        //------------------------------------------------------------------------------
        public string DeleteMethod { get; set; }

        // void Synchronize()
        //------------------------------------------------------------------------------
        public string SynchronizeMethod { get; set; }

        // bool Contains(string settingName, bool secure, SettingBaseAttribute attr)
        //------------------------------------------------------------------------------
        public string ContainsMethod { get; set; }

        //------------------------------------------------------------------------------
        public MethodSerializerAttribute(string loadMethod, string saveMethod,
                                         string deleteMethod = null,
                                         string synchronizeMethod = null,
                                         string containsMethod = null)
        {
            LoadMethod = loadMethod;
            SaveMethod = saveMethod;
            DeleteMethod = deleteMethod;
            SynchronizeMethod = synchronizeMethod;
            ContainsMethod = containsMethod;
        }
    }
}

