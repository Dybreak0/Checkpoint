using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using MobileJO.Data.ViewModels.EmailJO;

namespace MobileJO.Data.Contracts
{
    public interface IEmailJORepository
    {
        ListViewModel GetAll();
        void SaveEmails(string emailsJson);
        List<JobOrder> SearchJobOrder(List<string> jobOrderIDs);

        List<EmailAddressViewModel> GetAllEmailRecipient();
        List<SelectJOViewModel> GetTaggedCases(int createdBy, int caseID);
        bool UpdateJobOrder(int jobOrderID, int userID);
        JobOrder Find(int id);        
        List<string> GetAllAdminEmailRecipient();
        int GetEmailCount();
    }
}
