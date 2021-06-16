
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.EmailJO;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace MobileJO.Core.ViewModels
{
    public class ThirdViewModel : BaseViewModel
    {
        public ObservableCollection<AttachmentModel> _attachments { get; set; } = new ObservableCollection<AttachmentModel>();
        public ObservableCollection<JobOrderBillingTypeModel> _billingTypes { get; set; } = new ObservableCollection<JobOrderBillingTypeModel>();
        public JobOrder jobOrder;

        public JobOrder _jobOrderModel { get; set; } = new JobOrder();
        public BillingType _billing { get; set; } = new BillingType();
        public LocalJobOrder LocalJO { get; set; }

        public JobOrder JobOrderModel { get; set; }

        private AttachmentModel _selectedAttachment;
        public AttachmentModel SelectedAttachment
        {
            get => _selectedAttachment;
            set
            {
                SetProperty(ref _selectedAttachment, value);
                if (_selectedAttachment != null)
                {
                    attachmentID = _selectedAttachment.JobOrderID;
                    fileName = _selectedAttachment.Filename;
                    DownloadFile.Execute();       
                } 
            }
        }

        private void OnPropertyChanged(Func<AttachmentModel> p)
        {
            throw new NotImplementedException();
        }

        private bool _signature;
        public bool Signature
        {
            get => _signature;
            set => SetProperty(ref _signature, value);
        }

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

        private List<string> _attachment;
        public List<string> Attachment
        {
            get => _attachment;
            set => SetProperty(ref _attachment, value);
        }
        //Setting button values is until here

        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private Dictionary<string, string> _parameter;
        public int id { get; set; }
        public string nextStep;
        public string preventiveAction;
        public string remarks;
        public string attendees;
        private int attachmentID;
        private string fileName;
        public string billed;
        public string collaterals;
        public string isFixed;
        public string isSatisfied;
        public string clientRating;
        public int status;
        public string signature;
        public int serverJobOrderID;
        public int localJobOrderID;

        public ThirdViewModel(IMvxNavigationService navigationService,
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

            if (_parameter != null && _parameter.ContainsKey(Constants.Common.ID))
            {
                id = int.Parse(_parameter[Constants.Common.ID]);
                nextStep = _parameter[Constants.Common.NextStep];
                remarks = _parameter[Constants.Common.Remarks];
                preventiveAction = _parameter[Constants.Common.PreventiveAction];
                attendees = _parameter[Constants.Common.Attendees];
                billed = _parameter[Constants.Common.Billed];
                collaterals = _parameter[Constants.Common.Collaterals];
                isFixed = _parameter[Constants.Common.IsFixed];
                isSatisfied = _parameter[Constants.Common.IsSatisfied];
                clientRating = _parameter[Constants.Common.ClientRating];
                status = int.Parse(_parameter[Constants.Params.JobOrderStatus]);
                signature = _parameter[Constants.Params.Signature];
                serverJobOrderID = int.Parse(_parameter[Constants.Keys.ServerJobOrderID]);
                localJobOrderID = int.Parse(_parameter[Constants.Keys.LocalJobOrderID]);

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
                        ID = id,
                        Is_Billed = bool.Parse(billed),
                        Is_Collaterals = bool.Parse(collaterals),
                        ClientSignature = signature,
                        Is_Satisfied = bool.Parse(isSatisfied),
                        Is_Fixed = bool.Parse(isFixed),
                        ClientRating = int.Parse(clientRating)
                    };

                    ObservableCollection<JobOrderBillingTypeModel> _billingTypes = new ObservableCollection<JobOrderBillingTypeModel>(await _webService.GetBillingList(id));
                    _attachments = new ObservableCollection<AttachmentModel>(await _webService.GetAttachmentList(id));

                    SetButtonStatus(status);
                }
                else
                {

                    var appTypes = MvxApp.Database.GetAllApplicationTypesAsync();

                    JobOrderModel = new JobOrder()
                    {
                        ID = id,
                        Is_Billed = bool.Parse(billed),
                        Is_Collaterals = bool.Parse(collaterals),
                        ClientSignature = signature,
                        Is_Satisfied = bool.Parse(isSatisfied),
                        Is_Fixed = bool.Parse(isFixed),
                        ClientRating = int.Parse(clientRating)
                    };

                    var attachments = MvxApp.Database.GetAttachmentsAsync();

                    var localID = int.Parse(_parameter[Constants.Keys.LocalJobOrderID]);
                    var serverID = int.Parse(_parameter[Constants.Keys.ServerJobOrderID]);

                    var joBillingType = new List<JobOrderBillingType>();
                    var joAttachment  = new List<Attachment>();

                    if (serverID > 0)
                    {
                        joAttachment = MvxApp.Database.GetJOAttachments(serverID);
                    }
                    else
                    {
                        joAttachment = MvxApp.Database.GetLocalJOAttachments(localID);
                    }

                    foreach (var attachment in joAttachment)
                    {
                        _attachments.Add(new AttachmentModel
                        {
                            ID = attachment.ID,
                            JobOrderID = attachment.JobOrderID,
                            Filename = attachment.Filename
                        });
                    }

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

        public IMvxAsyncCommand DownloadFile => new MvxAsyncCommand(async () =>
        {
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    var param = new Dictionary<string, string> { { Constants.Params.ID, _selectedAttachment.JobOrderID.ToString() },
                                                             { Constants.Params.FileName,  _selectedAttachment.Filename },
                                                             { Constants.Params.AttachmentType, Constants.Params.Attachments } };



                    var localizedMessage = string.Empty;

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        localizedMessage = LocalizeService.Translate(Constants.Messages.DownloadConfirmation);
                        
                    }
                    else if(Device.RuntimePlatform == Device.iOS)
                    {
                        localizedMessage = LocalizeService.Translate(Constants.Messages.OpenConfirmation);
                    }

                    var dialogResult = await _userDialogs.ConfirmAsync(localizedMessage,
                                                                       Constants.Modal.Confirmation,
                                                                       Constants.Messages.Yes,
                                                                       Constants.Messages.No);

                    if (dialogResult == true)
                    {
                        PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                        if (status != PermissionStatus.Granted)
                        {
                            var result = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                            status = result[Permission.Storage];
                        }

                        if (status == PermissionStatus.Granted)
                        {
                            var nativeDirectoryService = DependencyService.Get<IDirectory>();

                            var folderPath = nativeDirectoryService.CreateDirectory("MobileJO");

                            byte[] fileContent = await _webService.DownloadFile(param);

                            var targetPath = string.Concat(folderPath,
                                                           "/",
                                                           string.Concat(Constants.Common.JobOrderNumberPrefix,
                                                                         _selectedAttachment.JobOrderID.ToString(Constants.Common.JobOrderNumberFormat)),
                                                           "/Attachments"); // ..JO-000011/Attachments

                            var filePath = await Data.FileAppData.SaveFile(fileContent, _selectedAttachment.Filename, targetPath);

                            if (Device.RuntimePlatform == Device.iOS)
                            {
                                var uri = new Uri(string.Format(Uri.EscapeUriString(filePath)));
                                var newFilePath = filePath.Replace(" ", "");
                                nativeDirectoryService.OpenFile(string.Format(Uri.EscapeUriString(filePath)));
                            }
                            else if(Device.RuntimePlatform == Device.Android)
                            {
                                var successMessage = LocalizeService.Translate(Constants.Messages.SuccessDownload);
                                await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                            }                            
                        }
                    }

                    SelectedAttachment = null;
                }
                else
                {
                    var noInternetMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                    await UserDialogs.AlertAsync(noInternetMessage, Constants.Modal.Warning, Constants.Common.OK);

                    SelectedAttachment = null;
                }
            }
            catch(Exception e)
            {
                error = true;

                if (error)
                {
                    var localizedMessage = LocalizeService.Translate(e.Message);
                    await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                }
            }

            if (error)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
        });

        public IMvxAsyncCommand DeleteJobOrder => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;


            if (NetworkCheck.HasInternet())
            {
                try
                {
                    var localizedMessage = LocalizeService.Translate(Constants.Messages.DeleteConfirmation);
                    var dialogResult = await _userDialogs.ConfirmAsync(localizedMessage, Constants.Modal.Confirmation, Constants.Messages.Yes, Constants.Messages.No);

                    if (dialogResult == true)
                    {
                        var result = await _webService.DeleteJobOrder(id);
                        if (result == true)
                        {
                            var del = MvxApp.Database.DeleteJOAsync(id);

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

                        if (checkStatus.StatusID == 1 && checkStatus.JobOrderNumber != null)
                        {
                            var del = MvxApp.Database.DeleteJOAsync(id);
                            var successMessage = LocalizeService.Translate(Constants.Messages.DeleteToLocalSuccess);
                            await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                        }

                        if (checkStatus.StatusID == 1 && checkStatus.JobOrderNumber == null)
                        {
                            //If job order is created offline and not yet sync to the server
                            var del = MvxApp.Database.DeleteIndividualJOAsync(id);

                            var successMessage = LocalizeService.Translate(Constants.Messages.SuccessDeletion);
                            await UserDialogs.AlertAsync(successMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                        }

                        GoToJobOrderList.Execute();
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
        });

        public IMvxAsyncCommand GoToJobOrderList => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string>();
            await _navigationService.Navigate<JobOrderListViewModel, Dictionary<string, string>>(param);
        });

        public IMvxAsyncCommand GoToSecondPage => new MvxAsyncCommand(async () =>
        {
            var parameter = new Dictionary<string, string>
                {
                    {
                        Constants.Common.ID, id.ToString()
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
            await _navigationService.Navigate<SecondViewModel, Dictionary<string, string>>(parameter);

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
                       RevertModel count = await _webService.GetRevertCount(id);
                       if (count == null)
                       {
                           var result = await _webService.RevertJobOrder(id);
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

        public IMvxAsyncCommand ViewSignature => new MvxAsyncCommand(async () =>
        {
            if (NetworkCheck.HasInternet())
            {
                var param = new Dictionary<string, string> {
                    { Constants.Params.ID, serverJobOrderID.ToString() },
                    { Constants.Params.FileName,  signature },
                    { Constants.Params.AttachmentType, Constants.Params.Signature }
                };

                await _navigationService.Navigate<SignatureViewModel, Dictionary<string, string>>(param);
            }
            else
            {
                //var noInternetMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                //await UserDialogs.AlertAsync(noInternetMessage, Constants.Modal.Warning, Constants.Common.OK);
                string filename = string.Empty;
                string filePath = string.Empty;
                var signatureParam = new Dictionary<string, string>();

                if (serverJobOrderID > 0)
                {
                    filename = string.Concat(serverJobOrderID, Constants.Common.SignatureNameExtension);
                    filePath = string.Concat(serverJobOrderID, Constants.Uploads.SignatureTargetFolder);
                }
                else
                {
                    filename = string.Concat(localJobOrderID, Constants.Common.SignatureNameExtension);
                    filePath = string.Concat(localJobOrderID, Constants.Uploads.SignatureTargetFolder);
                }

                if (_parameter.ContainsKey(Constants.Params.SignatureName))
                {
                    _parameter[Constants.Params.SignatureName] = filename;
                }
                else
                {
                    _parameter.Add(Constants.Params.SignatureName, filename);
                }

                if (_parameter.ContainsKey(Constants.Params.SignaturePath))
                {
                    _parameter[Constants.Params.SignaturePath] = filePath;
                }
                else
                {
                    _parameter.Add(Constants.Params.SignaturePath, filePath);
                }

                signatureParam = _parameter;
                await _navigationService.Navigate<SignatureViewModel, Dictionary<string, string>>(signatureParam);
            }
        });

        public IMvxAsyncCommand ViewBillingType => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string> { };

            if (NetworkCheck.HasInternet())
            {
                param.Add(Constants.Common.ID, serverJobOrderID.ToString());
            }
            else
            {                
                param.Add(Constants.Keys.ServerJobOrderID, serverJobOrderID.ToString());
                param.Add(Constants.Keys.LocalJobOrderID, localJobOrderID.ToString());                
            }
            
            await _navigationService.Navigate<BillingListViewModel, Dictionary<string, string>>(param);
        });

        public IMvxAsyncCommand EmailJobOrderCommand => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string>();
            await _navigationService.Navigate<EmailJOViewModel, Dictionary<string, string>>(param);
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

        private void SetButtonStatus(int statusID)
        {
            switch (statusID)
            {
                case Constants.Params.PendingValue:
                    EnableEdit = true;
                    EnableRevert = false;
                    EnableEmailJO = false;
                    EnableDelete = true;
                    Signature = false;
                    break;

                case Constants.Params.SignedValue:
                    EnableEdit = true;
                    EnableRevert = false;
                    EnableEmailJO = true;
                    EnableDelete = false;
                    Signature = true;
                    break;

                case Constants.Params.SentValue:
                    EnableEdit = false;
                    EnableRevert = true;
                    EnableEmailJO = false;
                    EnableDelete = false;
                    Signature = true;
                    break;

                case Constants.Params.RequestRevertValue:
                    EnableEdit = false;
                    EnableRevert = false;
                    EnableEmailJO = false;
                    EnableDelete = false;
                    Signature = true;
                    break;

            }
        }
    }
}
