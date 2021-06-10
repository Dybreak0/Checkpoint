using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MobileJO.Data.ViewModels
{
    public class UserEditViewModel
    {
        private string _userName;
        private string _firstName;
        private string _lastName;
        private string _password;
        private string _emailAddress;
        private string _address;
        private string _memo;
        private string _telephoneNo;
        private string _mobileNo;

        [JsonProperty("id")]
        public int ID { get; set; }

        [MaxLength(20)]
        [JsonProperty("user_name")]
        [Required(ErrorMessage = "User name is Required.")]
        public string UserName
        {
            get => _userName;
            set => _userName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("password")]
        public string Password
        {
            get => _password;
            set => _password = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("first_name")]
        [Required(ErrorMessage = "First name is Required.")]
        public string FirstName
        {
            get => _firstName;
            set => _firstName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("last_name")]
        [Required(ErrorMessage = "Last name is Required.")]
        public string LastName
        {
            get => _lastName;
            set => _lastName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("role_id")]
        [Required(ErrorMessage = "Role is Required.")]
        public int RoleID { get; set; }

        [MaxLength(64)]
        [JsonProperty("email_address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string EmailAddress
        {
            get => _emailAddress;
            set => _emailAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("allowed_to_login")]
        public bool AllowedToLogin { get; set; }

        [MaxLength(255)]
        [JsonProperty("memo")]
        public string Memo
        {
            get => _memo;
            set => _memo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(20)]
        [JsonProperty("telephone_no")]
        public string TelephoneNo
        {
            get => _telephoneNo;
            set => _telephoneNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(20)]
        [JsonProperty("mobile_no")]
        public string MobileNo
        {
            get => _mobileNo;
            set => _mobileNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("address")]
        public string Address
        {
            get => _address;
            set => _address = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("user_type_id")]
        public int UserTypeID { get; set; }

        [JsonProperty("company_id")]
        public int CompanyID { get; set; }

        [JsonProperty("branch_id")]
        public int BranchID { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("created_date")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("updated_date")]
        public DateTime? UpdatedDate { get; set; }
    }
}
