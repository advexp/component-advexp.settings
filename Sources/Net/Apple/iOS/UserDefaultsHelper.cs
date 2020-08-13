using System;
using Foundation;

namespace Advexp
{
    static class UserDefaultsHelper
    {
        //========================================================================
        /// <summary>
        /// Loads the default settings from the Settings.bundle/Root.plist file. Also
        /// calls nested settings (referred to in child pane items) recursively, to 
        /// load those defaults.
        /// </summary>
        public static void LoadDefaultSettings()
        {
            var prefs = NSUserDefaults.StandardUserDefaults;

            //---- check to see if they've already been loaded for the first time
            if (!prefs.BoolForKey("___DefaultsLoaded___"))
            {
#if DEBUG
                InternalConfiguration.PlatformHelper.Log(LogLevel.Info, 
                                                         "NSUserDefaults helper", 
                                                         "Load settings file for the first time");
#endif

                string rootSettingsFilePath = NSBundle.MainBundle.BundlePath + "/Settings.bundle/Root.plist";

                //---- check to see if there is event a settings file
                if (System.IO.File.Exists(rootSettingsFilePath))
                {
                    //---- load the settings
                    NSDictionary settings = NSDictionary.FromFile(rootSettingsFilePath);
                    LoadSettingsFile(settings);
                }

                //---- mark them as loaded so this doesn't run again
                prefs.SetBool(true, "___DefaultsLoaded___");
                prefs.Init();
            }
        }
        //========================================================================

        //========================================================================
        /// <summary>
        /// Recursive version of LoadDefautSetings
        /// </summary>
        private static void LoadSettingsFile(NSDictionary settings)
        {
            //---- declare vars
            bool foundTypeKey;
            bool foundDefaultValue;
            string prefKeyName;
            NSObject prefDefaultValue;
            NSObject key;

            //---- get the preference specifiers node
            NSArray prefs = settings.ObjectForKey(new NSString("PreferenceSpecifiers")) as NSArray;

            //---- loop through the settings
            for (uint i = 0; i < prefs.Count; i++)
            {
                //---- reset for each setting
                foundTypeKey = false;
                foundDefaultValue = false;
                prefKeyName = string.Empty;
                prefDefaultValue = new NSObject();

                //----
                NSDictionary pref = prefs.GetItem<NSDictionary>(i);

                //---- loop through the dictionary of any particular setting
                for (uint keyCount = 0; keyCount < pref.Keys.Length; keyCount++)
                {
                    //---- shortcut reference
                    key = pref.Keys[keyCount];

                    //---- get the key name and default value
                    if (key.ToString() == "Key")
                    {
                        foundTypeKey = true;
                        prefKeyName = pref[key].ToString();
                    }
                    else if (key.ToString() == "DefaultValue")
                    {
                        foundDefaultValue = true;
                        prefDefaultValue = pref[key];
                    }
                    else if (key.ToString() == "File")
                    {
#if DEBUG
                        InternalConfiguration.PlatformHelper.Log(LogLevel.Info,
                                                                 "NSUserDefaults helper",
                                                                 "Calling recursively");
                        InternalConfiguration.PlatformHelper.Log(LogLevel.Info, "", "<nested>");
#endif

                        NSDictionary nestedSettings = NSDictionary.FromFile(
                                        NSBundle.MainBundle.BundlePath + "/Settings.bundle/" + pref[key].ToString() + ".plist");

                        if (nestedSettings != null)
                        {
                            LoadSettingsFile(nestedSettings);
                        }

#if DEBUG
                        InternalConfiguration.PlatformHelper.Log(LogLevel.Info, "", "/<nested>");
#endif
                    }

                    //---- if we've found both, set it in our user preferences
                    if (foundTypeKey && foundDefaultValue)
                    {
                        var userPrefs = NSUserDefaults.StandardUserDefaults;
                        userPrefs[prefKeyName] = prefDefaultValue;
                    }
                }
            }
        }
    }
}
