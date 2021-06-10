using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class createdbyname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "337a8f2a-8a12-4a6b-9001-6aacec3b97b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aaf3835d-9a5a-4626-8395-70d99f11ffa0");

            migrationBuilder.AlterColumn<string>(
                name: "LoanStatus",
                table: "Loan",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 8, 51, 9, 134, DateTimeKind.Local).AddTicks(2554), new DateTime(2021, 6, 1, 8, 51, 9, 134, DateTimeKind.Local).AddTicks(8533) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2a197a8e-2931-4ff6-9563-e905592ec7d9", "0cc3ab53-4953-4d2d-844e-ad0a4c2a82d1", "Administrator", "ADMINISTRATOR" },
                    { "0e61fd0c-b5b0-48f3-92bb-41239593d72d", "dbeab46e-677f-42ec-ac63-af3c27cfb3f0", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 8, 51, 9, 136, DateTimeKind.Local).AddTicks(4224), new DateTime(2021, 6, 1, 8, 51, 9, 136, DateTimeKind.Local).AddTicks(4614) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 8, 51, 9, 136, DateTimeKind.Local).AddTicks(4983), new DateTime(2021, 6, 1, 8, 51, 9, 136, DateTimeKind.Local).AddTicks(4990) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 8, 51, 9, 135, DateTimeKind.Local).AddTicks(8146), new DateTime(2021, 6, 1, 8, 51, 9, 135, DateTimeKind.Local).AddTicks(6123), new DateTime(2021, 6, 1, 8, 51, 9, 135, DateTimeKind.Local).AddTicks(5903), new DateTime(2021, 6, 1, 8, 51, 9, 135, DateTimeKind.Local).AddTicks(8481) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 8, 51, 9, 135, DateTimeKind.Local).AddTicks(9027), new DateTime(2021, 6, 1, 8, 51, 9, 135, DateTimeKind.Local).AddTicks(8991), new DateTime(2021, 6, 1, 8, 51, 9, 135, DateTimeKind.Local).AddTicks(8988), new DateTime(2021, 6, 1, 8, 51, 9, 135, DateTimeKind.Local).AddTicks(9032) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e61fd0c-b5b0-48f3-92bb-41239593d72d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a197a8e-2931-4ff6-9563-e905592ec7d9");

            migrationBuilder.AlterColumn<string>(
                name: "LoanStatus",
                table: "Loan",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 59, 15, 936, DateTimeKind.Local).AddTicks(8555), new DateTime(2021, 6, 1, 0, 59, 15, 937, DateTimeKind.Local).AddTicks(4272) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "337a8f2a-8a12-4a6b-9001-6aacec3b97b3", "2fe4067c-7499-409f-8ac5-18541ca21498", "Administrator", "ADMINISTRATOR" },
                    { "aaf3835d-9a5a-4626-8395-70d99f11ffa0", "37fed3f3-f6a6-407e-ab8c-16f73acd674b", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 59, 15, 939, DateTimeKind.Local).AddTicks(574), new DateTime(2021, 6, 1, 0, 59, 15, 939, DateTimeKind.Local).AddTicks(971) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 59, 15, 939, DateTimeKind.Local).AddTicks(1344), new DateTime(2021, 6, 1, 0, 59, 15, 939, DateTimeKind.Local).AddTicks(1350) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 59, 15, 938, DateTimeKind.Local).AddTicks(4289), new DateTime(2021, 6, 1, 0, 59, 15, 938, DateTimeKind.Local).AddTicks(2305), new DateTime(2021, 6, 1, 0, 59, 15, 938, DateTimeKind.Local).AddTicks(2080), new DateTime(2021, 6, 1, 0, 59, 15, 938, DateTimeKind.Local).AddTicks(4613) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 59, 15, 938, DateTimeKind.Local).AddTicks(5148), new DateTime(2021, 6, 1, 0, 59, 15, 938, DateTimeKind.Local).AddTicks(5113), new DateTime(2021, 6, 1, 0, 59, 15, 938, DateTimeKind.Local).AddTicks(5108), new DateTime(2021, 6, 1, 0, 59, 15, 938, DateTimeKind.Local).AddTicks(5154) });
        }
    }
}
