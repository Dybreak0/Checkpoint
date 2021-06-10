using Newtonsoft.Json;
using System;

namespace MobileJO.Data.ViewModels.JobOrder
{
    public class JobOrderRevertViewModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("job_order_id")]
        public int JobOrderID { get; set; }

        [JsonProperty("request_date")]
        public int RequestDate { get; set; }

        [JsonProperty("revert_date")]
        public int RevertDate { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("is_used")]
        public bool IsUsed { get; set; }
    }
}
