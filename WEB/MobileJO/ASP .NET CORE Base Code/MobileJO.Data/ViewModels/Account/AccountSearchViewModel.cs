using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels.Account
{
    public class AccountSearchViewModel
    {
        private string _name;

        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }
    }
}
