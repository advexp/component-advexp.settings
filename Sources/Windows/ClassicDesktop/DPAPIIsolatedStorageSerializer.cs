using System;
using System.Security.Cryptography;
using System.Text;


namespace Advexp
{
    class DPAPIIsolatedStorageSerializer
        : IInternalSettingsSerializer
    {
        const string SettingNamePrefix = "DPAPI_";
        InternalIsolatedStorageSerializer m_Store = new InternalIsolatedStorageSerializer();

        #region IInternalSettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, SettingBaseAttribute attr, out object value)
        {
            settingName = SettingNamePrefix + settingName;

            object encryptedValue;
            bool loaded = m_Store.Load(settingName, attr, out encryptedValue);
            if (loaded)
            {
                var encryptedBase64StringData = SettingsSerializerHelper.ConvertUsingTypeConverter<String>(encryptedValue);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64StringData);

                var entropy = SettingsConfiguration.SecureSettingsAdditionalEntropy;
                var decryptedValue = ProtectedData.Unprotect(encryptedBytes, entropy, SettingsConfiguration.SecureSettingsScope);

                value = Encoding.ASCII.GetString(decryptedValue);
            }
            else
            {
                value = null;
            }

            return loaded;
        }

        //------------------------------------------------------------------------------
        public void Save(string settingName, SettingBaseAttribute attr, object value)
        {
            settingName = SettingNamePrefix + settingName;

            var simpleValue = SettingsSerializerHelper.SimplifyObject(value);
            var stringValue = SettingsSerializerHelper.ConvertUsingTypeConverter<String>(simpleValue);

            var binaryData = Encoding.ASCII.GetBytes(stringValue);

            var entropy = SettingsConfiguration.SecureSettingsAdditionalEntropy;
            var encryptedData = 
                ProtectedData.Protect(binaryData, entropy, SettingsConfiguration.SecureSettingsScope);

            string encryptedBase64StringData = Convert.ToBase64String(encryptedData);

            m_Store.Save(settingName, attr, encryptedBase64StringData);
        }

        //------------------------------------------------------------------------------
        public void Delete(string settingName, SettingBaseAttribute attr)
        {
            m_Store.Delete(settingName, attr);
        }

        //------------------------------------------------------------------------------
        public bool Contains(string settingName, SettingBaseAttribute attr)
        {
            return m_Store.Contains(settingName, attr);
        }

        //------------------------------------------------------------------------------
        public void Synchronize()
        {
        }

        #endregion
    }
}
