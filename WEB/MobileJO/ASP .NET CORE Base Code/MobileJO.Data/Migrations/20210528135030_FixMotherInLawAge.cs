using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class FixMotherInLawAge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c236b1e-c213-462a-aea1-6b3939243a3c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec80733a-5340-4512-b1c6-02fc1b629aaf");

            migrationBuilder.AddColumn<int>(
                name: "MotherInLawAge",
                table: "Loan",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 28, 21, 50, 29, 492, DateTimeKind.Local).AddTicks(5251), new DateTime(2021, 5, 28, 21, 50, 29, 493, DateTimeKind.Local).AddTicks(1271) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "63073254-e4e3-4c4c-b9c4-a28d33e84c2f", "7102371e-a354-4364-9233-eb06a4137530", "Administrator", "ADMINISTRATOR" },
                    { "e2f25bcb-6ea3-4bb3-b3b4-194c6bf8924a", "d36fe349-b1d3-4864-ac28-ff0864ee160f", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 28, 21, 50, 29, 494, DateTimeKind.Local).AddTicks(7547), new DateTime(2021, 5, 28, 21, 50, 29, 494, DateTimeKind.Local).AddTicks(7929) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 28, 21, 50, 29, 494, DateTimeKind.Local).AddTicks(8309), new DateTime(2021, 5, 28, 21, 50, 29, 494, DateTimeKind.Local).AddTicks(8316) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 28, 21, 50, 29, 494, DateTimeKind.Local).AddTicks(1131), new DateTime(2021, 5, 28, 21, 50, 29, 493, DateTimeKind.Local).AddTicks(8917), new DateTime(2021, 5, 28, 21, 50, 29, 493, DateTimeKind.Local).AddTicks(8697), new DateTime(2021, 5, 28, 21, 50, 29, 494, DateTimeKind.Local).AddTicks(1474) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 28, 21, 50, 29, 494, DateTimeKind.Local).AddTicks(2123), new DateTime(2021, 5, 28, 21, 50, 29, 494, DateTimeKind.Local).AddTicks(2089), new DateTime(2021, 5, 28, 21, 50, 29, 494, DateTimeKind.Local).AddTicks(2085), new DateTime(2021, 5, 28, 21, 50, 29, 494, DateTimeKind.Local).AddTicks(2129) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63073254-e4e3-4c4c-b9c4-a28d33e84c2f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2f25bcb-6ea3-4bb3-b3b4-194c6bf8924a");

            migrationBuilder.DropColumn(
                name: "MotherInLawAge",
                table: "Loan");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 28, 14, 28, 53, 146, DateTimeKind.Local).AddTicks(703), new DateTime(2021, 5, 28, 14, 28, 53, 146, DateTimeKind.Local).AddTicks(9164) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c236b1e-c213-462a-aea1-6b3939243a3c", "169d9857-a6b5-4c38-bbc0-400772f44212", "Administrator", "ADMINISTRATOR" },
                    { "ec80733a-5340-4512-b1c6-02fc1b629aaf", "ee289168-f7fe-42d0-a10b-9b44798a84ab", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 28, 14, 28, 53, 150, DateTimeKind.Local).AddTicks(1465), new DateTime(2021, 5, 28, 14, 28, 53, 150, DateTimeKind.Local).AddTicks(2242) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 28, 14, 28, 53, 150, DateTimeKind.Local).AddTicks(3383), new DateTime(2021, 5, 28, 14, 28, 53, 150, DateTimeKind.Local).AddTicks(3397) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 28, 14, 28, 53, 148, DateTimeKind.Local).AddTicks(6943), new DateTime(2021, 5, 28, 14, 28, 53, 148, DateTimeKind.Local).AddTicks(3217), new DateTime(2021, 5, 28, 14, 28, 53, 148, DateTimeKind.Local).AddTicks(2837), new DateTime(2021, 5, 28, 14, 28, 53, 148, DateTimeKind.Local).AddTicks(7545) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 28, 14, 28, 53, 148, DateTimeKind.Local).AddTicks(8584), new DateTime(2021, 5, 28, 14, 28, 53, 148, DateTimeKind.Local).AddTicks(8509), new DateTime(2021, 5, 28, 14, 28, 53, 148, DateTimeKind.Local).AddTicks(8501), new DateTime(2021, 5, 28, 14, 28, 53, 148, DateTimeKind.Local).AddTicks(8595) });
        }
    }
}
