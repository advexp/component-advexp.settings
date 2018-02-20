using System;
using UIKit;
using System.Collections.Generic;
using Advexp.LocalDynamicSettings.Plugin;

namespace Sample.DynamicSettings.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Advexp.SettingsBaseConfiguration.LogLevel = Advexp.LogLevel.Info;

            var lds = DynamicSettings.GetPlugin<ILocalDynamicSettingsPlugin>();

            lds.SetDefaultSettings(new Dictionary<string, object>()
            {
                {DynamicSettings.AirplaneModeSettingName, false},
                {DynamicSettings.NotificationsSettingName, false},
                {DynamicSettings.BrightnessSettingName, 50},
                {DynamicSettings.AutoBrightnessSettingName, false},
                {DynamicSettings.LoginSettingName, "login"},
                //{DynamicSettings.PasswordSettingName, "password"}, // Secure dynamic settings are not supported yet
                {DynamicSettings.TimeSettingName, new DateTime(2000, 1, 1, 7, 15, 0, DateTimeKind.Utc)},
                {DynamicSettings.DateSettingName, new DateTime(2009, 6, 15, 0, 0, 0, DateTimeKind.Utc)},
                {DynamicSettings.BluetoothSettingName, false},
                {DynamicSettings.LocationServicesSettingName, false},
                {DynamicSettings.AutoLockSettingName, AutoLock.Never},
                {DynamicSettings.HomeButtonDoubleClickSettingName, HomeButtonDoubleClick.Home},
            });

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
