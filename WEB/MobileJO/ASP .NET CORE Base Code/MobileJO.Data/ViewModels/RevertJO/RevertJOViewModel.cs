using Newtonsoft.Json;
using System;

namespace MobileJO.Data.ViewModels.RevertJO
{
    public class RevertJOViewModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("job_order_id")]
        public int JobOrderID { get; set; }

        [JsonProperty("job_order_number")]
        public string JobOrderNumber { get; set; }

        [JsonProperty("activity_details")]
        public string ActivityDetails { get; set; }

        [JsonProperty("reported_by_name")]
        public string ReportedByName { get; set; }

        [JsonProperty("application_type_name")]
        public string ApplicationTypeName { get; set; }

        [JsonProperty("job_order_datetime_start")]
        public DateTime DateTimeStart { get; set; }

        [JsonProperty("job_order_datetime_end")]
        public DateTime DateTimeEnd { get; set; }
    }
}
