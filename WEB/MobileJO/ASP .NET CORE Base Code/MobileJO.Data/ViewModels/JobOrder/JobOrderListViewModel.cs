namespace MobileJO.Data.ViewModels.JobOrder
{
    public class JobOrderListViewModel
    {
        public int ID { get; set; }
        public string JobOrderNumber { get; set; }
        public string StatusID { get; set; }
        public string ApplicationType { get; set; }
        public string JobOrderSubject { get; set; }
        public bool IsDeleted { get; set; }
        public string Color { get; set; }
        public int CreatedBy { get; set; }
    }
}

