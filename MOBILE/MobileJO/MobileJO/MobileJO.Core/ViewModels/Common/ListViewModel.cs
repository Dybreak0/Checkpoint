using MobileJO.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.ViewModels.Common
{
    public class ListViewModel
    {
        public Pagination Pagination { get; set; }
        public List<LocalJobOrder> Data { get; set; }
        public int Size { get; set; }
        public int Pages { get; set; }
    }
}
