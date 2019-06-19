using Foundation;
using UIKit;
using MonoTouch.Dialog;
using System;
using System.Collections.Generic;
using Advexp.iCloudSettings.Plugin;
using Advexp.DynamicSettings.Plugin;
using Advexp.Sample.iCloud;

namespace Sample.iCloudSettings.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // notification when Key-Value changes are triggered by server
        NSObject m_iCloudNotification;

        //------------------------------------------------------------------------------
        public override UIWindow Window
        {
            get;
            set;
        }

        //------------------------------------------------------------------------------
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            //var store = NSUbiquitousKeyValueStore.DefaultStore;
            //bool result = store.Synchronize();

            Settings.LoadSettings();

            Window = new UIWindow (UIScreen.MainScreen.Bounds);

            var rootElement = new RootElement("Advexp.Settings iCloud sample");

            bool boolChangeLock = false;
            var boolElement = new BooleanElement("bool value", Settings.Bool);
            boolElement.ValueChanged += (object sender, System.EventArgs e) => 
            {
                if (!boolChangeLock)
                {
                    Settings.Bool = boolElement.Value;
                    Settings.SaveSetting(s => Settings.Bool);
                }
            };

            bool stringChangeLock = false;
            var stringElement = new EntryElement("string value", "string value", Settings.Text);
            stringElement.AutocorrectionType = UITextAutocorrectionType.No;
            stringElement.Changed += (object sender, EventArgs e) => 
            {
                if (!stringChangeLock)
                {
                    Settings.Text = stringElement.Value;
                    Settings.SaveSetting(s => Settings.Text);
                }
            };

            var staticSection = new Section ("Static settings") {
                boolElement,
                stringElement,
            };

            var iCloudPlugin = Settings.GetPlugin<IiCloudSettingsPlugin>();
            var iCloudDynamicSettings = (IDynamicSettingsPlugin)iCloudPlugin;

            bool checkChangeLock = false;
            Action<CheckboxElementEx, EventArgs> checkDelegate = (sender, e) =>
            {
                if (!checkChangeLock)
                {
                    iCloudDynamicSettings.SetSetting(sender.Caption, sender.Value);
                    iCloudDynamicSettings.SaveSetting(sender.Caption);
                }
            };

            var value1 = new CheckboxElementEx("value1", iCloudDynamicSettings.GetSetting<bool>("value1", false), checkDelegate);
            var value2 = new CheckboxElementEx("value2", iCloudDynamicSettings.GetSetting<bool>("value2", false), checkDelegate);
            var value3 = new CheckboxElementEx("value3", iCloudDynamicSettings.GetSetting<bool>("value3", false), checkDelegate);
            var value4 = new CheckboxElementEx("value4", iCloudDynamicSettings.GetSetting<bool>("value4", false), checkDelegate);
            var value5 = new CheckboxElementEx("value5", iCloudDynamicSettings.GetSetting<bool>("value5", false), checkDelegate);
            var value6 = new CheckboxElementEx("value6", iCloudDynamicSettings.GetSetting<bool>("value6", false), checkDelegate);

            var dynamicSection = new Section("Dynamic settings") {
                value1,
                value2,
                value3,
                value4,
                value5,
                value6,
            };

            rootElement.Add(staticSection);
            rootElement.Add(dynamicSection);

            var rootVC = new DialogViewController(rootElement);
            var nav = new UINavigationController(rootVC);

            Window.RootViewController = nav;
            Window.MakeKeyAndVisible ();

            m_iCloudNotification =
                NSNotificationCenter.DefaultCenter.AddObserver(
                    NSUbiquitousKeyValueStore.DidChangeExternallyNotification, notification =>
                    {
                        Console.WriteLine("iCloud notification received - iOS");

                        InvokeOnMainThread(() =>
                        {
                            try
                            {
                                boolChangeLock = true;
                                stringChangeLock = true;
                                checkChangeLock = true;

                                var store = NSUbiquitousKeyValueStore.DefaultStore;
                                store.Synchronize();

                                Settings.LoadSettings();

                                boolElement.Value = Settings.Bool;
                                stringElement.Value = Settings.Text;

                                value1.Value = iCloudDynamicSettings.GetSetting<bool>("value1", false);
                                value2.Value = iCloudDynamicSettings.GetSetting<bool>("value2", false);
                                value3.Value = iCloudDynamicSettings.GetSetting<bool>("value3", false);
                                value4.Value = iCloudDynamicSettings.GetSetting<bool>("value4", false);
                                value5.Value = iCloudDynamicSettings.GetSetting<bool>("value5", false);
                                value6.Value = iCloudDynamicSettings.GetSetting<bool>("value6", false);
                            }
                            finally
                            {
                                boolChangeLock = false;
                                stringChangeLock = false;
                                checkChangeLock = false;
                            }

                            rootVC.ReloadData();
                        });
                    });

            return true;
        }

        public override void WillTerminate(UIApplication application)
        {
            if (m_iCloudNotification != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(m_iCloudNotification);
            }
        }
    }
}


