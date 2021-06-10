using Microsoft.EntityFrameworkCore;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Data.Repositories
{
    public class ReportRepository : BaseRepository, IReportRepository
    {
        public ReportRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        /// <summary>
        ///     Used to find an assigned case record
        /// </summary>
        /// <param name="id">Holds the assigned case id</param>
        /// <returns>Holds the assigned case data</returns>
        public AssignedCase FindAssignedCase(int id)
        {
            return GetDbSet<AssignedCase>()
                .Include(app => app.ApplicationType)
                .Include(acc => acc.Account)
                .Include(us => us.User)
                .Where(x => x.ID == id).FirstOrDefault();
        }

        /// <summary>
        ///     Used to find a job order record
        /// </summary>
        /// <param name="id">Holds the job order id</param>
        /// <returns>Holds the job order data</returns>
        public JobOrder FindJobOrder(int id)
        {
            return GetDbSet<JobOrder>()
                .Include(app => app.ApplicationType)
                .Include(s => s.Status)
                .Include(acc => acc.Account)
                .Include(uc => uc.UserCreatedBy)
                .Include(uu => uu.UserUpdatedBy)
                .Include(tc => tc.TaggedCase)
                    .ThenInclude(ac => ac.AssignedCases)
                .Include(jb => jb.JobOrderBillingType)
                .Include(att => att.Attachment)
                .Where(x => x.ID == id && !x.IsDeleted).FirstOrDefault();
        }

        /// <summary>
        ///     Used to retrieve an IQueryable list of assigned case records
        /// </summary>
        /// <returns>Holds an IQueryable list of assigned case records</returns>
        public IQueryable<AssignedCase> RetrieveAllAssignedCases()
        {
            return GetDbSet<AssignedCase>()
                .Include(app => app.ApplicationType)
                .Include(acc => acc.Account);
        }

        /// <summary>
        ///     Used to retrieve an IQueryable list of job order records
        /// </summary>
        /// <returns>Holds an IQueryable list of job order records</returns>
        public IQueryable<JobOrder> RetrieveAllJobOrders()
        {
            return GetDbSet<JobOrder>()
                .Include(app => app.ApplicationType)
                .Include(s => s.Status)
                .Include(acc => acc.Account)
                .Include(uc => uc.UserCreatedBy)
                .Include(uu => uu.UserUpdatedBy)
                .Include(tc => tc.TaggedCase)
                    .ThenInclude(ac => ac.AssignedCases)
                .Include(jb => jb.JobOrderBillingType)
                .Include(att => att.Attachment);
        }

        /// <summary>
        ///     Used to retrieve assigned case records from the database.
        /// </summary>
        /// <param name="searchModel">Holds the assigned case search filters</param>
        /// <returns>Holds the table data of assigned case records</returns>
        public ListViewModel SearchAssignedCase(AssignedCasesReportSearchViewModel searchModel)
        {
            var assignedCases = RetrieveAllAssignedCases()
                .Where(x => (searchModel.ApplicationType == 0 || x.ApplicationTypeID == searchModel.ApplicationType) && 
                            (string.IsNullOrEmpty(searchModel.CaseNumber) || Convert.ToString(x.CaseNumber).Contains(searchModel.CaseNumber)))
                .OrderByDescending(x => x.ID);

            if (searchModel.Page == 0)
                searchModel.Page = 1;
            var totalCount = assignedCases.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchModel.PageSize);

            var results = assignedCases.Skip(searchModel.PageSize * (searchModel.Page - 1))
                .Take(searchModel.PageSize)
                .AsEnumerable()
                .Select(assignedCase => new AssignedCasesReportViewModel
                {
                    ID = assignedCase.ID,
                    CaseNumber = Convert.ToString(assignedCase.CaseNumber),
                    CaseSubject = assignedCase.CaseSubject,
                    ApplicationTypeName = assignedCase.ApplicationType.ApplicationName,
                    AccountName = assignedCase.AccountName,
                    Status = assignedCase.Status
                })
                .ToList();

            var pagination = new
            {
                pages = totalPages,
                size = totalCount
            };

            return new ListViewModel { Pagination = pagination, Data = results };
        }

        /// <summary>
        ///     Used to retrieve assigned case records from the database to be used for download into an excel file
        /// </summary>
        /// <param name="searchModel">Holds the assigned case search filters</param>
        /// <returns>Holds the table data of assigned case records</returns>
        public List<AssignedCasesReportViewModel> DownloadAssignedCase(AssignedCasesReportSearchViewModel searchModel)
        {
            var assignedCases = RetrieveAllAssignedCases()
                .Where(x => (searchModel.ApplicationType == 0 || x.ApplicationTypeID == searchModel.ApplicationType) &&
                            (string.IsNullOrEmpty(searchModel.CaseNumber) || Convert.ToString(x.CaseNumber).Contains(searchModel.CaseNumber)))
                .OrderByDescending(x => x.ID)
                .AsEnumerable()
                .Select(assignedCase => new AssignedCasesReportViewModel
                {
                    CaseNumber = Convert.ToString(assignedCase.CaseNumber),
                    CaseSubject = assignedCase.CaseSubject,
                    ApplicationTypeName = assignedCase.ApplicationType.ApplicationName,
                    AccountName = assignedCase.AccountName,
                    Status = assignedCase.Status
                })
                .ToList();

            return assignedCases;
        }

        /// <summary>
        ///     Used to retrieve job order records from the database.
        /// </summary>
        /// <param name="searchModel">Holds the job order search filters</param>
        /// <returns>Holds the table data of job order records</returns>
        public ListViewModel SearchJobOrder(JobOrderReportSearchViewModel searchModel)
        {           
            var jobOrders = RetrieveAllJobOrders()
                .Where(x => (x.IsDeleted == false) && 
                            (searchModel.Status == 0 || x.StatusID == searchModel.Status) &&
                            (searchModel.ApplicationType == 0 || x.ApplicationTypeID == searchModel.ApplicationType) &&
                            (string.IsNullOrEmpty(searchModel.ReportedBy) || (x.UserCreatedBy.FirstName + " " + x.UserCreatedBy.LastName).Trim().Contains(searchModel.ReportedBy.Trim())) &&
                            (string.IsNullOrEmpty(searchModel.JobOrderNumber) || x.JobOrderNumber.Contains(searchModel.JobOrderNumber)) &&
                            (string.IsNullOrEmpty(searchModel.CaseNumber) || x.TaggedCase.Any(c => Convert.ToString(c.AssignedCases.CaseNumber).Contains(searchModel.CaseNumber))) &&
                            (x.DateTimeStart >= searchModel.JobOrderDateFrom && x.DateTimeEnd < searchModel.JobOrderDateTo.AddDays(1)))
                .OrderByDescending(x => x.ID);

            if (searchModel.Page == 0)
                searchModel.Page = 1;
            var totalCount = jobOrders.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchModel.PageSize);

            var results = jobOrders.Skip(searchModel.PageSize * (searchModel.Page - 1))
                .Take(searchModel.PageSize)
                .AsEnumerable()
                .Select(jobOrder => new JobOrderReportViewModel {
                    ID = jobOrder.ID,
                    JobOrderNumber = jobOrder.JobOrderNumber,
                    JobOrderSubject = jobOrder.JobOrderSubject,
                    CaseNumber = (jobOrder.TaggedCase != null) ? string.Join(Constants.Common.Comma, jobOrder.TaggedCase.Select(x => x.AssignedCases.CaseNumber)) : string.Empty,
                    ActivityDetails = jobOrder.ActivityDetails,
                    ReportedByName = jobOrder.ReportedByName,
                    ApplicationTypeName = jobOrder.ApplicationType.ApplicationName,
                    StatusName = jobOrder.Status.Status,
                    DateTimeStart = jobOrder.DateTimeStart,
                    DateTimeEnd = jobOrder.DateTimeEnd
                })
                .ToList();

            var pagination = new
            {
                pages = totalPages,
                size = totalCount
            };

            return new ListViewModel { Pagination = pagination, Data = results };
        }

        /// <summary>
        ///     Used to retrieve job order records from the database to be used for download into an excel file
        /// </summary>
        /// <param name="searchModel">Holds the job order search filters</param>
        /// <returns>Holds the table data of job order records</returns>
        public List<JobOrderReportViewModel> DownloadJobOrder(JobOrderReportSearchViewModel searchModel)
        {
            var jobOrders = RetrieveAllJobOrders()
                .Where(x => (x.IsDeleted == false) &&
                            (searchModel.Status == 0 || x.StatusID == searchModel.Status) &&
                            (searchModel.ApplicationType == 0 || x.ApplicationTypeID == searchModel.ApplicationType) &&
                            (string.IsNullOrEmpty(searchModel.ReportedBy) || (x.UserCreatedBy.FirstName + " " + x.UserCreatedBy.LastName).Trim().Contains(searchModel.ReportedBy.Trim())) &&
                            (string.IsNullOrEmpty(searchModel.JobOrderNumber) || x.JobOrderNumber.Contains(searchModel.JobOrderNumber)) &&
                            (string.IsNullOrEmpty(searchModel.CaseNumber) || x.TaggedCase.Any(c => Convert.ToString(c.AssignedCases.CaseNumber).Contains(searchModel.CaseNumber))) &&
                            (x.DateTimeStart >= searchModel.JobOrderDateFrom && x.DateTimeEnd < searchModel.JobOrderDateTo.AddDays(1)))
                .OrderByDescending(x => x.ID);

            var results = jobOrders
                .OrderByDescending(x => x.ID)
                .AsEnumerable()
                .Select(jobOrder => new JobOrderReportViewModel
                {
                    JobOrderNumber = jobOrder.JobOrderNumber,
                    CaseNumber = (jobOrder.TaggedCase != null) ? string.Join(Constants.Common.Comma, jobOrder.TaggedCase.Select(x => x.AssignedCases.CaseNumber)) : string.Empty,
                    ActivityDetails = jobOrder.JobOrderSubject,
                    ReportedByName = jobOrder.ReportedByName,
                    ApplicationTypeName = jobOrder.ApplicationType.ApplicationName,
                    StatusName = jobOrder.Status.Status,
                    DateTimeStart = jobOrder.DateTimeStart.Date,
                    DateTimeEnd = jobOrder.DateTimeEnd.Date
                })
                .ToList();

            return results;
        }

        /// <summary>
        ///     Used to retrieve job order client rating records from the database.
        /// </summary>
        /// <param name="searchModel">Holds the job order client rating search filters</param>
        /// <returns>Holds the table data of job order client rating records</returns>
        public ListViewModel SearchJobOrderClientRating(JobOrderClientRatingReportSearchViewModel searchModel)
        {
            var jobOrderClientRatings = RetrieveAllJobOrders()
                .Where(x => (x.IsDeleted == false) &&
                            (x.StatusID != Convert.ToInt32(Constants.Status.Pending)) &&
                            (searchModel.ApplicationType == 0 || x.ApplicationTypeID == searchModel.ApplicationType) &&
                            (string.IsNullOrEmpty(searchModel.AccountName) || x.Account.Name.Contains(searchModel.AccountName)) &&
                            (string.IsNullOrEmpty(searchModel.ReportedBy) || (x.UserCreatedBy.FirstName + " " + x.UserCreatedBy.LastName).Contains(searchModel.ReportedBy.Trim())) &&
                            (string.IsNullOrEmpty(searchModel.JobOrderNumber) || x.JobOrderNumber.Contains(searchModel.JobOrderNumber)) &&
                            (string.IsNullOrEmpty(searchModel.CaseNumber) || x.TaggedCase.Any(c => Convert.ToString(c.AssignedCases.CaseNumber).Contains(searchModel.CaseNumber))) &&
                            (x.DateTimeStart >= searchModel.JobOrderDateFrom && x.DateTimeEnd < searchModel.JobOrderDateTo.AddDays(1)))
                .OrderByDescending(x => x.ID);

            if (searchModel.Page == 0)
                searchModel.Page = 1;
            var totalCount = jobOrderClientRatings.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchModel.PageSize);

            var results = jobOrderClientRatings.Skip(searchModel.PageSize * (searchModel.Page - 1))
                .Take(searchModel.PageSize)
                .AsEnumerable()
                .Select(jobOrder => new JobOrderClientRatingReportViewModel
                {
                    ID = jobOrder.ID,
                    JobOrderNumber = jobOrder.JobOrderNumber,
                    CaseNumber = string.Join(Constants.Common.Comma, jobOrder.TaggedCase.Select(x => x.AssignedCases.CaseNumber)),
                    ReportedByName = jobOrder.ReportedByName,
                    ApplicationTypeName = jobOrder.ApplicationType.ApplicationName,
                    AccountName = jobOrder.Account.Name,
                    ClientRating = jobOrder.ClientRating,
                    DateTimeStart = jobOrder.DateTimeStart,
                    DateTimeEnd = jobOrder.DateTimeEnd
                })
                .ToList();

            var pagination = new
            {
                pages = totalPages,
                size = totalCount
            };

            return new ListViewModel { Pagination = pagination, Data = results };
        }

        /// <summary>
        ///     Used to retrieve job order client rating records from the database to be used for download into an excel file
        /// </summary>
        /// <param name="searchModel">Holds the job order client rating search filters</param>
        /// <returns>Holds the table data of job order client rating records</returns>
        public List<JobOrderClientRatingReportViewModel> DownloadJobOrderClientRating(JobOrderClientRatingReportSearchViewModel searchModel)
        {
            var jobOrderClientRatings = RetrieveAllJobOrders()
                .Where(x => (x.IsDeleted == false) &&
                            (x.StatusID != Convert.ToInt32(Constants.Status.Pending)) &&
                            (searchModel.ApplicationType == 0 || x.ApplicationTypeID == searchModel.ApplicationType) &&
                            (string.IsNullOrEmpty(searchModel.AccountName) || x.Account.Name.Contains(searchModel.AccountName)) &&
                            (string.IsNullOrEmpty(searchModel.ReportedBy) || (x.UserCreatedBy.FirstName + " " + x.UserCreatedBy.LastName).Contains(searchModel.ReportedBy.Trim())) &&
                            (string.IsNullOrEmpty(searchModel.JobOrderNumber) || x.JobOrderNumber.Contains(searchModel.JobOrderNumber)) &&
                            (string.IsNullOrEmpty(searchModel.CaseNumber) || x.TaggedCase.Any(c => Convert.ToString(c.AssignedCases.CaseNumber).Contains(searchModel.CaseNumber))) &&
                            (x.DateTimeStart >= searchModel.JobOrderDateFrom && x.DateTimeEnd < searchModel.JobOrderDateTo.AddDays(1)))
                .OrderByDescending(x => x.ID);

            var results = jobOrderClientRatings
                .OrderByDescending(x => x.ID)
                .AsEnumerable()
                .Select(jobOrder => new JobOrderClientRatingReportViewModel
                {
                    ID = jobOrder.ID,
                    JobOrderNumber = jobOrder.JobOrderNumber,
                    CaseNumber = string.Join(Constants.Common.Comma, jobOrder.TaggedCase.Select(x => x.AssignedCases.CaseNumber)),
                    ReportedByName = jobOrder.ReportedByName,
                    ApplicationTypeName = jobOrder.ApplicationType.ApplicationName,
                    AccountName = jobOrder.Account.Name,
                    ClientRating = jobOrder.ClientRating,
                    DateTimeStart = jobOrder.DateTimeStart,
                    DateTimeEnd = jobOrder.DateTimeEnd
                })
                .ToList();

            return results;
        }        
    }
}