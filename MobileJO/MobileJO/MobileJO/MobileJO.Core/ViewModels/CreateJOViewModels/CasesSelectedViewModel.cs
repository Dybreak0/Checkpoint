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
    class CasesSelectedViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;

        private Dictionary<string, string> _parameter;
        
        public ObservableCollection<Models.AssignedCases> CasesSelected { get; private set; } = new ObservableCollection<Models.AssignedCases>();

        public List<TaggedCase> JobOrderCases { get; private set; } = new List<TaggedCase>();

        private string _selectedCaseID;
        private string _serializedCases { get; set; }

        private Models.AssignedCases _selectedCase;
        public Models.AssignedCases SelectedCase
        {
            get => _selectedCase;
            set
            {
                SetProperty(ref _selectedCase, value);
                if (_selectedCase != null)
                {
                    _selectedCaseID = _selectedCase.ID.ToString();
                    GoToDetails.Execute();
                    SetProperty(ref _selectedCase, null);
                }

            }

        }

        private bool _noRecords;
        public bool NoRecords
        {
            get => _noRecords;
            set => SetProperty(ref _noRecords, value);
        }

        public CasesSelectedViewModel(IMvxNavigationService navigationService, 
                                      IAppSettings settings,
                                      IUserDialogs userDialogs, 
                                      ILocalizeService localizeService, 
                                      IMvxJsonConverter serializer) : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _userDialogs = userDialogs;
            _localizeService = localizeService;
            _serializer = serializer;
            
        }

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;

            GetCasesSelected.Execute();
        }

        public IMvxCommand GetCasesSelected => new MvxCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (_parameter.ContainsKey(Constants.Params.Cases))
                {                    
                    CasesSelected = _serializer.DeserializeObject<ObservableCollection<Models.AssignedCases>>(_parameter[Constants.Params.Cases]);
                    CasesSelected.OrderByDescending(x => x.CaseNumber);

                    NoRecords = CasesSelected.Count() > 0 ? false : true;
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

        public IMvxCommand GoToLastPageCommand => new MvxCommand(async () =>
        {
            if(_parameter.ContainsKey(Constants.Params.IsEdit))
            {
                if (_parameter.ContainsKey(Constants.Params.Cases))
                {
                    _parameter[Constants.Params.Cases] = _serializedCases;
                }
                else
                {
                    _parameter.Add(Constants.Params.Cases, _serializedCases);
                }
            }
            
            await _navigationService.Navigate<NewJOLastViewModel, Dictionary<string, string>>(_parameter);                           
        });

        public IMvxCommand GoToDetails => new MvxCommand(async () =>
        {
            var param = new Dictionary<string, string>
            {
                { Constants.Keys.ID, _selectedCaseID }
            };

            await _navigationService.Navigate<CaseDetailsViewModel, Dictionary<string, string>>(param);
        });

    }
}
