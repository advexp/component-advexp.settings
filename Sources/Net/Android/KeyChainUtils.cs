/// <summary>
/// Built using Xamarin.Auth.AndroidKeyStore
/// https://raw.githubusercontent.com/xamarin/Xamarin.Auth/master/src/Xamarin.Auth.Android/AndroidAccountStore.cs
/// </summary>

using System;
using Java.Security;
using Javax.Crypto;
using Java.IO;
using Android.Content;
using Android.Runtime;

namespace Advexp
{
    class KeyChainUtils
    {
        KeyStore _androidKeyStore;
        KeyStore.PasswordProtection _passwordProtection;
        static readonly object _fileLock = new object();
        static string _fileName = null;
        static string _serviceId = null;
        static string _keyStoreFileProtectionPassword = null;
        static char[] _fileProtectionPasswordArray = null;
        readonly Func<Context> getContext;

        public KeyChainUtils(Func<Context> context, string keyStoreFileProtectionPassword, string fileName, string serviceId)
        {
            if(String.IsNullOrEmpty(keyStoreFileProtectionPassword))
            {
                keyStoreFileProtectionPassword = String.Empty;
            }
            if(String.IsNullOrEmpty(fileName))
            {
                fileName = ".keystore";
            }
            if(String.IsNullOrEmpty(serviceId))
            {
                serviceId = "Advexp.Settings";
            }

            _keyStoreFileProtectionPassword = keyStoreFileProtectionPassword;
            _fileName = fileName;
            _serviceId = serviceId;

            _fileProtectionPasswordArray = _keyStoreFileProtectionPassword.ToCharArray();

            this.getContext = context;
            _androidKeyStore = KeyStore.GetInstance(KeyStore.DefaultType);
            _passwordProtection = new KeyStore.PasswordProtection(_fileProtectionPasswordArray);

            try
            {
                lock (_fileLock)
                {
                    using (var s = getContext().OpenFileInput(_fileName))
                    {
                        _androidKeyStore.Load(s, _fileProtectionPasswordArray);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                //ks.Load (null, Password);
                LoadEmptyKeyStore(_fileProtectionPasswordArray);
            }
        }

        /// <summary>
        /// Gets the key/password value from the keyChain.
        /// </summary>
        /// <returns>The key/password (or null if the password was not found in the KeyChain).</returns>
        /// <param name="keyName">Keyname/username.</param>
        public bool GetKey(string keyName, out string value)
        {
            var wantedAlias = MakeAlias(keyName, _serviceId);

            var aliases = _androidKeyStore.Aliases();
            while (aliases.HasMoreElements)
            {
                var alias = aliases.NextElement().ToString();
                if (alias.Contains(wantedAlias))
                {
                    var e = _androidKeyStore.GetEntry(alias, _passwordProtection) as KeyStore.SecretKeyEntry;
                    if (e != null)
                    {
                        var bytes = e.SecretKey.GetEncoded();
                        value = System.Text.Encoding.UTF8.GetString(bytes);
                        return true;
                    }
                }
            }

            value = null;

            return false;
        }

        /// <summary>
        /// Same as SetKey(name, value), but it deletes any old key before attempting to save
        /// </summary>
        /// <returns><c>true</c>, if key was saved, <c>false</c> otherwise.</returns>
        /// <param name="keyName">Key name.</param>
        /// <param name="keyValue">Key value.</param>
        public bool SaveKey(string keyName, string keyValue)
        {
            DeleteKey(keyName);

            return SetKey(keyName, keyValue);
        }

        /// <summary>
        /// Save a Key (or a Password) to the KeyChain
        /// </summary>
        /// <returns><c>true</c>, if key was saved, <c>false</c> otherwise.</returns>
        /// <param name="keyName">Key name or username.</param>
        /// <param name="keyValue">Key value or password.</param>
        public bool SetKey(string keyName, string keyValue)
        {
            var alias = MakeAlias(keyName, _serviceId);
            var secretKey = new SecretAccount(keyValue);
            var entry = new KeyStore.SecretKeyEntry(secretKey);
            _androidKeyStore.SetEntry(alias, entry, _passwordProtection);

            Save();
            return true;
        }

        /// <summary>
        /// Deletes a key (or a password) from the KeyChain.
        /// </summary>
        /// <returns><c>true</c>, if key was deleted, <c>false</c> otherwise.</returns>
        /// <param name="keyName">Key name (or username).</param>
        public bool DeleteKey(string keyName)
        {
            var alias = MakeAlias(keyName, _serviceId);

            _androidKeyStore.DeleteEntry(alias);
            Save();
            return true;
        }

        /// <summary>
        /// Check if a key is exists in the KeyChain.
        /// </summary>
        /// <returns><c>true</c>, if key is exists, <c>false</c> otherwise.</returns>
        /// <param name="keyName">Key name (or username).</param>
        public bool Contains(string keyName)
        {
            var wantedAlias = MakeAlias(keyName, _serviceId);

            var aliases = _androidKeyStore.Aliases();
            while (aliases.HasMoreElements)
            {
                var alias = aliases.NextElement().ToString();
                if (alias.Contains(wantedAlias))
                {
                    var e = _androidKeyStore.GetEntry(alias, _passwordProtection) as KeyStore.SecretKeyEntry;
                    if (e != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void Save()
        {
            lock (_fileLock)
            {
                using (var s = getContext().OpenFileOutput(_fileName, FileCreationMode.Private))
                {
                    _androidKeyStore.Store(s, _fileProtectionPasswordArray);
                }
            }
        }

        private static string MakeAlias(string username, string serviceId)
        {
            return username + "-" + serviceId;
        }

        class SecretAccount : Java.Lang.Object, ISecretKey
        {
            byte[] bytes;
            public SecretAccount(string password)
            {
                bytes = System.Text.Encoding.UTF8.GetBytes(password);
            }
            public byte[] GetEncoded()
            {
                return bytes;
            }
            public string Algorithm
            {
                get
                {
                    return "RAW";
                }
            }
            public string Format
            {
                get
                {
                    return "RAW";
                }
            }
        }

        static IntPtr id_load_Ljava_io_InputStream_arrayC;

        /// <summary>
        /// Work around Bug https://bugzilla.xamarin.com/show_bug.cgi?id=6766
        /// </summary>
        void LoadEmptyKeyStore(char[] password)
        {
            if (id_load_Ljava_io_InputStream_arrayC == IntPtr.Zero)
            {
                id_load_Ljava_io_InputStream_arrayC = JNIEnv.GetMethodID(_androidKeyStore.Class.Handle, "load", "(Ljava/io/InputStream;[C)V");
            }
            IntPtr intPtr = IntPtr.Zero;
            IntPtr intPtr2 = JNIEnv.NewArray(password);
            JNIEnv.CallVoidMethod(_androidKeyStore.Handle, id_load_Ljava_io_InputStream_arrayC, new JValue[]
                {
                    new JValue (intPtr),
                    new JValue (intPtr2)
                });
            JNIEnv.DeleteLocalRef(intPtr);
            if (password != null)
            {
                JNIEnv.CopyArray(intPtr2, password);
                JNIEnv.DeleteLocalRef(intPtr2);
            }
        }
    }
}


