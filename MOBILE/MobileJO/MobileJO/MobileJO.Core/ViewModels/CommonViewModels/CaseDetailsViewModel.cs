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
    public class CaseDetailsViewModel : BaseViewModel
    {
        private AssignedCase _assignedCaseModel { get;  set; } = new AssignedCase();
        public AssignedCase AssignedCaseModel
        {
            get
            {
                return _assignedCaseModel;
            }
            set
            {
                _assignedCaseModel = value;
                OnPropertyChanged();
            }
        }
        
        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;
        private Dictionary<string, string> _parameter;
        public int CaseID { get; set; }

        public CaseDetailsViewModel(IMvxNavigationService navigationService,
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

            if (_parameter != null && _parameter.ContainsKey(Constants.Keys.ID))
            {
                CaseID = int.Parse(_parameter[Constants.Keys.ID]);
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
                    _assignedCaseModel = await _webService.AssignedCase(CaseID);

                    if(_assignedCaseModel != null)
                    {
                        AssignedCaseModel = new AssignedCase()
                        {
                            ID = _assignedCaseModel.ID,
                            CaseNumber = _assignedCaseModel.CaseNumber,
                            Status = _assignedCaseModel.Status,
                            ApplicationType = _assignedCaseModel.ApplicationType,
                            CaseSubject = _assignedCaseModel.CaseSubject,
                            Priority = _assignedCaseModel.Priority,
                            AccountName = _assignedCaseModel.AccountName,
                            Description = _assignedCaseModel.Description,
                            AssignedTo = _assignedCaseModel.AssignedTo,
                            CreatedBy = _assignedCaseModel.CreatedBy,
                            ModifiedBy = _assignedCaseModel.ModifiedBy
                        };
                       
                    }

                }
                else
                {

                    var localCase = MvxApp.Database.GetCasesAsync(CaseID);

                    if (localCase != null)
                    {
                        var accounts = MvxApp.Database.GetAllAccountsAsync();
                        var appTypes = MvxApp.Database.GetAllApplicationTypesAsync();
                        var account = accounts.Where(x => x.ID == localCase.AccountID).FirstOrDefault();
                        var appType = appTypes.Where(x => x.ID == localCase.ApplicationTypeID).FirstOrDefault();

                        AssignedCaseModel = new AssignedCase()
                        {
                            ID = localCase.ID,
                            CaseNumber = localCase.CaseNumber,
                            Status = localCase.Status,
                            ApplicationType = appType.ApplicationName,
                            CaseSubject = localCase.CaseSubject,
                            Priority = localCase.Priority,
                            AccountName = account.Name,
                            Description = localCase.Description,
                            AssignedTo = localCase.AssignedTo,
                            CreatedBy = string.Format(Constants.Common.DateFormatWithName, Helpers.GetFormattedDate(localCase.CreatedDate), localCase.CreatedBy),
                            ModifiedBy = string.Format(Constants.Common.DateFormatWithName, Helpers.GetFormattedDate(localCase.UpdatedDate), localCase.UpdatedBy)
                        };

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

            if (error)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                await _navigationService.Close(this);
            }
        });

    }


}
