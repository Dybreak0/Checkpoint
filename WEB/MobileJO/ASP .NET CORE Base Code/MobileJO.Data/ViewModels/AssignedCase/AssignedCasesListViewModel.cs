using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.AssignedCase
{
    public class AssignedCasesListViewModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("case_number")]
        public string CaseNumber { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("application_type")]
        public string ApplicationType { get; set; }

        [JsonProperty("case_subject")]
        public string CaseSubject { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
