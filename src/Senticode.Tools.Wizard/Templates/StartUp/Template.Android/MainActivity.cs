using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Template.Android.Services;
using Template.MasterDetail;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Platform = Xamarin.Essentials.Platform;

namespace Template.Android
{
    [Activity(Label = "Senticode", Icon = "@drawable/iconLayout", Theme = "@style/MainTheme", MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        private const int PermissionsRequest = 101;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            ActivityLocator.Current = this;
            Forms.SetFlags("RadioButton_Experimental");
            Forms.SetFlags("CollectionView_Experimental");
            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            var instance = AndroidInitializer.Instance;
            LoadApplication(new App(instance));
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironmentOnUnhandledExceptionRaiser;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Debug("TEST", e.Exception.Message);
            e.SetObserved();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Debug("TEST", e.ExceptionObject.ToString());
        }

        private static void AndroidEnvironmentOnUnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            Log.Debug("TEST", e.Exception.Message);
            e.Handled = true;
        }

        protected override void OnResume()
        {
            base.OnResume();
            CheckAndRequestPermissions();
        }

        private void CheckAndRequestPermissions()
        {
            if (int.Parse(Build.VERSION.Sdk) < 23)
            {
                return;
            }

            var permissions = new List<string>();

            if (CheckSelfPermission(Manifest.Permission.ReadExternalStorage) != Permission.Granted)
            {
                permissions.Add(Manifest.Permission.ReadExternalStorage);
            }

            if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                permissions.Add(Manifest.Permission.WriteExternalStorage);
            }

            if (permissions.Count > 0)
            {
                RequestPermissions(permissions.ToArray(), PermissionsRequest);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode != PermissionsRequest)
            {
                return;
            }

            var i = 0;

            foreach (var permission in permissions)
            {
                Log.Info("Senticode", "Permission " + permission + " : " + grantResults[i]);

                i += 1;
            }
        }
    }
}