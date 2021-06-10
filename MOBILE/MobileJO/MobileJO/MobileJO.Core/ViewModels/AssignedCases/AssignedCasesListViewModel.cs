using MvvmCross.Commands;
using MvvmCross.Navigation;
using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using System.Collections.ObjectModel;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Core.ViewModels.AssignedCases
{
    public class AssignedCasesListViewModel : BaseViewModel
    {
        public AssignedCasesListViewModel(IMvxNavigationService navigationService,
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

        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;

        public ObservableCollection<AssignedCasesList> AssignedCases { get; private set; } = new ObservableCollection<AssignedCasesList>();

        private string _selectedCaseID;

        private AssignedCasesList _selectedCase;
        public AssignedCasesList SelectedCase
        {
            get => _selectedCase;
            set
            {
                SetProperty(ref _selectedCase, value);
                if(_selectedCase != null)
                {
                    _selectedCaseID = _selectedCase.id.ToString();
                    GoToDetails.Execute();
                    SetProperty(ref _selectedCase, null);
                }
                
            }
                
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private bool _hasRecords;
        public bool HasRecords
        {
            get => _hasRecords;
            set => SetProperty(ref _hasRecords, value);
        }

        private bool _showError;
        public bool ShowError
        {
            get => _showError;
            set => SetProperty(ref _showError, value);
        }

        private bool _canLoadMoreData;
        public bool CanLoadMoreData
        {
            get => _canLoadMoreData;
            set => SetProperty(ref _canLoadMoreData, value);
        }
        
        private Dictionary<string, string> _parameter;
        private int _totalPages;
        private int _totalRecords;
        private int _currentPage = 1;
        AssignedCasesSearchViewModel OfflineSearchParams { get; set; }

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;
            _parameter.Add(Constants.Params.Page, _currentPage.ToString());
            _parameter.Add(Constants.Params.PageSize, "7");

            OfflineSearchParams = new AssignedCasesSearchViewModel
            {
                assigned_to      = int.Parse(_parameter[Constants.Params.AssignedTo]),
                case_number      = _parameter.ContainsKey(Constants.Params.CaseNumber) ? _parameter[Constants.Params.CaseNumber] : string.Empty,
                status           = _parameter.ContainsKey(Constants.Params.CaseStatus) ? _parameter[Constants.Params.CaseStatus] : string.Empty,
                application_type = _parameter.ContainsKey(Constants.Params.ApplicationType) ? _parameter[Constants.Params.ApplicationType] : string.Empty,
                page             = int.Parse(_parameter[Constants.Params.Page]),
                page_size        = int.Parse(_parameter[Constants.Params.PageSize])
            };

            LoadList.Execute();
        }

        public IMvxAsyncCommand GoToDetails => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string>
            {
                { Constants.Keys.ID, _selectedCaseID }
            };
            
            await _navigationService.Navigate<AssignedCasesDetailsViewModel, Dictionary<string, string>>(param);
        });

        public IMvxAsyncCommand LoadMore => new MvxAsyncCommand(async() => 
        {
            if(_currentPage < _totalPages)
            {
                _currentPage++;
                CanLoadMoreData = (_currentPage == _totalPages) ? false : true;
            }
            else
            {
                CanLoadMoreData = (_currentPage == _totalPages) ? false : true;
            }
            _parameter[Constants.Params.Page] = _currentPage.ToString();
            OfflineSearchParams.page = _currentPage;

            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    var result = await _webService.AssignedCasesList(_parameter);

                    foreach (AssignedCasesList row in result.Data)
                    {
                        AssignedCases.Add(row);
                    }

                }
                else
                {
                    var result = MvxApp.Database.GetAssignedCasesList(OfflineSearchParams);

                    foreach (AssignedCasesList row in result.Data)
                    {
                        AssignedCases.Add(row);
                    }
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

        public IMvxAsyncCommand RefreshList => new MvxAsyncCommand(async () =>
        {
            IsRefreshing = true;

            _currentPage = 1;
            _parameter[Constants.Params.Page] = _currentPage.ToString(); 

            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    AssignedCases = new ObservableCollection<AssignedCasesList>();
                    LoadList.Execute();
                }
                else
                {
                    var localizedMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                    await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                }
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
        
        public IMvxCommand GoToSearchFilter => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            var param = new Dictionary<string, string>
            {
                { Constants.Keys.ID, string.Empty }
            };
            await _navigationService.Navigate<SearchFilterViewModel, Dictionary<string, string>>(param);
        });

        private IMvxAsyncCommand LoadList => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;
            try
            {
                if (NetworkCheck.HasInternet())
                {
                    var result = await _webService.AssignedCasesList(_parameter);

                    _totalPages = result.Pagination.Pages;
                    _totalRecords = result.Pagination.Size;

                    if(_totalRecords == 0)
                    {
                        ShowError = true;
                    }
                    else
                    {
                        ShowError = false;
                        foreach (AssignedCasesList row in result.Data)
                        {
                            AssignedCases.Add(row);
                        }

                        if(_totalRecords > 7)
                        {
                            CanLoadMoreData = true;
                        }
                        else
                        {
                            CanLoadMore = false;
                        }
                    }

                    HasRecords = true;

                }
                else
                {                    
                    var result = MvxApp.Database.GetAssignedCasesList(OfflineSearchParams);

                    _totalPages = result.Pagination.Pages;
                    _totalRecords = result.Pagination.Size;

                    if (_totalRecords == 0)
                    {
                        ShowError = true;
                    }
                    else
                    {
                        ShowError = false;
                        foreach (AssignedCasesList row in result.Data)
                        {
                            AssignedCases.Add(row);
                        }

                        if (_totalRecords > 7)
                        {
                            CanLoadMoreData = true;
                        }
                        else
                        {
                            CanLoadMore = false;
                        }
                    }

                    HasRecords = true;
                }
            }
            catch(Exception ex)
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

        public IMvxAsyncCommand GoToMenuPageCommand => new MvxAsyncCommand(async () =>
        {
            await _navigationService.Navigate<MainViewModel>();
        });

        public IMvxAsyncCommand GoToMainMenu => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string> { };

            await _navigationService.Navigate<MainViewModel, Dictionary<string, string>>(param);
        });

    }

}