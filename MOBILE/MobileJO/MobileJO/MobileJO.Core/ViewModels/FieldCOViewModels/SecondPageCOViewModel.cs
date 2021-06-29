using MobileJO.Core.Models;
using System;
using System.Collections.ObjectModel;

namespace MobileJO.Core.ViewModels.FieldCOViewModels
{
    public class SecondPageCOViewModel
    {

        public string ClientResCertNo { get; set; }
        public string ClientPlaceIssued { get; set; }
        public DateTime ClientDate { get; set; }
        public string ClientSignature { get; set; }

        public string SpouseResCertNo { get; set; }
        public string SpousePlaceIssued { get; set; }
        public DateTime SpouseDate { get; set; }
        public string SpouseSignature { get; set; }

        public DateTime DeliveryDate { get; set; }
        public TimeSpan DeliveryTime { get; set; }

        public string ClosingOfficer { get; set; }
        public DateTime ClosingOfficerDate { get; set; }
        public TimeSpan ClosingOfficerTime { get; set; }
        public string ClosingOfficerRemarks { get; set; }

        public string BranchManagerSignature { get; set; }
        public DateTime BranchManagerDate { get; set; }
        public TimeSpan BranchManagerTime { get; set; }
        public string BranchManagerRemarks { get; set; }
    }
}
