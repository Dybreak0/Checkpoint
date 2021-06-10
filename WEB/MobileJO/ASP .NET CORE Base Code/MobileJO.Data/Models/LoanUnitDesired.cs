using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    [Table("LoanUnitDesired")]
    public class LoanUnitDesired
    {
        [Key, Column("UnitDesiredID")]
        public int UnitDesiredID { get; set; }

        [Column("LoanID")]
        public int LoanID { get; set; }

        [Column("DesiredBrandModel", TypeName = "varchar(255)")]
        public string DesiredBrandModel { get; set; }

        [Column("DesiredSerialNo", TypeName = "varchar(255)")]
        public string DesiredSerialNo { get; set; }

        [Column("DesiredCode", TypeName = "varchar(255)")]
        public string DesiredCode { get; set; }

        [Column("DesiredAmount", TypeName = "varchar(255)")]
        public string DesiredAmount { get; set; }

        [Column("DesiredAccounting", TypeName = "varchar(255)")]
        public string DesiredAccounting { get; set; }
        
        // Foreign Keys
        [ForeignKey("LoanID")]
        [JsonIgnore]
        public virtual Loan Loan { get; set; }
    }
}
