using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MobileJO.Core.ViewModels
{
    public class EditBillingTypesViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private readonly IWebService _webService;

        private Dictionary<string, string> _parameter;

        public ObservableCollection<SelectableItemWrapper<BillingTypes>> SelectionBillingTypes { get; private set; } = new ObservableCollection<SelectableItemWrapper<BillingTypes>>();

        private LocalJobOrder JobOrderItem { get; set; }

        private string _billingTypes { get; set; }

        public EditBillingTypesViewModel(IMvxNavigationService navigationService, 
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

            if (_parameter.ContainsKey(Constants.Params.SelectedJobOrder))
                JobOrderItem = _serializer.DeserializeObject<LocalJobOrder>(_parameter[Constants.Params.SelectedJobOrder]);

            LoadBillingTypesCommand.Execute();
        }

        public IMvxCommand LoadBillingTypesCommand => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if(NetworkCheck.HasInternet() && JobOrderItem.ServerID > 0)
                {
                    var id = JobOrderItem.ServerID;

                    var billingTypes = new List<BillingTypes>(await _webService.BillingTypeList()); 

                    var selectedBillingTypes = new List<JobOrderBillingType>(await _webService.JobOrderBillingTypeList(id));

                    var ids = new List<int>();

                    foreach(var sbt in selectedBillingTypes)
                    {
                        ids.Add(sbt.BillingTypeID);
                    }
                    
                    foreach (var bt in billingTypes)
                    {                                
                        SelectionBillingTypes.Add(new SelectableItemWrapper<BillingTypes>
                        {
                            Item = bt,
                            IsSelected = ids.Contains(bt.ID) ? true : false
                        });                                                    
                    }
                   
                }
                else
                {
                    var selectedBillingTypes = new ObservableCollection<JobOrderBillingType>();

                    if (JobOrderItem.ServerID > 0)
                    {
                        selectedBillingTypes = new ObservableCollection<JobOrderBillingType>(MvxApp.Database.GetJOBillingTypes(JobOrderItem.ServerID));
                    }
                    else
                    {
                        selectedBillingTypes = new ObservableCollection<JobOrderBillingType>(MvxApp.Database.GetLocalJOBillingTypes(JobOrderItem.ID));
                    }

                    var billingTypes = new ObservableCollection<BillingTypes>(MvxApp.Database.GetAllBillingTypesAsync()); 

                    var ids = new List<int>();

                    foreach (var sbt in selectedBillingTypes)
                    {
                        ids.Add(sbt.BillingTypeID);
                    }

                    foreach (var bt in billingTypes)
                    {
                        SelectionBillingTypes.Add(new SelectableItemWrapper<BillingTypes>
                        {
                            Item = bt,
                            IsSelected = ids.Contains(bt.ID) ? true : false
                        });
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
            }

        });

        private ObservableCollection<BillingTypes> GetSelectedBillingTypes()
        {
            var selected = SelectionBillingTypes
                .Where(p => p.IsSelected)
                .Select(p => p.Item)
                .ToList();

            return new ObservableCollection<BillingTypes>(selected);
        }

        public IMvxCommand AddBillingTypesCommand => new MvxCommand(async () =>
        {
            var selectedBillingTypes = GetSelectedBillingTypes();

            if (selectedBillingTypes.Count <= 0)
            {
                await _userDialogs.AlertAsync(Constants.Messages.BillingTypeRequired,
                                              Constants.Modal.Warning,
                                              Constants.Common.OK);
                return;
            }

            _billingTypes = _serializer.SerializeObject(selectedBillingTypes);

            await _navigationService.Close(this, _billingTypes);

        });

        public IMvxCommand CloseCommand => new MvxCommand(async () =>
        {
            await _navigationService.Close(this, _billingTypes);
        });
    }
}
