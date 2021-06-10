using MobileJO.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MobileJO.Data.ViewModels.Common
{
    public class CasesListViewModel
    {
        public Pagination Pagination { get; set; }

        public List<AssignedCasesList> Data { get; set; }
    }
}
