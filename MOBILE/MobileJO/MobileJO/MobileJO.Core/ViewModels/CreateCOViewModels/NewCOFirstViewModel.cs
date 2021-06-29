using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using MobileJO.Core.Base;
using MvvmCross.Navigation;
using MobileJO.Core.Contracts;
using MvvmCross.Commands;
using MobileJO.Core.Utilities;
using MobileJO.Core.Models;
using MvvmCross.Base;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using MobileJO.Core.ViewModels.FieldCOViewModels;
using MobileJO.Core.ViewModels.Common;

namespace MobileJO.Core.ViewModels.CreateCOViewModels
{
    public class NewCOFirstViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private readonly IWebService _webService;

        public NewCOFirstViewModel(IMvxNavigationService navigationService,
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
        private int height;
        public int Height
        {
            get => height;
            set => SetProperty(ref height, value);
        }
        private string _addedUnitDesired { get; set; }

        private List<DropdownViewModel> _branch;
        public List<DropdownViewModel> Branch
        {
            get => _branch;
            set => SetProperty(ref _branch, value);
        }

        public DropdownViewModel SelectedBranch { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Spouse { get; set; }
        public string DeliveryAddress { get; set; }
        public ObservableCollection<UnitDesiredModel> UnitDesiredDDL { get; private set; } = new ObservableCollection<UnitDesiredModel>();

        public decimal Total { get; set; }
        public string OfficialReceipt { get; set; }

        //Error Boolean
        public bool SelectedBranchError { get; set; }
        public bool LastNameError { get; set; }
        public bool FirstNameError { get; set; }
        public bool MiddleNameError { get; set; }
        public bool SpouseError { get; set; }
        public bool DeliveryAddressError { get; set; }
        public bool UnitDesiredDDLError { get; set; }
        public bool OfficialReceiptError { get; set; }

        //Error Message
        public string LastNameErrorMsg { get; set; }
        public string FirstNameErrorMsg { get; set; }
        public string MiddleNameErrorMsg { get; set; }
        public string SpouseErrorMsg { get; set; }
        public string DeliveryAddressErrorMsg { get; set; }
        public string OfficialReceiptErrorMsg { get; set; }

        private UnitDesiredModel _selectedUnitDesired;

        public UnitDesiredModel SelectedUnitDesired
        {
            get => _selectedUnitDesired;
            set => SetProperty(ref _selectedUnitDesired, value);
        }
        public override void Prepare()
        {
            Height = (UnitDesiredDDL.Count * 18);
            UnitDesiredDDLError = UnitDesiredDDL.Count > 0 ? false : true;
            LoadPickerDataCommand.Execute();
        }
        public IMvxCommand AddUnitDesiredCommand => new MvxCommand(async () =>
        {
            var param = new Dictionary<string, string>
                {
                    { "AddUnitDesired", _addedUnitDesired }
                };

            var result = await _navigationService.Navigate<AddUnitDesiredViewModel, Dictionary<string, string>, string>(param);
            if (!string.IsNullOrEmpty(result))
            {
                var deserializedUnitDesired = _serializer.DeserializeObject<UnitDesiredModel>(result);

                UnitDesiredDDL.Add(deserializedUnitDesired);
                Height = (UnitDesiredDDL.Count * 18);
                UnitDesiredDDLError = UnitDesiredDDL.Count > 0 ? false : true;

                Total += Convert.ToDecimal(deserializedUnitDesired.DesiredAmount);
            }
        });
        public IMvxCommand RemoveUnitDesiredCommand => new MvxCommand(() =>
        {
            UnitDesiredDDL.Remove(SelectedUnitDesired);
            Height = (UnitDesiredDDL.Count * 18);
            UnitDesiredDDLError = UnitDesiredDDL.Count > 0 ? false : true;
        });
        public IMvxCommand LoadPickerDataCommand => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    Branch = new List<DropdownViewModel>(await _webService.GetBranches(Convert.ToInt32(_settings.BranchID)));
                }
                else
                {
                    //OFFLINE
                    Branch = MvxApp.Database.GetBranches(Convert.ToInt32(_settings.BranchID));
                }
            }
            catch (Exception)
            {
                error = true;
            }
            finally
            {
                IsBusy = false;
            }

