using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace MobileJO.Data.Models
{
    public class JobOrder
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("JobOrderNumber", TypeName = "varchar(9)")]
        public string JobOrderNumber { get; set; }

        [Column("JobOrderSubject", TypeName = "varchar(255)")]
        public string JobOrderSubject { get; set; }

        [Column("StatusID")]
        public int StatusID { get; set; }

        [Column("AccountID")]
        public int AccountID { get; set; }

        [Column("ApplicationTypeID")]
        public int ApplicationTypeID { get; set; }

        [Column("Branch", TypeName = "varchar(255)")]
        public string Branch { get; set; }

        [Column("DateTimeStart")]
        public DateTime DateTimeStart { get; set; }

        [Column("DateTimeEnd")]
        public DateTime DateTimeEnd { get; set; }

        [Column("ActivityDetails", TypeName = "varchar(1000)")]
        public string ActivityDetails { get; set; }

        [Column("RootCauseAnalysis", TypeName = "varchar(1000)")]
        public string RootCauseAnalysis { get; set; }

        [Column("NextStep", TypeName = "varchar(1000)")]
        public string NextStep { get; set; }

        [Column("PreventiveAction", TypeName = "varchar(1000)")]
        public string PreventiveAction { get; set; }

        [Column("Remarks", TypeName = "varchar(1000)")]
        public string Remarks { get; set; }

        [Column("Attendees", TypeName = "varchar(1000)")]
        public string Attendees { get; set; }

        [Column("IsBilled")]
        public bool IsBilled { get; set; }

        [Column("IsCollaterals")]
        public bool IsCollaterals { get; set; }

        [Column("IsFixed")]
        public bool IsFixed { get; set; }

        [Column("IsSatisfied")]
        public bool IsSatisfied { get; set; }

        [Column("ClientSignature", TypeName = "varchar(255)")]
        public string ClientSignature { get; set; }

        [Column("ClientRating")]
        public int ClientRating { get; set; }

        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Column("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        [Column("UpdatedBy")]
        public int UpdatedBy { get; set; }

        [Column("LastSyncDate")]
        public DateTime LastSyncDate { get; set; }

        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }

        // Foreign Keys
        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User UserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        [JsonIgnore]
        public virtual User UserUpdatedBy { get; set; }

        [ForeignKey("StatusID")]
        [JsonIgnore]
        public virtual JobOrderStatus Status { get; set; }

        [ForeignKey("AccountID")]
        [JsonIgnore]
        public virtual Account Account { get; set; }

        [ForeignKey("ApplicationTypeID")]
        [JsonIgnore]
        public virtual ApplicationType ApplicationType { get; set; }

        public virtual ICollection<JobOrderBillingType> JobOrderBillingType { get; set; }
        public virtual ICollection<TaggedCase> TaggedCase { get; set; }
        public virtual ICollection<Attachment> Attachment { get; set; }

        public virtual string ReportedByName
        {
            get
            {
                return (UserCreatedBy != null) ? string.Format(Constants.Common.NameFormat, UserCreatedBy.FirstName, UserCreatedBy.LastName) : string.Empty;
            }
        }

        public virtual string UpdatedByName
        {
            get
            {
                return (UserUpdatedBy != null) ? string.Format(Constants.Common.NameFormat, UserUpdatedBy.FirstName, UserUpdatedBy.LastName) : string.Empty;
            }
        }

        public virtual string ApplicationTypeName
        {
            get
            {
                return (ApplicationType != null) ? ApplicationType.ApplicationName : string.Empty;
            }
        }

        public virtual string StatusName
        {
            get
            {
                return (Status != null) ? Status.Status : string.Empty;
            }
        }

        public virtual string AccountName
        {
            get
            {
                return (Account != null) ? Account.Name : string.Empty;
            }
        }

        public virtual string CaseNumber
        {
            get
            {
                var taggedCases = string.Empty;
                if (TaggedCase != null)
                {
                    var cases = TaggedCase.Select(x => x.AssignedCases.CaseNumber).ToList();
                    taggedCases = string.Join(Constants.Common.Comma, cases);
                }

                return taggedCases;
            }
        }
        public virtual string CaseSubject
        {
            get
            {
                var taggedCases = string.Empty;
                if (TaggedCase != null)
                {
                    var cases = TaggedCase.Select(x => x.AssignedCases.CaseSubject).ToList();
                    taggedCases = string.Join(Constants.Common.Comma, cases);
                }

                return taggedCases;
            }
        }
    }
}
