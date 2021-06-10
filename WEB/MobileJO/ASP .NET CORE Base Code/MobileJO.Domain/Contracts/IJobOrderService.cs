using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.JobOrder;
using System.Collections.Generic;
using System.Linq;
using System;
using MobileJO.Data.ViewModels.RevertJO;
using MobileJO.Data.ViewModels.AssignedCase;

namespace MobileJO.Domain.Contracts
{
    public interface IJobOrderService
    {
        List<JobOrder> RetrieveJobOrdersList();
        JobOrder RetrieveJobOrder(int jobOrderID);
        List<ApplicationType> RetrieveApplicationTypes();
        List<Account> RetrieveAccountsList();
        List<BillingType> RetrieveBillingTypes();
        List<AssignedCasesViewModel> RetrieveAssignedCases(string assignedTo, int applicationTypeId, int accountId);
        List<TaggedCase> RetrieveTaggedCases(int jobOrderID);
        List<JobOrderBillingType> RetrieveJOBillingTypes(int jobOrderID);
        List<Attachment> RetrieveJOAttachments(int jobOrderID);
        List<AssignedCasesViewModel> RetrieveUserCases(int assignedTo);

        int Create(JobOrderDetailsViewModel jobOrderViewModel, string attachmentPath);
        bool Update(JobOrderDetailsViewModel jobOrderViewModel, string attachmentPath);
        void Sync(JobOrderDetailsViewModel jobOrderViewModel, string attachmentPath);
        bool IsJobOrderExists(int jobOrderID);
        bool IsValidFiles(FileViewModel signatureFile, List<FileViewModel> attachmentsFiles);
        JobOrderViewModel Find(int id);
        IQueryable<JobOrder> RetrieveAll();
        ListViewModel Search(JobOrderSearchViewModel searchModel);
        void Delete(int id);
        void CreateJORevertRequest(int id, int userID);
        List<string> GetApplicationType();
        List<string> GetJobOrderStatusList();
        List<TaggedCasesListViewModel> GetTaggedCases(Data.ViewModels.JobOrder.TaggedCasesViewModel taggedCasesViewModel);
        AssignedCasesViewModel FindCase(int id);
        List<AttachmentListViewModel> GetAttachments(int id);
        List<JobOrderBillingTypeListViewModel> GetBillingList(int id);
        List<JobOrderViewModel> RetrieveUserJobOrders(int id);
        JobOrderRevertViewModel GetRequestCount(int id);
        List<TaggedCase> GetAllJoCases(string ids);
        List<JobOrderBillingType> GetAllJoBillingTypes(string ids);
        List<Attachment> GetAllJoAttachments(string ids);
        List<JobOrderStatus> GetAllStatus();
        bool UpdateSync(JobOrderDetailsViewModel jobOrderViewModel, string attachmentPath);
    }
}
