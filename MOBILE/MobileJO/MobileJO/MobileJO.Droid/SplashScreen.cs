using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Platforms.Android;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileJO.Droid
{
    [Activity(
        Label = "Addessa Checkpoint", 
        MainLauncher = true,
        Icon = "@drawable/logo",
        Theme = "@style/Theme.Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxFormsSplashScreenActivity<Setup, Core.MvxApp, Core.FormsApp>
    {
        public SplashScreen() : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            UserDialogs.Init(() => Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity);

            // Leverage controls' StyleId attrib. to Xamarin.UITest
            Forms.ViewInitialized += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId))
                {
                    e.NativeView.ContentDescription = e.View.StyleId;
                }
            };

            base.OnCreate(bundle);
        }

        protected override Task RunAppStartAsync(Bundle bundle)
        {
            StartActivity(typeof(FormsApplicationActivity));
            return base.RunAppStartAsync(bundle);
        }
    }
}