using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class LoanStatus
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("Status", TypeName = "varchar(20)")]
        public string Status { get; set; }
    }
}
