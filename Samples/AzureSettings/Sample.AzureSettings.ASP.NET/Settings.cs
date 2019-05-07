using System;
using Advexp.AzureKeyVaultSettings.Plugin;
using Microsoft.Azure.KeyVault.Models;

namespace Sample.AzureSettings.ASP.NET
{

#error Set correct values

    [AzureKeyVaultInfo(DNSName = "{my-keyvault-dns-name}")]
    class Settings : Advexp.Settings<Settings>
    {
        [AzureKeyVaultSecret(Name = "{my-secret-name}")]
        public static SecretBundle Secret { get; set; }
    }
}
