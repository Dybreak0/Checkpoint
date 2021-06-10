using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.EmailJO
{
    public class SelectJOViewModel
    {

        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("job_order_no")]
        public string JobOrderNo { get; set; }

        [JsonProperty("job_order_subject")]
        public string JobOrderSubject { get; set; }
    }
}
