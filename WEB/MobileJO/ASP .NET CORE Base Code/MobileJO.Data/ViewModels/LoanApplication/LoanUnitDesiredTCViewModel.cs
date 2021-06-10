using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MobileJO.Data.ViewModels.LoanApplication
{
    public class LoanUnitDesiredTCViewModel
    {

        public string _desiredTCBrandModel;
        public string _desiredTCTerms;
        public string _desiredTCDownPayment;
        public string _desiredTCMonthlyInstallment;
        public string _desiredTCTotalPrice;
        public string _desiredTCTotalRebate;
        public string _desiredTCRemarks;


        public int UnitDesiredTCID { get; set; }

        public int LoanID { get; set; }


        public string DesiredTCBrandModel
        {
            get => _desiredTCBrandModel;
            set => _desiredTCBrandModel = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        public string DesiredTCTerms
        {
            get => _desiredTCTerms;
            set => _desiredTCTerms = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        public string DesiredTCDownPayment
        {
            get => _desiredTCDownPayment;
            set => _desiredTCDownPayment = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        public string DesiredTCMonthlyInstallment
        {
            get => _desiredTCMonthlyInstallment;
            set => _desiredTCMonthlyInstallment = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        public string DesiredTCTotalPrice
        {
            get => _desiredTCTotalPrice;
            set => _desiredTCTotalPrice = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        public string DesiredTCTotalRebate
        {
            get => _desiredTCTotalRebate;
            set => _desiredTCTotalRebate = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        public string DesiredTCRemarks
        {
            get => _desiredTCRemarks;
            set => _desiredTCRemarks = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
    }
}
