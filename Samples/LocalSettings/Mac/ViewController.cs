using System;

using AppKit;
using Foundation;
using Advexp.LocalDynamicSettings.Plugin;

namespace Sample.LocalSettings.Mac
{
    public partial class ViewController : NSViewController
    {
        //------------------------------------------------------------------------------
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        //------------------------------------------------------------------------------
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Settings.LoadSettings();
            GetStaticSettings();

            var lds = Settings.GetPlugin<ILocalDynamicSettingsPlugin>();
            lds.LoadSettings();

            GetDynamicSetting(this.m_Check1);
            GetDynamicSetting(this.m_Check2);
            GetDynamicSetting(this.m_Check3);
            GetDynamicSetting(this.m_Check4);
            GetDynamicSetting(this.m_Check5);
            GetDynamicSetting(this.m_Check6);
        }

        //------------------------------------------------------------------------------
        public override void ViewDidDisappear()
        {
            base.ViewDidDisappear();

            SetStaticSettings();
            Settings.SaveSettings();

            SetDynamicSetting(this.m_Check1);
            SetDynamicSetting(this.m_Check2);
            SetDynamicSetting(this.m_Check3);
            SetDynamicSetting(this.m_Check4);
            SetDynamicSetting(this.m_Check5);
            SetDynamicSetting(this.m_Check6);

            var lds = Settings.GetPlugin<ILocalDynamicSettingsPlugin>();
            lds.SaveSettings();
        }

        //------------------------------------------------------------------------------
        void GetStaticSettings()
        {
            this.m_Text.StringValue = Settings.Text;
            this.m_Check.State = Settings.CheckBox == true ? NSCellStateValue.On : NSCellStateValue.Off;

            this.m_Date.DateValue = (NSDate)Settings.DateTime;
        }

        //------------------------------------------------------------------------------
        void SetStaticSettings()
        {
            Settings.Text = this.m_Text.StringValue;
            Settings.CheckBox = this.m_Check.State == NSCellStateValue.On ? true : false;
            Settings.DateTime = (DateTime)this.m_Date.DateValue;
        }

        //------------------------------------------------------------------------------
        void GetDynamicSetting(AppKit.NSButton checkBox)
        {
            var settingName = checkBox.Identifier;

            var lds = Settings.GetPlugin<ILocalDynamicSettingsPlugin>();
            var val = lds.GetSetting<NSCellStateValue>(settingName, NSCellStateValue.Off);
            checkBox.State = val;
        }


        //------------------------------------------------------------------------------
        void SetDynamicSetting(AppKit.NSButton checkBox)
        {
            var settingName = checkBox.Identifier;

            var lds = Settings.GetPlugin<ILocalDynamicSettingsPlugin>();
            lds.SetSetting(settingName, checkBox.State);
        }

        //------------------------------------------------------------------------------
        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
