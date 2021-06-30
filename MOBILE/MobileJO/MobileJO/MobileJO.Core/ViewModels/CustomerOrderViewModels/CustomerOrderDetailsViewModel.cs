using MobileJO.Core.Models;
using System;
using System.Collections.Generic;

namespace MobileJO.Core.ViewModels.CustomerOrderViewModels
{
    public class CustomerOrderDetailsViewModel
    {
        public int OrderID { get; set; }
        public string CustomerOrderNumber { get; set; }
        public int BranchID { get; set; }
        public string CustomerOrderStatus { get; set; } = "Pending";
        public string Name { get; set; }
        public string SpouseName { get; set; }
        public string DeliveryAddress { get; set; }
        public string OfficialReceipt { get; set; }
        public string TotalAmount { get; set; }
        public string ClientResCertNo { get; set; }
        public string ClientPlaceIssued { get; set; }
        public DateTime? ClientDate { get; set; }
        public string SpouseResCertNo { get; set; }
        public string SpousePlaceIssued { get; set; }
        public DateTime? SpouseDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public TimeSpan? DeliveryTime { get; set; }
        public string ClosingOfficer { get; set; }
        public DateTime? ClosingOfficerDate { get; set; }
        public TimeSpan? ClosingOfficerTime { get; set; }
        public string ClosingOfficerRemarks { get; set; }
        public DateTime? BranchManagerDate { get; set; }
        public TimeSpan? BranchManagerTime { get; set; }
        public string BranchManagerRemarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedBy { get; set; }

        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
        public string ApprovedByName { get; set; }


        //SAVE TO SERVER
        public List<UnitDesiredModel> UnitDesireds { get; set; }
        public FileViewModel ClientSignature { get; set; }
        public FileViewModel SpouseSignature { get; set; }
        public FileViewModel BranchManagerSignature { get; set; }
    }
}
