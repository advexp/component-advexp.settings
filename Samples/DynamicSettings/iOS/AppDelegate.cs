using System;
using Foundation;
using UIKit;

namespace Sample.DynamicSettings.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        UINavigationController m_navigation;
        UIWindow m_window;

        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation(UIApplication application)
        {
        }

        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
        }

        // This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
        }

        // This method is called when the application is about to terminate. Save data, if needed.
        public override void WillTerminate(UIApplication application)
        {
        }

        // This method is required in iPhoneOS 3.0
        public override void OnActivated(UIApplication application)
        {
        }

        // This method is invoked when the application has loaded its UI and its ready to run
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var dynamicSettingsController = new DynamicSettingsController();

            m_navigation = new UINavigationController();
            m_navigation.PushViewController(dynamicSettingsController, true);

            // On iOS5 we use the new window.RootViewController, on older versions, we add the subview
            m_window = new UIWindow(UIScreen.MainScreen.Bounds);
            m_window.MakeKeyAndVisible();
            if (UIDevice.CurrentDevice.CheckSystemVersion(5, 0))
            {
                m_window.RootViewController = m_navigation;
            }
            else
            {
                m_window.AddSubview(m_navigation.View);
            }

            return true;
        }
    }
}

