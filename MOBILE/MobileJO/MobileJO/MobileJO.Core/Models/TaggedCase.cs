using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.Models
{
    public class TaggedCase
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

        [JsonProperty("created_date")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("updated_date")]
        public DateTime? UpdatedDate { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        public int JobOrderID { get; set; }

        public int CaseID { get; set; }
    }
}
