using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileJO.Data.Models
{
    public class QuestionType
    {
        [Key, Column("QuestionTypeID")]
        public int QuestionTypeID { get; set; }

        [Column("Type", TypeName = "varchar(255)")]
        public string Type { get; set; }
    }
}
