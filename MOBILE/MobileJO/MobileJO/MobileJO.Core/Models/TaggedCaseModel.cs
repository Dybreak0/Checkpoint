
namespace MobileJO.Core.Models
{
    public class TaggedCaseModel
    {

        public TaggedCaseModel()
        {
            Status = string.Empty;
            ApplicationType = string.Empty;
            CaseSubject = string.Empty;
        }
        
        public int ID { get; set; }
        public string CaseNumber { get; set; }
        public string Status { get; set; }
        public string ApplicationType { get; set; }
        public string CaseSubject { get; set; }
        public string AccountName { get; set; }
        
    }
}
