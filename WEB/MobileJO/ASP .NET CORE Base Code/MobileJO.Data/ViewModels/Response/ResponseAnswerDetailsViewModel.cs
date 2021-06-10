namespace MobileJO.Data.ViewModels.Response
{
    public class ResponseAnswerDetailsViewModel
    {
        public int AnswerID { get; set; }
        public int ResponseID { get; set; }
        public int TemplateID { get; set; }
        public int QuestionID { get; set; }
        public int QuestionTypeID { get; set; }
        public int ChoiceID { get; set; }
        public int UserID { get; set; }
        public string Value { get; set; }
    }
}
