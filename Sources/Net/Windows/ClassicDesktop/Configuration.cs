using System.Security.Cryptography;

namespace Advexp
{
    public class SettingsConfiguration : SettingsBaseConfiguration
    {
        public static byte[] SecureSettingsAdditionalEntropy = null;
        public static DataProtectionScope SecureSettingsScope = DataProtectionScope.CurrentUser;
    }
}

