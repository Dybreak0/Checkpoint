using System;

namespace MobileJO.Core.ViewModels.ResponseListViewModels
{
    public class ResponseSearchViewModel
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TemplateID { get; set; }
        public string UserID { get; set; }
        public string BranchID { get; set; }
        public string CompanyID { get; set; }
        public string UserTypeID { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
