using MobileJO.Core.Models;
using System.Collections.Generic;

namespace MobileJO.Core.ViewModels.QuestionnaireListViewModels
{
    public class QuestionnairePaginationViewModel
    {
        public Pagination Pagination { get; set; }
        public List<QuestionnaireModel> Data { get; set; }
    }
}
