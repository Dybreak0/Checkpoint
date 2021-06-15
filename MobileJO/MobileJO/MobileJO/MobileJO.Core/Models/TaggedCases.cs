using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.Models
{
     public class TaggedCases
    {
        public int ID { get; set; }

        public int CaseNumber { get; set; }

        public string Status { get; set; }

        public string ApplicationType { get; set; }

        public string CaseSubject { get; set; }

        public string Priority { get; set; }

        public string AccountName { get; set; }

        public string Description { get; set; }

        public string AssignedTo { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public int JobOrderID { get; set; }

        public int CaseID { get; set; }
    }
}
