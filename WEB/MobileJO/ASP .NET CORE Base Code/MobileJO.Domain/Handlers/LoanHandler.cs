using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.LoanApplication;
using MobileJO.Domain.Contracts;
using System.Collections.Generic;
using Constants = MobileJO.Data.Constants;

namespace MobileJO.Domain.Handlers
{
    public class LoanHandler
    {
        private readonly ILoanService _loanService;

        public LoanHandler(ILoanService loanService)
        {
            _loanService = loanService;
        }


        public IEnumerable<ValidationResult> CanDelete(int id)
        {
            var validationErrors = new List<ValidationResult>();

            var loanApplication = _loanService.FindLoanApplication(id);
            if (loanApplication == null)
            {
                if (loanApplication.LoanStatus != "Pending")
                {
                    validationErrors.Add(new ValidationResult(Constants.Common.CannotDelete));
                }

                else
                {
                    validationErrors.Add(new ValidationResult(Constants.Common.SuccessDelete));
                }
            }
            return validationErrors;
        }

        public IEnumerable<ValidationResult> CanUpdate(LoanDetailsViewModel loanDetails)
        {
            var validationResult = new List<ValidationResult>();

            if (loanDetails.LoanID > 0)
            {
                if (_loanService.IsLoanApplicationExists(loanDetails.LoanID))
                {
                    validationResult = new List<ValidationResult>();
                }
                else
                {
                    validationResult.Add(new ValidationResult(Constants.Common.RecordDoesNotExist));
                }
            }
            else
            {
                validationResult.Add(new ValidationResult("Invalid Loan Application ID"));
            }

            return validationResult;
        }
        public IEnumerable<ValidationResult> CanApprove(LoanApprovalViewModel approvalViewModel)
        {
            var validationResult = new List<ValidationResult>();

            foreach (int loanID in approvalViewModel.LoanIDs)
            {
                if (loanID > 0)
                {
                    if (_loanService.IsLoanApplicationExists(loanID))
                    {
                        validationResult = new List<ValidationResult>();
                    }
                    else
                    {
                        validationResult.Add(new ValidationResult(Constants.Common.RecordDoesNotExist));
                    }
                }
                else
                {
                    validationResult.Add(new ValidationResult("Invalid Loan Application ID"));
                }
            }
            return validationResult;
        }
    }
}
