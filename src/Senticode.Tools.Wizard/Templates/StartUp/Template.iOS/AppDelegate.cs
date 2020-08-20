using System;
using System.Diagnostics;
using System.Threading.Tasks;
using _template.MasterDetail;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace _template.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            Forms.SetFlags("CollectionView_Experimental");
            Forms.SetFlags("RadioButton_Experimental");
            Forms.Init();
            UINavigationBar.Appearance.BarTintColor = UIColor.White; // Apply color to Burger menu.
            LoadApplication(new App(IosInitializer.Instance));

            return base.FinishedLaunching(app, options);
        }

        public static UIViewController GetTopViewController(UIApplication app)
        {
            var viewController = app.KeyWindow.RootViewController;

            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }

        private void RequestAppPermissions(UIApplication app)
        {
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception);
            e.SetObserved();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.ExceptionObject);
        }
    }
}