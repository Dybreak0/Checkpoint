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

namespace MobileJO.Core.ViewModels.CustomerOrderViewModels
{
    public class SearchCustomerOrderViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly ILocalizeService _localizeService;
        private readonly IAppSettings _settings;
        private readonly IUserDialogs _userDialogs;
        private readonly IWebService _webService;
        private List<string> _customerOrderStatus;
        private string _customerOrderNumber;
        private string _selectedCustomerOrderStatus = Constants.Params.StatusAll;
        public string status;
        public int applicationType;

        public string CustomerOrderNumber
        {
            get => _customerOrderNumber;
            set => SetProperty(ref _customerOrderNumber, value);
        }

        public List<string> CustomerOrderStatus
        {
            get => _customerOrderStatus;
            set => SetProperty(ref _customerOrderStatus, value);
        }

        public string SelectedCustomerOrderStatus
        {
            get => _selectedCustomerOrderStatus;
            set => SetProperty(ref _selectedCustomerOrderStatus, value);
        }

        public SearchCustomerOrderViewModel(IMvxNavigationService navigationService, IAppSettings settings,
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
                    //CustomerOrderStatus = new List<string>(await _webService.GetCustomerOrderStatus());
                    CustomerOrderStatus = new List<string>();
                    CustomerOrderStatus.Add("All");
                    CustomerOrderStatus.Add("Pending");
                    CustomerOrderStatus.Add("Approved");
                }
                else
                {

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

                //List<JobOrderStatus> StatusTemp = MvxApp.Database.GetJobOrderStatusAsync();
                //CustomerOrderStatus = new List<string>();
                //foreach (JobOrderStatus jobOrderStatus in StatusTemp)
                //{
                //    JobOrderStatus.Add(jobOrderStatus.Status);
                //}
            }
        });

        public IMvxAsyncCommand SearchFilter => new MvxAsyncCommand(async () =>
        {
            var param = new Dictionary<string, string>();


            if (string.IsNullOrEmpty(CustomerOrderNumber))
            {
                param.Add("CustomerOrderNumber", "");
            }

            if (!string.IsNullOrEmpty(CustomerOrderNumber))
            {
                string customerOrder = CustomerOrderNumber.ToUpper();
                param.Add("CustomerOrderNumber", customerOrder);
            }

            if (SelectedCustomerOrderStatus == "All")
            {
                status = "All";
            }
            else if (SelectedCustomerOrderStatus == "Pending")
            {
                status = "Pending";
            }
            else if (SelectedCustomerOrderStatus == "Approved")
            {
                status = "Approved";
            }

            param.Add("CustomerOrderStatus", status);

            //await _navigationService.Close(this);

            await _navigationService.Navigate<CustomerOrderListViewModel, Dictionary<string, string>>(param);

        });


    }
}
