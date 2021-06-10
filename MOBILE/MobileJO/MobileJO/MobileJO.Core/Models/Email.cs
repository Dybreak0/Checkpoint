using Newtonsoft.Json;
namespace MobileJO.Core.Models
{
    public class Email
    {
        [JsonProperty(PropertyName = "type_id")]
        public int TypeID { get; set; }

        [JsonProperty(PropertyName = "email_address")]
        public string EmailAddress { get; set; }
    }
}
