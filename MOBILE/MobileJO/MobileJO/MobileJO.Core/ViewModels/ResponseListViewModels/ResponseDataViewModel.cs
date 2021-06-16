using MobileJO.Core.Models;
using System.Collections.Generic;

namespace MobileJO.Core.ViewModels.ResponseListViewModels
{
    public class ResponseDataViewModel
    {
        public int MaxLimit { get; set; }
        public int NumOfAnswered { get; set; }
        public List<ResponseModel> Result { get; set; }
        public Pagination Pagination { get; set; }
    }
}
