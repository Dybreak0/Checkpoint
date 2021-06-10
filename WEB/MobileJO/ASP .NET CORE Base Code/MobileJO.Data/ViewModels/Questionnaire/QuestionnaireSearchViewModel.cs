using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.Questionnaire
{
    public class QuestionnaireSearchViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("user_id")]
        public int UserID { get; set; }

        [JsonProperty("company_id")]
        public string CompanyID { get; set; }

        [JsonProperty("branch_id")]
        public string BranchID { get; set; }

        [JsonProperty("user_type_id")]
        public int UserTypeID { get; set; }

        [JsonProperty("is_mobile")]
        public bool IsMobile { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }
    }
}
