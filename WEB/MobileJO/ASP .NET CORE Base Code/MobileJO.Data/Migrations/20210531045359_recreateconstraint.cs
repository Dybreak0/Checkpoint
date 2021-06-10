using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class recreateconstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new DateTime(2021, 5, 31, 12, 53, 58, 979, DateTimeKind.Local).AddTicks(2974), new DateTime(2021, 5, 31, 12, 53, 58, 980, DateTimeKind.Local).AddTicks(3686) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e089ba03-1ac7-4297-b2c4-15b2fcf6a961", "222145d0-948b-4ae1-bd4f-e38894c5600b", "Administrator", "ADMINISTRATOR" },
                    { "245d2ce9-3ecf-4b31-8f8a-2ecb50338774", "2c7e2979-a817-4665-8d8e-ad7374dbc718", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 53, 58, 983, DateTimeKind.Local).AddTicks(3600), new DateTime(2021, 5, 31, 12, 53, 58, 983, DateTimeKind.Local).AddTicks(4281) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 53, 58, 983, DateTimeKind.Local).AddTicks(4919), new DateTime(2021, 5, 31, 12, 53, 58, 983, DateTimeKind.Local).AddTicks(4931) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 53, 58, 982, DateTimeKind.Local).AddTicks(1200), new DateTime(2021, 5, 31, 12, 53, 58, 981, DateTimeKind.Local).AddTicks(7700), new DateTime(2021, 5, 31, 12, 53, 58, 981, DateTimeKind.Local).AddTicks(7332), new DateTime(2021, 5, 31, 12, 53, 58, 982, DateTimeKind.Local).AddTicks(2057) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 12, 53, 58, 982, DateTimeKind.Local).AddTicks(3142), new DateTime(2021, 5, 31, 12, 53, 58, 982, DateTimeKind.Local).AddTicks(3072), new DateTime(2021, 5, 31, 12, 53, 58, 982, DateTimeKind.Local).AddTicks(3061), new DateTime(2021, 5, 31, 12, 53, 58, 982, DateTimeKind.Local).AddTicks(3151) });

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                keyValue: "245d2ce9-3ecf-4b31-8f8a-2ecb50338774");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e089ba03-1ac7-4297-b2c4-15b2fcf6a961");

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
    }
}
