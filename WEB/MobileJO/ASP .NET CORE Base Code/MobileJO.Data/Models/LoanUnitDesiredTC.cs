using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MobileJO.Data.Models
{
    public class LoanUnitDesiredTC
    {
        [Key, Column("UnitDesiredTCID")]
        public int UnitDesiredTCID { get; set; }

        [Column("LoanID")]
        public int LoanID { get; set; }

        [Column("DesiredTCBrandModel", TypeName = "varchar(255)")]
        public string DesiredTCBrandModel { get; set; }

        [Column("DesiredTCTerms", TypeName = "varchar(255)")]
        public string DesiredTCTerms { get; set; }

        [Column("DesiredTCDownPayment", TypeName = "varchar(255)")]
        public string DesiredTCDownPayment { get; set; }

        [Column("DesiredTCMonthlyInstallment", TypeName = "varchar(255)")]
        public string DesiredTCMonthlyInstallment { get; set; }

        [Column("DesiredTCTotalPrice", TypeName = "varchar(255)")]
        public string DesiredTCTotalPrice { get; set; }

        [Column("DesiredTCTotalRebate", TypeName = "varchar(255)")]
        public string DesiredTCTotalRebate { get; set; }

        [Column("DesiredTCRemarks", TypeName = "varchar(255)")]
        public string DesiredTCRemarks { get; set; }

        // Foreign Keys
        [ForeignKey("LoanID")]
        [JsonIgnore]
        public virtual Loan Loan { get; set; }
    }
}
