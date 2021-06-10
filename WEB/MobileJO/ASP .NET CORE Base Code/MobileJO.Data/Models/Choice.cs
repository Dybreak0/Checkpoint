using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileJO.Data.Models
{
    public class Choice
    {
        [Key, Column("ChoiceID")]
        public int ChoiceID { get; set; }
        
        [Column("QuestionID")]
        public int QuestionID { get; set; }

        [Column("Label", TypeName = "varchar(255)")]
        public string Label { get; set; }

        [Column("Value", TypeName = "varchar(255)")]
        public string Value { get; set; }
        
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

        [ForeignKey("QuestionID")]
        [JsonIgnore]
        public virtual Question QuestionId { get; set; }


        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User UserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        [JsonIgnore]
        public virtual User UserUpdatedBy { get; set; }
    }
}
