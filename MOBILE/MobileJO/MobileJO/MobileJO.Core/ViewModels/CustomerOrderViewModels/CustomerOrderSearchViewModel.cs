using Newtonsoft.Json;

namespace MobileJO.Core.ViewModels.CustomerOrderViewModels
{
    public class CustomerOrderSearchViewModel
    {
        public CustomerOrderSearchViewModel()
        {
            CustomerOrderNumber = string.Empty;
            //CustomerOrderStatus = "All";
            Page = 1;
            PageSize = 10;
        }
        [JsonProperty("order_number")]
        public string CustomerOrderNumber { get; set; }

        [JsonProperty("order_status")]
        public string CustomerOrderStatus { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }
    }
}
