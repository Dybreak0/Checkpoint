using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.AssignedCase;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.JobOrder;
using MobileJO.Data.ViewModels.RevertJO;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Data.Contracts
{
    public interface IJobOrderRepository
    {
       
        List<JobOrder> RetrieveJobOrdersList();
        JobOrder Find(int id);
        List<ApplicationType> RetrieveApplicationTypes();
        List<Account> RetrieveAccountsList();
        List<BillingType> RetrieveBillingTypes();
        List<AssignedCasesViewModel> RetrieveAssignedCases(string assignedTo, int applicationTypeId, int accountId);
        List<JobOrderBillingType> RetrieveJOBillingTypes(int jobOrderID);
        List<TaggedCase> RetrieveTaggedCases(int jobOrderID);
        List<Attachment> RetrieveJOAttachments(int jobOrderID);
        List<AssignedCasesViewModel> RetrieveUserCases(int assignedTo);

        void DeleteJOCases(int jobOrderID);
        void DeleteJOBillingTypes(int jobOrderID);
        void DeleteAttachments(int jobOrderID, IEnumerable<string> filenames);

        int Create(JobOrder jobOrder,
                   List<TaggedCase> taggedCases,
                   List<JobOrderBillingType> joBillingTypes,
                   List<Attachment> joAttachments,
                   string signatureFilename);

        bool Update(JobOrder jobOrder, 
                    List<TaggedCase> newJOCases,
                    List<Attachment> newJOAttachments,
                    List<string> removedJOAttachments,
                    List<JobOrderBillingType> newJOBillingTypes);

        void Delete(int id);
        void CreateJORevertRequest(int id);
        List<string> GetApplicationType();
        List<string> GetJobOrderStatusList();
       
        List<TaggedCasesListViewModel> GetTaggedCases(ViewModels.JobOrder.TaggedCasesViewModel taggedCasesViewModel);
        AssignedCasesViewModel FindCase(int id);
        List<AttachmentListViewModel> GetAttachments(int id);
        List<JobOrderBillingTypeListViewModel> GetBillingType(int id);
        IQueryable<JobOrder> RetrieveAll();
        ListViewModel Search(JobOrderSearchViewModel searchModel);
        JobOrderViewModel FindJobOrder(int id);
        List<JobOrderViewModel> RetrieveUserJobOrders(int userId);
        JobOrderRevertViewModel GetRequestCount(int id);
        List<TaggedCase> GetAllJoCases(List<int> ids);
        List<JobOrderBillingType> GetAllJoBillingTypes(List<int> ids);
        List<Attachment> GetAllJoAttachments(List<int> ids);
        List<JobOrderStatus> GetAllStatus();

        bool UpdateSync(JobOrder jobOrder,
                    List<TaggedCase> newJOCases,
                    List<Attachment> newJOAttachments,
                    List<string> removedJOAttachments,
                    List<JobOrderBillingType> newJOBillingTypes);
    }
}
