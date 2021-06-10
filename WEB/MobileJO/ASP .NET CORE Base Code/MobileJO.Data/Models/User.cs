using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    [Table("User")]
    public class User
    {
        [Column("UserID")]
        public string UserID { get; set; }

        [Key, Column("ID")]
        public int ID { get; set; }

        [Column("UserName", TypeName = "varchar(255)")]
        public string UserName { get; set; }

        [Column("Password", TypeName = "varchar(255)")]
        public string Password { get; set; }

        [Column("FirstName", TypeName = "varchar(255)")]
        public string FirstName { get; set; }

        [Column("LastName", TypeName = "varchar(255)")]
        public string LastName { get; set; }

        [Column("RoleID")]
        public int RoleID { get; set; }

        [Column("EmailAddress", TypeName = "varchar(64)")]
        public string EmailAddress { get; set; }

        [Column("Memo", TypeName = "varchar(255)")]
        public string Memo { get; set; }

        [Column("AllowedToLogin")]
        public bool AllowedToLogin { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Column("Address", TypeName = "varchar(255)")]
        public string Address { get; set; }

        [Column("TelephoneNo", TypeName = "varchar(20)")]
        public string TelephoneNo { get; set; }

        [Column("MobileNo", TypeName = "varchar(20)")]
        public string MobileNo { get; set; }

        [Column("CompanyID")]
        public int CompanyID { get; set; }

        [Column("BranchID")]
        public int BranchID { get; set; }

        [Column("UserTypeID")]
        public int UserTypeID { get; set; }

        [Column("CreatedBy", TypeName = "varchar(255)")]
        public string CreatedBy { get; set; }

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Column("UpdatedBy", TypeName = "varchar(255)")]
        public string UpdatedBy { get; set; }

        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }


        // Foreign Keys
        [ForeignKey("RoleID")]
        [JsonIgnore]
        public virtual Role Role { get; set; }

        [ForeignKey("CompanyID")]
        [JsonIgnore]
        public virtual Company Company { get; set; }


        [ForeignKey("BranchID")]
        [JsonIgnore]
        public virtual Branch Branch { get; set; }

        [ForeignKey("UserTypeID")]
        [JsonIgnore]
        public virtual UserType UserType { get; set; }

    }
}