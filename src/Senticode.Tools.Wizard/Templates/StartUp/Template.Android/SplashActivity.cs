using System.Threading.Tasks;
using Android.App;
using Android.Content;

namespace Template.Android
{
    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnResume()
        {
            base.OnResume();

            var startupWork = new Task(() =>
            {
                var intent = new Intent(this, typeof(MainActivity));

                if (Intent.Extras != null)
                {
                    intent.PutExtras(Intent.Extras);
                }

                intent.SetFlags(ActivityFlags.SingleTop);
                StartActivity(intent);
                Finish();
            });

            startupWork.Start();
        }
    }
}