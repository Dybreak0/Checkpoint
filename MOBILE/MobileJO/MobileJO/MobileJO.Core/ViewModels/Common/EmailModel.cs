using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.ViewModels.Common
{
    public class EmailModel
    {
        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }
    }
}
