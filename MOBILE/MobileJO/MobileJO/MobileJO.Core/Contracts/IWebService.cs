using MobileJO.Core.Models;
using MobileJO.Core.ViewModels.AssignedCases;
using MobileJO.Core.ViewModels.Common;
using MobileJO.Core.Views.EmailJO;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.EmailJO;
using System.Collections.Generic;
using MobileJO.Core.ViewModels;
using System.Threading.Tasks;
using MobileJO.Core.ViewModels.QuestionnaireListViewModels;
using MobileJO.Core.ViewModels.ResponseListViewModels;
using MobileJO.Core.ViewModels.FieldViewModels;
using Plugin.Media.Abstractions;

namespace MobileJO.Core.Contracts
{
    public interface IWebService
    {
        Task<List<Account>> AccountList();
        Task<List<ApplicationType>> ApplicationTypeList();        
        Task<List<BillingTypes>> BillingTypeList();
        Task<List<JobOrderBillingType>> JobOrderBillingTypeList(int jobOrderID);
        Task<JobOrderDetailsViewModel> SaveJobOrderDetails(JobOrderDetailsViewModel jobOrderViewModel);
        Task<bool> UpdateJobOrderDetails(JobOrderDetailsViewModel jobOrderViewModel);
        Task<List<AssignedCases>> AssignedCaseList(string assignedTo, int applicationTypeId, int accountId);
        Task<List<TaggedCase>> TaggedCaseList(int jobOrderID);
        Task<List<Attachment>> AttachmentList(int jobOrderID);        
        Task<JobOrder> GetJobOrder(int jobOrderID);
        Task<List<JobOrder>> JobOrdersList();
        Task<bool> SyncJobOrderDetails(JobOrderDetailsViewModel jobOrderViewModel);        
        Task<CasesListViewModel> AssignedCasesList(Dictionary<string, string> searchViewModel);
        Task<AssignedCase> AssignedCase(int id);
        Task<bool> SendEmailJO(EmailJOModel emailDetails);
        Task<List<SelectJOModel>> SelectJOList(int created_by, int case_id);
        Task<List<string>> GetCaseStatus();
        Task<List<string>> GetApplicationType();
        Task<bool> Login(UserCredentialsModel user);
        Task<ForgotPasswordResponseModel> ResetPassword(EmailModel emailModel); 
        Task<PaginationViewModel> GetJobOrderList(Dictionary<string, string> jobOrder);
        Task<JobOrder> JobOrderDetail(int id);
        Task<bool> DeleteJobOrder(int id);
        Task<RevertModel> GetRevertCount(int id);
        Task<bool> RevertJobOrder(int id);
        Task<List<string>> GetJobOrderStatus();
        Task<List<TaggedCaseModel>> TaggedCasesList(Dictionary<string, string> taggedCases);
        Task<TaggedCase> TagCaseDetail(int id);
        Task<List<JobOrderBillingTypeModel>> GetBillingList(int jobOrderID);
        Task<List<AttachmentModel>> GetAttachmentList(int jobOrderID);
        Task<byte[]> DownloadFile(Dictionary<string, string> path);
        Task<bool> Allowed(int id);
        Task<List<AssignedCases>> UserAssignedCasesList(int assignedTo);
        Task<List<JobOrder>> GetAllUserJobOrders(int userId);
        Task<List<AssignedCasesList>> FindCases(string ids);
        Task<List<TaggedCase>> JOCaseList(string ids);
        Task<List<JobOrderBillingType>> JOBillingTypeList(string ids);
        Task<List<AttachmentModel>> JOAttachmentList(string ids);
        Task<List<JobOrderStatus>> JOStatusList();
        Task<List<DropdownViewModel>> GetBranches(int companyID);
        Task<ResponseDetailsModel> GetResponseByResponseID(int responseID, int templateID);
        Task<List<DropdownViewModel>> GetCompanies(int templateID);
        Task<List<DropdownViewModel>> GetCompanies();
        Task<QuestionnairePaginationViewModel> GetQuestionnaireList(Dictionary<string, string> template);
        Task<ResponseDataViewModel> GetResponseList(Dictionary<string, string> response);
        Task<ResponseAnswerModel> SaveResponse(ResponseAnswerModel responseAnswer);
        Task<ResponseAnswerDetailsViewModel> SyncResponseAndAnswerDetails(ResponseAnswerDetailsViewModel responseAnswerDetailsViewModel);
        Task<List<QuestionnaireViewModel>> GetAllTemplates();
        Task<List<ResponseAnswerDetailsViewModel>> GetAllResponseByTemplateID(int templateID);
        Task<DataViewModel> UploadMedia(MediaFile uploadedFile);
    }
}
