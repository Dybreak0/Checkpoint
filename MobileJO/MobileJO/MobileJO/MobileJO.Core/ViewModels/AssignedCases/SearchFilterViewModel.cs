using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileJO.Core.ViewModels.AssignedCases
{
    public class SearchFilterViewModel : BaseViewModel
    {

        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;
        private List<string> _applicationType;
        private List<string> _caseStatus;
        private string _caseNumber;
        private string _selectedCaseStatus;
        private string _selectedApplicationType;

        public string CaseNumber
        {
            get => _caseNumber;
            set => SetProperty(ref _caseNumber, value);
        }

        public List<string> ApplicationType
        {
            get => _applicationType;
            set => SetProperty(ref _applicationType, value);
        }

        public List<string> CaseStatus
        {
            get => _caseStatus;
            set => SetProperty(ref _caseStatus, value);
        }

        public string SelectedCaseStatus
        {
            get => _selectedCaseStatus;
            set => SetProperty(ref _selectedCaseStatus, value);
        }

        public string SelectedApplicationType
        {
            get => _selectedApplicationType;
            set => SetProperty(ref _selectedApplicationType, value);
        }

        public SearchFilterViewModel(IMvxNavigationService navigationService,
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

        public override void Prepare(Dictionary<string, string> parameter)
        {
            LoadPickers.Execute();
        }

        private IMvxAsyncCommand LoadPickers => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    ApplicationType =  new List<string>(await _webService.GetApplicationType());
                    ApplicationType.Add(Constants.Common.All);
                    ApplicationType.Sort();
                    SelectedApplicationType = Constants.Common.All;

                    CaseStatus = new List<string>(await _webService.GetCaseStatus());
                    CaseStatus.Add(Constants.Common.All);
                    CaseStatus.Sort();
                    SelectedCaseStatus = Constants.Common.All;
                }
                else
                {
                    var appTypes = MvxApp.Database.GetAllApplicationTypesAsync();
                    ApplicationType = appTypes.Select(x => x.ApplicationName).ToList();
                    ApplicationType.Add(Constants.Common.All);
                    ApplicationType.Sort();
                    SelectedApplicationType = Constants.Common.All;

                    var caseStatuses = MvxApp.Database.GetAllCasesStatus();
                    CaseStatus = caseStatuses.Select(x => x.StatusName).ToList();
                    CaseStatus.Add(Constants.Common.All);
                    CaseStatus.Sort();
                    SelectedCaseStatus = Constants.Common.All;
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
            }


        });

        public IMvxCommand Close => new MvxCommand(async () =>
        {
            await _navigationService.Close(this);
        });


        public IMvxCommand Reset => new MvxCommand(() =>
        {
            CaseNumber = string.Empty;
            SelectedCaseStatus = Constants.Common.All;
            SelectedApplicationType = Constants.Common.All;
            OnPropertyChanged(Constants.Params.CaseNumber);
        });

        public IMvxAsyncCommand GoToAssignedCasesList => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string>();

            param.Add(Constants.Params.AssignedTo, CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID));

            if(!string.IsNullOrEmpty(CaseNumber))
            {
                param.Add(Constants.Params.CaseNumber, CaseNumber);
            }

            if (SelectedCaseStatus.Equals(Constants.Common.All))
            {
                param.Add(Constants.Params.CaseStatus, string.Empty);
            }
            else
            {
                param.Add(Constants.Params.CaseStatus, SelectedCaseStatus);
            }

            if (_selectedApplicationType.Equals(Constants.Common.All))
            {
                param.Add(Constants.Params.ApplicationType, string.Empty);
            }
            else
            {
                param.Add(Constants.Params.ApplicationType, SelectedApplicationType);
            }
 
            await _navigationService.Navigate<AssignedCasesListViewModel, Dictionary<string, string>>(param);
        });

    }
}
