using Newtonsoft.Json;
using System;

namespace MobileJO.Data.ViewModels.Reports
{
    public class AssignedCasesReportViewModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("case_number")]
        public string CaseNumber { get; set; }

        [JsonProperty("case_subject")]
        public string CaseSubject { get; set; }

        [JsonProperty("application_type_name")]
        public string ApplicationTypeName { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("assigned_to")]
        public string AssignedTo { get; set; }

        [JsonProperty("assigned_to_name")]
        public string FullName { get; set; }

        [JsonProperty("datetime_modified")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("datetime_created")]
        public DateTime CreatedDate { get; set; }
    }
}
