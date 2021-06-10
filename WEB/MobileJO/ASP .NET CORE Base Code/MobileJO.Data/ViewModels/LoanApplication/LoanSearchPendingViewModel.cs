using Newtonsoft.Json;
using System;

namespace MobileJO.Data.ViewModels.LoanApplication
{
    public class LoanSearchPendingViewModel
    {
        [JsonProperty("role")]
        public int RoleID { get; set; }

        [JsonProperty("branch_id")]
        public int BranchID { get; set; }

        [JsonProperty("application_no")]
        public string ApplicationNo { get; set; }

        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        [JsonProperty("created_by_name")]
        public string CreatedBy { get; set; }

        [JsonProperty("date_from")]
        public DateTime DateFrom { get; set; }

        [JsonProperty("date_to")]
        public DateTime DateTo { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }
    }
}
