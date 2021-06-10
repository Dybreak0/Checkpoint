using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MobileJO.Data.ViewModels.LoanApplication
{
    public class LoanCustomerChildrenViewModel
    {
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        ///

        private string _childName;
        private string _childAge;
        private string _childAddress;
        private string _childTelNo;
        private string _childEmploySchool;
        private string _childEmploySchoolAddress;
        private string _childPosGrade;
        private string _childHowLong;

        public int ChildID { get; set; }

        public int LoanID { get; set; }

        public string ChildName
        {
            get => _childName;
            set => _childName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        public string ChildAge
        {
            get => _childAge;
            set => _childAge = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        public string ChildHomeAddress
        {
            get => _childAddress;
            set => _childAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        public string ChildTelNo
        {
            get => _childTelNo;
            set => _childTelNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        public string ChildEmploySchool
        {
            get => _childEmploySchool;
            set => _childEmploySchool = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        public string ChildEmploySchoolAddress
        {
            get => _childEmploySchoolAddress;
            set => _childEmploySchoolAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        public string ChildPosGrade
        {
            get => _childPosGrade;
            set => _childPosGrade = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        public string ChildHowLong
        {
            get => _childHowLong;
            set => _childHowLong = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
    }
}
