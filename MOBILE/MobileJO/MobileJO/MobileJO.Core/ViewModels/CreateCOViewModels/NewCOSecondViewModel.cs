using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.FieldCOViewModels;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MobileJO.Core.ViewModels.CreateCOViewModels
{
    class NewCOSecondViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private readonly IWebService _webService;

        private Dictionary<string, string> _parameter;

        private string _iconFile;
        public string IconFile
        {
            get => _iconFile;
            set => SetProperty(ref _iconFile, value);
        }

        private string _signatureBytes { get; set; }

        public NewCOSecondViewModel(IMvxNavigationService navigationService,
                                  IAppSettings settings,
                                  IUserDialogs userDialogs,
                                  ILocalizeService localizeService,
                                  IMvxJsonConverter serializer,
                                  IWebService webService) : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _userDialogs = userDialogs;
            _localizeService = localizeService;
            _serializer = serializer;
            _webService = webService;
        }

        public string ClientResCertNo { get; set; }
        public string ClientPlaceIssued { get; set; }
        public DateTime ClientDate { get; set; } = DateTime.Now;
        public string ClientSignature { get; set; }

        public string SpouseResCertNo { get; set; }
        public string SpousePlaceIssued { get; set; }
        public DateTime SpouseDate { get; set; } = DateTime.Now;
        public string SpouseSignature { get; set; }

        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public TimeSpan DeliveryTime { get; set; }

        public string ClosingOfficer { get; set; }
        public DateTime ClosingOfficerDate { get; set; } = DateTime.Now;
        public TimeSpan ClosingOfficerTime { get; set; }
        public string ClosingOfficerRemarks { get; set; }

        public string BranchManagerSignature { get; set; }
        public DateTime BranchManagerDate { get; set; } = DateTime.Now;
        public TimeSpan BranchManagerTime { get; set; }
        public string BranchManagerRemarks { get; set; }

        //Error Boolean
        public bool ClientResCertNoError { get; set; }
        public bool ClientPlaceIssuedError { get; set; }
        public bool ClientDateError { get; set; }
        public bool ClientSignatureError { get; set; }

        public bool SpouseResCertNoError { get; set; }
        public bool SpousePlaceIssuedError { get; set; }
        public bool SpouseDateError { get; set; }
        public bool SpouseSignatureError { get; set; }

        public bool DeliveryDateError { get; set; }
        public bool DeliveryTimeError { get; set; }

        public bool ClosingOfficerError { get; set; }
        public bool ClosingOfficerDateError { get; set; }
        public bool ClosingOfficerTimeError { get; set; }
        public bool ClosingOfficerRemarksError { get; set; }

        public bool BranchManagerSignatureError { get; set; }
        public bool BranchManagerDateError { get; set; }
        public bool BranchManagerTimeError { get; set; }
        public bool BranchManagerRemarksError { get; set; }

        //Error Message
        public string ClientResCertNoErrorMsg { get; set; }
        public string ClientPlaceIssuedErrorMsg { get; set; }
        public string ClientDateErrorMsg { get; set; }
        public string ClientSignatureErrorMsg { get; set; }

        public string SpouseResCertNoErrorMsg { get; set; }
        public string SpousePlaceIssuedErrorMsg { get; set; }
        public string SpouseDateErrorMsg { get; set; }
        public string SpouseSignatureErrorMsg { get; set; }

        public string DeliveryDateErrorMsg { get; set; }
        public string DeliveryTimeErrorMsg { get; set; }

        public string ClosingOfficerErrorMsg { get; set; }
        public string ClosingOfficerDateErrorMsg { get; set; }
        public string ClosingOfficerTimeErrorMsg { get; set; }
        public string ClosingOfficerRemarksErrorMsg { get; set; }

        public string BranchManagerSignatureErrorMsg { get; set; }
        public string BranchManagerDateErrorMsg { get; set; }
        public string BranchManagerTimeErrorMsg { get; set; }
        public string BranchManagerRemarksErrorMsg { get; set; }

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;
            IconFile = "add_button.png";
        }

        public bool IsValidFields(SecondPageCOViewModel secondPageFields)
        {
            bool flag = true;

            if (string.IsNullOrWhiteSpace(secondPageFields.ClientResCertNo) || !Regex.IsMatch(secondPageFields.ClientResCertNo, Constants.Common.TextRegex))
            {
                ClientResCertNoErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.ClientResCertNo) ?
                                                                                            Constants.Messages.ClientResCertNoRequired :
                                                                                            Constants.Messages.ClientResCertNoInvalid;
                ClientResCertNoError = true;
                flag = false;
            }
            else { ClientResCertNoError = false; }

            if (string.IsNullOrWhiteSpace(secondPageFields.ClientPlaceIssued) || !Regex.IsMatch(secondPageFields.ClientPlaceIssued, Constants.Common.TextRegex))
            {
                ClientPlaceIssuedErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.ClientPlaceIssued) ?
                                                                                            Constants.Messages.ClientPlaceIssuedRequired :
                                                                                            Constants.Messages.ClientPlaceIssuedInvalid;
                ClientPlaceIssuedError = true;
                flag = false;
            }
            else { ClientPlaceIssuedError = false; }

            if (string.IsNullOrWhiteSpace(secondPageFields.SpouseResCertNo) || !Regex.IsMatch(secondPageFields.SpouseResCertNo, Constants.Common.TextRegex))
            {
                SpouseResCertNoErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.SpouseResCertNo) ?
                                                                                            Constants.Messages.SpouseResCertNoRequired :
                                                                                            Constants.Messages.SpouseResCertNoInvalid;
                SpouseResCertNoError = true;
                flag = false;
            }
            else { SpouseResCertNoError = false; }

            if (string.IsNullOrWhiteSpace(secondPageFields.SpousePlaceIssued) || !Regex.IsMatch(secondPageFields.SpousePlaceIssued, Constants.Common.TextRegex))
            {
                SpousePlaceIssuedErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.SpousePlaceIssued) ?
                                                                                            Constants.Messages.SpousePlaceIssuedRequired :
                                                                                            Constants.Messages.SpousePlaceIssuedInvalid;
                SpousePlaceIssuedError = true;
                flag = false;
            }
            else { SpousePlaceIssuedError = false; }

            if (string.IsNullOrWhiteSpace(secondPageFields.BranchManagerRemarks) || !Regex.IsMatch(secondPageFields.BranchManagerRemarks, Constants.Common.TextRegex))
            {
                BranchManagerRemarksErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.BranchManagerRemarks) ?
                                                                                            Constants.Messages.BranchManagerRemarksRequired :
                                                                                            Constants.Messages.BranchManagerRemarksInvalid;
                BranchManagerRemarksError = true;
                flag = false;
            }
            else { BranchManagerRemarksError = false; }

            if (string.IsNullOrWhiteSpace(secondPageFields.ClosingOfficer) || !Regex.IsMatch(secondPageFields.ClosingOfficer, Constants.Common.TextRegex))
            {
                ClosingOfficerErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.ClosingOfficer) ?
                                                                                            Constants.Messages.ClosingOfficerRequired :
                                                                                            Constants.Messages.ClosingOfficerInvalid;
                ClosingOfficerError = true;
                flag = false;
            }
            else { ClosingOfficerRemarksError = false; }

            if (string.IsNullOrWhiteSpace(secondPageFields.BranchManagerRemarks) || !Regex.IsMatch(secondPageFields.BranchManagerRemarks, Constants.Common.TextRegex))
            {
                BranchManagerRemarksErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.BranchManagerRemarks) ?
                                                                                            Constants.Messages.BranchManagerRemarksRequired :
                                                                                            Constants.Messages.BranchManagerRemarksInvalid;
                BranchManagerRemarksError = true;
                flag = false;
            }
            else { BranchManagerRemarksError = false; }


            if(string.IsNullOrEmpty(secondPageFields.ClientSignature))
            {
                ClientSignatureError = true;
                flag = false;
            }
            else { ClientSignatureError = false; }

            if (string.IsNullOrEmpty(secondPageFields.SpouseSignature))
            {
                SpouseSignatureError = true;
                flag = false;
            }
            else { SpouseSignatureError = false; }

            if (string.IsNullOrEmpty(secondPageFields.BranchManagerSignature))
            {
                BranchManagerSignatureError = true;
                flag = false;
            }
            else { BranchManagerSignatureError = false; }


            return flag;
        }
        public IMvxCommand SaveCustomerOrderCommand => new MvxCommand(async () =>
        {
            var error = false;

            try
            {
                if (IsBusy)
                    return;
                var secondPageVM = new SecondPageCOViewModel
                {
                    ClientResCertNo = ClientResCertNo,
                    ClientPlaceIssued = ClientPlaceIssued,
                    ClientDate = ClientDate,
                    ClientSignature = ClientSignature,

                    SpouseResCertNo = SpouseResCertNo,
                    SpousePlaceIssued = SpousePlaceIssued,
                    SpouseDate = SpouseDate,
                    SpouseSignature = SpouseSignature,

                    DeliveryDate = DeliveryDate,
                    DeliveryTime = DeliveryTime,

                    ClosingOfficer = ClosingOfficer,
                    ClosingOfficerDate = ClosingOfficerDate,
                    ClosingOfficerTime = ClosingOfficerTime,
                    ClosingOfficerRemarks = ClosingOfficerRemarks,

                    BranchManagerSignature = BranchManagerSignature,
                    BranchManagerDate = BranchManagerDate,
                    BranchManagerTime = BranchManagerTime,
                    BranchManagerRemarks = BranchManagerRemarks,
                };

                if (IsValidFields(secondPageVM))
                {
                    secondPageVM.ClientResCertNo.Trim();
                    secondPageVM.ClientPlaceIssued.Trim();

                    secondPageVM.SpouseResCertNo.Trim();
                    secondPageVM.SpousePlaceIssued.Trim();

                    secondPageVM.ClosingOfficerRemarks.Trim();
                    secondPageVM.BranchManagerRemarks.Trim();

                    bool confirmSave = await _userDialogs.ConfirmAsync(Constants.Messages.ConfirmSave,
                                                        Constants.Modal.Confirmation,
                                                        Constants.Messages.Yes,
                                                        Constants.Messages.No);

                    if (confirmSave)
                    {

                    }
                }
            }
            catch (Exception)
            {
                error = true;
            }

            if (error)
            {
                var localizedMessage = Constants.Messages.ErrorProcessing;
                await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
        });

        public async Task SaveSignature(int jobOrderID)
        {
            var signatureStringArray = _signatureBytes.Split(Constants.SpecialCharacters.CharComma);

            var signatureBytes = signatureStringArray.Select(byte.Parse).ToArray();

            var signatureFilename = string.Concat(jobOrderID, Constants.Common.SignatureNameExtension);

            var signaturePath = string.Concat(jobOrderID, Constants.Uploads.SignatureTargetFolder);

            LocalJobOrder joForUpdate = MvxApp.Database.GetJobOrderAsync(jobOrderID);

            if (joForUpdate != null)
            {
                joForUpdate.ClientSignature = signatureFilename;
                joForUpdate.StatusID = (int)Constants.Status.Signed;

                MvxApp.Database.SaveJobOrderAsync(joForUpdate);
            }

            await Data.FileAppData.SaveFile(signatureBytes, signatureFilename, signaturePath);
        }

        public IMvxCommand DrawClientSignaturePageCommand => new MvxAsyncCommand(async () =>
        {
            if (!string.IsNullOrEmpty(ClientSignature))
            {
                var param = new Dictionary<string, string>
                {
                    { Constants.Params.SignatureBytes, ClientSignature }
                };

                var result = await _navigationService.Navigate<ViewSignatureViewModel, Dictionary<string, string>, string>(param);

                if (result == Constants.Params.SignAgain)
                {
                    IconFile = "add_button.png";
                    ClientSignature = null;
                }
            }
            else
            {
                ClientSignature = await _navigationService.Navigate<ClientSignatureViewModel, Dictionary<string, string>, string>(_parameter);

                if (!string.IsNullOrEmpty(ClientSignature))
                {
                    IconFile = "view_button.png";
                }
            }
        }, allowConcurrentExecutions: true);

        public IMvxCommand DrawSpouseSignaturePageCommand => new MvxAsyncCommand(async () =>
        {
            if (!string.IsNullOrEmpty(SpouseSignature))
            {
                var param = new Dictionary<string, string>
                {
                    { Constants.Params.SignatureBytes, SpouseSignature }
                };

                var result = await _navigationService.Navigate<ViewSignatureViewModel, Dictionary<string, string>, string>(param);

                if (result == Constants.Params.SignAgain)
                {
                    IconFile = "add_button.png";
                    SpouseSignature = null;
                }
            }
            else
            {
                SpouseSignature = await _navigationService.Navigate<ClientSignatureViewModel, Dictionary<string, string>, string>(_parameter);

                if (!string.IsNullOrEmpty(SpouseSignature))
                {
                    IconFile = "view_button.png";
                }
            }
        }, allowConcurrentExecutions: true);

        public IMvxCommand DrawBranchManagerSignaturePageCommand => new MvxAsyncCommand(async () =>
        {
            if (!string.IsNullOrEmpty(BranchManagerSignature))
            {
                var param = new Dictionary<string, string>
                {
                    { Constants.Params.SignatureBytes, BranchManagerSignature }
                };

                var result = await _navigationService.Navigate<ViewSignatureViewModel, Dictionary<string, string>, string>(param);

                if (result == Constants.Params.SignAgain)
                {
                    IconFile = "add_button.png";
                    BranchManagerSignature = null;
                }
            }
            else
            {
                BranchManagerSignature = await _navigationService.Navigate<ClientSignatureViewModel, Dictionary<string, string>, string>(_parameter);

                if (!string.IsNullOrEmpty(BranchManagerSignature))
                {
                    IconFile = "view_button.png";
                }
            }
        }, allowConcurrentExecutions: true);
    }
}
