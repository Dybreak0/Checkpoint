using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using MobileJO.Core.Base;
using MvvmCross.Navigation;
using MobileJO.Core.Contracts;
using MvvmCross.Commands;
using MobileJO.Core.Utilities;
using MobileJO.Core.Models;
using System.Collections.ObjectModel;
using MvvmCross.Base;
using System.Linq;

namespace MobileJO.Core.ViewModels
{
    public class BillingTypesViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private readonly IWebService _webService;

        private Dictionary<string, string> _parameter;

        public ObservableCollection<BillingTypes> selectedBillingTypes { get; set; }

        private string _billingTypes { get; set; }

        public BillingTypesViewModel(IMvxNavigationService navigationService, 
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
        
        public ObservableCollection<SelectableItemWrapper<BillingTypes>> SelectionBillingTypes { get; private set; }  = new ObservableCollection<SelectableItemWrapper<BillingTypes>>();

        public List<BillingTypes> BillingTypes { get; private set; } = new List<BillingTypes>();

        public IMvxCommand LoadBillingTypesCommand => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                var ids = new List<int>();

                if (_parameter.ContainsKey(Constants.Params.BillingTypes))
                {                    
                    List<BillingTypes> selectedBillingTypes = _serializer.DeserializeObject<List<BillingTypes>>(_parameter[Constants.Params.BillingTypes]);

                    foreach (var sbt in selectedBillingTypes)
                    {
                        ids.Add(sbt.ID);
                    }
                }

                if (NetworkCheck.HasInternet())
                {
                    var billingTypes = new List<BillingTypes>(await _webService.BillingTypeList());

                    foreach (BillingTypes bt in billingTypes)
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
                    var localBillingTypes = MvxApp.Database.GetAllBillingTypesAsync();

                    var billingTypes = new List<BillingTypes>(localBillingTypes);

                    foreach (BillingTypes bt in billingTypes)
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

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;

            LoadBillingTypesCommand.Execute();

        }

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
