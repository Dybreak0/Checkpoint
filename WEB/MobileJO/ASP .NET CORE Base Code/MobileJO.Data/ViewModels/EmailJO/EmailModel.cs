using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.EmailJO
{
    public class EmailModel
    {
        [JsonProperty(PropertyName = "email_address")]
        public string EmailAddress { get; set; }
    }
}