            if (error)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                await _navigationService.Close(this);
            }

        });

        public bool IsValidFields(FirstPageCOViewModel firstPageFields)
        {
            bool flag = true;

            if (string.IsNullOrWhiteSpace(firstPageFields.LastName) || !Regex.IsMatch(firstPageFields.LastName, Constants.Common.TextRegex))
            {
                LastNameErrorMsg = string.IsNullOrWhiteSpace(firstPageFields.LastName) ?
                                                                                            Constants.Messages.LastNameRequired :
                                                                                            Constants.Messages.LastNameInvalid;
                LastNameError = true;
                flag = false;
            }
            else { LastNameError = false; }


            if (string.IsNullOrWhiteSpace(firstPageFields.FirstName) || !Regex.IsMatch(firstPageFields.FirstName, Constants.Common.TextRegex))
            {
                FirstNameErrorMsg = string.IsNullOrWhiteSpace(firstPageFields.FirstName) ?
                                                                                            Constants.Messages.FirstNameRequired :
                                                                                            Constants.Messages.FirstNameInvalid;
                FirstNameError = true;
                flag = false;
            }
            else { FirstNameError = false; }

            if (string.IsNullOrWhiteSpace(firstPageFields.MiddleName) || !Regex.IsMatch(firstPageFields.MiddleName, Constants.Common.TextRegex))
            {
                MiddleNameErrorMsg = string.IsNullOrWhiteSpace(firstPageFields.MiddleName) ?
                                                                                            Constants.Messages.MiddleNameRequired :
                                                                                            Constants.Messages.MiddleNameInvalid;
                MiddleNameError = true;
                flag = false;
            }
            else { MiddleNameError = false; }

            if (string.IsNullOrWhiteSpace(firstPageFields.Spouse) || !Regex.IsMatch(firstPageFields.Spouse, Constants.Common.TextRegex))
            {
                SpouseErrorMsg = string.IsNullOrWhiteSpace(firstPageFields.Spouse) ?
                                                                                            Constants.Messages.SpouseRequired :
                                                                                            Constants.Messages.SpouseInvalid;
                SpouseError = true;
                flag = false;
            }
            else { SpouseError = false; }

            if (string.IsNullOrWhiteSpace(firstPageFields.DeliveryAddress) || !Regex.IsMatch(firstPageFields.DeliveryAddress, Constants.Common.TextRegex))
            {
                DeliveryAddressErrorMsg = string.IsNullOrWhiteSpace(firstPageFields.DeliveryAddress) ?
                                                                                            Constants.Messages.DeliveryAddressRequired :
                                                                                            Constants.Messages.DeliveryAddressInvalid;
                DeliveryAddressError = true;
                flag = false;
            }
            else { DeliveryAddressError = false; }

            if (string.IsNullOrWhiteSpace(firstPageFields.OfficialReceipt) || !Regex.IsMatch(firstPageFields.OfficialReceipt, Constants.Common.TextRegex))
            {
                OfficialReceiptErrorMsg = string.IsNullOrWhiteSpace(firstPageFields.OfficialReceipt) ?
                                                                                            Constants.Messages.OfficialReceiptRequired :
                                                                                            Constants.Messages.OfficialReceiptInvalid;
                OfficialReceiptError = true;
                flag = false;
            }
            else { OfficialReceiptError = false; }

            if (firstPageFields.SelectedBranch <= 0)
            {
                SelectedBranchError = true;
                flag = false;
            }
            else { SelectedBranchError = false; }

            if (!(firstPageFields.UnitDesiredDDL.Count > 0))
            {
                UnitDesiredDDLError = true;
                flag = false;
            }
            else { UnitDesiredDDLError = false; }

            return flag;
        }

        public IMvxCommand GoToSecondPageCommand => new MvxCommand(async () =>
        {
            var error = false;

            try
            {
                if (IsBusy)
                    return;

                var firstPageVM = new FirstPageCOViewModel
                {
                    SelectedBranch = SelectedBranch != null ? SelectedBranch.Value : 0,
                    LastName = LastName,
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    Spouse = Spouse,
                    DeliveryAddress = DeliveryAddress,
                    UnitDesiredDDL = UnitDesiredDDL,
                    Total = Total,
                    OfficialReceipt = OfficialReceipt,
                };

                if (IsValidFields(firstPageVM))
                {
                    firstPageVM.LastName.Trim();
                    firstPageVM.FirstName.Trim();
                    firstPageVM.MiddleName.Trim();
                    firstPageVM.Spouse.Trim();
                    firstPageVM.OfficialReceipt.Trim();

                    var firstPageJsonText = _serializer.SerializeObject(firstPageVM);

                    var param = new Dictionary<string, string> { };

                    if (param.ContainsKey(Constants.Params.FirstPage))
                    {
                        param[Constants.Params.FirstPage] = firstPageJsonText;
                    }
                    else
                    {
                        param.Add(Constants.Params.FirstPage, firstPageJsonText);
                    }
                    SelectedBranchError = false;
                    LastNameError = false;
                    FirstNameError = false;
                    SpouseError = false;
                    DeliveryAddressError = false;
                    UnitDesiredDDLError = false;
                    OfficialReceiptError = false;

                    await _navigationService.Navigate<NewCOSecondViewModel, Dictionary<string, string>>(param);
                }
            }
            catch (Exception ex)
            {
                error = true;
            }

            if (error)
            {
                var localizedMessage = Constants.Messages.ErrorProcessing;
                await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }

        });

    }

}
