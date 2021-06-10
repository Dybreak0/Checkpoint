using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class decimaltostring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63073254-e4e3-4c4c-b9c4-a28d33e84c2f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2f25bcb-6ea3-4bb3-b3b4-194c6bf8924a");

            migrationBuilder.AlterColumn<string>(
                name: "RemittanceMonthlyAmount",
                table: "Loan",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OfficeUseAmount",
                table: "Loan",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BusinessMonthlyIncomeNet",
                table: "Loan",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BusinessMonthlyIncomeGross",
                table: "Loan",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BusinessCapital",
                table: "Loan",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 8, 40, 45, 58, DateTimeKind.Local).AddTicks(8635), new DateTime(2021, 5, 31, 8, 40, 45, 59, DateTimeKind.Local).AddTicks(5661) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4ae9dad4-8f71-41a0-b4ab-bcda2efcc462", "b5ecd535-5cb3-4b3f-a6de-69fb6f0c0f3d", "Administrator", "ADMINISTRATOR" },
                    { "dcaf3e31-2245-4102-86c2-81af14607bea", "808afa2b-2e0c-4f54-9a18-ef0102e656b4", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 8, 40, 45, 63, DateTimeKind.Local).AddTicks(1353), new DateTime(2021, 5, 31, 8, 40, 45, 63, DateTimeKind.Local).AddTicks(1813) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 8, 40, 45, 63, DateTimeKind.Local).AddTicks(2429), new DateTime(2021, 5, 31, 8, 40, 45, 63, DateTimeKind.Local).AddTicks(2437) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 8, 40, 45, 61, DateTimeKind.Local).AddTicks(7628), new DateTime(2021, 5, 31, 8, 40, 45, 60, DateTimeKind.Local).AddTicks(7021), new DateTime(2021, 5, 31, 8, 40, 45, 60, DateTimeKind.Local).AddTicks(6087), new DateTime(2021, 5, 31, 8, 40, 45, 61, DateTimeKind.Local).AddTicks(8408) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 8, 40, 45, 62, DateTimeKind.Local).AddTicks(247), new DateTime(2021, 5, 31, 8, 40, 45, 62, DateTimeKind.Local).AddTicks(170), new DateTime(2021, 5, 31, 8, 40, 45, 62, DateTimeKind.Local).AddTicks(157), new DateTime(2021, 5, 31, 8, 40, 45, 62, DateTimeKind.Local).AddTicks(259) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ae9dad4-8f71-41a0-b4ab-bcda2efcc462");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcaf3e31-2245-4102-86c2-81af14607bea");

            migrationBuilder.AlterColumn<decimal>(
                name: "RemittanceMonthlyAmount",
                table: "Loan",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OfficeUseAmount",
                table: "Loan",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BusinessMonthlyIncomeNet",
                table: "Loan",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BusinessMonthlyIncomeGross",
                table: "Loan",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BusinessCapital",
                table: "Loan",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

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
    }
}
