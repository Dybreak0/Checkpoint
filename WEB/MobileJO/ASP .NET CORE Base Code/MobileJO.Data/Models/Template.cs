using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileJO.Data.Models
{
    public class Template
    {
        [Key, Column("TemplateID")]
        public int TemplateID { get; set; }

        [Column("Title", TypeName = "varchar(255)")]
        public string Title { get; set; }

        [Column("Description", TypeName = "varchar(255)")]
        public string Description { get; set; }

        [Column("Category", TypeName = "varchar(255)")]
        public string Category { get; set; }

        [Column("StartDate")]
        public DateTime StartDate { get; set; }

        [Column("EndDate")]
        public DateTime EndDate { get; set; }

        [Column("MaxLimit")]
        public int MaxLimit { get; set; }

        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }


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

    }
}
