using System;
using Advexp.AzureKeyVaultSettings.Plugin;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Sample.AzureSettings.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Advexp.SettingsBaseConfiguration.LogLevel = Advexp.LogLevel.Debug;
            Advexp.SettingsBaseConfiguration.RegisterSettingsPlugin<IAzureKeyVaultSettingsPlugin, AzureKeyVaultSettingsPlugin>();
            KeyVaultClient.AuthenticationCallback authCallback = async (authority_, resource_, scope_) =>
            {

#error Set access credentials

                // access credentials
                string tenantId = "{my-tenant-id}";
                string applicationId = "{my-application-id}";
                string applicationSecret = "{my-application-secret}";

                string resource = "https://vault.azure.net";
                string authority = "https://login.windows.net/" + tenantId;

                var context = new AuthenticationContext(authority);
                ClientCredential credential = new ClientCredential(applicationId, applicationSecret);
                AuthenticationResult result = await context.AcquireTokenAsync(resource, credential);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to obtain token");
                }

                return result.AccessToken;
            };

            AzureKeyVaultConfiguration.AuthenticationCallback = authCallback;

            Settings1.LoadSettings();
            Settings2.LoadSettings();
            Settings3.LoadSettings();

            if (Settings1.Secret == null)
            {
                Console.WriteLine("Can`t get secret value");
            }
            else
            {
                Console.WriteLine("Secret value is: " + Settings1.Secret.Value);
            }
        }
    }
}
