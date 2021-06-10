using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.Questionnaire
{
    public class ChoiceViewModel
    {
        [JsonProperty("choice_id")]
        public int ChoiceID { get; set; }
        [JsonProperty("question_id")]
        public int QuestionID { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
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

    }
}
