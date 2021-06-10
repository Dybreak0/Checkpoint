using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.Questionnaire
{
    public class QuestionnaireDetailsViewModel
    {
        public int TemplateID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxLimit { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }

        public List<int> BranchIds { get; set; }
    }
}
