using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.Common;
using MobileJO.Core.ViewModels.FieldViewModels;
using MobileJO.Core.ViewModels.Login;
using MobileJO.Core.ViewModels.QuestionnaireListViewModels;
using MobileJO.Core.ViewModels.SettingsViewModels;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.Media.Abstractions;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MobileJO.Core.ViewModels
{
    public class QuestionnaireListViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;

        public ObservableCollection<QuestionnaireModel> _questionnaire { get; set; } = new ObservableCollection<QuestionnaireModel>();
        private Dictionary<string, string> searchViewModel;
        private QuestionnaireSearchViewModel OfflineSearchParams { get; set; }
        private int _totalPages;
        private int _totalRecords;
        private int _currentPage = 1;

        private int templateID;

        private QuestionnaireModel _selectedQuestionnaire;
        public QuestionnaireModel SelectedQuestionnaire
        {
            get => _selectedQuestionnaire;
            set
            {
                SetProperty(ref _selectedQuestionnaire, value);
                templateID = _selectedQuestionnaire.ID;
                GoToResponseList.Execute();
                SetProperty(ref _selectedQuestionnaire, null);
            }
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

        private bool _hasRecords;
        public bool HasRecords
        {
            get => _hasRecords;
            set => SetProperty(ref _hasRecords, value);
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private static bool _isInitialLoad = true;

        public QuestionnaireListViewModel(IMvxNavigationService navigationService, IAppSettings settings,
            IUserDialogs userDialogs, ILocalizeService localizeService, IWebService webService)
            : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _userDialogs = userDialogs;
            _localizeService = localizeService;
            _webService = webService;
        }

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
                    responseForSyncList = await GetUnsyncedResponseAndAnswerData();

                    if (responseForSyncList.Count > 0)
                    {
                        await SyncResponseAndAnswer(responseForSyncList);
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
                bool isSynced = await LoadUserData();

                if (isSynced)
                {
                    await _userDialogs.AlertAsync(Constants.Messages.SyncSuccess,
                                                  Constants.Modal.InfoMessage,
                                                  Constants.Common.OK);
                }

                IsBusy = false;
            }
        });

        public IMvxCommand GoToSettings => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            await _navigationService.Navigate<SettingsViewModel>();
        });

        public IMvxCommand GoToSearchQuestionnairePageCommand => new MvxCommand(async () =>
        {
            if (IsBusy) return;
            await _navigationService.Navigate<SearchQuestionnaireViewModel>();
        });

        public IMvxCommand Logout => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            bool confirmLogout = await _userDialogs.ConfirmAsync(Constants.Messages.ConfirmLogout,
                                                                 Constants.Modal.Confirmation,
                                                                 Constants.Messages.Yes,
                                                                 Constants.Messages.No);

            if (confirmLogout)
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
                                int endIndex = answer.Value.IndexOf(Constants.CheckpointAnswers.ImagePrefix) > 0 ?
                                    answer.Value.IndexOf(Constants.CheckpointAnswers.ImagePrefix)
                                    : answer.Value.IndexOf(Constants.CheckpointAnswers.VideoPrefix) > 0 ?
                                        answer.Value.IndexOf(Constants.CheckpointAnswers.VideoPrefix)
                                        : 0;
                                string filePath = answer.Value.Substring(0, endIndex);
                                string fileName = "temp" + answer.Value.Substring(endIndex);
                                if (File.Exists(filePath + fileName))
                                {
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
                            }

                            answerViewList.Add(new AnswerDetailsViewModel
                            {
                                LocalAnswerID = answer.ID,
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
                            LocalResponseID = response.ID,
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

        public override void Prepare(Dictionary<string, string> parameter)
        {
            searchViewModel = parameter;

            if (searchViewModel != null)
            {
                try
                {
                    if (!searchViewModel.ContainsKey(Constants.Params.Title))
                    {
                        searchViewModel.Add(Constants.Params.Title, Constants.SpecialCharacters.EmptyString);
                    }
                    if (!searchViewModel.ContainsKey(Constants.Params.Category))
                    {
                        searchViewModel.Add(Constants.Params.Category, Constants.SpecialCharacters.EmptyString);
                    }
                    if (!searchViewModel.ContainsKey(Constants.Params.CompanyID))
                    {
                        searchViewModel.Add(Constants.Params.CompanyID, Constants.SpecialCharacters.EmptyString);
                    }
                    if (!searchViewModel.ContainsKey(Constants.Params.BranchID))
                    {
                        searchViewModel.Add(Constants.Params.BranchID, Constants.SpecialCharacters.EmptyString);
                    }
                    if(searchViewModel.ContainsKey(Constants.Params.FromLogin))
                    {
                        _isInitialLoad = true;
                    }
                }
                catch (Exception ex)
                {
                    var test = ex;
                }

            }

            searchViewModel.Add(Constants.Params.IsMobile, true.ToString());
            searchViewModel.Add(Constants.Params.Page, _currentPage.ToString());
            searchViewModel.Add(Constants.Params.PageSize, Constants.Common.PageValue.ToString());

            SetOfflineSearchParams();
            LoadList.Execute();
        }

        public IMvxCommand LoadList => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (_isInitialLoad)
                {
                    var responseForSyncList = new List<ResponseAnswerDetailsViewModel>();
                    responseForSyncList = await GetUnsyncedResponseAndAnswerData();

                    if (responseForSyncList.Count > 0)
                    {
                        await SyncResponseAndAnswer(responseForSyncList);
                    }

                    if (await LoadUserData())
                    {
                        _isInitialLoad = false;
                    }
                }

                QuestionnairePaginationViewModel result = null;
                if (NetworkCheck.HasInternet())
                {
                    result = await _webService.GetQuestionnaireList(searchViewModel);
                }
                else
                {
                    result = MvxApp.Database.GetQuestionnaireList(OfflineSearchParams);
                }


                _totalPages = result.Pagination.Pages;
                _totalRecords = result.Pagination.Size;

                ShowError = true;
                if (_totalRecords > 0)
                {
                    ShowError = false;
                    foreach (QuestionnaireModel row in result.Data)
                    {
                        _questionnaire.Add(new QuestionnaireModel
                        {
                            ID = row.ID,
                            Title = row.Title,
                            Description = row.Description,
                            Respondents = row.Respondents,
                            CompanyBranchNames = row.CompanyBranchNames,
                            CreatedDate = row.CreatedDate

                        });
                    }

                    CanLoadMoreData = false;
                    if (_totalRecords > Constants.Common.PageValue && _currentPage < _totalPages)
                    {
                        CanLoadMoreData = true;
                    }
                }

                HasRecords = true;
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                await _navigationService.Close(this);
            }
            finally
            {
                IsBusy = false;
            }
        });

        public IMvxCommand LoadMore => new MvxCommand(() =>
        {
            if (IsBusy) return;

            if (_currentPage < _totalPages) _currentPage++;

            searchViewModel[Constants.Params.Page] = _currentPage.ToString();

            SetOfflineSearchParams();
            LoadList.Execute();
        });

        public IMvxCommand RefreshList => new MvxCommand(async () =>
        {
            IsRefreshing = true;

            try
            {
                _questionnaire = new ObservableCollection<QuestionnaireModel>();
                _currentPage = 1;

                var param = new Dictionary<string, string>();
                searchViewModel = param;

                searchViewModel.Add(Constants.Params.Title, Constants.SpecialCharacters.EmptyString);
                searchViewModel.Add(Constants.Params.Category, Constants.SpecialCharacters.EmptyString);
                searchViewModel.Add(Constants.Params.CompanyID, Constants.SpecialCharacters.EmptyString);
                searchViewModel.Add(Constants.Params.BranchID, Constants.SpecialCharacters.EmptyString);
                searchViewModel.Add(Constants.Params.IsMobile, true.ToString());
                searchViewModel.Add(Constants.Params.Page, _currentPage.ToString());
                searchViewModel.Add(Constants.Params.PageSize, Constants.Common.PageValue.ToString());
                SetOfflineSearchParams();
                LoadList.Execute();
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
            finally
            {
                IsRefreshing = false;
            }
        });

        public IMvxCommand GoToResponseList => new MvxCommand(async () =>
        {
            if (IsBusy) return;

            await Task.Delay(1000);

            try
            {

                var parameter = new Dictionary<string, string>
                {
                    {
                        Constants.Params.TemplateID, templateID.ToString()
                    }
                };
                await _navigationService.Navigate<ResponseListViewModel, Dictionary<string, string>>(parameter);
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                await _navigationService.Close(this);
            }
        });

        private void SetOfflineSearchParams()
        {
            OfflineSearchParams = new QuestionnaireSearchViewModel
            {

                Title = searchViewModel.ContainsKey(Constants.Params.Title) ? searchViewModel[Constants.Params.Title] : string.Empty,
                Category = searchViewModel.ContainsKey(Constants.Params.Category) ? searchViewModel[Constants.Params.Category] : string.Empty,
                CompanyID = searchViewModel.ContainsKey(Constants.Params.CompanyID) ? searchViewModel[Constants.Params.CompanyID] : string.Empty,
                BranchID = searchViewModel.ContainsKey(Constants.Params.BranchID) ? searchViewModel[Constants.Params.BranchID] : string.Empty,
                UserTypeID = _settings.UserTypeID,
                IsMobile = true.ToString(),
                Page = int.Parse(searchViewModel[Constants.Params.Page]),
                PageSize = int.Parse(searchViewModel[Constants.Params.PageSize])
            };
        }

        private async Task SyncResponseAndAnswer(List<ResponseAnswerDetailsViewModel> reponseForSync)
        {
            try
            {
                foreach (var response in reponseForSync)
                {
                    ResponseAnswerDetailsViewModel updated = await _webService.SyncResponseAndAnswerDetails(response);
                    MvxApp.Database.UpdateResponseAnswerIDs(updated);
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

                            var templateList = new List<QuestionnaireViewModel>();
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
                                    respo.Status = res.Status;
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

                            //Add templates data
                            MvxApp.Database.SaveTemplateAsync(templates);
                            MvxApp.Database.SaveQuestionsAsync(questions);
                            MvxApp.Database.SaveChoicesAsync(choices);

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
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
            finally
            {
                if (servicesCompleted)
                    IsBusy = false;

            }
            return servicesCompleted;
        }
    }
}
