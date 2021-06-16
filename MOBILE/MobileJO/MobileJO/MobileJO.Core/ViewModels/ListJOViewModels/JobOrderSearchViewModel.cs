using Newtonsoft.Json;

namespace MobileJO.Core.ViewModels
{
    public class JobOrderSearchViewModel
    {
        public JobOrderSearchViewModel()
        {
            JobOrderNumber = string.Empty;
            Status = string.Empty;
            ApplicationType = string.Empty;
            Page = 1;
            PageSize = 10;
        }


        [JsonProperty("job_order_number")]
        public string JobOrderNumber { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("application_type")]
        public string ApplicationType { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("sortBy")]
        public string SortBy { get; set; }

        [JsonProperty("sortOrder")]
        public string SortOrder { get; set; }
    }
}
