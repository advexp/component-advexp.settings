using System.Collections.Generic;

namespace Advexp
{
    abstract class ISettingsSerializerBaseImpl : ISettingsSerializer, IDynamicSettingsInfo, ISettingsSerializerWishes
    {
        //------------------------------------------------------------------------------
        // issue4 Default constructor not found for type Advexp.ISettingsSerializerImpl
        // https://bitbucket.org/advexp/component-advexp.settings/issues/4/default-constructor-not-found-for-type
        [Advexp.Preserve]
        public ISettingsSerializerBaseImpl()
        {
        }

        #region ISettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, bool secure, SettingBaseAttribute attr, out object value)
        {
            IInternalSettingsSerializer serializer = GetSerializer(secure);

            return serializer.Load(settingName, attr, out value);
        }

        //------------------------------------------------------------------------------
        public void Save(string settingName, bool secure, SettingBaseAttribute attr, object value)
        {
            IInternalSettingsSerializer serializer = GetSerializer(secure);

            serializer.Save(settingName, attr, value);
        }

        //------------------------------------------------------------------------------
        public void Delete(string settingName, bool secure, SettingBaseAttribute attr)
        {
            IInternalSettingsSerializer serializer = GetSerializer(secure);

            serializer.Delete(settingName, attr);
        }

        //------------------------------------------------------------------------------
        public bool Contains(string settingName, bool secure, SettingBaseAttribute attr)
        {
            IInternalSettingsSerializer serializer = GetSerializer(secure);
            bool contains = serializer.Contains(settingName, attr);

            return contains;
        }

        //------------------------------------------------------------------------------
        public void Synchronize()
        {
            // We need to sync only userdefaults
            IInternalSettingsSerializer serializer = GetSerializer(false);

            serializer.Synchronize();
        }

        #endregion

        #region IDynamicSettingsInfo implementation

        //------------------------------------------------------------------------------
        public IEnumerable<string> GetDynamicSettingsNames()
        {
            // Dynamic settings implemented only for unsecure storage
            IInternalSettingsSerializer serializer = GetSerializer(false);

            IDynamicSettingsInfo dynamicSettingsInfo = serializer as IDynamicSettingsInfo;
            if (dynamicSettingsInfo != null)
            {
                return dynamicSettingsInfo.GetDynamicSettingsNames();
            }

            return null;
        }

        #endregion

        #region ISettingsSerializerWishes implementation

        //------------------------------------------------------------------------------
        public char Delimeter()
        {
            return SettingsBaseConfiguration.DefaultDelimeter;
        }

        //------------------------------------------------------------------------------
        public string RemoveInappropriateSymbols(string text)
        {
            return SettingsSerializerUtils.RemoveInappropriateSymbols(text);
        }

        #endregion

        //------------------------------------------------------------------------------
        protected abstract IInternalSettingsSerializer GetSerializer(bool secure);

    }
}
