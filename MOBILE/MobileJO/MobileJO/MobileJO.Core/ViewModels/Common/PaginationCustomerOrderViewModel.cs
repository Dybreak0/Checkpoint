using MobileJO.Core.Models;
using System.Collections.Generic;

namespace MobileJO.Core.ViewModels.Common
{
    public class PaginationCustomerOrderViewModel
    {
        public Pagination Pagination { get; set; }
        public List<CustomerOrderModel> Data { get; set; }
    }
}