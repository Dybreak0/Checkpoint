using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileJO.Data.Models
{
    public class ForgotPassword
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Column("UserID")]
        public int UserID { get; set; }

        [Column("ResetToken", TypeName = "varchar(255)")]
        public string ResetToken { get; set; }
        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }
        [Column("ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }

        // Foreign Keys
        [ForeignKey("UserID")]
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
