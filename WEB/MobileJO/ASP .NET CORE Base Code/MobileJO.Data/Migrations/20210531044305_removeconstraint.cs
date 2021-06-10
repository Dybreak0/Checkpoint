using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class removeconstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_User_Loan_ApprovedBy",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Branch_BranchID",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_User_CreatedBy",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_User_UpdatedBy",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_Loan_Loan_ApprovedBy",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_Loan_BranchID",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_Loan_CreatedBy",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_Loan_UpdatedBy",
                table: "Loan");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7cc7e8a0-2ab5-46fd-8910-e84cb9a9d493");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f31702d-fcfc-4bed-a479-2ed79680b3a1");

            migrationBuilder.DropColumn(
                name: "Loan_ApprovedBy",
                table: "Loan");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 43, 4, 543, DateTimeKind.Local).AddTicks(1727), new DateTime(2021, 5, 31, 12, 43, 4, 544, DateTimeKind.Local).AddTicks(756) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3264256a-6a4a-474a-a87e-998376734d26", "c5ef2aec-bef8-4f64-bcc7-b2d4a632f7b1", "Administrator", "ADMINISTRATOR" },
                    { "d1e5c84a-ec7e-4d08-93b6-14e4fb26ecd7", "7cc7c2f6-c863-4e7d-93e6-b6589af57a4a", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 43, 4, 546, DateTimeKind.Local).AddTicks(2260), new DateTime(2021, 5, 31, 12, 43, 4, 546, DateTimeKind.Local).AddTicks(2795) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 43, 4, 546, DateTimeKind.Local).AddTicks(3209), new DateTime(2021, 5, 31, 12, 43, 4, 546, DateTimeKind.Local).AddTicks(3215) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 43, 4, 545, DateTimeKind.Local).AddTicks(4874), new DateTime(2021, 5, 31, 12, 43, 4, 545, DateTimeKind.Local).AddTicks(2802), new DateTime(2021, 5, 31, 12, 43, 4, 545, DateTimeKind.Local).AddTicks(2558), new DateTime(2021, 5, 31, 12, 43, 4, 545, DateTimeKind.Local).AddTicks(5230) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 43, 4, 545, DateTimeKind.Local).AddTicks(5782), new DateTime(2021, 5, 31, 12, 43, 4, 545, DateTimeKind.Local).AddTicks(5746), new DateTime(2021, 5, 31, 12, 43, 4, 545, DateTimeKind.Local).AddTicks(5742), new DateTime(2021, 5, 31, 12, 43, 4, 545, DateTimeKind.Local).AddTicks(5788) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3264256a-6a4a-474a-a87e-998376734d26");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1e5c84a-ec7e-4d08-93b6-14e4fb26ecd7");

            migrationBuilder.AddColumn<int>(
                name: "Loan_ApprovedBy",
                table: "Loan",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 36, 44, 750, DateTimeKind.Local).AddTicks(3136), new DateTime(2021, 5, 31, 12, 36, 44, 750, DateTimeKind.Local).AddTicks(9163) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7cc7e8a0-2ab5-46fd-8910-e84cb9a9d493", "456a7bb1-eb2c-4893-888c-c85757645487", "Administrator", "ADMINISTRATOR" },
                    { "8f31702d-fcfc-4bed-a479-2ed79680b3a1", "72f90d14-aa41-45f9-a52f-44ced9f9cadd", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 36, 44, 753, DateTimeKind.Local).AddTicks(142), new DateTime(2021, 5, 31, 12, 36, 44, 753, DateTimeKind.Local).AddTicks(693) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 36, 44, 753, DateTimeKind.Local).AddTicks(1223), new DateTime(2021, 5, 31, 12, 36, 44, 753, DateTimeKind.Local).AddTicks(1234) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 36, 44, 751, DateTimeKind.Local).AddTicks(9155), new DateTime(2021, 5, 31, 12, 36, 44, 751, DateTimeKind.Local).AddTicks(6969), new DateTime(2021, 5, 31, 12, 36, 44, 751, DateTimeKind.Local).AddTicks(6750), new DateTime(2021, 5, 31, 12, 36, 44, 751, DateTimeKind.Local).AddTicks(9608) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 36, 44, 752, DateTimeKind.Local).AddTicks(185), new DateTime(2021, 5, 31, 12, 36, 44, 752, DateTimeKind.Local).AddTicks(150), new DateTime(2021, 5, 31, 12, 36, 44, 752, DateTimeKind.Local).AddTicks(145), new DateTime(2021, 5, 31, 12, 36, 44, 752, DateTimeKind.Local).AddTicks(191) });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_User_Loan_ApprovedBy",
                table: "Loan",
                column: "Loan_ApprovedBy",
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
        }
    }
}
