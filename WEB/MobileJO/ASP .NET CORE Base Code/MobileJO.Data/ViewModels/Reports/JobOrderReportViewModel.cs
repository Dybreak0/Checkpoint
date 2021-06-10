using MobileJO.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MobileJO.Data.ViewModels.Reports
{
    public class JobOrderReportViewModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("job_order_number")]
        public string JobOrderNumber { get; set; }

        [JsonProperty("job_order_subject")]
        public string JobOrderSubject { get; set; }

        [JsonProperty("case_number")]
        public string CaseNumber { get; set; }

        [JsonProperty("activity_details")]
        public string ActivityDetails { get; set; }

        [JsonProperty("reported_by_name")]
        public string ReportedByName { get; set; }

        [JsonProperty("application_type_name")]
        public string ApplicationTypeName { get; set; }

        [JsonProperty("status_name")]
        public string StatusName { get; set; }

        [JsonProperty("job_order_datetime_start")]
        public DateTime DateTimeStart { get; set; }

        [JsonProperty("job_order_datetime_end")]
        public DateTime DateTimeEnd { get; set; }

        [JsonProperty("is_billed")]
        public bool IsBilled { get; set; }

        [JsonProperty("is_collaterals")]
        public bool IsCollaterals { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("branch_name")]
        public string Branch { get; set; }

        [JsonProperty("is_satisfied")]
        public bool IsSatisfied { get; set; }

        [JsonProperty("client_rating")]
        public int ClientRating { get; set; }

        [JsonProperty("root_cause_analysis")]
        public string RootCauseAnalysis { get; set; }

        [JsonProperty("next_step")]
        public string NextStep { get; set; }

        [JsonProperty("preventive_action")]
        public string PreventiveAction { get; set; }

        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        [JsonProperty("attendees")]
        public string Attendees { get; set; }

        [JsonProperty("client_signature")]
        public string ClientSignature { get; set; }

        [JsonProperty("job_order_revert_id")]
        public string JobOrderRevertId { get; set; }

        [JsonProperty("job_order_billing_type")]
        public List<JobOrderBillingType> JobOrderBillingType { get; set; }

        [JsonProperty("job_order_attachment")]
        public List<Attachment> Attachment { get; set; }
    }
}
