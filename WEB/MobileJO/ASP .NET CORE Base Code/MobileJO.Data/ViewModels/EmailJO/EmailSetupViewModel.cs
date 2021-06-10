using System;
using System.Collections.Generic;
using MobileJO.Data.ViewModels.EmailJO;
using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels
{
    public class EmailSetupViewModel
    {
        [JsonProperty(PropertyName = "email")]
        public List<EmailAddressViewModel> emailViewModel { get; set; }
    }
}
