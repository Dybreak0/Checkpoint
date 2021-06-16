using System.Collections.Generic;

namespace MobileJO.Core.Models
{
    public class ResponseDetailsModel
    {
        public int LocalResponseID { get; set; }
        public int ResponseID { get; set; }
        public int TemplateID { get; set; }
        public int UserID { get; set; }
        public int BranchID { get; set; }
        public int CompanyID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxLimit { get; set; }
        public bool Status { get; set; }
        public List<QuestionModel> QuestionList { get; set; }
        public bool CanEditAnswer { get; set; }
    }
}
