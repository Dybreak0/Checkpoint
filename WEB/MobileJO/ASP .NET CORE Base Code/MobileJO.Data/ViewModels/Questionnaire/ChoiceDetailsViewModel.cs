using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.Questionnaire
{
    public class ChoiceDetailsViewModel
    {
        public int ChoiceID { get; set; }
        public int QuestionID { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
    }
}

