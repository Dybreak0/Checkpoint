using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileJO.Data.Models
{
    public class EmailType
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("Type", TypeName = "varchar(5)")]
        public string Type { get; set; }
    }
}
