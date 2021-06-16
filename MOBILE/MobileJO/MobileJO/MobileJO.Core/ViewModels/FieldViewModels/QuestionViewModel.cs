using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MobileJO.Core.ViewModels
{
    public class QuestionsViewModel
    {
        [JsonProperty("question_id")]
        public int QuestionID { get; set; }
        [JsonProperty("template_id")]
        public int TemplateID { get; set; }
        [JsonProperty("question")]
        public string Question { get; set; }
        [JsonProperty("question_type_id")]
        public int QuestionTypeID { get; set; }
        [JsonProperty("question_type")]
        public string QuestionType { get; set; }
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
        public List<ChoiceViewModel> Choices { get; set; }
    }
}
