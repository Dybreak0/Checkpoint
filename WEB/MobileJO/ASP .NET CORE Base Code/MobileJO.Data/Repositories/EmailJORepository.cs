using Microsoft.EntityFrameworkCore;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.EmailJO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Data.Repositories
{
    public class EmailJORepository : BaseRepository, IEmailJORepository
    {
        /// <summary>
        ///     Constructor for unitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public EmailJORepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        ///     Used to retrieve the list of email addresses.
        /// </summary>
        /// <returns></returns>
        public ListViewModel GetAll()
        {
            var results = GetDbSet<EmailSetup>()
                          .Select(x => new
                          {
                              type_id = x.TypeID,
                              email_address = x.EmailAddress
                          });

            return new ListViewModel { Data = results };
        }

        /// <summary>
        ///     Used to save email entries
        /// </summary>
        /// <param name="emailsJson"></param>
        public void SaveEmails(string emailsJson)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                Context.SaveEmails(emailsJson);
                Context.SaveChanges();

                transaction.Commit();
            }
        }

        
        /// <summary>
        ///     Used to retrieve job orders by id
        /// </summary>
        /// <param name="id"></param>
        public JobOrder FindJobOrder(string id)
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
                    .ThenInclude(bt => bt.BillingType)
                .Include(att => att.Attachment)
                .Where(x => x.JobOrderNumber == id).FirstOrDefault();
        }

        /// <summary>
        ///     Retrieves all default email recipients
        /// </summary>
        public List<EmailAddressViewModel> GetAllEmailRecipient()
        {
            return GetDbSet<EmailSetup>()
                .Select(email => new EmailAddressViewModel
                {
                    TypeID = email.TypeID,
                    EmailAddress = email.EmailAddress
                })
                .Take(Constants.Common.MaxEmailCount)
                .ToList();
        }

        /// <summary>
        ///     Used to retrieve job orders by id
        /// </summary>
        /// <param name="jobOrderIDs"></param>
        public List<JobOrder> SearchJobOrder(List<string> jobOrderIDs)
        {
            var result = new List<JobOrder>();

            foreach (string id in jobOrderIDs)
            {
                var temp = FindJobOrder(id);
                if (temp != null)
                    result.Add(temp);
            }

            return result;
        }

        /// <summary>
        ///     Used to retrieve tagged job orders
        /// </summary>
        /// <param name="createdBy"></param>
        /// <param name="caseID"></param>
        public List<SelectJOViewModel> GetTaggedCases(int createdBy, int caseID)
        {

            return GetDbSet<TaggedCase>()
                           .Include(jobOrder => jobOrder.JobOrder)
                           .Where(x =>
                               (x.JobOrder.CreatedBy == createdBy) &&
                               (x.JobOrder.StatusID == (int)Constants.Status.Signed) &&
                               (x.CaseID == caseID)
                           )
                           .Select(jo => new SelectJOViewModel
                           {
                               ID = jo.JobOrderID,
                               JobOrderNo = jo.JobOrder.JobOrderNumber,
                               JobOrderSubject = jo.JobOrder.JobOrderSubject
                           })
                           .ToList();


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobOrderIDs"></param>
        /// <returns></returns>
        public bool UpdateJobOrder(int jobOrderID, int userID)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var jobOrder = GetDbSet<JobOrder>()
                                    .Where(x => x.ID == jobOrderID)
                                    .FirstOrDefault();

                    if (jobOrder != null)
                    {
                        var currentDateTime = DateTime.Now;

                        jobOrder.StatusID = (int)Constants.Status.Sent;
                        jobOrder.UpdatedDate = currentDateTime;
                        jobOrder.LastSyncDate = currentDateTime;
                        jobOrder.UpdatedBy = userID;

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
        ///     Retrieves all default email recipients
        /// </summary>
        public List<string> GetAllAdminEmailRecipient()
        {
            return GetDbSet<User>()
                .Where(x => x.RoleID == 1 &&
                            x.IsActive == true &&
                            x.AllowedToLogin == true)
                .Select(x => x.EmailAddress)
                .ToList();
        }

        /// <summary>
        ///     Used to retrieve email addresses count.
        /// </summary>
        /// <returns></returns>
        public int GetEmailCount()
        {
            return GetDbSet<EmailSetup>().Count();
        }
    }
}
