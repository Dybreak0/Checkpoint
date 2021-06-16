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

namespace MobileJO.Core.ViewModels
{
    public class BillingListViewModel : BaseViewModel
    {
        public ObservableCollection<JobOrderBillingTypeModel> _billingTypes { get; set; } = new ObservableCollection<JobOrderBillingTypeModel>();

        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;
        private Dictionary<string, string> _parameter;
        public int param { get; set; }

        public BillingListViewModel(IMvxNavigationService navigationService,
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
                param = int.Parse(_parameter[Constants.Common.ID]);
                LoadBillingList.Execute();
            }
            else
            {
                LoadBillingList.Execute();
            }
        }

        private IMvxAsyncCommand LoadBillingList => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    _billingTypes = new ObservableCollection<JobOrderBillingTypeModel>(await _webService.GetBillingList(param));

                    _billingTypes = new ObservableCollection<JobOrderBillingTypeModel>(_billingTypes.OrderBy(x => x.BillingTypeID).ToList());
                }
                else
                {
                    if (_parameter.ContainsKey(Constants.Keys.LocalJobOrderID) && _parameter.ContainsKey(Constants.Keys.ServerJobOrderID))
                    {
                        var localID = int.Parse(_parameter[Constants.Keys.LocalJobOrderID]);
                        var serverID = int.Parse(_parameter[Constants.Keys.ServerJobOrderID]);

                        var joBillingType = new List<JobOrderBillingType>();

                        if (serverID > 0)
                        {
                            joBillingType = MvxApp.Database.GetJOBillingTypes(serverID);
                        }
                        else
                        {
                            joBillingType = MvxApp.Database.GetLocalJOBillingTypes(localID);
                        }

                        var billingTypes = MvxApp.Database.GetAllBillingTypesAsync();
                        
                        foreach (var billingType in joBillingType)
                        {
                            var result = MvxApp.Database.GetBillingTypeAsync(billingType.BillingTypeID);
                            _billingTypes.Add(new JobOrderBillingTypeModel()
                            {
                                BillingTypeID = result.ID,
                                BillingTypeName = result.BillingTypeName
                            });
                        }
                        _billingTypes.OrderBy(x => x.BillingTypeID);
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
