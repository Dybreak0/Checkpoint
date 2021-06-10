using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    [Table("LoanAttachment")]
    public class LoanAttachment
    {
        [Key, Column("AttachmentID")]
        public int AttachmentID { get; set; }

        [Column("LoanID")]
        public int LoanID { get; set; }

        [Column("FileName", TypeName = "varchar(255)")]
        public string FileName { get; set; }

        // Foreign Keys
        [ForeignKey("LoanID")]
        [JsonIgnore]
        public virtual Loan Loan { get; set; }
    }
}
