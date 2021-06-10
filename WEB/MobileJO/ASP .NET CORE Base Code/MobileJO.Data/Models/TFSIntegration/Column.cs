using Newtonsoft.Json;

namespace MobileJO.Data.Models.TFSIntegration
{
    public class Column
    {
        [JsonProperty("referenceName")]
        public string ReferenceName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }
    }
}
