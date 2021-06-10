using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.JobOrder
{
    public class JobOrderCaseViewModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("job_order_id")]
        public int JobOrderID { get; set; }

        [JsonProperty("case_id")]
        public int CaseID { get; set; }
    }
}
