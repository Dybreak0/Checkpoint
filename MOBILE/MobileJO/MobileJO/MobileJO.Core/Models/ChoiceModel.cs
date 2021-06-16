namespace MobileJO.Core.Models
{
    public class ChoiceModel
    {
        public int ChoiceID { get; set; }
        public int AnswerID { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public string MinLabel { get; set; }
        public string MaxLabel { get; set; }
        public override string ToString() => Label;
    }
}
