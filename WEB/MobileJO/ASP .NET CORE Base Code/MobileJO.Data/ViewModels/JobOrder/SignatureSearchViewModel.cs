using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.JobOrder
{
    public class SignatureSearchViewModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("client_signature")]
        public string ClientSignature { get; set; }
    }
}
