using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.Questionnaire
{
    public class QuestionDetailsViewModel
    {
        public int QuestionID { get; set; }
        public int TemplateID { get; set; }
        public string Qquestion { get; set; }
        public int QuestionTypeID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
    }
}
