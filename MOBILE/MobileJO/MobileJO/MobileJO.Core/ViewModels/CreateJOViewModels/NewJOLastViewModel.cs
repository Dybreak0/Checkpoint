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

namespace MobileJO.Core.ViewModels
{
    class NewJOLastViewModel : BaseViewModel
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

        public int _clientRating;
        public int ClientRating
        {
            get => _clientRating;
            set
            {
                SetProperty(ref _clientRating, value);
            }
        } 

        public bool IsBilled { get; set; }
        public bool IsCollaterals { get; set; }
        public bool IsFixed { get; set; }
        public bool IsSatisfied { get; set; }        
        public bool IsWarranty { get; set; }
        public bool IsAPS { get; set; }
        public bool IsWebPOS { get; set; }
        public bool IsPending { get; set; }        

        List<Models.AssignedCases> deserializedCases = new List<Models.AssignedCases>();
        List<BillingTypes> deserializedBillingTypes = new List<BillingTypes>();
        List<Attachment> attachments = new List<Attachment>();

        public List<TaggedCase> CasesToAdd { get; private set; } = new List<TaggedCase>();

        public List<int> CaseIDs { get; private set; } = new List<int>();
        public List<int> BillingTypeIDs { get; private set; } = new List<int>();  
        
        public bool BillingTypesError { get; set; }
        public bool ClientRatingError { get; set; }

        private string _iconFile;
        public string IconFile
        {
            get => _iconFile;
            set => SetProperty(ref _iconFile, value);
        }

        private string _signatureBytes { get; set; }
        private string _serializedBillingTypes { get; set; }
        public string FileErrorMsg { get; set; } = Constants.Messages.ErrorExistingFile;

        public bool IsFixedEnabled { get; set; } = false; 
        public bool IsSatisfiedEnabled { get; set; } = false;
        public bool IsRatingEnabled { get; set; } = false;

        public NewJOLastViewModel(IMvxNavigationService navigationService, 
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

            IconFile = "add_button.png";            
        }

        public IMvxCommand SaveJobOrderCommand => new MvxCommand(async () =>
        {
            if (string.IsNullOrEmpty(_serializedBillingTypes))
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.BillingTypeRequired);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                return;
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
                if (IsBusy)
                    return;

                IsBusy = true;
                var error = false;

                try
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

                    int statusID = !string.IsNullOrEmpty(_signatureBytes) ? (int)Constants.Status.Signed : (int)Constants.Status.Pending;
                    int userID = int.Parse(CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID));
                    int insertID = 0;

                    LocalJobOrder jobOrder = new LocalJobOrder
                    {
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
                        StatusID = statusID,
                        CreatedBy = userID,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = userID,
                        UpdatedDate = DateTime.Now,
                    };

                    var jo = jobOrder;

                    var createJOResponse = new JobOrderDetailsViewModel();

                    if (NetworkCheck.HasInternet())
                    {
                        deserializedCases = _serializer.DeserializeObject<List<Models.AssignedCases>>(_parameter[Constants.Params.Cases]);

                        foreach (var cases in deserializedCases)
                        {
                            CaseIDs.Add(cases.ID);
                        }

                        if (!string.IsNullOrEmpty(_serializedBillingTypes))
                        {
                            deserializedBillingTypes = _serializer.DeserializeObject<List<BillingTypes>>(_serializedBillingTypes);

                            foreach (var billingType in deserializedBillingTypes)
                            {
                                BillingTypeIDs.Add(billingType.ID);
                            }
                        }

                        var attachmentFiles = new List<FileViewModel>();

                        foreach (FileData pickedFile in _pickedFiles)
                        {
                            attachmentFiles.Add(new FileViewModel
                            {
                                FileName = pickedFile.FileName,
                                FileDataArray = pickedFile.DataArray
                            });
                        }

                        FileViewModel signatureModel = null;

                        if (!string.IsNullOrEmpty(_signatureBytes))
                        {
                            var signatureStringArray = _signatureBytes.Split(Constants.SpecialCharacters.CharComma);
                            var signatureBytes = signatureStringArray.Select(byte.Parse).ToArray();

                            signatureModel = new FileViewModel
                            {
                                FileName = Constants.Common.SignatureNameExtension,
                                FileDataArray = signatureBytes
                            };
                        }

                        var jobOrderViewModel = new JobOrderDetailsViewModel
                        {
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
                            IsFixed = jobOrder.IsFixed,
                            IsSatisfied = jobOrder.IsSatisfied,
                            ClientRating = jobOrder.ClientRating,
                            StatusID = jobOrder.StatusID,
                            NewJOCases = CaseIDs,
                            NewJOBillingTypes = BillingTypeIDs,
                            JobOrderAttachments = attachmentFiles,
                            Signature = signatureModel
                        };

                        createJOResponse = await _webService.SaveJobOrderDetails(jobOrderViewModel);

                        if (createJOResponse.ID > 0)
                        {
                            jobOrder.ServerID = createJOResponse.ID;
                            jobOrder.JobOrderNumber = string.Concat(Constants.Common.JobOrderNumberPrefix,
                                                                    createJOResponse.ID.ToString(Constants.Common.JobOrderNumberFormat));

                            MvxApp.Database.SaveJobOrderAsync(jobOrder);
                            insertID = jobOrder.ID;

                            if (!string.IsNullOrEmpty(_signatureBytes))
                            {
                                await SaveSignature(createJOResponse.ID);
                            }

                            SaveCases(createJOResponse.ID, true);
                            SaveBillingTypes(createJOResponse.ID, true);
                            SaveAttachments(createJOResponse.ID, true);
                        }

                        await _userDialogs.AlertAsync(Constants.Messages.SaveToServerSuccess,
                                                      Constants.Modal.InfoMessage,
                                                      Constants.Common.OK);
                    }
                    else
                    {
                        MvxApp.Database.SaveJobOrderAsync(jobOrder);

                        insertID = jobOrder.ID;

                        SaveCases(insertID, false);
                        SaveBillingTypes(insertID, false);
                        SaveAttachments(insertID, false);

                        if (!string.IsNullOrEmpty(_signatureBytes)) { await SaveSignature(insertID); }

                        await _userDialogs.AlertAsync(Constants.Messages.SaveToLocalSuccess,
                                                      Constants.Modal.InfoMessage,
                                                      Constants.Common.OK);

                    }

