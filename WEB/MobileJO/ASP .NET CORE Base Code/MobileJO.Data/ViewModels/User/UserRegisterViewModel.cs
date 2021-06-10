using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MobileJO.Data.ViewModels
{
    public class UserRegisterViewModel
    {
        [MaxLength(20)]
        [Required]
        [JsonProperty(PropertyName = "uname")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [JsonProperty(PropertyName = "confirm_pass")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [MaxLength(255)]
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [MaxLength(255)]
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [MaxLength(64)]
        [Display(Name = "Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        [JsonProperty(PropertyName = "email")]
        public string EmailAddress { get; set; }
    }
}
