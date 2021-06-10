using System;

namespace MobileJO.Core.ViewModels.FieldViewModels
{
    public class AnswerDetailsViewModel
    {
        public int LocalAnswerID { get; set; }
        public int AnswerID { get; set; }
        public int ResponseID { get; set; }
        public int TemplateID { get; set; }
        public int QuestionID { get; set; }
        public int ChoiceID { get; set; }
        public int UserID { get; set; }
        public string Value { get; set; }
        public byte[] UploadedFile { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime LastSyncDate { get; set; }
    }
}
