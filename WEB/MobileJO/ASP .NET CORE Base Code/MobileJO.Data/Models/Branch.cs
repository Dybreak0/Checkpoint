using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class Branch
    {
        [Key, Column("BranchID")]
        public int BranchID { get; set; }
        [Column("CompanyID")]
        public int CompanyID { get; set; }
        [Column("Name", TypeName = "varchar(255)")]
        public string Name { get; set; }
        [Column("Description", TypeName = "varchar(255)")]
        public string Description { get; set; }
        [Column("Status")]
        public bool Status { get; set; }
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Column("UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        [Column("UpdatedBy")]
        public int UpdatedBy { get; set; }

        // Foreign Keys
        [ForeignKey("CompanyID")]
        [JsonIgnore]
        public virtual Company CompanyId { get; set; }
        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User UserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        [JsonIgnore]
        public virtual User UserUpdatedBy { get; set; }
    }
}
