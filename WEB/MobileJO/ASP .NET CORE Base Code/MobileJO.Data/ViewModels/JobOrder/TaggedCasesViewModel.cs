using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.JobOrder
{
    public class TaggedCasesViewModel
    {[JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        [JsonProperty("sort_by")]
        public string SortBy { get; set; }

        [JsonProperty("sort_order")]
        public string SortOrder { get; set; }

    }

}
