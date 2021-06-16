using MobileJO.Core.Models;
using System.Collections.Generic;

namespace MobileJO.Core.ViewModels.Common
{
    public class PaginationViewModel
    {
        public Pagination Pagination { get; set; }
        public List<JobOrderModel> Data { get; set; }
    }
}
