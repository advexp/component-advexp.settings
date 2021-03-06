using System;
using Security;
using System.Text;

// only in paid version of plugin or local library
// or dev version
#if !__PLUGIN__ || !__EVALUATION__ || __DEV__

namespace Advexp
{
    class KeyChainSerializer
        : IInternalSettingsSerializer
    {
        readonly string m_serviceName;
        readonly SecAccessible m_secAccessible;
        readonly bool m_synchronizable;

        //------------------------------------------------------------------------------
        public KeyChainSerializer(SecAccessible secAccessible, bool synchronizable)
        {
            m_serviceName = SettingsBaseConfiguration.EncryptionServiceID;
            m_secAccessible = secAccessible;
            m_synchronizable = synchronizable;
        }


        #region IInternalSettingsSerializer implementation
        //------------------------------------------------------------------------------
        public bool Load(string settingName, SettingBaseAttribute attr, out object value)
        {
            if (!KeyChainUtils.Contains(settingName, m_serviceName, m_synchronizable))
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "KeyChainSerializer.Load - no such setting",
                                                         settingName);

                value = null;
                return false;
            }

            string stringValue;
            SecStatusCode eCode = KeyChainUtils.GetPasswordForUsername(
                settingName,
                m_serviceName,
                out stringValue,
                m_synchronizable);

            if (eCode != SecStatusCode.Success || stringValue == null)
            {
                Advexp.LogLevel ll = Advexp.LogLevel.None;

                if (eCode == SecStatusCode.ItemNotFound)
                {
                    ll = LogLevel.Info;
                }
                else
                {
                    ll = LogLevel.Error;
                }

                InternalConfiguration.PlatformHelper.Log(ll,
                                                         "KeyChainSerializer.Load - error",
                                                         String.Format("Setting is {0}. Error code is {1}",
                                                                       settingName,
                                                                       eCode));
                value = null;
                return false;
            }

            // in this case simplified object will be corrected in the SettingsSerializer.cs
            value = stringValue;

            InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                     "KeyChainSerializer.Load - success",
                                                     settingName);

            return true;
        }

        //------------------------------------------------------------------------------
        public void Save(string settingName, SettingBaseAttribute attr, object value)
        {
            var simpleValue = SettingsSerializerHelper.SimplifyObject(value);
            var stringValue = SettingsSerializerHelper.ConvertUsingTypeConverter<String>(simpleValue);

            SecStatusCode eCode = KeyChainUtils.SetPasswordForUsername(
                settingName,
                stringValue,
                m_serviceName,
                m_secAccessible,
                m_synchronizable);

            if (eCode == SecStatusCode.Success)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "KeyChainSerializer.Save - success",
                                                         settingName);
            }
            else
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Error,
                                                         "KeyChainSerializer.Save - error",
                                                         String.Format("Setting name is {0}. Error code is {1}",
                                                                       settingName,
                                                                       eCode));
            }

            // keychain error handling from Xamarin.Auth
            // https://github.com/xamarin/Xamarin.Auth/blob/master/source/Core/Xamarin.Auth.XamarinIOS/AccountStore/KeyChainAccountStore.Aync.cs
            if (eCode != SecStatusCode.Success)
            {
                StringBuilder sb = new StringBuilder("error = ");
                sb.AppendLine("Could not save value to KeyChain: " + eCode);
                sb.AppendLine("Add Empty Entitlements.plist ");
                sb.AppendLine("File /+ New file /+ iOS /+ Entitlements.plist");

                /*
                    Error: Could not save account to KeyChain -- iOS 10 #128
                    https://github.com/xamarin/Xamarin.Auth/issues/128 
                    https://bugzilla.xamarin.com/show_bug.cgi?id=43514
                */
                if ((int)eCode == -34018)
                {
                    // http://stackoverflow.com/questions/38456471/secitemadd-always-returns-error-34018-in-xcode-8-in-ios-10-simulator
                    // NOTE: code was not copy/pasted! That was iOS sample

                    sb.AppendLine("SecKeyChain.Add returned : " + eCode);
                    sb.AppendLine("1. Add Keychain Access Groups to the Entitlements file.");
                    sb.AppendLine("2. Turn on the Keychain Sharing switch in the Capabilities section in the app.");
                }

                string msg = sb.ToString();

                throw new ExceptionForUser(new Exception(msg));
            }
        }

        //------------------------------------------------------------------------------
        public void Delete(string settingName, SettingBaseAttribute attr)
        {
            SecStatusCode eCode = KeyChainUtils.DeletePasswordForUsername(
                settingName,
                m_serviceName,
                m_synchronizable);

            if (eCode == SecStatusCode.Success)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "KeyChainSerializer.Delete - success",
                                                         settingName);
            }
            else
            {
                bool contains = KeyChainUtils.Contains(settingName, m_serviceName, m_synchronizable);
                if (contains)
                {
                    InternalConfiguration.PlatformHelper.Log(LogLevel.Error,
                                                             "KeyChainSerializer.Delete error",
                                                             String.Format("Setting name is {0}. Error code is {1}",
                                                                           settingName,
                                                                           eCode));
                }
                else
                {
                    InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                             "KeyChainSerializer.Delete - no such setting",
                                                             settingName);
                }
            }
        }

        //------------------------------------------------------------------------------
        public bool Contains(string settingName, SettingBaseAttribute attr)
        {
            bool contains = KeyChainUtils.Contains(
                settingName,
                m_serviceName,
                m_synchronizable);

            if (contains)
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "KeyChainSerializer.Contains - setting exists",
                                                         settingName);
            }
            else
            {
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                         "KeyChainSerializer.Contains - no such setting",
                                                         settingName);
            }

            return contains;
        }

        //------------------------------------------------------------------------------
        public void Synchronize()
        {
        }

        #endregion
    }
}

#endif // !__PLUGIN__ || !__EVALUATION__ || __DEV__
