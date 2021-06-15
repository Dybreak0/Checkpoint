using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.Models
{
    public class QuestionnaireDetailsModel
    {
        public int ResponseID { get; set; }

        public int TemplateID { get; set; }

        public int UserID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int MaxLimit { get; set; }

        public bool Status { get; set; }

        public List<QuestionModel> QuestionList { get; set; }

        public bool CanEditAnswer { get; set; }
    }
}
