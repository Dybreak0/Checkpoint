using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.Reports
{
    public class AssignedCasesReportSearchViewModel
    {
        [JsonProperty("case_number")]
        public string CaseNumber { get; set; }

        [JsonProperty("application_type")]
        public int ApplicationType { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }
    }
}
