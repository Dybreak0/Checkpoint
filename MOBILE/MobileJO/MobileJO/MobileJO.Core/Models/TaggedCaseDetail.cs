using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.Models
{
    public class TaggedCaseDetail
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("case_number")]
        public int CaseNumber { get; set; }

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

        [JsonProperty("created_date")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [JsonProperty("modified_by")]
        public string ModifiedBy { get; set; }
    }
}
