using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class Role
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("Role", TypeName = "varchar(255)")]
        public string RoleName { get; set; }

    }
}
