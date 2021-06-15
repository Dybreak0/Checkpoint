using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Core.ViewModels
{
    public class TagCaseDetailsViewModel : BaseViewModel
    {
        public TaggedCase _tagCaseModel { get; set; } = new TaggedCase();
        public TaggedCase TagModel { get; set; }

        private TaggedCase tagCaseModel;
        public TaggedCase TagCaseModel
        {
            get => _tagCaseModel;
            set
            {
                SetProperty(ref tagCaseModel, value);
                tagCaseID = tagCaseModel.ID;
            }
        }

        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;
        private Dictionary<string, string> _parameter;
        private int tagCaseID;

        public TagCaseDetailsViewModel(IMvxNavigationService navigationService,
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
                tagCaseID = int.Parse(_parameter[Constants.Common.ID]);
                LoadCaseDetails.Execute();
            }
        }

        private IMvxAsyncCommand LoadCaseDetails => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    _tagCaseModel = await _webService.TagCaseDetail(tagCaseID);

                    if (_tagCaseModel != null)
                    {
                        _tagCaseModel = new TaggedCase()
                        {
                            ID = _tagCaseModel.ID,
                            CaseNumber = _tagCaseModel.CaseNumber,
                            Status = _tagCaseModel.Status,
                            ApplicationType = _tagCaseModel.ApplicationType,
                            CaseSubject = _tagCaseModel.CaseSubject,
                            Priority = _tagCaseModel.Priority,
                            AccountName = _tagCaseModel.AccountName,
                            Description = _tagCaseModel.Description,
                            AssignedTo = _tagCaseModel.AssignedTo,
                            CreatedDate = _tagCaseModel.CreatedDate,
                            CreatedBy = _tagCaseModel.CreatedBy,
                            UpdatedDate = _tagCaseModel.UpdatedDate
                        };
                    }
                }
                else
                {

                    var localAssignedCase = MvxApp.Database.GetCasesAsync(tagCaseID);
                    var accounts = MvxApp.Database.GetAllAccountsAsync();
                    var appTypes = MvxApp.Database.GetAllApplicationTypesAsync();
                    var account = accounts.Where(x => x.ID == localAssignedCase.AccountID).FirstOrDefault();
                    var appType = appTypes.Where(x => x.ID == localAssignedCase.ApplicationTypeID).FirstOrDefault();



                    _tagCaseModel = new TaggedCase()
                    {
                        ID = localAssignedCase.ID,
                        CaseNumber = localAssignedCase.CaseNumber,
                        Status = localAssignedCase.Status,
                        ApplicationType = appType.ApplicationName,
                        CaseSubject = localAssignedCase.CaseSubject,
                        Priority = localAssignedCase.Priority,
                        AccountName = account.Name,
                        Description = localAssignedCase.Description,
                        AssignedTo = localAssignedCase.AssignedTo,
                        CreatedDate = localAssignedCase.CreatedDate,
                        CreatedBy = localAssignedCase.CreatedBy,
                        UpdatedBy = localAssignedCase.UpdatedBy,
                        UpdatedDate = localAssignedCase.UpdatedDate
                    };
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

        public AssignedCase TaggedCase { get; private set; }
    }
}
