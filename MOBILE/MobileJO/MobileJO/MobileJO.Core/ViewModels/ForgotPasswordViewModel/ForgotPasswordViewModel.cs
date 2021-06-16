using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.AssignedCases;
using MobileJO.Core.ViewModels.Common;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace MobileJO.Core.ViewModels.ForgotPassword
{
    public class ForgotPasswordViewModel : BaseViewModel
    {

        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;

        public ForgotPasswordViewModel(IMvxNavigationService navigationService,
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

            
            _buttonEnabled = NetworkCheck.HasInternet();

            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                ButtonEnabled = NetworkCheck.HasInternet();
                return true;
            });
        }

        private string _emailAddress;
        private bool _buttonEnabled;

        public string EmailAddress
        {
            get => _emailAddress;
            set => SetProperty(ref _emailAddress, value);
        }


        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set => SetProperty(ref _buttonEnabled, value);
        }



        public override void Prepare(Dictionary<string, string> parameter)
        {
            _buttonEnabled = true;
        }

       
        public IMvxCommand SubmitRequest => new MvxCommand(async () =>
        {
            var email = EmailAddress.ToString().Trim();
            if (string.IsNullOrEmpty(email))
            {
                await _userDialogs.AlertAsync(Constants.Messages.ErrorRequiredFields, Constants.Modal.Warning, Constants.Common.OK);
                return;
            }
            if (!Regex.IsMatch(email, Constants.Common.EmailRegex))
            {
                await _userDialogs.AlertAsync(Constants.Messages.InvalidEmail, Constants.Modal.Warning, Constants.Common.OK);
                return;
            }
                if (IsBusy)
                    return;

                IsBusy = true;
                var localizedMessage = string.Empty;

                try
                {
                    if (NetworkCheck.HasInternet())
                    {
                        EmailModel emailModel = new EmailModel();
                        emailModel.EmailAddress = email;

                        var response = await _webService.ResetPassword(emailModel);

                        if (response.message.Equals(Constants.Messages.RecordDoesNotExist))
                        {
                            await _userDialogs.AlertAsync(Constants.Messages.UserNotFound, Constants.Modal.Warning, Constants.Common.OK);
                            return;
                        }
                        if (response.message.Equals(Constants.Messages.ForgetPasswordSent))
                        {
                            await _userDialogs.AlertAsync(Constants.Messages.ForgetPasswordSent, Constants.Modal.InfoMessage, Constants.Common.OK);
                            return;
                        }

                        await _userDialogs.AlertAsync(Constants.Messages.ErrorProcessing, Constants.Modal.Warning, Constants.Common.OK);
                        return;
                    }
                    else
                    {
                        await _userDialogs.AlertAsync(Constants.Messages.NoInternet, Constants.Modal.Warning, Constants.Common.OK);
                    }
                }
                catch (Exception)
                {
                    localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                    await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                }
                finally
                {
                    IsBusy = false;
                }
        });

    }
}
