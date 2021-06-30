using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.CustomerOrderViewModels;
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

        List<UnitDesiredModel> deserializedUnitDesired = new List<UnitDesiredModel>();
        public List<UnitDesiredModel> UnitDesireds { get; private set; } = new List<UnitDesiredModel>();
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

            if (string.IsNullOrEmpty(secondPageFields.ClientSignature))
            {
                ClientSignatureErrorMsg = Constants.Messages.ClientSignatureRequired;
                ClientSignatureError = true;
                flag = false;
            }
            else { ClientSignatureError = false; }


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

            if (string.IsNullOrEmpty(secondPageFields.SpouseSignature))
            {
                SpouseSignatureErrorMsg = Constants.Messages.SpouseSignatureRequired;
                SpouseSignatureError = true;
                flag = false;
            }
            else { SpouseSignatureError = false; }


            if (string.IsNullOrWhiteSpace(secondPageFields.ClosingOfficer) || !Regex.IsMatch(secondPageFields.ClosingOfficer, Constants.Common.TextRegex))
            {
                ClosingOfficerErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.ClosingOfficer) ?
                                                                                            Constants.Messages.ClosingOfficerRequired :
                                                                                            Constants.Messages.ClosingOfficerInvalid;
                ClosingOfficerError = true;
                flag = false;
            }
            else { ClosingOfficerError = false; }

            if (string.IsNullOrWhiteSpace(secondPageFields.ClosingOfficerRemarks) || !Regex.IsMatch(secondPageFields.ClosingOfficerRemarks, Constants.Common.TextRegex))
            {
                ClosingOfficerRemarksErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.ClosingOfficerRemarks) ?
                                                                                            Constants.Messages.ClosingOfficerRemarksRequired :
                                                                                            Constants.Messages.ClosingOfficerRemarksInvalid;
                ClosingOfficerRemarksError = true;
                flag = false;
            }
            else { ClosingOfficerRemarksError = false; }

            if (string.IsNullOrEmpty(secondPageFields.BranchManagerSignature))
            {
                BranchManagerSignatureErrorMsg = Constants.Messages.BranchManagerRequired;
                BranchManagerSignatureError = true;
                flag = false;
            }
            else { BranchManagerSignatureError = false; }

            if (string.IsNullOrWhiteSpace(secondPageFields.BranchManagerRemarks) || !Regex.IsMatch(secondPageFields.BranchManagerRemarks, Constants.Common.TextRegex))
            {
                BranchManagerRemarksErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.BranchManagerRemarks) ?
                                                                                            Constants.Messages.BranchManagerRemarksRequired :
                                                                                            Constants.Messages.BranchManagerRemarksInvalid;
                BranchManagerRemarksError = true;
                flag = false;
            }
            else { BranchManagerRemarksError = false; }

            return flag;
        }
        public IMvxCommand SaveCustomerOrderCommand => new MvxCommand(async () =>
        {
            var error = false;

            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
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
                        FirstPageCOViewModel deserializedFirstPage = _serializer.DeserializeObject<FirstPageCOViewModel>(_parameter[Constants.Params.FirstPage]);
                        string serializedUnitDesired = _serializer.SerializeObject(deserializedFirstPage.UnitDesiredDDL);
                        string customerOrderStatus = !string.IsNullOrEmpty(BranchManagerSignature) ? "Approved" : "Pending";
                        int userID = int.Parse(CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID));
                        int insertID = 0;
                        if (NetworkCheck.HasInternet())
                        {
                            deserializedUnitDesired = _serializer.DeserializeObject<List<UnitDesiredModel>>(serializedUnitDesired);
                            foreach (var unit in deserializedUnitDesired)
                            {
                                UnitDesireds.Add(unit);
                            }
                            FileViewModel clientSignatureModel = null;

                            if (!string.IsNullOrEmpty(secondPageVM.ClientSignature))
                            {
                                var signatureStringArray = secondPageVM.ClientSignature.Split(Constants.SpecialCharacters.CharComma);
                                var signatureBytes = signatureStringArray.Select(byte.Parse).ToArray();

                                clientSignatureModel = new FileViewModel
                                {
                                    FileName = Constants.Common.ClientSignatureNameExtension,
                                    FileDataArray = signatureBytes
                                };
                            }
                            FileViewModel spouseSignatureModel = null;

                            if (!string.IsNullOrEmpty(secondPageVM.SpouseSignature))
                            {
                                var signatureStringArray = secondPageVM.SpouseSignature.Split(Constants.SpecialCharacters.CharComma);
                                var signatureBytes = signatureStringArray.Select(byte.Parse).ToArray();

                                spouseSignatureModel = new FileViewModel
                                {
                                    FileName = Constants.Common.SpouseSignatureNameExtension,
                                    FileDataArray = signatureBytes
                                };
                            }
                            FileViewModel branchManagerSignatureModel = null;

                            if (!string.IsNullOrEmpty(secondPageVM.BranchManagerSignature))
                            {
                                var signatureStringArray = secondPageVM.BranchManagerSignature.Split(Constants.SpecialCharacters.CharComma);
                                var signatureBytes = signatureStringArray.Select(byte.Parse).ToArray();

                                branchManagerSignatureModel = new FileViewModel
                                {
                                    FileName = Constants.Common.BranchManagerSignatureNameExtension,
                                    FileDataArray = signatureBytes
                                };
                            }

                            CustomerOrderDetailsViewModel customerOrderViewModel = new CustomerOrderDetailsViewModel
                            {
                                CustomerOrderNumber = null,
                                BranchID = deserializedFirstPage.SelectedBranch,
                                CustomerOrderStatus = "Pending",
                                Name = deserializedFirstPage.Name,
                                SpouseName = deserializedFirstPage.Spouse,
                                DeliveryAddress = deserializedFirstPage.DeliveryAddress,
                                OfficialReceipt = deserializedFirstPage.OfficialReceipt,
                                TotalAmount = deserializedFirstPage.Total.ToString(),
                                ClientResCertNo = secondPageVM.ClientResCertNo,
                                ClientPlaceIssued = deserializedFirstPage.Name,
                                ClientDate = secondPageVM.ClientDate,
                                SpouseResCertNo = secondPageVM.SpouseResCertNo,
                                SpousePlaceIssued = secondPageVM.SpousePlaceIssued,
                                SpouseDate = secondPageVM.SpouseDate,
                                DeliveryDate = secondPageVM.DeliveryDate,
                                DeliveryTime = secondPageVM.DeliveryTime,
                                ClosingOfficer = secondPageVM.ClosingOfficer,
                                ClosingOfficerDate = secondPageVM.ClosingOfficerDate,
                                ClosingOfficerTime = secondPageVM.ClosingOfficerTime,
                                ClosingOfficerRemarks = secondPageVM.ClosingOfficerRemarks,
                                BranchManagerDate = secondPageVM.BranchManagerDate,
                                BranchManagerTime = secondPageVM.BranchManagerTime,
                                BranchManagerRemarks = secondPageVM.BranchManagerRemarks,
                                UnitDesireds = UnitDesireds,
                                ClientSignature = clientSignatureModel,
                                SpouseSignature = spouseSignatureModel,
                                BranchManagerSignature = branchManagerSignatureModel
                            };

                            CustomerOrderDetailsViewModel createCOResponse = await _webService.SaveCustomerOrderDetails(customerOrderViewModel);

                            await _userDialogs.AlertAsync(Constants.Messages.SaveToServerSuccess,
                                                      Constants.Modal.InfoMessage,
                                                      Constants.Common.OK);
                        }
                        else
                        {
                            //OFFLINE
                        }
                    }
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
                var localizedMessage = Constants.Messages.ErrorProcessing;
                await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
        });

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
                    DrawClientSignaturePageCommand.Execute();
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
                    DrawSpouseSignaturePageCommand.Execute();
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
                    DrawBranchManagerSignaturePageCommand.Execute();
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
