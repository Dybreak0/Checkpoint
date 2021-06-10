using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    [Table("LoanCreditHistory")]
    public class LoanCreditHistory
    {
        [Key, Column("CreditHistoryID")]
        public int CreditHistoryID { get; set; }

        [Column("LoanID")]
        public int LoanID { get; set; }

        [Column("HistoryCompanyName", TypeName = "varchar(255)")]
        public string HistoryCompanyName { get; set; }

        [Column("HistoryTypeOfUnit", TypeName = "varchar(255)")]
        public string HistoryTypeOfUnit { get; set; }

        [Column("HistoryDatePurchase")]
        public DateTime? HistoryDatePurchase { get; set; }

        [Column("HistoryTerms", TypeName = "varchar(255)")]
        public string HistoryTerms { get; set; }

        [Column("HistoryRemainingBalance", TypeName = "varchar(255)")]
        public string HistoryRemainingBalance { get; set; }

        // Foreign Keys
        [ForeignKey("LoanID")]
        [JsonIgnore]
        public virtual Loan Loan { get; set; }
    }
}
