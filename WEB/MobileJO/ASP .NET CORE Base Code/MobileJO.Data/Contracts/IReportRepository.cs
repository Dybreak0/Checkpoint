using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Reports;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Data.Contracts
{
    public interface IReportRepository
    {
        JobOrder FindJobOrder(int id);
        AssignedCase FindAssignedCase(int id);
        IQueryable<JobOrder> RetrieveAllJobOrders();
        IQueryable<AssignedCase> RetrieveAllAssignedCases();
        ListViewModel SearchJobOrder(JobOrderReportSearchViewModel searchModel);
        ListViewModel SearchAssignedCase(AssignedCasesReportSearchViewModel searchModel);
        ListViewModel SearchJobOrderClientRating(JobOrderClientRatingReportSearchViewModel searchModel);
        List<JobOrderReportViewModel> DownloadJobOrder(JobOrderReportSearchViewModel searchModel);
        List<AssignedCasesReportViewModel> DownloadAssignedCase(AssignedCasesReportSearchViewModel searchModel);
        List<JobOrderClientRatingReportViewModel> DownloadJobOrderClientRating(JobOrderClientRatingReportSearchViewModel searchModel);
    }
}