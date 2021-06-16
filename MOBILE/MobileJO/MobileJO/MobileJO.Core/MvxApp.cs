using System.Reflection;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.IoC;
using MvvmCross.Plugin.Json;
using MvvmCross.ViewModels;
using PCLAppConfig;
using MobileJO.Core.Contracts;
using MobileJO.Core.Services;
using MobileJO.Core.Utilities;
using Xamarin.Forms;
using MobileJO.Core.Data;
using System.IO;
using System;

namespace MobileJO.Core
{
    public class MvxApp : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith(Constants.Common.Service)
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes().
                EndingWith(Constants.Common.Repository)
                .AsTypes()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.RegisterType<IWebService, WebService>();
            Mvx.IoCProvider.RegisterType<IAppSettings, AppSettings>();
            Mvx.IoCProvider.RegisterType<IMvxJsonConverter, MvxJsonConverter>();            
            Mvx.IoCProvider.RegisterSingleton(() => UserDialogs.Instance);

            Resources.AppResources.Culture = Mvx.IoCProvider.Resolve<ILocalizeService>().GetCurrentCultureInfo();

            var assembly = typeof(MvxApp).GetTypeInfo().Assembly;
            ConfigurationManager.Initialise(assembly.GetManifestResourceStream(Constants.AppSettings.AppConfig));

            if (Helpers.HasValidSession())
            {
                RegisterAppStart<ViewModels.QuestionnaireListViewModel>();
            }
            else
            {
                RegisterAppStart<ViewModels.Login.LoginViewModel>();
            }

            Plugin.InputKit.Shared.Controls.CheckBox.GlobalSetting.FontSize = 12;
            Plugin.InputKit.Shared.Controls.CheckBox.GlobalSetting.Size = 16;
            Plugin.InputKit.Shared.Controls.CheckBox.GlobalSetting.TextColor = Color.Black;
            Plugin.InputKit.Shared.Controls.RadioButton.GlobalSetting.Size = 16;
            Plugin.InputKit.Shared.Controls.RadioButton.GlobalSetting.TextColor = Color.Black;

            if (Device.RuntimePlatform == Device.iOS)
            {
                Plugin.InputKit.Shared.Controls.RadioButton.GlobalSetting.FontSize = 12;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                Plugin.InputKit.Shared.Controls.RadioButton.GlobalSetting.FontSize = 9;
            }
        }

        static JobOrderLocalDatabase database;

        public static JobOrderLocalDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new JobOrderLocalDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.Sqlite.LocalDatabase));
                }
                return database;
            }
        }
    }
}
