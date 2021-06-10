using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileJO.Data.Models
{
    public class Question
    {
        [Key, Column("QuestionID")]
        public int QuestionID { get; set; }
        [Column("TemplateID")]
        public int TemplateID { get; set; }

        [Column("Question", TypeName = "varchar(255)")]
        public string Qquestion { get; set; }

        [Column("QuestionTypeID")]
        public int QuestionTypeID { get; set; }

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

        [ForeignKey("TemplateID")]
        [JsonIgnore]
        public virtual Template TemplateId { get; set; }

        [ForeignKey("QuestionTypeID")]
        [JsonIgnore]
        public virtual QuestionType QuestionTypeId { get; set; }


        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User UserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        [JsonIgnore]
        public virtual User UserUpdatedBy { get; set; }
    }
}
