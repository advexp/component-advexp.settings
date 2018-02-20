// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Sample.FirebaseRemoteConfig.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UILabel m_CustomObject { get; set; }


        [Outlet]
        UIKit.UIDatePicker m_DatePicker { get; set; }


        [Outlet]
        UIKit.UILabel m_FetchStatus { get; set; }


        [Outlet]
        UIKit.UILabel m_Label { get; set; }


        [Outlet]
        UIKit.UISlider m_Slider { get; set; }


        [Outlet]
        UIKit.UISwitch m_Switch { get; set; }


        [Action ("UIButton44Ua86gi_TouchUpInside:")]
        partial void UIButton44Ua86gi_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (m_CustomObject != null) {
                m_CustomObject.Dispose ();
                m_CustomObject = null;
            }

            if (m_DatePicker != null) {
                m_DatePicker.Dispose ();
                m_DatePicker = null;
            }

            if (m_FetchStatus != null) {
                m_FetchStatus.Dispose ();
                m_FetchStatus = null;
            }

            if (m_Label != null) {
                m_Label.Dispose ();
                m_Label = null;
            }

            if (m_Slider != null) {
                m_Slider.Dispose ();
                m_Slider = null;
            }

            if (m_Switch != null) {
                m_Switch.Dispose ();
                m_Switch = null;
            }
        }
    }
}