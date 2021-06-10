using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MobileJO.Data.ViewModels.LoanApplication
{
    public class LoanApprovalViewModel
    {

        [JsonProperty("list_loan_id")]
        public List<int> LoanIDs { get; set; }

        [JsonProperty("loan_status")]
        public string LoanStatus { get; set; }

        [JsonProperty("approved_by")]
        public int ApprovedBy { get; set; }
    }
}
