using System;
using Microsoft.Extensions.Configuration;

namespace Sample.MicrosoftExtensionsConfiguration.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            // see https://www.paraesthesia.com/archive/2018/06/20/microsoft-extensions-configuration-deep-dive/
            // for more details about Microsoft Extentions Configuration

            Advexp.SettingsBaseConfiguration.LogLevel = Advexp.LogLevel.Debug;

            Advexp.MicrosoftExtensionsConfiguration.Plugin.Configuration.ConfigurationBuilder =
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);

            Advexp.SettingsBaseConfiguration.RegisterSettingsPlugin
            <
                Advexp.MicrosoftExtensionsConfiguration.Plugin.IMicrosoftExtensionsConfigurationPlugin,
                Advexp.MicrosoftExtensionsConfiguration.Plugin.MicrosoftExtensionsConfigurationPlugin
            >();

            Settings.LoadSettings();

            Console.WriteLine("BoolValue: {0}", Settings.BoolValue);
            Console.WriteLine("IntValue: {0}", Settings.IntValue);
            Console.WriteLine("StringValue: {0}", Settings.StringValue);
            Console.WriteLine("EnumValue: {0}", Settings.EnumValue);
            Console.WriteLine("DateTimeValue: {0}", Settings.DateTimeValue);

            if (Settings.PersonValue != null)
            {
                Console.WriteLine("PersonValue: {0}, {1}, person dateTime {2}",
                    Settings.PersonValue.FirstName, Settings.PersonValue.SecondName, Settings.PersonValue.DateTimeValue);
            }
            else
            {
                Console.WriteLine("PersonValue: null");
            }
        }
    }
}
