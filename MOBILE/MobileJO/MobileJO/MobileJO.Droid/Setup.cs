using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Core;
using MobileJO.Core.Contracts;
using MvvmCross.Forms.Presenters;

namespace MobileJO.Droid
{
    public class Setup : MvxFormsAndroidSetup<Core.MvxApp, Core.FormsApp>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.RegisterSingleton<ILocalizeService>(() => new Services.LocalizeService());
        }

        protected override IMvxFormsPagePresenter CreateFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            var formsPresenter = base.CreateFormsPagePresenter(viewPresenter);
            Mvx.RegisterSingleton(formsPresenter);
            return formsPresenter;
        }

    }
}