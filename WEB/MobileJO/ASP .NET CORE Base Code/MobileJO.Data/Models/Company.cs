using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileJO.Data.Models
{
    public class Company
    {
        [Key, Column("CompanyID")]
        public int CompanyID { get; set; }
        [Column("CompanyName", TypeName = "varchar(255)")]
        public string CompanyName { get; set; }
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
        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User UserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        [JsonIgnore]
        public virtual User UserUpdateBy { get; set; }

    }
}
