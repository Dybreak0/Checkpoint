using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class updateLoanTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bda8a3a7-ad6b-477f-b58f-dd699110cdac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc3aef5e-7ff5-4ad3-8864-448f8f6da0d0");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationType",
                table: "Loan",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "337a8f2a-8a12-4a6b-9001-6aacec3b97b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aaf3835d-9a5a-4626-8395-70d99f11ffa0");

            migrationBuilder.DropColumn(
                name: "ApplicationType",
                table: "Loan");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 18, 49, 587, DateTimeKind.Local).AddTicks(5939), new DateTime(2021, 6, 1, 0, 18, 49, 588, DateTimeKind.Local).AddTicks(1745) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cc3aef5e-7ff5-4ad3-8864-448f8f6da0d0", "1de61438-5151-4093-8377-c7a52d68b20a", "Administrator", "ADMINISTRATOR" },
                    { "bda8a3a7-ad6b-477f-b58f-dd699110cdac", "22132b87-15f6-4a7f-af5f-a87694294017", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 18, 49, 591, DateTimeKind.Local).AddTicks(2272), new DateTime(2021, 6, 1, 0, 18, 49, 591, DateTimeKind.Local).AddTicks(2670) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 18, 49, 591, DateTimeKind.Local).AddTicks(3099), new DateTime(2021, 6, 1, 0, 18, 49, 591, DateTimeKind.Local).AddTicks(3105) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(4795), new DateTime(2021, 6, 1, 0, 18, 49, 589, DateTimeKind.Local).AddTicks(9874), new DateTime(2021, 6, 1, 0, 18, 49, 589, DateTimeKind.Local).AddTicks(8585), new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(5266) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(5907), new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(5873), new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(5868), new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(5913) });
        }
    }
}
