using Foundation;
using UIKit;
using MonoTouch.Dialog;
using Advexp.JSONSettings.Plugin;
using Advexp.LocalDynamicSettings.Plugin;
using System;

namespace Sample.JSONSettings.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        //------------------------------------------------------------------------------
        public override UIWindow Window
        {
            get;
            set;
        }

        //------------------------------------------------------------------------------
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow (UIScreen.MainScreen.Bounds);

            Settings.LoadSettings();

            var dynamicTextSettingName = "dynamic_string";

            var ds = Settings.GetPlugin<ILocalDynamicSettingsPlugin>();
            ds.SetSetting(dynamicTextSettingName, Settings.String);

            var plugin = Settings.GetPlugin<IJSONSettingsPlugin>();
            var json = plugin.SaveSettingsToJSON();

            var rootElement = new RootElement("Advexp.Settings JSON sample");

            var jsonSettingsElement = new MultilineElement(json);

            var boolElement = new BooleanElement("bool value", Settings.Bool);
            boolElement.ValueChanged += (object sender, System.EventArgs e) => 
            {
                Settings.Bool = boolElement.Value;

                UpdateJSON(rootElement, jsonSettingsElement);
                Settings.SaveSetting(s => Settings.Bool);
            };

            var stringElement = new EntryElement("string value", "string value", Settings.String);
            stringElement.AutocorrectionType = UITextAutocorrectionType.No;
            stringElement.Changed += (object sender, EventArgs e) => 
            {
                Settings.String = stringElement.Value;

                ds.SetSetting(dynamicTextSettingName, stringElement.Value);

                UpdateJSON(rootElement, jsonSettingsElement);
                Settings.SaveSetting(s => Settings.String);
            };

            var enumElement = new RootElement("enum value", new RadioGroup((int)Settings.Enum));

            Action<RadioElementEx, EventArgs> enumDelegate = (sender, e) => 
            {
                Settings.Enum = (SampleEnum)enumElement.RadioSelected;

                UpdateJSON(rootElement, jsonSettingsElement);
                Settings.SaveSetting(s => Settings.Enum);
            };

            var enumsSection = new RootElement("enum value", new RadioGroup((int)Settings.Enum)) {
                new Section() {
                    new RadioElementEx("Zero", enumDelegate),
                    new RadioElementEx("One", enumDelegate),
                    new RadioElementEx("Two", enumDelegate),
                    new RadioElementEx("Three", enumDelegate),
                    new RadioElementEx("Four", enumDelegate),
                    new RadioElementEx("Five", enumDelegate),
                    new RadioElementEx("Six", enumDelegate),
                    new RadioElementEx("Seven", enumDelegate),
                    new RadioElementEx("Eight", enumDelegate),
                    new RadioElementEx("Nine", enumDelegate),
                    new RadioElementEx("Ten", enumDelegate),
                }
            };

            enumElement.Add(enumsSection);

            var rootSection = new Section () {
                boolElement,
                stringElement,
                enumElement,
                jsonSettingsElement,
            };

            rootElement.Add(rootSection);
                
            var rootVC = new DialogViewController(rootElement);
            var nav = new UINavigationController(rootVC);

            Window.RootViewController = nav;
            Window.MakeKeyAndVisible ();

            return true;
        }

        //------------------------------------------------------------------------------
        void UpdateJSON(RootElement rootElement, Element element)
        {
            var plugin = Settings.GetPlugin<IJSONSettingsPlugin>();

            var json = plugin.SaveSettingsToJSON();
            element.Caption = json;

            rootElement.Reload(element, UITableViewRowAnimation.Automatic);
        }
    }
}


