using Newtonsoft.Json;
using System;

namespace MobileJO.Data.ViewModels.Reports
{
    public class JobOrderReportSearchViewModel
    {
        [JsonProperty("job_order_number")]
        public string JobOrderNumber { get; set; }

        [JsonProperty("case_number")]
        public string CaseNumber { get; set; }

        [JsonProperty("reported_by")]
        public string ReportedBy { get; set; }

        [JsonProperty("application_type")]
        public int ApplicationType { get; set; }

        [JsonProperty("account_id")]
        public int AccountID { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; } 

        [JsonProperty("job_order_date_from")]
        public DateTime JobOrderDateFrom { get; set; }

        [JsonProperty("job_order_date_to")]
        public DateTime JobOrderDateTo { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }
    }
}
