using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class removeSampleColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44f0b7b4-b63a-4746-9326-069c37185d1f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71bf9fa2-87d2-4bca-8dac-b5076a66df5f");

            migrationBuilder.DropColumn(
                name: "SampleColumn",
                table: "Loan");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d380dcc-fb96-440b-a138-aee88b553c3a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5666c2ce-2929-4d41-9f3a-6de7ed7a9fbd");

            migrationBuilder.AddColumn<string>(
                name: "SampleColumn",
                table: "Loan",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 4, 14, 10, 49, 196, DateTimeKind.Local).AddTicks(2095), new DateTime(2021, 6, 4, 14, 10, 49, 196, DateTimeKind.Local).AddTicks(9460) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "44f0b7b4-b63a-4746-9326-069c37185d1f", "7d830c9f-f81e-4c88-bfe2-67d2c9e6812e", "Administrator", "ADMINISTRATOR" },
                    { "71bf9fa2-87d2-4bca-8dac-b5076a66df5f", "5ec2e868-bc28-4e8b-9f00-81003256bdb2", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 4, 14, 10, 49, 200, DateTimeKind.Local).AddTicks(5440), new DateTime(2021, 6, 4, 14, 10, 49, 200, DateTimeKind.Local).AddTicks(6123) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 4, 14, 10, 49, 200, DateTimeKind.Local).AddTicks(9200), new DateTime(2021, 6, 4, 14, 10, 49, 200, DateTimeKind.Local).AddTicks(9214) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 4, 14, 10, 49, 198, DateTimeKind.Local).AddTicks(6520), new DateTime(2021, 6, 4, 14, 10, 49, 198, DateTimeKind.Local).AddTicks(2982), new DateTime(2021, 6, 4, 14, 10, 49, 198, DateTimeKind.Local).AddTicks(2656), new DateTime(2021, 6, 4, 14, 10, 49, 198, DateTimeKind.Local).AddTicks(7178) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 4, 14, 10, 49, 198, DateTimeKind.Local).AddTicks(8076), new DateTime(2021, 6, 4, 14, 10, 49, 198, DateTimeKind.Local).AddTicks(8021), new DateTime(2021, 6, 4, 14, 10, 49, 198, DateTimeKind.Local).AddTicks(8014), new DateTime(2021, 6, 4, 14, 10, 49, 198, DateTimeKind.Local).AddTicks(8084) });
        }
    }
}
