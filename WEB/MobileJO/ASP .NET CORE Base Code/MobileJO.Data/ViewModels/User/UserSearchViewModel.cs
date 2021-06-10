using Newtonsoft.Json;

namespace MobileJO.Data.ViewModels
{
    public class UserSearchViewModel
    {
        private string _userName;
        private string _roleID;
        private string _companyID;

        [JsonProperty("user_name")]
        public string UserName 
        {
            get => _userName;
            set => _userName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

      
        [JsonProperty("company_id")]
        public string CompanyID
        {
            get => _companyID;
            set => _companyID = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("role_id")]
        public string RoleID
        {
            get => _roleID;
            set => _roleID = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }
    }
}
