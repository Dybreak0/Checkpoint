using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MobileJO.Data.ViewModels
{
    public class AccountViewModel
    {
        private string _name;
        private string _memo;
        private string _emailAddress;
        private string _address;
        private string _contactPerson;
        private string _contactNumber;


        [JsonProperty("id")]
        public int ID { get; set; }

        [MaxLength(255)]
        [JsonProperty("name")]
        [Required(ErrorMessage = "Name is Required.")]
        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("memo")]
        public string Memo
        {
            get => _memo;
            set => _memo = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        }

        [MaxLength(64)]
        [JsonProperty("email_address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        [Required(ErrorMessage = "Email is Required.")]
        public string EmailAddress
        {
            get => _emailAddress;
            set => _emailAddress = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("address")]
        [Required(ErrorMessage = "Address is Required.")]
        public string Address
        {
            get => _address;
            set => _address = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("contact_person")]
        [Required(ErrorMessage = "Contact Person is Required.")]
        public string ContactPerson
        {
            get => _contactPerson;
            set => _contactPerson = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        }

        [MaxLength(20)]
        [JsonProperty("contact_number")]
        [Required(ErrorMessage = "Contact Number is Required.")]
        public string ContactNo
        {
            get => _contactNumber;
            set => _contactNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; } = true;

        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty("created_date")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty("created_by_name")]
        public string CreatedByName { get; set; }

        [JsonProperty("updated_by")]
        public int UpdatedBy { get; set; }

        [JsonProperty("updated_date")]
        public DateTime? UpdatedDate { get; set; }

        [JsonProperty("updated_by_name")]
        public string UpdatedByName { get; set; }
    }
}
