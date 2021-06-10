using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class TaggedCase
    {

        public int ID { get; set; }
        public int JobOrderID { get; set; }
        public int CaseID { get; set; }

        [ForeignKey("JobOrderID")]
        [JsonIgnore]
        public virtual JobOrder JobOrder { get; set; }

        [ForeignKey("CaseID")]
        [JsonIgnore]
        public virtual AssignedCase AssignedCases { get; set; }
    }
}
