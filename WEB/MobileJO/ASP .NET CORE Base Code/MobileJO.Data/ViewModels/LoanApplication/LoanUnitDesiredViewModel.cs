using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MobileJO.Data.ViewModels.LoanApplication
{
    public class LoanUnitDesiredViewModel
    {
        private string _desiredBrandModel;
        private string _desiredSerialNo;
        private string _desiredCode;
        private string _desiredAmount;
        private string _desiredAccounting;

        public int UnitDesiredID { get; set; }

        public int LoanID { get; set; }
        public string DesiredBrandModel
        {
            get => _desiredBrandModel;
            set => _desiredBrandModel = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        public string DesiredSerialNo
        {
            get => _desiredSerialNo;
            set => _desiredSerialNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        public string DesiredCode
        {
            get => _desiredCode;
            set => _desiredCode = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        public string DesiredAmount
        {
            get => _desiredAmount;
            set => _desiredAmount = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        public string DesiredAccounting
        {
            get => _desiredAccounting;
            set => _desiredAccounting = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
    }
}
