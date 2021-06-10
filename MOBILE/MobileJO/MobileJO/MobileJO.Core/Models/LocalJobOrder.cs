using SQLite;
using System;

namespace MobileJO.Core.Models
{
    public class LocalJobOrder
    {
        [PrimaryKey][AutoIncrement]
        public int ID { get; set; }

        public int ServerID { get; set; }

        public string JobOrderNumber { get; set; }

        public string JobOrderSubject { get; set; }

        public string Branch { get; set; }

        public int AccountID { get; set; }

        public int ApplicationTypeID { get; set; }

        public DateTime DateTimeStart { get; set; }
        
        public DateTime DateTimeEnd { get; set; }

        public string ActivityDetails { get; set; }

        public string RootCauseAnalysis { get; set; }

        public string PreventiveAction { get; set; }

        public string Remarks { get; set; }

        public string NextStep { get; set; }

        public string Attendees { get; set; }

        public bool IsBilled { get; set; }

        public bool IsCollaterals { get; set; }

        public string ClientSignature { get; set; }

        public bool IsFixed { get; set; }

        public bool IsSatisfied { get; set; }

        public int ClientRating { get; set; }

        public int StatusID { get; set; }

        public bool IsDeleted { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public int CreatedBy { get; set; }
        
        public DateTime UpdatedDate { get; set; }

        public DateTime? LastSyncDate { get; set; }

        public int UpdatedBy { get; set; }

    }
}
