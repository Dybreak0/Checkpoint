using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.ResponseEditViewModels;
using MobileJO.Core.ViewModels.ResponseListViewModels;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MobileJO.Core.ViewModels
{
    public class ResponseListViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;

        private Dictionary<string, string> searchViewModel;
        private int _currentPage = 1;

        public ObservableCollection<ResponseModel> _response { get; set; } = new ObservableCollection<ResponseModel>();
        private ResponseSearchViewModel OfflineSearchParams { get; set; }
        private int templateID;
        private int responseID;
        private int localResponseID;

        private bool _showError;
        public bool ShowError
        {
            get => _showError;
            set => SetProperty(ref _showError, value);
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

        private string _fillUpBtnText;
        public string FillUpBtnText
        {
            get => _fillUpBtnText;
            set => SetProperty(ref _fillUpBtnText, value);
        }

        private bool _canFillUpQuestionnaire;
        public bool CanFillUpQuestionnaire
        {
            get => _canFillUpQuestionnaire;
            set => SetProperty(ref _canFillUpQuestionnaire, value);
        }

        private ResponseModel _selectedResponse;
        public ResponseModel SelectedResponse
        {
            get => _selectedResponse;
            set
            {
                SetProperty(ref _selectedResponse, value);
                responseID = _selectedResponse.ResponseID;
                localResponseID = _selectedResponse.LocalResponseID;
                searchViewModel.Add(Constants.Params.ResponseID, responseID.ToString());

                GoToResponseDetails.Execute();
                SetProperty(ref _selectedResponse, null);
            }
        }

        private bool _isForcedRefresh;
        public bool IsForcedRefresh
        {
            get => _isForcedRefresh;
            set => SetProperty(ref _isForcedRefresh, value);
        }

        private bool _isForcedFillUp;

        public ResponseListViewModel(IMvxNavigationService navigationService, IAppSettings settings,
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

            if (searchViewModel != null && searchViewModel.ContainsKey(Constants.Params.TemplateID))
            {
                templateID = Convert.ToInt32(searchViewModel[Constants.Params.TemplateID]);
            }

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
                ResponseDataViewModel result = null;
                if (NetworkCheck.HasInternet())
                {
                    result = await _webService.GetResponseList(searchViewModel);
                }
                else
                {
                    result = MvxApp.Database.GetResponseList(OfflineSearchParams);
                }

                if (result == null || result.Result == null || result.Result.Count == 0)
                {
                    if (NetworkCheck.HasInternet() || IsForcedRefresh == true)
                    {
                        await _navigationService.Close(this);
                    }

                    if (IsForcedRefresh == false)
                    {
                        IsBusy = false;
                        _isForcedFillUp = true;
                        GoToFillUp.Execute();
                    }
                }
                else
                {
                    ShowError = false;
                    foreach (ResponseModel row in result.Result)
                    {
                        _response.Add(new ResponseModel
                        {
                            LocalResponseID = row.LocalResponseID,
                            ResponseID = row.ResponseID,
                            TemplateID = row.TemplateID,
                            Title = row.Title,
                            Description = row.Description,
                            DateSubmitted = row.DateSubmitted,
                            StatusText = Helpers.GetResponseStatusText(row.Status),
                            Color = Helpers.GetResponseStatusColor(row.Status),
                        });
                    }

                    HasRecords = true;
                }

                FillUpBtnText = string.Format(Constants.Messages.FillUpQuestionnaire, result.NumOfAnswered, result.MaxLimit);
                CanFillUpQuestionnaire = result.NumOfAnswered < result.MaxLimit;
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
                _response = new ObservableCollection<ResponseModel>();
                _currentPage = 1;

                var param = new Dictionary<string, string>();
                searchViewModel = param;

                searchViewModel.Add(Constants.Params.TemplateID, templateID.ToString());
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
                IsForcedRefresh = false;
                IsRefreshing = false;
            }
        });

        public IMvxCommand GoToFillUp => new MvxCommand(async () =>
        {
            if (IsBusy) return;

            try
            {
                var parameter = new Dictionary<string, string>()
                {
                    {
                        Constants.Params.ResponseID, 0.ToString()
                    },
                    {
                        Constants.Params.TemplateID, templateID.ToString()
                    },
                    {
                        Constants.Params.LocalResponseID, 0.ToString()
                    },
                    {
                        Constants.Params.IsForcedFillUp, _isForcedFillUp.ToString()
                    }
                };
                await _navigationService.Navigate<ResponseViewEditViewModel, Dictionary<string, string>>(parameter);
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                await _navigationService.Close(this);
            }
        });

        public IMvxCommand GoToResponseDetails => new MvxCommand(async () =>
        {
            if (IsBusy) return;

            await Task.Delay(1000);

            try
            {
                var parameter = new Dictionary<string, string>()
                {
                    {
                        Constants.Params.ResponseID, responseID.ToString()
                    },
                    {
                        Constants.Params.TemplateID, templateID.ToString()
                    },
                    {
                        Constants.Params.LocalResponseID, localResponseID.ToString()
                    },
                    {
                        Constants.Params.IsForcedFillUp, _isForcedFillUp.ToString()
                    }
                };
                await _navigationService.Navigate<ResponseViewEditViewModel, Dictionary<string, string>>(parameter);
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
            OfflineSearchParams = new ResponseSearchViewModel
            {
                TemplateID = searchViewModel.ContainsKey(Constants.Params.TemplateID) ? searchViewModel[Constants.Params.TemplateID] : string.Empty,
                UserID = _settings.UserID,
                CompanyID = _settings.CompanyID,
                UserTypeID = _settings.UserTypeID,
                Page = int.Parse(searchViewModel[Constants.Params.Page]),
                PageSize = int.Parse(searchViewModel[Constants.Params.PageSize])
            };
        }

        public void CloseNavigation ()
        {
            _navigationService.Close(this);
        }
    }
}
