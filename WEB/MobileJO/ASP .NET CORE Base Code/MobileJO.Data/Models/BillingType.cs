using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class BillingType
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("BillingTypeName")]
        public string BillingTypeName { get; set; }
    }
}
