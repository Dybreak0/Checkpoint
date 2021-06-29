
using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.CreateCOViewModels;
using MobileJO.Core.ViewModels.CustomerOrderViewModels;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MobileJO.Core.ViewModels
{

    public class CustomerOrderListViewModel : BaseViewModel
    {
        public CustomerOrderListViewModel(IMvxNavigationService navigationService, IAppSettings settings,
            IUserDialogs userDialogs, ILocalizeService localizeService, IWebService webService)
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

        public ObservableCollection<CustomerOrderModel> _customerOrder { get; set; } = new ObservableCollection<CustomerOrderModel>();

        private int customerOrderID;
        private int serverCustomerOrderID;
        public string customerOrderNumber { get; set; }
        public string status { get; set; }

        private CustomerOrderModel _selectedCustomerOrder;
        public CustomerOrderModel SelectedCustomerOrder
        {
            get => _selectedCustomerOrder;
            set
            {
                SetProperty(ref _selectedCustomerOrder, value);
                customerOrderID = _selectedCustomerOrder.ID;
                serverCustomerOrderID = _selectedCustomerOrder.ServerID;
                //GoToDetails.Execute();
                SetProperty(ref _selectedCustomerOrder, null);
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

        private Dictionary<string, string> searchViewModel;
        private int _totalPages;
        private int _totalRecords;
        private int _currentPage = 1;

        LocalJobOrder localJobOrder { get; set; }

        public override void Prepare(Dictionary<string, string> parameter)
        {
            searchViewModel = parameter;

            if (searchViewModel.Count == 0 && !searchViewModel.ContainsKey("CustomerOrderStatus"))
            {
                searchViewModel.Add("CustomerOrderNumber", "");
                searchViewModel.Add("CustomerOrderStatus", "All");
            }

            searchViewModel.Add(Constants.Params.Page, _currentPage.ToString());
            searchViewModel.Add(Constants.Params.PageSize, Constants.Common.PageValue.ToString());
            LoadList.Execute();
        }

        public IMvxCommand LoadList => new MvxCommand(async () =>
        {

            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            _customerOrder = new ObservableCollection<CustomerOrderModel>();
            try
            {

                var result = await _webService.GetCustomerOrderList(searchViewModel);

                _totalPages = result.Pagination.Pages;
                _totalRecords = result.Pagination.Size;

                if (_totalRecords == 0)
                {
                    ShowError = true;
                }
                else
                {
                    ShowError = false;
                    foreach (CustomerOrderModel row in result.Data)
                    {
                        _customerOrder.Add(new CustomerOrderModel
                        {
                            ID = row.ID,
                            ServerID = row.ID,
                            CustomerOrderNumber = row.CustomerOrderNumber,
                            CustomerOrderStatus = row.CustomerOrderStatus,
                            Color = row.Color
                        });
                    }

                    if (_totalRecords > Constants.Common.PageValue)
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

        public IMvxCommand LoadMore => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            if (_currentPage < _totalPages)
            {
                _currentPage++;
                CanLoadMoreData = (_currentPage == _totalPages) ? false : true;
            }
            else
            {
                CanLoadMoreData = (_currentPage == _totalPages) ? false : true;
            }
            searchViewModel[Constants.Params.Page] = _currentPage.ToString();


            IsBusy = true;
            var error = false;


            if (NetworkCheck.HasInternet())
            {
                try
                {
                    var result = await _webService.GetJobOrderList(searchViewModel);

                    foreach (JobOrderModel row in result.Data)
                    {
                        //_jobOrder.Add(new JobOrderModel
                        //{
                        //    ID = row.ID,
                        //    ServerID = row.ID,
                        //    ApplicationType = row.ApplicationType,
                        //    JobOrderNumber = row.JobOrderNumber,
                        //    JobOrderSubject = row.JobOrderSubject,
                        //    StatusID = row.StatusID,
                        //    Color = row.Color
                        //});
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
            }
            else
            {
                try
                {
                    var result = MvxApp.Database.GetAllJobOrdersAsync(searchViewModel);

                    var applicationAllName = MvxApp.Database.GetAllApplicationTypesAsync();

                    _totalPages = result.Pages;
                    _totalRecords = result.Size;


                    if (_totalRecords == 0)
                    {
                        ShowError = true;
                    }
                    else
                    {
                        ShowError = false;

                        foreach (LocalJobOrder localJobOrder in result.Data)
                        {
                            int applicationID = localJobOrder.ApplicationTypeID;
                            var applicationName = MvxApp.Database.GetApplicationTypesAsync(applicationID);
                            var status = MvxApp.Database.GetStatusAsync(localJobOrder.StatusID);

                            //_jobOrder.Add(new JobOrderModel()
                            //{
                            //    ID = localJobOrder.ID,
                            //    ServerID = localJobOrder.ServerID,
                            //    JobOrderNumber = localJobOrder.JobOrderNumber,
                            //    StatusID = status.Status,
                            //    ApplicationType = applicationName.ApplicationName,
                            //    JobOrderSubject = localJobOrder.JobOrderSubject,
                            //    Color = Helpers.GetStatusColor(localJobOrder.StatusID)
                            //});
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
            }
        });

        public IMvxCommand RefreshList => new MvxCommand(async () =>
        {
            //if (IsBusy)
            //    return;

            //IsBusy = true;

            IsRefreshing = true;

            //_currentPage = 1;            

            var error = false;

            //var param = new Dictionary<string, string>();
            //searchViewModel = param;

            if (NetworkCheck.HasInternet())
            {
                try
                {
                    _currentPage = 1;

                    //var error = false;

                    var param = new Dictionary<string, string>();
                    searchViewModel = param;

                    searchViewModel.Add("CreatedBy", _settings.UserID);
                    searchViewModel.Add("Status", Constants.Params.PendingValue.ToString());
                    searchViewModel.Add("IsDeleted", Constants.Common.StringFalse);
                    searchViewModel.Add(Constants.Params.Page, _currentPage.ToString());
                    searchViewModel.Add(Constants.Params.PageSize, Constants.Common.PageValue.ToString());
                    //LoadList.Execute();
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
            }
            else
            {
                var noInternetMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                await UserDialogs.AlertAsync(noInternetMessage, Constants.Modal.Warning, Constants.Common.OK);
                IsRefreshing = false;
            }

            //IsBusy = false;
        });
        
        public IMvxCommand GoToFirstPageCommand => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            await _navigationService.Navigate<NewCOFirstViewModel, Dictionary<string, string>>(searchViewModel);
        });

        public IMvxCommand GoToMenuPageCommand => new MvxCommand(async () =>
        {
            if (IsBusy)

                return;
            await _navigationService.Navigate<MainViewModel>();
        });

        public IMvxCommand GoToSearch => new MvxCommand(async () =>
        {
            await _navigationService.Navigate<SearchCustomerOrderViewModel>();
        });

        public IMvxCommand GoToMainMenu => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            var param = new Dictionary<string, string> { };

            await _navigationService.Navigate<MainViewModel, Dictionary<string, string>>(param);
        });
    }
}