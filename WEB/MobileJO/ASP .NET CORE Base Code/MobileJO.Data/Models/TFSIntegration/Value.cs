using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MobileJO.Data.Models.TFSIntegration
{
    public class Value
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("rev")]
        public int Rev { get; set; }

        [JsonProperty("fields")]
        public Fields Fields { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
