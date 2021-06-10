using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MobileJO.Data.ViewModels.LoanApplication
{
    public class LoanPersonalPropertyViewModel
    {
        private string _property;

        public int PersonalPropertyID { get; set; }

        public int LoanID { get; set; }

        public string Property
        {
            get => _property;
            set => _property = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
    }
}
