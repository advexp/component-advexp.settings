using System;
using Advexp.CognitoSyncSettings.Plugin;
using Amazon.CognitoSync.SyncManager;
using MonoTouch.Dialog;
using UIKit;
using System.Collections.Generic;
using Facebook.LoginKit;
using Advexp.DynamicSettings.Plugin;

namespace Sample.CognitoSyncSettings.iOS
{
    class SettingsViewController : DialogViewController, ICognitoSyncEventsListerner
    {
        BooleanElement m_boolElement = null;
        EntryElement m_textElement = null;
        RootElement m_enumElement = null;

        Section m_syncLabelSection = null;

        public Boolean AddLoginButton
        {
            set
            {
                if (value)
                {
                    var sse = new StyledStringElement ("Login with facebook");

                    sse.Tapped += delegate {
                        ((AppDelegate)UIApplication.SharedApplication.Delegate).Continue(new LoginViewController());
                    };

                    Root.Add(new Section() {
                        sse
                    });
                }
            }
        }

        public Boolean AddLogoutButton
        {
            set
            {
                if (value)
                {
                    var sse = new StyledStringElement ("Logout");

                    sse.Tapped += delegate {
                        
                        LoginManager login = new LoginManager();
                        login.LogOut();

                        CognitoSyncSettingsConfiguration.Credentials.Clear();

                        ((AppDelegate)UIApplication.SharedApplication.Delegate).Continue(new SettingsViewController
                        { 
                            AddLoginButton = true,
                            AddLogoutButton = false,
                            AddSyncInProgressLabel = false,
                            EnableSettingsReload = false,
                            AddSyncDatasetButton = false,
                        });
                    };

                    Root.Add(new Section() {
                        sse
                    });
                }
            }
        }

        public Boolean AddSyncDatasetButton
        {
            set
            {
                if (value)
                {
                    var sse = new StyledStringElement ("Synchronize settings");

                    sse.Tapped += delegate {
                        var plugin = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();
                        plugin.SynchronizeDataset();
                    };

                    Root.Add(new Section() {
                        sse
                    });
                }
            }
        }
        //------------------------------------------------------------------------------
        public Boolean AddSyncInProgressLabel
        {
            set
            {
                if (value)
                {
                    if (value)
                    {
                        var syncLabelElement = new StyledStringElement ("Synchronization in progress");

                        m_syncLabelSection = new Section() 
                        {
                            syncLabelElement
                        };

                        Root.Add(m_syncLabelSection);
                    }
                }
            }
        }

        //------------------------------------------------------------------------------
        public Boolean EnableSettingsReload {get; set;}

        //------------------------------------------------------------------------------
        public SettingsViewController() : base(null, true)
        {
            this.Root = CreateRoot();
        }

        //------------------------------------------------------------------------------
        RootElement CreateRoot()
        {
            // IDynamicSettingsPlugin ds = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>() as IDynamicSettingsPlugin;
            // You can use CognitoSync dynamic settings
            // Just cast plugin interface to IDynamicSettingsPlugin

            var info = new MultilineElement("See Sample.CognitoSyncSettings.Android and Sample.CognitoSyncSettings.Windows projects for more details");

            m_boolElement = new BooleanElement("Bool value", CognitoSyncSettings.Boolean);
            m_boolElement.ValueChanged += (object sender, EventArgs e) => 
            {
                CognitoSyncSettings.Boolean = m_boolElement.Value;
                CognitoSyncSettings.SaveSetting(s => CognitoSyncSettings.Boolean);
            };

            m_textElement = new EntryElement("", "Text", 
                                             CognitoSyncSettings.Text);
            m_textElement.AutocorrectionType = UITextAutocorrectionType.No;
			m_textElement.AutocapitalizationType = UITextAutocapitalizationType.None;
            m_textElement.Changed += (object sender, EventArgs e) => 
            {
                CognitoSyncSettings.Text = m_textElement.Value;
                CognitoSyncSettings.SaveSetting(s => CognitoSyncSettings.Text);
            };

            Action<RadioElementEx, EventArgs> saveAutoLockDelegate = (sender, e) => 
            {
                CognitoSyncSettings.Enum = (EEnumValues)m_enumElement.RadioSelected;
                CognitoSyncSettings.SaveSetting(s => CognitoSyncSettings.Enum);
            };

            m_enumElement = new RootElement("Enum value", new RadioGroup((int)CognitoSyncSettings.Enum)) {
                new Section() {
                    new RadioElementEx("Zero", saveAutoLockDelegate),
                    new RadioElementEx("One", saveAutoLockDelegate),
                    new RadioElementEx("Two", saveAutoLockDelegate),
                    new RadioElementEx("Three", saveAutoLockDelegate),
                    new RadioElementEx("Four", saveAutoLockDelegate),
                    new RadioElementEx("Five", saveAutoLockDelegate),
                    new RadioElementEx("Six", saveAutoLockDelegate),
                    new RadioElementEx("Seven", saveAutoLockDelegate),
                    new RadioElementEx("Eight", saveAutoLockDelegate),
                    new RadioElementEx("Nine", saveAutoLockDelegate),
                    new RadioElementEx("Ten", saveAutoLockDelegate),
                }
            };

            return new RootElement("CognitoSync Settings")
            {
                new Section()
                {
                    info
                },
                new Section()
                {
                    m_boolElement,
                    m_textElement,
                    m_enumElement,
                }
            };
        }

        //------------------------------------------------------------------------------
        public void OnSyncSuccess (object sender, SyncSuccessEventArgs e)
        {
            if (!EnableSettingsReload)
            {
                return;
            }

            Root.Remove(m_syncLabelSection);
            CognitoSyncSettings.LoadSettings();

            m_boolElement.Value = CognitoSyncSettings.Boolean;
            m_textElement.Value = CognitoSyncSettings.Text;
            m_enumElement.RadioSelected = (int)CognitoSyncSettings.Enum;
            Root.Reload(m_enumElement, UITableViewRowAnimation.Automatic);
        }

        //------------------------------------------------------------------------------
        public void OnSyncFailure (object sender, SyncFailureEventArgs e)
        {
            if (m_syncLabelSection == null)
            {
                return;
            }

            var sse = (StyledStringElement)m_syncLabelSection.Elements[0];
            sse.Value = "Synchronization failed";
			Root.Reload(sse, UITableViewRowAnimation.Automatic);
        }
    }
}