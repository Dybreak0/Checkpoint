using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileJO.Data.Models
{
    public class EmailSetup
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("TypeID")]
        public int TypeID { get; set; }
        [Column("EmailAddress", TypeName = "varchar(255)")]
        public string EmailAddress { get; set; }


        // Foreign Keys
        [ForeignKey("TypeID")]
        [JsonIgnore]
        public virtual EmailType EmailType { get; set; }
    }
}
