using SQLite;
using System;

namespace MobileJO.Core.Models
{
    public class Question
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
