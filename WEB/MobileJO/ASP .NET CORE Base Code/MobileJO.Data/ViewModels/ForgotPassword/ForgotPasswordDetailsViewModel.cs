using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data.ViewModels.ForgotPassword
{
    public class ForgotPasswordDetailsViewModel
    {   
        public int ID { get; set; }
        public int userId { get; set; }
        public string token { get; set; }
        public string newPassword { get; set; }
    }
}
