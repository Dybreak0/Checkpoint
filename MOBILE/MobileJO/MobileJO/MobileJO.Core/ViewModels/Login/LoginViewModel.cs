using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.AssignedCases;
using MobileJO.Core.ViewModels.ForgotPassword;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MobileJO.Core.ViewModels.Login
{
    public class LoginViewModel : BaseViewModel
    {

        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;

        public LoginViewModel(IMvxNavigationService navigationService,
                                           IAppSettings settings,
                                           IUserDialogs userDialogs,
                                           ILocalizeService localizeService,
                                           IWebService webService)
           : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _userDialogs = userDialogs;
            _localizeService = localizeService;
            _webService = webService;
            if (!NetworkCheck.HasInternet())
            {
                _buttonEnabled = false;

                Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                {
                    if(NetworkCheck.HasInternet() && !ShowError)
                    {
                        ButtonEnabled = true;
                    }          
                    else
                    {
                        ButtonEnabled = false;
                    }
                    
                    return true;
                });
            }
            else
            {
                _buttonEnabled = true;

                Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                {
                    if (!NetworkCheck.HasInternet())
                    {
                        ButtonEnabled = false;
                    }
                    else
                    {
                        if(!ShowError)
                            ButtonEnabled = true;
                    }

                    return true;
                });
            }            
        }

        private string _userName;
        private string _password;
        private bool _buttonEnabled;
        private bool _showError;
        private string _temporaryLockedMessage;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set => SetProperty(ref _buttonEnabled, value);
        }

        public bool ShowError
        {
            get => _showError;
            set => SetProperty(ref _showError, value);
        }

        public string TemporaryLockedMessage
        {
            get { return _temporaryLockedMessage; }
            set { _temporaryLockedMessage = Constants.Messages.AccountLocked; } 
        }

        private string _localStorageUsername;
        private string _localStorageId;

        public string localStorageUsername
        {
            get => _localStorageUsername;
            set => SetProperty(ref _localStorageUsername, value);
        }

        public string localStorageId
        {
            get => _localStorageId;
            set => SetProperty(ref _localStorageId, value);
        }

        private UserCredentialsModel UserCredential
        {
            get
            {
                return new UserCredentialsModel
                {
                    UserName = UserName,
                    Password = Password,
                    MobileLogin = string.Empty
                };
            }
        }

        public override void Prepare(Dictionary<string, string> parameter)
        {            
            _buttonEnabled = true;
            _showError = false;
        }

        private bool HasValidData()
        {
            if (string.IsNullOrEmpty(UserCredential.UserName) || string.IsNullOrEmpty(UserCredential.Password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IMvxCommand Login => new MvxCommand(async () =>
        {
            if (HasValidData())
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                var error = false;
                var localizedMessage = string.Empty;

                try
                {
                    if (NetworkCheck.HasInternet())
                    {
                        var response = await _webService.Login(UserCredential);
                        var valid = false;
                        if (CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID) != null)
                        {
                            valid = await _webService.Allowed(int.Parse(CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID)));
                        }                        

                        if (!valid && response)
                        {
                            _settings.AccessToken = string.Empty;
                            _settings.RefreshToken = string.Empty;
                            _settings.UserID = string.Empty;
                            _settings.UserName = string.Empty;
                            CrossSecureStorage.Current.DeleteKey(Constants.AppSettings.AccessToken);
                            CrossSecureStorage.Current.DeleteKey(Constants.AppSettings.UserID);
                            CrossSecureStorage.Current.DeleteKey(Constants.AppSettings.UserName);
                            localizedMessage = LocalizeService.Translate(Constants.Messages.NotAllowedToLogin);
                            await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                        }
                        else if (response && valid)
                        {
                            _settings.LoginAttempts = 0;
                            var param = new Dictionary<string, string>
                            {
                                { Constants.Params.AssignedTo, _settings.UserID },
                                { Constants.Params.UserName, _settings.UserName},
                                { Constants.Params.FromLogin, Constants.Common.StringTrue}
                            };

                            await _navigationService.Navigate<MainViewModel, Dictionary<string, string>>(param);
                        }
                        else
                        {
                            _settings.LoginAttempts++;

                            if (_settings.LoginAttempts > 5)
                            {

                                ButtonEnabled = false;
                                ShowError = true;
                                Device.StartTimer(TimeSpan.FromSeconds(30), () =>
                                {
                                    ButtonEnabled = true;
                                    ShowError = false;
                                    return false;
                                });
                            }

                            localizedMessage = LocalizeService.Translate(Constants.Messages.IncorrectLogin);
                            await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                        }
                    }
                    else
                    {
                        localizedMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                        await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                    }

                }
                catch (Exception ex)
                {
                    error = true;
                }
                finally
                {
                    IsBusy = false;
                }

                if (error)
                {
                    localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                    await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                }
            }
            else
            {
                await _userDialogs.AlertAsync(Constants.Messages.ErrorRequiredFields, Constants.Modal.Warning, Constants.Common.OK);
            }


        });

        public IMvxCommand ForgotPassword => new MvxCommand(async () =>
        {
            await _navigationService.Navigate<ForgotPasswordViewModel, Dictionary<string, string>>(null);
        });
    }
}
