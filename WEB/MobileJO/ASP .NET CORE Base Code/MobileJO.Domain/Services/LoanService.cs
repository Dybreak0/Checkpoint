using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using MobileJO.Data;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.LoanApplication;
using MobileJO.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MobileJO.Domain.Services
{
    public class LoanService: ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IUserService _userService;

        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        ///     Constructor for ILoanRepository and mapper
        /// </summary>
        /// <param name="LoanRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="emailJOService"></param>
        /// <param name="userService"></param>
        public LoanService(ILoanRepository loanRepository,
                               IMapper mapper,
                               IUserService userService,
                               IHostingEnvironment hostingEnvironment)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _userService = userService;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Calls Loan Repository methods for saving loan application, list of children,credit history, personal property, unit desired, unit desired terms and conditions and attachments
        /// </summary>
        /// <param name="loanViewModel"></param>
        /// <returns></returns>
        public int Create(LoanDetailsViewModel loanDetailsViewModel, string attachmentPath)
        {
            // ================== Check first if all files passed are valid ================== //
            if (IsValidLoanFiles(loanDetailsViewModel.Signature,
                             loanDetailsViewModel.LoanAttachments) == false)
            {
                return 0;
            }

            Loan loan = new Loan
            {
                ApplicationNumber = loanDetailsViewModel.ApplicationNumber,
                BranchID = loanDetailsViewModel.BranchID,
                LoanStatus = loanDetailsViewModel.LoanStatus,
                Name = loanDetailsViewModel.Name,
                BirthDate = loanDetailsViewModel.BirthDate,
                Age = loanDetailsViewModel.Age,
                RegionID = loanDetailsViewModel.RegionID,
                CityID = loanDetailsViewModel.CityID,
                ZipCode = loanDetailsViewModel.ZipCode,
                HouseUnitBuildingNo = loanDetailsViewModel.HouseUnitBuildingNo,
                StreetBarangay = loanDetailsViewModel.StreetBarangay,
                Landmark = loanDetailsViewModel.Landmark,
                BirthPlace = loanDetailsViewModel.BirthPlace,
                PreviousAddress = loanDetailsViewModel.PreviousAddress,
                PhoneNo = loanDetailsViewModel.PhoneNo,
                TelNo = loanDetailsViewModel.TelNo,
                EmailAddress = loanDetailsViewModel.EmailAddress,
                Facebook = loanDetailsViewModel.Facebook,
                StabilityOfResidence = loanDetailsViewModel.StabilityOfResidence,
                RentingName = loanDetailsViewModel.RentingName,
                RentingAddress = loanDetailsViewModel.RentingAddress,
                RentingTelNo = loanDetailsViewModel.RentingTelNo,
                MaritalStatus = loanDetailsViewModel.MaritalStatus,
                Land = loanDetailsViewModel.Land,
                HouseMade = loanDetailsViewModel.HouseMade,
                Industry = loanDetailsViewModel.Industry,
                TypeOf = loanDetailsViewModel.TypeOf,
                EmployedWhere = loanDetailsViewModel.EmployedWhere,
                EmployedHowLong = loanDetailsViewModel.EmployedHowLong,
                EmployedPosition = loanDetailsViewModel.EmployedPosition,
                EmployedPresentBusinessAddress = loanDetailsViewModel.EmployedPresentBusinessAddress,
                EmployedTelNo = loanDetailsViewModel.EmployedTelNo,
                EmployedSalary = loanDetailsViewModel.EmployedSalary,
                EmployedPreviousEmployment = loanDetailsViewModel.EmployedPreviousEmployment,
                EmployedPreviousBusinessAddress = loanDetailsViewModel.EmployedPreviousBusinessAddress,

                BusinessNature = loanDetailsViewModel.BusinessNature,
                BusinessName = loanDetailsViewModel.BusinessName,
                BusinessAddress = loanDetailsViewModel.BusinessAddress,
                BusinessCapital = loanDetailsViewModel.BusinessCapital,
                BusinessMonthlyIncomeNet = loanDetailsViewModel.BusinessMonthlyIncomeNet,
                BusinessMonthlyIncomeGross = loanDetailsViewModel.BusinessMonthlyIncomeGross,
                BusinessPhoneNo = loanDetailsViewModel.BusinessPhoneNo,
                BusinessHowLong = loanDetailsViewModel.BusinessHowLong,

                PensionAgency = loanDetailsViewModel.PensionAgency,
                PensionMonthly = loanDetailsViewModel.PensionMonthly,

                RemittanceName = loanDetailsViewModel.RemittanceName,
                RemittanceLocation = loanDetailsViewModel.RemittanceLocation,
                RemittanceRelationship = loanDetailsViewModel.RemittanceRelationship,
                RemittanceMonthlyAmount = loanDetailsViewModel.RemittanceMonthlyAmount,
                RemittanceFrequency = loanDetailsViewModel.RemittanceFrequency,

                FatherName = loanDetailsViewModel.FatherName,
                FatherAge = loanDetailsViewModel.FatherAge,
                FatherAddress = loanDetailsViewModel.FatherAddress,
                FatherFacebook = loanDetailsViewModel.FatherFacebook,
                FatherIncomeSource = loanDetailsViewModel.FatherIncomeSource,
                FatherOfficeAddress = loanDetailsViewModel.FatherOfficeAddress,
                FatherPosition = loanDetailsViewModel.FatherPosition,
                FatherHowLong = loanDetailsViewModel.FatherHowLong,

                MotherMaidenName = loanDetailsViewModel.MotherMaidenName,
                MotherAge = loanDetailsViewModel.MotherAge,
                MotherAddress = loanDetailsViewModel.MotherAddress,
                MotherFacebook = loanDetailsViewModel.MotherFacebook,
                MotherIncomeSource = loanDetailsViewModel.MotherIncomeSource,
                MotherOfficeAddress = loanDetailsViewModel.MotherOfficeAddress,
                MotherPosition = loanDetailsViewModel.MotherPosition,
                MotherHowLong = loanDetailsViewModel.MotherHowLong,

                SpouseName = loanDetailsViewModel.SpouseName,
                SpouseAge = loanDetailsViewModel.SpouseAge,
                SpouseIncomeSource = loanDetailsViewModel.SpouseIncomeSource,
                SpouseOfficeAddress = loanDetailsViewModel.SpouseOfficeAddress,
                SpousePosition = loanDetailsViewModel.SpousePosition,
                SpouseHowLong = loanDetailsViewModel.SpouseHowLong,
                SpouseTelNo = loanDetailsViewModel.SpouseTelNo,
                SpouseSalary = loanDetailsViewModel.SpouseSalary,

                FatherInLawName = loanDetailsViewModel.FatherInLawName,
                FatherInLawAge = loanDetailsViewModel.FatherInLawAge,
                FatherInLawAddress = loanDetailsViewModel.FatherInLawAddress,
                FatherInLawFacebook = loanDetailsViewModel.FatherInLawFacebook,
                FatherInLawIncomeSource = loanDetailsViewModel.FatherInLawIncomeSource,
                FatherInLawOfficeAddress = loanDetailsViewModel.FatherInLawOfficeAddress,
                FatherInLawPosition = loanDetailsViewModel.FatherInLawPosition,
                FatherInLawHowLong = loanDetailsViewModel.FatherInLawHowLong,

                MotherInLawName = loanDetailsViewModel.MotherInLawName,
                MotherInLawAge = loanDetailsViewModel.MotherInLawAge,
                MotherInLawAddress = loanDetailsViewModel.MotherInLawAddress,
                MotherInLawFacebook = loanDetailsViewModel.MotherInLawFacebook,
                MotherInLawIncomeSource = loanDetailsViewModel.MotherInLawIncomeSource,
                MotherInLawOfficeAddress = loanDetailsViewModel.MotherInLawOfficeAddress,
                MotherInLawPosition = loanDetailsViewModel.MotherInLawPosition,
                MotherInLawHowLong = loanDetailsViewModel.MotherInLawHowLong,

                DesiredTCTerms = loanDetailsViewModel.DesiredTCTerms,
                DesiredTCDownPayment = loanDetailsViewModel.DesiredTCDownPayment,
                DesiredTCMonthlyInstallment = loanDetailsViewModel.DesiredTCMonthlyInstallment,
                DesiredTCTotalPrice = loanDetailsViewModel.DesiredTCTotalPrice,
                DesiredTCTotalRebate = loanDetailsViewModel.DesiredTCTotalRebate,
                DesiredTCRemarks = loanDetailsViewModel.DesiredTCRemarks,

                IsAgreed = loanDetailsViewModel.IsAgreed,
                ConfirmationOfficer = loanDetailsViewModel.ConfirmationOfficer,
                ConfirmationDate = loanDetailsViewModel.ConfirmationDate,
                ConfirmationTime = loanDetailsViewModel.ConfirmationTime,

                OfficeUseCAName = loanDetailsViewModel.OfficeUseCAName,
                OfficeUseCARemarks = loanDetailsViewModel.OfficeUseCARemarks,
                OfficeUseCADate = loanDetailsViewModel.OfficeUseCADate,
                OfficeUseCATime = loanDetailsViewModel.OfficeUseCATime,

                OfficeUseCCSName = loanDetailsViewModel.OfficeUseCCSName,
                OfficeUseCCSRemarks = loanDetailsViewModel.OfficeUseCCSRemarks,
                OfficeUseCCSDate = loanDetailsViewModel.OfficeUseCCSDate,
                OfficeUseCCSTime = loanDetailsViewModel.OfficeUseCCSTime,
                OfficeUseInvoiceNo = loanDetailsViewModel.OfficeUseInvoiceNo,
                OfficeUseInvoiceDate = loanDetailsViewModel.OfficeUseInvoiceDate,
                OfficeUseORNo = loanDetailsViewModel.OfficeUseORNo,
                OfficeUseORDate = loanDetailsViewModel.OfficeUseORDate,
                OfficeUseAmount = loanDetailsViewModel.OfficeUseAmount,
                OfficeUseCashier = loanDetailsViewModel.OfficeUseCashier,


                CreatedDate = loanDetailsViewModel.CreatedDate,
                CreatedBy = loanDetailsViewModel.CreatedBy,
            };

            var childrens = new List<LoanCustomerChildren>();
            var creditHistories = new List<LoanCreditHistory>();
            var personalProperties = new List<LoanPersonalProperty>();
            var unitDesireds = new List<LoanUnitDesired>();
            //var unitDesiredTCs = new List<LoanUnitDesiredTC>();
            var attachments = new List<LoanAttachment>();
            var signatureFilename = loanDetailsViewModel.Signature != null &&
                                    loanDetailsViewModel.Signature.FileDataArray != null ?
                                    Constants.Common.SignatureNameExtension :
                                    null;
            foreach (LoanCustomerChildrenViewModel child in loanDetailsViewModel.Childrens)
            {
                childrens.Add(new LoanCustomerChildren
                {
                    ChildName = child.ChildName,
                    ChildAge = child.ChildAge,
                    ChildHomeAddress = child.ChildHomeAddress,
                    ChildTelNo = child.ChildTelNo,
                    ChildEmploySchool = child.ChildEmploySchool,
                    ChildEmploySchoolAddress = child.ChildEmploySchoolAddress,
                    ChildPosGrade = child.ChildPosGrade,
                    ChildHowLong = child.ChildHowLong,
                });
            }

            foreach (LoanCreditHistoryViewModel history in loanDetailsViewModel.CreditHistories)
            {
                creditHistories.Add(new LoanCreditHistory
                {
                    HistoryCompanyName = history.HistoryCompanyName,
                    HistoryTypeOfUnit = history.HistoryTypeOfUnit,
                    HistoryDatePurchase = history.HistoryDatePurchase,
                    HistoryTerms = history.HistoryTerms,
                    HistoryRemainingBalance = history.HistoryRemainingBalance,
                });
            }
            foreach (LoanPersonalPropertyViewModel property in loanDetailsViewModel.PersonalProperties)
            {
                personalProperties.Add(new LoanPersonalProperty
                {
                    Property = property.Property,
                });
            }
            foreach (LoanUnitDesiredViewModel unit in loanDetailsViewModel.UnitDesireds)
            {
                unitDesireds.Add(new LoanUnitDesired
                {
                    DesiredBrandModel = unit.DesiredBrandModel,
                    DesiredSerialNo = unit.DesiredSerialNo,
                    DesiredCode = unit.DesiredCode,
                    DesiredAmount = unit.DesiredAmount,
                    DesiredAccounting = unit.DesiredAccounting,
                });
            }
            //foreach (LoanUnitDesiredTCViewModel unitTC in loanDetailsViewModel.UnitDesiredTCs)
            //{
            //    unitDesiredTCs.Add(new LoanUnitDesiredTC
            //    {
            //        DesiredTCBrandModel = unitTC.DesiredTCBrandModel,
            //        DesiredTCTerms = unitTC.DesiredTCTerms,
            //        DesiredTCDownPayment = unitTC.DesiredTCDownPayment,
            //        DesiredTCMonthlyInstallment = unitTC.DesiredTCMonthlyInstallment,
            //        DesiredTCTotalPrice = unitTC.DesiredTCTotalPrice,
            //        DesiredTCTotalRebate = unitTC.DesiredTCTotalRebate,
            //        DesiredTCRemarks = unitTC.DesiredTCRemarks,
            //    });
            //}
            foreach (LoanFileViewModel file in loanDetailsViewModel.LoanAttachments)
            {
                attachments.Add(new LoanAttachment { FileName = file.FileName });
            }

            int loanID = _loanRepository.Create(loan,
                                                    creditHistories,
                                                    childrens,
                                                    personalProperties,
                                                    unitDesireds,
                                                    attachments,
                                                    signatureFilename);

            if (loanID > 0)
            {
                if (loanDetailsViewModel.Signature != null &&
                    loanDetailsViewModel.Signature.FileDataArray != null)
                {
                    string filename = string.Concat(loanID, Constants.Common.SignatureNameExtension);

                    loanDetailsViewModel.Signature.FileName = filename;

                    SaveFiles(loanID,
                                new List<LoanFileViewModel> { loanDetailsViewModel.Signature },
                                attachmentPath,
                                Constants.Upload.ClientSignature);
                }

                if (loanDetailsViewModel.LoanAttachments.Count > 0)
                {
                    SaveFiles(loanID,
                               loanDetailsViewModel.LoanAttachments,
                               attachmentPath,
                               Constants.Upload.Attachment);
                }
            }

            return loanID;
        }
        
        /// <summary>
        /// Calls Helper method IsValidFile() to check if files uploaded are valid files
        /// </summary>
        /// <param name="signatureFile"></param>
        /// <param name="attachmentsFiles"></param>
        /// <returns></returns>
        public bool IsValidLoanFiles(LoanFileViewModel signatureFile, List<LoanFileViewModel> attachmentsFiles)
        {
            var filesToCheck = new List<LoanFileViewModel>();

            if (signatureFile != null &&
                signatureFile.FileDataArray != null) 
            { 
                filesToCheck.Add(signatureFile); 
            }

            if(attachmentsFiles != null)
            {
                if (attachmentsFiles.Count > 0)
                {
                    foreach (var attachment in attachmentsFiles)
                    {
                        filesToCheck.Add(attachment);
                    }
                }
            }
            

            if (Helper.IsValidLoanFile(filesToCheck) == false) 
            { 
                return false; 
            }

            return true;
        }

        /// <summary>
        /// Calls Helper method SaveFileToServer() to save signature to files
        /// </summary>
        /// <param name="loanID"></param>
        /// <param name="fileModelList"></param>
        /// <param name="attachmentPath"></param>
        /// <param name="targetFolder"></param>
        /// <returns></returns>
        public bool SaveFiles(int loanID,
                              List<LoanFileViewModel> fileModelList,
                              string attachmentPath,
                              string attachmentType)
        {
            bool isSaved = false;

            var rootPath = _hostingEnvironment.ContentRootPath;
            var filePath = string.Format(Constants.Attachment.FullFilePath, rootPath, attachmentPath, loanID, attachmentType);

            if (!Helper.DoesPathExist(filePath))    
            {
                Helper.CreateDirectory(filePath);
            }

            isSaved = Helper.SaveLoanFileToServer(fileModelList, filePath);

            return isSaved;
        }

        public ListViewModel ListLoanApplication(LoanSearchViewModel searchListViewModel)
        {
            return _loanRepository.ListLoanApplication(searchListViewModel);
        }

        public ListViewModel PendingLoanApplication(LoanSearchPendingViewModel searchPendingViewModel)
        {
            return _loanRepository.PendingLoanApplication(searchPendingViewModel);
        }

        public LoanDetailsViewModel FindLoanApplication(int id)
        {
            LoanDetailsViewModel loanViewModel = null;
            var loanApplication = _loanRepository.FindLoanApplication(id);

            if (loanApplication != null)
            {
                loanViewModel = _mapper.Map<LoanDetailsViewModel>(loanApplication);
            }

            return loanViewModel;
        }

        /// <summary>
        ///     Used to generate an excel format of retrieved Loan Applications records
        /// </summary>
        /// <param name="searchModel">Holds the Loan Applications search filters</param>
        /// <returns name="loanApplication">Holds the excel formatted of Loan Applications records</returns>
        public HttpResponseMessage DownloadLoanApplication(LoanSearchViewModel searchModel)
        {
            var loanApplicationList = _loanRepository.DownloadLoanApplication(searchModel);
            var loanApplication = new HttpResponseMessage();

            if (loanApplicationList != null && loanApplicationList.Count > 0)
            {
                var excelTable = new StringBuilder();
                var rows = new StringBuilder();

                foreach (var loan in loanApplicationList)
                {
                    rows.Append(String.Format(Constants.Loans.LoanApplicationExcelTableRows,
                                              loan.ApplicationNumber,
                                              loan.Name,
                                              loan.CreatedDate.ToShortDateString(),
                                              loan.CreatedByName,
                                              loan.LoanStatus));
                }

                excelTable.Append(String.Format(Constants.Loans.ExcelTable, Constants.Loans.ExcelTableHeaders, rows));
                loanApplication = Helper.LoanExportToExcel(excelTable.ToString(),
                    String.Format(Constants.Loans.InitialExcelFilename, Constants.Loans.LoanApplications));
            }
            else
            {
                loanApplication = Helper.ComposeResponse(HttpStatusCode.NoContent, Constants.Common.NoRecordsFound);
            }

            return loanApplication;
        }

        /// <summary>
        /// Calls Job Order Repository method Find() to check if job order exists
        /// </summary>
        /// <param name="loanID"></param>
        /// <returns></returns>
        public bool IsLoanApplicationExists(int loanID)
        {
            if (_loanRepository.FindLoan(loanID) != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calls Job Order Repository methods for updating job orders, tagged cases, billing types and attachments
        /// </summary>
        /// <param name="loanDetailsViewModel"></param>
        public bool Update(LoanDetailsViewModel loanDetailsViewModel, string attachmentPath)
        {
            if (IsValidLoanFiles(loanDetailsViewModel.Signature,
                             loanDetailsViewModel.LoanAttachments) == false)
            {
                return false;
            }

            int id = loanDetailsViewModel.LoanID;
            string signatureName = null;

            if (!string.IsNullOrEmpty(loanDetailsViewModel.ClientSignature))
            {
                signatureName = loanDetailsViewModel.ClientSignature;
            }
            else if (loanDetailsViewModel.Signature != null &&
                    !string.IsNullOrEmpty(loanDetailsViewModel.Signature.FileName) &&
                    string.IsNullOrEmpty(loanDetailsViewModel.ClientSignature))
            {
                signatureName = string.Concat(id, loanDetailsViewModel.Signature.FileName);
            }

            Loan loanForUpdate = new Loan();
            loanForUpdate.LoanID = id;
            loanForUpdate.BranchID = loanDetailsViewModel.BranchID;
            loanForUpdate.Name = loanDetailsViewModel.Name;
            loanForUpdate.BirthDate = loanDetailsViewModel.BirthDate;
            loanForUpdate.Age = loanDetailsViewModel.Age;
            loanForUpdate.RegionID = loanDetailsViewModel.RegionID;
            loanForUpdate.CityID = loanDetailsViewModel.CityID;
            loanForUpdate.ZipCode = loanDetailsViewModel.ZipCode;
            loanForUpdate.HouseUnitBuildingNo = loanDetailsViewModel.HouseUnitBuildingNo;
            loanForUpdate.StreetBarangay = loanDetailsViewModel.StreetBarangay;
            loanForUpdate.Landmark = loanDetailsViewModel.Landmark;
            loanForUpdate.BirthPlace = loanDetailsViewModel.BirthPlace;
            loanForUpdate.PreviousAddress = loanDetailsViewModel.PreviousAddress;
            loanForUpdate.PhoneNo = loanDetailsViewModel.PhoneNo;
            loanForUpdate.TelNo = loanDetailsViewModel.TelNo;
            loanForUpdate.EmailAddress = loanDetailsViewModel.EmailAddress;
            loanForUpdate.Facebook = loanDetailsViewModel.Facebook;
            loanForUpdate.StabilityOfResidence = loanDetailsViewModel.StabilityOfResidence;
            loanForUpdate.RentingName = loanDetailsViewModel.RentingName;
            loanForUpdate.RentingAddress = loanDetailsViewModel.RentingAddress;
            loanForUpdate.RentingTelNo = loanDetailsViewModel.RentingTelNo;
            loanForUpdate.MaritalStatus = loanDetailsViewModel.MaritalStatus;
            loanForUpdate.Land = loanDetailsViewModel.Land;
            loanForUpdate.HouseMade = loanDetailsViewModel.HouseMade;
            loanForUpdate.Industry = loanDetailsViewModel.Industry;
            loanForUpdate.TypeOf = loanDetailsViewModel.TypeOf;
            loanForUpdate.EmployedWhere = loanDetailsViewModel.EmployedWhere;
            loanForUpdate.EmployedHowLong = loanDetailsViewModel.EmployedHowLong;
            loanForUpdate.EmployedPosition = loanDetailsViewModel.EmployedPosition;
            loanForUpdate.EmployedPresentBusinessAddress = loanDetailsViewModel.EmployedPresentBusinessAddress;
            loanForUpdate.EmployedTelNo = loanDetailsViewModel.EmployedTelNo;
            loanForUpdate.EmployedSalary = loanDetailsViewModel.EmployedSalary;
            loanForUpdate.EmployedPreviousEmployment = loanDetailsViewModel.EmployedPreviousEmployment;
            loanForUpdate.EmployedPreviousBusinessAddress = loanDetailsViewModel.EmployedPreviousBusinessAddress;

            loanForUpdate.BusinessNature = loanDetailsViewModel.BusinessNature;
            loanForUpdate.BusinessName = loanDetailsViewModel.BusinessName;
            loanForUpdate.BusinessAddress = loanDetailsViewModel.BusinessAddress;
            loanForUpdate.BusinessCapital = loanDetailsViewModel.BusinessCapital;
            loanForUpdate.BusinessMonthlyIncomeNet = loanDetailsViewModel.BusinessMonthlyIncomeNet;
            loanForUpdate.BusinessMonthlyIncomeGross = loanDetailsViewModel.BusinessMonthlyIncomeGross;
            loanForUpdate.BusinessPhoneNo = loanDetailsViewModel.BusinessPhoneNo;
            loanForUpdate.BusinessHowLong = loanDetailsViewModel.BusinessHowLong;

            loanForUpdate.PensionAgency = loanDetailsViewModel.PensionAgency;
            loanForUpdate.PensionMonthly = loanDetailsViewModel.PensionMonthly;

            loanForUpdate.RemittanceName = loanDetailsViewModel.RemittanceName;
            loanForUpdate.RemittanceLocation = loanDetailsViewModel.RemittanceLocation;
            loanForUpdate.RemittanceRelationship = loanDetailsViewModel.RemittanceRelationship;
            loanForUpdate.RemittanceMonthlyAmount = loanDetailsViewModel.RemittanceMonthlyAmount;
            loanForUpdate.RemittanceFrequency = loanDetailsViewModel.RemittanceFrequency;

            loanForUpdate.FatherName = loanDetailsViewModel.FatherName;
            loanForUpdate.FatherAge = loanDetailsViewModel.FatherAge;
            loanForUpdate.FatherAddress = loanDetailsViewModel.FatherAddress;
            loanForUpdate.FatherFacebook = loanDetailsViewModel.FatherFacebook;
            loanForUpdate.FatherIncomeSource = loanDetailsViewModel.FatherIncomeSource;
            loanForUpdate.FatherOfficeAddress = loanDetailsViewModel.FatherOfficeAddress;
            loanForUpdate.FatherPosition = loanDetailsViewModel.FatherPosition;
            loanForUpdate.FatherHowLong = loanDetailsViewModel.FatherHowLong;

            loanForUpdate.MotherMaidenName = loanDetailsViewModel.MotherMaidenName;
            loanForUpdate.MotherAge = loanDetailsViewModel.MotherAge;
            loanForUpdate.MotherAddress = loanDetailsViewModel.MotherAddress;
            loanForUpdate.MotherFacebook = loanDetailsViewModel.MotherFacebook;
            loanForUpdate.MotherIncomeSource = loanDetailsViewModel.MotherIncomeSource;
            loanForUpdate.MotherOfficeAddress = loanDetailsViewModel.MotherOfficeAddress;
            loanForUpdate.MotherPosition = loanDetailsViewModel.MotherPosition;
            loanForUpdate.MotherHowLong = loanDetailsViewModel.MotherHowLong;

            loanForUpdate.SpouseName = loanDetailsViewModel.SpouseName;
            loanForUpdate.SpouseAge = loanDetailsViewModel.SpouseAge;
            loanForUpdate.SpouseIncomeSource = loanDetailsViewModel.SpouseIncomeSource;
            loanForUpdate.SpouseOfficeAddress = loanDetailsViewModel.SpouseOfficeAddress;
            loanForUpdate.SpousePosition = loanDetailsViewModel.SpousePosition;
            loanForUpdate.SpouseHowLong = loanDetailsViewModel.SpouseHowLong;
            loanForUpdate.SpouseTelNo = loanDetailsViewModel.SpouseTelNo;
            loanForUpdate.SpouseSalary = loanDetailsViewModel.SpouseSalary;

            loanForUpdate.FatherInLawName = loanDetailsViewModel.FatherInLawName;
            loanForUpdate.FatherInLawAge = loanDetailsViewModel.FatherInLawAge;
            loanForUpdate.FatherInLawAddress = loanDetailsViewModel.FatherInLawAddress;
            loanForUpdate.FatherInLawFacebook = loanDetailsViewModel.FatherInLawFacebook;
            loanForUpdate.FatherInLawIncomeSource = loanDetailsViewModel.FatherInLawIncomeSource;
            loanForUpdate.FatherInLawOfficeAddress = loanDetailsViewModel.FatherInLawOfficeAddress;
            loanForUpdate.FatherInLawPosition = loanDetailsViewModel.FatherInLawPosition;
            loanForUpdate.FatherInLawHowLong = loanDetailsViewModel.FatherInLawHowLong;

            loanForUpdate.MotherInLawName = loanDetailsViewModel.MotherInLawName;
            loanForUpdate.MotherInLawAge = loanDetailsViewModel.MotherInLawAge;
            loanForUpdate.MotherInLawAddress = loanDetailsViewModel.MotherInLawAddress;
            loanForUpdate.MotherInLawFacebook = loanDetailsViewModel.MotherInLawFacebook;
            loanForUpdate.MotherInLawIncomeSource = loanDetailsViewModel.MotherInLawIncomeSource;
            loanForUpdate.MotherInLawOfficeAddress = loanDetailsViewModel.MotherInLawOfficeAddress;
            loanForUpdate.MotherInLawPosition = loanDetailsViewModel.MotherInLawPosition;
            loanForUpdate.MotherInLawHowLong = loanDetailsViewModel.MotherInLawHowLong;

            loanForUpdate.DesiredTCTerms = loanDetailsViewModel.DesiredTCTerms;
            loanForUpdate.DesiredTCDownPayment = loanDetailsViewModel.DesiredTCDownPayment;
            loanForUpdate.DesiredTCMonthlyInstallment = loanDetailsViewModel.DesiredTCMonthlyInstallment;
            loanForUpdate.DesiredTCTotalPrice = loanDetailsViewModel.DesiredTCTotalPrice;
            loanForUpdate.DesiredTCTotalRebate = loanDetailsViewModel.DesiredTCTotalRebate;
            loanForUpdate.DesiredTCRemarks = loanDetailsViewModel.DesiredTCRemarks;

            loanForUpdate.ConfirmationOfficer = loanDetailsViewModel.ConfirmationOfficer;
            loanForUpdate.ConfirmationDate = loanDetailsViewModel.ConfirmationDate;
            loanForUpdate.ConfirmationTime = loanDetailsViewModel.ConfirmationTime;

            loanForUpdate.OfficeUseCAName = loanDetailsViewModel.OfficeUseCAName;
            loanForUpdate.OfficeUseCARemarks = loanDetailsViewModel.OfficeUseCARemarks;
            loanForUpdate.OfficeUseCADate = loanDetailsViewModel.OfficeUseCADate;
            loanForUpdate.OfficeUseCATime = loanDetailsViewModel.OfficeUseCATime;

            loanForUpdate.OfficeUseCCSName = loanDetailsViewModel.OfficeUseCCSName;
            loanForUpdate.OfficeUseCCSRemarks = loanDetailsViewModel.OfficeUseCCSRemarks;
            loanForUpdate.OfficeUseCCSDate = loanDetailsViewModel.OfficeUseCCSDate;
            loanForUpdate.OfficeUseCCSTime = loanDetailsViewModel.OfficeUseCCSTime;
            loanForUpdate.OfficeUseInvoiceNo = loanDetailsViewModel.OfficeUseInvoiceNo;
            loanForUpdate.OfficeUseInvoiceDate = loanDetailsViewModel.OfficeUseInvoiceDate;
            loanForUpdate.OfficeUseORNo = loanDetailsViewModel.OfficeUseORNo;
            loanForUpdate.OfficeUseORDate = loanDetailsViewModel.OfficeUseORDate;
            loanForUpdate.OfficeUseAmount = loanDetailsViewModel.OfficeUseAmount;
            loanForUpdate.OfficeUseCashier = loanDetailsViewModel.OfficeUseCashier;

            loanForUpdate.ClientSignature = signatureName;

            loanForUpdate.UpdatedDate = loanDetailsViewModel.UpdatedDate;
            loanForUpdate.UpdatedBy = loanDetailsViewModel.UpdatedBy;

            List<LoanCustomerChildren> newCustomerChildrens = new List<LoanCustomerChildren>();
            List<LoanCreditHistory> newCreditHistories = new List<LoanCreditHistory>();
            List<LoanPersonalProperty> newPersonalProperties = new List<LoanPersonalProperty>();
            List<LoanUnitDesired> newUnitDesireds = new List<LoanUnitDesired>();
            //List<LoanUnitDesiredTC> newUnitDesiredTCs = new List<LoanUnitDesiredTC>();
            List<LoanAttachment> newAttachments = new List<LoanAttachment>();

            foreach (LoanCustomerChildrenViewModel child in loanDetailsViewModel.Childrens)
            {
                newCustomerChildrens.Add(new LoanCustomerChildren
                {
                    LoanID = id,
                    ChildName = child.ChildName,
                    ChildAge = child.ChildAge,
                    ChildHomeAddress = child.ChildHomeAddress,
                    ChildTelNo = child.ChildTelNo,
                    ChildEmploySchool = child.ChildEmploySchool,
                    ChildEmploySchoolAddress = child.ChildEmploySchoolAddress,
                    ChildPosGrade = child.ChildPosGrade,
                    ChildHowLong = child.ChildHowLong,
                });
            }

            foreach (LoanCreditHistoryViewModel history in loanDetailsViewModel.CreditHistories)
            {
                newCreditHistories.Add(new LoanCreditHistory
                {
                    LoanID = id,
                    HistoryCompanyName = history.HistoryCompanyName,
                    HistoryTypeOfUnit = history.HistoryTypeOfUnit,
                    HistoryDatePurchase = history.HistoryDatePurchase,
                    HistoryTerms = history.HistoryTerms,
                    HistoryRemainingBalance = history.HistoryRemainingBalance,
                });
            }
            foreach (LoanPersonalPropertyViewModel property in loanDetailsViewModel.PersonalProperties)
            {
                newPersonalProperties.Add(new LoanPersonalProperty
                {
                    LoanID = id,
                    Property = property.Property,
                });
            }
            foreach (LoanUnitDesiredViewModel unit in loanDetailsViewModel.UnitDesireds)
            {
                newUnitDesireds.Add(new LoanUnitDesired
                {
                    LoanID = id,
                    DesiredBrandModel = unit.DesiredBrandModel,
                    DesiredSerialNo = unit.DesiredSerialNo,
                    DesiredCode = unit.DesiredCode,
                    DesiredAmount = unit.DesiredAmount,
                    DesiredAccounting = unit.DesiredAccounting,
                });
            }
            //foreach (LoanUnitDesiredTCViewModel unitTC in loanDetailsViewModel.UnitDesiredTCs)
            //{
            //    newUnitDesiredTCs.Add(new LoanUnitDesiredTC
            //    {
            //        LoanID = id,
            //        DesiredTCBrandModel = unitTC.DesiredTCBrandModel,
            //        DesiredTCTerms = unitTC.DesiredTCTerms,
            //        DesiredTCDownPayment = unitTC.DesiredTCDownPayment,
            //        DesiredTCMonthlyInstallment = unitTC.DesiredTCMonthlyInstallment,
            //        DesiredTCTotalPrice = unitTC.DesiredTCTotalPrice,
            //        DesiredTCTotalRebate = unitTC.DesiredTCTotalRebate,
            //        DesiredTCRemarks = unitTC.DesiredTCRemarks,
            //    });
            //}
            foreach (LoanFileViewModel file in loanDetailsViewModel.LoanAttachments)
            {
                newAttachments.Add(new LoanAttachment { LoanID = id, FileName = file.FileName });
            }

            var result = _loanRepository.Update(loanForUpdate,
                                                     newCreditHistories,
                                                     newCustomerChildrens,
                                                     newPersonalProperties,
                                                     newUnitDesireds,
                                                     newAttachments,
                                                     loanDetailsViewModel.RemovedAttachments
                                                     );

            if (loanDetailsViewModel.Signature != null &&
                    loanDetailsViewModel.Signature.FileDataArray != null)
            {
                string filename = string.Concat(id, Constants.Common.SignatureNameExtension);

                loanDetailsViewModel.Signature.FileName = filename;

                SaveFiles(id,
                          new List<LoanFileViewModel> { loanDetailsViewModel.Signature },
                          attachmentPath,
                          Constants.Upload.ClientSignature);
            }

            UpdateLoanFileAttachments(id,
                                  loanDetailsViewModel.RemovedAttachments,
                                  loanDetailsViewModel.LoanAttachments,
                                  attachmentPath);

            return result;

        }
        public void UpdateLoanFileAttachments(int loanID, List<string> removedAttachments, List<LoanFileViewModel> newAttachments, string attachmentPath)
        {
            var rootPath = _hostingEnvironment.ContentRootPath;
            var filePath = string.Format(Constants.Attachment.FullFilePath, rootPath, attachmentPath, loanID, Constants.Upload.Attachment);

            if (removedAttachments.Count > 0)
            {
                List<string> removedAttachmentNames = new List<string>();

                foreach (var removedAttachment in removedAttachments)
                {
                    removedAttachmentNames.Add(removedAttachment);
                }

                if (Helper.DoesPathExist(filePath) == true)
                {
                    Helper.DeleteFiles(removedAttachmentNames, filePath);
                }
            }

            if (newAttachments.Count > 0)
            {
                SaveFiles(loanID,
                          newAttachments,
                          attachmentPath,
                          Constants.Upload.Attachment);
            }
        }

        public void Delete(int id)
        {
            _loanRepository.Delete(id);
        }

        public bool UpdateLoanStatus(LoanApprovalViewModel approvalViewModel)
        {
            return _loanRepository.UpdateLoanStatus(approvalViewModel);
        }

    }

}
