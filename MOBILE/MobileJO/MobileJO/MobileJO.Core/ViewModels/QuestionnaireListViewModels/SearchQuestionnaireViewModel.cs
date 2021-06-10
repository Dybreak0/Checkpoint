using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.Common;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileJO.Core.ViewModels
{
    public class SearchQuestionnaireViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly ILocalizeService _localizeService;
        private readonly IAppSettings _settings;
        private readonly IUserDialogs _userDialogs;
        private readonly IWebService _webService;

        public SearchQuestionnaireViewModel(IMvxNavigationService navigationService, IAppSettings settings,
            IUserDialogs userDialogs, ILocalizeService localizeService, IWebService webService)
            : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _userDialogs = userDialogs;
            _localizeService = localizeService;
            _webService = webService;
            LoadPickers.Execute();
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _category;
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        private bool _canFilterByCompany;
        public bool CanFilterByCompany
        {
            get => _canFilterByCompany;
            set => SetProperty(ref _canFilterByCompany, value);
        }

        private bool _canFilterByBranch;
        public bool CanFilterByBranch
        {
            get => _canFilterByBranch;
            set => SetProperty(ref _canFilterByBranch, value);
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
                GetBranchList.Execute();
            }
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

        private IMvxAsyncCommand LoadPickers => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (_settings.UserTypeID == Constants.UserType.SuperAdmin.ToString("d"))
                {
                    if (NetworkCheck.HasInternet())
                    {
                        Company = new List<DropdownViewModel>(await _webService.GetCompanies());
                    }
                    else
                    {
                        Company = MvxApp.Database.GetCompanies();
                    }
                            
                    CanFilterByCompany = false;
                    if (Company != null && Company.Count > 0)
                    {
                        CanFilterByCompany = true;
                    }
                }

                if (_settings.UserTypeID == Constants.UserType.CompanyAdmin.ToString("d"))
                {
                    SelectedCompany = new DropdownViewModel();
                    SelectedCompany.Value = Convert.ToInt32(_settings.CompanyID);

                    if (NetworkCheck.HasInternet())
                    {
                        Branch = new List<DropdownViewModel>(await _webService.GetBranches(SelectedCompany.Value));
                    }
                    else
                    {
                        Branch = MvxApp.Database.GetBranches(SelectedCompany.Value);
                    }
                        
                    CanFilterByCompany = false;
                    CanFilterByBranch = false;
                    if (Branch != null && Branch.Count > 0)
                    {
                        CanFilterByBranch = true;
                    }
                }
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
            finally
            {
                IsBusy = false;
            }
        });

        public IMvxAsyncCommand GetBranchList => new MvxAsyncCommand(async () =>
        {
            if (IsBusy) return;

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

                CanFilterByBranch = false;
                if (Branch != null && Branch.Count > 0)
                {
                    CanFilterByBranch = true;
                }
            }
            catch (Exception)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                await _navigationService.Close(this);
            }
        });

        public IMvxAsyncCommand SearchQuestionnaire => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string>();

            param.Add(Constants.Params.Title, string.IsNullOrEmpty(Title) ? Constants.SpecialCharacters.EmptyString : _title);
            param.Add(Constants.Params.Category, string.IsNullOrEmpty(Category) ? Constants.SpecialCharacters.EmptyString : _category);
            param.Add(Constants.Params.CompanyID, SelectedCompany == null ? Constants.SpecialCharacters.EmptyString : _selectedCompany.Value.ToString());
            param.Add(Constants.Params.BranchID, SelectedBranch == null ? Constants.SpecialCharacters.EmptyString : _selectedBranch.Value.ToString());

            await _navigationService.Navigate<QuestionnaireListViewModel, Dictionary<string, string>>(param);
        });
    }
}
