using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.ViewModels
{
    public class LastPageViewModel
    {
        public bool IsBilled { get; set; }
        public bool IsCollaterals { get; set; }
        public bool IsFixed { get; set; }
        public bool IsSatisfied { get; set; }
        public int ClientRating { get; set; }
    }
}
