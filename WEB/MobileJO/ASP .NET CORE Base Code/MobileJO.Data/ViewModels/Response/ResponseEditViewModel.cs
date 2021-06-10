using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MobileJO.Data.ViewModels
{
    public class ResponseEditViewModel
    {
        private string _remarks;

        [JsonProperty("responseID")]
        public int ResponseID { get; set; }

        [JsonProperty("templateID")]
        public int TemplateID { get; set; }

        [JsonProperty("userID")]
        public int UserID { get; set; }

        [JsonProperty("branchID")]
        public int BranchID { get; set; }

        [JsonProperty("companyID")]
        public int CompanyID { get; set; }

        [MaxLength(255)]
        [JsonProperty("remarks")]
        public string Remarks
        {
            get => _remarks;
            set => _remarks = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("isApproved")]
        public bool IsApproved { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [JsonProperty("updatedBy")]
        public int UpdatedBy { get; set; }
    }
}
