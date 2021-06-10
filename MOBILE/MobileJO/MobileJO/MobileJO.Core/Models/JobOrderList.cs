using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.Models
{
    public class JobOrderList
    {
        public int id { get; set; }
        public string job_order_number { get; set; }
        public string application_type { get; set; }
        public string activity_details { get; set; }
        public string status { get; set; }
        public string color { get; set; }
    }
}
