using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using MobileJO.Data;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.AssignedCase;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.JobOrder;
using MobileJO.Domain.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Domain.Services
{
    public class JobOrderService : IJobOrderService
    {
        private readonly IJobOrderRepository _jobOrderRepository;
        private readonly IEmailJOService _emailJOService;
        private readonly IUserService _userService;

        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        ///     Constructor for IJobOrderRepository and mapper
        /// </summary>
        /// <param name="jobOrderRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="emailJOService"></param>
        /// <param name="userService"></param>
        public JobOrderService(IJobOrderRepository jobOrderRepository,
                               IMapper mapper,
                               IEmailJOService emailJOService,
                               IUserService userService,
                               IHostingEnvironment hostingEnvironment)
        {
            _jobOrderRepository = jobOrderRepository;
            _mapper = mapper;
            _emailJOService = emailJOService;
            _userService = userService;
            _hostingEnvironment = hostingEnvironment;
        }

        // for testing only
        public List<JobOrder> RetrieveJobOrdersList()
        {
            return _jobOrderRepository.RetrieveJobOrdersList();
        }

        /// <summary>
        /// Calls Job Order Repository method Find() 
        /// </summary>
        /// <param name="jobOrderID"></param>
        /// <returns></returns>
        public JobOrder RetrieveJobOrder(int jobOrderID)
        {
            return _jobOrderRepository.Find(jobOrderID);
        }

        /// <summary>
        /// Calls Job Order Repository method RetrieveApplicationTypes()
        /// </summary>
        /// <returns></returns>
        public List<ApplicationType> RetrieveApplicationTypes()
        {
            return _jobOrderRepository.RetrieveApplicationTypes();
        }

        /// <summary>
        /// Calls Job Order Repository method RetrieveAccountsList()
        /// </summary>
        /// <returns></returns>
        public List<Account> RetrieveAccountsList()
        {
            return _jobOrderRepository.RetrieveAccountsList();
        }

        /// <summary>
        /// Calls Job Order Repository method RetrieveBillingTypes()
        /// </summary>
        /// <returns></returns>
        public List<BillingType> RetrieveBillingTypes()
        {
            return _jobOrderRepository.RetrieveBillingTypes();
        }

        /// <summary>
        /// Calls Job Order Repository method RetrieveAssignedCases()
        /// </summary>
        /// <returns></returns>
        public List<AssignedCasesViewModel> RetrieveAssignedCases(string assignedTo, int applicationTypeId, int accountId)
        {
            return _jobOrderRepository.RetrieveAssignedCases(assignedTo, applicationTypeId, accountId);
        }

        /// <summary>
        /// Calls Job Order Repository method RetrieveJOBillingTypes()
        /// </summary>
        /// <param name="jobOrderID"></param>
        /// <returns></returns>
        public List<JobOrderBillingType> RetrieveJOBillingTypes(int jobOrderID)
        {
            return _jobOrderRepository.RetrieveJOBillingTypes(jobOrderID);
        }

        /// <summary>
        /// Calls Job Order Repository method RetrieveTaggedCases()
        /// </summary>
        /// <param name="jobOrderID"></param>
        /// <returns></returns>
        public List<TaggedCase> RetrieveTaggedCases(int jobOrderID)
        {
            return _jobOrderRepository.RetrieveTaggedCases(jobOrderID);
        }

        /// <summary>
        /// Calls Job Order Repository method RetrieveJOAttachments()
        /// </summary>
        /// <param name="jobOrderID"></param>
        /// <returns></returns>
        public List<Attachment> RetrieveJOAttachments(int jobOrderID)
        {
            return _jobOrderRepository.RetrieveJOAttachments(jobOrderID);
        }

        /// <summary>
        /// Calls Job Order Repository method RetrieveUserCases()
        /// </summary>
        /// <param name="assignedTo"></param>
        /// <returns></returns>
        public List<AssignedCasesViewModel> RetrieveUserCases(int assignedTo)
        {
            return _jobOrderRepository.RetrieveUserCases(assignedTo);
        }

        /// <summary>
        /// Calls Job Order Repository methods for saving job orders, tagged cases, billing types and attachments
        /// </summary>
        /// <param name="jobOrderViewModel"></param>
        /// <returns></returns>
        public int Create(JobOrderDetailsViewModel jobOrderViewModel, string attachmentPath)
        {
            // ================== Check first if all files passed are valid ================== //
            if (IsValidFiles(jobOrderViewModel.Signature,
                             jobOrderViewModel.JobOrderAttachments) == false)
            {
                return 0;
            }

            JobOrder jobOrder = new JobOrder
            {
                JobOrderNumber = jobOrderViewModel.JobOrderNumber,
                JobOrderSubject = jobOrderViewModel.JobOrderSubject,
                StatusID = jobOrderViewModel.StatusID,
                AccountID = jobOrderViewModel.AccountID,
                ApplicationTypeID = jobOrderViewModel.ApplicationTypeID,
                Branch = jobOrderViewModel.Branch,
                DateTimeStart = jobOrderViewModel.DateTimeStart,
                DateTimeEnd = jobOrderViewModel.DateTimeEnd,
                ActivityDetails = jobOrderViewModel.ActivityDetails,
                RootCauseAnalysis = jobOrderViewModel.RootCauseAnalysis,
                NextStep = jobOrderViewModel.NextStep,
                PreventiveAction = jobOrderViewModel.PreventiveAction,
                Remarks = jobOrderViewModel.Remarks,
                Attendees = jobOrderViewModel.Attendees,
                IsBilled = jobOrderViewModel.IsBilled,
                IsCollaterals = jobOrderViewModel.IsCollaterals,
                IsFixed = jobOrderViewModel.IsFixed,
                IsSatisfied = jobOrderViewModel.IsSatisfied,
                ClientSignature = jobOrderViewModel.ClientSignature,
                ClientRating = jobOrderViewModel.ClientRating,
                CreatedDate = jobOrderViewModel.CreatedDate,
                CreatedBy = jobOrderViewModel.CreatedBy,
                UpdatedDate = jobOrderViewModel.UpdatedDate,
                UpdatedBy = jobOrderViewModel.UpdatedBy,
                LastSyncDate = jobOrderViewModel.UpdatedDate
            };

            var cases = new List<TaggedCase>();
            var attachments = new List<Attachment>();
            var billingTypes = new List<JobOrderBillingType>();
            var signatureFilename = jobOrderViewModel.Signature != null &&
                                    jobOrderViewModel.Signature.FileDataArray != null ?
                                    Constants.Common.SignatureNameExtension :
                                    null;

            foreach (int caseID in jobOrderViewModel.NewJOCases)
            {
                cases.Add(new TaggedCase { CaseID = caseID });
            }

            foreach (FileViewModel file in jobOrderViewModel.JobOrderAttachments)
            {
                attachments.Add(new Attachment { Filename = file.FileName });
            }

            foreach (int billingTypeID in jobOrderViewModel.NewJOBillingTypes)
            {
                billingTypes.Add(new JobOrderBillingType { BillingTypeID = billingTypeID });
            }

            int jobOrderId = _jobOrderRepository.Create(jobOrder,
                                                         cases,
                                                         billingTypes,
                                                         attachments,
                                                         signatureFilename);

            if (jobOrderId > 0)
            {
                if (jobOrderViewModel.Signature != null &&
                    jobOrderViewModel.Signature.FileDataArray != null)
                {
                    string filename = string.Concat(jobOrderId, Constants.Common.SignatureNameExtension);

                    jobOrderViewModel.Signature.FileName = filename;

                    SaveFiles(jobOrderId,
                                new List<FileViewModel> { jobOrderViewModel.Signature },
                                attachmentPath,
                                Constants.Upload.ClientSignature);
                }

                if (jobOrderViewModel.JobOrderAttachments.Count > 0)
                {
                    SaveFiles(jobOrderId,
                               jobOrderViewModel.JobOrderAttachments,
                               attachmentPath,
                               Constants.Upload.Attachment);
                }
            }

            return jobOrderId;

        }

        /// <summary>
        /// Calls Job Order Repository methods for updating job orders, tagged cases, billing types and attachments
        /// </summary>
        /// <param name="jobOrderViewModel"></param>
        public bool Update(JobOrderDetailsViewModel jobOrderViewModel, string attachmentPath)
        {
            if (IsValidFiles(jobOrderViewModel.Signature,
                             jobOrderViewModel.JobOrderAttachments) == false)
            {
                return false;
            }

            int id = jobOrderViewModel.ID;
            string signatureName = null;

            if (!string.IsNullOrEmpty(jobOrderViewModel.ClientSignature))
            {
                signatureName = jobOrderViewModel.ClientSignature;
            }
            else if (jobOrderViewModel.Signature != null &&
                    !string.IsNullOrEmpty(jobOrderViewModel.Signature.FileName) &&
                    string.IsNullOrEmpty(jobOrderViewModel.ClientSignature))
            {
                signatureName = string.Concat(id, jobOrderViewModel.Signature.FileName);
            }

            JobOrder jobOrderUpdate = new JobOrder();
            jobOrderUpdate.ID = id;
            jobOrderUpdate.JobOrderNumber = jobOrderViewModel.JobOrderNumber;
            jobOrderUpdate.JobOrderSubject = jobOrderViewModel.JobOrderSubject;
            jobOrderUpdate.StatusID = jobOrderViewModel.StatusID;
            jobOrderUpdate.AccountID = jobOrderViewModel.AccountID;
            jobOrderUpdate.ApplicationTypeID = jobOrderViewModel.ApplicationTypeID;
            jobOrderUpdate.Branch = jobOrderViewModel.Branch;
            jobOrderUpdate.DateTimeStart = jobOrderViewModel.DateTimeStart;
            jobOrderUpdate.DateTimeEnd = jobOrderViewModel.DateTimeEnd;
            jobOrderUpdate.ActivityDetails = jobOrderViewModel.ActivityDetails;
            jobOrderUpdate.RootCauseAnalysis = jobOrderViewModel.RootCauseAnalysis;
            jobOrderUpdate.NextStep = jobOrderViewModel.NextStep;
            jobOrderUpdate.PreventiveAction = jobOrderViewModel.PreventiveAction;
            jobOrderUpdate.Remarks = jobOrderViewModel.Remarks;
            jobOrderUpdate.Attendees = jobOrderViewModel.Attendees;
            jobOrderUpdate.IsBilled = jobOrderViewModel.IsBilled;
            jobOrderUpdate.IsCollaterals = jobOrderViewModel.IsCollaterals;
            jobOrderUpdate.IsFixed = jobOrderViewModel.IsFixed;
            jobOrderUpdate.IsSatisfied = jobOrderViewModel.IsSatisfied;
            jobOrderUpdate.ClientSignature = signatureName;
            jobOrderUpdate.ClientRating = jobOrderViewModel.ClientRating;
            jobOrderUpdate.UpdatedBy = jobOrderViewModel.UpdatedBy;
            jobOrderUpdate.UpdatedDate = jobOrderViewModel.UpdatedDate;
            jobOrderUpdate.LastSyncDate = jobOrderViewModel.LastSyncDate;

            List<TaggedCase> newCases = new List<TaggedCase>();
            List<Attachment> newAttachments = new List<Attachment>();
            List<JobOrderBillingType> newBillingTypes = new List<JobOrderBillingType>();

            foreach (var newcase in jobOrderViewModel.NewJOCases)
            {
                newCases.Add(new TaggedCase { JobOrderID = id, CaseID = newcase });
            }

            foreach (var attachment in jobOrderViewModel.JobOrderAttachments)
            {
                newAttachments.Add(new Attachment { JobOrderID = id, Filename = attachment.FileName });
            }

            foreach (var billingType in jobOrderViewModel.NewJOBillingTypes)
            {
                newBillingTypes.Add(new JobOrderBillingType { JobOrderID = id, BillingTypeID = billingType });
            }

            var result = _jobOrderRepository.Update(jobOrderUpdate,
                                                     newCases,
                                                     newAttachments,
                                                     jobOrderViewModel.RemovedAttachments,
                                                     newBillingTypes);

            if (jobOrderViewModel.Signature != null &&
                    jobOrderViewModel.Signature.FileDataArray != null)
            {
                string filename = string.Concat(id, Constants.Common.SignatureNameExtension);

                jobOrderViewModel.Signature.FileName = filename;

                SaveFiles(id,
                          new List<FileViewModel> { jobOrderViewModel.Signature },
                          attachmentPath,
                          Constants.Upload.ClientSignature);
            }

            UpdateFileAttachments(id,
                                  jobOrderViewModel.RemovedAttachments,
                                  jobOrderViewModel.JobOrderAttachments,
                                  attachmentPath);

            return result;

        }

        /// <summary>
        /// Calls Job Order Repository methods for syncing job orders, tagged cases, billing types and attachments
        /// </summary>
        /// <param name="jobOrderDetails"></param>
        public void Sync(JobOrderDetailsViewModel jobOrderDetails, string attachmentPath)
        {
            if (jobOrderDetails.JobOrderNumber == null)
            {
                Create(jobOrderDetails, attachmentPath);
            }
            else
            {
                UpdateSync(jobOrderDetails, attachmentPath);
            }

        }

        /// <summary>
        /// Calls Job Order Repository method Find() to check if job order exists
        /// </summary>
        /// <param name="jobOrderID"></param>
        /// <returns></returns>
        public bool IsJobOrderExists(int jobOrderID)
        {
            if (_jobOrderRepository.Find(jobOrderID) != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calls Helper method SaveFileToServer() to save signature to files
        /// </summary>
        /// <param name="jobOrderID"></param>
        /// <param name="fileModelList"></param>
        /// <param name="attachmentPath"></param>
        /// <param name="targetFolder"></param>
        /// <returns></returns>
        public bool SaveFiles(int jobOrderID,
                              List<FileViewModel> fileModelList,
                              string attachmentPath,
                              string attachmentType)
        {
            bool isSaved = false;

            var rootPath = _hostingEnvironment.ContentRootPath;
            var filePath = string.Format(Constants.Attachment.FullFilePath, rootPath, attachmentPath, jobOrderID, attachmentType);

            if (!Helper.DoesPathExist(filePath))
            {
                Helper.CreateDirectory(filePath);
            }

            isSaved = Helper.SaveFileToServer(fileModelList, filePath);

            return isSaved;
        }

        /// <summary>
        /// Calls Helper method SaveFileToServer() and Job Order Repository method CreateJobOrderAttachments
        /// </summary>
        /// <param name="jobOrderID"></param>
        /// <param name="removedAttachments"></param>
        /// <param name="newAttachments"></param>
        public void UpdateFileAttachments(int jobOrderID, List<string> removedAttachments, List<FileViewModel> newAttachments, string attachmentPath)
        {
            var rootPath = _hostingEnvironment.ContentRootPath;
            var filePath = string.Format(Constants.Attachment.FullFilePath, rootPath, attachmentPath, jobOrderID, Constants.Upload.Attachment);

            if (removedAttachments.Count > 0)
            {
                List<string> removedAttachmentNames = new List<string>();

                foreach (var removedAttachment in removedAttachments)
                {
                    removedAttachmentNames.Add(removedAttachment);
                }

                if (Helper.DoesPathExist(filePath) == true)
                {
                    Helper.DeleteFiles(removedAttachmentNames, filePath);
                }
            }

            if (newAttachments.Count > 0)
            {
                SaveFiles(jobOrderID,
                          newAttachments,
                          attachmentPath,
                          Constants.Upload.Attachment);
            }
        }

        /// <summary>
        /// Calls Helper method IsValidFile() to check if files uploaded are valid files
        /// </summary>
        /// <param name="signatureFile"></param>
        /// <param name="attachmentsFiles"></param>
        /// <returns></returns>
        public bool IsValidFiles(FileViewModel signatureFile, List<FileViewModel> attachmentsFiles)
        {
            var filesToCheck = new List<FileViewModel>();

            if (signatureFile != null &&
                signatureFile.FileDataArray != null) { filesToCheck.Add(signatureFile); }

            if (attachmentsFiles.Count > 0)
            {
                foreach (var attachment in attachmentsFiles) { filesToCheck.Add(attachment); }
            }

            if (Helper.IsValidFile(filesToCheck) == false) { return false; }

            return true;
        }

        /// <summary>
        ///     Calls JobOrder repository method Find().
        /// </summary>
        /// <returns></returns>
        public JobOrderViewModel Find(int id)
        {
            JobOrderViewModel jobOrderViewModel = null;
            var joborder = _jobOrderRepository.FindJobOrder(id);

            if (joborder != null)
            {
                jobOrderViewModel = _mapper.Map<JobOrderViewModel>(joborder);
            }

            return jobOrderViewModel;
        }

        /// <summary>
        ///     Calls JobOrder repository method RetrieveAll().
        /// </summary>
        /// <returns></returns>
        public IQueryable<JobOrder> RetrieveAll()
        {
            return _jobOrderRepository.RetrieveAll();
        }

        /// <summary>
        ///     Calls JobOrder repository method Search().
        /// </summary>
        /// <returns></returns>
        public ListViewModel Search(JobOrderSearchViewModel searchModel)
        {
            return _jobOrderRepository.Search(searchModel);
        }

        /// <summary>
        ///     Calls JobOrder repository method Delete().
        /// </summary>
        /// <returns></returns>
        public void Delete(int id)
        {
            _jobOrderRepository.Delete(id);
        }

        /// <summary>
        ///     Calls JobOrder repository method CreateJORevertRequest().
        ///     Calls EmailJO service method SendEmailJORevert().
        /// </summary>
        /// <returns></returns>
        public void CreateJORevertRequest(int id, int userID)
        {
            _jobOrderRepository.CreateJORevertRequest(id);

            var jobOrder = _jobOrderRepository.Find(id);
            string jobOrderNumber = jobOrder.JobOrderNumber;

            var user = _userService.Find(userID);
            var userVm = _mapper.Map<User>(user);
            string firstname = user.FirstName;

            _emailJOService.SendEmailJORevert(jobOrderNumber, userVm);
        }

        /// <summary>
        ///     Calls JobOrder repository method GetApplicationType().
        /// </summary>
        /// <returns></returns>
        public List<string> GetApplicationType()
        {
            return _jobOrderRepository.GetApplicationType();
        }

        /// <summary>
        ///     Calls JobOrder repository method GetJobOrderStatusList().
        /// </summary>
        /// <returns></returns>
        public List<string> GetJobOrderStatusList()
        {
            return _jobOrderRepository.GetJobOrderStatusList();
        }

        /// <summary>
        ///     Calls JobOrder repository method GetTaggedCases().
        /// </summary>
        /// <returns></returns>
        public List<TaggedCasesListViewModel> GetTaggedCases(Data.ViewModels.JobOrder.TaggedCasesViewModel taggedCasesViewModel)
        {
            return _jobOrderRepository.GetTaggedCases(taggedCasesViewModel);
        }

        /// <summary>
        ///     Calls JobOrder repository method FindCase().
        /// </summary>
        /// <returns></returns>
        public AssignedCasesViewModel FindCase(int id)
        {
            AssignedCasesViewModel taggedCaseViewModel = null;

            var tagCase = _jobOrderRepository.FindCase(id);

            if (tagCase != null)
            {
                taggedCaseViewModel = _mapper.Map<AssignedCasesViewModel>(tagCase);
            }

            return taggedCaseViewModel;
        }

        public JobOrderRevertViewModel GetRequestCount(int id)
        {
            return _jobOrderRepository.GetRequestCount(id);
        }


        // <summary>
        //     Calls JobOrder repository method GetAttachments().
        // </summary>
        // <returns></returns>
        public List<AttachmentListViewModel> GetAttachments(int id)
        {
            return _jobOrderRepository.GetAttachments(id);
        }

        public List<JobOrderBillingTypeListViewModel> GetBillingList(int id)
        {
            return _jobOrderRepository.GetBillingType(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public List<JobOrderViewModel> RetrieveUserJobOrders(int userId)
        {
            return _jobOrderRepository.RetrieveUserJobOrders(userId);
        }

        public List<TaggedCase> GetAllJoCases(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return new List<TaggedCase>();


            var joIDs = ids.Split(',').Select(int.Parse).ToList();

            return _jobOrderRepository.GetAllJoCases(joIDs);
        }

        public List<JobOrderBillingType> GetAllJoBillingTypes(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return new List<JobOrderBillingType>();

            var joIDs = ids.Split(',').Select(int.Parse).ToList();

            return _jobOrderRepository.GetAllJoBillingTypes(joIDs);            
        }

        public List<Attachment> GetAllJoAttachments(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return new List<Attachment>();

            var joIDs = ids.Split(',').Select(int.Parse).ToList();

            return _jobOrderRepository.GetAllJoAttachments(joIDs);
        }

        public List<JobOrderStatus> GetAllStatus()
        {
            return _jobOrderRepository.GetAllStatus();
        }

        /// <summary>
        /// Calls Job Order Repository methods for sync update of the job orders, tagged cases, billing types and attachments
        /// </summary>
        /// <param name="jobOrderViewModel"></param>
        public bool UpdateSync(JobOrderDetailsViewModel jobOrderViewModel, string attachmentPath)
        {
            if (IsValidFiles(jobOrderViewModel.Signature,
                             jobOrderViewModel.JobOrderAttachments) == false)
            {
                return false;
            }

            int id = jobOrderViewModel.ID;
            string signatureName = null;

            if (!string.IsNullOrEmpty(jobOrderViewModel.ClientSignature))
            {
                signatureName = jobOrderViewModel.ClientSignature;
            }
            else if (jobOrderViewModel.Signature != null &&
                    !string.IsNullOrEmpty(jobOrderViewModel.Signature.FileName) &&
                    string.IsNullOrEmpty(jobOrderViewModel.ClientSignature))
            {
                signatureName = string.Concat(id, jobOrderViewModel.Signature.FileName);
            }

            JobOrder jobOrderUpdate = new JobOrder();
            jobOrderUpdate.ID = id;
            jobOrderUpdate.JobOrderNumber = jobOrderViewModel.JobOrderNumber;
            jobOrderUpdate.JobOrderSubject = jobOrderViewModel.JobOrderSubject;
            jobOrderUpdate.StatusID = jobOrderViewModel.StatusID;
            jobOrderUpdate.AccountID = jobOrderViewModel.AccountID;
            jobOrderUpdate.ApplicationTypeID = jobOrderViewModel.ApplicationTypeID;
            jobOrderUpdate.Branch = jobOrderViewModel.Branch;
            jobOrderUpdate.DateTimeStart = jobOrderViewModel.DateTimeStart;
            jobOrderUpdate.DateTimeEnd = jobOrderViewModel.DateTimeEnd;
            jobOrderUpdate.ActivityDetails = jobOrderViewModel.ActivityDetails;
            jobOrderUpdate.RootCauseAnalysis = jobOrderViewModel.RootCauseAnalysis;
            jobOrderUpdate.NextStep = jobOrderViewModel.NextStep;
            jobOrderUpdate.PreventiveAction = jobOrderViewModel.PreventiveAction;
            jobOrderUpdate.Remarks = jobOrderViewModel.Remarks;
            jobOrderUpdate.Attendees = jobOrderViewModel.Attendees;
            jobOrderUpdate.IsBilled = jobOrderViewModel.IsBilled;
            jobOrderUpdate.IsCollaterals = jobOrderViewModel.IsCollaterals;
            jobOrderUpdate.IsFixed = jobOrderViewModel.IsFixed;
            jobOrderUpdate.IsSatisfied = jobOrderViewModel.IsSatisfied;
            jobOrderUpdate.IsDeleted = jobOrderViewModel.IsDeleted;
            jobOrderUpdate.ClientSignature = signatureName;
            jobOrderUpdate.ClientRating = jobOrderViewModel.ClientRating;
            jobOrderUpdate.UpdatedBy = jobOrderViewModel.UpdatedBy;
            jobOrderUpdate.UpdatedDate = jobOrderViewModel.UpdatedDate;
            jobOrderUpdate.LastSyncDate = jobOrderViewModel.LastSyncDate;

            List<TaggedCase> newCases = new List<TaggedCase>();
            List<Attachment> newAttachments = new List<Attachment>();
            List<JobOrderBillingType> newBillingTypes = new List<JobOrderBillingType>();

            foreach (var newcase in jobOrderViewModel.NewJOCases)
            {
                newCases.Add(new TaggedCase { JobOrderID = id, CaseID = newcase });
            }

            foreach (var attachment in jobOrderViewModel.JobOrderAttachments)
            {
                newAttachments.Add(new Attachment { JobOrderID = id, Filename = attachment.FileName });
            }

            foreach (var billingType in jobOrderViewModel.NewJOBillingTypes)
            {
                newBillingTypes.Add(new JobOrderBillingType { JobOrderID = id, BillingTypeID = billingType });
            }

            var result = _jobOrderRepository.UpdateSync(jobOrderUpdate,
                                                        newCases,
                                                        newAttachments,
                                                        jobOrderViewModel.RemovedAttachments,
                                                        newBillingTypes);

            if (jobOrderViewModel.Signature != null &&
                    jobOrderViewModel.Signature.FileDataArray != null)
            {
                string filename = string.Concat(id, Constants.Common.SignatureNameExtension);

                jobOrderViewModel.Signature.FileName = filename;

                SaveFiles(id,
                          new List<FileViewModel> { jobOrderViewModel.Signature },
                          attachmentPath,
                          Constants.Upload.ClientSignature);
            }

            UpdateFileAttachments(id,
                                  jobOrderViewModel.RemovedAttachments,
                                  jobOrderViewModel.JobOrderAttachments,
                                  attachmentPath);

            return result;

        }
    }
}
