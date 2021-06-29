namespace MobileJO.Core.Models
{
    public class CustomerOrderModel
    {
        public CustomerOrderModel()
        {
            CustomerOrderNumber = string.Empty;
            CustomerOrderStatus = string.Empty;
        }

        public int ID { get; set; }
        public int ServerID { get; set; }
        public string CustomerOrderNumber { get; set; }
        public string CustomerOrderStatus { get; set; }
        public string Color { get; set; }
    }
}
