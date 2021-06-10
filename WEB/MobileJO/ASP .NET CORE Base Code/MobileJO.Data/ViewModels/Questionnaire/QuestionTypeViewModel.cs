using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.Questionnaire
{
    public class QuestionTypeViewModel
    {
        [JsonProperty("question_type_id")]
        public int QuestionTypeID { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
