using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace MobileJO.Core.ViewModels.EmailJO
{
    public class EmailJOViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;
        private Dictionary<string, string> _parameter;
        private bool _useDefaultEmailAddress { get; set; }
        private string _jobOrderNos { get; set; }
        private string _recipientTo { get; set; }
        private string _recipientCc { get; set; }
        private string _recipientBcc { get; set; }
        private bool _isOnsite { get; set; }
        private bool _isOffsite { get; set; }
        private string _conformeSlip { get; set; }

        public EmailJOViewModel(IMvxNavigationService navigationService,
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
        }

        public bool UseDefaultEmailAddress
        {
            get { return _useDefaultEmailAddress; }
            set { _useDefaultEmailAddress = value; }
        }

        public string JobOrderNos
        {
            get { return _jobOrderNos; }
        }

        public void SetJobOrder(string ids)
        {
            _jobOrderNos = ids;
        }

        public List<string> GetJobOrderNos()
        {
            return JobOrderNos.Split(Constants.SpecialCharacters.CharSemiColon).ToList();
        }

        public string RecipientTo
        {
            get { return _recipientTo; }
            set { _recipientTo = value; }
        }

        public string RecipientCc
        {
            get { return _recipientCc; }
            set { _recipientCc = value; }
        }

        public string RecipientBcc
        {
            get { return _recipientBcc; }
            set { _recipientBcc = value; }
        }

        public bool IsOnsite
        {
            get { return _isOnsite; }
            set { _isOnsite = value; }
        }

        public bool IsOffsite
        {
            get { return _isOffsite; }
            set { _isOffsite = value; }
        }

        public string ConformeSlip
        {
            get { return _conformeSlip; }
            set { _conformeSlip = value; }
        }

        private bool _enableCancel;
        public bool EnableCancel
        {
            get => _enableCancel;
            set => SetProperty(ref _enableCancel, value);
        }
                
        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;
            _useDefaultEmailAddress = true;
            _isOnsite = true;
            if (_parameter != null && _parameter.ContainsKey(Constants.Keys.JobOrderIDs))
            {
                SetJobOrder(_parameter[Constants.Keys.JobOrderIDs]);
            }
        }

        public IMvxCommand Cancel => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            var confirm = await _userDialogs.ConfirmAsync(Constants.Messages.ConfirmCancel,
                                                          Constants.Modal.Confirmation, 
                                                          Constants.Messages.Yes, 
                                                          Constants.Messages.No);

            if (confirm)
            {
                await _navigationService.Close(this);
            }


        });

        public IMvxCommand ConfirmEmailSending => new MvxCommand(async () =>
        {
            
            if(string.IsNullOrEmpty(ConformeSlip))
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRequiredFields);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }            
            else
            {
              

                var validConformeNo = Regex.IsMatch(ConformeSlip, Constants.Common.ConformeRegex);
                if (!UseDefaultEmailAddress)
                {
                    var validEmail = ValidateEmail();
                    
                    if (validEmail)
                    {
                        if(validConformeNo)
                        {
                            var confirm = await _userDialogs.ConfirmAsync(Constants.Messages.ConfirmEmail,
                                                                      Constants.Modal.Confirmation,
                                                                      Constants.Messages.Yes,
                                                                      Constants.Messages.No);

                            if (confirm)
                            {
                                SendEmailJO.Execute();
                            }
                        }
                        else
                        {
                            var localizedMessage = LocalizeService.Translate(Constants.Messages.InvalidConformeNumber);
                            await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                        }
                    }
                    else
                    {
                        var localizedMessage = LocalizeService.Translate(Constants.Messages.InvalidEmail);
                        await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                    }
                }
                else
                {
                    if (validConformeNo)
                    {
                        if (NetworkCheck.HasInternet())
                        {
                            var confirm = await _userDialogs.ConfirmAsync(Constants.Messages.ConfirmEmail,
                                                                      Constants.Modal.Confirmation,
                                                                      Constants.Messages.Yes,
                                                                      Constants.Messages.No);

                            if (confirm)
                            {
                                SendEmailJO.Execute();
                            }
                        }
                        else
                        {
                            var localizedMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                            await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                        }
                    }
                    else
                    {
                        var localizedMessage = LocalizeService.Translate(Constants.Messages.InvalidConformeNumber);
                        await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                    }
                }

               
               
            }
           

        });

        private bool ValidateEmail()
        {
            bool valid = true;

            if (!string.IsNullOrEmpty(RecipientTo))
            {
                var EmailAddress = RecipientTo.Split(Constants.SpecialCharacters.CharComma);

                foreach (string email in EmailAddress)
                {
                    if(!IsEmailValid(email))
                    {
                        return false;
                    }
                }

                if(!string.IsNullOrEmpty(RecipientCc))
                {
                    var cc = RecipientCc.Split(Constants.SpecialCharacters.CharComma);
                    foreach (string email in cc)
                    {
                        if (!IsEmailValid(email))
                        {
                            return false;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(RecipientBcc))
                {
                    var bcc = RecipientBcc.Split(Constants.SpecialCharacters.CharComma);
                    foreach (string email in bcc)
                    {
                        if (!IsEmailValid(email))
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                valid = false;
            }

            return valid;
        }

        public bool IsEmailValid(string emailaddress)
        {
            if (Regex.IsMatch(emailaddress, Constants.Common.EmailRegex))
            {
                return true;
            }

            return false;
        }

        public IMvxCommand SendEmailJO => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    var supportType = (IsOnsite) ? 
                                        Constants.EmailJO.Onsite :
                                        Constants.EmailJO.Offsite;

                    var joIDs = GetJobOrderNos();
                    var emailRecipient = new List<Email>();

                    if(!UseDefaultEmailAddress)
                    {
                        emailRecipient = Helpers.AssembleEmail(_recipientTo, _recipientCc, _recipientBcc);
                    }

                    var emailParams = new EmailJOModel()
                    {
                        JobOrderNos = joIDs,
                        Recipient = emailRecipient,
                        UseDefaultAddress = UseDefaultEmailAddress,
                        ConformeSlip = ConformeSlip,
                        SupportType = supportType
                    };
                    await _webService.SendEmailJO(emailParams);
                    
                }
                else
                {
                    var localizedMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                    await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                }

            }
            catch(Exception)
            {
                error = true;
            }
            finally
            {

                if (!error)
                {                                    
                    var localizedMessage = LocalizeService.Translate(Constants.Messages.EmailSent);
                    await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                    
                }

                var param = new Dictionary<string, string>();
                await _navigationService.Navigate<MainViewModel, Dictionary<string, string>>(param);
            }

            if (error)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }

            IsBusy = false;
        });

    }
}
