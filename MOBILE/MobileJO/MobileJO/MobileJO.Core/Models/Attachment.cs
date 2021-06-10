using Newtonsoft.Json;

namespace MobileJO.Core.Models
{
    public class Attachment
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("job_order_id")]
        public int JobOrderID { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

    }
}
