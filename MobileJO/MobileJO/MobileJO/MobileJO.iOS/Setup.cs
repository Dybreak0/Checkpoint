using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using MobileJO.Core.Contracts;
using MobileJO.iOS.Services;

namespace MobileJO.iOS
{
    public class Setup : MvxFormsIosSetup<Core.MvxApp, Core.FormsApp>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.RegisterSingleton<ILocalizeService>(() => new LocalizeService());
        }
    }
}