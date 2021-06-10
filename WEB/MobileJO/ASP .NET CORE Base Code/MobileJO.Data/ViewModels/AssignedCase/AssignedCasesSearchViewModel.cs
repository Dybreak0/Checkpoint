using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.AssignedCase
{
    public class AssignedCasesSearchViewModel
    {
        [JsonProperty("case_number")]
        public string CaseNumber { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("application_type")]
        public string ApplicationType { get; set; }

        [JsonProperty("assigned_to")]
        public int AssignedTo { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }
    }
}
