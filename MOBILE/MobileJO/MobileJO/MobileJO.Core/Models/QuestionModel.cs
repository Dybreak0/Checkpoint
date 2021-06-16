using System.Collections.Generic;

namespace MobileJO.Core.Models
{
    public class QuestionModel
    {
        public int QuestionID { get; set; }
        public int QuestionTypeID { get; set; }
        public int QuestionNo { get; set; }
        public string Question { get; set; }
        public List<ChoiceModel> Choices { get; set; }
        public ChoiceModel SelectedItem { get; set; }
        public ChoiceModel SliderProps { get; set; }
        public AnswerModel Answer { get; set; }
    }
}
