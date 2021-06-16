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
    public class SecondViewModel : BaseViewModel
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

        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private Dictionary<string, string> _parameter;
        public int jobOrderID { get; set; }
        public string nextStep;
        public string preventiveAction;
        public string remarks;
        public string attendees;
        public string billed;
        public string collaterals;
        public string isFixed;
        public string isSatisfied;
        public string clientRating;
        public int status;
        public string signature;
        public int serverJobOrderID;
        public int localJobOrderID;
        
       
        public SecondViewModel(IMvxNavigationService navigationService,
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
                if (_parameter.ContainsKey(Constants.Common.ID))
                {
                    jobOrderID = int.Parse(_parameter[Constants.Common.ID]);
                }
                if (_parameter.ContainsKey(Constants.Common.NextStep))
                {
                    nextStep = _parameter[Constants.Common.NextStep];
                }
                if (_parameter.ContainsKey(Constants.Common.Remarks))
                {
                    remarks = _parameter[Constants.Common.Remarks];
                }
                if (_parameter.ContainsKey(Constants.Common.PreventiveAction))
                {
                    preventiveAction = _parameter[Constants.Common.PreventiveAction];
                }
                if (_parameter.ContainsKey(Constants.Common.Attendees))
                {
                    attendees = _parameter[Constants.Common.Attendees];
                }
                if (_parameter.ContainsKey(Constants.Common.Billed))
                {
                    billed = _parameter[Constants.Common.Billed];
                }
                if (_parameter.ContainsKey(Constants.Common.Collaterals))
                {
                    collaterals = _parameter[Constants.Common.Collaterals];
                }
                if (_parameter.ContainsKey(Constants.Common.IsFixed))
                {
                    isFixed = _parameter[Constants.Common.IsFixed];
                }
                if (_parameter.ContainsKey(Constants.Common.IsSatisfied))
                {
                    isSatisfied = _parameter[Constants.Common.IsSatisfied];
                }
                if (_parameter.ContainsKey(Constants.Common.ClientRating))
                {
                    clientRating = _parameter[Constants.Common.ClientRating];
                }
                if (_parameter.ContainsKey(Constants.Params.JobOrderStatus))
                {
                    status = int.Parse(_parameter[Constants.Params.JobOrderStatus]);
                }
                if (_parameter.ContainsKey(Constants.Params.Signature))
                {
                    signature = _parameter[Constants.Params.Signature];
                }
                if (_parameter.ContainsKey(Constants.Keys.ServerJobOrderID))
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

        private IMvxAsyncCommand LoadDetails => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {

                    JobOrderModel = new JobOrder()
                    {
                        ID = jobOrderID,
                        NextStep = nextStep,
                        PreventiveAction = preventiveAction,
                        Remarks = remarks,
                        Attendees = attendees
                    };

                    SetButtonStatus(status);
                }
                else
                {

                    JobOrderModel = new JobOrder()
                    {
                        ID = jobOrderID,
                        PreventiveAction = preventiveAction,
                        Remarks = remarks,
                        NextStep = nextStep,
                        Attendees = attendees,
                    };

                    SetButtonStatus(status);

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

        public IMvxAsyncCommand GoToFirstPage => new MvxAsyncCommand(async () =>
        {
            var parameter = new Dictionary<string, string>
                {
                    {
                        Constants.Keys.ServerJobOrderID, serverJobOrderID.ToString()
                    },
                    {
                        Constants.Keys.LocalJobOrderID, localJobOrderID.ToString()
                    },
                };
            await _navigationService.Navigate<JobOrderViewModel, Dictionary<string, string>>(parameter);
        });

        public IMvxAsyncCommand GoToThirdPage => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string>
                {
                    {
                        Constants.Common.ID, serverJobOrderID.ToString()
        },
                    {
                        Constants.Common.NextStep, nextStep
                    },
                    {
                        Constants.Common.Remarks, remarks
                    },
                     {
                        Constants.Common.PreventiveAction, preventiveAction
                    },
                    {
                        Constants.Common.Attendees, attendees
                    },
                    {
                        Constants.Common.Billed, billed
                    },
                    {
                        Constants.Common.Collaterals, collaterals
                    },
                    {
                        Constants.Common.IsFixed, isFixed
                    },
                    {
                        Constants.Common.IsSatisfied, isSatisfied
                    },
                    {
                        Constants.Common.ClientRating, clientRating
                    },
                     {
                        Constants.Params.JobOrderStatus, status.ToString()
                    },
                    {
                        Constants.Params.Signature, signature
                    },
                    {
                        Constants.Keys.ServerJobOrderID, serverJobOrderID.ToString()
                    },
                    {
                        Constants.Keys.LocalJobOrderID, localJobOrderID.ToString()
                    }

                };
            await _navigationService.Navigate<ThirdViewModel, Dictionary<string, string>>(param);
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
                            var successMessage = LocalizeService.Translate(Constants.Messages.UnSuccessfulDeletion);
                            await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
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
                catch (Exception)
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

        public IMvxAsyncCommand EmailJobOrderCommand => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string>();
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

                var serializedJobOrder = _serializer.SerializeObject(localJobOrder);

                var param = new Dictionary<string, string> { };

                param.Add(Constants.Params.SelectedJobOrder, serializedJobOrder);

                await _navigationService.Navigate<EditJOFirstViewModel, Dictionary<string, string>>(param);
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
    }
}