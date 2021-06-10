using Newtonsoft.Json;

namespace MobileJO.Core.ViewModels.AssignedCases
{
    public class AssignedCasesSearchViewModel
    {
        public AssignedCasesSearchViewModel()
        {
            case_number = string.Empty;
            status = string.Empty;
            application_type = string.Empty;
            assigned_to = 0;
            page = 1;
            page_size = 10;
        }


        [JsonProperty("case_number")]
        public string case_number { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("application_type")]
        public string application_type { get; set; }

        [JsonProperty("assigned_to")]
        public int assigned_to { get; set; }

        [JsonProperty("page")]
        public int page { get; set; }

        [JsonProperty("page_size")]
        public int page_size { get; set; }
    }
}
