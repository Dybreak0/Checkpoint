using MobileJO.Core.ViewModels.FieldViewModels;
using System;
using System.Collections.Generic;

namespace MobileJO.Core.ViewModels
{
    public class ResponseAnswerDetailsViewModel
    {
        public int LocalResponseID { get; set; }
        public int ResponseID { get; set; }
        public int TemplateID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Remarks { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime LastSyncDate { get; set; }
        public List<AnswerDetailsViewModel> Answers { get; set; }
    }
}
