using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.Common
{
    public class DropdownViewModel
    {
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
    }
}
