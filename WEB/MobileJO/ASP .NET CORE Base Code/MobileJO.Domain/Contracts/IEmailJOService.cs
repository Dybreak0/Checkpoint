using System;
using System.Collections.Generic;
using System.Text;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using System.Linq;
using Newtonsoft.Json.Linq;
using MobileJO.Data.ViewModels.EmailJO;

namespace MobileJO.Domain.Contracts
{
    public interface IEmailJOService
    {
        ListViewModel GetAll ();
        void SaveEmails(string emailsJson);
        List<JobOrder> SearchJobOrder(List<string> jobOrderIDs);
        void SendEmailJO(EmailJOViewModel emailDetails, int userID, string fullname);
        List<SelectJOViewModel> GetTaggedCases(int createdBy, int caseID);

       
        void SendEmailJORevert(string id, User user);

        int EmailCount();
    }
}
