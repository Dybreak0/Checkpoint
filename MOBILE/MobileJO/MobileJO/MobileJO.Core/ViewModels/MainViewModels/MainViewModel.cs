using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.AssignedCases;
using MobileJO.Core.ViewModels.Common;
using MobileJO.Core.ViewModels.FieldViewModels;
using MobileJO.Core.ViewModels.Login;
using MobileJO.Core.ViewModels.SettingsViewModels;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.Media.Abstractions;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MobileJO.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private Dictionary<string, string> _parameter;

        public MainViewModel(IMvxNavigationService navigationService, IAppSettings settings,
            IUserDialogs userDialogs, ILocalizeService localizeService, IWebService webService)
            : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _webService = webService;
            _userDialogs = userDialogs;
            
        }       

        public override async void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;
            if (_parameter != null && _parameter.ContainsKey(Constants.Params.UserName))
            {
                UserName = _parameter[Constants.Params.UserName];

                var unsyncedData = await GetUnsyncedData();
                var unsyncedResponseAndAnswerData = await GetUnsyncedResponseAndAnswerData();

                if (unsyncedData.Count > 0)
                {
                    IsBusy = true;
                    await SyncRecords(unsyncedData);
                    IsBusy = false;
                }

                if (unsyncedResponseAndAnswerData.Count > 0)
                {
                    IsBusy = true;
                    await SyncResponseAndAnswer(unsyncedResponseAndAnswerData);
                    IsBusy = false;
                }


                await LoadUserData();
            }            
        }

        public IMvxAsyncCommand CallWebService => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                // calling to web service

                //Simulating Web Service Call
                await Task.Delay(5000);
                var localizedMessage = LocalizeService.Translate(Constants.Messages.SuccessGet);
                await UserDialogs.AlertAsync(localizedMessage);
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
            finally
            {
                IsBusy = false;
            }
        });        

        public IMvxCommand Logout => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            bool confirmLogout = await _userDialogs.ConfirmAsync(Constants.Messages.ConfirmLogout, 
                                                                 Constants.Modal.Confirmation, 
                                                                 Constants.Messages.Yes,
                                                                 Constants.Messages.No);

            if(confirmLogout)
            {
                _settings.AccessToken = string.Empty;
                _settings.RefreshToken = string.Empty;
                _settings.UserID = string.Empty;
                _settings.UserName = string.Empty;
                CrossSecureStorage.Current.DeleteKey(Constants.AppSettings.AccessToken);
                CrossSecureStorage.Current.DeleteKey(Constants.AppSettings.UserID);
                CrossSecureStorage.Current.DeleteKey(Constants.AppSettings.UserName);
                await _navigationService.Navigate<LoginViewModel>();
            }            
        });

        public IMvxCommand GoToSettings => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            await _navigationService.Navigate<SettingsViewModel>();
        });

        public string ButtonText { get; set; }
        public string UserName { get; set; }

        public string PersistentText
        {
            get => _settings.PersistentText;
            set => _settings.PersistentText = value;
        }

        public IMvxCommand GoToJobOrderList => new MvxCommand(async () =>
        {
            var param = new Dictionary<string, string>();
            await _navigationService.Navigate<JobOrderListViewModel, Dictionary<string, string>>(param);
        });

        public IMvxCommand GoToAssignedCasesList => new MvxCommand(async () =>
        {
            var param = new Dictionary<string, string>
            {
                { Constants.Params.AssignedTo, CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID) }
            };

            await _navigationService.Navigate<AssignedCasesListViewModel, Dictionary<string, string>>(param);
        });

        public IMvxCommand SyncCommand => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var joForSyncList = new List<JobOrderDetailsViewModel>();
            var responseForSyncList = new List<ResponseAnswerDetailsViewModel>();

            try
            {                
                if (NetworkCheck.HasInternet())
                {
                    joForSyncList = await GetUnsyncedData();
                    responseForSyncList = await GetUnsyncedResponseAndAnswerData();

                    if (joForSyncList.Count > 0)
                    {
                        await SyncRecords(joForSyncList);
                    }
                    if (responseForSyncList.Count > 0)
                    {
                        await SyncResponseAndAnswer(responseForSyncList);
                    }
                    else
                    {
                        await _userDialogs.AlertAsync(Constants.Messages.RecordsUpToDate, Constants.Modal.InfoMessage, Constants.Common.OK);
                    }
                }
                else
                {
                    await _userDialogs.AlertAsync(Constants.Messages.NoInternet, Constants.Modal.Warning, Constants.Common.OK);
                }
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
            finally
            {
                IsBusy = false;

                if (joForSyncList.Count > 0 || responseForSyncList.Count > 0)
                {
                    bool isSynced = await LoadUserData();

                    if(isSynced)
                    {
                        await _userDialogs.AlertAsync(Constants.Messages.SyncSuccess, 
                                                      Constants.Modal.InfoMessage,
                                                      Constants.Common.OK);
                    }
                  
                }                               
            }
        });

        public async Task<List<ResponseAnswerDetailsViewModel>> GetUnsyncedResponseAndAnswerData()
        {
            var forSyncResponseAnswerList = new List<ResponseAnswerDetailsViewModel>();

            var responseList = new List<Response>();
            var answerList = new List<Answer>();

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    responseList = MvxApp.Database.GetResponsesAsync();

                    var answerViewList = new List<AnswerDetailsViewModel>();

                    foreach (var response in responseList)
                    {
                        if (response.ResponseID > 0 && response.UpdatedDate <= response.LastSyncDate)
                            continue;

                        answerList = MvxApp.Database.GetAnswersByResponseID(response.ID);

                        foreach (var answer in answerList)
                        {
                            if (answer.AnswerID > 0 && answer.UpdatedDate <= answer.LastSyncDate)
                                continue;

                            byte[] uploadedFile = null;
                            if (answer != null
                                && !string.IsNullOrEmpty(answer.Value)
                                && (answer.Value.Contains(Constants.CheckpointAnswers.ImagePrefix)
                                    || answer.Value.Contains(Constants.CheckpointAnswers.VideoPrefix)))
                            {
                                string filePath = answer.Value.Substring(0, answer.Value.IndexOf(Constants.CheckpointAnswers.ImagePrefix));
                                string fileName = "temp" + answer.Value.Substring(answer.Value.IndexOf(Constants.CheckpointAnswers.ImagePrefix));
                                MediaFile file = new MediaFile(filePath + fileName, () =>
                                {
                                    return new MemoryStream(File.ReadAllBytes(answer.Value));
                                });

                                DataViewModel dataViewModel = await _webService.UploadMedia(file);
                                if (dataViewModel != null && dataViewModel.Data != null)
                                {
                                    answer.Value = dataViewModel.Data.ToString();
                                }
                            }

                            answerViewList.Add(new AnswerDetailsViewModel
                            {
                                AnswerID = answer.AnswerID,
                                ResponseID = answer.ResponseID,
                                TemplateID = answer.TemplateID,
                                QuestionID = answer.QuestionID,
                                ChoiceID = answer.ChoiceID,
                                UserID = answer.UserID,
                                Value = answer.Value,
                                UploadedFile = uploadedFile,
                                DateSubmitted = answer.DateSubmitted,
                                CreatedDate = answer.CreatedDate,
                                CreatedBy = answer.CreatedBy,
                                UpdatedDate = answer.UpdatedDate,
                                UpdatedBy = answer.UpdatedBy
                            });
                        }

                        forSyncResponseAnswerList.Add(new ResponseAnswerDetailsViewModel
                        {
                            ResponseID = response.ResponseID,
                            TemplateID = response.TemplateID,
                            UserID = response.UserID,
                            BranchID = response.BranchID,
                            CompanyID = response.CompanyID,
                            DateSubmitted = response.DateSubmitted,
                            Remarks = response.Remarks,
                            Status = response.Status,
                            CreatedDate = response.CreatedDate,
                            CreatedBy = response.CreatedBy,
                            UpdatedDate = response.UpdatedDate,
                            UpdatedBy = response.UpdatedBy,
                            Answers = answerViewList
                        });

                        answerViewList = new List<AnswerDetailsViewModel>();
                        answerList = new List<Answer>();
                    }

                }
                else
                {
                    await _userDialogs.AlertAsync(Constants.Messages.NoInternet, Constants.Modal.Warning, Constants.Common.OK);
                }
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }

            return forSyncResponseAnswerList;
        }

        public IMvxCommand GoToQuestionnaireList => new MvxCommand(async () =>
        {
            var param = new Dictionary<string, string>();
            await _navigationService.Navigate<QuestionnaireListViewModel, Dictionary<string, string>>(param);
        });

        private async Task<List<JobOrderDetailsViewModel>> GetUnsyncedData()
        {
            var joForSyncList = new List<JobOrderDetailsViewModel>();

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    var localJobOrders = MvxApp.Database.GetJobOrdersAsync();

                    foreach (LocalJobOrder joRecord in localJobOrders)
                    {
                        if (joRecord.JobOrderNumber == null || joRecord.UpdatedDate > joRecord.LastSyncDate)
                        {
                            int id = joRecord.ServerID > 0
                                   ? joRecord.ServerID
                                   : joRecord.ID;

                            var joCases = new List<TaggedCase>();
                            var joBillingTypes = new List<JobOrderBillingType>();
                            var joAttachmentNames = new List<Attachment>();

                            if (joRecord.ServerID > 0)
                            {
                                joCases = MvxApp.Database.GetTaggedCases(joRecord.ServerID);
                                joBillingTypes = MvxApp.Database.GetJOBillingTypes(joRecord.ServerID);
                                joAttachmentNames = MvxApp.Database.GetJOAttachments(joRecord.ServerID);
                            }
                            else
                            {
                                joCases = MvxApp.Database.GetLocalTaggedCases(joRecord.ID);
                                joBillingTypes = MvxApp.Database.GetLocalJOBillingTypes(joRecord.ID);
                                joAttachmentNames = MvxApp.Database.GetLocalJOAttachments(joRecord.ID);
                            }

                            var joAttachments = new List<FileViewModel>();
                            var attachmentPath = string.Concat(id, Constants.Uploads.AttachmentsTargetFolder);
                            var joClientSignature = new FileViewModel();
                            var removedFiles = new List<string>();

                            if (joRecord.ServerID > 0)
                            {
                                var dbAttachments = await _webService.AttachmentList(joRecord.ServerID);

                                var serverAttachments = dbAttachments.Select(x => x.Filename).ToList();

                                var localAttachments = joAttachmentNames.Select(x => x.Filename).ToList();

                                foreach (var serverAttachment in serverAttachments)
                                {
                                    if (!localAttachments.Contains(serverAttachment))
                                    {
                                        removedFiles.Add(serverAttachment);
                                    }
                                }

                            }

                            foreach (Attachment joAttachment in joAttachmentNames)
                            {
                                bool isAttachmentFolderExists = await Data.FileAppData.IsFolderExistAsync(attachmentPath);

                                if (isAttachmentFolderExists)
                                {
                                    string filename = joAttachment.Filename;

                                    bool isAttachmentFileExists = await Data.FileAppData.IsFileExistAsync(filename, attachmentPath);

                                    if (isAttachmentFileExists)
                                    {
                                        byte[] fileBytes = await Data.FileAppData.LoadFile(filename, attachmentPath);

                                        joAttachments.Add(new FileViewModel
                                        {
                                            FileName = joAttachment.Filename,
                                            FileDataArray = fileBytes
                                        });
                                    }
                                }

                            }

                            if (joRecord.ClientSignature != null)
                            {
                                var signaturePath = string.Concat(id, Constants.Uploads.SignatureTargetFolder);

                                var isSignatureFolderExists = await Data.FileAppData.IsFolderExistAsync(signaturePath);

                                if (isSignatureFolderExists)
                                {
                                    bool isSignatureFileExists = await Data.FileAppData.IsFileExistAsync(joRecord.ClientSignature, signaturePath);

                                    if (isSignatureFileExists)
                                    {
                                        joClientSignature = new FileViewModel
                                        {
                                            FileName = joRecord.ClientSignature,
                                            FileDataArray = await Data.FileAppData.LoadFile(joRecord.ClientSignature, signaturePath)
                                        };
                                    }
                                }
                            }

                            joForSyncList.Add(new JobOrderDetailsViewModel
                            {
                                ID = joRecord.ServerID,
                                JobOrderNumber = joRecord.JobOrderNumber,
                                AccountID = joRecord.AccountID,
                                ApplicationTypeID = joRecord.ApplicationTypeID,
                                Branch = joRecord.Branch,
                                JobOrderSubject = joRecord.JobOrderSubject,
                                DateTimeStart = joRecord.DateTimeStart,
                                DateTimeEnd = joRecord.DateTimeEnd,
                                ActivityDetails = joRecord.ActivityDetails,
                                RootCauseAnalysis = joRecord.RootCauseAnalysis,
                                NextStep = joRecord.NextStep,
                                PreventiveAction = joRecord.PreventiveAction,
                                Remarks = joRecord.Remarks,
                                Attendees = joRecord.Attendees,
                                IsBilled = joRecord.IsBilled,
                                IsCollaterals = joRecord.IsCollaterals,
                                ClientSignature = joRecord.ClientSignature,
                                IsFixed = joRecord.IsFixed = joRecord.IsFixed,
                                IsSatisfied = joRecord.IsSatisfied,
                                ClientRating = joRecord.ClientRating,
                                StatusID = joRecord.StatusID,
                                CreatedDate = joRecord.CreatedDate,
                                CreatedBy = joRecord.CreatedBy,
                                UpdatedDate = joRecord.UpdatedDate,
                                UpdatedBy = int.Parse(CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID)),
                                NewJOCases = joCases.Select(i => i.CaseID).ToList(),
                                NewJOBillingTypes = joBillingTypes.Select(i => i.BillingTypeID).ToList(),
                                RemovedAttachments = removedFiles,
                                JobOrderAttachments = joAttachments,
                                Signature = joClientSignature,
                                IsDeleted = joRecord.IsDeleted
                            });
                        }
                    }
                }
                else
                {
                    await _userDialogs.AlertAsync(Constants.Messages.NoInternet, Constants.Modal.Warning, Constants.Common.OK);
                }
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }

            return joForSyncList;
        }

        private async Task SyncResponseAndAnswer(List<ResponseAnswerDetailsViewModel> reponseForSync)
        {
            try
            {
                foreach (var response in reponseForSync)
                {
                    await _webService.SyncResponseAndAnswerDetails(response);
                }
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
        }

        private async Task SyncRecords(List<JobOrderDetailsViewModel> joForSync)
        {
            try
            {
                foreach (var jobOrder in joForSync)
                {
                    await _webService.SyncJobOrderDetails(jobOrder);
                }

            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
        }

        private async Task<bool> LoadUserData()
        {
            if (IsBusy)
                return false;

            IsBusy = true;
            var servicesCompleted = false;

            try
            {                
                if (NetworkCheck.HasInternet())
                {
                    await Task.Run(async () =>
                    {
                        try
                        {
                            // Download data here, make sure you're
                            // not doing anything that has to be done
                            // on the UI Thread
                            int userID = int.Parse(CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID));

                            var joTaggedCases = new List<TaggedCase>();
                            var joAttachments = new List<AttachmentModel>();
                            var joBillingTypes = new List<JobOrderBillingType>();
                            var templateList = new List<QuestionnaireViewModel>();

                            var serverAccountList = new List<Account>(await _webService.AccountList());
                            var serverApplicationTypeList = new List<ApplicationType>(await _webService.ApplicationTypeList());
                            var serverBillingTypes = new List<BillingTypes>(await _webService.BillingTypeList());
                            var jobOrderStatuses = new List<JobOrderStatus>(await _webService.JOStatusList());

                            var jobOrdersToSave = await _webService.GetAllUserJobOrders(userID);

                            var joIDs = jobOrdersToSave.Select(x => x.ID).ToList();

                            var csvIDs = string.Join(Constants.SpecialCharacters.Comma, joIDs);

                            joTaggedCases = await _webService.JOCaseList(csvIDs);
                            joBillingTypes = await _webService.JOBillingTypeList(csvIDs);
                            joAttachments = await _webService.JOAttachmentList(csvIDs);
                            templateList = await _webService.GetAllTemplates();

                            var templates = new List<Template>();
                            var questions = new List<Question>();
                            var choices = new List<Choice>();

                            var response = new List<ResponseAnswerDetailsViewModel>();

                            foreach (var template in templateList)
                            {

                                templates.Add(new Template
                                {
                                    TemplateID = template.TemplateID,
                                    Title = template.Title,
                                    Description = template.Description,
                                    Category = template.Category,
                                    StartDate = template.StartDate,
                                    EndDate = template.EndDate,
                                    MaxLimit = template.MaxLimit,
                                    IsDeleted = template.IsDeleted,
                                    CreatedDate = template.CreatedDate,
                                    CreatedBy = template.CreatedBy,
                                    UpdatedDate = template.UpdatedDate,
                                    UpdatedBy = template.UpdatedBy
                                });

                                response = await _webService.GetAllResponseByTemplateID(template.TemplateID);

                                foreach (var res in response)
                                {

                                    Response respo = new Response();

                                    respo.ResponseID = res.ResponseID;
                                    respo.TemplateID = res.TemplateID;
                                    respo.UserID = res.UserID;
                                    respo.CompanyID = res.CompanyID;
                                    respo.BranchID = res.BranchID;
                                    respo.DateSubmitted = res.DateSubmitted;
                                    respo.Remarks = res.Remarks;
                                    respo.CreatedDate = res.CreatedDate;
                                    respo.CreatedBy = res.CreatedBy;
                                    respo.UpdatedDate = res.UpdatedDate;
                                    respo.UpdatedBy = res.UpdatedBy;
                                    respo.LastSyncDate = res.LastSyncDate;


                                    int localResponseID = MvxApp.Database.SaveResponseAsync(respo);

                                    foreach (var ans in res.Answers)
                                    {
                                        Answer a = new Answer();

                                        a.LocalResponseID = localResponseID;
                                        a.AnswerID = ans.AnswerID;
                                        a.ResponseID = ans.ResponseID;
                                        a.TemplateID = ans.TemplateID;
                                        a.QuestionID = ans.QuestionID;
                                        a.ChoiceID = ans.ChoiceID;
                                        a.UserID = ans.UserID;
                                        a.Value = ans.Value;
                                        a.DateSubmitted = ans.DateSubmitted;
                                        a.CreatedDate = ans.CreatedDate;
                                        a.CreatedBy = ans.CreatedBy;
                                        a.UpdatedDate = ans.UpdatedDate;
                                        a.UpdatedBy = ans.UpdatedBy;
                                        a.LastSyncDate = ans.LastSyncDate;

                                        MvxApp.Database.SaveAnswerAsync(a, res.CompanyID, res.BranchID);

                                    }

                                }

                                foreach (var companyBranch in template.Company_Branches)
                                {
                                    MvxApp.Database.SaveCompanyBranch(companyBranch);
                                }

                                foreach (var templateBranch in template.Template_Branches)
                                {
                                    MvxApp.Database.SaveTemplateBranch(templateBranch);
                                }



                                foreach (var question in template.Questions)
                                {
                                    questions.Add(new Question
                                    {
                                        QuestionID = question.QuestionID,
                                        TemplateID = question.TemplateID,
                                        Qquestion = question.Question,
                                        QuestionTypeID = question.QuestionTypeID,
                                        IsDeleted = question.IsDeleted,
                                        CreatedDate = question.CreatedDate,
                                        CreatedBy = question.CreatedBy,
                                        UpdatedDate = question.UpdatedDate,
                                        UpdatedBy = question.UpdatedBy
                                    });

                                    foreach (var choice in question.Choices)
                                    {
                                        choices.Add(new Choice
                                        {
                                            ChoiceID = choice.ChoiceID,
                                            QuestionID = choice.QuestionID,
                                            Label = choice.Label,
                                            Value = choice.Value,
                                            IsDeleted = choice.IsDeleted,
                                            CreatedDate = choice.CreatedDate,
                                            CreatedBy = choice.CreatedBy,
                                            UpdatedDate = choice.UpdatedDate,
                                            UpdatedBy = choice.UpdatedBy
                                        });
                                    }
                                }

                            }

                            var joAttachmentsModel = new List<Attachment>();

                            foreach (var x in joAttachments)
                            {
                                joAttachmentsModel.Add(new Attachment
                                {
                                    ID = x.ID,
                                    JobOrderID = x.JobOrderID,
                                    Filename = x.Filename
                                });
                            }

                             //Add templates data
                            MvxApp.Database.SaveTemplateAsync(templates);
                            MvxApp.Database.SaveQuestionsAsync(questions);
                            MvxApp.Database.SaveChoicesAsync(choices);

                            MvxApp.Database.DeleteJobOrderDetails();

                            MvxApp.Database.SaveJobOrdersAsync(jobOrdersToSave);
                            MvxApp.Database.SaveJobOrderDetails(joTaggedCases, joBillingTypes, joAttachmentsModel);
                            MvxApp.Database.SaveAccountsAsync(serverAccountList);
                            MvxApp.Database.SaveApplicationTypesAsync(serverApplicationTypeList);
                            MvxApp.Database.SaveBillingTypesAsync(serverBillingTypes);
                            MvxApp.Database.SaveStatusAsync(jobOrderStatuses);

                            var serverCases = await _webService.UserAssignedCasesList(int.Parse(CrossSecureStorage.Current.GetValue(Constants.AppSettings.UserID)));
                            var statuses = await _webService.GetCaseStatus();
                            var caseStatus = new List<CaseStatus>();

                            foreach (var s in statuses)
                            {
                                caseStatus.Add(new CaseStatus
                                {
                                    StatusName = s
                                });
                            }

                            MvxApp.Database.SaveAssignedCasesAsync(serverCases);
                            MvxApp.Database.SaveCaseStatus(caseStatus);

                            servicesCompleted = true;
                        }
                        catch (Exception)
                        {
                            var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                            await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                        }
                    });

                }                
            }
            catch(Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
            finally
            {
                if(servicesCompleted)
                    IsBusy = false;
                
            }
            return servicesCompleted;
        }

    }
}
