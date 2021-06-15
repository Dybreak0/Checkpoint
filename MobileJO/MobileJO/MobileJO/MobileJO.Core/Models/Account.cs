using System;

namespace MobileJO.Core.Models
{
    public class Account
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string ContactNo { get; set; }

        public string ContactPerson { get; set; }

        public string Address { get; set; }

        public string Memo { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int UpdatedBy { get; set; }
    }
}
