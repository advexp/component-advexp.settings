using System;
using UIKit;
using Advexp.FirebaseRemoteConfig.Plugin;
using Foundation;

namespace Sample.FirebaseRemoteConfig.iOS
{
    public partial class ViewController : UIViewController
    {
        //------------------------------------------------------------------------------
        protected ViewController(IntPtr handle) : base(handle)
        {
            var plugin = Settings.GetPlugin<IFirebaseRemoteConfigPlugin>();
            plugin.FetchCompletionHandler = (status) =>
            {
                switch (status)
                {
                    case FetchStatus.Success:
                        UpdateUI();
                        break;
                    case FetchStatus.Throttled:
                        break;
                    case FetchStatus.NoFetchYet:
                        break;
                    case FetchStatus.Failure:
                        break;
                }

                UpdateStatusText(status);
            };
        }

        //------------------------------------------------------------------------------
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UpdateUI();
        }

        //------------------------------------------------------------------------------
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        //------------------------------------------------------------------------------
        partial void UIButton44Ua86gi_TouchUpInside(UIButton sender)
        {
            m_FetchStatus.Text = "Fetching...";

            var plugin = Settings.GetPlugin<IFirebaseRemoteConfigPlugin>();
            plugin.Fetch();
        }

        //------------------------------------------------------------------------------
        void UpdateUI()
        {
            InvokeOnMainThread(() =>
            {
                Settings.LoadSettings();

                m_Label.Text = 
                    Settings.String == null ?
                    "null" :
                    Settings.String;
                m_Switch.On = Settings.Boolean;
                m_Slider.Value = Settings.SliderValue;
                m_DatePicker.SetDate((NSDate)Settings.DateTime, true);
                m_CustomObject.Text = 
                    Settings.CustomObject == null ? 
                    "null" : 
                    Settings.CustomObject.ToString();
            });
        }

        //------------------------------------------------------------------------------
        void UpdateStatusText(FetchStatus status)
        {
            InvokeOnMainThread(() =>
            {
                switch (status)
                {
                    case FetchStatus.Success:
                        m_FetchStatus.Text = "Status - Success";
                        break;
                    case FetchStatus.Throttled:
                        m_FetchStatus.Text = "Status - Throttled";
                        break;
                    case FetchStatus.NoFetchYet:
                        m_FetchStatus.Text = "Status - NoFetchYet";
                        break;
                    case FetchStatus.Failure:
                        m_FetchStatus.Text = "Status - Failure";
                        break;
                    default:
                        m_FetchStatus.Text = "Status - Unknown status code";
                        break;
                }
            });
        }
    }
}
