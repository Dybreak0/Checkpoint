using System;
using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.SyncLog
{
    public class SyncLogViewModel
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("CaseNumber")]
        public string CaseNumber { get; set; }

        [JsonProperty("DateSync")]
        public DateTime DateSync { get; set; }

        [JsonProperty("ErrMsg")]
        public string ErrMsg { get; set; }
    }
}