                    var param = new Dictionary<string, string>
                    {
                        {
                            Constants.Keys.ServerJobOrderID, createJOResponse.ID.ToString()
                        },
                        {
                            Constants.Keys.LocalJobOrderID, insertID.ToString()
                        },
                    };

                    var presentationBundle = new MvxBundle(new Dictionary<string, string> { { "NavigationMode", "" } });                    

                    await _navigationService.Navigate<JobOrderViewModel, Dictionary<string, string>>(param, presentationBundle);
                                            
                    
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
            }
            
        });

        public void SaveCases(int insertID, bool savedToServer)
        {

            deserializedCases = _serializer.DeserializeObject<List<Models.AssignedCases>>(_parameter[Constants.Params.Cases]);

            CasesToAdd = new List<TaggedCase>();

            foreach (var cases in deserializedCases)
            {
                CasesToAdd.Add(new TaggedCase
                {
                    ID = savedToServer ? 1 : 0,
                    JobOrderID = insertID,
                    CaseID = cases.ID
                });

                CaseIDs.Add(cases.ID); 
            }

            MvxApp.Database.SaveCases(CasesToAdd);
        }

        public void SaveBillingTypes(int insertID, bool savedToServer)
        {

            if (!string.IsNullOrEmpty(_serializedBillingTypes))
            {
                deserializedBillingTypes = _serializer.DeserializeObject<List<BillingTypes>>(_serializedBillingTypes);
            
                var billingTypesToAdd = new List<JobOrderBillingType>();

                foreach (var billingType in deserializedBillingTypes)
                {
                    billingTypesToAdd.Add(new JobOrderBillingType
                    {
                        ID = savedToServer ? 1 : 0,
                        JobOrderID = insertID,
                        BillingTypeID = billingType.ID
                    });

                }

                MvxApp.Database.SaveBillingTypes(billingTypesToAdd);
            }
        }

        public async void SaveAttachments(int insertID, bool savedToServer)
        {
            var targetPath = string.Concat(insertID, Constants.Uploads.AttachmentsTargetFolder);            

            foreach (var file in _pickedFiles)
            {
                attachments.Add(new Attachment
                {
                    ID = savedToServer ? 1 : 0,
                    JobOrderID = insertID,
                    Filename = file.FileName
                });                

                await Data.FileAppData.SaveFile(file.DataArray, file.FileName, targetPath);
            }

            MvxApp.Database.SaveAttachments(attachments);            
        }

        public async Task SaveSignature(int jobOrderID)
        {
            var signatureStringArray = _signatureBytes.Split(Constants.SpecialCharacters.CharComma);

            var signatureBytes = signatureStringArray.Select(byte.Parse).ToArray();

            var signatureFilename = string.Concat(jobOrderID, Constants.Common.SignatureNameExtension);

            var signaturePath = string.Concat(jobOrderID, Constants.Uploads.SignatureTargetFolder);

            LocalJobOrder joForUpdate = MvxApp.Database.GetJobOrderAsync(jobOrderID);

            if (joForUpdate != null)
            {
                joForUpdate.ClientSignature = signatureFilename;
                joForUpdate.StatusID = (int)Constants.Status.Signed;

                MvxApp.Database.SaveJobOrderAsync(joForUpdate);
            }

            await Data.FileAppData.SaveFile(signatureBytes, signatureFilename, signaturePath);
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
            catch (Exception ex)
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

        public IMvxCommand GoToBillingTypesPage => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string> { };

            if (!string.IsNullOrEmpty(_serializedBillingTypes))
            {
                param.Add(Constants.Params.BillingTypes, _serializedBillingTypes);                    
            }

            _serializedBillingTypes = await _navigationService.Navigate<BillingTypesViewModel, 
                                                                        Dictionary<string, string>, 
                                                                        string>(param);     
            
        }, allowConcurrentExecutions: true);

        public IMvxCommand ViewBillingTypesPage => new MvxCommand(async () =>
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
        });

        public IMvxCommand DrawSignaturePageCommand => new MvxAsyncCommand(async () =>
        {
            if (!string.IsNullOrEmpty(_signatureBytes))
            {
                var param = new Dictionary<string, string>
                {
                    { Constants.Params.SignatureBytes, _signatureBytes }
                };

                var result = await _navigationService.Navigate<ViewSignatureViewModel, Dictionary<string, string>, string>(param);

                if (result == Constants.Params.SignAgain)
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
        }, allowConcurrentExecutions: true);

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
