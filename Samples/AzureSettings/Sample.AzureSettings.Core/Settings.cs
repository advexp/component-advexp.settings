using Advexp.AzureKeyVaultSettings.Plugin;
using Microsoft.Azure.KeyVault.Models;

namespace Sample.AzureSettings.Core
{
    /*
    Evaluation version of Advexp.Settings for MS Azure allow load no more then one value per time.
    Use full version instead https://advexp.bitbucket.io/
    */

    [AzureKeyVaultInfo(DNSName = "{my-keyvault-dns-name}")]
    class Settings1 : Advexp.Settings<Settings1>
    {
        [AzureKeyVaultSecret(Name = "{my-secret-name}")]
        public static SecretBundle Secret { get; set; }
    }

    [AzureKeyVaultInfo(DNSName = "{my-keyvault-dns-name}")]
    class Settings2 : Advexp.Settings<Settings2>
    {
        [AzureKeyVaultKey(Name = "{my-key-name}")]
        public static KeyBundle Key { get; set; }
    }

    [AzureKeyVaultInfo(DNSName = "{my-keyvault-dns-name}")]
    class Settings3 : Advexp.Settings<Settings3>
    {
        [AzureKeyVaultCertificate(Name = "{my-certificate-name}")]
        public static CertificateBundle Certificate { get; set; }
    }
}
