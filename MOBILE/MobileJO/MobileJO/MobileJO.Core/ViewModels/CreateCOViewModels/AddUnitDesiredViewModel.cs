
using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using MobileJO.Core.Base;
using MvvmCross.Navigation;
using MobileJO.Core.Contracts;
using MvvmCross.Commands;
using MobileJO.Core.Utilities;
using MobileJO.Core.Models;
using SignaturePad.Forms;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;
using MvvmCross.Forms.Presenters.Attributes;
using System.Linq;
using MvvmCross.Base;
using System.Text.RegularExpressions;

namespace MobileJO.Core.ViewModels.CreateCOViewModels
{
    public class AddUnitDesiredViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly IMvxJsonConverter _serializer;
        private readonly ILocalizeService _localizeService;

        private UnitDesiredModel _addedUnitDesired { get; set; }

        public AddUnitDesiredViewModel(IMvxNavigationService navigationService,
                                        IAppSettings settings,
                                        IUserDialogs userDialogs,
                                        IMvxJsonConverter serializer,
                                        ILocalizeService localizeService) : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _userDialogs = userDialogs;
            _serializer = serializer;
            _localizeService = localizeService;
        }
        private string unitDesiredJsonText { get; set; }
        public string DesiredBrandModel { get; set; }
        public string DesiredSerialNo { get; set; }
        public string DesiredCode { get; set; }
        public string DesiredAmount { get; set; }
        public string DesiredAccounting { get; set; }

        //Error Boolean
        public bool DesiredBrandModelError { get; set; }
        public bool DesiredSerialNoError { get; set; }
        public bool DesiredCodeError { get; set; }
        public bool DesiredAmountError { get; set; }
        public bool DesiredAccountingError { get; set; }

        //Error Message
        public string DesiredBrandModelErrorMsg { get; set; }
        public string DesiredSerialNoErrorMsg { get; set; }
        public string DesiredCodeErrorMsg { get; set; }
        public string DesiredAmountErrorMsg { get; set; }
        public string DesiredAccountingtErrorMsg { get; set; }

        public IMvxCommand CloseCommand => new MvxCommand(async () =>
        {
            await _navigationService.Close(this, unitDesiredJsonText);
        });

        public IMvxCommand SaveUnitDesired => new MvxCommand(async () =>
        {
            var error = false;

            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;

                var unitDesired = new UnitDesiredModel
                {
                    DesiredBrandModel = DesiredBrandModel,
                    DesiredSerialNo = DesiredSerialNo,
                    DesiredCode = DesiredCode,
                    DesiredAmount = DesiredAmount,
                    DesiredAccounting = DesiredAccounting
                };

                if (IsValidFields(unitDesired))
                {
                    unitDesired.DesiredBrandModel.Trim();
                    unitDesired.DesiredSerialNo.Trim();
                    unitDesired.DesiredCode.Trim();
                    unitDesired.DesiredAmount.Trim();
                    unitDesired.DesiredAccounting.Trim();

                    var unitDesiredJsonText = _serializer.SerializeObject(unitDesired);

                    DesiredBrandModelError = false;
                    DesiredSerialNoError = false;
                    DesiredCodeError = false;
                    DesiredAmountError = false;
                    DesiredAccountingError = false;

                    await _navigationService.Close(this, unitDesiredJsonText);
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
                var localizedMessage = Constants.Messages.ErrorProcessing;
                await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }

        });
        public bool IsValidFields(UnitDesiredModel unitDesiredFields)
        {
            bool flag = true;

            if (string.IsNullOrWhiteSpace(unitDesiredFields.DesiredBrandModel) || !Regex.IsMatch(unitDesiredFields.DesiredBrandModel, Constants.Common.TextRegex))
            {
                DesiredBrandModelErrorMsg = string.IsNullOrWhiteSpace(unitDesiredFields.DesiredBrandModel) ?
                                                                                            Constants.Messages.DesiredBrandModelRequired :
                                                                                            Constants.Messages.DesiredBrandModelInvalid;
                DesiredBrandModelError = true;
                flag = false;
            }
            else { DesiredBrandModelError = false; }

            if (string.IsNullOrWhiteSpace(unitDesiredFields.DesiredSerialNo) || !Regex.IsMatch(unitDesiredFields.DesiredSerialNo, Constants.Common.TextRegex))
            {
                DesiredSerialNoErrorMsg = string.IsNullOrWhiteSpace(unitDesiredFields.DesiredSerialNo) ?
                                                                                            Constants.Messages.DesiredSerialNoRequired :
                                                                                            Constants.Messages.DesiredSerialNoInvalid;
                DesiredSerialNoError = true;
                flag = false;
            }
            else { DesiredSerialNoError = false; }

            if (string.IsNullOrWhiteSpace(unitDesiredFields.DesiredCode) || !Regex.IsMatch(unitDesiredFields.DesiredCode, Constants.Common.TextRegex))
            {
                DesiredCodeErrorMsg = string.IsNullOrWhiteSpace(unitDesiredFields.DesiredCode) ?
                                                                                            Constants.Messages.DesiredCodeRequired :
                                                                                            Constants.Messages.DesiredCodeInvalid;
                DesiredCodeError = true;
                flag = false;
            }
            else { DesiredCodeError = false; }

            if (string.IsNullOrWhiteSpace(unitDesiredFields.DesiredAmount) || !Regex.IsMatch(unitDesiredFields.DesiredAmount, Constants.Common.DecimalRegex))
            {
                DesiredAmountErrorMsg = string.IsNullOrWhiteSpace(unitDesiredFields.DesiredAmount) ?
                                                                                            Constants.Messages.DesiredAmountRequired :
                                                                                            Constants.Messages.DesiredAmountInvalid;
                DesiredAmountError = true;
                flag = false;
            }
            else { DesiredAmountError = false; }

            if (string.IsNullOrWhiteSpace(unitDesiredFields.DesiredAccounting) || !Regex.IsMatch(unitDesiredFields.DesiredAccounting, Constants.Common.TextRegex))
            {
                DesiredAccountingtErrorMsg = string.IsNullOrWhiteSpace(unitDesiredFields.DesiredAccounting) ?
                                                                                            Constants.Messages.DesiredAccountingRequired :
                                                                                            Constants.Messages.DesiredAccountingInvalid;
                DesiredAccountingError = true;
                flag = false;
            }
            else { DesiredAccountingError = false; }

            return flag;
        }
    }
}
