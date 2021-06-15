

namespace MobileJO.Core.Models
{
    public class JobOrderModel
    {
        public JobOrderModel()
        {
            JobOrderNumber = string.Empty;
            StatusID = string.Empty;
            ApplicationType = string.Empty;
            JobOrderSubject = string.Empty;
        }

        public int ID { get; set; }
        public int ServerID { get; set; }
        public string JobOrderNumber { get; set; }
        public string StatusID { get; set; }
        public string ApplicationType { get; set; }
        public string JobOrderSubject { get; set; }
        public string Color { get; set; }
    }
}
