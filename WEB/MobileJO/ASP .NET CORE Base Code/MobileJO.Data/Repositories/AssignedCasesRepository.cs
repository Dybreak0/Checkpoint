using Microsoft.EntityFrameworkCore;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.Models.TFSIntegration;
using MobileJO.Data.ViewModels.AssignedCase;
using MobileJO.Data.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Data.Repositories
{
    public class AssignedCasesRepository : BaseRepository, IAssignedCasesRepository
    {
        /// <summary>
        ///     Constructor for IUnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public AssignedCasesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }
        
        /// <summary>
        ///   Used to retrieve case details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AssignedCasesViewModel Find(int id)
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
                    UpdatedBy = string.Format(Constants.Common.DateFormatWithName, Helper.GetFormattedDate(z.UpdatedDate), z.UpdatedBy),
                    HasSignedJOs = HasSignedJO(id)
                })
                .FirstOrDefault();
        }

        /// <summary>
        ///   Used to check if the job order is already signed
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        private bool HasSignedJO(int caseID)
        {
            return GetDbSet<TaggedCase>()
                            .Include(jo => jo.JobOrder)
                            .Any(x => x.CaseID == caseID && x.JobOrder.StatusID == 2);
                            
        }

        /// <summary>
        ///   Retrieves all assgigned cases
        /// </summary>
        public IQueryable<AssignedCase> RetrieveAllAssignedCases()
        {
            return GetDbSet<AssignedCase>()
                .Include(app => app.ApplicationType)
                .Include(acc => acc.Account);
        }
        
        /// <summary>
        ///    Retrieves a list of assigned cases
        /// </summary>
        /// <param name="searchViewModel">Search filters for finding assigned cases</param>
        /// <returns></returns>
        public ListViewModel Search(AssignedCasesSearchViewModel searchViewModel)
        {
            var assignedCases = RetrieveAllAssignedCases()
                     .Where(x => ((searchViewModel.CaseNumber == null) || x.CaseNumber == searchViewModel.CaseNumber) &&
                                 (string.IsNullOrEmpty(searchViewModel.Status) || x.Status.Equals(searchViewModel.Status)) &&
                                 (string.IsNullOrEmpty(searchViewModel.ApplicationType) || x.ApplicationType.ApplicationName == searchViewModel.ApplicationType) &&
                                 (x.AssignedUserID == searchViewModel.AssignedTo) && !x.Status.Equals(Constants.Common.Closed))
                     .ToList()
                     .OrderByDescending(x => x.CaseNumber);

            if (searchViewModel.Page == 0)
                searchViewModel.Page = 1;

            if(searchViewModel.PageSize <= 0)
                searchViewModel.PageSize = 10;

            var totalCount = assignedCases.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchViewModel.PageSize);

            var results = assignedCases.Skip(searchViewModel.PageSize * (searchViewModel.Page - 1))
                    .Take(searchViewModel.PageSize)
                    .AsEnumerable()
                    .Select(cases => new AssignedCasesListViewModel
                    {
                        ID = cases.ID,
                        CaseNumber = cases.CaseNumber,
                        Status = cases.Status,
                        ApplicationType = cases.ApplicationType.ApplicationName,
                        CaseSubject = cases.CaseSubject,
                        Priority = cases.Priority,
                        Description = cases.Description,
                        Color = Helper.GetPriorityColor(cases.Priority)
                    })
                    .OrderByDescending(x => x.CaseNumber)
                    .ToList();

            var pagination = new
            {
                pages = totalPages,
                size = totalCount
            };

            return new ListViewModel { Pagination = pagination, Data = results };
        }

        /// <summary>
        ///   Retrieves all application types
        /// </summary>
        public List<string> GetApplicationTypes()
        {
            return GetDbSet<ApplicationType>()
                .Select(x => x.ApplicationName)
                .ToList();
        }

        /// <summary>
        ///   Retrieves all case status
        /// </summary>
        public List<string> GetCaseStatusList()
        {
            return GetDbSet<AssignedCase>()
                .Where(x => !x.Status.Equals(Constants.Common.Closed))
                .Select(x => x.Status)
                .Distinct()
                .ToList();
        }

        public List<AssignedCasesListViewModel> FindCases(List<int> ids)
        {            

            return GetDbSet<AssignedCase>()
                .Include(app => app.ApplicationType)
                .Include(acc => acc.Account)
                .Where(s => ids.Contains(s.ID))
                .Select(z => new AssignedCasesListViewModel
                {
                    ID = z.ID,
                    CaseNumber = z.CaseNumber,
                    Status = z.Status,
                    CaseSubject = z.CaseSubject,
                    Priority = z.Priority,
                })                
                .OrderByDescending(z => z.CaseNumber)
                .ToList();
        }

        /// <summary>
        ///   Used to retrieve case details by id
        /// </summary>
        /// <param name="CaseNumber"></param>
        /// <returns></returns>
        public AssignedCase FindByCaseNumber(string CaseNumber)
        {
            return GetDbSet<AssignedCase>()
                .Include(app => app.ApplicationType)
                .Include(acc => acc.Account)
                .Include(user => user.User)
                .Where(x => x.CaseNumber == CaseNumber)
                .Select(z => new AssignedCase
                {
                    ID = z.ID,
                    CaseNumber = z.CaseNumber,
                    Status = z.Status,
                    CaseSubject = z.CaseSubject,
                    Priority = z.Priority,
                    Description = z.Description,
                    CreatedBy = string.Format(Constants.Common.DateFormatWithName, Helper.GetFormattedDate(z.CreatedDate), z.CreatedBy),
                    UpdatedBy = string.Format(Constants.Common.DateFormatWithName, Helper.GetFormattedDate(z.UpdatedDate), z.UpdatedBy)
                })
                .FirstOrDefault();
        }

        /// <summary>
        ///   Used to retrieve case details by id
        /// </summary>
        /// <param name="assignedCase"></param>
        /// <returns></returns>
        public string Create(AssignedCase assignedCase)
        {
            string msg = string.Empty;

            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    GetDbSet<AssignedCase>().Add(assignedCase);

                    UnitOfWork.Database.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    msg = e.InnerException.Message;
                    dbContextTransaction.Rollback();
                }
            }

            return msg;
        }

        /// <summary>
        ///   Used to retrieve case details by id
        /// </summary>
        /// <param name="assignedCase"></param>
        /// <returns></returns>
        public string Update(AssignedCase assignedCase)
        {
            string msg = string.Empty;

            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    AssignedCase assignedCaseUpdate = FindByID(assignedCase.ID);

                    assignedCaseUpdate.CaseNumber = assignedCase.CaseNumber;
                    assignedCaseUpdate.Status = assignedCase.Status;
                    assignedCaseUpdate.ApplicationTypeID = assignedCase.ApplicationTypeID; 
                    assignedCaseUpdate.CaseSubject = assignedCase.CaseSubject;
                    assignedCaseUpdate.Priority = assignedCase.Priority;
                    assignedCaseUpdate.AccountID = assignedCase.AccountID; 
                    assignedCaseUpdate.Description = assignedCase.Description; 
                    assignedCaseUpdate.AssignedUserID = assignedCase.AssignedUserID; 
                    assignedCaseUpdate.CreatedDate = assignedCase.CreatedDate;
                    assignedCaseUpdate.CreatedBy = assignedCase.CreatedBy;
                    assignedCaseUpdate.UpdatedDate = assignedCase.UpdatedDate;
                    assignedCaseUpdate.CreatedBy = assignedCase.UpdatedBy;
                    UnitOfWork.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    msg = e.InnerException.Message;
                    dbContextTransaction.Rollback();
                }
            }

            return msg;
        }

        /// <summary>
        ///   Used to retrieve case details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AssignedCase FindByID(int id)
        {
            AssignedCase assignedCase = null;
            assignedCase = GetDbSet<AssignedCase>().Find(id);

            return assignedCase;
        }
    }
}
