using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class AssignedCase
    {

        [Column("ID")]
        public int ID { get; set; }

        [Column("CaseNumber", TypeName = "varchar(255)")]
        public string CaseNumber { get; set; }

        [Column("CaseSubject", TypeName = "varchar(255)")]
        public string CaseSubject { get; set; }

        [Column("Status", TypeName = "varchar(255)")]
        public string Status { get; set; }

        [Column("ApplicationTypeID")]
        public int ApplicationTypeID { get; set; }

        [Column("Priority", TypeName = "varchar(10)")]
        public string Priority { get; set; }

        [Column("AccountID")]
        public int AccountID { get; set; }

        [Column("Description", TypeName = "varchar(1000)")]
        public string Description { get; set; }

        [Column("AssignedUserID")]
        public int AssignedUserID { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("CreatedBy")]
        public string CreatedBy { get; set; }

        [Column("ModifiedDate")]
        public DateTime UpdatedDate { get; set; }

        [Column("ModifiedBy")]
        public string UpdatedBy { get; set; }

        // Foreign Keys
        [ForeignKey("ApplicationTypeID")]
        [JsonIgnore]
        public virtual ApplicationType ApplicationType { get; set; }

        [ForeignKey("AccountID")]
        [JsonIgnore]
        public virtual Account Account { get; set; }

        [ForeignKey("AssignedUserID")]
        [JsonIgnore]
        public virtual User User { get; set; }

        public virtual string ApplicationTypeName
        {
            get
            {
                return (ApplicationType != null) ? ApplicationType.ApplicationName : string.Empty;
            }
        }

        public virtual string AccountName
        {
            get
            {
                return (Account != null) ? Account.Name : string.Empty;
            }
        }

        public virtual string ApplicationName
        {
            get
            {
                return (ApplicationType != null) ? ApplicationType.ApplicationName : string.Empty;
            }
        }

        public virtual string FullName
        {
            get
            {
                return (User != null) ? string.Format(Constants.Common.NameFormat, User.FirstName, User.LastName) : string.Empty;
            }
        }
    }
}
