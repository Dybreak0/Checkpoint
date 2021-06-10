namespace MobileJO.Core.ViewModels.QuestionnaireListViewModels
{
    public class QuestionnaireSearchViewModel
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string CompanyID { get; set; }
        public string BranchID { get; set; }
        public string UserTypeID { get; set; }
        public string IsMobile { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
