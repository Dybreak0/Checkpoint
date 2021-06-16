using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.Models
{
    public class Branch
    {

        public int BranchID { get; set; }
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
    }
}
