using Newtonsoft.Json;

namespace MobileJO.Data.Models.TFSIntegration
{
    public class Workitem
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }
    }
}
