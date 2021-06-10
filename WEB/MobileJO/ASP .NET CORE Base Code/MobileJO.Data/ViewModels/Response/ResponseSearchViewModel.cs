using Newtonsoft.Json;
using System;

namespace MobileJO.Data.ViewModels.Response
{
    public class ResponseSearchViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }
        [JsonProperty("template_id")]
        public string TemplateID { get; set; }

        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("branch_id")]
        public string BranchID { get; set; }

        [JsonProperty("company_id")]
        public string CompanyID { get; set; }

        [JsonProperty("user_type_id")]
        public string UserTypeID { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }
    }
}
