using Newtonsoft.Json;
using System;

namespace MobileJO.Data.ViewModels.JobOrder
{
    public class JobOrderBillingTypeViewModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("job_order_id")]
        public int JobOrderID { get; set; }

        [JsonProperty("billing_type_id")]
        public int BillingTypeID { get; set; }
    }
}
