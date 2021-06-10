using Microsoft.EntityFrameworkCore;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.LoanApplication;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Data.Repositories
{
    public class LoanRepository : BaseRepository, ILoanRepository
    {
        /// <summary>
        ///     Constructor for IUnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public LoanRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public int Create(Loan loan,
                   List<LoanCreditHistory> creditHistories,
                   List<LoanCustomerChildren> customerChildrens,
                   List<LoanPersonalProperty> personalProperties,
                   List<LoanUnitDesired> unitDesireds,
                   List<LoanAttachment> loanAttachments,
                   string signatureFilename = null)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    int insert_id = 0;

                    GetDbSet<Loan>().Add(loan);          
                    UnitOfWork.SaveChanges();

                    insert_id = loan.LoanID;

                    Loan loanForUpdate = FindLoan(insert_id);

                    if (loanForUpdate != null)
                    {
                        loanForUpdate.ApplicationNumber = string.Concat("APP-", insert_id.ToString("000000.##"));

                        if (!string.IsNullOrEmpty(signatureFilename))
                        {
                            loanForUpdate.ClientSignature = string.Concat(insert_id, signatureFilename);
                            var temp = loanForUpdate.ClientSignature.Length;
                        }

                        UnitOfWork.SaveChanges();
                    }

                    if (creditHistories.Count() > 0)
                    {
                        creditHistories.Select(x => { x.LoanID = insert_id; return x; }).ToList();
                        GetDbSet<LoanCreditHistory>().AddRange(creditHistories);
                        UnitOfWork.SaveChanges();
                    }

                    if (customerChildrens.Count() > 0)
                    {
                        customerChildrens.Select(x => { x.LoanID = insert_id; return x; }).ToList();
                        GetDbSet<LoanCustomerChildren>().AddRange(customerChildrens);
                        UnitOfWork.SaveChanges();
                    }

                    if (personalProperties.Count > 0)
                    {
                        personalProperties.Select(x => { x.LoanID = insert_id; return x; }).ToList();
                        GetDbSet<LoanPersonalProperty>().AddRange(personalProperties);
                        UnitOfWork.SaveChanges();
                    }
                    if (unitDesireds.Count > 0)
                    {
                        unitDesireds.Select(x => { x.LoanID = insert_id; return x; }).ToList();
                        GetDbSet<LoanUnitDesired>().AddRange(unitDesireds);
                        UnitOfWork.SaveChanges();
                    }
                    //if (unitDesiredTCs.Count > 0)
                    //{
                    //    unitDesiredTCs.Select(x => { x.LoanID = insert_id; return x; }).ToList();
                    //    GetDbSet<LoanUnitDesiredTC>().AddRange(unitDesiredTCs);
                    //    UnitOfWork.SaveChanges();
                    //}
                    if (loanAttachments.Count > 0)
                    {
                        loanAttachments.Select(x => { x.LoanID = insert_id; return x; }).ToList();
                        GetDbSet<LoanAttachment>().AddRange(loanAttachments);
                        UnitOfWork.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    return insert_id;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }
        public Loan FindLoan(int id)
        {
            return GetDbSet<Loan>()
                .Where(x => x.LoanID == id)
                .FirstOrDefault();
        }
        public Loan FindLoanApplication(int id)
        {
            return GetDbSet<Loan>()
                .Include(ch => ch.LoanCreditHistory)
                .Include(cc => cc.LoanCustomerChildren)
                .Include(pp => pp.LoanPersonalProperty)
                .Include(ud => ud.LoanUnitDesired)
                .Include(udtc => udtc.LoanUnitDesiredTC)
                .Include(at => at.LoanAttachment)
                .Include(uc => uc.UserCreatedBy)
                .Include(uu => uu.UserUpdatedBy)
                .Include(ua => ua.UserApprovedBy)
                .Where(x => x.LoanID == id && !x.IsDeleted).FirstOrDefault();
        }

        public ListViewModel ListLoanApplication(LoanSearchViewModel searchListViewModel)
        {
            var loanApplications = RetrieveAllLoanApplication()
                .Where(x => (x.IsDeleted == false) &&
                            (searchListViewModel.Status == "All" || x.LoanStatus == searchListViewModel.Status) &&
                            (string.IsNullOrEmpty(searchListViewModel.CreatedBy) || (x.UserCreatedBy.FirstName + " " + x.UserCreatedBy.LastName).Trim().Contains(searchListViewModel.CreatedBy.Trim())) &&
                            (string.IsNullOrEmpty(searchListViewModel.ApplicationNo) || x.ApplicationNumber.Contains(searchListViewModel.ApplicationNo)) &&
                            (string.IsNullOrEmpty(searchListViewModel.ClientName) || (x.Name).Trim().Contains(searchListViewModel.ClientName.Trim())) &&
                            (x.CreatedDate >= searchListViewModel.DateFrom && x.CreatedDate <= searchListViewModel.DateTo.AddDays(1)))
                .OrderByDescending(x => x.LoanID);

            if (searchListViewModel.Page == 0)
                searchListViewModel.Page = 1;
            var totalCount = loanApplications.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchListViewModel.PageSize);

            var results = loanApplications.Skip(searchListViewModel.PageSize * (searchListViewModel.Page - 1))
                .Take(searchListViewModel.PageSize)
                .AsEnumerable()
                .Select(loanApplication => new LoanDetailsViewModel
                {
                    LoanID = loanApplication.LoanID,
                    ApplicationNumber = loanApplication.ApplicationNumber,
                    Name = loanApplication.Name,
                    CreatedDate = loanApplication.CreatedDate,
                    CreatedByName = loanApplication.CreatedByName,
                    LoanStatus = loanApplication.LoanStatus
                })
                .ToList();

            var pagination = new
            {
                pages = totalPages,
                size = totalCount
            };

            return new ListViewModel { Pagination = pagination, Data = results };
        }

        public ListViewModel PendingLoanApplication(LoanSearchPendingViewModel searchPendingViewModel)
        {
            var loanApplications = RetrieveAllLoanApplication()
                .Where(x => (x.IsDeleted == false) && (x.LoanStatus == "Pending") && (x.BranchID == searchPendingViewModel.BranchID) && ((int)Constants.RoleType.Administrator == searchPendingViewModel.RoleID) &&
                            (string.IsNullOrEmpty(searchPendingViewModel.CreatedBy) || (x.UserCreatedBy.FirstName + " " + x.UserCreatedBy.LastName).Trim().Contains(searchPendingViewModel.CreatedBy.Trim())) &&
                            (string.IsNullOrEmpty(searchPendingViewModel.ApplicationNo) || x.ApplicationNumber.Contains(searchPendingViewModel.ApplicationNo)) &&
                            (string.IsNullOrEmpty(searchPendingViewModel.ClientName) || (x.Name).Trim().Contains(searchPendingViewModel.ClientName.Trim())) &&
                           (x.CreatedDate >= searchPendingViewModel.DateFrom && x.CreatedDate <= searchPendingViewModel.DateTo.AddDays(1)))
                .OrderByDescending(x => x.LoanID);
            if (searchPendingViewModel.Page == 0)
                searchPendingViewModel.Page = 1;
            var totalCount = loanApplications.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchPendingViewModel.PageSize);

            var results = loanApplications.Skip(searchPendingViewModel.PageSize * (searchPendingViewModel.Page - 1))
                .Take(searchPendingViewModel.PageSize)
                .AsEnumerable()
                .Select(loanApplication => new LoanDetailsViewModel
                {
                    LoanID = loanApplication.LoanID,
                    ApplicationNumber = loanApplication.ApplicationNumber,
                    Name = loanApplication.Name,
                    CreatedDate = loanApplication.CreatedDate,
                    CreatedByName = loanApplication.CreatedByName
                })
                .ToList();

            var pagination = new
            {
                pages = totalPages,
                size = totalCount
            };

            return new ListViewModel { Pagination = pagination, Data = results };
        }
        public IQueryable<Loan> RetrieveAllLoanApplication()
        {
            return GetDbSet<Loan>()
                .Include(ch => ch.LoanCreditHistory)
                .Include(cc => cc.LoanCustomerChildren)
                .Include(pp => pp.LoanPersonalProperty)
                .Include(ud => ud.LoanUnitDesired)
                .Include(udtc => udtc.LoanUnitDesiredTC)
                .Include(at => at.LoanAttachment)
                .Include(uc => uc.UserCreatedBy)
                .Include(uu => uu.UserUpdatedBy)
                .Include(ua => ua.UserApprovedBy);
        }

        /// <summary>
        ///     Used to retrieve loan application records from the database to be used for download into an excel file
        /// </summary>
        /// <param name="searchListViewModel">Holds the jloan application search filters</param>
        /// <returns>Holds the table data of loan application records</returns>
        public List<LoanDetailsViewModel> DownloadLoanApplication(LoanSearchViewModel searchListViewModel)
        {
            var loanApplications = RetrieveAllLoanApplication()
                .Where(x => (x.IsDeleted == false) &&
                            (searchListViewModel.Status == "All" || x.LoanStatus == searchListViewModel.Status) &&
                            (string.IsNullOrEmpty(searchListViewModel.CreatedBy) || (x.UserCreatedBy.FirstName + " " + x.UserCreatedBy.LastName).Trim().Contains(searchListViewModel.CreatedBy.Trim())) &&
                            (string.IsNullOrEmpty(searchListViewModel.ApplicationNo) || x.ApplicationNumber.Contains(searchListViewModel.ApplicationNo)) &&
                            (string.IsNullOrEmpty(searchListViewModel.ClientName) || (x.Name).Trim().Contains(searchListViewModel.ClientName.Trim())) &&
                            (x.CreatedDate >= searchListViewModel.DateFrom && x.CreatedDate <= searchListViewModel.DateTo.AddDays(1)))
                .OrderByDescending(x => x.LoanID);

            var results = loanApplications
                .OrderByDescending(x => x.ApplicationNumber)
                .AsEnumerable()
                .Select(loanApplication => new LoanDetailsViewModel
                {
                    ApplicationNumber = loanApplication.ApplicationNumber,
                    Name = loanApplication.Name,
                    CreatedDate = loanApplication.CreatedDate,
                    CreatedByName = loanApplication.CreatedByName,
                    LoanStatus = loanApplication.LoanStatus
                })
                .ToList();

            return results;
        }

        /// <summary>
        /// Updates a loan application record 
        /// </summary>
        /// <param name="loan"></param>
        /// <param name="newCreditHistories"></param>
        /// <param name="newCustomerChildrens"></param>
        /// <param name="newPersonalProperties"></param>
        /// <param name="newUnitDesireds"></param>
        /// <param name="newUnitDesiredTCs"></param>
        /// <param name="newAttachments"></param
        /// <param name="removedAttachments"></param>
        /// <returns></returns>
        public bool Update(Loan loan,
                   List<LoanCreditHistory> newCreditHistories,
                   List<LoanCustomerChildren> newCustomerChildrens,
                   List<LoanPersonalProperty> newPersonalProperties,
                   List<LoanUnitDesired> newUnitDesireds,
                   List<LoanAttachment> newAttachments,
                   List<string> removedAttachments)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    int id = loan.LoanID;

                    Loan loanForUpdate = FindLoan(id);

                    loanForUpdate.BranchID = loan.BranchID;
                    loanForUpdate.Name = loan.Name;
                    loanForUpdate.BirthDate = loan.BirthDate;
                    loanForUpdate.Age = loan.Age;
                    loanForUpdate.RegionID = loan.RegionID;
                    loanForUpdate.CityID = loan.CityID;
                    loanForUpdate.ZipCode = loan.ZipCode;
                    loanForUpdate.HouseUnitBuildingNo = loan.HouseUnitBuildingNo;
                    loanForUpdate.StreetBarangay = loan.StreetBarangay;
                    loanForUpdate.Landmark = loan.Landmark;
                    loanForUpdate.BirthPlace = loan.BirthPlace;
                    loanForUpdate.PreviousAddress = loan.PreviousAddress;
                    loanForUpdate.PhoneNo = loan.PhoneNo;
                    loanForUpdate.TelNo = loan.TelNo;
                    loanForUpdate.EmailAddress = loan.EmailAddress;
                    loanForUpdate.Facebook = loan.Facebook;
                    loanForUpdate.StabilityOfResidence = loan.StabilityOfResidence;
                    loanForUpdate.RentingName = loan.RentingName;
                    loanForUpdate.RentingAddress = loan.RentingAddress;
                    loanForUpdate.RentingTelNo = loan.RentingTelNo;
                    loanForUpdate.MaritalStatus = loan.MaritalStatus;
                    loanForUpdate.Land = loan.Land;
                    loanForUpdate.HouseMade = loan.HouseMade;
                    loanForUpdate.Industry = loan.Industry;
                    loanForUpdate.TypeOf = loan.TypeOf;
                    loanForUpdate.EmployedWhere = loan.EmployedWhere;
                    loanForUpdate.EmployedHowLong = loan.EmployedHowLong;
                    loanForUpdate.EmployedPosition = loan.EmployedPosition;
                    loanForUpdate.EmployedPresentBusinessAddress = loan.EmployedPresentBusinessAddress;
                    loanForUpdate.EmployedTelNo = loan.EmployedTelNo;
                    loanForUpdate.EmployedSalary = loan.EmployedSalary;
                    loanForUpdate.EmployedPreviousEmployment = loan.EmployedPreviousEmployment;
                    loanForUpdate.EmployedPreviousBusinessAddress = loan.EmployedPreviousBusinessAddress;

                    loanForUpdate.BusinessNature = loan.BusinessNature;
                    loanForUpdate.BusinessName = loan.BusinessName;
                    loanForUpdate.BusinessAddress = loan.BusinessAddress;
                    loanForUpdate.BusinessCapital = loan.BusinessCapital;
                    loanForUpdate.BusinessMonthlyIncomeNet = loan.BusinessMonthlyIncomeNet;
                    loanForUpdate.BusinessMonthlyIncomeGross = loan.BusinessMonthlyIncomeGross;
                    loanForUpdate.BusinessPhoneNo = loan.BusinessPhoneNo;
                    loanForUpdate.BusinessHowLong = loan.BusinessHowLong;

                    loanForUpdate.PensionAgency = loan.PensionAgency;
                    loanForUpdate.PensionMonthly = loan.PensionMonthly;

                    loanForUpdate.RemittanceName = loan.RemittanceName;
                    loanForUpdate.RemittanceLocation = loan.RemittanceLocation;
                    loanForUpdate.RemittanceRelationship = loan.RemittanceRelationship;
                    loanForUpdate.RemittanceMonthlyAmount = loan.RemittanceMonthlyAmount;
                    loanForUpdate.RemittanceFrequency = loan.RemittanceFrequency;

                    loanForUpdate.FatherName = loan.FatherName;
                    loanForUpdate.FatherAge = loan.FatherAge;
                    loanForUpdate.FatherAddress = loan.FatherAddress;
                    loanForUpdate.FatherFacebook = loan.FatherFacebook;
                    loanForUpdate.FatherIncomeSource = loan.FatherIncomeSource;
                    loanForUpdate.FatherOfficeAddress = loan.FatherOfficeAddress;
                    loanForUpdate.FatherPosition = loan.FatherPosition;
                    loanForUpdate.FatherHowLong = loan.FatherHowLong;

                    loanForUpdate.MotherMaidenName = loan.MotherMaidenName;
                    loanForUpdate.MotherAge = loan.MotherAge;
                    loanForUpdate.MotherAddress = loan.MotherAddress;
                    loanForUpdate.MotherFacebook = loan.MotherFacebook;
                    loanForUpdate.MotherIncomeSource = loan.MotherIncomeSource;
                    loanForUpdate.MotherOfficeAddress = loan.MotherOfficeAddress;
                    loanForUpdate.MotherPosition = loan.MotherPosition;
                    loanForUpdate.MotherHowLong = loan.MotherHowLong;

                    loanForUpdate.SpouseName = loan.SpouseName;
                    loanForUpdate.SpouseAge = loan.SpouseAge;
                    loanForUpdate.SpouseIncomeSource = loan.SpouseIncomeSource;
                    loanForUpdate.SpouseOfficeAddress = loan.SpouseOfficeAddress;
                    loanForUpdate.SpousePosition = loan.SpousePosition;
                    loanForUpdate.SpouseHowLong = loan.SpouseHowLong;
                    loanForUpdate.SpouseTelNo = loan.SpouseTelNo;
                    loanForUpdate.SpouseSalary = loan.SpouseSalary;

                    loanForUpdate.FatherInLawName = loan.FatherInLawName;
                    loanForUpdate.FatherInLawAge = loan.FatherInLawAge;
                    loanForUpdate.FatherInLawAddress = loan.FatherInLawAddress;
                    loanForUpdate.FatherInLawFacebook = loan.FatherInLawFacebook;
                    loanForUpdate.FatherInLawIncomeSource = loan.FatherInLawIncomeSource;
                    loanForUpdate.FatherInLawOfficeAddress = loan.FatherInLawOfficeAddress;
                    loanForUpdate.FatherInLawPosition = loan.FatherInLawPosition;
                    loanForUpdate.FatherInLawHowLong = loan.FatherInLawHowLong;

                    loanForUpdate.MotherInLawName = loan.MotherInLawName;
                    loanForUpdate.MotherInLawAge = loan.MotherInLawAge;
                    loanForUpdate.MotherInLawAddress = loan.MotherInLawAddress;
                    loanForUpdate.MotherInLawFacebook = loan.MotherInLawFacebook;
                    loanForUpdate.MotherInLawIncomeSource = loan.MotherInLawIncomeSource;
                    loanForUpdate.MotherInLawOfficeAddress = loan.MotherInLawOfficeAddress;
                    loanForUpdate.MotherInLawPosition = loan.MotherInLawPosition;
                    loanForUpdate.MotherInLawHowLong = loan.MotherInLawHowLong;

                    loanForUpdate.DesiredTCTerms = loan.DesiredTCTerms;
                    loanForUpdate.DesiredTCDownPayment = loan.DesiredTCDownPayment;
                    loanForUpdate.DesiredTCMonthlyInstallment = loan.DesiredTCMonthlyInstallment;
                    loanForUpdate.DesiredTCTotalPrice = loan.DesiredTCTotalPrice;
                    loanForUpdate.DesiredTCTotalRebate = loan.DesiredTCTotalRebate;
                    loanForUpdate.DesiredTCRemarks = loan.DesiredTCRemarks;

                    loanForUpdate.ConfirmationOfficer = loan.ConfirmationOfficer;
                    loanForUpdate.ConfirmationDate = loan.ConfirmationDate;
                    loanForUpdate.ConfirmationTime = loan.ConfirmationTime;

                    loanForUpdate.OfficeUseCAName = loan.OfficeUseCAName;
                    loanForUpdate.OfficeUseCARemarks = loan.OfficeUseCARemarks;
                    loanForUpdate.OfficeUseCADate = loan.OfficeUseCADate;
                    loanForUpdate.OfficeUseCATime = loan.OfficeUseCATime;

                    loanForUpdate.OfficeUseCCSName = loan.OfficeUseCCSName;
                    loanForUpdate.OfficeUseCCSRemarks = loan.OfficeUseCCSRemarks;
                    loanForUpdate.OfficeUseCCSDate = loan.OfficeUseCCSDate;
                    loanForUpdate.OfficeUseCCSTime = loan.OfficeUseCCSTime;
                    loanForUpdate.OfficeUseInvoiceNo = loan.OfficeUseInvoiceNo;
                    loanForUpdate.OfficeUseInvoiceDate = loan.OfficeUseInvoiceDate;
                    loanForUpdate.OfficeUseORNo = loan.OfficeUseORNo;
                    loanForUpdate.OfficeUseORDate = loan.OfficeUseORDate;
                    loanForUpdate.OfficeUseAmount = loan.OfficeUseAmount;
                    loanForUpdate.OfficeUseCashier = loan.OfficeUseCashier;

                    loanForUpdate.ClientSignature = loan.ClientSignature;

                    loanForUpdate.UpdatedDate = loan.UpdatedDate;
                    loanForUpdate.UpdatedBy = loan.UpdatedBy;

                    UnitOfWork.SaveChanges();

                    var loanCreditHistories = RetrieveLoanCreditHistories(id);
                    var loanCustomerChildrens = RetrieveLoanCustomerChildrens(id);
                    var loanPersonalProperties = RetrieveLoanPersonalProperties(id);
                    var loanUnitDesireds = RetrieveLoanUnitDesireds(id);
                    //var loanUnitDesiredTCs = RetrieveLoanUnitDesiredTCs(id);
                    var loanAttachments = RetrieveLoanAttachments(id);

                    if (loanCreditHistories.Count > 0)
                    {
                        GetDbSet<LoanCreditHistory>().RemoveRange(loanCreditHistories);
                        UnitOfWork.SaveChanges();
                    }
                    if (loanCustomerChildrens.Count > 0)
                    {
                        GetDbSet<LoanCustomerChildren>().RemoveRange(loanCustomerChildrens);
                        UnitOfWork.SaveChanges();
                    }
                    if (loanPersonalProperties.Count > 0)
                    {
                        GetDbSet<LoanPersonalProperty>().RemoveRange(loanPersonalProperties);
                        UnitOfWork.SaveChanges();
                    }
                    if (loanUnitDesireds.Count > 0)
                    {
                        GetDbSet<LoanUnitDesired>().RemoveRange(loanUnitDesireds);
                        UnitOfWork.SaveChanges();
                    }
                    //if (loanUnitDesiredTCs.Count > 0)
                    //{
                    //    GetDbSet<LoanUnitDesiredTC>().RemoveRange(loanUnitDesiredTCs);
                    //    UnitOfWork.SaveChanges();
                    //}

                    if (loanAttachments.Count > 0)
                    {
                        foreach (var removed_attachment in removedAttachments)
                        {
                            var attachmententsForDeletion = loanAttachments.Where(x => x.FileName == removed_attachment);

                            if (attachmententsForDeletion.Count() > 0)
                            {
                                GetDbSet<LoanAttachment>().RemoveRange(attachmententsForDeletion);
                            }

                            UnitOfWork.SaveChanges();
                        }
                    }

                    GetDbSet<LoanCreditHistory>().AddRange(newCreditHistories);
                    UnitOfWork.SaveChanges();

                    GetDbSet<LoanCustomerChildren>().AddRange(newCustomerChildrens);
                    UnitOfWork.SaveChanges();

                    GetDbSet<LoanPersonalProperty>().AddRange(newPersonalProperties);
                    UnitOfWork.SaveChanges();

                    GetDbSet<LoanUnitDesired>().AddRange(newUnitDesireds);
                    UnitOfWork.SaveChanges();

                    //GetDbSet<LoanUnitDesiredTC>().AddRange(newUnitDesiredTCs);
                    //UnitOfWork.SaveChanges();

                    if (newAttachments.Count > 0)
                    {
                        var attachmentForInsert = new List<LoanAttachment>();
                        var updated_attachments = RetrieveLoanAttachments(id).Select(x => x.FileName).ToList();
                        var new_attachments = newAttachments.Select(x => x.FileName).ToList();

                        foreach (var new_attachment in new_attachments)
                        {
                            if (!updated_attachments.Contains(new_attachment))
                            {
                                attachmentForInsert.Add(new LoanAttachment
                                {
                                    LoanID = id,
                                    FileName = new_attachment
                                });

                            }
                        }

                        GetDbSet<LoanAttachment>().AddRange(attachmentForInsert);
                        UnitOfWork.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }   
        }
        
        public List<LoanCreditHistory> RetrieveLoanCreditHistories(int loanID)
        {
            return GetDbSet<LoanCreditHistory>().Where(i => i.LoanID == loanID).ToList();
        }
        public List<LoanCustomerChildren> RetrieveLoanCustomerChildrens(int loanID)
        {
            return GetDbSet<LoanCustomerChildren>().Where(i => i.LoanID == loanID).ToList();
        }
        public List<LoanPersonalProperty> RetrieveLoanPersonalProperties(int loanID)
        {
            return GetDbSet<LoanPersonalProperty>().Where(i => i.LoanID == loanID).ToList();
        }
        public List<LoanUnitDesired> RetrieveLoanUnitDesireds(int loanID)
        {
            return GetDbSet<LoanUnitDesired>().Where(i => i.LoanID == loanID).ToList();
        }
        public List<LoanUnitDesiredTC> RetrieveLoanUnitDesiredTCs(int loanID)
        {
            return GetDbSet<LoanUnitDesiredTC>().Where(i => i.LoanID == loanID).ToList();
        }
        public List<LoanAttachment> RetrieveLoanAttachments(int loanID)
        {
            return GetDbSet<LoanAttachment>().Where(i => i.LoanID == loanID).ToList();
        }

        public void Delete(int id)
        {
            Loan loanDB = Context.Loan.Find(id);

            loanDB.UpdatedBy = 1;
            loanDB.UpdatedDate = DateTime.Now;
            loanDB.IsDeleted = true;
            UnitOfWork.SaveChanges();
        }


        /// <summary>
        ///     Used to approve/deny the job order revert request
        /// </summary>
        /// <param name="requestModel">Holds the job order revert request data</param>
        /// <returns>Holds the value whether the job order revert request was successfully approved/denied</returns>
        public bool UpdateLoanStatus(LoanApprovalViewModel approvalViewModel)
        {
            var isUpdated = false;

            foreach (int loanID in approvalViewModel.LoanIDs)
            {
                var loanApplication = GetDbSet<Loan>()
                .Where(x => x.LoanID == loanID &&
                            !x.IsDeleted &&
                            x.LoanStatus == "Pending")
                .FirstOrDefault();

                if (loanApplication != null)
                {
                    // Update the status of the job order to Signed
                    loanApplication.LoanStatus = approvalViewModel.LoanStatus;
                    loanApplication.ApprovedDate = DateTime.Now;
                    loanApplication.ApprovedBy = approvalViewModel.ApprovedBy;
                    isUpdated = true;
                    UnitOfWork.SaveChanges();
                }
            }
            return isUpdated;
        }
    }
}