using Advexp.CognitoSyncSettings.Plugin;
using Advexp.DynamicSettings.Plugin;
using Amazon.CognitoSync.SyncManager;
using Facebook;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Sample.CognitoSyncSettings.Windows
{
    public partial class Form1 : Form
    {
        enum LoginProvider
        {
            Unknown,
            Facebook,
            Google,
        };

        bool m_enableEnumComboBoxEvent = false;
        int m_textCounter = -1;

        string m_AccessToken = null;

        LoginProvider m_ProviderName = LoginProvider.Unknown;

        public Form1()
        {
            // plugins are persistant and next call will return the same object
            var plugin = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();

            plugin.OnSyncSuccess += (object sender, SyncSuccessEventArgs e) =>
            {
                m_enableEnumComboBoxEvent = false;
                Invoke(new MethodInvoker(UpdateUI));
                m_enableEnumComboBoxEvent = true;
            };

            m_enableEnumComboBoxEvent = false;

            InitializeComponent();

            var enumDataSource = new List<string>(Enum.GetNames(typeof(EEnumValues)));
            this.enumValue.DataSource = enumDataSource;

            UpdateUI();

            m_enableEnumComboBoxEvent = true;
        }

        void UpdateUI()
        {
            this.UIThread(() => 
            {
                CognitoSyncSettings.LoadSettings();

                // set static settings
                this.boolValue.Checked = CognitoSyncSettings.Boolean;

                if (m_textCounter <= 0)
                {
                    this.textValue.Text = CognitoSyncSettings.Text;
                }
                else
                {
                    m_textCounter--;
                }
                
                this.enumValue.SelectedIndex = (int)CognitoSyncSettings.Enum;

                // set dynamic settings
                var cognitoSyncSettings = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();
                var dynamicCognitoSyncSettings = (IDynamicSettingsPlugin)cognitoSyncSettings;

                dynamicCognitoSyncSettings.LoadSettings();

                SetDynamicCheckBox(this.dynamicValue1, dynamicCognitoSyncSettings);
                SetDynamicCheckBox(this.dynamicValue2, dynamicCognitoSyncSettings);
                SetDynamicCheckBox(this.dynamicValue3, dynamicCognitoSyncSettings);
                SetDynamicCheckBox(this.dynamicValue4, dynamicCognitoSyncSettings);
                SetDynamicCheckBox(this.dynamicValue5, dynamicCognitoSyncSettings);
                SetDynamicCheckBox(this.dynamicValue6, dynamicCognitoSyncSettings);
            });
        }

        private void textValue_TextChanged(object sender, EventArgs e)
        {
            m_textCounter++;

            CognitoSyncSettings.Text = this.textValue.Text;
            CognitoSyncSettings.SaveSetting(s => CognitoSyncSettings.Text);
        }

        private void dynamicValue1_CheckedChanged(object sender, EventArgs e)
        {
            SetDynamicSettingValue(this.dynamicValue1);
        }

        private void dynamicValue2_CheckedChanged(object sender, EventArgs e)
        {
            SetDynamicSettingValue(this.dynamicValue2);
        }

        private void dynamicValue3_CheckedChanged(object sender, EventArgs e)
        {
            SetDynamicSettingValue(this.dynamicValue3);
        }

        private void dynamicValue4_CheckedChanged(object sender, EventArgs e)
        {
            SetDynamicSettingValue(this.dynamicValue4);
        }

        private void dynamicValue5_CheckedChanged(object sender, EventArgs e)
        {
            SetDynamicSettingValue(this.dynamicValue5);
        }

        private void dynamicValue6_CheckedChanged(object sender, EventArgs e)
        {
            SetDynamicSettingValue(this.dynamicValue6);
        }

        void facebookLogin_Click(object sender, EventArgs e)
        {
            const string extendedPermissions = "public_profile";
            var fbLoginDlg = new FacebookLoginDialog(Constants.FacebookAppId, extendedPermissions);
            fbLoginDlg.ShowDialog();

            if (TakeFacebookLoggedInAction(fbLoginDlg.FacebookOAuthResult))
            {
                MakeLoginVisible(false);
            }
        }

        void googleLogin_Click(object sender, EventArgs e)
        {
            var gl = new GoogleLogin();
            gl.doOAuth((authToken) => 
            {
                if (TakeGoogleLoggedInAction(authToken))
                {
                    MakeLoginVisible(false);
                }
            });
        }

        private void logout_Click(object sender, EventArgs e)
        {
            switch (m_ProviderName)
            {
                case LoginProvider.Facebook:
                    {
                        var fb = new FacebookClient();

                        var logoutUrl = fb.GetLogoutUrl(new
                        {
                            next = "https://www.facebook.com/connect/login_success.html",
                            access_token = m_AccessToken
                        });

                        var webBrowser = new WebBrowser();
                        webBrowser.Navigate(logoutUrl.AbsoluteUri);

                        break;
                    }
                case LoginProvider.Google:
                    {
                        var logoutUrl = new Uri("https://www.google.com/accounts/Logout?continue=https://appengine.google.com/_ah/logout?continue=https://advexp.bitbucket.io/");

                        System.Diagnostics.Process.Start(logoutUrl.AbsoluteUri);

                        break;
                    }
            }

            CognitoSyncSettingsConfiguration.Credentials.Clear();

            m_AccessToken = null;
            m_ProviderName = LoginProvider.Unknown;

            MakeLoginVisible(true);
        }

        void MakeLoginVisible(bool loginVisible)
        {
            this.facebookLogin.Visible = loginVisible;
            this.googleLogin.Visible = loginVisible;
            this.logout.Visible = !loginVisible;

            this.synchronize.Visible = !loginVisible;
        }

        private bool TakeFacebookLoggedInAction(FacebookOAuthResult facebookOAuthResult)
        {
            if (facebookOAuthResult == null)
            {
                // the user closed the FacebookLoginDialog, so do nothing.
                MessageBox.Show("Cancelled!");
            }
            else if (facebookOAuthResult.IsSuccess)
            {
                m_AccessToken = facebookOAuthResult.AccessToken;
                m_ProviderName = LoginProvider.Facebook;

                CognitoSyncSettingsConfiguration.Credentials.Clear();
                CognitoSyncSettingsConfiguration.Credentials.AddLogin(Constants.CognitoSyncProviderName_Facebook, m_AccessToken);

                // plugins are persistant and next call return the same object
                var plugin = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();
                plugin.SynchronizeDataset();

                return true;
            }
            else
            {
                MessageBox.Show(facebookOAuthResult.ErrorDescription);
            }

            return false;
        }

        bool TakeGoogleLoggedInAction(string token)
        {
            if (!String.IsNullOrEmpty(token))
            {
                m_AccessToken = token;
                m_ProviderName = LoginProvider.Google;

                CognitoSyncSettingsConfiguration.Credentials.Clear();
                CognitoSyncSettingsConfiguration.Credentials.AddLogin(Constants.CognitoSyncProviderName_Google, token);

                // plugins are persistant and next call return the same object
                var plugin = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();
                plugin.SynchronizeDataset();

                return true;
            }
            else
            {
                MessageBox.Show("Incorrect access token.");
            }

            return false;
        }

        void SetDynamicCheckBox(CheckBox checkBox, IDynamicSettingsPlugin ds)
        {
            var settingName = checkBox.Name;
            bool value = ds.GetSetting<bool>(settingName, false);

            checkBox.Checked = value;
        }

        void SetDynamicSettingValue(CheckBox checkBox)
        {
            var cognitoSyncSettings = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();
            var ds = (IDynamicSettingsPlugin)cognitoSyncSettings;

            var settingName = checkBox.Name;
            bool value = checkBox.Checked;

            ds.SetSetting(settingName, value);

            ds.SaveSetting(settingName);
        }
        
        private void SynchronizeDataset_Click(object sender, EventArgs e)
        {
            var plugin = CognitoSyncSettings.GetPlugin<ICognitoSyncSettingsPlugin>();
            plugin.SynchronizeDataset();
        }

        private void boolValue_CheckedChanged(object sender, EventArgs e)
        {
            CognitoSyncSettings.Boolean = this.boolValue.Checked;
            CognitoSyncSettings.SaveSetting(s => CognitoSyncSettings.Boolean);
        }

        private void enumValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_enableEnumComboBoxEvent)
            {
                var index = this.enumValue.SelectedIndex;

                CognitoSyncSettings.Enum = (EEnumValues)index;
                CognitoSyncSettings.SaveSetting(s => CognitoSyncSettings.Enum);
            }
        }
    }
}

