using MobileJO.Data.ViewModels.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.EmailJO
{
    public class EmailJOViewModel
    {
        [JsonProperty("job_order_ids")]
        public List<string> JobOrderNos { get; set; }

        [JsonProperty("use_default_address")]
        public bool UseDefaultAddress { get; set; }

        [JsonProperty("recipient")]
        public List<EmailAddressViewModel> Recipient { get; set; }

        [JsonProperty("support_type")]
        public string SupportType { get; set; }

        [JsonProperty("conforme_slip")]
        public string ConformeSlip { get; set; }
    }
}
