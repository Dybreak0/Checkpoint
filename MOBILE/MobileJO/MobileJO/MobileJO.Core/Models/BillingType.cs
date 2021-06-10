using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.Models
{
    public class BillingType
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("job_order_id")]
        public string JobOrderID { get; set; }

        [JsonProperty("billing_type_id")]
        public int BillingTypeID { get; set; }
    }
}
