using Newtonsoft.Json;
using SQLite;
using System;

namespace MobileJO.Core.Models
{
    public class Response
    {

        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }
        public int ResponseID { get; set; }
        public int TemplateID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int UserID { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Remarks { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime LastSyncDate { get; set; }

    }
}
