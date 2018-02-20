using System;
using Foundation;
using MonoTouch.Dialog;
using UIKit;
using Advexp.LocalDynamicSettings.Plugin;

namespace Sample.DynamicSettings.iOS
{
    class DynamicSettingsController : DialogViewController
    {
        BooleanElement m_airplaneMode = null;
        BooleanElement m_notifications = null;

        FloatElementEx m_brightness = null;
        BooleanElement m_autoBrightness = null;

        EntryElement m_login = null;

        DateElement m_date = null;
        TimeElement m_time = null;

        BooleanElement m_bluetooth = null;

        BooleanElement m_locationService = null;

        RootElement m_autoLock = null;

        RootElement m_homeButtonDoubleClick = null;

        //------------------------------------------------------------------------------
        public DynamicSettingsController() : base(null, true)
        {
            var lds = DynamicSettings.GetPlugin<ILocalDynamicSettingsPlugin>();
            lds.LoadSettings();

            this.Root = CreateRoot();

            NSNotificationCenter.DefaultCenter.AddObserver(
                UIApplication.DidBecomeActiveNotification,
                (notify) =>
            {
                InvokeOnMainThread(() => 
                {
                    UpdateDynamicSettings();
                });
            });
        }

        //------------------------------------------------------------------------------
        RootElement CreateRoot()
        {
            var lds = DynamicSettings.GetPlugin<ILocalDynamicSettingsPlugin>();

            var info = new MultilineElement("See the Settings.app for more details");

            m_login = new EntryElement("Login", "Your login name", 
                                       lds.GetSetting<string>(DynamicSettings.LoginSettingName));
            m_login.Changed += (object sender, EventArgs e) => 
            {
                lds.SetSetting(DynamicSettings.LoginSettingName, m_login.Value);
                lds.SaveSetting(DynamicSettings.LoginSettingName);
            };

            m_airplaneMode = new BooleanElement("Airplane Mode", 
                                                lds.GetSetting<bool>(DynamicSettings.AirplaneModeSettingName));
            m_airplaneMode.ValueChanged += (object sender, EventArgs e) => 
            {
                lds.SetSetting(DynamicSettings.AirplaneModeSettingName, m_airplaneMode.Value);
                lds.SaveSetting(DynamicSettings.AirplaneModeSettingName);
            };

            m_notifications = new BooleanElement("Notifications", lds.GetSetting<bool>(DynamicSettings.NotificationsSettingName));
            m_notifications.ValueChanged += (object sender, EventArgs e) => 
            {
                lds.SetSetting(DynamicSettings.NotificationsSettingName, m_notifications.Value);
                lds.SaveSetting(DynamicSettings.NotificationsSettingName);
            };

            m_brightness = new FloatElementEx(lds.GetSetting<int>(DynamicSettings.BrightnessSettingName), 
                                              lockable: false, 
                                              valueChanged: (val) => 
            {
                lds.SetSetting(DynamicSettings.BrightnessSettingName, val);
                lds.SaveSetting(DynamicSettings.BrightnessSettingName);
            });

            m_autoBrightness = new BooleanElement("Auto-brightness", lds.GetSetting<bool>(DynamicSettings.AutoBrightnessSettingName));
            m_autoBrightness.ValueChanged += (object sender, EventArgs e) => 
            {
                lds.SetSetting(DynamicSettings.AutoBrightnessSettingName, m_autoBrightness.Value);
                lds.SaveSetting(DynamicSettings.AutoBrightnessSettingName);
            };

            m_date = new DateElement("Date", lds.GetSetting<DateTime>(DynamicSettings.DateSettingName));
            m_date.DateSelected += (DateTimeElement) => 
            {
                lds.SetSetting(DynamicSettings.DateSettingName, m_date.DateValue);
                lds.SaveSetting(DynamicSettings.DateSettingName);
            };

            m_time = new TimeElement("Time", lds.GetSetting<DateTime>(DynamicSettings.TimeSettingName));
            m_time.DateSelected += (DateTimeElement) => 
            {
                lds.SetSetting(DynamicSettings.TimeSettingName, m_time.DateValue);
                lds.SaveSetting(DynamicSettings.TimeSettingName);
            };

            return new RootElement("Dynamic Settings") {
                new Section() {
                    info
                },
                new Section() {
                    m_airplaneMode,
                    new RootElement("Notifications", 0, 0) {
                        new Section(null, "Turn off Notifications to disable Sounds " +
                                    "Alerts and Home Screen Badges for the applications below.") {
                            m_notifications
                        }
                    }
                },
                new Section() {
                    new RootElement("Brightness") {
                        new Section() {
                            m_brightness,
                            m_autoBrightness,
                        }
                    },
                },
                new Section() {
                    m_login,
                },
                new Section() {
                    m_date,
                    m_time,
                },
                new Section() {
                    CreateGeneralSection(),
                }
            };
        }

