using Microsoft.EntityFrameworkCore;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.AssignedCase;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.JobOrder;
using MobileJO.Data.ViewModels.RevertJO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Data.Repositories
{
    public class JobOrderRepository : BaseRepository, IJobOrderRepository
    {
        /// <summary>
        ///     Constructor for IUnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public JobOrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Retrieves all job orders
        /// </summary>
        /// <returns></returns>
        public List<JobOrder> RetrieveJobOrdersList()
        {
            return GetDbSet<JobOrder>().ToList();
        }

        /// <summary>
        /// Finds a job order by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JobOrder Find(int id)
        {
            return GetDbSet<JobOrder>()
                .Where(x => x.ID == id)                
                .FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all application types
        /// </summary>
        /// <returns></returns>
        public List<ApplicationType> RetrieveApplicationTypes()
        {
            return GetDbSet<ApplicationType>().ToList();
        }

        /// <summary>
        /// Retrieves all accounts (clients)
        /// </summary>
        /// <returns></returns>
        public List<Account> RetrieveAccountsList()
        {
            return GetDbSet<Account>()
                .Where(x => x.IsActive == true)                            
                .ToList();            
        }

        /// <summary>
        /// Retrieves all billing types
        /// </summary>
        /// <returns></returns>
        public List<BillingType> RetrieveBillingTypes()
        {
            return GetDbSet<BillingType>().ToList();
        }

        /// <summary>
        /// Retrieves aspplication type cases assigned to a user 
        /// </summary>
        /// <param name="assignedTo"></param>
        /// <param name="applicationTypeId"></param>
        /// <returns></returns>
        public List<AssignedCasesViewModel> RetrieveAssignedCases(string assignedTo, int applicationTypeId, int accountId)
        {
            return GetDbSet<AssignedCase>()
                .Include(user => user.User)
                .Where(x => x.AssignedUserID.ToString() == assignedTo && 
                       x.ApplicationTypeID == applicationTypeId && 
                       x.AccountID == accountId && 
                       x.Status.ToLower() != Constants.Common.Closed.ToLower())
                       .Select(x => new AssignedCasesViewModel
                       {
                           ID = x.ID,
                           CaseNumber = x.CaseNumber,
                           CaseSubject = x.CaseSubject,
                           Status = x.Status,
                           ApplicationTypeID = x.ApplicationTypeID,
                           AccountID = x.AccountID,
                           Priority = x.Priority,
                           AssignedUserID = x.AssignedUserID,
                           AssignedTo = string.Format(Constants.Common.NameFormat, x.User.FirstName, x.User.LastName),
                           Description = x.Description,
                           CreatedBy = x.CreatedBy,
                           UpdatedBy = x.UpdatedBy,
                           CreatedDate = x.CreatedDate,
                           UpdatedDate = x.UpdatedDate
                       })
                      .ToList();
        }

        /// <summary>
        /// Retrieves tagged cases of a job order
        /// </summary>
        /// <param name="jobOrderID"></param>
        /// <returns></returns>
        public List<TaggedCase> RetrieveTaggedCases(int jobOrderID)
        {
            return GetDbSet<TaggedCase>().Where(i => i.JobOrderID == jobOrderID).ToList();
        }

        /// <summary>
        /// Retrieves billing types of a job order
        /// </summary>
        /// <param name="jobOrderID"></param>
        /// <returns></returns>
        public List<JobOrderBillingType> RetrieveJOBillingTypes(int jobOrderID)
        {
            return GetDbSet<JobOrderBillingType>().Where(i => i.JobOrderID == jobOrderID).ToList();
        }

        /// <summary>
        /// Retrieves attachments of a job order
        /// </summary>
        /// <param name="jobOrderID"></param>
        /// <returns></returns>
        public List<Attachment> RetrieveJOAttachments(int jobOrderID)
        {
            return GetDbSet<Attachment>().Where(i => i.JobOrderID == jobOrderID).ToList();
        }

        /// <summary>
        /// Retrieves all cases of a user from all types of application 
        /// </summary>
        /// <param name="assignedTo"></param>
        /// <returns></returns>
        public List<AssignedCasesViewModel> RetrieveUserCases(int assignedTo)
        {
            return GetDbSet<AssignedCase>()
                .Include(user => user.User)
                .Where(x => x.AssignedUserID == assignedTo && 
                       x.Status != Constants.Common.Closed)
                       .Select(x => new AssignedCasesViewModel
                       {
                            ID = x.ID,
                            CaseNumber = x.CaseNumber,
                            CaseSubject = x.CaseSubject,
                            Status = x.Status,                            
                            ApplicationTypeID = x.ApplicationTypeID,
                            AccountID = x.AccountID,
                            Priority = x.Priority,
                            AssignedUserID = x.AssignedUserID,
                            AssignedTo = string.Format(Constants.Common.NameFormat, x.User.FirstName, x.User.LastName),
                            Description = x.Description,                            
                            CreatedBy = x.CreatedBy,
                            UpdatedBy = x.UpdatedBy,
                            CreatedDate = x.CreatedDate,
                            UpdatedDate = x.UpdatedDate
                        })
                       .ToList();
        }

        /// <summary>
        /// Deletes job order cases
        /// </summary>
        /// <param name="jobOrderID"></param>
        public void DeleteJOCases(int jobOrderID)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var jobOrderCases = RetrieveTaggedCases(jobOrderID);

                    if (jobOrderCases.Count > 0)
                    {
                        GetDbSet<TaggedCase>().RemoveRange(jobOrderCases);
                        UnitOfWork.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }
        }

        /// <summary>
        /// Deletes job order billing types
        /// </summary>
        /// <param name="jobOrderID"></param>
        public void DeleteJOBillingTypes(int jobOrderID)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var jobOrderBilllingTypes = RetrieveJOBillingTypes(jobOrderID);

                    if (jobOrderBilllingTypes.Count > 0)
                    {
                        GetDbSet<JobOrderBillingType>().RemoveRange(jobOrderBilllingTypes);
                        UnitOfWork.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }

        }

        /// <summary>
        /// Deletes job order attachments
        /// </summary>
        /// <param name="jobOrderID"></param>
        /// <param name="filenames"></param>
        public void DeleteAttachments(int jobOrderID, IEnumerable<string> filenames)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var jobOrderAttachments = new List<Attachment>();

                    foreach (var filename in filenames)
                    {
                        var attachment = GetDbSet<Attachment>()
                                          .Where(i => i.JobOrderID == jobOrderID && i.Filename == filename)
                                          .FirstOrDefault();

                        if (attachment != null)
                        {
                            jobOrderAttachments.Add(attachment);
                        }
                    }

                    if (jobOrderAttachments.Count > 0)
                    {
                        GetDbSet<Attachment>().RemoveRange(jobOrderAttachments);
                        UnitOfWork.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }
        }


        /// <summary>
        /// Creates a job order record
        /// </summary>
        /// <param name="jobOrder"></param>
        /// <param name="taggedCases"></param>
        /// <param name="joBillingTypes"></param>
        /// <param name="joAttachments"></param>
        /// <param name="signatureFilename"></param>
        /// <returns></returns>
        public int Create(JobOrder jobOrder,
                           List<TaggedCase> taggedCases,
                           List<JobOrderBillingType> joBillingTypes,
                           List<Attachment> joAttachments,
                           string signatureFilename = null)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    int insert_id = 0;

                    GetDbSet<JobOrder>().Add(jobOrder);
                    UnitOfWork.SaveChanges();

                    insert_id = jobOrder.ID;

                    JobOrder joForUpdate = Find(insert_id);

                    if (joForUpdate != null) 
                    {
                        joForUpdate.JobOrderNumber = string.Concat(Constants.JobOrder.JobOrderNumberPrefix, 
                                                                   insert_id.ToString(Constants.Common.JobOrderNumberFormat));

                        if (!string.IsNullOrEmpty(signatureFilename))
                        {
                            joForUpdate.ClientSignature = string.Concat(insert_id, signatureFilename);
                        }
                        
                        UnitOfWork.SaveChanges();
                    }

                    if(taggedCases.Count() > 0)
                    {
                        taggedCases.Select(c => { c.JobOrderID = insert_id; return c; }).ToList();
                        GetDbSet<TaggedCase>().AddRange(taggedCases);
                        UnitOfWork.SaveChanges();
                    }

                    if(joBillingTypes.Count() > 0)
                    {
                        joBillingTypes.Select(b => { b.JobOrderID = insert_id; return b; }).ToList();
                        GetDbSet<JobOrderBillingType>().AddRange(joBillingTypes);
                        UnitOfWork.SaveChanges();
                    }
                    
                    if(joAttachments.Count > 0)
                    {
                        joAttachments.Select(a => { a.JobOrderID = insert_id; return a; }).ToList();
                        GetDbSet<Attachment>().AddRange(joAttachments);
                        UnitOfWork.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    return insert_id;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// Updates a job order record 
        /// </summary>
        /// <param name="jobOrder"></param>
        /// <param name="newJOCases"></param>
        /// <param name="newJOAttachments"></param>
        /// <param name="newJOBillingTypes"></param>
        /// <returns></returns>
        public bool Update(JobOrder jobOrder,
                           List<TaggedCase> newJOCases,
                           List<Attachment> newJOAttachments,
                           List<string> removedJOAttachments,
                           List<JobOrderBillingType> newJOBillingTypes)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    int id = jobOrder.ID;

                    JobOrder jobOrderUpdate = Find(id);

                    jobOrderUpdate.JobOrderNumber    = jobOrder.JobOrderNumber;
                    jobOrderUpdate.JobOrderSubject   = jobOrder.JobOrderSubject;
                    jobOrderUpdate.StatusID          = jobOrder.StatusID;
                    jobOrderUpdate.AccountID         = jobOrder.AccountID;
                    jobOrderUpdate.ApplicationTypeID = jobOrder.ApplicationTypeID;
                    jobOrderUpdate.Branch            = jobOrder.Branch;
                    jobOrderUpdate.DateTimeStart     = jobOrder.DateTimeStart;
                    jobOrderUpdate.DateTimeEnd       = jobOrder.DateTimeEnd;
                    jobOrderUpdate.ActivityDetails   = jobOrder.ActivityDetails;
                    jobOrderUpdate.RootCauseAnalysis = jobOrder.RootCauseAnalysis;
                    jobOrderUpdate.NextStep          = jobOrder.NextStep;
                    jobOrderUpdate.PreventiveAction  = jobOrder.PreventiveAction;
                    jobOrderUpdate.Remarks           = jobOrder.Remarks;
                    jobOrderUpdate.Attendees         = jobOrder.Attendees;
                    jobOrderUpdate.IsBilled          = jobOrder.IsBilled;
                    jobOrderUpdate.IsCollaterals     = jobOrder.IsCollaterals;
                    jobOrderUpdate.IsFixed           = jobOrder.IsFixed;
                    jobOrderUpdate.IsSatisfied       = jobOrder.IsSatisfied;
                    jobOrderUpdate.ClientSignature   = jobOrder.ClientSignature;
                    jobOrderUpdate.ClientRating      = jobOrder.ClientRating;
                    jobOrderUpdate.UpdatedBy         = jobOrder.UpdatedBy;
                    jobOrderUpdate.UpdatedDate       = jobOrder.UpdatedDate;
                    jobOrderUpdate.LastSyncDate      = jobOrder.LastSyncDate;
                    UnitOfWork.SaveChanges();

                    var jobOrderCases         = RetrieveTaggedCases(id);
                    var jobOrderBilllingTypes = RetrieveJOBillingTypes(id);
                    var jobOrderAttachments   = RetrieveJOAttachments(id);

                    if (jobOrderCases.Count > 0)
                    {
                        GetDbSet<TaggedCase>().RemoveRange(jobOrderCases);
                        UnitOfWork.SaveChanges();
                    }

                    if (jobOrderBilllingTypes.Count > 0)
                    {
                        GetDbSet<JobOrderBillingType>().RemoveRange(jobOrderBilllingTypes);
                        UnitOfWork.SaveChanges();
                    }

                    if(jobOrderAttachments.Count > 0)
                    {
                        foreach(var removed_attachment in removedJOAttachments)
                        {
                            var attachmententsForDeletion = jobOrderAttachments.Where(x => x.Filename == removed_attachment);

                            if(attachmententsForDeletion.Count() > 0)
                            {
                                GetDbSet<Attachment>().RemoveRange(attachmententsForDeletion);
                            }
                            
                            UnitOfWork.SaveChanges();
                        }                        
                    }

                    GetDbSet<TaggedCase>().AddRange(newJOCases);
                    UnitOfWork.SaveChanges();

                    GetDbSet<JobOrderBillingType>().AddRange(newJOBillingTypes);
                    UnitOfWork.SaveChanges();

                    GetDbSet<Attachment>().AddRange(newJOAttachments);
                    UnitOfWork.SaveChanges();

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }
        }

        public JobOrderViewModel FindJobOrder(int id)
        {
            return GetDbSet<JobOrder>()
                .Include(acc => acc.Account)
                .Include(ap => ap.ApplicationType)
                .Include(bill => bill.JobOrderBillingType)
                .Include(tag => tag.TaggedCase)
                .Include(use => use.UserCreatedBy.FirstName)
                .Include(att => att.Attachment)
                .Where(x => x.ID == id)
                .Select(z => new JobOrderViewModel
                {
                    ID = z.ID,
                    AccountName = z.Account.Name,
                    JobOrderNumber = z.JobOrderNumber,
                    JobOrderSubject = z.JobOrderSubject,
                    Branch = z.Branch,
                    DateTimeStart = DateTime.Parse(Helper.GetFormattedDate(z.DateTimeStart)),
                    DateTimeEnd = DateTime.Parse(Helper.GetFormattedDate(z.DateTimeEnd)),
                    ApplicationName = z.ApplicationType.ApplicationName,
                    ActivityDetails = z.ActivityDetails,
                    RootCauseAnalysis = z.RootCauseAnalysis,
                    NextStep = z.NextStep,
                    PreventiveAction = z.PreventiveAction,
                    Remarks = z.Remarks,
                    Attendees = z.Attendees,
                    Is_Billed = z.IsBilled,
                    Is_Collaterals = z.IsCollaterals,
                    ClientSignature = z.ClientSignature,
                    Is_Fixed = z.IsFixed,
                    Is_Satisfied = z.IsSatisfied,
                    StatusID = z.StatusID,
                    CreatedDate = z.CreatedDate,
                    CreatedBy = z.CreatedBy,
                    UpdatedDate = z.UpdatedDate,
                    UpdatedBy = z.UpdatedBy,
                    IsDeleted = z.IsDeleted,
                    ClientRating = z.ClientRating,
                })
                .FirstOrDefault();
        }

        /// <summary>
        ///     Used to retrieve the list of job orders
        /// </summary>
        /// <returns></returns>
        public IQueryable<JobOrder> RetrieveAll()
        {
            return GetDbSet<JobOrder>()
                .Include(status => status.Status.Status)
                .Include(app => app.ApplicationTypeName);
        }

        /// <summary>
        ///     Used to retrieve job orders based on filters
        /// </summary>
        /// <returns></returns>
        public ListViewModel Search(JobOrderSearchViewModel searchModel)
        {

            var sortDir = ((!string.IsNullOrEmpty(searchModel.SortOrder) && searchModel.SortOrder.Equals(Constants.Common.Desc))) ?
            Constants.SortDirection.Descending : Constants.SortDirection.Ascending;


            var joborders = RetrieveAll()
                .Include(user => user.Account)
                .Where(x => (string.IsNullOrEmpty(searchModel.JobOrderNumber) || x.JobOrderNumber.Contains(Convert.ToString(searchModel.JobOrderNumber))) &&
                            (string.IsNullOrEmpty(searchModel.Status) || x.StatusID.ToString().Contains(searchModel.Status.ToString())) &&
                            (string.IsNullOrEmpty(searchModel.ApplicationType) || x.ApplicationType.ToString().Contains(searchModel.ApplicationType.ToString())) &&
                            (x.IsDeleted == searchModel.IsDeleted) && (x.CreatedBy == searchModel.CreatedBy)
                            )
                .Select(x => new JobOrderListViewModel
                {
                    ID = x.ID,
                    JobOrderNumber = x.JobOrderNumber,
                    JobOrderSubject = x.JobOrderSubject,
                    ApplicationType = x.ApplicationType.ApplicationName,
                    StatusID = x.Status.Status,
                    IsDeleted = x.IsDeleted,
                    Color = Helper.GetStatusColor(x.Status.Status),
                    CreatedBy = x.CreatedBy

                })
                //.OrderByPropertyName(Constants.Common.JobOrderNumber, sortDir);
                .OrderByDescending(x => x.JobOrderNumber);

            var results = new List<JobOrderListViewModel>();
            var totalCount = joborders.Count();
            var totalPages = 1;
            if (searchModel.PageSize > 0)
            {
                totalPages = (int)Math.Ceiling((double)totalCount / searchModel.PageSize);
                results = joborders.Skip(searchModel.PageSize * (searchModel.Page - 1))
                .Take(searchModel.PageSize)
                .AsEnumerable().ToList();
            }
            else
            {
                results = joborders.ToList();
            }

            var pagination = new
            {
                pages = totalPages,
                size = totalCount
            };

            return new ListViewModel { Pagination = pagination, Data = results };
        }

        /// <summary>
        ///     Used to retrieve the list cases
        /// </summary>
        /// <returns></returns>
        public IQueryable<TaggedCase> RetrieveAllCases()
        {
            return GetDbSet<TaggedCase>()
                .Include(jo => jo.JobOrder)
                   .ThenInclude(app => app.ApplicationType)
                .Include(jo => jo.JobOrder)
                   .ThenInclude(acc => acc.Account)
                .Include(x => x.AssignedCases);
        }

        /// <summary>
        ///     Used to soft delete a job order
        /// </summary>
        /// <returns></returns>
        public void Delete(int id)
        {
            JobOrder jobOrderDB = Context.JobOrder.Find(id);

            jobOrderDB.UpdatedBy = 1;
            jobOrderDB.UpdatedDate = DateTime.Now;
            jobOrderDB.IsDeleted = true;
            UnitOfWork.SaveChanges();
        }

        public void UpdateStatus(int id)
        {
            JobOrder jobOrderDB = Context.JobOrder.Find(id);

            jobOrderDB.StatusID = Constants.Common.RequestRevertValue;
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Used to retrieve create a job order revert request
        /// </summary>
        /// <returns></returns>
        public void CreateJORevertRequest(int id)
        {
            RevertJobOrder jobRequestRevert = new RevertJobOrder();
            jobRequestRevert.JobOrderID = id;
            jobRequestRevert.RequestDate = DateTime.Now;
            jobRequestRevert.IsUsed = true;
            GetDbSet<RevertJobOrder>().Add(jobRequestRevert);

            UpdateStatus(id);
           
            UnitOfWork.SaveChanges();

        }

       

        /// <summary>
        ///     Used to retrieve all application types
        /// </summary>
        /// <returns></returns>
        public List<string> GetApplicationType()
        {
            return GetDbSet<ApplicationType>()
                .Select(x => x.ApplicationName)
                .ToList();
        }

        /// <summary>
        ///     Used to retrieve all job order status
        /// </summary>
        /// <returns></returns>
        public List<string> GetJobOrderStatusList()
        {
            return GetDbSet<JobOrderStatus>()
                .Select(x => x.Status)
                .Distinct()
                .ToList();
        }

        /// <summary>
        ///     Used to retrieve all tagged cases of a job order
        /// </summary>
        /// <returns></returns>
        public List<TaggedCasesListViewModel> GetTaggedCases(ViewModels.JobOrder.TaggedCasesViewModel taggedCasesViewModel)
        {
            var tagged = RetrieveAllCases()
                .Where(x => x.JobOrderID == taggedCasesViewModel.ID)
                .Select(x => new TaggedCasesListViewModel
                {
                    ID = x.CaseID,
                    CaseNumber = x.AssignedCases.CaseNumber.ToString(),
                    ApplicationType = x.JobOrder.ApplicationType.ApplicationName,
                    Status = x.JobOrder.Status.Status,
                    CaseSubject = x.AssignedCases.CaseSubject,
                    AccountName = x.JobOrder.Account.Name

                }).ToList();

            return tagged;
        }

        public AssignedCasesViewModel FindCase(int id)
        {
            return GetDbSet<AssignedCase>()
                .Include(app => app.ApplicationType)

                .Include(acc => acc.Account)
                .Include(user => user.User)
                .Where(x => x.ID == id)
                .Select(z => new AssignedCasesViewModel
                {
                    CaseNumber = z.CaseNumber,
                    Status = z.Status,
                    ApplicationType = z.ApplicationType.ApplicationName,
                    CaseSubject = z.CaseSubject,
                    Priority = z.Priority,
                    AccountName = z.Account.Name,
                    Description = z.Description,
                    AssignedTo = string.Format(Constants.Common.NameFormat, z.User.FirstName, z.User.LastName),
                    CreatedBy = string.Format(Constants.Common.DateFormatWithName, Helper.GetFormattedDate(z.CreatedDate), z.CreatedBy),
                    UpdatedBy = string.Format(Constants.Common.DateFormatWithName, Helper.GetFormattedDate(z.UpdatedDate), z.UpdatedBy)

                })
                .FirstOrDefault();
        }

        public JobOrderRevertViewModel GetRequestCount(int id)
        {
            return GetDbSet<RevertJobOrder>()
                .Where(x => x.JobOrderID == id)
                .Select(q => new JobOrderRevertViewModel
                {
                   IsUsed = q.IsUsed
                })
                .FirstOrDefault();
        }

        /// <summary>
        ///     Used to retrieve all attachments
        /// </summary>
        /// <returns></returns>
        public IQueryable<Attachment> RetrieveAllAttachments()
        {
            return GetDbSet<Attachment>();
        }

        /// <summary>
        ///     Used to retrieve attachments of a particular job order
        /// </summary>
        /// <returns></returns>
        public List<AttachmentListViewModel> GetAttachments(int id)
        {
            var attachments = RetrieveAllAttachments()
                .Where(z => z.JobOrderID == id)
                .Select(x => new AttachmentListViewModel
                {
                    ID = x.ID,
                    JobOrderID = x.JobOrderID.ToString(),
                    Filename = x.Filename,

                }).ToList();

            return attachments;
        }

        public IQueryable<JobOrderBillingType> RetrieveAllBillingType()
        {
            return GetDbSet<JobOrderBillingType>()
                 .Include(x => x.BillingType);
        }

        public List<JobOrderBillingTypeListViewModel> GetBillingType(int id)
        {
            var billingtypes = RetrieveAllBillingType()
                .Where(z => z.JobOrderID == id)
                .Select(x => new JobOrderBillingTypeListViewModel
                {
                    ID = x.ID,
                    JobOrderID = x.JobOrderID.ToString(),
                    BillingTypeID = x.BillingTypeID,
                    BillingTypeName = x.BillingType.BillingTypeName,
                }).ToList();

            return billingtypes;
        }

        public List<JobOrderViewModel> RetrieveUserJobOrders(int userId)
        {
            return GetDbSet<JobOrder>()
                .Where(x => x.CreatedBy == userId && x.IsDeleted == false)
                .Select(x => new JobOrderViewModel
                {
                    ID = x.ID,
                    JobOrderSubject = x.JobOrderSubject,
                    JobOrderNumber = x.JobOrderNumber,
                    Branch = x.Branch,
                    AccountID = x.AccountID,
                    ApplicationTypeID = x.ApplicationTypeID,
                    DateTimeStart = x.DateTimeStart,
                    DateTimeEnd = x.DateTimeEnd,
                    ActivityDetails = x.ActivityDetails,
                    RootCauseAnalysis = x.RootCauseAnalysis,
                    NextStep = x.NextStep,
                    PreventiveAction = x.PreventiveAction,
                    Remarks = x.Remarks,
                    Attendees = x.Attendees,
                    Is_Billed = x.IsBilled,
                    Is_Collaterals = x.IsCollaterals,
                    ClientSignature = x.ClientSignature,
                    Is_Fixed = x.IsFixed,
                    Is_Satisfied = x.IsSatisfied,
                    StatusID = x.StatusID,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,
                    IsDeleted = x.IsDeleted,
                    ClientRating = x.ClientRating,
                    LastSyncDate = x.LastSyncDate
                })
                .ToList();
        }

        public List<TaggedCase> GetAllJoCases(List<int> ids)
        {
            return GetDbSet<TaggedCase>()
                .Where(s => ids.Contains(s.JobOrderID))
                .ToList();
        }

        public List<JobOrderBillingType> GetAllJoBillingTypes(List<int> ids)
        {
            return GetDbSet<JobOrderBillingType>()
                .Where(s => ids.Contains(s.JobOrderID))
                .ToList();
        }

        public List<Attachment> GetAllJoAttachments(List<int> ids)
        {
            return GetDbSet<Attachment>()
                .Where(s => ids.Contains(s.JobOrderID))                
                .ToList();
        }

        public List<JobOrderStatus> GetAllStatus()
        {
            return GetDbSet<JobOrderStatus>().ToList();
        }

        /// <summary>
        /// Update function for sync job orders
        /// </summary>
        /// <param name="jobOrder"></param>
        /// <param name="newJOCases"></param>
        /// <param name="newJOAttachments"></param>
        /// <param name="newJOBillingTypes"></param>
        /// <returns></returns>
        public bool UpdateSync(JobOrder jobOrder,
                           List<TaggedCase> newJOCases,
                           List<Attachment> newJOAttachments,
                           List<string> removedJOAttachments,
                           List<JobOrderBillingType> newJOBillingTypes)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    int id = jobOrder.ID;

                    JobOrder jobOrderUpdate = Find(id);

                    jobOrderUpdate.JobOrderNumber = jobOrder.JobOrderNumber;
                    jobOrderUpdate.JobOrderSubject = jobOrder.JobOrderSubject;
                    jobOrderUpdate.StatusID = jobOrder.StatusID;
                    jobOrderUpdate.AccountID = jobOrder.AccountID;
                    jobOrderUpdate.ApplicationTypeID = jobOrder.ApplicationTypeID;
                    jobOrderUpdate.Branch = jobOrder.Branch;
                    jobOrderUpdate.DateTimeStart = jobOrder.DateTimeStart;
                    jobOrderUpdate.DateTimeEnd = jobOrder.DateTimeEnd;
                    jobOrderUpdate.ActivityDetails = jobOrder.ActivityDetails;
                    jobOrderUpdate.RootCauseAnalysis = jobOrder.RootCauseAnalysis;
                    jobOrderUpdate.NextStep = jobOrder.NextStep;
                    jobOrderUpdate.PreventiveAction = jobOrder.PreventiveAction;
                    jobOrderUpdate.Remarks = jobOrder.Remarks;
                    jobOrderUpdate.Attendees = jobOrder.Attendees;
                    jobOrderUpdate.IsBilled = jobOrder.IsBilled;
                    jobOrderUpdate.IsCollaterals = jobOrder.IsCollaterals;
                    jobOrderUpdate.IsFixed = jobOrder.IsFixed;
                    jobOrderUpdate.IsSatisfied = jobOrder.IsSatisfied;
                    jobOrderUpdate.IsDeleted = jobOrder.IsDeleted;
                    jobOrderUpdate.ClientSignature = jobOrder.ClientSignature;
                    jobOrderUpdate.ClientRating = jobOrder.ClientRating;
                    jobOrderUpdate.UpdatedBy = jobOrder.UpdatedBy;
                    jobOrderUpdate.UpdatedDate = jobOrder.UpdatedDate;
                    jobOrderUpdate.LastSyncDate = jobOrder.LastSyncDate;
                    UnitOfWork.SaveChanges();

                    var jobOrderCases = RetrieveTaggedCases(id);
                    var jobOrderBilllingTypes = RetrieveJOBillingTypes(id);
                    var jobOrderAttachments = RetrieveJOAttachments(id);

                    if (jobOrderCases.Count > 0)
                    {
                        GetDbSet<TaggedCase>().RemoveRange(jobOrderCases);
                        UnitOfWork.SaveChanges();
                    }

                    if (jobOrderBilllingTypes.Count > 0)
                    {
                        GetDbSet<JobOrderBillingType>().RemoveRange(jobOrderBilllingTypes);
                        UnitOfWork.SaveChanges();
                    }

                    if (jobOrderAttachments.Count > 0)
                    {
                        foreach (var removed_attachment in removedJOAttachments)
                        {
                            var attachmententsForDeletion = jobOrderAttachments.Where(x => x.Filename == removed_attachment);

                            if(attachmententsForDeletion.Count() > 0)
                            {
                                GetDbSet<Attachment>().RemoveRange(attachmententsForDeletion);
                            }
                            
                            UnitOfWork.SaveChanges();
                        }
                    }

                    GetDbSet<TaggedCase>().AddRange(newJOCases);
                    UnitOfWork.SaveChanges();

                    GetDbSet<JobOrderBillingType>().AddRange(newJOBillingTypes);
                    UnitOfWork.SaveChanges();
                    
                    if (newJOAttachments.Count > 0)
                    {
                        var attachmentForInsert = new List<Attachment>();
                        var updated_attachments = RetrieveJOAttachments(id).Select(x => x.Filename).ToList();
                        var new_attachments = newJOAttachments.Select(x => x.Filename).ToList();

                        foreach (var new_attachment in new_attachments)
                        {
                            if (!updated_attachments.Contains(new_attachment))
                            {
                                attachmentForInsert.Add(new Attachment {
                                    JobOrderID = id,
                                    Filename = new_attachment
                                });

                            }
                        }

                        GetDbSet<Attachment>().AddRange(attachmentForInsert);
                        UnitOfWork.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }
        }
    }
}
