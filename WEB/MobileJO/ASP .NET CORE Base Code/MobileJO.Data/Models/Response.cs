using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class Response
    {
        [Key, Column("ResponseID")]
        public int ResponseID { get; set; }

        [Column("TemplateID")]
        public int TemplateID { get; set; }

        [Column("UserID")]
        public int UserID { get; set; }

        [Column("BranchID")]
        public int BranchID { get; set; }

        [Column("CompanyID")]
        public int CompanyID { get; set; }

        [Column("DateSubmitted")]
        public DateTime DateSubmitted { get; set; }

        [Column("Remarks", TypeName = "varchar(255)")]
        public string Remarks { get; set; }

        [Column("Status")]
        public bool Status { get; set; }

        [Column("IsApproved")]
        public bool IsApproved { get; set; }

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


        [Column("Shet")]
        public DateTime Shet { get; set; }

        // Foreign Keys

        [ForeignKey("TemplateID")]
        [JsonIgnore]
        public virtual Template Template { get; set; }

        [ForeignKey("UserID")]
        [JsonIgnore]
        public virtual User User { get; set; }

        [ForeignKey("BranchID")]
        [JsonIgnore]
        public virtual Branch Branch { get; set; }

        [ForeignKey("CompanyID")]
        [JsonIgnore]
        public virtual Company Company { get; set; }

        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User UserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        [JsonIgnore]
        public virtual User UserUpdatedBy { get; set; }

    }
}