        //------------------------------------------------------------------------------
        RootElement CreateGeneralSection()
        {
            var lds = DynamicSettings.GetPlugin<ILocalDynamicSettingsPlugin>();

            m_bluetooth = new BooleanElement("Bluetooth", lds.GetSetting<bool>(DynamicSettings.BluetoothSettingName));
            m_bluetooth.ValueChanged += (object sender, EventArgs e) => 
            {
                lds.SetSetting(DynamicSettings.BluetoothSettingName, m_bluetooth.Value);
                lds.SaveSetting(DynamicSettings.BluetoothSettingName);
            };

            m_locationService = new BooleanElement("Location Services", lds.GetSetting<bool>(DynamicSettings.LocationServicesSettingName));
            m_locationService.ValueChanged += (object sender, EventArgs e) => 
            {
                lds.SetSetting(DynamicSettings.LocationServicesSettingName, m_locationService.Value);
                lds.SaveSetting(DynamicSettings.LocationServicesSettingName);
            };

            Action<RadioElementEx, EventArgs> saveAutoLockDelegate = (sender, e) => 
            {
                lds.SetSetting(DynamicSettings.AutoLockSettingName, (AutoLock)m_autoLock.RadioSelected);
                lds.SaveSetting(DynamicSettings.AutoLockSettingName);
            };
            m_autoLock = new RootElement("Auto-Lock", new RadioGroup((int)lds.GetSetting<AutoLock>(DynamicSettings.AutoLockSettingName))) {
                new Section() {
                    new RadioElementEx("Never", saveAutoLockDelegate),
                    new RadioElementEx("1 Minute", saveAutoLockDelegate),
                    new RadioElementEx("2 Minutes", saveAutoLockDelegate),
                    new RadioElementEx("3 Minutes", saveAutoLockDelegate),
                    new RadioElementEx("4 Minutes", saveAutoLockDelegate),
                    new RadioElementEx("5 Minutes", saveAutoLockDelegate),
                }
            };

            Action<RadioElementEx, EventArgs> saveHomeButtonDoubleClickDelegate = (sender, e) => 
            {
                lds.SetSetting(DynamicSettings.HomeButtonDoubleClickSettingName, (HomeButtonDoubleClick)m_homeButtonDoubleClick.RadioSelected);
                lds.SaveSetting(DynamicSettings.HomeButtonDoubleClickSettingName);
            };
            m_homeButtonDoubleClick = new RootElement("Home", new RadioGroup((int)lds.GetSetting<HomeButtonDoubleClick>(DynamicSettings.HomeButtonDoubleClickSettingName))) {
                new Section("Double-click the Home Button for:") {
                    new RadioElementEx("Home", saveHomeButtonDoubleClickDelegate),
                    new RadioElementEx("Search", saveHomeButtonDoubleClickDelegate),
                    new RadioElementEx("Phone favorites", saveHomeButtonDoubleClickDelegate),
                    new RadioElementEx("Camera", saveHomeButtonDoubleClickDelegate),
                    new RadioElementEx("iPod", saveHomeButtonDoubleClickDelegate),
                }
            };

            return new RootElement("General") {
                new Section() {
                    new RootElement("Bluetooth", 0, 0) {
                        new Section() {
                            m_bluetooth
                        }
                    },
                    m_locationService,
                },
                new Section() {
                    m_autoLock,
                    m_homeButtonDoubleClick,
                },
            };
        }

        //------------------------------------------------------------------------------
        void UpdateDynamicSettings()
        {
            var lds = DynamicSettings.GetPlugin<ILocalDynamicSettingsPlugin>();

            lds.LoadSettings();

            m_airplaneMode.Value = lds.GetSetting<bool>(DynamicSettings.AirplaneModeSettingName);
            m_notifications.Value = lds.GetSetting<bool>(DynamicSettings.NotificationsSettingName);

            m_brightness.SetValue(lds.GetSetting<int>(DynamicSettings.BrightnessSettingName));
            m_autoBrightness.Value = lds.GetSetting<bool>(DynamicSettings.AutoBrightnessSettingName);

            m_login.Value = lds.GetSetting<string>(DynamicSettings.LoginSettingName);

            m_date.DateValue = lds.GetSetting<DateTime>(DynamicSettings.DateSettingName);
            m_time.DateValue = lds.GetSetting<DateTime>(DynamicSettings.TimeSettingName);

            m_bluetooth.Value = lds.GetSetting<bool>(DynamicSettings.BluetoothSettingName);

            m_locationService.Value = lds.GetSetting<bool>(DynamicSettings.LocationServicesSettingName);

            m_autoLock.RadioSelected = (int)lds.GetSetting<AutoLock>(DynamicSettings.AutoLockSettingName);

            m_homeButtonDoubleClick.RadioSelected = (int)lds.GetSetting<HomeButtonDoubleClick>(DynamicSettings.HomeButtonDoubleClickSettingName);
        }
    }
}