using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
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

    public class JobOrderListViewModel : BaseViewModel
    {
        public JobOrderListViewModel(IMvxNavigationService navigationService, IAppSettings settings,
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

        public ObservableCollection<JobOrderModel> _jobOrder { get; set; } = new ObservableCollection<JobOrderModel>();

        private int jobOrderID;
        private int serverJobOrderID;
        public string jobOrderNumber { get; set; }
        public string status { get; set; }
        public string applicationType { get; set; }

        private JobOrderModel _selectedJobOrder;
        public JobOrderModel SelectedJobOrder
        {
            get => _selectedJobOrder;
            set
            {
                SetProperty(ref _selectedJobOrder, value);
                jobOrderID = _selectedJobOrder.ID;
                serverJobOrderID = _selectedJobOrder.ServerID;
                GoToDetails.Execute();
                SetProperty(ref _selectedJobOrder, null);
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
            if (NetworkCheck.HasInternet())
            {

                if (searchViewModel != null && searchViewModel.ContainsKey(Constants.Params.JobOrderStatus))
                {

                    jobOrderNumber = searchViewModel[Constants.Params.JobOrderNumber];
                    applicationType = searchViewModel[Constants.Params.ApplicationType];
                    status = searchViewModel[Constants.Params.JobOrderStatus];
                    try
                    {
                        if (searchViewModel.ContainsKey(Constants.Params.ApplicationType))
                        {
                            searchViewModel[Constants.Params.ApplicationType] = applicationType;
                        }

                        else
                        {
                            searchViewModel.Add(Constants.Params.ApplicationType, applicationType);
                        }
                        searchViewModel.Add("CreatedBy", _settings.UserID);
                        searchViewModel.Add("IsDeleted", Constants.Common.StringFalse);
                        searchViewModel.Add(Constants.Params.Page, _currentPage.ToString());
                        searchViewModel.Add(Constants.Params.PageSize, Constants.Common.PageValue.ToString());
                    }
                    catch (Exception ex)
                    {
                        var test = ex;
                    }

                    LoadList.Execute();
                }
                else
                {
                    searchViewModel.Add("CreatedBy", _settings.UserID);
                    searchViewModel.Add("Status", Constants.Params.PendingValue.ToString());
                    searchViewModel.Add("IsDeleted", Constants.Common.StringFalse);
                    searchViewModel.Add(Constants.Params.Page, _currentPage.ToString());
                    searchViewModel.Add(Constants.Params.PageSize, Constants.Common.PageValue.ToString());

                    LoadList.Execute();
                }
            }
            else
            {
                

                if (searchViewModel != null && searchViewModel.ContainsKey(Constants.Params.JobOrderStatus))
                {

                    jobOrderNumber = searchViewModel[Constants.Params.JobOrderNumber];
                    applicationType = searchViewModel[Constants.Params.ApplicationType];
                    status = searchViewModel[Constants.Params.JobOrderStatus];
                    try
                    {
                        if (searchViewModel.ContainsKey(Constants.Params.ApplicationType))
                        {
                            searchViewModel[Constants.Params.ApplicationType] = applicationType;
                        }

                        else
                        {
                            searchViewModel.Add(Constants.Params.ApplicationType, applicationType);
                        }                        
                        
                        searchViewModel.Add("IsDeleted", Constants.Common.StringFalse);
                        searchViewModel.Add(Constants.Params.Page, _currentPage.ToString());
                        searchViewModel.Add(Constants.Params.PageSize, Constants.Common.PageValue.ToString());
                    }
                    catch (Exception ex)
                    {
                        var test = ex;
                    }

                    LoadListOffline.Execute();
                }
                else
                {

                    searchViewModel.Add("JobOrderNumber", Constants.Params.Empty);
                    searchViewModel.Add("ApplicationType", Constants.Params.Empty);
                    searchViewModel.Add("Status", Constants.Params.PendingValue.ToString());
                    searchViewModel.Add(Constants.Params.Page, _currentPage.ToString());
                    searchViewModel.Add(Constants.Params.PageSize, Constants.Common.PageValue.ToString());

                    LoadListOffline.Execute();
                }
            }
        }

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
                        _jobOrder.Add(new JobOrderModel
                        {
                            ID = row.ID,
                            ServerID = row.ID,
                            ApplicationType = row.ApplicationType,
                            JobOrderNumber = row.JobOrderNumber,
                            JobOrderSubject = row.JobOrderSubject,
                            StatusID = row.StatusID,
                            Color = row.Color
                        });
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

                            _jobOrder.Add(new JobOrderModel()
                            {
                                ID = localJobOrder.ID,
                                ServerID = localJobOrder.ServerID,
                                JobOrderNumber = localJobOrder.JobOrderNumber,
                                StatusID = status.Status,
                                ApplicationType = applicationName.ApplicationName,
                                JobOrderSubject = localJobOrder.JobOrderSubject,
                                Color = Helpers.GetStatusColor(localJobOrder.StatusID)
                            });
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
                    LoadList.Execute();
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

        public IMvxCommand LoadListOffline => new MvxCommand(async () =>
        {
            
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {

                var result = MvxApp.Database.GetAllJobOrdersAsync(searchViewModel);
                
                var applicationAllName = MvxApp.Database.GetAllApplicationTypesAsync();

                _totalPages = result.Pages;
                _totalRecords = result.Size;

                //_jobOrder = new ObservableCollection<JobOrderModel>();
                if (_totalRecords == 0)
                {
                    ShowError = true;
                    HasRecords = true;
                }
                else
                {
                    ShowError = false;
                    
                    foreach (LocalJobOrder localJobOrder in result.Data)
                    {
                        int applicationID = localJobOrder.ApplicationTypeID;
                        var applicationName = MvxApp.Database.GetApplicationTypesAsync(applicationID);
                        var status = MvxApp.Database.GetStatusAsync(localJobOrder.StatusID);

                        _jobOrder.Add(new JobOrderModel()
                        {
                            ID = localJobOrder.ID,
                            ServerID = localJobOrder.ServerID,
                            JobOrderNumber = localJobOrder.JobOrderNumber,
                            StatusID = status.Status,
                            ApplicationType = applicationName.ApplicationName,
                            JobOrderSubject = localJobOrder.JobOrderSubject,
                            Color = Helpers.GetStatusColor(localJobOrder.StatusID)
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
                    HasRecords = true;
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

        public IMvxCommand LoadList => new MvxCommand(async () =>
        {

            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

              _jobOrder = new ObservableCollection<JobOrderModel>();
                try
                {

                    var result = await _webService.GetJobOrderList(searchViewModel);

                    _totalPages = result.Pagination.Pages;
                    _totalRecords = result.Pagination.Size;

                    if (_totalRecords == 0)
                    {
                        ShowError = true;
                    }
                    else
                    {
                        ShowError = false;
                        foreach (JobOrderModel row in result.Data)
                        {
                            _jobOrder.Add(new JobOrderModel
                            {
                                ID = row.ID,
                                ServerID = row.ID,
                                ApplicationType = row.ApplicationType,
                                JobOrderNumber = row.JobOrderNumber,
                                JobOrderSubject = row.JobOrderSubject,
                                StatusID = row.StatusID,
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

        public IMvxCommand GoToSearch => new MvxCommand(async () => 
         {             
             await _navigationService.Navigate<SearchViewModel>(); 
         });

        public IMvxCommand DeleteCommand => new MvxCommand<Dictionary<string, string>>(async (param) =>
        {

            if (IsBusy)
                return;


            searchViewModel = param;

            int serverID = int.Parse(searchViewModel[Constants.Keys.ServerJobOrderID]);
            int localID = int.Parse(searchViewModel[Constants.Keys.LocalJobOrderID]);
            string status = searchViewModel[Constants.Params.JobOrderStatus];


            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {

                    if (status.Equals(Constants.Params.Pending))
                    {
                        var localizedMessage = LocalizeService.Translate(Constants.Messages.DeleteConfirmation);
                        var dialogResult = await _userDialogs.ConfirmAsync(localizedMessage, Constants.Modal.Confirmation, Constants.Messages.Yes, Constants.Messages.No);

                        if (dialogResult == true)
                        {
                            var result = await _webService.DeleteJobOrder(serverID);
                            if (result == true)
                            {
                                var checkStatus = MvxApp.Database.GetJobOrderByServerIDAsync(serverID);

                                if (checkStatus.StatusID == 1)
                                {
                                    var del = MvxApp.Database.DeleteJOAsync(serverID);
                                }

                                var successMessage = LocalizeService.Translate(Constants.Messages.SuccessDeletion);
                                await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);

                                var index = 0;
                                foreach (var jobOrder in _jobOrder)
                                {
                                    if (jobOrder.ServerID == serverID)
                                    {
                                        _jobOrder.RemoveAt(index);
                                        break;
                                    }
                                    else
                                    {
                                        index++;
                                    }
                                }


                                RefreshList.Execute();

                                //if (_totalRecords <= 7)
                                //{
                                //    CanLoadMoreData = false;
                                //    ShowError = false;
                                //    if (_jobOrder.Count() == 0)
                                //    {
                                //        ShowError = true;
                                //    }
                                //}
                            }

                            else
                            {
                                var unsuccessMessage = LocalizeService.Translate(Constants.Messages.UnSuccessfulDeletion);
                                await UserDialogs.AlertAsync(unsuccessMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                            }
                        }
                    }
                    else
                    {
                        var unsuccessMessage = LocalizeService.Translate(Constants.Messages.NotAllowedToDelete);
                        await UserDialogs.AlertAsync(unsuccessMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                    }

                }
                else
                {

                    var localizedMessage = LocalizeService.Translate(Constants.Messages.DeleteConfirmation);
                    var dialogResult = await _userDialogs.ConfirmAsync(localizedMessage, Constants.Modal.Confirmation, Constants.Messages.Yes, Constants.Messages.No);

                    if (dialogResult == true)
                    {
                        var jobOrders = MvxApp.Database.GetJobOrdersAsync();

                        var joID = serverID > 0 ? serverID : localID;

                        var checkStatus = new LocalJobOrder();

                        if (serverID > 0)
                        {
                            checkStatus = jobOrders.Where(x => x.ServerID == serverID).FirstOrDefault();
                        }
                        else
                        {
                            checkStatus = MvxApp.Database.GetJobOrderAsync(joID);
                        }

                        if (checkStatus.JobOrderNumber != null && checkStatus.StatusID == 1)
                        {
                            //If JobOrder is already saved in the server and user deletes job order during offline
                            var del = MvxApp.Database.DeleteJOAsync(joID);
                            var successMessage = LocalizeService.Translate(Constants.Messages.DeleteToLocalSuccess);
                            await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);

                            var index = 0;
                            foreach (var jobOrder in _jobOrder)
                            {
                                if (jobOrder.ServerID == joID)
                                {
                                    _jobOrder.RemoveAt(index);
                                    break;
                                }
                                else
                                {
                                    index++;
                                }
                            }

                            //if (_jobOrder.Count() > 7)
                            //{
                            //    CanLoadMoreData = false;
                            //    ShowError = false;
                            //    if (_jobOrder.Count() == 0)
                            //    {
                            //        ShowError = true;
                            //    }
                            //}

                            var parameter = new Dictionary<string, string>();
                            searchViewModel = param;

                            searchViewModel.Add("JobOrderNumber", Constants.Params.Empty);
                            searchViewModel.Add(Constants.Params.PageSize, Constants.Common.PageValue.ToString());
                            searchViewModel.Add("ApplicationType", Constants.Params.Empty);
                            if (searchViewModel.ContainsKey(Constants.Params.JobOrderStatus))
                            {
                                //searchViewModel.Add("Status", Constants.Params.PendingValue.ToString());
                                searchViewModel[Constants.Params.JobOrderStatus] = Constants.Params.PendingValue.ToString();
                            }
                            searchViewModel.Add(Constants.Params.Page, _currentPage.ToString());

                            //LoadListOffline.Execute();
                        }

                        else if (checkStatus.JobOrderNumber == null && checkStatus.StatusID == 1)
                        {
                            //If job order is created offline and not yet sync to the server

                            var del = MvxApp.Database.DeleteIndividualJOAsync(joID);

                            var successMessage = LocalizeService.Translate(Constants.Messages.SuccessDeletion);
                            await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);

                            var index = 0;
                            foreach (var jobOrder in _jobOrder)
                            {
                                if (jobOrder.ID == joID)
                                {
                                    _jobOrder.RemoveAt(index);
                                    break;
                                }
                                else
                                {
                                    index++;
                                }
                            }

                            if (_jobOrder.Count() <= 7)
                            {
                                CanLoadMoreData = false;
                                ShowError = false;
                                if (_jobOrder.Count() == 0)
                                {
                                    ShowError = true;
                                }
                            }
                            // LoadListOffline.Execute();
                        }


                        else if (checkStatus.StatusID != 1)
                        {
                            var unsuccessMessage = LocalizeService.Translate(Constants.Messages.NotAllowedToDelete);
                            await UserDialogs.AlertAsync(unsuccessMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                        }
                    }

                }
            }
            catch(Exception)
            {
                error = true;
            }
            finally
            {
                IsBusy = false;
            }

            if (error)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }

        });

        public IMvxCommand RevertCommand => new MvxCommand<Dictionary<string, string>>(async (param) =>
        {
            if (IsBusy)
                return;

            searchViewModel = param;

            int serverID = int.Parse(searchViewModel[Constants.Keys.ServerJobOrderID]);
            int localID = int.Parse(searchViewModel[Constants.Keys.LocalJobOrderID]);
            string status = searchViewModel[Constants.Params.JobOrderStatus];

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {                
                    if (status.Equals(Constants.Params.Sent))
                    {
                        var localizedMessage = LocalizeService.Translate(Constants.Messages.RevertConfirmation);
                        var dialogResult = await _userDialogs.ConfirmAsync(localizedMessage, Constants.Modal.Confirmation, Constants.Messages.Yes, Constants.Messages.No);

                        if (dialogResult == true)
                        {

                            var result = await _webService.RevertJobOrder(serverID);
                            if (result == true)
                            {
                                var successMessage = LocalizeService.Translate(Constants.Messages.SuccessRevert);
                                await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);

                                var index = 0;
                                foreach (var jobOrder in _jobOrder)
                                {
                                    if (jobOrder.ID == serverID)
                                    {
                                        _jobOrder.RemoveAt(index);
                                        break;
                                    }
                                    else
                                    {
                                        index++;
                                    }
                                }
                            }
                            else
                            {
                                var unsuccessMessage = LocalizeService.Translate(Constants.Messages.UnSuccessfulRevert);
                                await UserDialogs.AlertAsync(unsuccessMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                            }

                        }
                    }
                    else
                    {
                        var unsuccessMessage = LocalizeService.Translate(Constants.Messages.NotAllowedToRevert);
                        await UserDialogs.AlertAsync(unsuccessMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                    }
                }
                else
                {
                    var noInternetMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                    await UserDialogs.AlertAsync(noInternetMessage, Constants.Modal.Warning, Constants.Common.OK);
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
        });

        public IMvxCommand GoToFirstPageCommand => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            await _navigationService.Navigate<NewJOFirstViewModel, Dictionary<string, string>>(searchViewModel);
        });

        public IMvxCommand GoToMenuPageCommand => new MvxCommand(async () =>
        {
            if (IsBusy)

                return;
            await _navigationService.Navigate<MainViewModel>();
        });

        public IMvxCommand GoToDetails => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            await Task.Delay(1000);

            var error = false;

            try
            {

                var parameter = new Dictionary<string, string>
                {
                    {
                        Constants.Keys.ServerJobOrderID, serverJobOrderID.ToString()
                    },
                    {
                        Constants.Keys.LocalJobOrderID, jobOrderID.ToString() 
                    }
                };
                var presentationBundle = new MvxBundle(new Dictionary<string, string> { });
                await _navigationService.Navigate<JobOrderViewModel, Dictionary<string, string>>(parameter, presentationBundle);
            }
            catch(Exception)
            {
                error = true;
            }

            if (error)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                await _navigationService.Close(this);
            }
        });

        public IMvxCommand GoToMainMenu => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            var param = new Dictionary<string, string> {};

            await _navigationService.Navigate<MainViewModel, Dictionary<string, string>>(param);
        });
    }
}