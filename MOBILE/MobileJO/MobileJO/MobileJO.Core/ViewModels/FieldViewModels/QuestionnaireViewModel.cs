﻿using MobileJO.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MobileJO.Core.ViewModels
{
    public class QuestionnaireViewModel
    {
        [JsonProperty("id")]
        public int TemplateID { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("template_branches")]
        public List<Template_Branch> Template_Branches { get; set; }
        [JsonProperty("company_branches")]
        public List<Company_Branch> Company_Branches { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }
        [JsonProperty("max_limit")]
        public int MaxLimit { get; set; }
        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("updated_by")]
        public int UpdatedBy { get; set; }

        public List<QuestionsViewModel> Questions { get; set; }

    }
}
