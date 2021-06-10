using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MobileJO.Data.ViewModels.LoanApplication
{
    public class LoanViewModel
    {
        private string _clientName;
        private DateTime _dateTime;
        private int _age;
        private string _houseUnitBuildingNo;
        private string _streetBarangay;
        private string _landmark;
        private string _birthPlace;
        private string _previousAddress;
        private string _phoneNo;
        private string _telNo;
        private string _emailAddress;
        private string _facebook;
        private string _rentingName;
        private string _rentingAddress;
        private string _rentingTelNo;
        private string _maritalStatus;
        private string _stabilityOfResidence;
        private string _land;
        private string _houseMade;
        private string _industry;
        private string _typeOf;

        private string _employedWhere;
        private string _employedHowLong;
        private string _employedPosition;
        private string _employedPresentBusinessAddress;
        private string _employedTelNo;
        private string _employedSalary;
        private string _employedPreviousEmployment;
        private string _employedPreviousBusinessAddress;

        private string _businessNature;
        private string _businessName;
        private string _businessAddress;
        private string _businessCapital;
        private string _businessMonthlyIncomeNet;
        private string _businessMonthlyIncomeGross;
        private string _businessPhoneNo;
        private string _businessHowLong;

        private string _pensionAgency;
        private string _pensionMonthly;

        private string _remittanceName;
        private string _remittanceLocation;
        private string _remittanceRelationship;
        private string _remittanceMonthlyAmount;
        private string _remittanceFrequency;

        private string _spouseName;
        private int _spouseAge;
        private string _spouseIncomeSource;
        private string _spouseOfficeAddress;
        private string _spousePosition;
        private string _spouseHowLong;
        private string _spouseTelNo;
        private string _spouseSalary;

        private string _fatherName;
        private int _fatherAge;
        private string _fatherAddress;
        private string _fatherFacebook;
        private string _fatherIncomeSource;
        private string _fatherOfficeAddress;
        private string _fatherPosition;
        private string _fatherHowLong;

        private string _motherMaidenName;
        private int _motherAge;
        private string _motherAddress;
        private string _motherFacebook;
        private string _motherIncomeSource;
        private string _motherOfficeAddress;
        private string _motherPosition;
        private string _motherHowLong;

        private string _fatherInLawName;
        private int _fatherInLawAge;
        private string _fatherInLawAddress;
        private string _fatherInLawFacebook;
        private string _fatherInLawIncomeSource;
        private string _fatherInLawOfficeAddress;
        private string _fatherInLawPosition;
        private string _fatherInLawHowLong;

        private string _motherInLawName;
        private int _motherInLawAge;
        private string _motherInLawAddress;
        private string _motherInLawFacebook;
        private string _motherInLawIncomeSource;
        private string _motherInLawOfficeAddress;
        private string _motherInLawPosition;
        private string _motherInLawHowLong;

        private string _confirmationOfficer;
        private DateTime _confirmationDate;
        private TimeSpan _confirmationTime;

        private string _officeUseCAName;
        private string _officeUseCARemarks;
        private DateTime _officeUseCADate;
        private TimeSpan _officeUseCATime;

        private string _officeUseCCSName;
        private string _officeUseCCSRemarks;
        private DateTime _officeUseCCSDate;
        private TimeSpan _officeUseCCSTime;

        private string _officeUseInvoiceNo;
        private DateTime _officeUseInvoiceDate;

        private string _officeUseORNo;
        private DateTime _officeUseORDate;

        private string _officeUseAmount;
        private string _officeUseCashier;

        [JsonProperty("loan_id")]
        public int LoanID { get; set; }

        [JsonProperty("application_no")]
        public string ApplicationNumber { get; set; }

        [JsonProperty("branch_id")]
        public int BranchID { get; set; }

        [JsonProperty("loan_status")]
        public string LoanStatus { get; set; }

        [MaxLength(255)]
        [JsonProperty("client_name")]
        public string Name
        {
            get => _clientName;
            set => _clientName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("birth_date")]
        public DateTime BirthDate {
            get => _dateTime; 
            set => _dateTime = value; 
        }

        [JsonProperty("age")]
        [Required(ErrorMessage = "Age is Required.")]
        public int Age 
        {
            get => _age;
            set => _age = value;
        }

        [JsonProperty("region_id")]
        [Required(ErrorMessage = "Region is Required.")]
        public int RegionID { get; set; }

        [JsonProperty("city_id")]
        [Required(ErrorMessage = "City is Required.")]
        public int CityID { get; set; }

        [MaxLength(255)]
        [JsonProperty("house_unit_building_no")]
        [Required(ErrorMessage = "House/Unit/Building No. is Required.")]
        public string HouseUnitBuildingNo
        {
            get => _houseUnitBuildingNo;
            set => _houseUnitBuildingNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("street_barangay")]
        [Required(ErrorMessage = "Street or Barangay is Required.")]
        public string StreetBarangay
        {
            get => _streetBarangay;
            set => _streetBarangay = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("landmark")]
        [Required(ErrorMessage = "Landmark is Required.")]
        public string Landmark
        {
            get => _landmark;
            set => _landmark = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("birth_place")]
        [Required(ErrorMessage = "Birth Place is Required.")]
        public string BirthPlace
        {
            get => _birthPlace;
            set => _birthPlace = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("previous_address")]
        [Required(ErrorMessage = "Previous Address is Required.")]
        public string PreviousAddress
        {
            get => _previousAddress;
            set => _previousAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(20)]
        [JsonProperty("phone_no")]
        [Required(ErrorMessage = "Phone Number is Required.")]
        public string PhoneNo
        {
            get => _phoneNo;
            set => _phoneNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(20)]
        [JsonProperty("tel_no")]
        [Required(ErrorMessage = "Telephone Number is Required.")]
        public string TelNo
        {
            get => _telNo;
            set => _telNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("email_address")]     
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        [Required(ErrorMessage = "Email Address is Required.")]
        public string EmailAddress
        {
            get => _emailAddress;
            set => _emailAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("facebook")]
        public string Facebook
        {
            get => _facebook;
            set => _facebook = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("renting_name")]
        public string RentingName
        {
            get => _rentingName;
            set => _rentingName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("renting_address")]
        public string RentingAddress
        {
            get => _rentingAddress;
            set => _rentingAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(20)]
        [JsonProperty("renting_tel_no")]
        public string RentingTelNo
        {
            get => _rentingTelNo;
            set => _rentingTelNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(20)]
        [JsonProperty("stability_of_residence")]
        public string StabilityOfResidence
        {
            get => _stabilityOfResidence;
            set => _stabilityOfResidence = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(40)]
        [JsonProperty("marital_status")]
        public string MaritalStatus
        {
            get => _maritalStatus;
            set => _maritalStatus = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(20)]
        [JsonProperty("land")]
        public string Land
        {
            get => _land;
            set => _land = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(20)]
        [JsonProperty("house_made")]
        public string HouseMade
        {
            get => _houseMade;
            set => _houseMade = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        [MaxLength(255)]
        [JsonProperty("industry")]
        public string Industry
        {
            get => _industry;
            set => _industry = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        [MaxLength(255)]
        [JsonProperty("type_of")]
        public string TypeOf
        {
            get => _typeOf;
            set => _typeOf = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("employed_where")]
        public string EmployedWhere
        {
            get => _employedWhere;
            set => _employedWhere = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("employed_how_long")]
        public string EmployedHowLong
        {
            get => _employedHowLong;
            set => _employedHowLong = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("employed_position")]
        public string EmployedPosition
        {
            get => _employedPosition;
            set => _employedPosition = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("employed_present_business_address")]
        public string EmployedPresentBusinessAddress
        {
            get => _employedPresentBusinessAddress;
            set => _employedPresentBusinessAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(25)]
        [JsonProperty("employed_tel_no")]
        public string EmployedTelNo
        {
            get => _employedTelNo;
            set => _employedTelNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("employed_salary")]
        public string EmployedSalary
        {
            get => _employedSalary;
            set => _employedSalary = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("employed_previous")]
        public string EmployedPreviousEmployment
        {
            get => _employedPreviousEmployment;
            set => _employedPreviousEmployment = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("employed_previous_business_address")]
        public string EmployedPreviousBusinessAddress
        {
            get => _employedPreviousBusinessAddress;
            set => _employedPreviousBusinessAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        /// 
        [MaxLength(255)]
        [JsonProperty("business_nature")]
        public string BusinessNature
        {
            get => _businessNature;
            set => _businessNature = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("business_name")]
        public string BusinessName
        {
            get => _businessName;
            set => _businessName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("business_address")]
        public string BusinessAddress
        {
            get => _businessAddress;
            set => _businessAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("business_capital")]
        public string BusinessCapital
        {
            get => _businessCapital;
            set => _businessCapital = value;
        }

        [JsonProperty("business_monthly_income_net")]
        public string BusinessMonthlyIncomeNet
        {
            get => _businessMonthlyIncomeNet;
            set => _businessMonthlyIncomeNet = value;
        }

        [JsonProperty("business_monthly_income_gross")]
        public string BusinessMonthlyIncomeGross
        {
            get => _businessMonthlyIncomeGross;
            set => _businessMonthlyIncomeGross = value;
        }

        [MaxLength(25)]
        [JsonProperty("business_phone_no")]
        public string BusinessPhoneNo
        {
            get => _businessPhoneNo;
            set => _businessPhoneNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("business_how_long")]
        public string BusinessHowLong
        {
            get => _businessHowLong;
            set => _businessHowLong = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        /// 
        [MaxLength(255)]
        [JsonProperty("pension_agency")]
        public string PensionAgency
        {
            get => _pensionAgency;
            set => _pensionAgency = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("pension_monthly")]
        public string PensionMonthly
        {
            get => _pensionMonthly;
            set => _pensionMonthly = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        ///
        [MaxLength(255)]
        [JsonProperty("remittance_name")]
        public string RemittanceName
        {
            get => _remittanceName;
            set => _remittanceName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        [MaxLength(255)]
        [JsonProperty("remittance_location")]
        public string RemittanceLocation
        {
            get => _remittanceLocation;
            set => _remittanceLocation = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        [MaxLength(40)]
        [JsonProperty("remittance_relationship")]
        public string RemittanceRelationship
        {
            get => _remittanceRelationship;
            set => _remittanceRelationship = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        [JsonProperty("remittance_monthly_amount")]
        public string RemittanceMonthlyAmount
        {
            get => _remittanceMonthlyAmount;
            set => _remittanceMonthlyAmount = value;
        }
        [MaxLength(40)]
        [JsonProperty("remittance_frequency")]
        public string RemittanceFrequency
        {
            get => _remittanceFrequency;
            set => _remittanceFrequency = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        ///
        [MaxLength(255)]
        [JsonProperty("spouse_name")]
        public string SpouseName
        {
            get => _spouseName;
            set => _spouseName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("spouse_age")]
        public int SpouseAge
        {
            get => _spouseAge;
            set => _spouseAge = value;
        }

        [MaxLength(255)]
        [JsonProperty("spouse_income_source")]
        public string SpouseIncomeSource
        {
            get => _spouseIncomeSource;
            set => _spouseIncomeSource = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("spouse_office_address")]
        public string SpouseOfficeAddress
        {
            get => _spouseOfficeAddress;
            set => _spouseOfficeAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("spouse_position")]
        public string SpousePosition
        {
            get => _spousePosition;
            set => _spousePosition = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("spouse_how_long")]
        public string SpouseHowLong
        {
            get => _spouseHowLong;
            set => _spouseHowLong = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(25)]
        [JsonProperty("spouse_tel_no")]
        public string SpouseTelNo
        {
            get => _spouseTelNo;
            set => _spouseTelNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("spouse_salary")]
        public string SpouseSalary
        {
            get => _spouseSalary;
            set => _spouseSalary = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        ///
        [MaxLength(255)]
        [JsonProperty("father_name")]
        [Required(ErrorMessage = "Father's Name is Required.")]
        public string FatherName
        {
            get => _fatherName;
            set => _fatherName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("father_age")]
        [Required(ErrorMessage = "Father's Age is Required.")]
        public int FatherAge
        {
            get => _fatherAge;
            set => _fatherAge = value;
        }

        [MaxLength(255)]
        [JsonProperty("father_address")]
        [Required(ErrorMessage = "Father's Address is Required.")]
        public string FatherAddress
        {
            get => _fatherAddress;
            set => _fatherAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("father_facebook")]
        public string FatherFacebook
        {
            get => _fatherFacebook;
            set => _fatherFacebook = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("father_income_source")]
        public string FatherIncomeSource
        {
            get => _fatherIncomeSource;
            set => _fatherIncomeSource = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("father_office_address")]
        public string FatherOfficeAddress
        {
            get => _fatherOfficeAddress;
            set => _fatherOfficeAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("father_position")]
        public string FatherPosition
        {
            get => _fatherPosition;
            set => _fatherPosition = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("father_how_long")]
        public string FatherHowLong
        {
            get => _fatherHowLong;
            set => _fatherHowLong = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        ///
        [MaxLength(255)]
        [JsonProperty("mother_name")]
        [Required(ErrorMessage = "Mother's Name is Required.")]
        public string MotherMaidenName
        {
            get => _motherMaidenName;
            set => _motherMaidenName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("mother_age")]
        [Required(ErrorMessage = "Mother's Age is Required.")]
        public int MotherAge
        {
            get => _motherAge;
            set => _motherAge = value;
        }

        [MaxLength(255)]
        [JsonProperty("mother_address")]
        [Required(ErrorMessage = "Mother's Address is Required.")]
        public string MotherAddress
        {
            get => _motherAddress;
            set => _motherAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("mother_facebook")]
        public string MotherFacebook
        {
            get => _motherFacebook;
            set => _motherFacebook = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("mother_income_source")]
        public string MotherIncomeSource
        {
            get => _motherIncomeSource;
            set => _motherIncomeSource = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("mother_office_address")]
        public string MotherOfficeAddress
        {
            get => _motherOfficeAddress;
            set => _motherOfficeAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("mother_position")]
        public string MotherPosition
        {
            get => _motherPosition;
            set => _motherPosition = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("mother_how_long")]
        public string MotherHowLong
        {
            get => _motherHowLong;
            set => _motherHowLong = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        ///
        [MaxLength(255)]
        [JsonProperty("father_in_law_name")]
        [Required(ErrorMessage = "Father's Name is Required.")]
        public string FatherInLawName
        {
            get => _fatherInLawName;
            set => _fatherInLawName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("father_in_law_age")]
        [Required(ErrorMessage = "Father In Law's Age is Required.")]
        public int FatherInLawAge
        {
            get => _fatherInLawAge;
            set => _fatherInLawAge = value;
        }

        [MaxLength(255)]
        [JsonProperty("father_in_law_address")]
        [Required(ErrorMessage = "Father In Law's Address is Required.")]
        public string FatherInLawAddress
        {
            get => _fatherInLawAddress;
            set => _fatherInLawAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("father_in_law_facebook")]
        public string FatherInLawFacebook
        {
            get => _fatherInLawFacebook;
            set => _fatherInLawFacebook = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("father_in_law_income_source")]
        public string FatherInLawIncomeSource
        {
            get => _fatherInLawIncomeSource;
            set => _fatherInLawIncomeSource = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("father_in_law_office_address")]
        public string FatherInLawOfficeAddress
        {
            get => _fatherInLawOfficeAddress;
            set => _fatherInLawOfficeAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("father_in_law_position")]
        public string FatherInLawPosition
        {
            get => _fatherInLawPosition;
            set => _fatherInLawPosition = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("father_in_law_how_long")]
        public string FatherInLawHowLong
        {
            get => _fatherInLawHowLong;
            set => _fatherInLawHowLong = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        [MaxLength(255)]
        [JsonProperty("mother_in_law_name")]
        [Required(ErrorMessage = "Mother In Law's Name is Required.")]
        public string MotherInLawName
        {
            get => _motherInLawName;
            set => _motherInLawName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [JsonProperty("mother_in_law_age")]
        [Required(ErrorMessage = "Mother In Law's Age is Required.")]
        public int MotherInLawAge
        {
            get => _motherInLawAge;
            set => _motherInLawAge = value;
        }

        [MaxLength(255)]
        [JsonProperty("mother_in_law_address")]
        [Required(ErrorMessage = "Mother In Law's Address is Required.")]
        public string MotherInLawAddress
        {
            get => _motherInLawAddress;
            set => _motherInLawAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("mother_in_law_facebook")]
        public string MotherInLawFacebook
        {
            get => _motherInLawFacebook;
            set => _motherInLawFacebook = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("mother_in_law_income_source")]
        public string MotherInLawIncomeSource
        {
            get => _motherInLawIncomeSource;
            set => _motherInLawIncomeSource = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("mother_in_law_office_address")]
        public string MotherInLawOfficeAddress
        {
            get => _motherInLawOfficeAddress;
            set => _motherInLawOfficeAddress = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("mother_in_law_position")]
        public string MotherInLawPosition
        {
            get => _motherInLawPosition;
            set => _motherInLawPosition = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        [MaxLength(255)]
        [JsonProperty("mother_in_law_how_long")]
        public string MotherInLawHowLong
        {
            get => _motherInLawHowLong;
            set => _motherInLawHowLong = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        [MaxLength(255)]
        [JsonProperty("confirmation_officer")]
        public string ConfirmationOfficer
        {
            get => _confirmationOfficer;
            set => _confirmationOfficer = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        [JsonProperty("confirmation_date")]
        public DateTime ConfirmationDate
        {
            get => _confirmationDate;
            set => _confirmationDate = value;
        }
        [JsonProperty("confirmation_time")]
        public TimeSpan ConfirmationTime
        {
            get => _confirmationTime;
            set => _confirmationTime = value;
        }
        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        [MaxLength(255)]
        [JsonProperty("office_use_ca_name")]
        public string OfficeCAName
        {
            get => _officeUseCAName;
            set => _officeUseCAName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        [MaxLength(255)]
        [JsonProperty("office_use_ca_remarks")]
        public string OfficeUseCARemarks
        {
            get => _officeUseCARemarks;
            set => _officeUseCARemarks = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        [JsonProperty("office_use_ca_date")]
        public DateTime OfficeUseCADate
        {
            get => _officeUseCADate;
            set => _officeUseCADate = value;
        }
        [JsonProperty("office_use_ca_time")]
        public TimeSpan OfficeUseCATime
        {
            get => _officeUseCATime;
            set => _officeUseCATime = value;
        }

        [MaxLength(255)]
        [JsonProperty("office_use_ccs_name")]
        public string OfficeUseCCSName
        {
            get => _officeUseCCSName;
            set => _officeUseCCSName = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        [MaxLength(255)]
        [JsonProperty("office_use_ccs_remarks")]
        public string OfficeUseCCSRemarks
        {
            get => _officeUseCCSRemarks;
            set => _officeUseCCSRemarks = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        [JsonProperty("office_use_ccs_date")]
        public DateTime OfficeUseCCSDate
        {
            get => _officeUseCCSDate;
            set => _officeUseCCSDate = value;
        }
        [JsonProperty("office_use_ccs_time")]
        public TimeSpan OfficeUseCCSTime
        {
            get => _officeUseCCSTime;
            set => _officeUseCCSTime = value;
        }

        [MaxLength(255)]
        [JsonProperty("office_use_invoice_no")]
        public string OfficeUseInvoiceNo
        {
            get => _officeUseInvoiceNo;
            set => _officeUseInvoiceNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        [JsonProperty("office_use_invoice_date")]
        public DateTime OfficeUseInvoiceDate
        {
            get => _officeUseInvoiceDate;
            set => _officeUseInvoiceDate = value;
        }

        [MaxLength(255)]
        [JsonProperty("office_use_or_no")]
        public string OfficeUseORNo
        {
            get => _officeUseORNo;
            set => _officeUseORNo = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }
        [JsonProperty("office_use_or_date")]
        public DateTime OfficeUseORDate
        {
            get => _officeUseORDate;
            set => _officeUseORDate = value;
        }

        [JsonProperty("office_use_amount")]
        public string OfficeUseAmount
        {
            get => _officeUseAmount;
            set => _officeUseAmount = value;
        }

        [MaxLength(255)]
        [JsonProperty("office_use_cashier")]
        public string OfficeUseCashier
        {
            get => _officeUseCashier;
            set => _officeUseCashier = string.IsNullOrEmpty(value) ? "" : value.Trim();
        }

        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("approved_date")]
        public DateTime ApprovedDate { get; set; }


        [JsonProperty("created_by_name")]
        public string CreatedByName { get; set; }

        [JsonProperty("updated_by_name")]
        public string UpdatedByName { get; set; }

        [JsonProperty("approved_by_name")]
        public string ApprovedByName { get; set; }
    }
}
