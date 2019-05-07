using System;
using Advexp.EnvironmentVariables.Plugin;

namespace Sample.EnvironmentVariables
{
    class Program
    {
        static void Main(string[] args)
        {
            Advexp.SettingsBaseConfiguration.LogLevel = Advexp.LogLevel.Debug;

            Advexp.SettingsBaseConfiguration.RegisterSettingsPlugin<IEnvironmentVariables, EnvironmentVariablesPlugin>();

            Console.WriteLine("--- Advexp.Settings Environment Variables ---");

            // get environment variable
            Settings.LoadSettings();
            Console.WriteLine("Settings.Path = " + Settings.Path);

            // set environment variable value
            Settings.UserVariable = "test value";
            // default EnvironmentVariableTarget is EnvironmentVariableTarget.Process
            Settings.SetSettingMetadata(s => Settings.UserVariable, Metadata.EnvironmentVariableTarget, EnvironmentVariableTarget.Process);
            Settings.SaveSetting(s => Settings.UserVariable);
            Console.WriteLine("Settings.UserVariable was saved");

            //get saved variable
            Settings.UserVariable = null;
            Settings.LoadSetting(s => Settings.UserVariable);
            Console.WriteLine("Settings.UserVariable was loaded");
            Console.WriteLine("Settings.UserVariable = " + Settings.UserVariable);
        }
    }
}
