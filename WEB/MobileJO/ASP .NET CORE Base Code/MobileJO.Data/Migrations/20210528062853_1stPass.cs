using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class _1stPass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    RegionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RegionName = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.RegionID);
                });

            migrationBuilder.CreateTable(
                name: "RegionCity",
                columns: table => new
                {
                    CityID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RegionID = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(type: "varchar(100)", nullable: true),
                    ZipCode = table.Column<string>(type: "varchar(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionCity", x => x.CityID);
                    table.ForeignKey(
                        name: "FK_RegionCity_Region_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Region",
                        principalColumn: "RegionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Loan",
                columns: table => new
                {
                    LoanID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationNumber = table.Column<string>(type: "varchar(9)", nullable: true),
                    BranchID = table.Column<int>(nullable: false),
                    LoanStatus = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    RegionID = table.Column<int>(nullable: false),
                    CityID = table.Column<int>(nullable: false),
                    HouseUnitBuildingNo = table.Column<string>(type: "varchar(250)", nullable: true),
                    StreetBarangay = table.Column<string>(type: "varchar(250)", nullable: true),
                    Landmark = table.Column<string>(type: "varchar(250)", nullable: true),
                    BirthPlace = table.Column<string>(type: "varchar(250)", nullable: true),
                    PreviousAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    PhoneNo = table.Column<string>(type: "varchar(25)", nullable: true),
                    TelNo = table.Column<string>(type: "varchar(25)", nullable: true),
                    EmailAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    Facebook = table.Column<string>(type: "varchar(250)", nullable: true),
                    RentingName = table.Column<string>(type: "varchar(250)", nullable: true),
                    RentingAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    RentingTelNo = table.Column<string>(type: "varchar(25)", nullable: true),
                    StabilityOfResidence = table.Column<string>(type: "varchar(40)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "varchar(40)", nullable: true),
                    Land = table.Column<string>(type: "varchar(20)", nullable: true),
                    HouseMade = table.Column<string>(type: "varchar(20)", nullable: true),
                    Industry = table.Column<string>(type: "varchar(250)", nullable: true),
                    TypeOf = table.Column<string>(type: "varchar(50)", nullable: true),
                    EmployedWhere = table.Column<string>(type: "varchar(250)", nullable: true),
                    EmployedHowLong = table.Column<string>(type: "varchar(50)", nullable: true),
                    EmployedPosition = table.Column<string>(type: "varchar(250)", nullable: true),
                    EmployedPresentBusinessAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    EmployedTelNo = table.Column<string>(type: "varchar(25)", nullable: true),
                    EmployedSalary = table.Column<string>(type: "varchar(100)", nullable: true),
                    EmployedPreviousEmployment = table.Column<string>(type: "varchar(250)", nullable: true),
                    EmployedPreviousBusinessAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    BusinessNature = table.Column<string>(type: "varchar(250)", nullable: true),
                    BusinessName = table.Column<string>(type: "varchar(250)", nullable: true),
                    BusinessAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    BusinessCapital = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    BusinessMonthlyIncomeNet = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    BusinessMonthlyIncomeGross = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    BusinessPhoneNo = table.Column<string>(type: "varchar(25)", nullable: true),
                    BusinessHowLong = table.Column<string>(type: "varchar(50)", nullable: true),
                    PensionAgency = table.Column<string>(type: "varchar(250)", nullable: true),
                    PensionMonthly = table.Column<string>(type: "varchar(250)", nullable: true),
                    RemittanceName = table.Column<string>(type: "varchar(250)", nullable: true),
                    RemittanceLocation = table.Column<string>(type: "varchar(250)", nullable: true),
                    RemittanceRelationship = table.Column<string>(type: "varchar(40)", nullable: true),
                    RemittanceMonthlyAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    RemittanceFrequency = table.Column<string>(type: "varchar(40)", nullable: true),
                    FatherName = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherAge = table.Column<int>(nullable: true),
                    FatherAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherFacebook = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherIncomeSource = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherOfficeAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherPosition = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherHowLong = table.Column<string>(type: "varchar(50)", nullable: true),
                    MotherMaidenName = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherAge = table.Column<int>(nullable: true),
                    MotherAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherFacebook = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherIncomeSource = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherOfficeAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherPosition = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherHowLong = table.Column<string>(type: "varchar(50)", nullable: true),
                    SpouseName = table.Column<string>(type: "varchar(250)", nullable: true),
                    SpouseAge = table.Column<int>(nullable: true),
                    SpouseIncomeSource = table.Column<string>(type: "varchar(250)", nullable: true),
                    SpouseOfficeAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    SpousePosition = table.Column<string>(type: "varchar(250)", nullable: true),
                    SpouseHowLong = table.Column<string>(type: "varchar(50)", nullable: true),
                    SpouseTelNo = table.Column<string>(type: "varchar(25)", nullable: true),
                    SpouseSalary = table.Column<string>(type: "varchar(100)", nullable: true),
                    FatherInLawName = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherInLawAge = table.Column<int>(nullable: true),
                    FatherInLawAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherInLawFacebook = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherInLawIncomeSource = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherInLawOfficeAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherInLawPosition = table.Column<string>(type: "varchar(250)", nullable: true),
                    FatherInLawHowLong = table.Column<string>(type: "varchar(50)", nullable: true),
                    MotherInLawName = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherInLawAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherInLawFacebook = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherInLawIncomeSource = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherInLawOfficeAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherInLawPosition = table.Column<string>(type: "varchar(250)", nullable: true),
                    MotherInLawHowLong = table.Column<string>(type: "varchar(50)", nullable: true),
                    IsAgreed = table.Column<bool>(nullable: false),
                    ClientSignature = table.Column<string>(type: "varchar(250)", nullable: true),
                    ConfirmationOfficer = table.Column<string>(type: "varchar(250)", nullable: true),
                    ConfirmationDate = table.Column<DateTime>(nullable: true),
                    ConfirmationTime = table.Column<TimeSpan>(nullable: true),
                    OfficeUseCAName = table.Column<string>(type: "varchar(250)", nullable: true),
                    OfficeUseCARemarks = table.Column<string>(type: "varchar(250)", nullable: true),
                    OfficeUseCADate = table.Column<DateTime>(nullable: true),
                    OfficeUseCATime = table.Column<TimeSpan>(nullable: true),
                    OfficeUseCCSName = table.Column<string>(type: "varchar(250)", nullable: true),
                    OfficeUseCCSRemarks = table.Column<string>(type: "varchar(250)", nullable: true),
                    OfficeUseCCSDate = table.Column<DateTime>(nullable: true),
                    OfficeUseCCSTime = table.Column<TimeSpan>(nullable: true),
                    OfficeUseInvoiceNo = table.Column<string>(type: "varchar(250)", nullable: true),
                    OfficeUseInvoiceDate = table.Column<DateTime>(nullable: true),
                    OfficeUseORNo = table.Column<string>(type: "varchar(250)", nullable: true),
                    OfficeUseORDate = table.Column<DateTime>(nullable: true),
                    OfficeUseAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    OfficeUseCashier = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: true),
                    ApprovedBy = table.Column<int>(nullable: true),
                    Loan_ApprovedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan", x => x.LoanID);
                });

            migrationBuilder.CreateTable(
                name: "LoanAttachment",
                columns: table => new
                {
                    AttachmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LoanID = table.Column<int>(nullable: false),
                    Filename = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanAttachment", x => x.AttachmentID);
                    table.ForeignKey(
                        name: "FK_LoanAttachment_Loan_LoanID",
                        column: x => x.LoanID,
                        principalTable: "Loan",
                        principalColumn: "LoanID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanCreditHistory",
                columns: table => new
                {
                    CreditHistoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LoanID = table.Column<int>(nullable: false),
                    HistoryCompanyName = table.Column<string>(type: "varchar(250)", nullable: true),
                    HistoryTypeOfUnit = table.Column<string>(type: "varchar(250)", nullable: true),
                    HistoryDatePurchase = table.Column<DateTime>(nullable: true),
                    HistoryTerms = table.Column<string>(type: "varchar(250)", nullable: true),
                    HistoryRemainingBalance = table.Column<decimal>(type: "decimal(18,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanCreditHistory", x => x.CreditHistoryID);
                    table.ForeignKey(
                        name: "FK_LoanCreditHistory_Loan_LoanID",
                        column: x => x.LoanID,
                        principalTable: "Loan",
                        principalColumn: "LoanID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanCustomerChildren",
                columns: table => new
                {
                    ChildID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LoanID = table.Column<int>(nullable: false),
                    ChildName = table.Column<string>(type: "varchar(250)", nullable: true),
                    ChildAge = table.Column<int>(nullable: true),
                    ChildHomeAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    ChildTelNo = table.Column<string>(type: "varchar(20)", nullable: true),
                    ChildEmploySchool = table.Column<string>(type: "varchar(250)", nullable: true),
                    ChildEmploySchoolAddress = table.Column<string>(type: "varchar(250)", nullable: true),
                    ChildPosGrade = table.Column<string>(type: "varchar(250)", nullable: true),
                    ChildHowLong = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanCustomerChildren", x => x.ChildID);
                    table.ForeignKey(
                        name: "FK_LoanCustomerChildren_Loan_LoanID",
                        column: x => x.LoanID,
                        principalTable: "Loan",
                        principalColumn: "LoanID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanPersonalProperty",
                columns: table => new
                {
                    PersonalPropertyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LoanID = table.Column<int>(nullable: false),
                    Property = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanPersonalProperty", x => x.PersonalPropertyID);
                    table.ForeignKey(
                        name: "FK_LoanPersonalProperty_Loan_LoanID",
                        column: x => x.LoanID,
                        principalTable: "Loan",
                        principalColumn: "LoanID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanUnitDesired",
                columns: table => new
                {
                    UnitDesiredID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LoanID = table.Column<int>(nullable: false),
                    DesiredBrandModel = table.Column<string>(type: "varchar(250)", nullable: true),
                    DesiredSerialNo = table.Column<string>(type: "varchar(250)", nullable: true),
                    DesiredCode = table.Column<string>(type: "varchar(150)", nullable: true),
                    DesiredAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DesiredAccounting = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanUnitDesired", x => x.UnitDesiredID);
                    table.ForeignKey(
                        name: "FK_LoanUnitDesired_Loan_LoanID",
                        column: x => x.LoanID,
                        principalTable: "Loan",
                        principalColumn: "LoanID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanUnitDesiredTC",
                columns: table => new
                {
                    UnitDesiredTCID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LoanID = table.Column<int>(nullable: false),
                    DesiredTCBrandModel = table.Column<string>(type: "varchar(250)", nullable: true),
                    DesiredTCTerms = table.Column<string>(type: "varchar(250)", nullable: true),
                    DesiredTCDownPayment = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DesiredTCMonthlyInstallment = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DesiredTCTotalPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DesiredTCTotalRebate = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DesiredTCRemarks = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanUnitDesiredTC", x => x.UnitDesiredTCID);
                    table.ForeignKey(
                        name: "FK_LoanUnitDesiredTC_Loan_LoanID",
                        column: x => x.LoanID,
                        principalTable: "Loan",
                        principalColumn: "LoanID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loan_Loan_ApprovedBy",
                table: "Loan",
                column: "Loan_ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_BranchID",
                table: "Loan",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_CreatedBy",
                table: "Loan",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_UpdatedBy",
                table: "Loan",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LoanAttachment_LoanID",
                table: "LoanAttachment",
                column: "LoanID");

            migrationBuilder.CreateIndex(
                name: "IX_LoanCreditHistory_LoanID",
                table: "LoanCreditHistory",
                column: "LoanID");

            migrationBuilder.CreateIndex(
                name: "IX_LoanCustomerChildren_LoanID",
                table: "LoanCustomerChildren",
                column: "LoanID");

            migrationBuilder.CreateIndex(
                name: "IX_LoanPersonalProperty_LoanID",
                table: "LoanPersonalProperty",
                column: "LoanID");

            migrationBuilder.CreateIndex(
                name: "IX_LoanUnitDesired_LoanID",
                table: "LoanUnitDesired",
                column: "LoanID");

            migrationBuilder.CreateIndex(
                name: "IX_LoanUnitDesiredTC_LoanID",
                table: "LoanUnitDesiredTC",
                column: "LoanID");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_User_Loan_ApprovedBy",
                table: "Loan",
                column: "Loan_ApprovedBy",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_User_CreatedBy",
                table: "Loan",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_User_UpdatedBy",
                table: "Loan",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Branch_BranchID",
                table: "Loan",
                column: "BranchID",
                principalTable: "Branch",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.CreateIndex(
               name: "IX_RegionCity_RegionID",
               table: "RegionCity",
               column: "RegionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_User_CreatedBy",
                table: "Branch");

            migrationBuilder.DropForeignKey(
                name: "FK_Branch_User_UpdatedBy",
                table: "Branch");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_User_CreatedBy",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_User_UpdatedBy",
                table: "Company");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "Choice");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "EmailSetup");

            migrationBuilder.DropTable(
                name: "ForgotPassword");

            migrationBuilder.DropTable(
                name: "JobOrderBillingType");

            migrationBuilder.DropTable(
                name: "LoanAttachment");

            migrationBuilder.DropTable(
                name: "LoanCreditHistory");

            migrationBuilder.DropTable(
                name: "LoanCustomerChildren");

            migrationBuilder.DropTable(
                name: "LoanPersonalProperty");

            migrationBuilder.DropTable(
                name: "LoanUnitDesired");

            migrationBuilder.DropTable(
                name: "LoanUnitDesiredTC");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "RegionCity");

            migrationBuilder.DropTable(
                name: "RevertJobOrder");

            migrationBuilder.DropTable(
                name: "SyncLog");

            migrationBuilder.DropTable(
                name: "TaggedCase");

            migrationBuilder.DropTable(
                name: "Template_Branch");

            migrationBuilder.DropTable(
                name: "Response");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "EmailType");

            migrationBuilder.DropTable(
                name: "BillingType");

            migrationBuilder.DropTable(
                name: "Loan");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "AssignedCase");

            migrationBuilder.DropTable(
                name: "JobOrder");

            migrationBuilder.DropTable(
                name: "QuestionType");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "ApplicationType");

            migrationBuilder.DropTable(
                name: "JobOrderStatus");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "UserType");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
