using SQLite;
using System;

namespace MobileJO.Core.Models
{
    public class Answer
    {
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }
        public int LocalResponseID { get; set; }
        public int AnswerID { get; set; }
        public int ResponseID { get; set; }
        public int TemplateID { get; set; }
        public int QuestionID { get; set; }
        public int ChoiceID { get; set; }
        public int UserID { get; set; }
        public string Value { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime LastSyncDate { get; set; }
    }
}
