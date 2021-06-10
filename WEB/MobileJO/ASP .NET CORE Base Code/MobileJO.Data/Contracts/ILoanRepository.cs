
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.LoanApplication;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Data.Contracts
{
    public interface ILoanRepository
    {
        int Create(Loan loan,
                   List<LoanCreditHistory> creditHistories,
                   List<LoanCustomerChildren> customerChildrens,
                   List<LoanPersonalProperty> personalProperties,
                   List<LoanUnitDesired> unitDesireds,
                   List<LoanAttachment> loanAttachments,
                   string signatureFilename);
        Loan FindLoan(int id);

        ListViewModel ListLoanApplication(LoanSearchViewModel searchListViewModel);
        ListViewModel PendingLoanApplication(LoanSearchPendingViewModel searchPendingViewModel);
        IQueryable<Loan> RetrieveAllLoanApplication();
        List<LoanDetailsViewModel> DownloadLoanApplication(LoanSearchViewModel searchListViewModel);
        Loan FindLoanApplication(int id);

        bool Update(Loan loan,
                   List<LoanCreditHistory> newCreditHistories,
                   List<LoanCustomerChildren> newCustomerChildrens,
                   List<LoanPersonalProperty> newPersonalProperties,
                   List<LoanUnitDesired> newUnitDesireds,
                   List<LoanAttachment> newAttachments,
                   List<string> removedAttachments);

        List<LoanCreditHistory> RetrieveLoanCreditHistories(int loanID);
        List<LoanCustomerChildren> RetrieveLoanCustomerChildrens(int loanID);
        List<LoanPersonalProperty> RetrieveLoanPersonalProperties(int loanID);
        List<LoanUnitDesired> RetrieveLoanUnitDesireds(int loanID);
        List<LoanUnitDesiredTC> RetrieveLoanUnitDesiredTCs(int loanID);
        List<LoanAttachment> RetrieveLoanAttachments(int loanID);

        void Delete(int id);

        bool UpdateLoanStatus(LoanApprovalViewModel approvalViewModel);
    }
}
