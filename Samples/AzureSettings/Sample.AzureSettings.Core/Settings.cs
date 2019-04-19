using Advexp.AzureKeyVaultSettings.Plugin;
using Microsoft.Azure.KeyVault.Models;

namespace Sample.AzureSettings.Core
{
    /*
    Evaluation version of Advexp.Settings for MS Azure allow load no more then one value per time.
    To load more use full version https://advexp.bitbucket.io/
    */

    [AzureKeyVaultInfo(DNSName = "https://advexpkeyvault.vault.azure.net/")]
    class Settings1 : Advexp.Settings<Settings1>
    {
        [AzureKeyVaultSecret(Name = "secret")]
        public static SecretBundle Secret { get; set; }
    }

    [AzureKeyVaultInfo(DNSName = "https://advexpkeyvault.vault.azure.net/")]
    class Settings2 : Advexp.Settings<Settings2>
    {
        [AzureKeyVaultKey(Name = "key")]
        public static KeyBundle Key { get; set; }
    }

    [AzureKeyVaultInfo(DNSName = "https://advexpkeyvault.vault.azure.net/")]
    class Settings3 : Advexp.Settings<Settings3>
    {
        [AzureKeyVaultCertificate(Name = "certificate")]
        public static CertificateBundle Certificate { get; set; }
    }
}
