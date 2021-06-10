using Newtonsoft.Json;
using System;

namespace MobileJO.Data.ViewModels.Reports
{
    public class JobOrderClientRatingReportViewModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("job_order_number")]
        public string JobOrderNumber { get; set; }

        [JsonProperty("case_number")]
        public string CaseNumber { get; set; }

        [JsonProperty("reported_by_name")]
        public string ReportedByName { get; set; }

        [JsonProperty("application_type_name")]
        public string ApplicationTypeName { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("client_rating")]
        public int ClientRating { get; set; }

        [JsonProperty("job_order_datetime_start")]
        public DateTime DateTimeStart { get; set; }

        [JsonProperty("job_order_datetime_end")]
        public DateTime DateTimeEnd { get; set; }
    }
}
