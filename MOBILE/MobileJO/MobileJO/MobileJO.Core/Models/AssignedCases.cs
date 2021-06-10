using Newtonsoft.Json;
using System;

namespace MobileJO.Core.Models
{
    public class AssignedCases
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("case_number")]
        public string CaseNumber { get; set; }

        [JsonProperty("case_subject")]
        public string CaseSubject { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("application_type_id")]
        public int ApplicationTypeID { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }

        [JsonProperty("account_id")]
        public int AccountID { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("assigned_user_id")]
        public int AssignedUserID { get; set; }

        [JsonProperty("assigned_to")]
        public string AssignedTo { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("modified_by")]
        public string UpdatedBy { get; set; }
    }
}
