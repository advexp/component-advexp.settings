// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Sample.LocalSettings.Mac
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        AppKit.NSButton m_Check { get; set; }

        [Outlet]
        AppKit.NSButton m_Check1 { get; set; }

        [Outlet]
        AppKit.NSButton m_Check2 { get; set; }

        [Outlet]
        AppKit.NSButton m_Check3 { get; set; }

        [Outlet]
        AppKit.NSButton m_Check4 { get; set; }

        [Outlet]
        AppKit.NSButton m_Check5 { get; set; }

        [Outlet]
        AppKit.NSButton m_Check6 { get; set; }

        [Outlet]
        AppKit.NSDatePicker m_Date { get; set; }

        [Outlet]
        AppKit.NSTextField m_Text { get; set; }
        
        void ReleaseDesignerOutlets ()
        {
            if (m_Text != null) {
                m_Text.Dispose ();
                m_Text = null;
            }

            if (m_Check != null) {
                m_Check.Dispose ();
                m_Check = null;
            }

            if (m_Date != null) {
                m_Date.Dispose ();
                m_Date = null;
            }

            if (m_Check1 != null) {
                m_Check1.Dispose ();
                m_Check1 = null;
            }

            if (m_Check2 != null) {
                m_Check2.Dispose ();
                m_Check2 = null;
            }

            if (m_Check3 != null) {
                m_Check3.Dispose ();
                m_Check3 = null;
            }

            if (m_Check4 != null) {
                m_Check4.Dispose ();
                m_Check4 = null;
            }

            if (m_Check5 != null) {
                m_Check5.Dispose ();
                m_Check5 = null;
            }

            if (m_Check6 != null) {
                m_Check6.Dispose ();
                m_Check6 = null;
            }
        }
    }
}
