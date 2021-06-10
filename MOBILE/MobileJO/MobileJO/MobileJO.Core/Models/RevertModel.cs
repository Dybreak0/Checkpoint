using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.Models
{
    public class RevertModel
    {
        [JsonProperty("is_used")]
        public bool IsUsed { get; set; }
    }
}
