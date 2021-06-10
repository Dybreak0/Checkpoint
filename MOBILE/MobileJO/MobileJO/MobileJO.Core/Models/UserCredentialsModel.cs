using Newtonsoft.Json;

namespace MobileJO.Core.Models
{
    public class UserCredentialsModel
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("mobile_login")]
        public string MobileLogin { get; set; }
    }
}
