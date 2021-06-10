using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MobileJO.Data.Models
{
    [Table("Loan")]
    public class Loan
    {
        [Key, Column("LoanID")]
        public int LoanID { get; set; }

        [Column("ApplicationNumber", TypeName = "varchar(20)")]
        public string ApplicationNumber { get; set; }

        [Column("BranchID")]
        public int BranchID { get; set; }

        /// <summary>
        /// PERSONAL INFO
        /// </summary>
        [Column("LoanStatus", TypeName = "varchar(20)")]
        public string LoanStatus { get; set; } = "Pending";

        [Column("Name", TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Column("BirthDate")]
        public DateTime BirthDate { get; set; }

        [Column("Age")]
        public int Age { get; set; }

        [Column("RegionID")]
        public int RegionID { get; set; }

        [Column("CityID")]
        public int CityID { get; set; }

        [Column("ZipCode")]
        public string ZipCode { get; set; }

        [Column("HouseUnitBuildingNo", TypeName = "varchar(255)")]
        public string HouseUnitBuildingNo { get; set; }

        [Column("StreetBarangay", TypeName = "varchar(255)")]
        public string StreetBarangay { get; set; }

        [Column("Landmark", TypeName = "varchar(255)")]
        public string Landmark { get; set; }

        [Column("BirthPlace", TypeName = "varchar(255)")]
        public string BirthPlace { get; set; }

        [Column("PreviousAddress", TypeName = "varchar(255)")]
        public string PreviousAddress { get; set; }

        [Column("PhoneNo", TypeName = "varchar(255)")]
        public string PhoneNo { get; set; }

        [Column("TelNo", TypeName = "varchar(255)")]
        public string TelNo { get; set; }

        [Column("EmailAddress", TypeName = "varchar(255)")]
        public string EmailAddress { get; set; }

        [Column("Facebook", TypeName = "varchar(255)")]
        public string Facebook { get; set; }

        [Column("RentingName", TypeName = "varchar(255)")]
        public string RentingName { get; set; }

        [Column("RentingAddress", TypeName = "varchar(255)")]
        public string RentingAddress { get; set; }

        [Column("RentingTelNo", TypeName = "varchar(255)")]
        public string RentingTelNo { get; set; }

        [Column("StabilityOfResidence", TypeName = "varchar(40)")]
        public string StabilityOfResidence { get; set; }

        [Column("MaritalStatus", TypeName = "varchar(40)")]
        public string MaritalStatus { get; set; }

        [Column("Land", TypeName = "varchar(20)")]
        public string Land { get; set; }

        [Column("HouseMade", TypeName = "varchar(20)")]
        public string HouseMade { get; set; }

        /// <summary>
        /// TypeOff
        /// </summary>
        [Column("Industry", TypeName = "varchar(255)")]
        public string Industry { get; set; }

        [Column("TypeOf", TypeName = "varchar(255)")]
        public string TypeOf { get; set; }

        /// <summary>
        /// Employed
        /// </summary>
        [Column("EmployedWhere", TypeName = "varchar(255)")]
        public string EmployedWhere { get; set; }

        [Column("EmployedHowLong", TypeName = "varchar(255)")]
        public string EmployedHowLong { get; set; }

        [Column("EmployedPosition", TypeName = "varchar(255)")]
        public string EmployedPosition { get; set; }

        [Column("EmployedPresentBusinessAddress", TypeName = "varchar(255)")]
        public string EmployedPresentBusinessAddress { get; set; }

        [Column("EmployedTelNo", TypeName = "varchar(255)")]
        public string EmployedTelNo { get; set; }

        [Column("EmployedSalary", TypeName = "varchar(255)")]
        public string EmployedSalary { get; set; }

        [Column("EmployedPreviousEmployment", TypeName = "varchar(255)")]
        public string EmployedPreviousEmployment { get; set; }

        [Column("EmployedPreviousBusinessAddress", TypeName = "varchar(255)")]
        public string EmployedPreviousBusinessAddress { get; set; }

        /// <summary>
        /// SELF/OWNED
        /// </summary>
        [Column("BusinessNature", TypeName = "varchar(255)")]
        public string BusinessNature { get; set; }

        [Column("BusinessName", TypeName = "varchar(255)")]
        public string BusinessName { get; set; }

        [Column("BusinessAddress", TypeName = "varchar(255)")]
        public string BusinessAddress { get; set; }

        [Column("BusinessCapital", TypeName = "varchar(255)")]
        public string BusinessCapital { get; set; }

        [Column("BusinessMonthlyIncomeNet", TypeName = "varchar(255)")]
        public string BusinessMonthlyIncomeNet { get; set; }

        [Column("BusinessMonthlyIncomeGross", TypeName = "varchar(255)")]
        public string BusinessMonthlyIncomeGross { get; set; }

        [Column("BusinessPhoneNo", TypeName = "varchar(255)")]
        public string BusinessPhoneNo { get; set; }

        [Column("BusinessHowLong", TypeName = "varchar(255)")]
        public string BusinessHowLong { get; set; }

        /// <summary>
        /// PENSION
        /// </summary>
        [Column("PensionAgency", TypeName = "varchar(255)")]
        public string PensionAgency { get; set; }

        [Column("PensionMonthly", TypeName = "varchar(255)")]
        public string PensionMonthly { get; set; }

        /// <summary>
        /// REMITTANCE
        /// </summary>
        [Column("RemittanceName", TypeName = "varchar(255)")]
        public string RemittanceName { get; set; }

        [Column("RemittanceLocation", TypeName = "varchar(255)")]
        public string RemittanceLocation { get; set; }

        [Column("RemittanceRelationship", TypeName = "varchar(40)")]
        public string RemittanceRelationship { get; set; }

        [Column("RemittanceMonthlyAmount", TypeName = "varchar(255)")]
        public string RemittanceMonthlyAmount { get; set; }

        [Column("RemittanceFrequency", TypeName = "varchar(40)")]
        public string RemittanceFrequency { get; set; }

        /// <summary>
        /// FATHER
        /// </summary>
        [Column("FatherName", TypeName = "varchar(255)")]
        public string FatherName { get; set; }

        [Column("FatherAge")]
        public int? FatherAge { get; set; }

        [Column("FatherAddress", TypeName = "varchar(255)")]
        public string FatherAddress { get; set; }

        [Column("FatherFacebook", TypeName = "varchar(255)")]
        public string FatherFacebook { get; set; }

        [Column("FatherIncomeSource", TypeName = "varchar(255)")]
        public string FatherIncomeSource { get; set; }

        [Column("FatherOfficeAddress", TypeName = "varchar(255)")]
        public string FatherOfficeAddress { get; set; }

        [Column("FatherPosition", TypeName = "varchar(255)")]
        public string FatherPosition { get; set; }

        [Column("FatherHowLong", TypeName = "varchar(255)")]
        public string FatherHowLong { get; set; }

        /// <summary>
        /// MOTHER
        /// </summary>
        [Column("MotherMaidenName", TypeName = "varchar(255)")]
        public string MotherMaidenName { get; set; }

        [Column("MotherAge")]
        public int? MotherAge { get; set; }

        [Column("MotherAddress", TypeName = "varchar(255)")]
        public string MotherAddress { get; set; }

        [Column("MotherFacebook", TypeName = "varchar(255)")]
        public string MotherFacebook { get; set; }

        [Column("MotherIncomeSource", TypeName = "varchar(255)")]
        public string MotherIncomeSource { get; set; }

        [Column("MotherOfficeAddress", TypeName = "varchar(255)")]
        public string MotherOfficeAddress { get; set; }

        [Column("MotherPosition", TypeName = "varchar(255)")]
        public string MotherPosition { get; set; }

        [Column("MotherHowLong", TypeName = "varchar(255)")]
        public string MotherHowLong { get; set; }

        /// <summary>
        /// SPOUSE
        /// </summary>
        [Column("SpouseName", TypeName = "varchar(255)")]
        public string SpouseName { get; set; }

        [Column("SpouseAge")]
        public int? SpouseAge { get; set; }

        [Column("SpouseIncomeSource", TypeName = "varchar(255)")]
        public string SpouseIncomeSource { get; set; }

        [Column("SpouseOfficeAddress", TypeName = "varchar(255)")]
        public string SpouseOfficeAddress { get; set; }

        [Column("SpousePosition", TypeName = "varchar(255)")]
        public string SpousePosition { get; set; }

        [Column("SpouseHowLong", TypeName = "varchar(255)")]
        public string SpouseHowLong { get; set; }

        [Column("SpouseTelNo", TypeName = "varchar(255)")]
        public string SpouseTelNo { get; set; }

        [Column("SpouseSalary", TypeName = "varchar(255)")]
        public string SpouseSalary { get; set; }

        /// <summary>
        /// FATHER IN LAW
        /// </summary>
        [Column("FatherInLawName", TypeName = "varchar(255)")]
        public string FatherInLawName { get; set; }

        [Column("FatherInLawAge")]
        public int? FatherInLawAge { get; set; }

        [Column("FatherInLawAddress", TypeName = "varchar(255)")]
        public string FatherInLawAddress { get; set; }

        [Column("FatherInLawFacebook", TypeName = "varchar(255)")]
        public string FatherInLawFacebook { get; set; }

        [Column("FatherInLawIncomeSource", TypeName = "varchar(255)")]
        public string FatherInLawIncomeSource { get; set; }

        [Column("FatherInLawOfficeAddress", TypeName = "varchar(255)")]
        public string FatherInLawOfficeAddress { get; set; }

        [Column("FatherInLawPosition", TypeName = "varchar(255)")]
        public string FatherInLawPosition { get; set; }

        [Column("FatherInLawHowLong", TypeName = "varchar(255)")]
        public string FatherInLawHowLong { get; set; }

        /// <summary>
        /// MOTHER IN LAW
        /// </summary>
        [Column("MotherInLawName", TypeName = "varchar(255)")]
        public string MotherInLawName { get; set; }

        [Column("MotherInLawAge")]
        public int? MotherInLawAge { get; set; }

        [Column("MotherInLawAddress", TypeName = "varchar(255)")]
        public string MotherInLawAddress { get; set; }

        [Column("MotherInLawFacebook", TypeName = "varchar(255)")]
        public string MotherInLawFacebook { get; set; }

        [Column("MotherInLawIncomeSource", TypeName = "varchar(255)")]
        public string MotherInLawIncomeSource { get; set; }

        [Column("MotherInLawOfficeAddress", TypeName = "varchar(255)")]
        public string MotherInLawOfficeAddress { get; set; }

        [Column("MotherInLawPosition", TypeName = "varchar(255)")]
        public string MotherInLawPosition { get; set; }

        [Column("MotherInLawHowLong", TypeName = "varchar(255)")]
        public string MotherInLawHowLong { get; set; }

        /// <summary>
        /// RATE
        /// </summary>

        [Column("DesiredTCTerms", TypeName = "varchar(255)")]
        public string DesiredTCTerms { get; set; }

        [Column("DesiredTCDownPayment", TypeName = "varchar(255)")]
        public string DesiredTCDownPayment { get; set; }

        [Column("DesiredTCMonthlyInstallment", TypeName = "varchar(255)")]
        public string DesiredTCMonthlyInstallment { get; set; }

        [Column("DesiredTCTotalPrice", TypeName = "varchar(255)")]
        public string DesiredTCTotalPrice { get; set; }

        [Column("DesiredTCTotalRebate", TypeName = "varchar(255)")]
        public string DesiredTCTotalRebate { get; set; }

        [Column("DesiredTCRemarks", TypeName = "varchar(255)")]
        public string DesiredTCRemarks { get; set; }

        /// <summary>
        /// CONFIRMATION
        /// </summary>
        [Column("IsAgreed")]
        public bool IsAgreed { get; set; }

        [Column("ClientSignature", TypeName = "varchar(255)")]
        public string ClientSignature { get; set; }

        [Column("ConfirmationOfficer", TypeName = "varchar(255)")]
        public string ConfirmationOfficer { get; set; }

        [Column("ConfirmationDate")]
        public DateTime? ConfirmationDate { get; set; }

        [Column("ConfirmationTime")]
        public TimeSpan? ConfirmationTime { get; set; }

        /// <summary>
        /// OFFICE USE
        /// </summary>

        [Column("OfficeUseCAName", TypeName = "varchar(255)")]
        public string OfficeUseCAName { get; set; }

        [Column("OfficeUseCARemarks", TypeName = "varchar(255)")]
        public string OfficeUseCARemarks { get; set; }

        [Column("OfficeUseCADate")]
        public DateTime? OfficeUseCADate { get; set; }

        [Column("OfficeUseCATime")]
        public TimeSpan? OfficeUseCATime { get; set; }

        [Column("OfficeUseCCSName", TypeName = "varchar(255)")]
        public string OfficeUseCCSName { get; set; }

        [Column("OfficeUseCCSRemarks", TypeName = "varchar(255)")]
        public string OfficeUseCCSRemarks { get; set; }

        [Column("OfficeUseCCSDate")]
        public DateTime? OfficeUseCCSDate { get; set; }

        [Column("OfficeUseCCSTime")]
        public TimeSpan? OfficeUseCCSTime { get; set; }

        [Column("OfficeUseInvoiceNo", TypeName = "varchar(255)")]
        public string OfficeUseInvoiceNo { get; set; }

        [Column("OfficeUseInvoiceDate")]
        public DateTime? OfficeUseInvoiceDate { get; set; }

        [Column("OfficeUseORNo", TypeName = "varchar(255)")]
        public string OfficeUseORNo { get; set; }

        [Column("OfficeUseORDate")]
        public DateTime? OfficeUseORDate { get; set; }

        [Column("OfficeUseAmount", TypeName = "varchar(255)")]
        public string OfficeUseAmount { get; set; }

        [Column("OfficeUseCashier", TypeName = "varchar(255)")]
        public string OfficeUseCashier { get; set; }


        /// <summary>
        /// Audit Trail
        /// </summary>
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [Column("UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [Column("ApprovedDate")]
        public DateTime? ApprovedDate { get; set; }

        [Column("ApprovedBy")]
        public int? ApprovedBy { get; set; }

        [Column("IsDeleted")]
        public bool IsDeleted { get; set; } = false;

        // Foreign Keys
        [ForeignKey("BranchID")]
        [JsonIgnore]
        public virtual Branch Branch { get; set; }

        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public virtual User UserCreatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        [JsonIgnore]
        public virtual User UserUpdatedBy { get; set; }

        [ForeignKey("ApprovedBy")]
        [JsonIgnore]
        public virtual User UserApprovedBy { get; set; }

        public virtual ICollection<LoanCreditHistory> LoanCreditHistory { get; set; }
        public virtual ICollection<LoanCustomerChildren> LoanCustomerChildren { get; set; }
        public virtual ICollection<LoanPersonalProperty> LoanPersonalProperty { get; set; }
        public virtual ICollection<LoanUnitDesired> LoanUnitDesired { get; set; }
        public virtual ICollection<LoanUnitDesiredTC> LoanUnitDesiredTC { get; set; }
        public virtual ICollection<LoanAttachment> LoanAttachment { get; set; }

        public virtual string CreatedByName
        {
            get
            {
                return (UserCreatedBy != null) ? string.Format(Constants.Common.NameFormat, UserCreatedBy.FirstName, UserCreatedBy.LastName) : string.Empty;
            }
        }

        public virtual string UpdatedByName
        {
            get
            {
                return (UserUpdatedBy != null) ? string.Format(Constants.Common.NameFormat, UserUpdatedBy.FirstName, UserUpdatedBy.LastName) : string.Empty;
            }
        }

        public virtual string ApprovedByName
        {
            get
            {
                return (UserApprovedBy != null) ? string.Format(Constants.Common.NameFormat, UserApprovedBy.FirstName, UserApprovedBy.LastName) : string.Empty;
            }
        }
    }
}
