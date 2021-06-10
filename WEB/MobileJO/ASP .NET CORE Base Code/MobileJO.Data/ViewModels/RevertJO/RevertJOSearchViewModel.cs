using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.RevertJO
{
    public class RevertJOSearchViewModel
    {
        [JsonProperty("job_order_number")]
        public string JobOrderNumber { get; set; }

        [JsonProperty("reported_by")]
        public string ReportedBy { get; set; }        

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }
    }
}
