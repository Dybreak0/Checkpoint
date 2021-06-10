using Newtonsoft.Json;
using SQLite;

namespace MobileJO.Core.Models
{
    public class CaseStatus
    {
        [PrimaryKey][AutoIncrement]
        public int ID { get; set; }
        public string StatusName { get; set; }
    }
}
