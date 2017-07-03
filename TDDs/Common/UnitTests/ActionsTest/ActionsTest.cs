using Advexp;
using NUnit.Framework;

#if __IOS__
using Foundation;
using Security;
#endif // __IOS__

#if __ANDROID__
using Android.Content;
using Android.Preferences;
using Android.App;
using TDD.Android;
#endif // __ANDROID__

namespace TDD
{
    [TestFixture]
    public class ActionsTest
    {
        //------------------------------------------------------------------------------
        [TestFixtureSetUp]
        public void Setup()
        {
            SettingsConfiguration.EnableFormatMigration = false;
        }

        #if __ANDROID__

        //------------------------------------------------------------------------------
        static string Android_GetValueFromKeyChain(string sKey)
        {
            KeyChainUtils keyChainUtils = new KeyChainUtils(() => Application.Context, 
                                                                SettingsConfiguration.KeyStoreFileProtectionPassword, 
                                                                SettingsConfiguration.KeyStoreFileName, 
                                                                SettingsConfiguration.EncryptionServiceID);

            string stringValue;
            bool status = keyChainUtils.GetKey(sKey, out stringValue);
            if (!status || stringValue == null)
            {
                return null;
                //throw new Exception(String.Format("Can`t get secure setting {0}", sKey));
            }

            return stringValue;
        }

        #endif // __ANDROID__

        //------------------------------------------------------------------------------
        static public string GetSecureStringValue(string sKey)
        {
            #if __IOS__

            string sService = SettingsConfiguration.EncryptionServiceID;

            SecStatusCode eCode;
            // Query the record.
            SecRecord oQueryRec = new SecRecord(SecKind.GenericPassword) { Service = sService, Label = sService, Account = sKey };
            oQueryRec = SecKeyChain.QueryAsRecord(oQueryRec, out eCode);

            // If found, try to get password.
            if (eCode == SecStatusCode.Success && oQueryRec != null && oQueryRec.Generic != null)
            {
                // Decode from UTF8.
                return NSString.FromData(oQueryRec.Generic, NSStringEncoding.UTF8);
            }

            // Something went wrong.
            return null;

            #endif // __IOS__

            #if __ANDROID__


            var value = Android_GetValueFromKeyChain(sKey);

            return value;

            #endif // __ANDROID__
        }

        //------------------------------------------------------------------------------
        static public string GetLocalStringValue(string sKey
                                          #if __ANDROID__
                                          , ISharedPreferences prefs = null
                                          #endif // __ANDROID__
                                         )
        {
            #if __ANDROID__

            if (prefs == null)
            {
                Context context = Application.Context;
                prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            }

            string sValue = null;
            if (prefs.Contains(sKey))
            {
                sValue = prefs.GetString(sKey, null);
            }

            return sValue;

            #endif // __ANDROID__

            #if __IOS__

            var prefs = NSUserDefaults.StandardUserDefaults;
            string sValue = prefs.StringForKey(sKey);

            return sValue;

            #endif // __IOS__
        }

        //------------------------------------------------------------------------------
        [Test]
        public void LocalTest()
        {
            const string key = "v2.foo";
            const string value = "FooStringValue";

            ActionsTestSettings.LocalFooString = value;
            ActionsTestSettings.SaveSettings();

            string loadedValue = GetLocalStringValue(key);
            Assert.AreEqual(loadedValue, value);

            ActionsTestSettings.LocalFooString = null;
            ActionsTestSettings.LoadSettings();
            Assert.AreEqual(ActionsTestSettings.LocalFooString, value);

            ActionsTestSettings.DeleteSettings();
            string loadedNullNSUserDefaultsValue = GetLocalStringValue(key);
            Assert.IsNull(loadedNullNSUserDefaultsValue);
            Assert.IsNull(ActionsTestSettings.LocalFooString);
        }

        //------------------------------------------------------------------------------
        [Test]
        public void SecureTest()
        {
            const string secureKey = "v2.secureFoo";
            const string secureValue = "SecureFooStringValue";

            ActionsTestSettings.SecureFooString = secureValue;
            ActionsTestSettings.SaveSettings();

            string loadedSecureValue = ActionsTest.GetSecureStringValue(secureKey);
            Assert.AreEqual(ActionsTestSettings.SecureFooString, loadedSecureValue);
            Assert.AreEqual(secureValue, loadedSecureValue);

            ActionsTestSettings.SecureFooString = null;
            ActionsTestSettings.LoadSettings();
            Assert.AreEqual(ActionsTestSettings.SecureFooString, secureValue);

            ActionsTestSettings.DeleteSettings();
            string loadedNullSecureValue = ActionsTest.GetSecureStringValue(secureKey);
            Assert.IsNull(loadedNullSecureValue);
            Assert.IsNull(ActionsTestSettings.SecureFooString);
        }
    }
}