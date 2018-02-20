using System;
using Foundation;
using MonoTouch.Dialog;
using UIKit;

namespace Sample.LocalSettings.iOS
{
    class LocalSettingsController : DialogViewController
    {
        BooleanElement m_airplaneMode = null;
        BooleanElement m_notifications = null;

        FloatElementEx m_brightness = null;
        BooleanElement m_autoBrightness = null;

        EntryElement m_login = null;
        EntryElement m_password = null;

        DateElement m_date = null;
        TimeElement m_time = null;

        BooleanElement m_bluetooth = null;

        BooleanElement m_locationService = null;

        RootElement m_autoLock = null;

        RootElement m_homeButtonDoubleClick = null;

        //------------------------------------------------------------------------------
        public LocalSettingsController() : base(null, true)
        {
            //LocalSettings.LoadSettings();

            this.Root = CreateRoot();

            NSNotificationCenter.DefaultCenter.AddObserver(
                UIApplication.DidBecomeActiveNotification,
                (notify) =>
            {
                InvokeOnMainThread(() => 
                {
                    UpdateLocalSettings();
                });
            });
        }

        //------------------------------------------------------------------------------
        RootElement CreateRoot()
        {
            var info = new MultilineElement("See the Settings.app for more details");

            m_login = new EntryElement("Login", "Your login name", 
                                       LocalSettings.Login);
            m_login.Changed += (object sender, EventArgs e) => 
            {
                LocalSettings.Login = m_login.Value;
                LocalSettings.SaveSetting(s => LocalSettings.Login);
            };

            m_password = new EntryElement("Password", "Your password", 
                                          LocalSettings.Password, 
                                          true);
            m_password.Changed += (object sender, EventArgs e) => 
            {
                LocalSettings.Password = m_password.Value;
                LocalSettings.SaveSetting(s => LocalSettings.Password);
            };

            m_airplaneMode = new BooleanElement("Airplane Mode", LocalSettings.AirplaneMode);
            m_airplaneMode.ValueChanged += (object sender, EventArgs e) => 
            {
                LocalSettings.AirplaneMode = m_airplaneMode.Value;
                LocalSettings.SaveSetting(s => LocalSettings.AirplaneMode);
            };

            m_notifications = new BooleanElement("Notifications", LocalSettings.Notifications);
            m_notifications.ValueChanged += (object sender, EventArgs e) => 
            {
                LocalSettings.Notifications = m_notifications.Value;
                LocalSettings.SaveSetting(s => LocalSettings.Notifications);
            };

            m_brightness = new FloatElementEx(LocalSettings.Brightness, 
                                              lockable: false, 
                                              valueChanged: (val) => 
            {
                LocalSettings.Brightness = val;
                LocalSettings.SaveSetting(s => LocalSettings.Brightness);
            });

            m_autoBrightness = new BooleanElement("Auto-brightness", LocalSettings.AutoBrightness);
            m_autoBrightness.ValueChanged += (object sender, EventArgs e) => 
            {
                LocalSettings.AutoBrightness = m_autoBrightness.Value;
                LocalSettings.SaveSetting(s => LocalSettings.AutoBrightness);
            };

            m_date = new DateElement("Date", LocalSettings.Date);
            m_date.DateSelected += (DateTimeElement) => 
            {
                LocalSettings.Date = m_date.DateValue;
                LocalSettings.SaveSetting(s => LocalSettings.Date);
            };

            m_time = new TimeElement("Time", LocalSettings.Time);
            m_time.DateSelected += (DateTimeElement) => 
            {
                LocalSettings.Time = m_time.DateValue;
                LocalSettings.SaveSetting(s => LocalSettings.Time);
            };

            return new RootElement("Local Settings") {
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
                    m_password,
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
            m_bluetooth = new BooleanElement("Bluetooth", LocalSettings.Bluetooth);
            m_bluetooth.ValueChanged += (object sender, EventArgs e) => 
            {
                LocalSettings.Bluetooth = m_bluetooth.Value;
                LocalSettings.SaveSetting(s => LocalSettings.Bluetooth);
            };

            m_locationService = new BooleanElement("Location Services", LocalSettings.LocationServices);
            m_locationService.ValueChanged += (object sender, EventArgs e) => 
            {
                LocalSettings.LocationServices = m_locationService.Value;
                LocalSettings.SaveSetting(s => LocalSettings.LocationServices);
            };

            Action<RadioElementEx, EventArgs> saveAutoLockDelegate = (sender, e) => 
            {
                LocalSettings.AutoLock = (AutoLock)m_autoLock.RadioSelected;
                LocalSettings.SaveSetting(s => LocalSettings.AutoLock);
            };
            m_autoLock = new RootElement("Auto-Lock", new RadioGroup((int)LocalSettings.AutoLock)) {
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
                LocalSettings.HomeButtonDoubleClick = (HomeButtonDoubleClick)m_homeButtonDoubleClick.RadioSelected;
                LocalSettings.SaveSetting(s => LocalSettings.HomeButtonDoubleClick);
            };
            m_homeButtonDoubleClick = new RootElement("Home", new RadioGroup((int)LocalSettings.HomeButtonDoubleClick)) {
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
        void UpdateLocalSettings()
        {
            LocalSettings.LoadSettings();

            m_airplaneMode.Value = LocalSettings.AirplaneMode;
            m_notifications.Value = LocalSettings.Notifications;

            m_brightness.SetValue(LocalSettings.Brightness);
            m_autoBrightness.Value = LocalSettings.AutoBrightness;

            m_login.Value = LocalSettings.Login;

            m_date.DateValue = LocalSettings.Date;
            m_time.DateValue = LocalSettings.Time;

            m_bluetooth.Value = LocalSettings.Bluetooth;

            m_locationService.Value = LocalSettings.LocationServices;

            m_autoLock.RadioSelected = (int)LocalSettings.AutoLock;

            m_homeButtonDoubleClick.RadioSelected = (int)LocalSettings.HomeButtonDoubleClick;
        }
    }
}