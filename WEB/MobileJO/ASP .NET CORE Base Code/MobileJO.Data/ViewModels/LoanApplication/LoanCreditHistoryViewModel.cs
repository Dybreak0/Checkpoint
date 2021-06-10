using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MobileJO.Data.ViewModels.LoanApplication
{
    public class LoanCreditHistoryViewModel
    {
        private string _historyCompanyName;
        private string _historyTypeOfUnit;
        private DateTime _historyDatePurchase;
        private string _historyTerms;
        private string _historyRemainingBalance;

        public int CreditHistoryID { get; set; }

        public int LoanID { get; set; }


        public string HistoryCompanyName
        {
            get => _historyCompanyName;
            set => _historyCompanyName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        public string HistoryTypeOfUnit
        {
            get => _historyTypeOfUnit;
            set => _historyTypeOfUnit = string.IsNullOrEmpty(value) ? "" : value.Trim();

        }
        public DateTime HistoryDatePurchase
        {
            get => _historyDatePurchase;
            set => _historyDatePurchase = value;
        }
        public string HistoryTerms
        {
            get => _historyTerms;
            set => _historyTerms = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        public string HistoryRemainingBalance
        {
            get => _historyRemainingBalance;
            set => _historyRemainingBalance = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
    }
}
