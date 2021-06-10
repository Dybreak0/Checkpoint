using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MobileJO.Data.Models;
using System;

namespace MobileJO.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedJobOrderStatus(modelBuilder);
            SeedBillingType(modelBuilder);
            SeedApplicationType(modelBuilder);
            SeedUserRoles(modelBuilder);
            SeedRoles(modelBuilder);
            SeedEmailTypes(modelBuilder);

            //Loan Seeds
            SeedLoanStatus(modelBuilder);
            SeedLoanSourceOfIncome(modelBuilder);


            // Seed methods below are for development/testing purposes only. 
            // Remove when deploying to production. 
            SeedAccounts(modelBuilder);
            SeedJobOrders(modelBuilder);
            SeedAssignedCases(modelBuilder);
            SeedTaggedCases(modelBuilder);
            SeedJobOrderBillingType(modelBuilder);
        }

        //Loan Seeds
        private static void SeedLoanStatus(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoanStatus>().HasData(
               new LoanStatus { ID = 1, Status = "Pending" },
               new LoanStatus { ID = 2, Status = "Denied" },
               new LoanStatus { ID = 3, Status = "Approved" }
           );
        }
        private static void SeedLoanSourceOfIncome(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoanSourceOfIncome>().HasData(
               new LoanSourceOfIncome { ID = 1, SourceType = "Employed" },
               new LoanSourceOfIncome { ID = 2, SourceType = "Self Employed" },
               new LoanSourceOfIncome { ID = 3, SourceType = "Own Business" },
               new LoanSourceOfIncome { ID = 4, SourceType = "Pension" },
               new LoanSourceOfIncome { ID = 5, SourceType = "Remittance" }
           );
        }

        private static void SeedJobOrderStatus(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobOrderStatus>().HasData(
               new JobOrderStatus { ID = 1, Status = "Pending" },
               new JobOrderStatus { ID = 2, Status = "Signed" },
               new JobOrderStatus { ID = 3, Status = "Sent" },
               new JobOrderStatus { ID = 4, Status = "Requested For Revert" }
           );
        }

        private static void SeedBillingType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillingType>().HasData(
               new BillingType { ID = 1, BillingTypeName = "Warranty" },
               new BillingType { ID = 2, BillingTypeName = "WebPOS" },
               new BillingType { ID = 3, BillingTypeName = "APS" },
               new BillingType { ID = 4, BillingTypeName = "Pending" }
           );
        }

        private static void SeedApplicationType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationType>().HasData(
              new ApplicationType { ID = 1, ApplicationName = "Portfolio" },
              new ApplicationType { ID = 2, ApplicationName = "WebPOS" },
              new ApplicationType { ID = 3, ApplicationName = "HRIS Notes" }
           );
        }

        private static void SeedUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
              new IdentityRole { Name = "Administrator", NormalizedName = "Administrator".ToUpper() },
              new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() }
           );
        }

        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
              new Role { ID = 1, RoleName = "Administrator" },
              new Role { ID = 2, RoleName = "User" }
           );
        }

        private static void SeedEmailTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailType>().HasData(
              new EmailType { ID = Constants.Common.EmailType.To, Type = "to" },
              new EmailType { ID = Constants.Common.EmailType.Cc, Type = "cc" },
              new EmailType { ID = Constants.Common.EmailType.Bcc, Type = "bcc" }
           );
        }

        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
              new User
              {
                  ID = 1,
                  UserID = "",
                  UserName = "admin",
                  Password = "",
                  FirstName = "Alliance Checkpoint",
                  LastName = "Admin",
                  RoleID = 1,
                  EmailAddress = "admin@email.com",
                  Memo = "",
                  AllowedToLogin = true,
                  IsActive = true,
                  Address = "Alliance Software Inc., Buildcomm Center, Sumilon Road, Cebu Business Park, Cebu City",
                  TelephoneNo = "238-6595",
                  MobileNo = "09123456789",
                  CreatedBy = "admin",
                  CreatedDate = DateTime.Now,
                  UpdatedBy = "admin",
                  UpdatedDate = DateTime.Now
              }              
           );
        }

        // Test Data
        private static void SeedAccounts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(
              new Account
              {
                  ID = 1,
                  Name = "Sample Account",
                  EmailAddress = "sampleaccount@email.com",
                  ContactNo = "123456789",
                  ContactPerson = "Sample Contact Person",
                  Address = "Sample Address, Cebu City",
                  Memo = "Sample Memo",
                  IsActive = true,
                  CreatedDate = DateTime.Now,
                  CreatedBy = 1,
                  UpdatedDate = DateTime.Now,
                  UpdatedBy = 1
              }
           );
        }

        private static void SeedJobOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobOrder>().HasData(
              new JobOrder
              {
                  ID = 1,
                  JobOrderNumber = "JO-00001",
                  JobOrderSubject = "Sample JO Subject",
                  StatusID = 1,
                  AccountID = 1,
                  ApplicationTypeID = 1,
                  Branch = "Sample Branch",
                  DateTimeStart = DateTime.Now,
                  DateTimeEnd = DateTime.Now,
                  ActivityDetails = "Sample Activity Details",
                  RootCauseAnalysis = "Sample Root Cause Analaysis",
                  NextStep = "Next Step",
                  PreventiveAction = "PreventiveAction",
                  Remarks = "Remarks",
                  Attendees = "Attendees",
                  IsBilled = true,
                  IsCollaterals = true,
                  IsFixed = false,
                  IsSatisfied = false,
                  ClientSignature = "samplesignature.jpg",
                  ClientRating = 0,                  
                  CreatedDate = DateTime.Now,
                  CreatedBy = 1,
                  UpdatedDate = DateTime.Now,
                  UpdatedBy = 1,
                  IsDeleted = false
              },

              new JobOrder
              {
                  ID = 2,
                  JobOrderNumber = "JO-00002",
                  JobOrderSubject = "Sample JO Subject 2",
                  StatusID = 2,
                  AccountID = 1,
                  ApplicationTypeID = 1,
                  Branch = "Sample Branch 2",
                  DateTimeStart = DateTime.Now,
                  DateTimeEnd = DateTime.Now,
                  ActivityDetails = "Sample Activity Details 2",
                  RootCauseAnalysis = "Sample Root Cause Analaysis 2",
                  NextStep = "Next Step 2",
                  PreventiveAction = "Preventive Action 2",
                  Remarks = "Remarks 2",
                  Attendees = "Attendees 2",
                  IsBilled = true,
                  IsCollaterals = true,
                  IsFixed = true,
                  IsSatisfied = true,
                  ClientSignature = "samplesignature2.jpg",
                  ClientRating = 3,
                  CreatedDate = DateTime.Now,
                  CreatedBy = 1,
                  UpdatedDate = DateTime.Now,
                  UpdatedBy = 1,
                  IsDeleted = false
              }             
           );
        }

        private static void SeedAssignedCases(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssignedCase>().HasData(
              new AssignedCase
              {
                  ID = 1,
                  CaseNumber = "1234",
                  CaseSubject = "Sample Case Subject 1",
                  ApplicationTypeID = 1,
                  AccountID = 1,
                  Priority = "Priority1",
                  Description = "Sample Description",
                  Status = "Ongoing",
                  AssignedUserID = 1,
                  CreatedDate = DateTime.Now,
                  CreatedBy = "admin",
                  UpdatedDate = DateTime.Now,
                  UpdatedBy = "admin"
              },
              new AssignedCase
              {
                  ID = 2,
                  CaseNumber = "5678",
                  CaseSubject = "Sample Case Subject 2",
                  ApplicationTypeID = 1,
                  AccountID = 1,
                  Priority = "Priority2",
                  Description = "Sample Description 2",
                  Status = "Pending",
                  AssignedUserID = 1,
                  CreatedDate = DateTime.Now,
                  CreatedBy = "admin",
                  UpdatedDate = DateTime.Now,
                  UpdatedBy = "admin"
              }
           );
        }

        private static void SeedTaggedCases(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaggedCase>().HasData(
              new TaggedCase { ID = 1, JobOrderID = 1, CaseID = 1 },
              new TaggedCase { ID = 2, JobOrderID = 1, CaseID = 2 }
           );
        }

        private static void SeedJobOrderBillingType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobOrderBillingType>().HasData(
              new JobOrderBillingType { ID = 1, JobOrderID = 1, BillingTypeID = 1 },
              new JobOrderBillingType { ID = 2, JobOrderID = 1, BillingTypeID = 2 },
              new JobOrderBillingType { ID = 3, JobOrderID = 1, BillingTypeID = 3 },
              new JobOrderBillingType { ID = 4, JobOrderID = 1, BillingTypeID = 4 }
           );
        }
    }
}
