using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.Models
{
    public class Pagination
    {
        [JsonProperty("pages")]
        public int Pages { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }
    }
}
