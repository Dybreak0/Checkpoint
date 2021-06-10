using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    [Table("RegionCity")]
    public class RegionCity
    {
        [Key, Column("CityID")]
        public int CityID { get; set; }

        [Column("RegionID")]
        public int RegionID { get; set; }

        [Column("CityName", TypeName = "varchar(255)")]
        public string CityName { get; set; }

        [Column("ZipCode", TypeName = "varchar(6)")]
        public string ZipCode { get; set; }

        [ForeignKey("RegionID")]
        [JsonIgnore]
        public virtual Region Region { get; set; }
    }
}
