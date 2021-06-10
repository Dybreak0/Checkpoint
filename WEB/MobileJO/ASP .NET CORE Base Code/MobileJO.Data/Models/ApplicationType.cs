using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class ApplicationType
    {
        [Column("ID")]
        public int ID { get; set; }

        [Column("ApplicationName", TypeName = "varchar(255)")]
        public string ApplicationName { get; set; }
    }
}
