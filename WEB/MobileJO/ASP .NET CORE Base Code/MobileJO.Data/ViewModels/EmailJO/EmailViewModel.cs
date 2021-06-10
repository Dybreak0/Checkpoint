using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.EmailJO
{
    public class EmailViewModel
    {
        [JsonProperty(PropertyName = "type_id")]
        public int TypeID { get; set; }

        [JsonProperty(PropertyName = "email_address")]
        public string EmailAddress { get; set; }
    }
}
