    using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.RevertJO
{
    public class RevertJORequestViewModel
    {
        [JsonProperty("job_order_id")]
        public int JobOrderId { get; set; }

        [JsonProperty("job_order_revert_id")]
        public int JobOrderRevertID { get; set; }

        [JsonProperty("is_approved")]
        public bool IsApproved { get; set; }

        [JsonProperty("approved_by")]
        public int ApprovedBy { get; set; }

    }
}
