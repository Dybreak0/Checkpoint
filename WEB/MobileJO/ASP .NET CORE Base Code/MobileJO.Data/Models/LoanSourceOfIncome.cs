
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class LoanSourceOfIncome
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("SourceType", TypeName = "varchar(20)")]
        public string SourceType { get; set; }
    }
}
