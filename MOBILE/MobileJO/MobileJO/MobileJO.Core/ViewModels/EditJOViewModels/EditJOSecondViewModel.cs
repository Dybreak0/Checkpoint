using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using MobileJO.Core.Base;
using MvvmCross.Navigation;
using MobileJO.Core.Contracts;
using MvvmCross.Commands;
using MobileJO.Core.Utilities;
using MvvmCross.Base;
using MobileJO.Core.Models;
using System.Text.RegularExpressions;

namespace MobileJO.Core.ViewModels
{
    class EditJOSecondViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;
        private readonly IMvxJsonConverter _serializer;
        private Dictionary<string, string> _parameter;        

        public EditJOSecondViewModel(IMvxNavigationService navigationService, 
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

        public string NextStep { get; set; }
        public string PreventiveAction { get; set; }
        public string Remarks { get; set; }
        public string Attendees { get; set; }

        public bool NextStepError { get; set; }
        public bool PreventiveActionError { get; set; }
        public bool RemarksError { get; set; }
        public bool AttendeesError { get; set; }

        public string NextStepErrorMsg { get; set; }
        public string PreventiveActionErrorMsg { get; set; }
        public string RemarksErrorMsg { get; set; }
        public string AttendeesErrorMsg { get; set; }

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;

            PopulateFields();
        }

        public void PopulateFields()
        {
            var jobOrderItem = new LocalJobOrder();

            if (_parameter.ContainsKey(Constants.Params.SelectedJobOrder))
                jobOrderItem = _serializer.DeserializeObject<LocalJobOrder>(_parameter[Constants.Params.SelectedJobOrder]);

            NextStep = jobOrderItem.NextStep;
            PreventiveAction = jobOrderItem.PreventiveAction;
            Remarks = jobOrderItem.Remarks;
            Attendees = jobOrderItem.Attendees;
        }

        public IMvxCommand GoToCaseSelectedCommand => new MvxCommand(async () =>
        {
            var secondPageVM = new SecondPageViewModel
            {
                NextStep = NextStep,
                PreventiveAction = PreventiveAction,
                Remarks = Remarks,
                Attendees = Attendees
            };            

            if (IsValidFields(secondPageVM))
            {
                secondPageVM.NextStep.Trim();
                secondPageVM.PreventiveAction.Trim();
                secondPageVM.Remarks.Trim();
                secondPageVM.Attendees.Trim();

                var secondPageJsonText = _serializer.SerializeObject(secondPageVM);

                if (_parameter.ContainsKey(Constants.Params.SecondPage))
                {
                    _parameter[Constants.Params.SecondPage] = secondPageJsonText;
                }
                else
                {
                    _parameter.Add(Constants.Params.SecondPage, secondPageJsonText);
                }

                NextStepError = false;
                PreventiveActionError = false;
                RemarksError = false;
                AttendeesError = false;

                if(_parameter.ContainsKey(Constants.Params.EditedCases)) { _parameter.Remove(Constants.Params.EditedCases); }
                await _navigationService.Navigate<EditCasesSelectedViewModel, Dictionary<string, string>>(_parameter);
            }
        });

        public bool IsValidFields(SecondPageViewModel secondPageFields)
        {
            bool flag = true;

            if (string.IsNullOrWhiteSpace(secondPageFields.NextStep) || !Regex.IsMatch(secondPageFields.NextStep, Constants.Common.TextRegex))
            {
                NextStepErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.NextStep) ?
                                                                                            Constants.Messages.NextStepRequired :
                                                                                            Constants.Messages.NextStepInvalid;
                NextStepError = true;
                flag = false;
            }
            else { NextStepError = false; }

            if (string.IsNullOrWhiteSpace(secondPageFields.PreventiveAction) || !Regex.IsMatch(secondPageFields.PreventiveAction, Constants.Common.TextRegex))
            {
                PreventiveActionErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.PreventiveAction) ?
                                                                                            Constants.Messages.PreventiveActionRequired :
                                                                                            Constants.Messages.PreventiveActionInvalid;
                PreventiveActionError = true;
                flag = false;
            }
            else { PreventiveActionError = false; }

            if (string.IsNullOrWhiteSpace(secondPageFields.Remarks) || !Regex.IsMatch(secondPageFields.Remarks, Constants.Common.TextRegex))
            {
                RemarksErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.Remarks) ?
                                                                                               Constants.Messages.RemarksRequired :
                                                                                               Constants.Messages.RemarksInvalid;
                RemarksError = true;
                flag = false;
            }
            else { RemarksError = false; }

            if (string.IsNullOrWhiteSpace(secondPageFields.Attendees) || !Regex.IsMatch(secondPageFields.Attendees, Constants.Common.TextRegex))
            {
                AttendeesErrorMsg = string.IsNullOrWhiteSpace(secondPageFields.Attendees) ?
                                                                                               Constants.Messages.AttendeesRequired :
                                                                                               Constants.Messages.AttendeesInvalid;
                AttendeesError = true;
                flag = false;
            }
            else { AttendeesError = false; }

            return flag;
        }

    }
}

