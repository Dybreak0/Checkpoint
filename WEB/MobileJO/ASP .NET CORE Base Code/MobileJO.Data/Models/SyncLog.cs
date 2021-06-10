using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class SyncLog
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("CaseNumber", TypeName = "varchar(255)")]
        public string CaseNumber { get; set; }

        [Column("DateSync")]
        public DateTime DateSync { get; set; }

        [Column("ErrMsg", TypeName = "varchar(255)")]
        public string ErrMsg { get; set; }
    }
}
