using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using MobileJO.Data.Models;

namespace MobileJO.Data.ViewModels.LoanApplication
{
    public class LoanDetailsViewModel
    {
        [JsonProperty("loan_id")]
        public int LoanID { get; set; }

        [JsonProperty("application_no")]
        public string ApplicationNumber { get; set; }

        [JsonProperty("branch_id")]
        public int BranchID { get; set; }

        [JsonProperty("loan_status")]
        public string LoanStatus { get; set; } = "Pending";

        [JsonProperty("client_name")]
        public string Name { get; set; }

        [JsonProperty("birth_date")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("region_id")]
        public int RegionID { get; set; }

        [JsonProperty("city_id")]
        public int CityID { get; set; }

        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }

        [JsonProperty("house_unit_building_no")]
        public string HouseUnitBuildingNo { get; set; }

        [JsonProperty("street_barangay")]
        public string StreetBarangay { get; set; }

        [JsonProperty("landmark")]
        public string Landmark { get; set; }

        [JsonProperty("birth_place")]
        public string BirthPlace { get; set; }

        [JsonProperty("previous_address")]
        public string PreviousAddress { get; set; }

        [JsonProperty("phone_no")]
        public string PhoneNo { get; set; }

        [JsonProperty("tel_no")]
        public string TelNo { get; set; }

        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("renting_name")]
        public string RentingName { get; set; }

        [JsonProperty("renting_address")]
        public string RentingAddress { get; set; }

        [JsonProperty("renting_tel_no")]
        public string RentingTelNo { get; set; }

        [JsonProperty("stability_of_residence")]
        public string StabilityOfResidence { get; set; }

        [JsonProperty("marital_status")]
        public string MaritalStatus { get; set; }

        [JsonProperty("land")]
        public string Land { get; set; }

        [JsonProperty("house_made")]
        public string HouseMade { get; set; }

        [JsonProperty("industry")]
        public string Industry { get; set; }


        [JsonProperty("type_of")]
        public string TypeOf { get; set; }

        [JsonProperty("employed_where")]
        public string EmployedWhere { get; set; }

        [JsonProperty("employed_how_long")]
        public string EmployedHowLong { get; set; }

        [JsonProperty("employed_position")]
        public string EmployedPosition { get; set; }

        [JsonProperty("employed_present_business_address")]
        public string EmployedPresentBusinessAddress { get; set; }

        [JsonProperty("employed_tel_no")]
        public string EmployedTelNo { get; set; }

        [JsonProperty("employed_salary")]
        public string EmployedSalary { get; set; }

        [JsonProperty("employed_previous")]
        public string EmployedPreviousEmployment { get; set; }

        [JsonProperty("employed_previous_business_address")]
        public string EmployedPreviousBusinessAddress { get; set; }

        [JsonProperty("business_nature")]
        public string BusinessNature { get; set; }

        [JsonProperty("business_name")]
        public string BusinessName { get; set; }

        [JsonProperty("business_address")]
        public string BusinessAddress { get; set; }

        [JsonProperty("business_capital")]
        public string BusinessCapital { get; set; }

        [JsonProperty("business_monthly_income_net")]
        public string BusinessMonthlyIncomeNet { get; set; }

        [JsonProperty("business_monthly_income_gross")]
        public string BusinessMonthlyIncomeGross { get; set; }

        [JsonProperty("business_phone_no")]
        public string BusinessPhoneNo { get; set; }

        [JsonProperty("business_how_long")]
        public string BusinessHowLong { get; set; }

        [JsonProperty("pension_agency")]
        public string PensionAgency { get; set; }

        [JsonProperty("pension_monthly")]
        public string PensionMonthly { get; set; }

        [JsonProperty("remittance_name")]
        public string RemittanceName { get; set; }

        [JsonProperty("remittance_location")]
        public string RemittanceLocation { get; set; }

        [JsonProperty("remittance_relationship")]
        public string RemittanceRelationship { get; set; }

        [JsonProperty("remittance_monthly_amount")]
        public string RemittanceMonthlyAmount { get; set; }

        [JsonProperty("remittance_frequency")]
        public string RemittanceFrequency { get; set; }

        [JsonProperty("father_name")]
        public string FatherName { get; set; }

        [JsonProperty("father_age")]
        public int? FatherAge { get; set; }

        [JsonProperty("father_address")]
        public string FatherAddress { get; set; }

        [JsonProperty("father_facebook")]
        public string FatherFacebook { get; set; }

        [JsonProperty("father_income_source")]
        public string FatherIncomeSource { get; set; }

        [JsonProperty("father_office_address")]
        public string FatherOfficeAddress { get; set; }

        [JsonProperty("father_position")]
        public string FatherPosition { get; set; }

        [JsonProperty("father_how_long")]
        public string FatherHowLong { get; set; }

        [JsonProperty("mother_name")]
        public string MotherMaidenName { get; set; }

        [JsonProperty("mother_age")]
        public int? MotherAge { get; set; }

        [JsonProperty("mother_address")]
        public string MotherAddress { get; set; }

        [JsonProperty("mother_facebook")]
        public string MotherFacebook { get; set; }

        [JsonProperty("mother_income_source")]
        public string MotherIncomeSource { get; set; }

        [JsonProperty("mother_office_address")]
        public string MotherOfficeAddress { get; set; }

        [JsonProperty("mother_position")]
        public string MotherPosition { get; set; }

        [JsonProperty("mother_how_long")]
        public string MotherHowLong { get; set; }

        [JsonProperty("spouse_name")]
        public string SpouseName { get; set; }

        [JsonProperty("spouse_age")]
        public int? SpouseAge { get; set; }

        [JsonProperty("spouse_income_source")]
        public string SpouseIncomeSource { get; set; }

        [JsonProperty("spouse_office_address")]
        public string SpouseOfficeAddress { get; set; }

        [JsonProperty("spouse_position")]
        public string SpousePosition { get; set; }

        [JsonProperty("spouse_how_long")]
        public string SpouseHowLong { get; set; }

        [JsonProperty("spouse_tel_no")]
        public string SpouseTelNo { get; set; }

        [JsonProperty("spouse_salary")]
        public string SpouseSalary { get; set; }

        [JsonProperty("father_in_law_name")]
        public string FatherInLawName { get; set; }

        [JsonProperty("father_in_law_age")]
        public int? FatherInLawAge { get; set; }

        [JsonProperty("father_in_law_address")]
        public string FatherInLawAddress { get; set; }

        [JsonProperty("father_in_law_facebook")]
        public string FatherInLawFacebook { get; set; }

        [JsonProperty("father_in_law_income_source")]
        public string FatherInLawIncomeSource { get; set; }

        [JsonProperty("father_in_law_office_address")]
        public string FatherInLawOfficeAddress { get; set; }

        [JsonProperty("father_in_law_position")]
        public string FatherInLawPosition { get; set; }

        [JsonProperty("father_in_law_how_long")]
        public string FatherInLawHowLong { get; set; }

        [JsonProperty("mother_in_law_name")]
        public string MotherInLawName { get; set; }

        [JsonProperty("mother_in_law_age")]
        public int? MotherInLawAge { get; set; }

        [JsonProperty("mother_in_law_address")]
        public string MotherInLawAddress { get; set; }

        [JsonProperty("mother_in_law_facebook")]
        public string MotherInLawFacebook { get; set; }

        [JsonProperty("mother_in_law_income_source")]
        public string MotherInLawIncomeSource { get; set; }

        [JsonProperty("mother_in_law_office_address")]
        public string MotherInLawOfficeAddress { get; set; }

        [JsonProperty("mother_in_law_position")]
        public string MotherInLawPosition { get; set; }

        [JsonProperty("mother_in_law_how_long")]
        public string MotherInLawHowLong { get; set; }

        [JsonProperty("desired_tc_terms")]
        public string DesiredTCTerms { get; set; }

        [JsonProperty("desired_tc_down_payment")]
        public string DesiredTCDownPayment { get; set; }

        [JsonProperty("desired_tc_monthly_installment")]
        public string DesiredTCMonthlyInstallment { get; set; }

        [JsonProperty("desired_tc_total_price")]
        public string DesiredTCTotalPrice { get; set; }

        [JsonProperty("desired_tc_total_rebate")]
        public string DesiredTCTotalRebate { get; set; }

        [JsonProperty("desired_tc_remarks")]
        public string DesiredTCRemarks { get; set; }

        [JsonProperty("is_agreed")]
        public bool IsAgreed { get; set; }

        [JsonProperty("confirmation_officer")]
        public string ConfirmationOfficer { get; set; }

        [JsonProperty("confirmation_date")]
        public DateTime? ConfirmationDate { get; set; }

        [JsonProperty("confirmation_time")]
        public TimeSpan? ConfirmationTime { get; set; }

        [JsonProperty("office_use_ca_name")]
        public string OfficeUseCAName { get; set; }

        [JsonProperty("office_use_ca_remarks")]
        public string OfficeUseCARemarks { get; set; }

        [JsonProperty("office_use_ca_date")]
        public DateTime? OfficeUseCADate { get; set; }

        [JsonProperty("office_use_ca_time")]
        public TimeSpan? OfficeUseCATime { get; set; }

        [JsonProperty("office_use_ccs_name")]
        public string OfficeUseCCSName { get; set; }

        [JsonProperty("office_use_ccs_remarks")]
        public string OfficeUseCCSRemarks { get; set; }

        [JsonProperty("office_use_ccs_date")]
        public DateTime? OfficeUseCCSDate { get; set; }

        [JsonProperty("office_use_ccs_time")]
        public TimeSpan? OfficeUseCCSTime { get; set; }

        [JsonProperty("office_use_invoice_no")]
        public string OfficeUseInvoiceNo { get; set; }

        [JsonProperty("office_use_invoice_date")]
        public DateTime? OfficeUseInvoiceDate { get; set; }

        [JsonProperty("office_use_or_no")]
        public string OfficeUseORNo { get; set; }

        [JsonProperty("office_use_or_date")]
        public DateTime? OfficeUseORDate { get; set; }

        [JsonProperty("office_use_amount")]
        public string OfficeUseAmount { get; set; }

        [JsonProperty("office_use_cashier")]
        public string OfficeUseCashier { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty("updated_date")]
        public DateTime? UpdatedDate { get; set; }

        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("approved_date")]
        public DateTime? ApprovedDate { get; set; }

        [JsonProperty("approved_by")]
        public int? ApprovedBy { get; set; }


        [JsonProperty("created_by_name")]
        public string CreatedByName { get; set; }

        [JsonProperty("updated_by_name")]
        public string UpdatedByName { get; set; }

        [JsonProperty("approved_by_name")]
        public string ApprovedByName { get; set; }



        //SAVE TO SERVER
        [JsonProperty("list_childrens")]
        public List<LoanCustomerChildrenViewModel> Childrens { get; set; }

        [JsonProperty("list_credit_histories")]
        public List<LoanCreditHistoryViewModel> CreditHistories { get; set; }

        [JsonProperty("list_personal_properties")]
        public List<LoanPersonalPropertyViewModel> PersonalProperties { get; set; }

        [JsonProperty("list_unit_desireds")]
        public List<LoanUnitDesiredViewModel> UnitDesireds { get; set; }

        [JsonProperty("list_unit_desired_tcs")]
        public List<LoanUnitDesiredTCViewModel> UnitDesiredTCs { get; set; }

        [JsonProperty("list_removed_attachments")]
        public List<string> RemovedAttachments { get; set; }

        [JsonProperty("list_attachments")]
        public List<LoanFileViewModel> LoanAttachments { get; set; }

        [JsonProperty("signature")]
        public LoanFileViewModel Signature { get; set; }

        //GET FROM SERVER
        [JsonProperty("loan_customer_children")]
        public List<LoanCustomerChildren> LoanCustomerChildren { get; set; }

        [JsonProperty("loan_credit_history")]
        public List<LoanCreditHistory> LoanCreditHistory { get; set; }

        [JsonProperty("loan_personal_property")]
        public List<LoanPersonalProperty> LoanPersonalProperty { get; set; }

        [JsonProperty("loan_unit_desired")]
        public List<LoanUnitDesired> LoanUnitDesired { get; set; }

        [JsonProperty("loan_unit_desiredtc")]
        public List<LoanUnitDesiredTC> LoanUnitDesiredTC { get; set; }

        [JsonProperty("loan_attachment")]
        public List<LoanAttachment> LoanAttachment { get; set; }

        [JsonProperty("client_signature")]
        public string ClientSignature { get; set; }
    }
}
