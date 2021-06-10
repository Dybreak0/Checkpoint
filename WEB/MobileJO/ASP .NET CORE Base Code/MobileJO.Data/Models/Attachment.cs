using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class Attachment
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("JobOrderID")]
        public int JobOrderID { get; set; }

        [Column("Filename", TypeName = "varchar(255)")]
        public string Filename { get; set; }

        // Foreign Keys
        [ForeignKey("JobOrderID")]
        [JsonIgnore]
        public virtual JobOrder JobOrder { get; set; }
    }
}
