using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class Account
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("Name", TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Column("EmailAddress", TypeName = "varchar(64)")]
        public string EmailAddress { get; set; }

        [Column("ContactNo", TypeName = "varchar(20)")]
        public string ContactNo { get; set; }

        [Column("ContactPerson", TypeName = "varchar(255)")]
        public string ContactPerson { get; set; }

        [Column("Address", TypeName = "varchar(255)")]
        public string Address { get; set; }

        [Column("Memo", TypeName = "varchar(255)")]
        public string Memo { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Column("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        [Column("UpdatedBy")]
        public int UpdatedBy { get; set; }

        // Foreign Keys
        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User UserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        [JsonIgnore]
        public virtual User UserUpdatedBy { get; set; }

        public virtual string CreatedByName
        {
            get
            {
                return (UserCreatedBy != null) ? UserCreatedBy.UserName : string.Empty;
            }
        }

        public virtual string UpdatedByName
        {
            get
            {
                return (UserUpdatedBy != null) ? UserUpdatedBy.UserName : string.Empty;
            }
        }

    }
}
