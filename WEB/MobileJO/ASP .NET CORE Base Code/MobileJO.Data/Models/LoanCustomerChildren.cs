using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    [Table("LoanCustomerChildren")]
    public class LoanCustomerChildren
    {
        [Key, Column("ChildID")]
        public int ChildID { get; set; }

        [Column("LoanID")]
        public int LoanID { get; set; }

        [Column("ChildName", TypeName = "varchar(255)")]
        public string ChildName { get; set; }

        [Column("ChildAge")]
        public string ChildAge { get; set; }

        [Column("ChildHomeAddress", TypeName = "varchar(255)")]
        public string ChildHomeAddress { get; set; }

        [Column("ChildTelNo", TypeName = "varchar(20)")]
        public string ChildTelNo { get; set; }

        [Column("ChildEmploySchool", TypeName = "varchar(255)")]
        public string ChildEmploySchool { get; set; }

        [Column("ChildEmploySchoolAddress", TypeName = "varchar(255)")]
        public string ChildEmploySchoolAddress { get; set; }

        [Column("ChildPosGrade", TypeName = "varchar(255)")]
        public string ChildPosGrade { get; set; }

        [Column("ChildHowLong", TypeName = "varchar(255)")]
        public string ChildHowLong { get; set; }

        // Foreign Keys
        [ForeignKey("LoanID")]
        [JsonIgnore]
        public virtual Loan Loan { get; set; }
    }
}
