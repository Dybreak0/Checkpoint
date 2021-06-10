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
using Xamarin.Forms;

namespace MobileJO.Core.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly ILocalizeService _localizeService;
        private readonly IAppSettings _settings;
        private readonly IUserDialogs _userDialogs;
        private readonly IWebService _webService;
        private List<string> _jobOrderStatus;
        private List<string> _applicationType;
        private string _jobOrderNumber;
        private string _selectedJobOrderStatus = Constants.Params.Pending;
        private string _selectedApplicationType = Constants.Params.StatusAll;
        public int status;
        public int applicationType;

        public string JobOrderNumber
        {
            get => _jobOrderNumber;
            set => SetProperty(ref _jobOrderNumber, value);
        }

        public List<string> JobOrderStatus
        {
            get => _jobOrderStatus;
            set => SetProperty(ref _jobOrderStatus, value);
        }
        
        public List<string> ApplicationType
        {
            get => _applicationType;
            set => SetProperty(ref _applicationType, value);
        }
        
        public string SelectedJobOrderStatus
        {
            get => _selectedJobOrderStatus;
            set => SetProperty(ref _selectedJobOrderStatus, value);
        }

       
        public string SelectedApplicationType
        {
            get => _selectedApplicationType;
            set => SetProperty(ref _selectedApplicationType, value);
        }

        public SearchViewModel(IMvxNavigationService navigationService, IAppSettings settings,
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

        private IMvxAsyncCommand LoadPickers => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    ApplicationType = new List<string>(await _webService.GetApplicationType());
                    ApplicationType.Add(Constants.Params.StatusAll);
                    JobOrderStatus = new List<string>(await _webService.GetJobOrderStatus());
                }
                else
                {
                    List<ApplicationType> ApplicationTypeTemp = MvxApp.Database.GetAllApplicationTypesAsync();
                    ApplicationType = new List<string>();
                    foreach (ApplicationType applicationType in ApplicationTypeTemp)
                    {
                        ApplicationType.Add(applicationType.ApplicationName);
                    }
                    ApplicationType.Add(Constants.Params.StatusAll);



                    List<JobOrderStatus> StatusTemp = MvxApp.Database.GetJobOrderStatusAsync();
                    JobOrderStatus = new List<string>();
                    foreach (JobOrderStatus jobOrderStatus in StatusTemp)
                    {
                        JobOrderStatus.Add(jobOrderStatus.Status);
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
                //Retrieve different types of status and application from local if an error is encountered.

                List<ApplicationType> ApplicationTypeTemp = MvxApp.Database.GetAllApplicationTypesAsync();
                ApplicationType = new List<string>();
                foreach (ApplicationType applicationType in ApplicationTypeTemp)
                {
                    ApplicationType.Add(applicationType.ApplicationName);
                }
                ApplicationType.Add(Constants.Params.StatusAll);



                List<JobOrderStatus> StatusTemp = MvxApp.Database.GetJobOrderStatusAsync();
                JobOrderStatus = new List<string>();
                foreach (JobOrderStatus jobOrderStatus in StatusTemp)
                {
                    JobOrderStatus.Add(jobOrderStatus.Status);
                }
            }


        });

        public IMvxAsyncCommand SearchFilter => new MvxAsyncCommand(async () =>
        {


            var param = new Dictionary<string, string>();


            if (string.IsNullOrEmpty(JobOrderNumber))
            {
                param.Add(Constants.Params.JobOrderNumber, "");
            }

           

            if (!string.IsNullOrEmpty(JobOrderNumber))
            {
                string jobOrder = JobOrderNumber.ToUpper();
                param.Add(Constants.Params.JobOrderNumber, jobOrder);
            }

            if (string.IsNullOrEmpty(SelectedJobOrderStatus))
            {
                param.Add(Constants.Params.JobOrderStatus, Constants.Params.PendingValue.ToString());
            }

            if (!string.IsNullOrEmpty(SelectedJobOrderStatus))
            {
                if (SelectedJobOrderStatus == Constants.Params.Pending)
                {
                    status = Constants.Params.PendingValue;
                }

                if (SelectedJobOrderStatus == Constants.Params.Signed)
                {
                    status = Constants.Params.SignedValue;
                }

                if (SelectedJobOrderStatus == Constants.Params.Sent)
                {
                    status = Constants.Params.SentValue;
                }

                if (SelectedJobOrderStatus == Constants.Params.RequestRevert)
                {
                    status = Constants.Params.RequestRevertValue;
                }

                param.Add(Constants.Params.JobOrderStatus, status.ToString());
            }

            if (string.IsNullOrEmpty(SelectedApplicationType))
            {
                param.Add(Constants.Params.ApplicationType, "");
            }

            if (!string.IsNullOrEmpty(SelectedApplicationType))
            {
                if (SelectedApplicationType == Constants.Params.WebPOS)
                {
                    applicationType = Constants.Params.WebPOSValue;
                    param.Add(Constants.Params.ApplicationType, applicationType.ToString());
                }

                if (SelectedApplicationType == Constants.Params.Portfolio)
                {
                    applicationType = Constants.Params.PortfolioValue;
                    param.Add(Constants.Params.ApplicationType, applicationType.ToString());
                }

                if (SelectedApplicationType == Constants.Params.StatusAll)
                {
                    param.Add(Constants.Params.ApplicationType, "");
                }


                //param.Add(Constants.Params.ApplicationType, applicationType.ToString());
            }

            //await _navigationService.Close(this);

            await _navigationService.Navigate<JobOrderListViewModel, Dictionary<string, string>>(param);
                
        });

      
    }
}
