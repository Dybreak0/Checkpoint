using Newtonsoft.Json;
using System;

namespace MobileJO.Data.Models.TFSIntegration
{
    public class WorkItemDetails
    {
        [JsonProperty("count")]
        public int count { get; set; }

        [JsonProperty("value")]
        public Value[] value { get; set; }
    }
}
