using Foundation;
using UIKit;
using MonoTouch.Dialog;
using Sample.Assembly.PCL;
using System;

namespace Sample.App.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window
        {
            get;
            set;
        }

        //------------------------------------------------------------------------------
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow (UIScreen.MainScreen.Bounds);

            AssemblyClass.InitializeAndLoadSettings();

            var rootElement = new RootElement ("Advexp.Settings PCL-iOS sample");
            var jsonSettingsElement = new MultilineElement(AssemblyClass.GetJSON());

            var boolElement = new BooleanElement("bool value", 
                                                 AssemblyClass.BoolValue);
            boolElement.ValueChanged += (object sender, System.EventArgs e) => 
            {
                AssemblyClass.BoolValue = boolElement.Value;

                UpdateJSON(rootElement, jsonSettingsElement);
                AssemblyClass.SaveSettings();
            };

            var stringElement = new EntryElement("string value", "string value", 
                                                 AssemblyClass.StringValue);
            stringElement.AutocorrectionType = UITextAutocorrectionType.No;
            stringElement.Changed += (object sender, EventArgs e) => 
            {
                AssemblyClass.StringValue = stringElement.Value;

                UpdateJSON(rootElement, jsonSettingsElement);
                AssemblyClass.SaveSettings();
            };

            var enumElement = new RootElement("enum value", new RadioGroup((int)AssemblyClass.EnumValue));

            Action<RadioElementEx, EventArgs> enumDelegate = (sender, e) => 
            {
                AssemblyClass.EnumValue = (SampleEnum)enumElement.RadioSelected;

                UpdateJSON(rootElement, jsonSettingsElement);
                AssemblyClass.SaveSettings();
            };

            var enumsSection = new RootElement("enum value", new RadioGroup((int)AssemblyClass.EnumValue)) {
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
        void UpdateJSON(RootElement rootElement, MultilineElement element)
        {
            element.Caption = AssemblyClass.GetJSON();
            rootElement.Reload(element, UITableViewRowAnimation.Automatic);
        }
    }
}


