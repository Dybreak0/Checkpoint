﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.Models
{
    public class JobOrderBillingTypeModel
    {

        public int ID { get; set; }
        public string JobOrderID { get; set; }
        public int BillingTypeID { get; set; }
        public string BillingTypeName { get; set; }
    }
}
