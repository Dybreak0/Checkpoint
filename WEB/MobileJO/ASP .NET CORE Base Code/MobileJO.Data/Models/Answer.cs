using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class Answer
    {
        [Key, Column("AnswerID")]
        public int AnswerID { get; set; }

        [Column("ResponseID")]
        public int ResponseID { get; set; }

        [Column("TemplateID")]
        public int TemplateID { get; set; }

        [Column("QuestionID")]
        public int QuestionID { get; set; }

        [Column("ChoiceID")]
        public int ChoiceID { get; set; }

        [Column("UserID")]
        public int UserID { get; set; }

        [Column("Value")]
        public string Value { get; set; }

        [Column("DateSubmitted")]
        public DateTime DateSubmitted { get; set; }

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

        // Foreign Keys
        [ForeignKey("ResponseID")]
        [JsonIgnore]
        public virtual Response Response { get; set; }

        [ForeignKey("TemplateID")]
        [JsonIgnore]
        public virtual Template Template { get; set; }

        [ForeignKey("QuestionID")]
        [JsonIgnore]
        public virtual Question Question { get; set; }

        [ForeignKey("UserID")]
        [JsonIgnore]
        public virtual User User { get; set; }

        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User UserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        [JsonIgnore]
        public virtual User UserUpdatedBy { get; set; }

    }
}
