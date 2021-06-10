using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.Login;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.SecureStorage;

namespace MobileJO.Core.ViewModels.SettingsViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;

        public SettingsViewModel(IMvxNavigationService navigationService, IAppSettings settings,
            IUserDialogs userDialogs, ILocalizeService localizeService, IWebService webService)
            : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _webService = webService;
            _userDialogs = userDialogs;

        }

        public IMvxCommand GoToMainPage => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;
            await _navigationService.Navigate<MainViewModel>();
        });

        public IMvxCommand GoToPrivacyPolicy => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;
            await _navigationService.Navigate<PrivacyPolicyViewModel>();
        });

        public IMvxCommand GoToTermsAndCondition => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;
            await _navigationService.Navigate<TermsAndConditionViewModel>();
        });

        public IMvxCommand Logout => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            bool confirmLogout = await _userDialogs.ConfirmAsync(Constants.Messages.ConfirmLogout,
                                                                 Constants.Modal.Confirmation,
                                                                 Constants.Messages.Yes,
                                                                 Constants.Messages.No);

            if (confirmLogout)
            {
                _settings.AccessToken = string.Empty;
                _settings.RefreshToken = string.Empty;
                _settings.UserID = string.Empty;
                _settings.UserName = string.Empty;
                CrossSecureStorage.Current.DeleteKey(Constants.AppSettings.AccessToken);
                CrossSecureStorage.Current.DeleteKey(Constants.AppSettings.UserID);
                CrossSecureStorage.Current.DeleteKey(Constants.AppSettings.UserName);
                await _navigationService.Navigate<LoginViewModel>();
            }
        });
    }
}
