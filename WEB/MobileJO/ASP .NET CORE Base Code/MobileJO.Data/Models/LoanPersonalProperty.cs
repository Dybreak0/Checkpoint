using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    [Table("LoanPersonalProperty")]
    public class LoanPersonalProperty
    {
        [Key, Column("PersonalPropertyID")]
        public int PersonalPropertyID { get; set; }

        [Column("LoanID")]
        public int LoanID { get; set; }

        [Column("Property", TypeName = "varchar(255)")]
        public string Property { get; set; }

        // Foreign Keys
        [ForeignKey("LoanID")]
        [JsonIgnore]
        public virtual Loan Loan { get; set; }
    }
}
