using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class fixApprovedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_User_Loan_ApprovedBy",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_Loan_Loan_ApprovedBy",
                table: "Loan");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d380dcc-fb96-440b-a138-aee88b553c3a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5666c2ce-2929-4d41-9f3a-6de7ed7a9fbd");

            migrationBuilder.DropColumn(
                name: "Loan_ApprovedBy",
                table: "Loan");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 6, 22, 9, 33, 458, DateTimeKind.Local).AddTicks(7200), new DateTime(2021, 6, 6, 22, 9, 33, 459, DateTimeKind.Local).AddTicks(3435) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4dbb2914-4940-4979-95f5-2cefa06a45b7", "6dc09bb5-8283-452b-83fa-c7d4c588cb98", "Administrator", "ADMINISTRATOR" },
                    { "8a29acbc-3789-41db-b40e-29271fd2ee18", "5cbe23e4-cc22-4906-b3a2-7d3ec6ecc2d0", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 6, 22, 9, 33, 460, DateTimeKind.Local).AddTicks(9988), new DateTime(2021, 6, 6, 22, 9, 33, 461, DateTimeKind.Local).AddTicks(366) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 6, 22, 9, 33, 461, DateTimeKind.Local).AddTicks(744), new DateTime(2021, 6, 6, 22, 9, 33, 461, DateTimeKind.Local).AddTicks(750) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 6, 22, 9, 33, 460, DateTimeKind.Local).AddTicks(3455), new DateTime(2021, 6, 6, 22, 9, 33, 460, DateTimeKind.Local).AddTicks(1400), new DateTime(2021, 6, 6, 22, 9, 33, 460, DateTimeKind.Local).AddTicks(1176), new DateTime(2021, 6, 6, 22, 9, 33, 460, DateTimeKind.Local).AddTicks(3793) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 6, 22, 9, 33, 460, DateTimeKind.Local).AddTicks(4473), new DateTime(2021, 6, 6, 22, 9, 33, 460, DateTimeKind.Local).AddTicks(4438), new DateTime(2021, 6, 6, 22, 9, 33, 460, DateTimeKind.Local).AddTicks(4434), new DateTime(2021, 6, 6, 22, 9, 33, 460, DateTimeKind.Local).AddTicks(4478) });

            migrationBuilder.CreateIndex(
                name: "IX_Loan_ApprovedBy",
                table: "Loan",
                column: "ApprovedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_User_ApprovedBy",
                table: "Loan",
                column: "ApprovedBy",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_User_ApprovedBy",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_Loan_ApprovedBy",
                table: "Loan");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4dbb2914-4940-4979-95f5-2cefa06a45b7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a29acbc-3789-41db-b40e-29271fd2ee18");

            migrationBuilder.AddColumn<int>(
                name: "Loan_ApprovedBy",
                table: "Loan",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 4, 14, 15, 8, 265, DateTimeKind.Local).AddTicks(5418), new DateTime(2021, 6, 4, 14, 15, 8, 266, DateTimeKind.Local).AddTicks(4451) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2d380dcc-fb96-440b-a138-aee88b553c3a", "28a4660c-f002-4ace-8bf4-ea935ae76720", "Administrator", "ADMINISTRATOR" },
                    { "5666c2ce-2929-4d41-9f3a-6de7ed7a9fbd", "71195a60-e5a3-4447-a820-b76412b689dc", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 4, 14, 15, 8, 270, DateTimeKind.Local).AddTicks(209), new DateTime(2021, 6, 4, 14, 15, 8, 270, DateTimeKind.Local).AddTicks(948) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 4, 14, 15, 8, 270, DateTimeKind.Local).AddTicks(1690), new DateTime(2021, 6, 4, 14, 15, 8, 270, DateTimeKind.Local).AddTicks(1719) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 4, 14, 15, 8, 268, DateTimeKind.Local).AddTicks(5385), new DateTime(2021, 6, 4, 14, 15, 8, 268, DateTimeKind.Local).AddTicks(1531), new DateTime(2021, 6, 4, 14, 15, 8, 268, DateTimeKind.Local).AddTicks(1160), new DateTime(2021, 6, 4, 14, 15, 8, 268, DateTimeKind.Local).AddTicks(5941) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 4, 14, 15, 8, 268, DateTimeKind.Local).AddTicks(6826), new DateTime(2021, 6, 4, 14, 15, 8, 268, DateTimeKind.Local).AddTicks(6767), new DateTime(2021, 6, 4, 14, 15, 8, 268, DateTimeKind.Local).AddTicks(6761), new DateTime(2021, 6, 4, 14, 15, 8, 268, DateTimeKind.Local).AddTicks(6836) });

            migrationBuilder.CreateIndex(
                name: "IX_Loan_Loan_ApprovedBy",
                table: "Loan",
                column: "Loan_ApprovedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_User_Loan_ApprovedBy",
                table: "Loan",
                column: "Loan_ApprovedBy",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
