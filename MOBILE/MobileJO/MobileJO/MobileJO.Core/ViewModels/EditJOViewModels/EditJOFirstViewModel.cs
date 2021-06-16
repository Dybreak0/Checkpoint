using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using MobileJO.Core.Base;
using MvvmCross.Navigation;
using MobileJO.Core.Contracts;
using MvvmCross.Commands;
using MobileJO.Core.Utilities;
using MobileJO.Core.Models;
using MvvmCross.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace MobileJO.Core.ViewModels
{
    public class EditJOFirstViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private readonly IWebService _webService;

        private Dictionary<string, string> _parameter;

        public ObservableCollection<Account> AccountsDDL { get; private set; } = new ObservableCollection<Account>();
        public ObservableCollection<ApplicationType> ApplicationTypesDDL { get; private set; } = new ObservableCollection<ApplicationType>();
        
        public EditJOFirstViewModel(IMvxNavigationService navigationService, 
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

        public string JobOrderNumber { get; set; }
        public string Branch { get; set; }
        public int AccountID { get; set; }
        public int ApplicationType { get; set; }
        public string JobOrderSubject { get; set; }
        public DateTime DateStart{ get; set; }
        public TimeSpan TimeStart{ get; set; }
        public DateTime DateEnd { get; set; }
        public TimeSpan TimeEnd{ get; set; }
        public string ActivityDetails { get; set; }
        public string RootCauseAnalysis { get; set; }

        public bool JobOrderSubjectError { get; set; }
        public bool AccountIDError { get; set; }
        public bool BranchError { get; set; }
        public bool DateStartError { get; set; }
        public bool TimeStartError { get; set; }
        public bool DateEndError { get; set; }
        public bool TimeEndError { get; set; }
        public bool ApplicationTypeError { get; set; }
        public bool ActivityDetailsError { get; set; }
        public bool RootCauseAnalysisError { get; set; }

        public string JOSubjectErrorMsg { get; set; }
        public string BranchErrorMsg { get; set; }
        public string ActivityDetailsErrorMsg { get; set; }
        public string RootCauseErrorMsg { get; set; }

        public Account SelectedAccount { get; set; }
        public ApplicationType SelectedApplication { get; set; }
        public int CurrentAccountSelected { get; set; }

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;

            LoadJobOrderDataCommand.Execute();          
        }

        public IMvxCommand LoadJobOrderDataCommand => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    var accounts = new List<Account>(await _webService.AccountList());                    

                    var applicationTypes = new List<ApplicationType>(await _webService.ApplicationTypeList());

                    MvxApp.Database.SaveAccountsAsync(accounts);

                    MvxApp.Database.SaveApplicationTypesAsync(applicationTypes);

                    AccountsDDL = new ObservableCollection<Account>(accounts);

                    ApplicationTypesDDL = new ObservableCollection<ApplicationType>(applicationTypes);
                }
                else
                {
                    var localAccountList = MvxApp.Database.GetAllAccountsAsync();

                    var localApplicationTypeList = MvxApp.Database.GetAllApplicationTypesAsync();

                    AccountsDDL = new ObservableCollection<Account>(localAccountList);

                    ApplicationTypesDDL = new ObservableCollection<ApplicationType>(localApplicationTypeList);
                }
                                
                var jobOrderItem = new LocalJobOrder();

                if (_parameter.ContainsKey(Constants.Params.SelectedJobOrder))
                {
                    jobOrderItem = _serializer.DeserializeObject<LocalJobOrder>(_parameter[Constants.Params.SelectedJobOrder]);
                }

                JobOrderNumber    = jobOrderItem.JobOrderNumber;
                Branch            = jobOrderItem.Branch;
                ApplicationType   = jobOrderItem.ApplicationTypeID;
                JobOrderSubject   = jobOrderItem.JobOrderSubject;
                DateStart         = jobOrderItem.DateTimeStart;
                TimeStart         = TimeSpan.Parse(jobOrderItem.DateTimeStart.ToString(Constants.Common.TimePickerFormat));
                DateEnd           = jobOrderItem.DateTimeEnd;
                TimeEnd           = TimeSpan.Parse(jobOrderItem.DateTimeEnd.ToString(Constants.Common.TimePickerFormat));
                ActivityDetails   = jobOrderItem.ActivityDetails;
                RootCauseAnalysis = jobOrderItem.RootCauseAnalysis;

                SelectedAccount = AccountsDDL.FirstOrDefault(x => x.ID == jobOrderItem.AccountID);
                SelectedApplication = ApplicationTypesDDL.FirstOrDefault(x => x.ID == jobOrderItem.ApplicationTypeID);
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

        public IMvxCommand GoToSecondPageCommand => new MvxCommand(async () =>
        {
            var error = false;

            try
            {
                if (IsBusy)
                    return;

                var firstPageVM = new FirstPageViewModel
                {
                    JobOrderSubject = JobOrderSubject,
                    AccountID = SelectedAccount != null ? SelectedAccount.ID : 0,
                    Branch = Branch,
                    DateTimeStart = string.Concat(DateStart.ToString(Constants.Common.DateFormat), TimeStart.ToString()),
                    DateTimeEnd = string.Concat(DateEnd.ToString(Constants.Common.DateFormat), TimeEnd.ToString()),
                    ApplicationType = SelectedApplication.ID,
                    ActivityDetails = ActivityDetails,
                    RootCauseAnalysis = RootCauseAnalysis
                };

                if (IsValidFields(firstPageVM))
                {
                    firstPageVM.JobOrderSubject.Trim();
                    firstPageVM.Branch.Trim();
                    firstPageVM.ActivityDetails.Trim();
                    firstPageVM.RootCauseAnalysis.Trim();

                    var firstPageJsonText = _serializer.SerializeObject(firstPageVM);

                    if (_parameter.ContainsKey(Constants.Params.FirstPage))
                    {
                        _parameter[Constants.Params.FirstPage] = firstPageJsonText;
                    }
                    else
                    {
                        _parameter.Add(Constants.Params.FirstPage, firstPageJsonText);
                    }

                    JobOrderSubjectError = false;
                    AccountIDError = false;
                    BranchError = false;
                    ApplicationTypeError = false;
                    ActivityDetailsError = false;
                    RootCauseAnalysisError = false;

                    await _navigationService.Navigate<EditJOSecondViewModel, Dictionary<string, string>>(_parameter);
                }
            }
            catch (Exception)
            {
                error = true;
            }

            if (error)
            {
                var localizedMessage = Constants.Messages.ErrorProcessing;
                await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }
        });

        public bool IsValidFields(FirstPageViewModel firstPageFields)
        {
            bool flag = true;

            if (string.IsNullOrWhiteSpace(firstPageFields.JobOrderSubject) || !Regex.IsMatch(firstPageFields.JobOrderSubject, Constants.Common.TextRegex))
            {
                JOSubjectErrorMsg = string.IsNullOrWhiteSpace(firstPageFields.JobOrderSubject) ?
                                                                                            Constants.Messages.JOSubjectRequired :
                                                                                            Constants.Messages.JOSubjectInvalid;
                JobOrderSubjectError = true;
                flag = false;
            }
            else { JobOrderSubjectError = false; }

            if (firstPageFields.AccountID <= 0)
            {
                AccountIDError = true;
                flag = false;
            }
            else { AccountIDError = false; }

            if (string.IsNullOrWhiteSpace(firstPageFields.Branch) || !Regex.IsMatch(firstPageFields.Branch, Constants.Common.TextRegex))
            {
                BranchErrorMsg = string.IsNullOrWhiteSpace(firstPageFields.Branch) ?
                                                                                            Constants.Messages.BranchNameRequired :
                                                                                            Constants.Messages.BranchNameInvalid;
                BranchError = true;
                flag = false;
            }
            else { BranchError = false; }

            int dateCompareResult = DateStart.Date.CompareTo(DateEnd.Date);
            int timeCompareResult = TimeSpan.Compare(TimeStart, TimeEnd);

            if (dateCompareResult > 0)
            {
                DateStartError = true;
                DateEndError = true;
                flag = false;
            }
            else
            {
                DateStartError = false;
                DateEndError = false;
            }

            if (dateCompareResult == 0 && timeCompareResult > 0)
            {
                TimeStartError = true;
                TimeEndError = true;
                flag = false;
            }
            else
            {
                TimeStartError = false;
                TimeEndError = false;
            }

            if (firstPageFields.ApplicationType <= 0)
            {
                ApplicationTypeError = true;
                flag = false;
            }
            else { ApplicationTypeError = false; }

            if (string.IsNullOrWhiteSpace(firstPageFields.ActivityDetails) || !Regex.IsMatch(firstPageFields.ActivityDetails, Constants.Common.TextRegex))
            {
                ActivityDetailsErrorMsg = string.IsNullOrWhiteSpace(firstPageFields.ActivityDetails) ?
                                                                                            Constants.Messages.ActivityDetailsRequired :
                                                                                            Constants.Messages.ActivityDetailsInvalid;
                ActivityDetailsError = true;
                flag = false;
            }
            else { ActivityDetailsError = false; }

            if (string.IsNullOrWhiteSpace(firstPageFields.RootCauseAnalysis) || !Regex.IsMatch(firstPageFields.RootCauseAnalysis, Constants.Common.TextRegex))
            {
                RootCauseErrorMsg = string.IsNullOrWhiteSpace(firstPageFields.RootCauseAnalysis) ?
                                                                                            Constants.Messages.RootCauseRequired :
                                                                                            Constants.Messages.RootCauseInvalid;
                RootCauseAnalysisError = true;
                flag = false;
            }
            else { RootCauseAnalysisError = false; }

            return flag;
        }

    }

}
