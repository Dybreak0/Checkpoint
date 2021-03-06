using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.AssignedCases;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MobileJO.Core.ViewModels
{
    class CaseTaggingViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private readonly IWebService _webService;

        private Dictionary<string, string> _parameter;

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private string _selectedCaseID;

        private SelectableItemWrapper<Models.AssignedCases> _selectedCase;
        public SelectableItemWrapper<Models.AssignedCases> SelectedCase
        {
            get => _selectedCase;
            set
            {
                SetProperty(ref _selectedCase, value);
                if (_selectedCase != null)
                {
                    _selectedCaseID = _selectedCase.Item.ID.ToString();
                    GoToDetails.Execute();
                    SetProperty(ref _selectedCase, null);
                }

            }

        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);

                SearchCase.Execute();
            }
        }

        private bool _noRecords;
        public bool NoRecords
        {
            get => _noRecords;
            set => SetProperty(ref _noRecords, value);
        }

        public ObservableCollection<SelectableItemWrapper<Models.AssignedCases>> AssignedCases { get; private set; } = new ObservableCollection<SelectableItemWrapper<Models.AssignedCases>>();

        public ObservableCollection<SelectableItemWrapper<Models.AssignedCases>> Selection { get; private set; } = new ObservableCollection<SelectableItemWrapper<Models.AssignedCases>>();

        public CaseTaggingViewModel(IMvxNavigationService navigationService, 
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

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;
            
            GetCases.Execute();
        }

        public IMvxCommand GetCases => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                FirstPageViewModel firstPageData = _serializer.DeserializeObject<FirstPageViewModel>(_parameter[Constants.Params.FirstPage]);

                string assignedUser = CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID);

                int applicationTypeId = firstPageData.ApplicationType;
                int accountId = firstPageData.AccountID;

                if (_parameter.ContainsKey(Constants.Params.EditedCasesSelected))
                {
                    if (NetworkCheck.HasInternet())
                    {
                        var serverCases = await _webService.AssignedCaseList(assignedUser,
                                                                             applicationTypeId,
                                                                             accountId);

                        var casesSelected = _serializer.DeserializeObject<List<Models.AssignedCases>>(_parameter[Constants.Params.EditedCasesSelected]);

                        AssignedCases = GetAssignedCases(serverCases);

                        foreach (var activeCase in AssignedCases)
                        {
                            foreach (var caseSelected in casesSelected)
                            {
                                if (caseSelected.ID == activeCase.Item.ID)
                                {
                                    activeCase.IsSelected = true;
                                }
                            }
                        }

                        Selection = AssignedCases;
                    }
                    else
                    {
                        var activeCases = MvxApp.Database.GetUserCasesAsync(assignedUser, applicationTypeId, accountId);

                        var casesSelected = _serializer.DeserializeObject<List<Models.AssignedCases>>(_parameter[Constants.Params.EditedCasesSelected]);

                        AssignedCases = GetAssignedCases(activeCases);

                        foreach (var activeCase in AssignedCases)
                        {
                            foreach (var caseSelected in casesSelected)
                            {
                                if (caseSelected.ID == activeCase.Item.ID)
                                {
                                    activeCase.IsSelected = true;
                                }
                            }
                        }

                        Selection = AssignedCases;
                    }
                    
                    NoRecords = AssignedCases.Count > 0 ? false : true;
                }
                else
                {
                    

                    if (NetworkCheck.HasInternet())
                    {
                        var serverCases = await _webService.AssignedCaseList(assignedUser, 
                                                                             applicationTypeId, 
                                                                             accountId);

                        AssignedCases = GetAssignedCases(serverCases);

                        Selection = AssignedCases;
                    }
                    else
                    {
                        var localCases = new List<Models.AssignedCases>();

                        localCases = MvxApp.Database.GetUserCasesAsync(assignedUser, 
                                                                             applicationTypeId, 
                                                                             accountId);

                        AssignedCases = GetAssignedCases(localCases);

                        Selection = AssignedCases;
                    }
                                        
                    NoRecords = AssignedCases.Count > 0 ? false : true;
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

        public ObservableCollection<SelectableItemWrapper<Models.AssignedCases>> GetAssignedCases(List<Models.AssignedCases> cases)
        {
            var tempCases = new ObservableCollection<SelectableItemWrapper<Models.AssignedCases>>();

            foreach (var activeCase in cases)
            {
                tempCases.Add(new SelectableItemWrapper<Models.AssignedCases>
                {
                    Item = activeCase
                });
            }

            return tempCases;
        }

        private ObservableCollection<Models.AssignedCases> GetSelectedCases()
        {
            var selected = AssignedCases
                .Where(p => p.IsSelected)
                .Select(p => p.Item)
                .ToList();
            return new ObservableCollection<Models.AssignedCases>(selected);
        }

        public IMvxCommand GoToSelectedCasesPageCommand => new MvxCommand(async () =>
        {
            var selectedCases = GetSelectedCases();

            if(AssignedCases.Count > 0 && selectedCases.Count <= 0)
            {
                await _userDialogs.AlertAsync(Constants.Messages.CaseRequired, 
                                              Constants.Modal.Warning, 
                                              Constants.Common.OK);
                return;
            }
            
            var serializedCases = _serializer.SerializeObject(selectedCases);

            if (_parameter.ContainsKey(Constants.Params.Cases))
            {
                _parameter[Constants.Params.Cases] = serializedCases;
            }
            else
            {
                _parameter.Add(Constants.Params.Cases, serializedCases);
            }

            _parameter.Remove(Constants.Params.FromSecondPage);

            await _navigationService.Navigate<CasesSelectedViewModel, Dictionary<string, string>>(_parameter);                        
        });

        private IMvxCommand closeCommand;
        public IMvxCommand CloseCommand => closeCommand ?? (closeCommand = new MvxCommand(() => _navigationService.Close(this)));

        public IMvxCommand RefreshCaseList => new MvxCommand(async () =>
        {
            IsRefreshing = true;

            var error = false;

            try
            {
                AssignedCases = new ObservableCollection<SelectableItemWrapper<Models.AssignedCases>>();
                GetCases.Execute();
            }
            catch (Exception)
            {
                error = true;
            }
            finally
            {
                IsRefreshing = false;
            }

            if (error)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
        });

        public IMvxCommand GoToDetails => new MvxCommand(async () =>
        {
            var param = new Dictionary<string, string>
            {
                { Constants.Keys.ID, _selectedCaseID }
            };

            await _navigationService.Navigate<CaseDetailsViewModel, Dictionary<string, string>>(param);
        });

        public IMvxAsyncCommand SearchCase => new MvxAsyncCommand(async () =>
        {
            if (string.IsNullOrWhiteSpace(_searchText))
                Selection = AssignedCases;

            Selection = new ObservableCollection<SelectableItemWrapper<Models.AssignedCases>>(AssignedCases
                                                                                                        .Where(c =>
                                                                                                        c.Item.CaseSubject.ToLower().Contains(_searchText.ToLower()) ||
                                                                                                        c.Item.CaseNumber.ToString().Contains(_searchText)));
            NoRecords = Selection.Count > 0 ? false : true;
        });
    }
}
