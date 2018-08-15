// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Sample.FirebaseRemoteConfig.App.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UILabel m_CustomObject { get; set; }

        [Outlet]
        UIKit.UILabel m_FetchStatus { get; set; }

        [Outlet]
        UIKit.UILabel m_Label { get; set; }


        [Action ("UIButton44Ua86gi_TouchUpInside:")]
        partial void UIButton44Ua86gi_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (m_CustomObject != null) {
                m_CustomObject.Dispose ();
                m_CustomObject = null;
            }

            if (m_FetchStatus != null) {
                m_FetchStatus.Dispose ();
                m_FetchStatus = null;
            }

            if (m_Label != null) {
                m_Label.Dispose ();
                m_Label = null;
            }
        }
    }
}