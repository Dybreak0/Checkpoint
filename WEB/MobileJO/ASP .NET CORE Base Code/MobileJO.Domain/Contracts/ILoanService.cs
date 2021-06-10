using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.LoanApplication;
using System.Collections.Generic;
using System.Net.Http;

namespace MobileJO.Domain.Contracts
{
    public interface ILoanService
    {
        int Create(LoanDetailsViewModel loanDetailsViewModel, string attachmentPath);
        bool IsValidLoanFiles(LoanFileViewModel signatureFile,
                               List<LoanFileViewModel> attachmentsFiles);
        bool SaveFiles(int loanID,
                        List<LoanFileViewModel> fileModelList,
                        string attachmentPath,
                        string attachmentType);

        ListViewModel ListLoanApplication(LoanSearchViewModel searchListViewModel);
        ListViewModel PendingLoanApplication(LoanSearchPendingViewModel searchPendingViewModel);
        HttpResponseMessage DownloadLoanApplication(LoanSearchViewModel searchListViewModel);
        LoanDetailsViewModel FindLoanApplication(int id);
        bool IsLoanApplicationExists(int loanID);
        bool Update(LoanDetailsViewModel loanDetailsViewModel, string attachmentPath);

        void UpdateLoanFileAttachments(int jobOrderID, List<string> removedAttachments, List<LoanFileViewModel> newAttachments, string attachmentPath);
        void Delete(int id);

        bool UpdateLoanStatus(LoanApprovalViewModel approvalViewModel);
    }
}
