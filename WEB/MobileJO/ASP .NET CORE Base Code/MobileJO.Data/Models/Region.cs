using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    [Table("Region")]
    public class Region
    {
        [Key, Column("RegionID")]
        public int RegionID { get; set; }

        [Column("RegionName", TypeName = "varchar(255)")]
        public string RegionName { get; set; }
    }
}
