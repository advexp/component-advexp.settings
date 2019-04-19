using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.KeyVault.Models;
using Advexp.AzureKeyVaultSettings.Plugin;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;

namespace Sample.AzureSettings.ASP.NET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Advexp.SettingsBaseConfiguration.RegisterSettingsPlugin<IAzureKeyVaultSettingsPlugin, AzureKeyVaultSettingsPlugin>();
            Advexp.SettingsBaseConfiguration.LogLevel = Advexp.LogLevel.Debug;

            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            Advexp.AzureKeyVaultSettings.Plugin.AzureKeyVaultConfiguration.AuthenticationCallback =
                new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback);

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
