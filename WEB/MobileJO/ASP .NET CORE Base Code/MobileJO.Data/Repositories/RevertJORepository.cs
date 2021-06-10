using Microsoft.EntityFrameworkCore;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.RevertJO;
using System;
using System.Linq;

namespace MobileJO.Data.Repositories
{
    public class RevertJORepository : BaseRepository, IRevertJORepository
    {
        public RevertJORepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        /// <summary>
        ///     Used to find a job order revert request
        /// </summary>
        /// <param name="jobOrderId">Holds the job order ID</param>
        /// <returns>Holds the retrieved RevertJobOrder data</returns>
        public RevertJobOrder FindRevertJOId(int jobOrderId)
        {
            RevertJobOrder jobOrder = null;
            if (jobOrderId > 0)
            {
                jobOrder = GetDbSet<RevertJobOrder>().FirstOrDefault(x => x.JobOrderID == jobOrderId && x.IsApproved == null);
            }

            return jobOrder;
        }

        /// <summary>
        ///     Used to check if a job order record exists
        /// </summary>
        /// <param name="jobOrderId">Holds the job order ID</param>
        /// <returns>Holds the value whether job order exists or not</returns>
        public bool IsJobOrderExists(int jobOrderId)
        {
            return GetDbSet<JobOrder>().Any(x => x.ID == jobOrderId && !x.IsDeleted);
        }

        /// <summary>
        ///     Used to check if job order status is Requested For Revert
        /// </summary>
        /// <param name="jobOrderId">Holds the job order ID</param>
        /// <returns>Holds the value whether job order status is Requested For Revert</returns>
        public bool IsJobOrderForRevert(int jobOrderId)
        {
            return GetDbSet<JobOrder>().Any(x => x.ID == jobOrderId && x.StatusID.Equals((int)Constants.Status.RequestedForRevert));
        }

        /// <summary>
        ///     Used to retrieve an IQueryable list of job order revert requests
        /// </summary>
        /// <returns>Holds a list of IQueryable job order revert requests</returns>
        public IQueryable<RevertJobOrder> RetrieveRevertJO()
        {
            return GetDbSet<RevertJobOrder>()
                .Include(us => us.UserApprovedBy)
                .Include(jo => jo.JobOrder)
                    .ThenInclude(us => us.UserCreatedBy)
                .Include(jo => jo.JobOrder)
                    .ThenInclude(app => app.ApplicationType);
        }

        /// <summary>
        ///     Used to approve/deny the job order revert request
        /// </summary>
        /// <param name="requestModel">Holds the job order revert request data</param>
        /// <returns>Holds the value whether the job order revert request was successfully approved/denied</returns>
        public bool RevertJO(RevertJORequestViewModel requestModel)
        {
            var reverted = false;

            var jobOrder = GetDbSet<JobOrder>()
                .Where(x => x.ID == requestModel.JobOrderId &&
                            !x.IsDeleted &&
                            x.StatusID.Equals((int)Constants.Status.RequestedForRevert))
                .FirstOrDefault();

            if (jobOrder != null)
            {
                var jobOrderRevert = GetDbSet<RevertJobOrder>()
                    .Where(x => x.ID == requestModel.JobOrderRevertID)
                    .FirstOrDefault();

                // Update the details of the job order revert request
                jobOrderRevert.IsApproved = requestModel.IsApproved;
                jobOrderRevert.ApprovedBy = requestModel.ApprovedBy;
                jobOrderRevert.RevertDate = DateTime.Now;

                // Update the status of the job order to Signed
                jobOrder.StatusID = (requestModel.IsApproved) ? (int)Constants.Status.Signed : (int)Constants.Status.Sent;

                reverted = true;
            }

            UnitOfWork.SaveChanges();
            return reverted;
        }

        /// <summary>
        ///     Used to retrieve job order revert request records
        /// </summary>
        /// <param name="searchModel">Holds the job order revert request search filters</param>
        /// <returns>Holds the table data of job order revert request records</returns>
        public ListViewModel SearchRevertJO(RevertJOSearchViewModel searchModel)
        {
            var assignedCases = RetrieveRevertJO()
                .Where(x => x.JobOrder.StatusID.Equals((int)Constants.Status.RequestedForRevert) &&
                            x.IsApproved == null &&
                            (string.IsNullOrEmpty(searchModel.JobOrderNumber) || x.JobOrder.JobOrderNumber.Contains(searchModel.JobOrderNumber)) &&
                            (string.IsNullOrEmpty(searchModel.ReportedBy) || x.JobOrder.UserCreatedBy.FirstName.Contains(searchModel.ReportedBy) || x.JobOrder.UserCreatedBy.LastName.Contains(searchModel.ReportedBy)))
                .OrderByDescending(x => x.ID);

            if (searchModel.Page == 0)
                searchModel.Page = 1;
            var totalCount = assignedCases.Count();
            var totalPages = (int) Math.Ceiling((double)totalCount / searchModel.PageSize);

            var results = assignedCases.Skip(searchModel.PageSize * (searchModel.Page - 1))
                .Take(searchModel.PageSize)
                .AsEnumerable()
                .Select(revertJO => new RevertJOViewModel
                {
                    ID = revertJO.ID,
                    JobOrderID = revertJO.JobOrderID,
                    JobOrderNumber = revertJO.JobOrder.JobOrderNumber,
                    ApplicationTypeName = revertJO.JobOrder.ApplicationTypeName,
                    DateTimeStart = revertJO.JobOrder.DateTimeStart,
                    DateTimeEnd = revertJO.JobOrder.DateTimeEnd,
                    ActivityDetails = revertJO.JobOrder.ActivityDetails,
                    ReportedByName = revertJO.JobOrder.ReportedByName
                })
                .ToList();

            var pagination = new
            {
                pages = totalPages,
                size = totalCount
            };

            return new ListViewModel { Pagination = pagination, Data = results };
        }
    }
}