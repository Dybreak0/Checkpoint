using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileJO.Data.Models
{
    public class RevertJobOrder
    {
        //public int ID { get; set; }
        //public string JobOrderID { get; set; }
        //public DateTime? RequestDate { get; set; }
        //public DateTime? RevertDate { get; set; }
        //public string Status { get; set; }
        //public bool? IsUsed { get; set; }

        [Column("ID")]
        public int ID { get; set; }

        [Column("JobOrderID")]
        public int JobOrderID { get; set; }

        [Column("RequestDate")]
        public DateTime RequestDate { get; set; }

        [Column("RevertDate")]
        public DateTime? RevertDate { get; set; }

        [Column("IsApproved")]
        public bool? IsApproved { get; set; }

        [Column("ApprovedBy")]
        public int? ApprovedBy { get; set; }

        [Column("IsUsed")]
        public bool IsUsed { get; set; }

        // Foreign Keys
        [ForeignKey("JobOrderID")]
        [JsonIgnore]
        public virtual JobOrder JobOrder { get; set; }

        [ForeignKey("ApprovedBy")]
        [JsonIgnore]
        public virtual User UserApprovedBy { get; set; }
    }
}
