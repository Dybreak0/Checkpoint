using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels;
using MobileJO.Core.ViewModels.AssignedCases;
using MobileJO.Core.ViewModels.Common;
using MobileJO.Core.ViewModels.FieldViewModels;
using MobileJO.Core.ViewModels.QuestionnaireListViewModels;
using MobileJO.Core.ViewModels.ResponseListViewModels;
using MobileJO.Data.ViewModels.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MobileJO.Core.Data
{
    public class JobOrderLocalDatabase
    {
        private Dictionary<string, string> _parameter;
        readonly SQLiteConnection _database;
        readonly static object locker = new object();

        public JobOrderLocalDatabase(string dbPath)
        {               
            _database = new SQLiteConnection(dbPath);

            _database.CreateTable<LocalJobOrder>();
            _database.CreateTable<Attachment>();
            _database.CreateTable<JobOrderBillingType>();
            _database.CreateTable<TaggedCase>();
            _database.CreateTable<AssignedCases>();
            _database.CreateTable<BillingTypes>();
            _database.CreateTable<LocalJobOrder>();
            _database.CreateTable<ApplicationType>();
            _database.CreateTable<JobOrderStatus>();
            _database.CreateTable<Account>();
            _database.CreateTable<ApplicationType>();
            _database.CreateTable<CaseStatus>();
            _database.CreateTable<Response>();
            _database.CreateTable<Answer>();
            _database.CreateTable<Template>();
            _database.CreateTable<Question>();
            _database.CreateTable<Choice>();
            _database.CreateTable<Company_Branch>();
            _database.CreateTable<Template_Branch>();
        }

        public List<LocalJobOrder> GetJobOrdersAsync()
        {
            lock (locker)
            {
                return _database.Table<LocalJobOrder>().ToList();
            }
        }

        public List<Attachment> GetAttachmentsAsync()
        {
            lock (locker)
            {
                return _database.Table<Attachment>().ToList();
            }
        }

        public List<JobOrderBillingType> GetBillingTypesAsync()
        {
            lock (locker)
            {
                return _database.Table<JobOrderBillingType>().ToList();
            }
        }

        public List<BillingTypes> GetAllBillingTypesAsync()
        {
            lock (locker)
            {
                return _database.Table<BillingTypes>().ToList();
            }
        }

        public List<Template> GetAllTemplatesAsync()
        {
            lock (locker)
            {
                return _database.Table<Template>().ToList();
            }
        }

        public LocalJobOrder GetJobOrderAsync(int jobOrderID)
        {
            lock (locker)
            {
                var result = _database.Table<LocalJobOrder>()
                            .Where(i => i.ID == jobOrderID)
                            .FirstOrDefault();
            
                return result;
            }
        }

        public List<TaggedCase> GetTaggedCases(int jobOrderID)
        {
            lock (locker)
            {
                return _database.Table<TaggedCase>()
                            .Where(i => i.JobOrderID == jobOrderID)
                            .ToList();
            }
        }

        public List<JobOrderBillingType> GetJOBillingTypes(int jobOrderID)
        {
            lock (locker)
            { 
                return _database.Table<JobOrderBillingType>()
                                .Where(i => i.JobOrderID == jobOrderID && i.ID != 0)
                                .ToList();
            }
        }

        public List<Attachment> GetJOAttachments(int jobOrderID)
        {
            lock (locker)
            {
                var attachments = _database.Table<Attachment>()
                            .Where(i => i.JobOrderID == jobOrderID && i.ID != 0)
                            .ToList();

                return attachments;
            }
        }

        public AssignedCases GetCasesAsync(int caseID)
        {
            lock (locker)
            {
                return _database.Table<AssignedCases>()
                            .Where(i => i.ID == caseID)
                            .FirstOrDefault();
            }
        }

        public BillingTypes GetBillingTypeAsync(int billingTypeID) 
        {
            lock (locker)
            {
                return _database.Table<BillingTypes>()
                            .Where(i => i.ID == billingTypeID)
                            .FirstOrDefault();
            }
        }

        public List<AssignedCases> GetAllCasesAsync()
        {
            lock (locker)
            {
                return _database.Table<AssignedCases>().ToList();
            }
        }

        public List<AssignedCases> GetUserCasesAsync(string assignedTo, int applicationTypeId, int accountId)
        {
            lock (locker)
            {
                var assignedToID = int.Parse(assignedTo);
                var result = _database.Table<AssignedCases>()
                    .Where(x => x.AssignedUserID == assignedToID &&
                    x.ApplicationTypeID == applicationTypeId &&
                    x.AccountID == accountId)
                    .ToList();

                return result;
            }
        }

        public List<Account> GetAllAccountsAsync()
        {
            lock (locker)
            {
                return _database.Table<Account>().ToList();
            }
        }

        public ListViewModel GetAllJobOrdersAsync(Dictionary<string, string> searchViewModel)
        {
            lock (locker)
            {
                _parameter = searchViewModel;

                // get values from search view model
                string JobOrderNumber = _parameter[Constants.Params.JobOrderNumber];
                int PageSize = int.Parse(_parameter[Constants.Params.PageSize]);
                int Page = int.Parse(_parameter[Constants.Params.Page]);
                int Status = int.Parse(_parameter[Constants.Params.JobOrderStatus]);
                var joborders = _database.Table<LocalJobOrder>();

                if (_parameter[Constants.Params.ApplicationType] == string.Empty)
                {
                    joborders = _database.Table<LocalJobOrder>()
                             .Where(y => (JobOrderNumber == string.Empty || y.JobOrderNumber.Contains(JobOrderNumber)) &&
                             (y.StatusID == Status) &&
                             (y.IsDeleted == false))
                             .OrderByDescending(x => x.JobOrderNumber);
                }

                else
                {
                    string ApplicationType = _parameter[Constants.Params.ApplicationType];
                    int Application = !string.IsNullOrEmpty(ApplicationType) ? int.Parse(ApplicationType) : 0;

                    joborders = _database.Table<LocalJobOrder>()
                     .Where(y => (JobOrderNumber == string.Empty || y.JobOrderNumber.Contains(JobOrderNumber)) &&
                                  (y.StatusID == Status) && (y.ApplicationTypeID == Application) &&
                                  (y.IsDeleted == false))
                      .OrderByDescending(x => x.JobOrderNumber);

                }

                var results = new List<LocalJobOrder>();
                var totalCount = joborders.Count();
                var totalPages = 1;
                double total = (double)totalCount / PageSize;
                if (PageSize > 0)
                {
                    totalPages = (int)Math.Ceiling(total);
                    results = joborders.Skip(PageSize * (Page - 1))
                    .Take(PageSize)
                    .ToList();
                }
                else
                {
                    results = joborders.ToList();
                }

                var Pagination = new
                {
                    pages = totalPages,
                    size = totalCount
                };

                return new ListViewModel { Pages = totalPages, Size = totalCount, Data = results };
            }
        }

        public List<LocalJobOrder> GetAllJobOrderStatusAsync()
        {
            lock (locker)
            {
                return _database.Table<LocalJobOrder>().ToList();
            }
        }

        public List<JobOrderStatus> GetJobOrderStatusAsync()
        {
            lock (locker)
            {
                return _database.Table<JobOrderStatus>().ToList();
            }
        }

        public List<ApplicationType> GetAllApplicationTypesAsync()
        {
            lock (locker)
            {
                return _database.Table<ApplicationType>().ToList();
            }
        }

        public int SaveStatusAsync(IEnumerable<JobOrderStatus> status)
        {
            lock (locker)
            {
                if (GetJobOrderStatusAsync().Count > 0)
                {
                    _database.Execute("DELETE FROM JobOrderStatus");
                    _database.Execute("DELETE FROM SQLITE_SEQUENCE WHERE NAME='JobOrderStatus'");
                    return _database.InsertAll(status);
                }

                return _database.InsertAll(status);
            }
        }

        public LocalJobOrder GetJobOrderByServerIDAsync(int jobOrderID)
        {
            lock (locker)
            {
                return _database.Table<LocalJobOrder>()
                            .Where(i => i.ServerID == jobOrderID)
                            .FirstOrDefault();
            }
        }

        public int SaveJobOrdersAsync(IEnumerable<JobOrder> jobOrders)
        {
            lock (locker)
            {
                List<LocalJobOrder> localJobOrders = new List<LocalJobOrder>();

                foreach (JobOrder jobOrder in jobOrders)
                {
                    localJobOrders.Add(new LocalJobOrder
                    {
                        ServerID = jobOrder.ID,
                        JobOrderNumber = jobOrder.JobOrderNumber,
                        JobOrderSubject = jobOrder.JobOrderSubject,
                        Branch = jobOrder.Branch,
                        AccountID = jobOrder.AccountID,
                        ApplicationTypeID = jobOrder.ApplicationTypeID,
                        DateTimeStart = jobOrder.DateTimeStart,
                        DateTimeEnd = jobOrder.DateTimeEnd,
                        ActivityDetails = jobOrder.ActivityDetails,
                        RootCauseAnalysis = jobOrder.RootCauseAnalysis,
                        PreventiveAction = jobOrder.PreventiveAction,
                        Remarks = jobOrder.Remarks,
                        NextStep = jobOrder.NextStep,
                        Attendees = jobOrder.Attendees,
                        IsBilled = jobOrder.Is_Billed,
                        IsCollaterals = jobOrder.Is_Collaterals,
                        ClientSignature = jobOrder.ClientSignature,
                        IsFixed = jobOrder.Is_Fixed,
                        IsSatisfied = jobOrder.Is_Satisfied,
                        ClientRating = jobOrder.ClientRating,
                        StatusID = jobOrder.StatusID,
                        IsDeleted = jobOrder.Is_Deleted,
                        CreatedDate = jobOrder.CreatedDate,
                        CreatedBy = jobOrder.CreatedBy,
                        UpdatedDate = jobOrder.UpdatedDate,
                        UpdatedBy = jobOrder.UpdatedBy,
                        LastSyncDate = jobOrder.LastSyncDate
                    });
                }

                return _database.InsertAll(localJobOrders);
            }
        }

        public int SaveAccountsAsync(IEnumerable<Account> accounts)
        {
            lock (locker)
            {
                if (GetAllAccountsAsync().Count > 0)
                {
                    _database.Execute("DELETE FROM Account");
                    _database.Execute("DELETE FROM SQLITE_SEQUENCE WHERE NAME='Account'");
                    return _database.InsertAll(accounts);
                }

                return _database.InsertAll(accounts);
            }
        }

        public int SaveApplicationTypesAsync(IEnumerable<ApplicationType> applicationTypes)
        {
            lock (locker)
            {
                if (GetAllApplicationTypesAsync().Count > 0)
                {
                    _database.Execute("DELETE FROM ApplicationType");
                    _database.Execute("DELETE FROM SQLITE_SEQUENCE WHERE NAME='ApplicationType'");
                    return _database.InsertAll(applicationTypes);
                }

                return _database.InsertAll(applicationTypes);
            }
        }

        public int SaveBillingTypesAsync(IEnumerable<BillingTypes> billingTypes)
        {
            lock (locker)
            {
                _database.Execute("DELETE FROM BillingTypes");
                _database.Execute("DELETE FROM SQLITE_SEQUENCE WHERE NAME='BillingTypes'");

                return _database.InsertAll(billingTypes);
            }
        }

        public ApplicationType GetApplicationTypesAsync(int applicationID)
        {
            lock (locker)
            {
                return _database.Table<ApplicationType>()
                            .Where(i => i.ID == applicationID)
                            .FirstOrDefault();
            }
        }

        public JobOrderStatus GetStatusAsync(int statusID)
        {
            lock (locker)
            {
                return _database.Table<JobOrderStatus>()
                            .Where(s => s.ID == statusID)
                            .FirstOrDefault();
            }
        }

        public int SaveJobOrderAsync(LocalJobOrder jobOrder)
        {
            lock (locker)
            {
                if (jobOrder.ID != 0)
                {
                    return _database.Update(jobOrder);
                }
                else
                {
                    return _database.Insert(jobOrder);
                }
            }
        }

        public int SaveAssignedCasesAsync(IEnumerable<AssignedCases> assignedCases)
        {
            lock (locker)
            {
                if (GetAllCasesAsync().Count > 0)
                {
                    _database.Execute("DELETE FROM AssignedCases");
                    _database.Execute("DELETE FROM SQLITE_SEQUENCE WHERE NAME='AssignedCases'");
                    return _database.InsertAll(assignedCases);
                }

                return _database.InsertAll(assignedCases);
            }
        }

        public int SaveAttachments(IEnumerable<Attachment> attachments)
        {
            lock (locker)
            {
                return _database.InsertAll(attachments);
            }
        }

        public int SaveBillingTypes(IEnumerable<JobOrderBillingType> jobOrderBillingTypes)
        {
            lock (locker)
            {
                return _database.InsertAll(jobOrderBillingTypes);
            }
        }

        public int SaveCases(IEnumerable<TaggedCase> taggedCases)
        {
            lock (locker)
            {
                return _database.InsertAll(taggedCases);
            }
        }

        public int SaveJobOrderDetails(IEnumerable<TaggedCase> taggedCases, 
                                       IEnumerable<JobOrderBillingType> jobOrderBillingTypes, 
                                       IEnumerable<Attachment> attachments)
        {
            lock (locker)
            {
                _database.InsertAll(taggedCases);
                _database.InsertAll(jobOrderBillingTypes);
                return _database.InsertAll(attachments);
            }
        }

        public int DeleteJobOrderAsync(LocalJobOrder jobOrder)
        {
            lock (locker)
            {
                return _database.Delete(jobOrder);
            }
        }

        public int DeleteTaggedCases(int jobOrderID)
        {
            lock (locker)
            {
                return _database.Table<TaggedCase>()
                                .Where(i => i.JobOrderID == jobOrderID && i.ID != 0)
                                .Delete();
            }
        }

        public int DeleteBillingTypes(int jobOrderID)
        {
            lock (locker)
            {
                return _database.Table<JobOrderBillingType>()
                .Where(i => i.JobOrderID == jobOrderID && i.ID != 0)
                .Delete();
            }
        }

        public int DeleteJOAsync(int jobOrderID)
        {
            lock (locker)
            {
                LocalJobOrder jobOrder = _database.Table<LocalJobOrder>()
                                                    .Where(x => x.ServerID == jobOrderID)
                                                    .FirstOrDefault();

                if (jobOrder != null)
                {
                    if (string.IsNullOrEmpty(jobOrder.JobOrderNumber))
                    {
                        DeleteJobOrderAsync(jobOrder);
                    }
                    else
                    {
                        jobOrder.IsDeleted = true;
                        jobOrder.UpdatedDate = DateTime.Now;
                    }
                }

                return _database.Update(jobOrder);
            }
        }

        public int DeleteIndividualJOAsync(int jobOrderID)
        {
            lock (locker)
            {
                return _database.Table<LocalJobOrder>()
                .Where(i => i.ServerID == jobOrderID)
                .Delete();
            }
        }


        public int DeleteAttachments(int jobOrderID, string attachmentName)
        {
            lock (locker)
            {
                return _database.Table<Attachment>()
                        .Where(i => i.JobOrderID == jobOrderID && 
                                    i.Filename == attachmentName &&
                                    i.ID != 0)                        
                        .Delete();
            }
        }

        public int DeleteAllJobOrders()
        {
            lock (locker)
            {
                return _database.DeleteAll<LocalJobOrder>();
            }
        }

        public int DeleteAllTaggedCases()
        {
            lock (locker)
            {
                return _database.DeleteAll<TaggedCase>();
            }
        }

        public int DeleteAllJOBillingTypes()
        {
            lock (locker)
            {
                return _database.DeleteAll<JobOrderBillingType>();
            }
        }

        public int DeleteAllJOAttachments()
        {
            lock (locker)
            {
                return _database.DeleteAll<Attachment>();
            }
        }

        public int DeleteJobOrderDetails()
        {
            lock (locker)
            {
                _database.Execute("DELETE FROM TaggedCase");
                _database.Execute("DELETE FROM JobOrderBillingType");
                _database.Execute("DELETE FROM Attachment");
                _database.Execute("DELETE FROM LocalJobOrder");

                _database.Execute("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='TaggedCase'");
                _database.Execute("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='JobOrderBillingType'");
                _database.Execute("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Attachment'");
                return _database.Execute("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='LocalJobOrder'"); ;
            }
        }

        public List<AssignedCases> GetCasesSelectedAsync(IEnumerable<int> ids)
        {
            lock (locker)
            {
                return _database.Table<AssignedCases>()
                            .Where(i => ids.Contains(i.ID))
                            .ToList();
            }
        }

        public CasesListViewModel GetAssignedCasesList(AssignedCasesSearchViewModel searchViewModel)
        {
            lock (locker)
            {
                var appTypes = GetAllApplicationTypesAsync();

                var appTypeID = string.IsNullOrEmpty(searchViewModel.application_type) ?
                                    0 : appTypes
                                                .Where(x => x.ApplicationName == searchViewModel.application_type)
                                                .FirstOrDefault()
                                                .ID;

                var assignedCases = _database.Table<AssignedCases>()
                         .Where(x => ((searchViewModel.case_number == string.Empty) || x.CaseNumber == searchViewModel.case_number) &&
                                     (searchViewModel.status == string.Empty || x.Status.Equals(searchViewModel.status)) &&
                                     (searchViewModel.application_type == string.Empty || x.ApplicationTypeID == appTypeID) &&
                                     (x.AssignedUserID == searchViewModel.assigned_to) && !x.Status.Equals(Constants.Common.Closed))
                         .ToList()
                         .OrderByDescending(x => x.CaseNumber);

                if (searchViewModel.page == 0)
                    searchViewModel.page = 1;

                if (searchViewModel.page_size <= 0)
                    searchViewModel.page_size = 10;

                var totalCount = assignedCases.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / searchViewModel.page_size);

                var results = assignedCases.Skip(searchViewModel.page_size * (searchViewModel.page - 1))
                        .Take(searchViewModel.page_size)
                        .AsEnumerable()
                        .Select(cases => new AssignedCasesList
                        {
                            id = cases.ID,
                            case_number = cases.CaseNumber.ToString(),
                            status = cases.Status,
                            application_type = appTypes.Where(x => x.ID == cases.ApplicationTypeID).FirstOrDefault().ApplicationName,
                            case_subject = cases.CaseSubject,
                            priority = cases.Priority,
                            color = Helpers.GetPriorityColor(cases.Priority)
                        })
                        .OrderByDescending(x => x.case_number)
                        .ToList();

                Pagination pagination = new Pagination
                {
                    Pages = totalPages,
                    Size = totalCount
                };

                return new CasesListViewModel { Pagination = pagination, Data = results };
            }
        }

        public List<CaseStatus> GetAllCasesStatus()
        {
            lock (locker)
            {
                return _database.Table<CaseStatus>().ToList();
            }
        }

        public int SaveCaseStatus(IEnumerable<CaseStatus> caseStatuses)
        {
            lock (locker)
            {
                if (GetAllCasesStatus().Count > 0)
                {
                    _database.Execute("DELETE FROM CaseStatus");
                    _database.Execute("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='CaseStatus'");
                    return _database.InsertAll(caseStatuses);
                }

                return _database.InsertAll(caseStatuses);
            }
        }

        public List<JobOrderBillingType> GetLocalJOBillingTypes(int jobOrderID)
        {
            lock (locker)
            {
                return _database.Table<JobOrderBillingType>()
                                .Where(i => i.JobOrderID == jobOrderID && i.ID == 0)
                                .ToList();
            }
        }

        public List<TaggedCase> GetLocalTaggedCases(int jobOrderID)
        {
            lock (locker)
            {
                return _database.Table<TaggedCase>()
                            .Where(i => i.JobOrderID == jobOrderID && i.ID == 0)
                            .ToList();
            }
        }

        public List<Attachment> GetLocalJOAttachments(int jobOrderID)
        {
            lock (locker)
            {
                var attachments = _database.Table<Attachment>()
                            .Where(i => i.JobOrderID == jobOrderID && i.ID == 0)
                            .ToList();

                return attachments;
            }
        }

        public int DeleteLocalTaggedCases(int jobOrderID)
        {
            lock (locker)
            {
                return _database.Table<TaggedCase>()
                                .Where(i => i.JobOrderID == jobOrderID && i.ID == 0)
                                .Delete();
            }
        }

        public int DeleteLocalBillingTypes(int jobOrderID)
        {
            lock (locker)
            {
                return _database.Table<JobOrderBillingType>()
                .Where(i => i.JobOrderID == jobOrderID && i.ID == 0)
                .Delete();
            }
        }

        public int DeleteLocalAttachments(int jobOrderID, string attachmentName)
        {
            lock (locker)
            {
                return _database.Table<Attachment>()
                        .Where(i => i.JobOrderID == jobOrderID &&
                                    i.Filename == attachmentName &&
                                    i.ID == 0)
                        .Delete();
            }
        }

        public List<DropdownViewModel> GetBranches(int companyID)
        {
            lock (locker)
            {
                return _database.Table<Company_Branch>()
                    .Where(companyBranch => companyBranch.CompanyID == companyID)
                    .Select(companyBranch => new DropdownViewModel
                    {
                        Value = companyBranch.BranchID,
                        Text = companyBranch.BranchName
                    })
                    .ToList();
            }
        }

        public ResponseDetailsModel GetResponseByResponseID(int localResponseID, int responseID, int templateID, int userID)
        {
            lock (locker)
            {
                Response responseDetails = _database.Table<Response>()
                .Where(res => res.ID == localResponseID)
                .FirstOrDefault();

                Template templateDetails = _database.Table<Template>()
                    .Where(template => template.TemplateID == templateID && !template.IsDeleted)
                    .FirstOrDefault();

                List<Choice> choiceList = _database.Table<Choice>()
                    .Where(choice => !choice.IsDeleted)
                    .ToList();

                var answerList = _database.Table<Answer>()
                        .Where(answer => answer.LocalResponseID == localResponseID)
                        .ToList();

                List<QuestionModel> questionList = _database.Table<Question>()
                    .AsEnumerable()
                    .Where(question => !question.IsDeleted && question.TemplateID == templateID)
                    .Select(question => new QuestionModel
                    {
                        QuestionID = question.QuestionID,
                        QuestionTypeID = question.QuestionTypeID,
                        Question = question.Qquestion,
                        // for radio buttons and checkboxes
                        Choices = choiceList
                            .Where(choice => choice.QuestionID == question.QuestionID)
                            .Select(choice => new ChoiceModel
                            {
                                ChoiceID = choice.ChoiceID,
                                AnswerID = answerList == null || answerList.Count == 0 ? 0 : answerList
                                    .Where(answer => answer.QuestionID == question.QuestionID
                                        && answer.ChoiceID == choice.ChoiceID)
                                    .Select(answer => answer.AnswerID)
                                    .FirstOrDefault(),
                                Label = choice.Label,
                                Value = choice.Value,
                                IsSelected = answerList == null || answerList.Count == 0 ? false : answerList
                                    .Where(answer => answer.QuestionID == question.QuestionID && answer.ChoiceID == choice.ChoiceID)
                                    .Count() == 1
                            })
                            .ToList(),
                        // for text, image, video, slider
                        Answer = answerList
                            .Where(answer => answer.QuestionID == question.QuestionID)
                            .Select(answer => new AnswerModel
                            {
                                AnswerID = answer.AnswerID,
                                Value = answer.Value
                            })
                            .FirstOrDefault()
                    })
                    .ToList();

                ResponseDetailsModel response = new ResponseDetailsModel
                {
                    LocalResponseID = responseDetails == null ? 0 : responseDetails.ID,
                    ResponseID = responseID,
                    TemplateID = templateDetails.TemplateID,
                    UserID = responseDetails == null ? 0 : responseDetails.UserID,
                    CompanyID = responseDetails == null ? 0 : responseDetails.CompanyID,
                    BranchID = responseDetails == null ? 0 : responseDetails.BranchID,
                    Title = templateDetails.Title,
                    Description = templateDetails.Description,
                    MaxLimit = templateDetails.MaxLimit,
                    Status = responseDetails == null ? false : responseDetails.Status,
                    CanEditAnswer = responseDetails == null ? true : userID == responseDetails.UserID,
                    QuestionList = new List<QuestionModel>(questionList)
                };

                return response;
            }
        }

        public List<DropdownViewModel> GetCompanies()
        {
            lock (locker)
            {
                List<DropdownViewModel> companyQueryList = _database.Table<Company_Branch>()
                    .Select(companyBranch => new DropdownViewModel
                    {
                        Value = companyBranch.CompanyID,
                        Text = companyBranch.CompanyName
                    })
                    .ToList();

                List<DropdownViewModel> companyList = new List<DropdownViewModel>();
                foreach (DropdownViewModel companyQuery in companyQueryList)
                {
                    bool exists = false;
                    foreach (DropdownViewModel company in companyList)
                    {
                        if (company.Value == companyQuery.Value)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        companyList.Add(companyQuery);
                    }
                }
                return companyList;
            }
        }


        public List<DropdownViewModel> GetCompanies(int userTypeID, int companyID, int templateID)
        {
            lock (locker)
            {
                List<int> branchIDs = _database.Table<Template_Branch>()
                    .Where(template_branch => template_branch.TemplateID == templateID)
                    .Select(template_branch => template_branch.BranchID)
                    .ToList();

                List<DropdownViewModel> companyQueryList = _database.Table<Company_Branch>()
                    .Where(companyBranch => ((int)Constants.UserType.SuperAdmin == userTypeID && branchIDs.Contains(companyBranch.BranchID))
                        || companyBranch.CompanyID == companyID)
                    .Select(companyBranch => new DropdownViewModel
                    {
                        Value = companyBranch.CompanyID,
                        Text = companyBranch.CompanyName
                    })
                    .ToList();

                if (companyQueryList == null || companyQueryList.Count <= 1)
                {
                    return companyQueryList;
                }

                // check for duplicates
                List<DropdownViewModel> companyList = new List<DropdownViewModel>();
                foreach (DropdownViewModel companyQuery in companyQueryList)
                {
                    bool exists = false;
                    foreach (DropdownViewModel company in companyList)
                    {
                        if (company.Value == companyQuery.Value)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        companyList.Add(companyQuery);
                    }
                }
                return companyList;
            }
        }

        public QuestionnairePaginationViewModel GetQuestionnaireList(QuestionnaireSearchViewModel searchViewModel)
        {
            lock (locker)
            {
                int branchID = 0;
                int companyID = 0;

                string title = null;
                string category = null;
                List<int> templateIDs = null;

                if (searchViewModel != null)
                {
                    branchID = string.IsNullOrEmpty(searchViewModel.BranchID) ? 0 : Convert.ToInt32(searchViewModel.BranchID);
                    companyID = string.IsNullOrEmpty(searchViewModel.CompanyID) ? 0 : Convert.ToInt32(searchViewModel.CompanyID);
                    title = searchViewModel.Title;
                    category = searchViewModel.Category;
                }

                if (branchID > 0)
                {
                    templateIDs = _database.Table<Template_Branch>()
                        .Where(template_branch => template_branch.BranchID == branchID)
                        .Select(o => o.TemplateID)
                        .ToList();
                }
                else if (companyID > 0)
                {
                    var branchIDs = _database.Table<Company_Branch>()
                        .Where(companyBranch => companyBranch.CompanyID == companyID)
                        .Select(companyBranch => companyBranch.BranchID)
                        .ToList();

                    templateIDs = _database.Table<Template_Branch>()
                        .Where(template_branch => branchIDs.Contains(template_branch.BranchID))
                        .Select(template_branch => template_branch.TemplateID)
                        .ToList();
                }

                DateTime now = DateTime.Now.Date;
                List<Template> templateList = GetAllTemplatesAsync();
                templateList = templateList
                    .Where(template =>
                        template.IsDeleted == false
                        && (string.IsNullOrEmpty(title)
                            || template.Title.Contains(title))
                        && (string.IsNullOrEmpty(category)
                            || template.Category.Contains(category))
                        && (templateIDs == null
                            || templateIDs.Contains(template.TemplateID))
                        && (template.StartDate.CompareTo(now) <= 0
                            && template.EndDate.CompareTo(now) >= 0))
                    .OrderByDescending(template => template.UpdatedDate)
                    .ToList();

                if (0 == searchViewModel.Page)
                    searchViewModel.Page = 1;

                var totalCount = templateList.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / searchViewModel.PageSize);

                List<Template_Branch> templateBranchList = _database.Table<Template_Branch>().ToList();
                List<Company_Branch> companyBranchList = _database.Table<Company_Branch>().ToList();

                var companyBranchNameList = (templateBranchList.Join(companyBranchList, a => a.BranchID, b => b.BranchID, (a, b) => new { a.TemplateID, b.CompanyName, b.BranchName }));
                List<QuestionnaireModel> questionnaireList = templateList.Skip(searchViewModel.PageSize * (searchViewModel.Page - 1))
                .Take(searchViewModel.PageSize)
                .AsEnumerable()
                .Select(template => new QuestionnaireModel
                {
                    ID = template.TemplateID,
                    Title = template.Title,
                    Description = template.Description,
                    Category = template.Category,
                    CompanyBranchNames = string.Join(", ", companyBranchNameList
                                    .Where(a => a.TemplateID == template.TemplateID)
                                    .ToList()
                                    .Select(o => o.CompanyName + " - " + o.BranchName)),
                    CreatedDate = template.CreatedDate.ToString(Constants.Common.MonthNameDateFormat, CultureInfo.InvariantCulture)
                })
                .ToList();

                Pagination pagination = new Pagination
                {
                    Pages = totalPages,
                    Size = totalCount
                };

                return new QuestionnairePaginationViewModel { Pagination = pagination, Data = questionnaireList };
            }
        }

        public ResponseDataViewModel GetResponseList(ResponseSearchViewModel searchViewModel)
        {
            lock (locker)
            {
                int userTypeID = Convert.ToInt32(searchViewModel.UserTypeID);
                int userID = Convert.ToInt32(searchViewModel.UserID);
                int templateID = Convert.ToInt32(searchViewModel.TemplateID);
                int companyID = Convert.ToInt32(searchViewModel.CompanyID);

                var responses = _database.Table<Response>()
                    .Where(response => response.TemplateID == templateID
                        && ((int)Constants.UserType.SuperAdmin == userTypeID
                            || ((int)Constants.UserType.CompanyAdmin == userTypeID && response.CompanyID == companyID)
                            || ((int)Constants.UserType.Employee == userTypeID && response.UserID == userID)))
                    .OrderByDescending(x => x.DateSubmitted)
                    .ToList();

                Template template = _database.Table<Template>()
                    .Where(t => t.TemplateID == templateID)
                    .First();

                var totalCount = 0;
                int numOfAnswered = 0;
                List<ResponseModel> results = null;

                if (responses != null && responses.Count > 0)
                {
                    totalCount = responses.Count();
                    numOfAnswered = responses
                        .Where(response => response.UserID == userID
                            && response.TemplateID == templateID
                            && response.DateSubmitted.Date.CompareTo(DateTime.Now.Date) == 0)
                        .ToList()
                        .Count();

                    results = responses
                        .Select(response => new ResponseModel
                        {
                            LocalResponseID = response.ID,
                            ResponseID = response.ResponseID,
                            TemplateID = response.TemplateID,
                            UserID = response.UserID,
                            Title = template.Title,
                            Description = template.Description,
                            DateSubmitted = response.DateSubmitted.ToString(Constants.Common.MonthNameDateFormat, CultureInfo.InvariantCulture),
                            Status = response.Status,
                        })
                        .ToList();
                }

                Pagination pagination = new Pagination
                {
                    Pages = 1,
                    Size = totalCount
                };

                return new ResponseDataViewModel { Pagination = pagination, MaxLimit = template.MaxLimit, NumOfAnswered = numOfAnswered, Result = results };
            }
        }

        public void SaveResponse(ResponseAnswerModel responseAnswerModel)
        {
            lock (locker)
            {
                _database.BeginTransaction();

                try
                {
                    int responseID = responseAnswerModel.LocalResponseID;
                    DateTime now = responseAnswerModel.LastSyncDate;

                    int localResponseID = 0;
                    Response response = CopyResponseValues(responseAnswerModel);
                    Response localResponse = null;

                    if (responseID > 0)
                    {
                        localResponse = FindResponseByID(responseID);
                    }

                    if (localResponse == null)
                    {
                        localResponseID = InsertResponse(response);
                    }
                    else
                    {
                        // Update existing response data
                        localResponseID = localResponse.ID;
                        localResponse.ResponseID = response.ResponseID;
                        localResponse.UserID = response.UserID;
                        localResponse.UpdatedBy = response.UpdatedBy;
                        localResponse.UpdatedDate = response.UpdatedDate;
                        localResponse.Status = response.Status;
                        localResponse.DateSubmitted = response.DateSubmitted;
                        UpdateResponse(localResponse);
                    }

                    List<Answer> answersToAdd = new List<Answer>();
                    List<int> answersToDelete = new List<int>();
                    foreach (AnswerModel answerDetails in responseAnswerModel.AnswerList)
                    {
                        if (answerDetails.QuestionTypeID == (int)Constants.QuestionType.Checkbox)
                        {
                            // delete existing answer data
                            answersToDelete.Add(answerDetails.QuestionID);

                            // construct new answer data
                            answersToAdd.Add(ConstructNewAnswer(answerDetails, localResponseID, now));
                            continue;
                        }

                        // update existing answer data
                        if (answerDetails.AnswerID > 0)
                        {
                            Answer answerForUpdate = FindAnswer(answerDetails.AnswerID, localResponseID, answerDetails.QuestionID);

                            if (answerForUpdate == null)
                            {
                                answersToAdd.Add(ConstructNewAnswer(answerDetails, localResponseID, now));
                                continue;
                            }

                            answerForUpdate.LocalResponseID = localResponseID;
                            answerForUpdate.ResponseID = answerDetails.ResponseID;
                            answerForUpdate.ChoiceID = answerDetails.ChoiceID;
                            answerForUpdate.UserID = answerDetails.UserID;
                            answerForUpdate.Value = answerDetails.Value;
                            answerForUpdate.DateSubmitted = now;
                            answerForUpdate.UpdatedDate = now;
                            answerForUpdate.UpdatedBy = answerDetails.UserID;
                            answerForUpdate.LastSyncDate = now;
                            UpdateAnswer(answerForUpdate, now);
                            continue;
                        }

                        Answer answer = FindAnswerByQuestionID(localResponseID, answerDetails.QuestionID);

                        if (answer == null)
                        {
                            // construct new answer data
                            answersToAdd.Add(ConstructNewAnswer(answerDetails, localResponseID, now));
                            continue;
                        }

                        answer.LocalResponseID = localResponseID;
                        answer.ResponseID = answerDetails.ResponseID;
                        answer.ChoiceID = answerDetails.ChoiceID;
                        answer.UserID = answerDetails.UserID;
                        answer.Value = answerDetails.Value;
                        answer.DateSubmitted = now;
                        answer.UpdatedDate = now;
                        answer.UpdatedBy = answerDetails.UserID;
                        UpdateAnswer(answer, now);
                        
                    }

                    if (answersToDelete.Count > 0)
                    {
                        DeleteAnswers(localResponseID, answersToDelete);
                    }

                    if (answersToAdd.Count > 0)
                    {
                        _database.InsertAll(answersToAdd);
                    }
                }
                catch (Exception e)
                {
                    _database.Rollback();
                    throw e;
                }

                _database.Commit();
                List<Answer> answerList = _database.Table<Answer>().ToList();
            }
        }

        private Response FindResponseByID (int id)
        {
            return _database.Table<Response>()
                .Where(response => response.ID == id)
                .Select(response => response)
                .FirstOrDefault();
        }

        private Response FindResponseByResponseID(int responseID)
        {
            return _database.Table<Response>()
                .Where(response => response.ResponseID == responseID)
                .Select(response => response)
                .OrderByDescending(response => response.UpdatedDate)
                .FirstOrDefault();
        }

        private Response FindSpecificResponse (int responseID, int templateID, int companyID, int branchID, int userID)
        {
            return _database.Table<Response>()
                .Where(response => response.ResponseID == responseID
                    || (response.TemplateID == templateID
                        && response.CompanyID == companyID
                        && response.BranchID == branchID
                        && response.UserID == userID))
                .Select(response => response)
                .FirstOrDefault();
        }

        private Response CopyResponseValues(ResponseAnswerModel responseAnswerModel)
        {
            Response response = new Response()
            {
                ResponseID = responseAnswerModel.ResponseID,
                TemplateID = responseAnswerModel.TemplateID,
                UserID = responseAnswerModel.UserID,
                BranchID = responseAnswerModel.BranchID,
                CompanyID = responseAnswerModel.CompanyID,
                DateSubmitted = responseAnswerModel.DateSubmitted,
                Remarks = null,
                Status = responseAnswerModel.Status,
                CreatedDate = responseAnswerModel.CreatedDate,
                CreatedBy = responseAnswerModel.CreatedBy,
                UpdatedDate = responseAnswerModel.UpdatedDate,
                UpdatedBy = responseAnswerModel.UpdatedBy,
                LastSyncDate = responseAnswerModel.LastSyncDate
            };

            return response;
        }

        private int UpdateResponse(Response response)
        {
            return _database.Update(response);
        }

        private int InsertResponse(Response response)
        {
            if (_database.Insert(response) == 1)
            {
                Response localResponse = FindResponseByResponseID(response.ResponseID);
                return localResponse.ID;
            }

            return 0;
        }

        private int DeleteAnswers(int responseID, List<int> answersToDelete)
        {
            return _database.Execute("DELETE FROM Answer WHERE LocalResponseID = " + responseID + " AND QuestionID IN (" + string.Join(",", answersToDelete) + ")");
        }

        private Answer ConstructNewAnswer(AnswerModel answerDetails, int responseID, DateTime now)
        {
            return new Answer()
            {
                AnswerID = answerDetails.AnswerID,
                ResponseID = answerDetails.ResponseID,
                LocalResponseID = responseID,
                TemplateID = answerDetails.TemplateID,
                QuestionID = answerDetails.QuestionID,
                ChoiceID = answerDetails.ChoiceID,
                UserID = answerDetails.UserID,
                Value = answerDetails.Value,
                DateSubmitted = now,
                CreatedDate = now,
                CreatedBy = answerDetails.UserID,
                UpdatedDate = now,
                UpdatedBy = answerDetails.UserID,
                LastSyncDate = now
            };
        }

        private void UpdateAnswer(Answer answerDetails, DateTime now)
        {
            _database.Update(answerDetails);
        }

        private Answer FindAnswer(int answerID, int localResponseID, int questionID)
        {
            Answer answerData = _database.Table<Answer>()
                .Where(answer => answer.AnswerID == answerID)
                .FirstOrDefault();

            if (answerData == null)
            {
                answerData = _database.Table<Answer>()
                    .Where(answer => answer.LocalResponseID == localResponseID
                        && answer.QuestionID == questionID)
                    .FirstOrDefault();
            }
            return answerData;
        }

        private Answer FindAnswerByQuestionID(int localResponseID, int questionID)
        {
            return _database.Table<Answer>()
                .Where(answer => answer.LocalResponseID == localResponseID
                    && answer.QuestionID == questionID)
                .FirstOrDefault();
        }

        public List<Answer> GetAnswersSync()
        {
            lock (locker)
            {
                return _database.Table<Answer>().ToList();
            }
        }

        public List<Response> GetResponsesAsync()
        {
            lock (locker)
            {
                return _database.Table<Response>().ToList();
            }
        }

        public List<Answer> GetAnswersByResponseID(int id)
        {
            lock (locker)
            {
                return _database.Table<Answer>()
                            .Where(i => i.LocalResponseID == id)
                            .ToList();
            }
        }

        public List<Question> GetAllQuestionsAsync()
        {
            lock (locker)
            {
                return _database.Table<Question>().ToList();
            }
        }

        public List<Question> GetAllQuestionsByTemplateIDAsync(int templateID)
        {
            lock (locker)
            {
                return _database.Table<Question>()
                    .Where(question => question.TemplateID == templateID)
                    .ToList();
            }
        }

        public List<Choice> GetAllChoicesAsync()
        {
            lock (locker)
            {
                return _database.Table<Choice>().ToList();
            }
        }

        public int SaveTemplateAsync(IEnumerable<Template> templates)
        {
            lock (locker)
            {
                if (GetAllTemplatesAsync().Count > 0)
                {
                    _database.Execute("DELETE FROM Template");
                    _database.Execute("DELETE FROM SQLITE_SEQUENCE WHERE NAME='Template'");
                    return _database.InsertAll(templates);
                }

                return _database.InsertAll(templates);
            }
        }

        public int SaveQuestionsAsync(IEnumerable<Question> questions)
        {
            lock (locker)
            {
                if (GetAllQuestionsAsync().Count > 0)
                {
                    _database.Execute("DELETE FROM Question");
                    _database.Execute("DELETE FROM SQLITE_SEQUENCE WHERE NAME='Question'");
                    return _database.InsertAll(questions);
                }

                return _database.InsertAll(questions);
            }
        }

        public int SaveChoicesAsync(IEnumerable<Choice> choices)
        {
            lock (locker)
            {
                if (GetAllChoicesAsync().Count > 0)
                {
                    _database.Execute("DELETE FROM Choice");
                    _database.Execute("DELETE FROM SQLITE_SEQUENCE WHERE NAME='Choice'");
                    return _database.InsertAll(choices);
                }

                return _database.InsertAll(choices);
            }
        }

        public int SaveResponseAsync(Response response)
        {
            lock (locker)
            {
                var result = _database.Table<Response>()
                            .Where(i => i.ResponseID == response.ResponseID
                                || (i.ResponseID == 0
                                    && i.TemplateID == response.TemplateID
                                    && i.CompanyID == response.CompanyID
                                    && i.BranchID == response.BranchID
                                    && i.UserID == response.UserID))
                            .FirstOrDefault();

                if (null == result)
                {
                    _database.Insert(response);

                    var savedResponse = _database.Table<Response>()
                    .Where(i => i.ResponseID == response.ResponseID
                        || (i.ResponseID == 0
                            && i.TemplateID == response.TemplateID
                            && i.CompanyID == response.CompanyID
                            && i.BranchID == response.BranchID
                            && i.UserID == response.UserID))
                    .FirstOrDefault();

                    if (savedResponse != null) return savedResponse.ID;
                } else
                {
                    response.ID = result.ID;
                    _database.Update(response);
                    return result.ID;
                }
                return 0;
            }
        }

        public int SaveAnswersAsync(IEnumerable<Answer> answer)
        {
            lock (locker)
            {
                return _database.InsertAll(answer);
            }
        }

        public int SaveAnswerAsync(Answer answer, int companyID, int branchID)
        {
            lock (locker)
            {
                int localResponseID = answer.LocalResponseID;
                if (answer.LocalResponseID == 0)
                {
                    var response = _database.Table<Response>()
                        .Where(res => res.TemplateID == answer.TemplateID
                            && res.CompanyID == companyID
                            && res.BranchID == branchID)
                        .FirstOrDefault();

                    if (response != null) localResponseID = response.ID;
                }

                var result = _database.Table<Answer>()
                    .Where(i => i.AnswerID == answer.AnswerID
                    || (i.AnswerID == 0
                        && i.LocalResponseID == localResponseID
                        && i.TemplateID == answer.TemplateID
                        && i.QuestionID == answer.QuestionID
                        && i.UserID == answer.UserID))
                    .FirstOrDefault();

                if (null == result)
                {
                    return _database.Insert(answer);
                }
                answer.ID = result.ID;
                return _database.Update(answer);
            }
        }

        public int SaveCompanyBranch(Company_Branch company_Branch)
        {
            lock (locker)
            {
                var result = _database.Table<Company_Branch>()
                    .Where(i => i.CompanyID == company_Branch.CompanyID && i.BranchID == company_Branch.BranchID)
                    .FirstOrDefault();

                if (null == result)
                    return _database.Insert(company_Branch);

                return 0;
            }
        }

        public int SaveTemplateBranch(Template_Branch template_Branch)
        {
            lock (locker)
            {
                var result = _database.Table<Template_Branch>()
                    .Where(i => i.TemplateID == template_Branch.TemplateID && i.BranchID == template_Branch.BranchID)
                    .FirstOrDefault();

                if (null == result)
                    return _database.Insert(template_Branch);
                return 0;
            }
        }

        public int UpdateResponseAsync(Response response)
        {
            lock (locker)
            {
                return _database.Update(response);
            }
        }

        public int UpdateAnswersAsync(IEnumerable<Answer> answer)
        {
            lock (locker)
            {
                return _database.UpdateAll(answer);
            }
        }

        public void UpdateResponseAnswerIDs (ResponseAnswerDetailsViewModel updated)
        {
            if (updated == null) return;

            Response res = _database.Table<Response>()
                .Where(response => response.ID == updated.LocalResponseID)
                .FirstOrDefault();

            res.ResponseID = updated.ResponseID;
            _database.Update(res);

            if (updated.Answers == null || updated.Answers.Count == 0) return;
            
            foreach (AnswerDetailsViewModel answerData in updated.Answers)
            {
                Answer ans = _database.Table<Answer>()
                    .Where(answer => answer.ID == answerData.LocalAnswerID)
                    .FirstOrDefault();

                ans.AnswerID = answerData.AnswerID;
                _database.Update(ans);
            }
        }
    }
}

