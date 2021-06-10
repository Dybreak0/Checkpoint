using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileJO.Data.Models
{
    public class Template_Branch
    {
        [Key, Column("BranchID")]
        public int BranchID { get; set; }
        [Key, Column("TemplateID")]
        public int TemplateID { get; set; }

        // Foreign Keys
        [ForeignKey("BranchID")]
        [JsonIgnore]
        public virtual Branch BranchId { get; set; }
        [ForeignKey("TemplateID")]
        [JsonIgnore]
        public virtual Template TemplateId { get; set; }
    }
}
