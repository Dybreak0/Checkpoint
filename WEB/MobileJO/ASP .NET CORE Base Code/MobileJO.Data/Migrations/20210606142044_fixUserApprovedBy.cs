using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class fixUserApprovedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4dbb2914-4940-4979-95f5-2cefa06a45b7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a29acbc-3789-41db-b40e-29271fd2ee18");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 6, 22, 20, 43, 147, DateTimeKind.Local).AddTicks(2759), new DateTime(2021, 6, 6, 22, 20, 43, 147, DateTimeKind.Local).AddTicks(8970) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "df641525-56b0-4b38-9ae5-23089290c747", "17b3e9de-7847-441c-8f76-60c02343fd6f", "Administrator", "ADMINISTRATOR" },
                    { "c2581bd7-3783-4fa1-834a-de08e094ec89", "4c38518d-575c-4f05-80d2-99919c4189ca", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 6, 22, 20, 43, 149, DateTimeKind.Local).AddTicks(7409), new DateTime(2021, 6, 6, 22, 20, 43, 149, DateTimeKind.Local).AddTicks(7788) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 6, 22, 20, 43, 149, DateTimeKind.Local).AddTicks(8158), new DateTime(2021, 6, 6, 22, 20, 43, 149, DateTimeKind.Local).AddTicks(8164) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 6, 22, 20, 43, 149, DateTimeKind.Local).AddTicks(553), new DateTime(2021, 6, 6, 22, 20, 43, 148, DateTimeKind.Local).AddTicks(8249), new DateTime(2021, 6, 6, 22, 20, 43, 148, DateTimeKind.Local).AddTicks(8013), new DateTime(2021, 6, 6, 22, 20, 43, 149, DateTimeKind.Local).AddTicks(891) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 6, 22, 20, 43, 149, DateTimeKind.Local).AddTicks(1434), new DateTime(2021, 6, 6, 22, 20, 43, 149, DateTimeKind.Local).AddTicks(1398), new DateTime(2021, 6, 6, 22, 20, 43, 149, DateTimeKind.Local).AddTicks(1394), new DateTime(2021, 6, 6, 22, 20, 43, 149, DateTimeKind.Local).AddTicks(1439) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2581bd7-3783-4fa1-834a-de08e094ec89");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df641525-56b0-4b38-9ae5-23089290c747");

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
        }
    }
}
