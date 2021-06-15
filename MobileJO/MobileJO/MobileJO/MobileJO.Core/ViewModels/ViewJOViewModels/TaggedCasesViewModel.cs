using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MobileJO.Core.ViewModels
{
    public class TaggedCasesViewModel : BaseViewModel
    {
        public ObservableCollection<TaggedCaseModel> _taggedCases { get; set; } = new ObservableCollection<TaggedCaseModel>();

        private int tagCaseID;
        private TaggedCaseModel _selectedCase;
        public TaggedCaseModel SelectedCase
        {
            get => _selectedCase;
            set
            {
                SetProperty(ref _selectedCase, value);
                tagCaseID = _selectedCase.ID;
                GoToCaseDetails.Execute();
                SetProperty(ref _selectedCase, null);
            }
        }
       

        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;
        private Dictionary<string, string> _parameter;
        public int jobOrderID { get; set; }        

        public TaggedCasesViewModel(IMvxNavigationService navigationService,
                                    IAppSettings settings,
                                    IUserDialogs userDialogs,
                                    ILocalizeService localizeService,
                                    IWebService webService)
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
            _parameter = parameter;

            if (_parameter != null && _parameter.ContainsKey(Constants.Common.ID))
            {
                jobOrderID = int.Parse(_parameter[Constants.Common.ID]);
                _parameter.Add(Constants.Params.Page, Constants.Common.PageNo.ToString());
                _parameter.Add(Constants.Params.PageSize, Constants.Common.PageNo.ToString());
                TaggedCasesList.Execute();
            }
            else
            {
                TaggedCasesList.Execute();
            }
        }

        public IMvxAsyncCommand TaggedCasesList => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (NetworkCheck.HasInternet())
                {
                
                    _taggedCases = new ObservableCollection<TaggedCaseModel>(await _webService.TaggedCasesList(_parameter));
               
                }
                else
                {
                    if(_parameter.ContainsKey(Constants.Keys.LocalJobOrderID) && _parameter.ContainsKey(Constants.Keys.ServerJobOrderID))
                    {
                        var localID = int.Parse(_parameter[Constants.Keys.LocalJobOrderID]);
                        var serverID = int.Parse(_parameter[Constants.Keys.ServerJobOrderID]);

                        var taggedCases = new List<TaggedCase>();

                        if (serverID > 0)
                        {
                            taggedCases = MvxApp.Database.GetTaggedCases(serverID);
                        }
                        else
                        {
                            taggedCases = MvxApp.Database.GetLocalTaggedCases(localID);
                        }

                        var appTypes = MvxApp.Database.GetAllApplicationTypesAsync();
                        var account = MvxApp.Database.GetAllAccountsAsync();

                        foreach (TaggedCase taggedCase in taggedCases)
                        {
                            var tagCased = MvxApp.Database.GetCasesAsync(taggedCase.CaseID);

                            _taggedCases.Add(new TaggedCaseModel()
                            {
                                ID = tagCased.ID,
                                CaseNumber = tagCased.CaseNumber,
                                Status = tagCased.Status,
                                ApplicationType = appTypes.Where(x => x.ID == tagCased.ApplicationTypeID).FirstOrDefault().ApplicationName,
                                CaseSubject = tagCased.CaseSubject,
                                AccountName = account.Where(a => a.ID == tagCased.AccountID).FirstOrDefault().Name
                            });

                        }
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
        });

        public IMvxAsyncCommand GoToCaseDetails => new MvxAsyncCommand(async () =>
        {

            await Task.Delay(1000);

            var param = new Dictionary<string, string> { };
            if (NetworkCheck.HasInternet())
            {
                param = new Dictionary<string, string> { { Constants.Common.ID, tagCaseID.ToString() } };
            }
            else
            {
                param = new Dictionary<string, string> { { Constants.Common.ID, tagCaseID.ToString() } };
            }
            await _navigationService.Navigate<TagCaseDetailsViewModel, Dictionary<string, string>>(param);
        });
    }
}
