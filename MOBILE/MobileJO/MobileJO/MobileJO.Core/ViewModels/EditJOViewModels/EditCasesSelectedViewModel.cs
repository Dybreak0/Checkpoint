using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MobileJO.Core.ViewModels
{
    class EditCasesSelectedViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private readonly IWebService _webService;

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

        public EditCasesSelectedViewModel(IMvxNavigationService navigationService, 
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
                if (_parameter.ContainsKey(Constants.Params.EditedCases))
                {
                    CasesSelected = _serializer.DeserializeObject<ObservableCollection<Models.AssignedCases>>(_parameter[Constants.Params.EditedCases]);
                }
                else if (_parameter.ContainsKey(Constants.Params.SelectedJobOrder))
                {
                    var selectedJobOrder = _serializer.DeserializeObject<LocalJobOrder>(_parameter[Constants.Params.SelectedJobOrder]);

                    var caseIDs = new List<string>();

                    if (NetworkCheck.HasInternet() && selectedJobOrder.ServerID > 0)
                    {
                        JobOrderCases = await _webService.TaggedCaseList(selectedJobOrder.ServerID);

                        foreach (var jobOrderCase in JobOrderCases)
                        {
                            caseIDs.Add(jobOrderCase.CaseID.ToString());
                        }

                        var csvCaseIDs = string.Join(Constants.SpecialCharacters.Comma, caseIDs);

                        var cases = string.IsNullOrEmpty(csvCaseIDs) ? new List<AssignedCasesList>() : await _webService.FindCases(csvCaseIDs);

                        foreach (var temp in cases)
                        {
                            CasesSelected.Add(
                                new Models.AssignedCases
                                {
                                    ID = temp.id,
                                    CaseNumber = temp.case_number,
                                    CaseSubject = temp.case_subject
                                }
                            );
                        }
                        CasesSelected.OrderByDescending(x => x.CaseNumber);
                    }
                    else
                    {
                        if (selectedJobOrder.ServerID > 0)
                        {
                            JobOrderCases = MvxApp.Database.GetTaggedCases(selectedJobOrder.ServerID);
                        }
                        else
                        {
                            JobOrderCases = MvxApp.Database.GetLocalTaggedCases(selectedJobOrder.ID);
                        }
                        
                        foreach (var jobOrderCase in JobOrderCases)
                        {
                            caseIDs.Add(jobOrderCase.CaseID.ToString());
                        }

                        var intCaseIDs = caseIDs.Select(int.Parse).ToList();
                        CasesSelected = new ObservableCollection<Models.AssignedCases>(MvxApp.Database.GetCasesSelectedAsync(intCaseIDs));
                        CasesSelected.OrderByDescending(x => x.CaseNumber);
                    }                   
                                        
                    var serializedDefaultCases = _serializer.SerializeObject(CasesSelected);

                    if (_parameter.ContainsKey(Constants.Params.DefaultCases))
                    {
                        _parameter[Constants.Params.DefaultCases] = serializedDefaultCases;
                    }
                    else
                    {
                        _parameter.Add(Constants.Params.DefaultCases, serializedDefaultCases);
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

        public IMvxCommand EditCasesCommand => new MvxCommand(async () => 
        {
            if (IsBusy)
                return;
            IsBusy = true;
            var error = false;

            try
            {
                var serializedCases = _serializer.SerializeObject(CasesSelected);

                if (_parameter.ContainsKey(Constants.Params.EditedCasesSelected))
                {
                    _parameter[Constants.Params.EditedCasesSelected] = serializedCases;
                }                    
                else
                {
                    _parameter.Add(Constants.Params.EditedCasesSelected, serializedCases);
                }

                _serializedCases = await _navigationService.Navigate<EditCaseTaggingViewModel,
                                                                    Dictionary<string, string>,
                                                                    string>
                                                                    (_parameter);
                

                if (string.IsNullOrEmpty(_serializedCases))
                {
                    NoRecords = true;
                }
                else
                {
                    NoRecords = false;
                    CasesSelected = _serializer.DeserializeObject<ObservableCollection<Models.AssignedCases>>(_serializedCases);
                    CasesSelected.OrderByDescending(x => x.CaseNumber);

                    if (_parameter.ContainsKey(Constants.Params.EditedCases))
                    {
                        _parameter[Constants.Params.EditedCases] = _serializedCases;
                    }
                    else
                    {
                        _parameter.Add(Constants.Params.EditedCases, _serializedCases);
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
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorProcessing);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                await _navigationService.Close(this);
            }
        });

        public IMvxCommand GoToLastPageCommand => new MvxCommand(async () =>
        {
            if (_parameter.ContainsKey(Constants.Params.SelectedJobOrder))
            {
                await _navigationService.Navigate<EditJOLastViewModel, Dictionary<string, string>>(_parameter);
            }            
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
