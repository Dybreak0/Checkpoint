using System;
using System.Collections.Generic;
using System.Text;
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

namespace MobileJO.Core.ViewModels
{
    class BillingTypesSelectedViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private readonly IWebService _webService;

        private Dictionary<string, string> _parameter;
        
        public ObservableCollection<BillingTypes> BillingTypesSelected { get; private set; } = new ObservableCollection<BillingTypes>();        

        private bool _noRecords;
        public bool NoRecords
        {
            get => _noRecords;
            set => SetProperty(ref _noRecords, value);
        }

        public BillingTypesSelectedViewModel(IMvxNavigationService navigationService, 
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

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;

            GetBillingTypesSelected.Execute();
        }

        public IMvxCommand GetBillingTypesSelected => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (_parameter.ContainsKey(Constants.Params.EditedBillingTypes))
                {
                    NoRecords = false;
                    var newBillingTypes = _serializer.DeserializeObject<ObservableCollection<BillingTypes>>(_parameter[Constants.Params.EditedBillingTypes]);

                    BillingTypesSelected = newBillingTypes;

                    BillingTypesSelected = new ObservableCollection< BillingTypes >(BillingTypesSelected.OrderBy(x => x.ID).ToList());
                }
                else if (_parameter.ContainsKey(Constants.Params.SelectedJobOrder))
                {
                    NoRecords = false;

                    var JobOrderBillingTypes = new List<JobOrderBillingType>();

                    var selectedJobOrder = _serializer.DeserializeObject<LocalJobOrder>(_parameter[Constants.Params.SelectedJobOrder]);

                    if (NetworkCheck.HasInternet() && selectedJobOrder.ServerID > 0)
                    {
                        JobOrderBillingTypes = await _webService.JobOrderBillingTypeList(selectedJobOrder.ServerID);

                        var billingTypes = new List<BillingTypes>(await _webService.BillingTypeList());

                        foreach (var jobOrderBillingType in JobOrderBillingTypes)
                        {
                            var tempBillingType = billingTypes.Where(x => x.ID == jobOrderBillingType.BillingTypeID)
                                                              .FirstOrDefault();

                            BillingTypesSelected.Add(tempBillingType);
                        }
                    }
                    else
                    {
                        if (selectedJobOrder.ServerID > 0)
                        {
                            JobOrderBillingTypes = MvxApp.Database.GetJOBillingTypes(selectedJobOrder.ServerID);
                        }
                        else
                        {
                            JobOrderBillingTypes = MvxApp.Database.GetLocalJOBillingTypes(selectedJobOrder.ID);
                        }

                        foreach (var jobOrderBillingType in JobOrderBillingTypes)
                        {
                            var tempBillingType = MvxApp.Database.GetBillingTypeAsync(jobOrderBillingType.BillingTypeID);

                            BillingTypesSelected.Add(tempBillingType);
                        }
                    }
                    
                    BillingTypesSelected = new ObservableCollection<BillingTypes>(BillingTypesSelected.OrderBy(x => x.ID)
                                                                                                      .ToList());
                }
                else
                {
                    NoRecords = true;
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
            }
        });

    }
}
