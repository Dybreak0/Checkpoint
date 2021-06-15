namespace MobileJO.Core.ViewModels
{
    public class FirstPageViewModel
    {

        public string JobOrderSubject { get; set; }
        public int AccountID { get; set; }
        public string Branch { get; set; }
        public string DateTimeStart { get; set; }
        public string DateTimeEnd { get; set; }

        public int ApplicationType { get; set; }
        public string ActivityDetails { get; set; }
        public string RootCauseAnalysis { get; set; }
    }
}
