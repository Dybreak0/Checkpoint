using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.JobOrder
{
    public class TaggedCaseViewModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("case_number")]
        public string CaseNumber { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("application_type")]
        public string ApplicationType { get; set; }

        [JsonProperty("case_subject")]
        public string CaseSubject { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("assigned_to")]
        public string AssignedTo { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("created_date")]
        public string CreatedDate { get; set; }

        [JsonProperty("updated_date")]
        public string UpdatedDate { get; set; }
    }
}
