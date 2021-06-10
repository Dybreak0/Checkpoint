using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class JobOrderBillingType
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("JobOrderID")]
        public int JobOrderID { get; set; }

        [Column("BillingTypeID")]
        public int BillingTypeID { get; set; }

        // Foreign Keys
        [ForeignKey("JobOrderID")]
        [JsonIgnore]
        public virtual JobOrder JobOrder { get; set; }

        [ForeignKey("BillingTypeID")]
        [JsonIgnore]
        public virtual BillingType BillingType { get; set; }
    }
}
