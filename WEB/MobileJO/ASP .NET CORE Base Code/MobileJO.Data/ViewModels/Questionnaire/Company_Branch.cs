using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.Questionnaire
{
    public class Company_Branch
    {
        [JsonProperty("CompanyID")]
        public int CompanyID { get; set; }

        [JsonProperty("CompanyName")]
        public string CompanyName { get; set; }

        [JsonProperty("BranchID")]
        public int BranchID { get; set; }

        [JsonProperty("BranchName")]
        public string BranchName { get; set; }
    }
}
