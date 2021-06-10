using AutoMapper;
using MobileJO.Data;
using MobileJO.Data.Contracts;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Reports;
using MobileJO.Domain.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MobileJO.Domain.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IRevertJORepository _revertJORepository;
        private readonly IMapper _mapper;

        public ReportService(IReportRepository reportRepository, IMapper mapper, IRevertJORepository revertJORepository)
        {
            _reportRepository = reportRepository;
            _revertJORepository = revertJORepository;
            _mapper = mapper;
        }

        /// <summary>
        ///      Used to call the repository layer to find a job order record
        /// </summary>
        /// <param name="id">Holds the job order id</param>
        /// <returns>Holds the job order data</returns>
        public JobOrderReportViewModel FindJobOrder(int id)
        {
            JobOrderReportViewModel jobOrderReportViewModel = null;
            var jobOrder = _reportRepository.FindJobOrder(id);

            if (jobOrder != null)
            {
                jobOrderReportViewModel = _mapper.Map<JobOrderReportViewModel>(jobOrder);
                var jobOrderRevertId = _revertJORepository.FindRevertJOId(id);
                if (jobOrderRevertId == null)
                {
                    jobOrderReportViewModel.JobOrderRevertId = Constants.Common.Zero;
                }
                else
                {
                    jobOrderReportViewModel.JobOrderRevertId = jobOrderRevertId.ID.ToString();
                }                
            }

            return jobOrderReportViewModel;
        }

        /// <summary>
        ///     Used to call the repository layer to find an assigned case record
        /// </summary>
        /// <param name="id">Holds the assigned case id</param>
        /// <returns>Holds the assigned case data</returns>
        public AssignedCasesReportViewModel FindAssignedCase(int id)
        {
            AssignedCasesReportViewModel assignedCaseReportViewModel = null;
            var assignedCase = _reportRepository.FindAssignedCase(id);

            if (assignedCase != null)
            {
                assignedCaseReportViewModel = _mapper.Map<AssignedCasesReportViewModel>(assignedCase);
            }

            return assignedCaseReportViewModel;
        }

        /// <summary>
        ///     Used to call the repository layer to retrieve assigned case records
        /// </summary>
        /// <param name="searchModel">Holds the assigned case search filters</param>
        /// <returns>Holds the list of assigned case records</returns>
        public ListViewModel SearchAssignedCases(AssignedCasesReportSearchViewModel searchModel)
        {
            return _reportRepository.SearchAssignedCase(searchModel);
        }

        /// <summary>
        ///     Used to call the repository layer to retrieve job order records
        /// </summary>
        /// <param name="searchModel">Holds the job order search filters</param>
        /// <returns>Holds the list of job order records</returns>
        public ListViewModel SearchJobOrder(JobOrderReportSearchViewModel searchModel)
        {
            return _reportRepository.SearchJobOrder(searchModel);
        }

        /// <summary>
        ///     Used to call the repository layer to retrieve job order client rating records
        /// </summary>
        /// <param name="searchModel">Holds the job order client rating search filters</param>
        /// <returns>Holds the list of job order client rating records</returns>
        public ListViewModel SearchJobOrderClientRating(JobOrderClientRatingReportSearchViewModel searchModel)
        {
            return _reportRepository.SearchJobOrderClientRating(searchModel);
        }

        /// <summary>
        ///     Used to generate an excel format of retrieved assigned case records
        /// </summary>
        /// <param name="searchModel">Holds the assigned case search filters</param>
        /// <returns name="assignedCasesReport">Holds the excel formatted report of assigned cases records</returns>
        public HttpResponseMessage DownloadAssignedCases(AssignedCasesReportSearchViewModel searchModel)
        {
            var assignedCasesList = _reportRepository.DownloadAssignedCase(searchModel);
            var assignedCasesReport = new HttpResponseMessage();

            if (assignedCasesList != null && assignedCasesList.Count > 0)
            {
                var excelTable = new StringBuilder();
                var rows = new StringBuilder();

                foreach (var assignedCases in assignedCasesList)
                {
                    rows.Append(String.Format(Constants.Reports.AssignedCasesReportExcelTableRows,
                                              assignedCases.CaseNumber,
                                              assignedCases.CaseSubject,
                                              assignedCases.AccountName,
                                              assignedCases.ApplicationTypeName,
                                              assignedCases.Status));
                }

                excelTable.Append(String.Format(Constants.Reports.ExcelTable, Constants.Reports.AssignedCasesReportExcelTableHeaders, rows));
                assignedCasesReport = Helper.ExportToExcel(excelTable.ToString(),
                    String.Format(Constants.Reports.InitialExcelFilename, Constants.Reports.AssignedCasesReport));
            }
            else
            {
                assignedCasesReport = Helper.ComposeResponse(HttpStatusCode.NoContent, Constants.Common.NoRecordsFound);
            }

            return assignedCasesReport;
        }

        /// <summary>
        ///     Used to generate an excel format of retrieved job order records
        /// </summary>
        /// <param name="searchModel">Holds the job order search filters</param>
        /// <returns name="jobOrderReport">Holds the excel formatted report of job order records</returns>
        public HttpResponseMessage DownloadJobOrder(JobOrderReportSearchViewModel searchModel)
        {
            var jobOrderList = _reportRepository.DownloadJobOrder(searchModel);            
            var jobOrderReport = new HttpResponseMessage();

            if (jobOrderList != null && jobOrderList.Count > 0)
            {
                var excelTable = new StringBuilder();
                var rows = new StringBuilder();

                foreach(var jobOrder in jobOrderList)
                {
                    rows.Append(String.Format(Constants.Reports.JobOrderReportExcelTableRows,
                                              jobOrder.JobOrderNumber,
                                              jobOrder.CaseNumber,
                                              jobOrder.ActivityDetails,
                                              jobOrder.ReportedByName,
                                              jobOrder.DateTimeStart.ToShortDateString(),
                                              jobOrder.DateTimeEnd.ToShortDateString(),
                                              jobOrder.ApplicationTypeName,
                                              jobOrder.StatusName));
                }

                excelTable.Append(String.Format(Constants.Reports.ExcelTable, Constants.Reports.JobOrderReportExcelTableHeaders, rows));
                jobOrderReport = Helper.ExportToExcel(excelTable.ToString(), 
                    String.Format(Constants.Reports.InitialExcelFilename, Constants.Reports.JobOrderReport));
            }
            else
            {
                jobOrderReport = Helper.ComposeResponse(HttpStatusCode.NoContent, Constants.Common.NoRecordsFound);
            }
            
            return jobOrderReport;
        }



        /// <summary>
        ///     Used to generate an excel format of retrieved job order client rating records
        /// </summary>
        /// <param name="searchModel">Holds the job order client rating search filters</param>
        /// <returns name="jobOrderClientRatingReport">Holds the excel formatted report of job order client rating records</returns>
        public HttpResponseMessage DownloadJobOrderClientRating(JobOrderClientRatingReportSearchViewModel searchModel)
        {
            var jobOrderClientRatingList = _reportRepository.DownloadJobOrderClientRating(searchModel);
            var jobOrderClientRatingReport = new HttpResponseMessage();

            if (jobOrderClientRatingList != null && jobOrderClientRatingList.Count > 0)
            {
                var excelTable = new StringBuilder();
                var rows = new StringBuilder();

                foreach (var jobOrderClientRating in jobOrderClientRatingList)
                {
                    rows.Append(String.Format(Constants.Reports.JobOrderReportExcelTableRows,
                                              jobOrderClientRating.JobOrderNumber,
                                              jobOrderClientRating.CaseNumber,
                                              jobOrderClientRating.ApplicationTypeName,
                                              jobOrderClientRating.DateTimeStart.ToShortDateString(),
                                              jobOrderClientRating.DateTimeEnd.ToShortDateString(),
                                              jobOrderClientRating.ReportedByName,
                                              jobOrderClientRating.AccountName,
                                              jobOrderClientRating.ClientRating));
                }

                excelTable.Append(String.Format(Constants.Reports.ExcelTable, Constants.Reports.JobOrderClientRatingReportExcelTableHeaders, rows));
                jobOrderClientRatingReport = Helper.ExportToExcel(excelTable.ToString(),
                    String.Format(Constants.Reports.InitialExcelFilename, Constants.Reports.JobOrderClientRatingReport));
            }
            else
            {
                jobOrderClientRatingReport = Helper.ComposeResponse(HttpStatusCode.NoContent, Constants.Common.NoRecordsFound);
            }

            return jobOrderClientRatingReport;
        }        
    }
}