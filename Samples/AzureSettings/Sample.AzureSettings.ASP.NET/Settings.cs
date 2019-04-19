using System;
using Advexp.AzureKeyVaultSettings.Plugin;
using Microsoft.Azure.KeyVault.Models;

namespace Sample.AzureSettings.ASP.NET
{
    [AzureKeyVaultInfo(DNSName = "https://advexpkeyvault.vault.azure.net/")]
    class Settings : Advexp.Settings<Settings>
    {
        [AzureKeyVaultSecret(Name = "secret")]
        public static SecretBundle Secret { get; set; }

        //[AzureKeyVaultSecret(SecretId = "https://advexpkeyvault.vault.azure.net/secrets/secret/03b1301791904fc99e97c90d869f272d")]
        //public static SecretBundle Secret2 { get; set; }
    }
}
