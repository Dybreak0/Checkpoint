using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Reports;
using System.Net.Http;

namespace MobileJO.Domain.Contracts
{
    public interface IReportService
    {
        JobOrderReportViewModel FindJobOrder(int id);
        AssignedCasesReportViewModel FindAssignedCase(int id);
        ListViewModel SearchJobOrder(JobOrderReportSearchViewModel searchModel);
        ListViewModel SearchAssignedCases(AssignedCasesReportSearchViewModel searchModel);
        ListViewModel SearchJobOrderClientRating(JobOrderClientRatingReportSearchViewModel searchModel);
        HttpResponseMessage DownloadJobOrder(JobOrderReportSearchViewModel searchModel);
        HttpResponseMessage DownloadAssignedCases(AssignedCasesReportSearchViewModel searchModel);
        HttpResponseMessage DownloadJobOrderClientRating(JobOrderClientRatingReportSearchViewModel searchModel);
    }
}