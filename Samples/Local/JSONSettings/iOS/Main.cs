﻿using UIKit;
using Advexp.JSONSettings.Plugin;

namespace Sample.JSONSettings.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Advexp.SettingsModuleInitializer.Initialize();
            Advexp.BaseSettingsConfiguration.EnablePlugin<IJSONSettingsPlugin, JSONSettingsPlugin>();

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}