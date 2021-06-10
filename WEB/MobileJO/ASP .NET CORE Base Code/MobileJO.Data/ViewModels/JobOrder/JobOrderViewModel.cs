using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobileJO.Data.ViewModels.JobOrder
{
    public class JobOrderViewModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("job_order_number")]
        public string JobOrderNumber { get; set; }

        [JsonProperty("job_order_subject")]
        public string JobOrderSubject { get; set; }

        [JsonProperty("status_id")]
        public int StatusID { get; set; }

        [JsonProperty("account_id")]
        public int AccountID { get; set; }

        [JsonProperty("application_id")]
        public int ApplicationTypeID { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("application_type")]
        public int ApplicationType { get; set; }

        [JsonProperty("application_name")]
        public string ApplicationName { get; set; }

        

        [JsonProperty("date_time_start")]
        public DateTime? DateTimeStart { get; set; }

        [JsonProperty("date_time_end")]
        public DateTime? DateTimeEnd { get; set; }

        [JsonProperty("activity_details")]
        public string ActivityDetails { get; set; }

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

        [JsonProperty("is_billed")]
        public bool? Is_Billed { get; set; }

        [JsonProperty("is_collaterals")]
        public bool? Is_Collaterals { get; set; }

        [JsonProperty("client_signature")]
        public string ClientSignature { get; set; }

        [JsonProperty("is_fixed")]
        public bool? Is_Fixed { get; set; }

        [JsonProperty("is_satisfied")]
        public bool? Is_Satisfied { get; set; }

        

        [JsonProperty("created_date")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty("updated_date")]
        public DateTime? UpdatedDate { get; set; }

        [JsonProperty("updated_by")]
        public int UpdatedBy { get; set; }

        [JsonProperty("is_deleted")]
        public bool? IsDeleted { get; set; }

        [JsonProperty("client_rating")]
        public int ClientRating { get; set; }

        

       

        [JsonProperty("last_sync_date")]
        public DateTime LastSyncDate { get; set; }

    }
}
