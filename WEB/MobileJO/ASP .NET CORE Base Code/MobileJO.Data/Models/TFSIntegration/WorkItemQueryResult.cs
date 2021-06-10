using Newtonsoft.Json;
using System;

namespace MobileJO.Data.Models.TFSIntegration
{
    public class WorkItemQueryResult
    {
        [JsonProperty("queryType")]
        public string QueryType { get; set; }

        [JsonProperty("queryResultType")]
        public string QueryResultType { get; set; }

        [JsonProperty("asOf")]
        public DateTime AsOf { get; set; }

        [JsonProperty("columns")]
        public Column[] Columns { get; set; }

        [JsonProperty("workItems")]
        public Workitem[] WorkItems { get; set; }
    }
}
