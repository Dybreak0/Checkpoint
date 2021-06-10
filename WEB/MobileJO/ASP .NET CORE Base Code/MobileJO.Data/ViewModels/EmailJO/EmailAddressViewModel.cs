using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.EmailJO
{
    public class EmailAddressViewModel
    {
        [JsonProperty(PropertyName = "type_id")]
        public int TypeID { get; set; }

        [JsonProperty(PropertyName = "email_address")]
        public string EmailAddress { get; set; }
    }
}
