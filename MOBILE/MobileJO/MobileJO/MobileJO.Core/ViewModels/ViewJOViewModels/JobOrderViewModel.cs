using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.EmailJO;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Core.ViewModels
{
    public class JobOrderViewModel : BaseViewModel
    {
        public JobOrder _jobOrderModel { get; set; } = new JobOrder();


        public JobOrder JobOrderModel { get; set; }

        
        //Set button values either enable or disable base on logic
        private bool _enableEdit;
        public bool EnableEdit
        {
            get => _enableEdit;
            set => SetProperty(ref _enableEdit, value);
        }

        private bool _enableRevert;
        public bool EnableRevert
        {
            get => _enableRevert;
            set => SetProperty(ref _enableRevert, value);
        }

        private bool _enableEmailJO;
        public bool EnableEmailJO
        {
            get => _enableEmailJO;
            set => SetProperty(ref _enableEmailJO, value);
        }

        private bool _enableDelete;
        public bool EnableDelete
        {
            get => _enableDelete;
            set => SetProperty(ref _enableDelete, value);
        }
        //Setting button values is until here

        //Setting date and time values
        private string _dateStart;
        public string DateStart
        {
            get => _dateStart;
            set => SetProperty(ref _dateStart, value);
        }

        private string _dateEnd;
        public string DateEnd
        {
            get => _dateEnd;
            set => SetProperty(ref _dateEnd, value);
        }

        private string _timeStart;
        public string TimeStart
        {
            get => _timeStart;
            set => SetProperty(ref _timeStart, value);
        }

        private string _timeEnd;
        public string TimeEnd
        {
            get => _timeEnd;
            set => SetProperty(ref _timeEnd, value);
        }
        //Setting date and time values ends here

        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private Dictionary<string, string> _parameter;
        
        public int serverJobOrderID { get; set; }
        public int localJobOrderID { get; set; }
        public string jobOrderID { get; set; }
        public int status { get; set; }
        public int createdBy { get; set; }
        public int isDeleted { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }

        public int applicationType { get; set; }

        public bool editButtonValue;
        public bool revertButtonValue;
        public bool emailButtonValue;
        public bool deleteButtonValue;

        

        public JobOrderViewModel(IMvxNavigationService navigationService,
                                    IAppSettings settings,
                                    IUserDialogs userDialogs,
                                    ILocalizeService localizeService,
                                    IWebService webService,
                                    IMvxJsonConverter serializer)
        : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _userDialogs = userDialogs;
            _localizeService = localizeService;
            _webService = webService;
            _serializer = serializer;


        }

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;

            if (_parameter != null)
            {
                if(_parameter.ContainsKey(Constants.Keys.ServerJobOrderID))
                {
                    serverJobOrderID = int.Parse(_parameter[Constants.Keys.ServerJobOrderID]);
                }

                if (_parameter.ContainsKey(Constants.Keys.LocalJobOrderID))
                {
                    localJobOrderID = int.Parse(_parameter[Constants.Keys.LocalJobOrderID]);
                }

                LoadDetails.Execute();
            }
        }

        private IMvxCommand LoadDetails => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet() && serverJobOrderID > 0)
                {
                    _jobOrderModel = await _webService.JobOrderDetail(serverJobOrderID);

                    if (_jobOrderModel != null)
                    {
                        JobOrderModel = new JobOrder()
                        {
                            ID = _jobOrderModel.ID,
                            JobOrderNumber = _jobOrderModel.JobOrderNumber,
                            JobOrderSubject = _jobOrderModel.JobOrderSubject,
                            Branch = _jobOrderModel.Branch,
                            AccountName = _jobOrderModel.AccountName,
                            ApplicationName = _jobOrderModel.ApplicationName,
                            DateTimeStart = DateTime.Parse(_jobOrderModel.DateTimeStart.ToShortDateString()),
                            DateTimeEnd = Convert.ToDateTime(_jobOrderModel.DateTimeEnd).Date,
                            ActivityDetails = _jobOrderModel.ActivityDetails,
                            RootCauseAnalysis = _jobOrderModel.RootCauseAnalysis,
                            NextStep = _jobOrderModel.NextStep,
                            PreventiveAction = _jobOrderModel.PreventiveAction,
                            Remarks = _jobOrderModel.Remarks,
                            Attendees = _jobOrderModel.Attendees,
                            Is_Billed = _jobOrderModel.Is_Billed,
                            Is_Collaterals = _jobOrderModel.Is_Collaterals,
                            ClientSignature = _jobOrderModel.ClientSignature,
                            Is_Fixed = _jobOrderModel.Is_Fixed,
                            Is_Satisfied = _jobOrderModel.Is_Satisfied,
                            StatusID = _jobOrderModel.StatusID,
                            CreatedDate = _jobOrderModel.CreatedDate,
                            CreatedBy = _jobOrderModel.CreatedBy,
                            UpdatedDate = _jobOrderModel.UpdatedDate,
                            UpdatedBy = _jobOrderModel.UpdatedBy,
                            Is_Deleted = _jobOrderModel.Is_Deleted,
                            ClientRating = _jobOrderModel.ClientRating
                        };

                        DateStart = _jobOrderModel.DateTimeStart.ToShortDateString();
                        DateEnd = _jobOrderModel.DateTimeEnd.ToShortDateString();
                        TimeStart = _jobOrderModel.DateTimeStart.ToShortTimeString();
                        TimeEnd = _jobOrderModel.DateTimeEnd.ToShortTimeString();

                        SetButtonStatus(_jobOrderModel.StatusID);
                    }                    
                }
                else
                {
                    var localJO = new LocalJobOrder();

                    if (serverJobOrderID > 0)
                    {
                        localJO = MvxApp.Database.GetJobOrderByServerIDAsync(serverJobOrderID);
                    }
                    else
                    {
                        localJO = MvxApp.Database.GetJobOrderAsync(localJobOrderID);
                    }

                    if (localJO != null)
                    {
                       
                        var accounts = MvxApp.Database.GetAllAccountsAsync();
                        var appTypes = MvxApp.Database.GetAllApplicationTypesAsync();
                        var account = accounts.Where(x => x.ID == localJO.AccountID).FirstOrDefault();
                        var appType = appTypes.Where(x => x.ID == localJO.ApplicationTypeID).FirstOrDefault();

                        JobOrderModel = new JobOrder()
                        {
                            ID = localJO.ServerID,
                            JobOrderNumber = localJO.JobOrderNumber,
                            JobOrderSubject = localJO.JobOrderSubject,
                            Branch = localJO.Branch,
                            AccountName = account.Name,
                            ApplicationName = appType.ApplicationName,
                            DateTimeStart = DateTime.Parse(localJO.DateTimeStart.ToShortDateString()),
                            DateTimeEnd = Convert.ToDateTime(localJO.DateTimeEnd).Date,
                            ActivityDetails = localJO.ActivityDetails,
                            RootCauseAnalysis = localJO.RootCauseAnalysis,
                            NextStep = localJO.NextStep,
                            PreventiveAction = localJO.PreventiveAction,
                            Remarks = localJO.Remarks,
                            Attendees = localJO.Attendees,
                            Is_Billed = localJO.IsBilled,
                            Is_Collaterals = localJO.IsCollaterals,
                            ClientSignature = localJO.ClientSignature,
                            Is_Fixed = localJO.IsFixed,
                            Is_Satisfied = localJO.IsSatisfied,
                            StatusID = localJO.StatusID,
                            CreatedDate = localJO.CreatedDate,
                            CreatedBy = localJO.CreatedBy,
                            UpdatedDate = localJO.UpdatedDate,
                            UpdatedBy = localJO.UpdatedBy,
                            Is_Deleted = localJO.IsDeleted,
                            ClientRating = localJO.ClientRating, 
                        };

                        DateStart = localJO.DateTimeStart.ToShortDateString();
                        DateEnd = localJO.DateTimeEnd.ToShortDateString();
                        TimeStart = localJO.DateTimeStart.ToShortTimeString();
                        TimeEnd = localJO.DateTimeEnd.ToShortTimeString();

                        SetButtonStatus(localJO.StatusID);
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
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                await _navigationService.Close(this);
            }
           
        });

        public IMvxCommand GoToListPage => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var param = new Dictionary<string, string>(){};
            await _navigationService.Navigate<JobOrderListViewModel, Dictionary<string, string>>(param);

            IsBusy = false;
        });

        public IMvxCommand GoToSecondPage => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;

            int id = NetworkCheck.HasInternet() ? serverJobOrderID : localJobOrderID;
            
            var param = new Dictionary<string, string>
                {
                    {
                        Constants.Common.ID, serverJobOrderID.ToString()
                    },
                    {
                        Constants.Common.NextStep, JobOrderModel.NextStep
                    },
                    {
                        Constants.Common.PreventiveAction, JobOrderModel.PreventiveAction
                    },
                    {
                        Constants.Common.Remarks, JobOrderModel.Remarks
                    },
                    {
                        Constants.Common.Attendees, JobOrderModel.Attendees
                    },
                    {
                        Constants.Common.Billed, JobOrderModel.Is_Billed.ToString()
                    },
                    {
                        Constants.Common.Collaterals, JobOrderModel.Is_Collaterals.ToString()
                    },
                    {
                        Constants.Common.IsFixed, JobOrderModel.Is_Fixed.ToString()
                    },
                    {
                        Constants.Common.IsSatisfied, JobOrderModel.Is_Satisfied.ToString()
                    },
                    {
                        Constants.Common.ClientRating, JobOrderModel.ClientRating.ToString()
                    },
                     {
                        Constants.Params.JobOrderStatus, JobOrderModel.StatusID.ToString()
                    },
                     {
                        Constants.Params.Signature, JobOrderModel.ClientSignature
                    },
                     {
                       Constants.Keys.ServerJobOrderID, serverJobOrderID.ToString()
                    },
                    {
                       Constants.Keys.LocalJobOrderID, localJobOrderID.ToString()
                    }
                };
            await _navigationService.Navigate<SecondViewModel, Dictionary<string, string>>(param);

            IsBusy = false;
        });

        public IMvxAsyncCommand DeleteJobOrder => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            if (NetworkCheck.HasInternet())
            {
                try
                {
                    var localizedMessage = LocalizeService.Translate(Constants.Messages.DeleteConfirmation);
                    var dialogResult = await _userDialogs.ConfirmAsync(localizedMessage, Constants.Modal.Confirmation, Constants.Messages.Yes, Constants.Messages.No);

                    if (dialogResult == true)
                    {
                        var result = await _webService.DeleteJobOrder(serverJobOrderID);

                        if (result == true)
                        {
                            var del = MvxApp.Database.DeleteJOAsync(serverJobOrderID);

                            var successMessage = LocalizeService.Translate(Constants.Messages.SuccessDeletion);
                            await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);

                            GoToJobOrderList.Execute();

                        }

                        else
                        {
                            var unsuccessMessage = LocalizeService.Translate(Constants.Messages.UnSuccessfulDeletion);
                            await UserDialogs.AlertAsync(unsuccessMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var test = ex;
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                try
                {
                    var localizedMessage = LocalizeService.Translate(Constants.Messages.DeleteConfirmation);
                    var dialogResult = await _userDialogs.ConfirmAsync(localizedMessage, Constants.Modal.Confirmation, Constants.Messages.Yes, Constants.Messages.No);

                    int id = serverJobOrderID > 0 ? serverJobOrderID : localJobOrderID;
                    var checkStatus = new LocalJobOrder();
                    var jobOrders = MvxApp.Database.GetJobOrdersAsync();

                    if (dialogResult == true)
                    {

                        if (serverJobOrderID > 0)
                        {
                            checkStatus = jobOrders.Where(x => x.ServerID == serverJobOrderID).FirstOrDefault();
                        }
                        else
                        {
                            checkStatus = MvxApp.Database.GetJobOrderAsync(id);
                        }

                        if (checkStatus.JobOrderNumber != null)
                        {
                            var del = MvxApp.Database.DeleteJOAsync(id);
                            var successMessage = LocalizeService.Translate(Constants.Messages.DeleteToLocalSuccess);
                            await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                        }

                        if (checkStatus.JobOrderNumber == null)
                        {
                            //If job order is created offline and not yet sync to the server
                            var del = MvxApp.Database.DeleteIndividualJOAsync(id);

                            var successMessage = LocalizeService.Translate(Constants.Messages.SuccessDeletion);
                            await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                        }

                        GoToJobOrderList.Execute();
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

        public IMvxAsyncCommand GoToJobOrderList => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string>();
            await _navigationService.Navigate<JobOrderListViewModel, Dictionary<string, string>>(param);
          
        });

        public IMvxAsyncCommand RevertJobOrder => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            if (NetworkCheck.HasInternet())
            {
                try
                {
                    var localizedMessage = LocalizeService.Translate(Constants.Messages.RevertConfirmation);
                    var dialogResult = await _userDialogs.ConfirmAsync(localizedMessage, Constants.Modal.Confirmation, Constants.Messages.Yes, Constants.Messages.No);

                    if (dialogResult == true)
                    {
                        RevertModel count = await _webService.GetRevertCount(serverJobOrderID);
                        if (count == null)
                        {
                            var result = await _webService.RevertJobOrder(serverJobOrderID);
                            if (result == true)
                            {
                                var successMessage = LocalizeService.Translate(Constants.Messages.SuccessRevert);
                                await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);

                                GoToJobOrderList.Execute();
                            }
                            else
                            {
                                var unsuccessMessage = LocalizeService.Translate(Constants.Messages.UnSuccessfulRevert);
                                await UserDialogs.AlertAsync(unsuccessMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                            }
                        }

                        else if (count.IsUsed == true)
                        {
                            var allowedRequestCount = LocalizeService.Translate(Constants.Messages.AllowedRequestCount);
                            await UserDialogs.AlertAsync(allowedRequestCount, Constants.Modal.InfoMessage, Constants.Common.OK);
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
            }
            else
            {
                var noInternetMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                await UserDialogs.AlertAsync(noInternetMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
        });

        public IMvxCommand TaggedCases => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;

                try
                {
                    var param = new Dictionary<string, string>();
                
                    if (NetworkCheck.HasInternet())
                    {
                        param.Add(Constants.Common.ID, serverJobOrderID.ToString());                            
                    }
                    else
                    {
                        param.Add(Constants.Keys.ServerJobOrderID, serverJobOrderID.ToString());
                        param.Add(Constants.Keys.LocalJobOrderID, localJobOrderID.ToString());  
                    }

                    await _navigationService.Navigate<TaggedCasesViewModel, Dictionary<string, string>>(param);              
                }
                catch (Exception ex)
                {
                    var test = ex;
                }
                finally
                {
                    IsBusy = false;
                }


        });

        public IMvxCommand EditJobOrderCommand => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                LocalJobOrder localJobOrder = new LocalJobOrder();

                if (NetworkCheck.HasInternet() && serverJobOrderID > 0)
                {
                    var accounts = await _webService.AccountList();
                    var appTypes = await _webService.ApplicationTypeList();

                    var result = await _webService.JobOrderDetail(serverJobOrderID);

                    localJobOrder.ID = result.ID;
                    localJobOrder.ServerID = result.ID;
                    localJobOrder.JobOrderNumber = result.JobOrderNumber;
                    localJobOrder.JobOrderSubject = result.JobOrderSubject;
                    localJobOrder.Branch = result.Branch;
                    localJobOrder.AccountID = accounts.Where(x => x.Name == result.AccountName).FirstOrDefault().ID;
                    localJobOrder.ApplicationTypeID = appTypes.Where(x => x.ApplicationName == result.ApplicationName).FirstOrDefault().ID;
                    localJobOrder.DateTimeStart = result.DateTimeStart;
                    localJobOrder.DateTimeEnd = result.DateTimeEnd;
                    localJobOrder.ActivityDetails = result.ActivityDetails;
                    localJobOrder.RootCauseAnalysis = result.RootCauseAnalysis;
                    localJobOrder.PreventiveAction = result.PreventiveAction;
                    localJobOrder.Remarks = result.Remarks;
                    localJobOrder.NextStep = result.NextStep;
                    localJobOrder.Attendees = result.Attendees;
                    localJobOrder.IsBilled = result.Is_Billed;
                    localJobOrder.IsCollaterals = result.Is_Collaterals;
                    localJobOrder.ClientSignature = result.ClientSignature;
                    localJobOrder.IsFixed = result.Is_Fixed;
                    localJobOrder.IsSatisfied = result.Is_Satisfied;
                    localJobOrder.ClientRating = result.ClientRating;
                    localJobOrder.StatusID = result.StatusID;
                    localJobOrder.IsDeleted = result.Is_Deleted;
                    localJobOrder.CreatedDate = result.CreatedDate;
                    localJobOrder.CreatedBy = result.CreatedBy;
                    localJobOrder.UpdatedDate = result.UpdatedDate;
                    localJobOrder.UpdatedBy = result.UpdatedBy;
                    localJobOrder.LastSyncDate = result.LastSyncDate;
                }
                else
                {
                    if (serverJobOrderID > 0)
                    {
                        localJobOrder = MvxApp.Database.GetJobOrderByServerIDAsync(serverJobOrderID);
                    }
                    else
                    {
                        localJobOrder = MvxApp.Database.GetJobOrderAsync(localJobOrderID);
                    }
                }

                var serializedJobOrder = string.Empty;

                if (localJobOrder != null)
                {
                    serializedJobOrder = _serializer.SerializeObject(localJobOrder);
                }                

                var param = new Dictionary<string, string> { };

                param.Add(Constants.Params.SelectedJobOrder, serializedJobOrder);

                await _navigationService.Navigate<EditJOFirstViewModel, Dictionary<string, string>>(param);
            }
            catch (Exception ex)
            {
                error = true;
            }
            finally
            {
                IsBusy = false;
            }

        });

        public IMvxCommand EmailJobOrderCommand => new MvxCommand(async () =>
        {
            var param = new Dictionary<string, string> { { Constants.Keys.JobOrderIDs , JobOrderModel.JobOrderNumber } };
            await _navigationService.Navigate<EmailJOViewModel, Dictionary<string, string>>(param);
        });

        private void SetButtonStatus(int statusID)
        {
            switch (statusID)
            {
                case Constants.Params.PendingValue:
                    EnableEdit = true;
                    EnableRevert = false;
                    EnableEmailJO = false;
                    EnableDelete = true;
                    break;

                case Constants.Params.SignedValue:
                    EnableEdit = true;
                    EnableRevert = false;
                    EnableEmailJO = true;
                    EnableDelete = false;
                    break;

                case Constants.Params.SentValue:
                    EnableEdit = false;
                    EnableRevert = true;
                    EnableEmailJO = false;
                    EnableDelete = false;
                    break;

                case Constants.Params.RequestRevertValue:
                    EnableEdit = false;
                    EnableRevert = false;
                    EnableEmailJO = false;
                    EnableDelete = false;
                    break;

            }
        }

        public IMvxCommand GoToList => new MvxCommand(async () =>
        {
            var param = new Dictionary<string, string>();
            await _navigationService.Navigate<JobOrderListViewModel, Dictionary<string, string>>(param);
        });

        private IMvxCommand closeCommand;
        public IMvxCommand CloseCommand => closeCommand ?? (closeCommand = new MvxAsyncCommand(() => _navigationService.Close(this)));

    }
}
