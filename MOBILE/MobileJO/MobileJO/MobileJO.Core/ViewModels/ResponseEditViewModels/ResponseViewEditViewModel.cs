using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.Common;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MobileJO.Core.ViewModels.ResponseEditViewModels
{
    public class ResponseViewEditViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;

        private Dictionary<string, string> searchViewModel;
        private ResponseDetailsModel _response;
        public ResponseDetailsModel ResponseDetails
        {
            get => _response;
            set => SetProperty(ref _response, value);
        }

        private string _status;
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private ObservableCollection<QuestionModel> _questions;
        public ObservableCollection<QuestionModel> Questions
        {
            get => _questions;
            set => SetProperty(ref _questions, value);
        }

        private bool _canEditAnswer;
        public bool CanEditAnswer
        {
            get => _canEditAnswer;
            set => SetProperty(ref _canEditAnswer, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private int _maxLimit;
        public int MaxLimit
        {
            get => _maxLimit;
            set => SetProperty(ref _maxLimit, value);
        }

        private List<DropdownViewModel> _company;
        public List<DropdownViewModel> Company
        {
            get => _company;
            set => SetProperty(ref _company, value);
        }

        private DropdownViewModel _selectedCompany;
        public DropdownViewModel SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                SetProperty(ref _selectedCompany, value);
                GetBranches.Execute();
            }
        }

        private bool _canSelectCompany;
        public bool CanSelectCompany
        {
            get => _canSelectCompany;
            set => SetProperty(ref _canSelectCompany, value);
        }

        private List<DropdownViewModel> _branch;
        public List<DropdownViewModel> Branch
        {
            get => _branch;
            set => SetProperty(ref _branch, value);
        }

        private DropdownViewModel _selectedBranch;
        public DropdownViewModel SelectedBranch
        {
            get => _selectedBranch;
            set => SetProperty(ref _selectedBranch, value);
        }

        private bool _canSelectBranch;
        public bool CanSelectBranch
        {
            get => _canSelectBranch;
            set => SetProperty(ref _canSelectBranch, value);
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private MediaFile _uploadedMediaFile;
        public MediaFile UploadedMediaFile
        {
            get => _uploadedMediaFile;
            set => SetProperty(ref _uploadedMediaFile, value);
        }

        private bool _isForcedFillUp;

        public const int ResponseActionSave = 1;
        public const int ResponseActionSubmit = 2;

        public ResponseViewEditViewModel(IMvxNavigationService navigationService, IAppSettings settings,
            IUserDialogs userDialogs, ILocalizeService localizeService, IWebService webService)
            : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _userDialogs = userDialogs;
            _localizeService = localizeService;
            _webService = webService;
        }

        public override void Prepare(Dictionary<string, string> parameter)
        {
            searchViewModel = parameter;
            LoadResponse.Execute();
        }
        public IMvxCommand LoadResponse => new MvxCommand(async () =>
        {
            if (IsBusy) return;

            IsBusy = true;
            int responseID = Convert.ToInt32(searchViewModel[Constants.Params.ResponseID]);
            int templateID = Convert.ToInt32(searchViewModel[Constants.Params.TemplateID]);
            int localResponseID = Convert.ToInt32(searchViewModel[Constants.Params.LocalResponseID]);
            _isForcedFillUp = Convert.ToBoolean(searchViewModel[Constants.Params.IsForcedFillUp]);

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    ResponseDetails = await _webService.GetResponseByResponseID(responseID, templateID);
                    Company = new List<DropdownViewModel>(await _webService.GetCompanies(templateID));
                }
                else
                {
                    ResponseDetails = MvxApp.Database.GetResponseByResponseID(localResponseID, responseID, templateID, Convert.ToInt32(_settings.UserID));
                    Company = MvxApp.Database.GetCompanies(Convert.ToInt32(_settings.UserTypeID), Convert.ToInt32(_settings.CompanyID), templateID);
                }

                await Task.Delay(1000);
                Status = Constants.Messages.StatusNew;

                if ((searchViewModel.ContainsKey(Constants.Params.ResponseID) && Convert.ToInt32(searchViewModel[Constants.Params.ResponseID]) > 0)
                || (searchViewModel.ContainsKey(Constants.Params.LocalResponseID) && Convert.ToInt32(searchViewModel[Constants.Params.LocalResponseID]) > 0))
                {
                    Status = Helpers.GetResponseStatusText(ResponseDetails.Status);
                }

                Questions = new ObservableCollection<QuestionModel>();
                int questionNo = 1;
                foreach (QuestionModel row in ResponseDetails.QuestionList)
                {
                    ChoiceModel sliderProps = row.QuestionTypeID == (int)Constants.QuestionType.Slider ? GetSliderProps(row.Choices) : InitSliderProps();
                    ConstructQuestionList(row, questionNo, sliderProps);
                    questionNo++;
                }

                CanEditAnswer = ResponseDetails.CanEditAnswer;
                Title = ResponseDetails.Title;
                Description = ResponseDetails.Description;
                MaxLimit = ResponseDetails.MaxLimit;

                if (_settings.UserTypeID != Constants.UserType.SuperAdmin.ToString("d"))
                {
                    ResponseDetails.CompanyID = Convert.ToInt32(_settings.CompanyID);
                }
                SelectedCompany = GetSelectedCompanyBranch(Company, ResponseDetails.CompanyID);

                if (_settings.UserTypeID == Constants.UserType.Employee.ToString("d"))
                {
                    ResponseDetails.BranchID = Convert.ToInt32(_settings.BranchID);
                }

                CanSelectCompany = ResponseDetails.UserID == 0 ? true : Convert.ToInt32(_settings.UserID) == ResponseDetails.UserID;
                CanSelectCompany = CanSelectCompany
                    && _settings.UserTypeID == Constants.UserType.SuperAdmin.ToString("d")
                    && Company != null
                    && Company.Count > 0
                    && !ResponseDetails.Status;
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

        public IMvxCommand RefreshList => new MvxCommand(async () =>
        {
            IsRefreshing = true;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    ResponseDetails = new ResponseDetailsModel();
                    LoadResponse.Execute();
                }
                else
                {
                    var noInternetMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                    await UserDialogs.AlertAsync(noInternetMessage, Constants.Modal.Warning, Constants.Common.OK);
                }
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

        public IMvxCommand GetBranches => new MvxCommand(async () =>
        {
            await Task.Delay(1000);

            try
            {

                if (NetworkCheck.HasInternet())
                {
                    Branch = new List<DropdownViewModel>(await _webService.GetBranches(SelectedCompany.Value));
                }
                else
                {
                    Branch = MvxApp.Database.GetBranches(SelectedCompany.Value);
                }

                SelectedBranch = GetSelectedCompanyBranch(Branch, ResponseDetails.BranchID);
                CanSelectBranch = ResponseDetails.UserID == 0 ? true : Convert.ToInt32(_settings.UserID) == ResponseDetails.UserID;
                CanSelectBranch = CanSelectBranch
                    && _settings.UserTypeID != Constants.UserType.Employee.ToString("d")
                    && Branch != null
                    && Branch.Count > 0
                    && !ResponseDetails.Status;
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
        });

        public IMvxCommand UploadMedia => new MvxCommand<int>(async (questionID) =>
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                if (UploadedMediaFile == null) return;

                if (NetworkCheck.HasInternet())
                {
                    DataViewModel dataViewModel = await _webService.UploadMedia(UploadedMediaFile);
                    foreach (QuestionModel question in Questions)
                    {
                        if (question.QuestionID != questionID) continue;

                        var localizedMessage = string.Empty;
                        if (dataViewModel == null)
                        {
                            localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                            await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                            question.Answer.Value = string.Empty;
                            return;
                        }

                        localizedMessage = LocalizeService.Translate(dataViewModel.Message);
                        await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.InfoMessage, Constants.Common.OK);
                        question.Answer.Value = dataViewModel.Data.ToString();
                        break;
                    }
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
            }
        });

        public IMvxCommand SaveSubmitResponse => new MvxCommand<int>(async (action) =>
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                if (SelectedCompany == null || SelectedBranch == null)
                {
                    await _userDialogs.AlertAsync(Constants.Messages.FillUpCompanyBranch,
                        Constants.Modal.Warning,
                        Constants.Common.OK);
                    return;
                }

                if (!IsValidInputs(action))
                {
                    await _userDialogs.AlertAsync(Constants.Messages.InvalidInputsError,
                        Constants.Modal.Warning,
                        Constants.Common.OK);
                    return;
                }

                bool confirmSave = await _userDialogs.ConfirmAsync(Constants.Messages.ConfirmSave,
                    Constants.Modal.Confirmation,
                    Constants.Messages.Yes,
                    Constants.Messages.No);

                if (!confirmSave) return;

                DateTime currentDateTime = DateTime.Now;
                int userID = Convert.ToInt32(_settings.UserID);
                ResponseAnswerModel responseAnswerModel = new ResponseAnswerModel()
                {
                    LocalResponseID = ResponseDetails.LocalResponseID,
                    ResponseID = ResponseDetails.ResponseID,
                    TemplateID = ResponseDetails.TemplateID,
                    UserID = userID,
                    BranchID = SelectedBranch == null ? 0 : SelectedBranch.Value,
                    CompanyID = SelectedCompany == null ? 0 : SelectedCompany.Value,
                    DateSubmitted = currentDateTime,
                    Status = action != ResponseActionSave,
                    CreatedDate = currentDateTime,
                    CreatedBy = userID,
                    UpdatedDate = currentDateTime,
                    UpdatedBy = userID,
                    LastSyncDate = currentDateTime,
                    AnswerList = ConstructAnswerList()
                };

                if (NetworkCheck.HasInternet())
                {
                    ResponseAnswerModel saveResponseModel = await _webService.SaveResponse(responseAnswerModel);
                    if (saveResponseModel != null && saveResponseModel.ResponseID > 0)
                    {
                        CopySaveResponseModel(responseAnswerModel, saveResponseModel);
                        MvxApp.Database.SaveResponse(responseAnswerModel);
                        await _userDialogs.AlertAsync(Constants.Messages.SaveToServerSuccess,
                                                  Constants.Modal.InfoMessage,
                                                  Constants.Common.OK);
                        await _navigationService.Close(this);
                        RedirectToResponseList.Execute();
                    }
                    else
                    {
                        var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                        await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                    }
                }
                else
                {
                    MvxApp.Database.SaveResponse(responseAnswerModel);
                    await _userDialogs.AlertAsync(Constants.Messages.SaveResponseToLocalSuccess,
                                                  Constants.Modal.InfoMessage,
                                                  Constants.Common.OK);
                    await _navigationService.Close(this);
                    RedirectToResponseList.Execute();
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
            }
        });

        public IMvxCommand RedirectToResponseList => new MvxCommand(async () =>
        {
            if (_isForcedFillUp)
            {
                var parameter = new Dictionary<string, string>
                {
                    {
                        Constants.Params.TemplateID, searchViewModel[Constants.Params.TemplateID].ToString()
                    }
                };
                await _navigationService.Navigate<ResponseListViewModel, Dictionary<string, string>>(parameter);
            }
        });

        private ChoiceModel InitSliderProps()
        {
            return new ChoiceModel()
            {
                Maximum = 1,
                Minimum = 0,
                MaxLabel = "",
                MinLabel = ""
            };
        }

        public ChoiceModel GetSliderProps(List<ChoiceModel> choices)
        {
            ChoiceModel sliderProps = new ChoiceModel();
            double minimum = 0;
            double maximum = 0;
            string minLabel = "";
            string maxLabel = "";

            foreach (ChoiceModel choice in choices)
            {
                double value = Convert.ToDouble(choice.Label.ToString());
                if (maximum < value)
                {
                    minimum = maximum;
                    maximum = value;
                    minLabel = maxLabel;
                    maxLabel = choice.Value;
                    continue;
                }

                minimum = value;
                minLabel = choice.Value;

                if (minimum > maximum)
                {
                    minimum = maximum;
                    maximum = value;
                    minLabel = maxLabel;
                    maxLabel = choice.Value;
                }
            }

            sliderProps.Minimum = minimum;
            sliderProps.Maximum = maximum;
            sliderProps.MinLabel = minLabel;
            sliderProps.MaxLabel = maxLabel;

            return sliderProps;
        }

        private void ConstructQuestionList(QuestionModel question, int questionNo, ChoiceModel sliderProps)
        {
            string strValue = question.Answer == null || string.IsNullOrEmpty(question.Answer.Value) ? "" : question.Answer.Value;

            if ((int)Constants.QuestionType.Video == question.QuestionTypeID
                || (int)Constants.QuestionType.Image == question.QuestionTypeID)
            {
                if (question.Answer != null)
                {
                    strValue = string.IsNullOrEmpty(strValue) ?
                        strValue
                        : strValue.Contains(Constants.Uploads.ImagePrefix) ?
                            strValue.Substring(strValue.IndexOf(Constants.Uploads.ImagePrefix))
                            : strValue.Substring(strValue.IndexOf(Constants.Uploads.VideoPrefix));
                }
            }

            Questions.Add(new QuestionModel
            {
                QuestionID = question.QuestionID,
                QuestionTypeID = question.QuestionTypeID,
                QuestionNo = questionNo,
                Question = question.Question,
                Choices = question.Choices,
                SelectedItem = GetSelectedItem(question),
                SliderProps = sliderProps,
                Answer = new AnswerModel
                {
                    AnswerID = question.Answer == null ? 0 : question.Answer.AnswerID,
                    Value = strValue
                }
            });
        }

        private ChoiceModel GetSelectedItem(QuestionModel question)
        {
            ChoiceModel selectedItems = new ChoiceModel();

            foreach (ChoiceModel choice in question.Choices)
            {
                if (choice.IsSelected)
                {
                    selectedItems = choice;
                    break;
                }
            }

            return selectedItems;
        }

        private DropdownViewModel GetSelectedCompanyBranch(List<DropdownViewModel> list, int id)
        {
            foreach (DropdownViewModel data in list)
            {
                if (data.Value == id)
                {
                    return data;
                }
            }

            return null;
        }

        private bool IsValidInputs(int action)
        {
            if (action == ResponseActionSave) return true;

            foreach (QuestionModel question in Questions)
            {
                if (question.QuestionTypeID == (int)Constants.QuestionType.Checkbox)
                {
                    int count = 0;
                    foreach (ChoiceModel choice in question.Choices)
                    {
                        if (choice.IsSelected == true)
                        {
                            count++;
                            break;
                        }
                    }

                    if (count == 0)
                    {
                        return false;
                    }

                    continue;
                }

                if (question.QuestionTypeID == (int)Constants.QuestionType.Radio)
                {
                    if (question.SelectedItem == null)
                        return false;

                    continue;
                }

                if (question.Answer == null
                    || string.IsNullOrEmpty(question.Answer.Value))
                    return false;
            }

            return true;
        }

        private List<AnswerModel> ConstructAnswerList()
        {
            // construct response data for each question
            List<AnswerModel> answerModel = new List<AnswerModel>();

            foreach (QuestionModel question in Questions)
            {
                if ((question.QuestionTypeID == (int)Constants.QuestionType.Text
                    || question.QuestionTypeID == (int)Constants.QuestionType.Slider)
                    && question.Answer != null)
                {
                    answerModel.Add(ConstructAnswer(question.Answer.AnswerID, question.QuestionID, question.QuestionTypeID, 0, question.Answer.Value));
                    continue;
                }

                if (question.QuestionTypeID == (int)Constants.QuestionType.Checkbox)
                {
                    foreach (ChoiceModel choice in question.Choices)
                    {
                        if (choice.IsSelected)
                        {
                            answerModel.Add(ConstructAnswer(choice.AnswerID, question.QuestionID, question.QuestionTypeID, choice.ChoiceID, null));
                        }
                    }

                    continue;
                }
                if (question.QuestionTypeID == (int)Constants.QuestionType.Radio
                    && question.SelectedItem != null)
                {
                    foreach (ChoiceModel choice in question.Choices)
                    {
                        if (choice.IsSelected)
                        {
                            answerModel.Add(ConstructAnswer(question.Answer.AnswerID, question.QuestionID, question.QuestionTypeID, question.SelectedItem.ChoiceID, null));
                            break;
                        }
                    }
                    
                    continue;
                }

                if ((question.QuestionTypeID == (int)Constants.QuestionType.Video
                    || question.QuestionTypeID == (int)Constants.QuestionType.Image)
                    && question.Answer != null)
                {
                    answerModel.Add(ConstructAnswer(question.Answer.AnswerID, question.QuestionID, question.QuestionTypeID, 0, question.Answer.Value));
                }
            }

            return answerModel;
        }

        private AnswerModel ConstructAnswer(int answerID, int questionID, int questionTypeID, int choiceID, string value)
        {
            return new AnswerModel()
            {
                AnswerID = answerID,
                ResponseID = ResponseDetails.ResponseID,
                TemplateID = ResponseDetails.TemplateID,
                QuestionID = questionID,
                QuestionTypeID = questionTypeID,
                ChoiceID = choiceID,
                UserID = Convert.ToInt32(_settings.UserID),
                Value = value
            };
        }

        private void CopySaveResponseModel(ResponseAnswerModel responseAnswerModel, ResponseAnswerModel saveResponseModel)
        {
            responseAnswerModel.ResponseID = saveResponseModel.ResponseID;

            List<AnswerModel> saveAnswerList = saveResponseModel.AnswerList;
            List<AnswerModel> answerList = responseAnswerModel.AnswerList;

            foreach (AnswerModel answer in answerList)
            {
                foreach (AnswerModel saveAnswer in saveAnswerList)
                {
                    if (answer.QuestionID == saveAnswer.QuestionID)
                    {
                        answer.AnswerID = saveAnswer.AnswerID;
                        break;
                    }
                }
            }

            responseAnswerModel.AnswerList = answerList;
        }
    }
}
