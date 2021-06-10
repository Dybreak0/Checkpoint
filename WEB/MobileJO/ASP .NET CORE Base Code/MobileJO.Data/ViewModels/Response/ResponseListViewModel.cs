using Newtonsoft.Json;
using System;

namespace MobileJO.Data.ViewModels.Response
{
    public class ResponseListViewModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("date_submitted")]
        public DateTime DateSubmitted { get; set; }

        [JsonProperty("submitted_by")]
        public string SubmittedBy { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }


    }
}
