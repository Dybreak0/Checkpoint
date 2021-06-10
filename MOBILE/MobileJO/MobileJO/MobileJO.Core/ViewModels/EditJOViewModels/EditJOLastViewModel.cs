using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileJO.Core.ViewModels
{
    class EditJOLastViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private readonly IWebService _webService;

        private Dictionary<string, string> _parameter;

        public ObservableCollection<FileData> _pickedFiles { get; private set; } = new ObservableCollection<FileData>();

        public ObservableCollection<Attachment> Attachments { get; private set; } = new ObservableCollection<Attachment>();

        public List<Attachment> _attachments { get; private set; }  = new List<Attachment>();
        
        private LocalJobOrder jobOrderItem { get; set; }

        public bool IsBilled { get; set; }
        public bool IsCollaterals { get; set; }
        public bool IsFixed { get; set; }
        public bool IsSatisfied { get; set; }
        public int _clientRating;
        public int ClientRating
        {
            get => _clientRating;
            set
            {                
                SetProperty(ref _clientRating, value);                
            }
        }
        
        public bool IsSignatureEnabled { get; set; } = false;
        public bool IsFixedEnabled { get; set; } = false;
        public bool IsSatisfiedEnabled { get; set; } = false;
        public bool IsRatingEnabled { get; set; } = false;
        private List<string> RemovedAttachments { get; set; } = new List<string>();

        private string _iconFile;
        public string IconFile
        {
            get => _iconFile;
            set => SetProperty(ref _iconFile, value);
        }

        private string _signatureBytes { get; set; }
        private string _serializedBillingTypes { get; set; }
        public string FileErrorMsg { get; set; } = Constants.Messages.ErrorExistingFile;

        public EditJOLastViewModel(IMvxNavigationService navigationService, 
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

            PopulateFields.Execute();
        }

        public IMvxCommand PopulateFields => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (_parameter.ContainsKey(Constants.Params.SelectedJobOrder))
                {
                    jobOrderItem = _serializer.DeserializeObject<LocalJobOrder>(_parameter[Constants.Params.SelectedJobOrder]);
                }

                IsBilled      = jobOrderItem.IsBilled;
                IsCollaterals = jobOrderItem.IsCollaterals;

                if(string.IsNullOrEmpty(jobOrderItem.ClientSignature))
                {
                    _iconFile = "add_button.png";
                    IsSignatureEnabled = true;
                    IsFixedEnabled = false;
                    IsSatisfiedEnabled = false;
                    IsRatingEnabled = false;
                }       
                else
                {
                    _iconFile = "view_button.png";
                    IsSignatureEnabled = true;
                }

                IsFixed       = jobOrderItem.IsFixed;
                IsSatisfied   = jobOrderItem.IsSatisfied;
                ClientRating  = jobOrderItem.ClientRating;

                GetAttachmentsAsync();                

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

        public async void GetAttachmentsAsync()
        {
            if (NetworkCheck.HasInternet())
            {
                _attachments = await _webService.AttachmentList(jobOrderItem.ServerID);

                foreach (var attachment in _attachments)
                {
                    Attachments.Add(attachment);
                }
            }
            else
            {
                if (jobOrderItem.ServerID > 0)
                {
                    _attachments = MvxApp.Database.GetJOAttachments(jobOrderItem.ServerID);
                }
                else
                {
                    _attachments = MvxApp.Database.GetLocalJOAttachments(jobOrderItem.ID);
                }

                foreach (var attachment in _attachments)
                {
                    Attachments.Add(attachment);
                }
            }
        }

        public bool IsChangedCases()
        {
            return _parameter.ContainsKey(Constants.Params.EditedCases);
        }

        public bool IsChangedBillingTypes()
        {
            return _parameter.ContainsKey(Constants.Params.EditedBillingTypes);
        }

        public IMvxCommand UpdateJobOrderCommand => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {

                if (string.IsNullOrEmpty(_serializedBillingTypes))
                {
                    var defaultBT = new List<JobOrderBillingType>();

                    if (NetworkCheck.HasInternet())
                    {
                        defaultBT = await _webService.JobOrderBillingTypeList(jobOrderItem.ServerID);
                    }
                    else
                    {                       
                        defaultBT = jobOrderItem.ServerID > 0
                                  ? MvxApp.Database.GetJOBillingTypes(jobOrderItem.ServerID)
                                  : MvxApp.Database.GetLocalJOBillingTypes(jobOrderItem.ID);
                    }

                    if (defaultBT.Count <= 0)
                    {
                        var localizedMessage = LocalizeService.Translate(Constants.Messages.BillingTypeRequired);
                        await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(_signatureBytes) && (ClientRating < 1 || ClientRating > 5))
                {
                    await UserDialogs.AlertAsync(Constants.Messages.InvalidClientRating, Constants.Modal.Warning, Constants.Common.OK);
                    return;
                }

                bool confirmSave = await _userDialogs.ConfirmAsync(Constants.Messages.ConfirmSave,
                                                                   Constants.Modal.Confirmation,
                                                                   Constants.Messages.Yes,
                                                                   Constants.Messages.No);

                if (confirmSave)
                {
                    var lastPageVM = new LastPageViewModel
                    {
                        IsBilled = IsBilled,
                        IsCollaterals = IsCollaterals,
                        IsFixed = IsFixed,
                        IsSatisfied = IsSatisfied,
                        ClientRating = ClientRating
                    };

                    var deserializedFirstPage = _serializer.DeserializeObject<FirstPageViewModel>(_parameter[Constants.Params.FirstPage]);
                    var deserializedSecondPage = _serializer.DeserializeObject<SecondPageViewModel>(_parameter[Constants.Params.SecondPage]);

                    var userID = int.Parse(CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID));

                    var jobOrder = new LocalJobOrder
                    {
                        ID = jobOrderItem.ID,
                        ServerID = jobOrderItem.ServerID,
                        JobOrderNumber = jobOrderItem.JobOrderNumber,
                        Branch = deserializedFirstPage.Branch,
                        AccountID = deserializedFirstPage.AccountID,
                        ApplicationTypeID = deserializedFirstPage.ApplicationType,
                        JobOrderSubject = deserializedFirstPage.JobOrderSubject,
                        DateTimeStart = DateTime.TryParseExact(deserializedFirstPage.DateTimeStart,
                                                                Constants.Common.DateTimeFormat,
                                                                null,
                                                                DateTimeStyles.None,
                                                                out DateTime dateTimeStart) ? dateTimeStart : DateTime.Now,

                        DateTimeEnd = DateTime.TryParseExact(deserializedFirstPage.DateTimeEnd,
                                                            Constants.Common.DateTimeFormat,
                                                            null,
                                                            DateTimeStyles.None,
                                                            out DateTime dateTimeEnd) ? dateTimeEnd : DateTime.Now,

                        ActivityDetails = deserializedFirstPage.ActivityDetails,
                        RootCauseAnalysis = deserializedFirstPage.RootCauseAnalysis,
                        NextStep = deserializedSecondPage.NextStep,
                        PreventiveAction = deserializedSecondPage.PreventiveAction,
                        Remarks = deserializedSecondPage.Remarks,
                        Attendees = deserializedSecondPage.Attendees,
                        IsBilled = lastPageVM.IsBilled,
                        IsCollaterals = lastPageVM.IsCollaterals,
                        IsFixed = lastPageVM.IsFixed,
                        IsSatisfied = lastPageVM.IsSatisfied,
                        ClientRating = lastPageVM.ClientRating,
                        ClientSignature = jobOrderItem.ClientSignature,
                        StatusID = jobOrderItem.StatusID,
                        CreatedBy = jobOrderItem.CreatedBy,
                        CreatedDate = jobOrderItem.CreatedDate,
                        UpdatedBy = userID,
                        UpdatedDate = DateTime.Now,
                        LastSyncDate = jobOrderItem.LastSyncDate
                    };

                    if (NetworkCheck.HasInternet() && !string.IsNullOrEmpty(jobOrder.JobOrderNumber))
                    {
                        var caseIDs = new List<int>();
                        var billingTypeIDs = new List<int>();
                        var attachmentFiles = new List<FileViewModel>();

                        if (IsChangedCases())
                        {
                            var newCases = _serializer.DeserializeObject<List<Models.AssignedCases>>(_parameter[Constants.Params.EditedCases]);

                            foreach (var newCase in newCases)
                            {
                                caseIDs.Add(newCase.ID);
                            }
                        }
                        else
                        {
                            var currentTaggedCases = await _webService.TaggedCaseList(jobOrderItem.ServerID);

                            foreach (var currentTaggedCase in currentTaggedCases)
                            {
                                caseIDs.Add(currentTaggedCase.CaseID);
                            }
                        }

                        if (!string.IsNullOrEmpty(_serializedBillingTypes))
                        {
                            var newBillingTypes = _serializer.DeserializeObject<List<BillingTypes>>(_serializedBillingTypes);

                            foreach (var newBillingType in newBillingTypes)
                            {
                                billingTypeIDs.Add(newBillingType.ID);
                            }
                        }
                        else
                        {
                            var currentBillingTypes = await _webService.JobOrderBillingTypeList(jobOrderItem.ServerID);

                            foreach (var currentBillingType in currentBillingTypes)
                            {
                                billingTypeIDs.Add(currentBillingType.BillingTypeID);
                            }
                        }

                        foreach (FileData pickedFile in _pickedFiles)
                        {
                            attachmentFiles.Add(new FileViewModel
                            {
                                FileName = pickedFile.FileName,
                                FileDataArray = pickedFile.DataArray
                            });
                        }

                        FileViewModel signatureModel = null;
                        int statusID = jobOrder.StatusID;

                        if (!string.IsNullOrEmpty(_signatureBytes))
                        {
                            var signatureStringArray = _signatureBytes.Split(Constants.SpecialCharacters.CharComma);
                            var signatureBytes = signatureStringArray.Select(byte.Parse).ToArray();

                            signatureModel = new FileViewModel
                            {
                                FileName = Constants.Common.SignatureNameExtension,
                                FileDataArray = signatureBytes
                            };

                            statusID = (int)Constants.Status.Signed;
                        }

                        var jobOrderViewModel = new JobOrderDetailsViewModel
                        {
                            ID = jobOrder.ServerID,
                            JobOrderNumber = jobOrder.JobOrderNumber,
                            Branch = jobOrder.Branch,
                            AccountID = jobOrder.AccountID,
                            ApplicationTypeID = jobOrder.ApplicationTypeID,
                            JobOrderSubject = jobOrder.JobOrderSubject,
                            DateTimeStart = jobOrder.DateTimeStart,
                            DateTimeEnd = jobOrder.DateTimeEnd,
                            ActivityDetails = jobOrder.ActivityDetails,
                            RootCauseAnalysis = jobOrder.RootCauseAnalysis,
                            NextStep = jobOrder.NextStep,
                            PreventiveAction = jobOrder.PreventiveAction,
                            Remarks = jobOrder.Remarks,
                            Attendees = jobOrder.Attendees,
                            IsBilled = jobOrder.IsBilled,
                            IsCollaterals = jobOrder.IsCollaterals,
                            ClientSignature = jobOrder.ClientSignature,
                            IsFixed = jobOrder.IsFixed,
                            IsSatisfied = jobOrder.IsSatisfied,
                            ClientRating = jobOrder.ClientRating,
                            StatusID = statusID,
                            CreatedBy = jobOrder.CreatedBy,
                            CreatedDate = jobOrder.CreatedDate,
                            NewJOCases = caseIDs,
                            NewJOBillingTypes = billingTypeIDs,
                            JobOrderAttachments = attachmentFiles,
                            RemovedAttachments = RemovedAttachments,
                            Signature = signatureModel
                        };

                        bool updateSuccess = await _webService.UpdateJobOrderDetails(jobOrderViewModel);

                        if (updateSuccess)
                        {
                            MvxApp.Database.SaveJobOrderAsync(jobOrder);

                            

                            UpdateCases(jobOrder.ID, jobOrder.ServerID);
                            UpdateBillingTypes(jobOrder.ID, jobOrder.ServerID);
                            UpdateAttachments(jobOrder.ID, jobOrder.ServerID);

                            if (!string.IsNullOrEmpty(_signatureBytes))
                            {
                                await SaveSignature(jobOrder.ID, 
                                                    jobOrder.ServerID);
                            }

                            await _userDialogs.AlertAsync(Constants.Messages.UpdateToServerSuccess, 
                                                          Constants.Modal.InfoMessage, 
                                                          Constants.Common.OK);
                        }                        
                    }
                    else
                    {
                        MvxApp.Database.SaveJobOrderAsync(jobOrder);

                        UpdateCases(jobOrder.ID, jobOrder.ServerID);
                        UpdateBillingTypes(jobOrder.ID, jobOrder.ServerID);
                        UpdateAttachments(jobOrder.ID, jobOrder.ServerID);

                        if (!string.IsNullOrEmpty(_signatureBytes))
                        {
                            await SaveSignature(jobOrder.ID, 
                                                jobOrder.ServerID);
                        }

                        await _userDialogs.AlertAsync(Constants.Messages.UpdateToLocalSuccess, Constants.Modal.InfoMessage, Constants.Common.OK);
                    }

                    var jobOrderServerID = string.IsNullOrEmpty(jobOrder.ServerID.ToString()) ? "0" : jobOrder.ServerID.ToString();

                    var param = new Dictionary<string, string>
                    {
                        {
                            Constants.Keys.ServerJobOrderID, jobOrderServerID
                        },
                        {
                            Constants.Keys.LocalJobOrderID, jobOrder.ID.ToString()
                        },
                    };
                    var presentationBundle = new MvxBundle(new Dictionary<string, string> { { "NavigationMode", string.Empty } });
                    await _navigationService.Navigate<JobOrderViewModel, Dictionary<string, string>>(param, presentationBundle);
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
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
            
            
        });

        public async Task SaveSignature(int localID, int serverID)
        {
            var id = serverID > 0 ? serverID : localID;

            var signatureStringArray = _signatureBytes.Split(Constants.SpecialCharacters.CharComma);
            var signatureBytes = signatureStringArray.Select(byte.Parse).ToArray();
            var signatureFilename = string.Concat(id, Constants.Common.SignatureNameExtension);
            var signaturePath = string.Concat(id, Constants.Uploads.SignatureTargetFolder);

            LocalJobOrder joForUpdate = new LocalJobOrder();

            if (serverID > 0)
            {
                joForUpdate = MvxApp.Database.GetJobOrderByServerIDAsync(serverID);  
            }
            else
            {
                joForUpdate = MvxApp.Database.GetJobOrderAsync(localID);
            }

            if (joForUpdate != null)
            {
                joForUpdate.ClientSignature = signatureFilename;
                joForUpdate.StatusID        = (int)Constants.Status.Signed;
                joForUpdate.UpdatedBy       = int.Parse(CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID));
                joForUpdate.UpdatedDate     = DateTime.Now;

                MvxApp.Database.SaveJobOrderAsync(joForUpdate);
            }

            await Data.FileAppData.SaveFile(signatureBytes, signatureFilename, signaturePath);            
        }

        public void UpdateCases(int localID, int serverID)
        {
            var deserializedCases = new List<Models.AssignedCases>();
            
            if(serverID > 0)
            {
                MvxApp.Database.DeleteTaggedCases(serverID);
            }
            else
            {
                MvxApp.Database.DeleteLocalTaggedCases(localID);
            }

            var jobOrderID = serverID > 0
                           ? serverID
                           : localID;

            if (_parameter.ContainsKey(Constants.Params.EditedCases))
            {
                deserializedCases = _serializer.DeserializeObject<List<Models.AssignedCases>>(_parameter[Constants.Params.EditedCases]);
            }
            else
            {
                deserializedCases = _serializer.DeserializeObject<List<Models.AssignedCases>>(_parameter[Constants.Params.DefaultCases]);
            }

            var CasesToAdd = new List<TaggedCase>();

            foreach (var cases in deserializedCases)
            {
                CasesToAdd.Add(new TaggedCase
                {
                    ID = serverID > 0 ? 1 : 0,
                    JobOrderID = jobOrderID,
                    CaseID = cases.ID
                });
            }

            MvxApp.Database.SaveCases(CasesToAdd);             
        }

        public void UpdateBillingTypes(int localID, int serverID)
        { 
            var deserializedBillingTypes = new List<BillingTypes>();
            var billingTypesToAdd        = new List<JobOrderBillingType>();
            var defaultBillingTypes      = new List<JobOrderBillingType>();

            if (serverID > 0)
            {                
                defaultBillingTypes = MvxApp.Database.GetJOBillingTypes(serverID);

                MvxApp.Database.DeleteBillingTypes(serverID);
            }
            else
            {                
                defaultBillingTypes = MvxApp.Database.GetLocalJOBillingTypes(localID);

                MvxApp.Database.DeleteLocalBillingTypes(localID);
            }

            var jobOrderID = serverID > 0
                           ? serverID
                           : localID;

            if (!string.IsNullOrEmpty(_serializedBillingTypes))
            {
                deserializedBillingTypes = _serializer.DeserializeObject<List<BillingTypes>>(_serializedBillingTypes);

                foreach (var billingType in deserializedBillingTypes)
                {
                    billingTypesToAdd.Add(new JobOrderBillingType
                    {
                        ID = serverID > 0 ? 1 : 0,
                        JobOrderID = jobOrderID,
                        BillingTypeID = billingType.ID
                    });
                }
            }
            else
            {                
                billingTypesToAdd = defaultBillingTypes;
            }                        

            MvxApp.Database.SaveBillingTypes(billingTypesToAdd);            
        }

        public async void UpdateAttachments(int localID, int serverID)
        {
            var jobOrderID = serverID > 0
                           ? serverID
                           : localID;

            var folderPath = string.Concat(jobOrderID, Constants.Uploads.AttachmentsTargetFolder);            

            foreach (string attachmentName in RemovedAttachments)
            {
                if(serverID > 0)
                {
                    MvxApp.Database.DeleteAttachments(serverID, attachmentName);
                }
                else
                {
                    var filePath = string.Concat(localID, Constants.Uploads.AttachmentsTargetFolder);
                    var fileExists = await Data.FileAppData.IsFileExistAsync(attachmentName, filePath);

                    if (fileExists)
                    {
                        await Data.FileAppData.DeleteFile(attachmentName, filePath);
                    }
                }
                
            }
                 
            List<Attachment> attachments = new List<Attachment>();

            if(_pickedFiles.Count > 0)
            {
                foreach (FileData file in _pickedFiles)
                {
                    attachments.Add(new Attachment
                    {
                        ID = serverID > 0 ? 1 : 0,
                        JobOrderID = jobOrderID,
                        Filename = file.FileName
                    });

                    await Data.FileAppData.SaveFile(file.DataArray, file.FileName, folderPath);
                }

                MvxApp.Database.SaveAttachments(attachments);
            }            
        }

        public IMvxCommand OpenFilePickerCommand => new MvxCommand(async () =>
        {
            var error = false;

            try
            {
                if (Attachments.Count() == 3)
                {
                    await _userDialogs.AlertAsync(Constants.Messages.MaxAttachmentsError,
                                                  Constants.Modal.InfoMessage,
                                                  Constants.Common.OK);
                    return;
                }

                PickAndShowFile.Execute();
            }            
            catch(Exception ex)
            {
                error = true;
            }

            if (error)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
        });

        public IMvxCommand PickAndShowFile => new MvxCommand(async () =>
        {

           var error = false;

           try
           {
               FileData currentFile = await CrossFilePicker.Current.PickFile();

               if (currentFile != null)
               {
                   const int ThreeMegaBytes = 3145728;

                   int fileSize = currentFile.DataArray.Length;

                   if (currentFile.FileName.Length > 250)
                   {
                       FileErrorMsg = Constants.Messages.InvalidFilenameError;
                       return;
                   }
                   else if (fileSize == 0)
                   {
                       FileErrorMsg = Constants.Messages.EmptyFileError;
                       return;
                   }
                   else if (fileSize > ThreeMegaBytes)
                   {
                       FileErrorMsg = Constants.Messages.InvalidFileSizeError;
                       return;
                   }
                   else if (IsExistingFile(currentFile.FileName))
                   {
                       FileErrorMsg = Constants.Messages.ErrorExistingFile;
                       return;
                   }
                   else
                   {
                       _pickedFiles.Add(currentFile);
                       Attachments.Add(new Attachment
                       {
                           Filename = currentFile.FileName
                       });
                       FileErrorMsg = Constants.Messages.ErrorExistingFile;
                    }

               }
           }
           catch (Exception ex)
           {
               error = true;
           }

           if (error)
           {
               var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorUploading);
               await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
           }
        });

        public IMvxCommand DrawSignaturePageCommand => new MvxAsyncCommand(async () =>
        {
            if (string.IsNullOrEmpty(jobOrderItem.ClientSignature))
            {
                if(!string.IsNullOrEmpty(_signatureBytes))
                {                    
                    var param = new Dictionary<string, string>
                    {
                        { Constants.Params.SignatureBytes, _signatureBytes }
                    };

                    var result = await _navigationService.Navigate<ViewSignatureViewModel, Dictionary<string, string>, string>(param);

                    if(result == Constants.Params.SignAgain)
                    {
                        IconFile = "add_button.png";
                        _signatureBytes = null;

                        IsFixedEnabled = false;
                        IsSatisfiedEnabled = false;
                        IsRatingEnabled = false;
                        IsFixed = false;
                        IsSatisfied = false;
                        ClientRating = 0;
                    }
                }
                else
                {
                    _signatureBytes = await _navigationService.Navigate<ClientSignatureViewModel, Dictionary<string, string>, string>(_parameter);

                    if (!string.IsNullOrEmpty(_signatureBytes))
                    {
                        IconFile = "view_button.png";

                        IsFixedEnabled = true;
                        IsSatisfiedEnabled = true;
                        IsRatingEnabled = true;
                    }
                }                
            }
            else
            {
                ViewSignaturePageCommand.Execute();
            }
        }, allowConcurrentExecutions: true);

        public IMvxCommand ViewSignaturePageCommand => new MvxCommand(async () =>
        {
            string filename = string.Empty;
            string filePath = string.Empty;
            var signatureParam = new Dictionary<string, string>();

            if (NetworkCheck.HasInternet())
            {
                if (jobOrderItem.ServerID > 0)
                {
                    filename = string.Concat(jobOrderItem.ServerID, Constants.Common.SignatureNameExtension);

                    var param = new Dictionary<string, string> {
                        { Constants.Params.ID, jobOrderItem.ServerID.ToString() },
                        { Constants.Params.FileName,  filename },
                        { Constants.Params.AttachmentType, Constants.Params.Signature },
                    };

                    signatureParam = param;
                }                
            }
            else
            {
                if (jobOrderItem.ServerID > 0)
                {
                    filename = string.Concat(jobOrderItem.ServerID, Constants.Common.SignatureNameExtension);
                    filePath = string.Concat(jobOrderItem.ServerID, Constants.Uploads.SignatureTargetFolder);
                }
                else
                {
                    filename = string.Concat(jobOrderItem.ID, Constants.Common.SignatureNameExtension);
                    filePath = string.Concat(jobOrderItem.ID, Constants.Uploads.SignatureTargetFolder);
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
            }                         

            await _navigationService.Navigate<ViewSignatureViewModel, Dictionary<string, string>>(signatureParam);
        });

        public IMvxCommand GoToBillingTypesPage => new MvxAsyncCommand(async () =>
        {
            _serializedBillingTypes = await _navigationService.Navigate<EditBillingTypesViewModel, 
                                                                        Dictionary<string, string>, 
                                                                        string>(_parameter);          
            
        }, allowConcurrentExecutions: true);

        public IMvxCommand ViewBillingTypesPage => new MvxAsyncCommand(async () =>
        {
            if(!string.IsNullOrEmpty(_serializedBillingTypes))
            {
                if (!_parameter.ContainsKey(Constants.Params.EditedBillingTypes))
                {
                    _parameter.Add(Constants.Params.EditedBillingTypes, _serializedBillingTypes);
                }
                else
                {
                    _parameter[Constants.Params.EditedBillingTypes] = _serializedBillingTypes;
                }
            }
               
            await _navigationService.Navigate<BillingTypesSelectedViewModel, Dictionary<string, string>>(_parameter);

        }, allowConcurrentExecutions : true);

        public IMvxCommand ConfirmRemoveAttachment => new MvxCommand<Attachment>(async (file) =>
        {

            var confirm = await _userDialogs.ConfirmAsync(Constants.Messages.ConfirmRemove,
                                                      Constants.Modal.Confirmation,
                                                      Constants.Messages.Yes,
                                                      Constants.Messages.No);

            if (confirm)
            {
                var index = 0;

                foreach (var x in Attachments)
                {
                    var parameter = file as Attachment;
                                        
                    if (x.Filename == parameter.Filename)
                    {
                        var checkAttachment = _attachments.Where(i => i.Filename == parameter.Filename).FirstOrDefault();

                        if (checkAttachment != null)
                        {
                            RemovedAttachments.Add(parameter.Filename);
                        }

                        _pickedFiles.Remove(_pickedFiles.Where(i => i.FileName == parameter.Filename).FirstOrDefault());

                        Attachments.RemoveAt(index);
                        break;
                    }
                    else
                    {
                        index++;
                    }
                }
            }

        });

        private IMvxCommand closeCommand;
        public IMvxCommand CloseCommand => closeCommand ?? (closeCommand = new MvxAsyncCommand(() => _navigationService.Close(this)));

        private bool IsExistingFile(string filename)
        {
            return Attachments.Where(x => x.Filename == filename).Any();
        }
    }
}
