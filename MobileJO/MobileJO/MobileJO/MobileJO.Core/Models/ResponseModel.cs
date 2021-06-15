namespace MobileJO.Core.Models
{
    public class ResponseModel
    {
        public int LocalResponseID { get; set; }
        public int ResponseID { get; set; }
        public int TemplateID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DateSubmitted { get; set; }
        public bool Status { get; set; }
        public string StatusText { get; set; }
        public string Color { get; set; }
        public int MaxLimit { get; set; }
    }
}
