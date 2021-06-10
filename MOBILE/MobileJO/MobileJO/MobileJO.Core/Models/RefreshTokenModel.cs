using Newtonsoft.Json;

namespace MobileJO.Core.Models
{
    public class RefreshTokenModel
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
