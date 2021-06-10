using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class removeListUDTC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2581bd7-3783-4fa1-834a-de08e094ec89");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df641525-56b0-4b38-9ae5-23089290c747");

            migrationBuilder.AddColumn<string>(
                name: "DesiredTCDownPayment",
                table: "Loan",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesiredTCMonthlyInstallment",
                table: "Loan",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesiredTCRemarks",
                table: "Loan",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesiredTCTerms",
                table: "Loan",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesiredTCTotalPrice",
                table: "Loan",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesiredTCTotalRebate",
                table: "Loan",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 7, 0, 42, 8, 762, DateTimeKind.Local).AddTicks(1812), new DateTime(2021, 6, 7, 0, 42, 8, 762, DateTimeKind.Local).AddTicks(7659) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "015330a8-59c1-40a1-83a8-ab136287f3b4", "ab408c90-5e28-4034-9b60-5fcb8acf0ff9", "Administrator", "ADMINISTRATOR" },
                    { "8b34843a-0f54-4421-83af-fce5abebbd43", "d9861d4f-40fa-4a3f-ba57-eb02d0095bac", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 7, 0, 42, 8, 764, DateTimeKind.Local).AddTicks(3339), new DateTime(2021, 6, 7, 0, 42, 8, 764, DateTimeKind.Local).AddTicks(3804) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 7, 0, 42, 8, 764, DateTimeKind.Local).AddTicks(4195), new DateTime(2021, 6, 7, 0, 42, 8, 764, DateTimeKind.Local).AddTicks(4202) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 7, 0, 42, 8, 763, DateTimeKind.Local).AddTicks(7326), new DateTime(2021, 6, 7, 0, 42, 8, 763, DateTimeKind.Local).AddTicks(5279), new DateTime(2021, 6, 7, 0, 42, 8, 763, DateTimeKind.Local).AddTicks(5065), new DateTime(2021, 6, 7, 0, 42, 8, 763, DateTimeKind.Local).AddTicks(7653) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 7, 0, 42, 8, 763, DateTimeKind.Local).AddTicks(8178), new DateTime(2021, 6, 7, 0, 42, 8, 763, DateTimeKind.Local).AddTicks(8143), new DateTime(2021, 6, 7, 0, 42, 8, 763, DateTimeKind.Local).AddTicks(8139), new DateTime(2021, 6, 7, 0, 42, 8, 763, DateTimeKind.Local).AddTicks(8183) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "015330a8-59c1-40a1-83a8-ab136287f3b4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b34843a-0f54-4421-83af-fce5abebbd43");

            migrationBuilder.DropColumn(
                name: "DesiredTCDownPayment",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "DesiredTCMonthlyInstallment",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "DesiredTCRemarks",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "DesiredTCTerms",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "DesiredTCTotalPrice",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "DesiredTCTotalRebate",
                table: "Loan");

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
    }
}
