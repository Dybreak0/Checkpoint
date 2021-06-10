using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Data.ViewModels.EmailJO;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MobileJO.Core.ViewModels.EmailJO
{
    public class SelectJOViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;
        private int _createdBy { get; set; }
        private int _caseID { get; set; }
        private bool _toggleSelect { get; set; }
        private string _selectToggle { get; set; }
        public bool IsSelectAll { get; set; }
        private Dictionary<string, string> _parameter;

        private ObservableCollection<SelectableItemWrapper<SelectJOModel>> _jOTaggedCase;
        
        public ObservableCollection<SelectableItemWrapper<SelectJOModel>> JOTaggedCase
        {
            get => _jOTaggedCase;
            set
            {
                SetProperty(ref _jOTaggedCase, value);
            }
        }
        

        public SelectJOViewModel(IMvxNavigationService navigationService,
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
            _toggleSelect = false;
            _selectToggle = "Select All";
        }
        
        public string SelectToggle
        {
            get => _selectToggle; 
            set => SetSelectToggle();
        }

        private void SetSelectToggle()
        {
            if (_toggleSelect)
            {
                _selectToggle = "Select All";
                _toggleSelect = false;
                IsSelectAll = true;
            }
            else
            {
                _selectToggle = "Unselect All";
                _toggleSelect = true;
                IsSelectAll = false;
            }
        }

        private void SelectUnselectAll(bool val)
        {

            if (JOTaggedCase.Count() > 0)
            {
                foreach (SelectableItemWrapper<SelectJOModel> selectedItem in JOTaggedCase)
                {
                    selectedItem.IsSelected = val;
                }
            }
        }
        
        private string [] GetSelectedJOs()
        {
            return JOTaggedCase
                .Where(p => p.IsSelected)
                .Select(p => p.Item.job_order_no)
                .ToArray();           
        }


        public IMvxCommand ToggleSelectAll => new MvxCommand(() =>
        {
            SetSelectToggle();

            if(_toggleSelect)
            {
                SelectUnselectAll(true);
            }
            else
            {
                SelectUnselectAll(false);
            }
        });

        public IMvxAsyncCommand GoTaggedCasesPageCommand => new MvxAsyncCommand(async () =>
        {
            var jobOrders = GetSelectedJOs();

            if (jobOrders.Count() > 0)
            {
                var selectedJos = string.Join(Constants.SpecialCharacters.SemiColon, GetSelectedJOs());
                _parameter = new Dictionary<string, string> { { Constants.Keys.JobOrderIDs, selectedJos } };

                await _navigationService.Navigate<EmailJOViewModel, Dictionary<string, string>>(_parameter);
            }
            else
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.NoSelectedJo);
                await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
            }

            
        });

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;

            if (_parameter != null && _parameter.ContainsKey(Constants.Params.AssignedTo) && _parameter.ContainsKey(Constants.Params.CaseID))
            {
                _caseID    = int.Parse(_parameter[Constants.Params.CaseID]);
                _createdBy = int.Parse(_parameter[Constants.Params.AssignedTo]);
                LoadList.Execute();
            }
        }

        private IMvxAsyncCommand LoadList => new MvxAsyncCommand(async () =>
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    var cases = await _webService.SelectJOList(_createdBy, _caseID);
                    if(cases.Count > 0)
                    {
                        var tempTaggedCases = new ObservableCollection<SelectableItemWrapper<SelectJOModel>>();
                        foreach (SelectJOModel _case in cases)
                        {
                            tempTaggedCases.Add(new SelectableItemWrapper<SelectJOModel>
                            {
                                Item = new SelectJOModel
                                {
                                    id = _case.id,
                                    job_order_no = _case.job_order_no,
                                    job_order_subject = _case.job_order_subject
                                }
                            });
                        }

                        JOTaggedCase = new ObservableCollection<SelectableItemWrapper<SelectJOModel>>(tempTaggedCases.OrderByDescending(x => x.Item.id).ToList());
                    }
                                       
                }
                else
                {
                    var localizedMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                    await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
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
