using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class BoolToStringLoanStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "245d2ce9-3ecf-4b31-8f8a-2ecb50338774");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e089ba03-1ac7-4297-b2c4-15b2fcf6a961");

            migrationBuilder.AlterColumn<string>(
                name: "LoanStatus",
                table: "Loan",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 13, 14, 29, 625, DateTimeKind.Local).AddTicks(3823), new DateTime(2021, 5, 31, 13, 14, 29, 626, DateTimeKind.Local).AddTicks(198) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "48112c74-1d0e-4225-bbe2-aa7807cac200", "8d92e729-107f-40ed-bce7-b0ad7a7b4a83", "Administrator", "ADMINISTRATOR" },
                    { "1cdc10b3-dcc0-4a16-a071-a862f6b9aa4b", "58a02e73-d6f1-4e3c-9129-353a95bb5e63", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 13, 14, 29, 627, DateTimeKind.Local).AddTicks(7253), new DateTime(2021, 5, 31, 13, 14, 29, 627, DateTimeKind.Local).AddTicks(7657) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 13, 14, 29, 627, DateTimeKind.Local).AddTicks(8065), new DateTime(2021, 5, 31, 13, 14, 29, 627, DateTimeKind.Local).AddTicks(8072) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 13, 14, 29, 627, DateTimeKind.Local).AddTicks(599), new DateTime(2021, 5, 31, 13, 14, 29, 626, DateTimeKind.Local).AddTicks(8494), new DateTime(2021, 5, 31, 13, 14, 29, 626, DateTimeKind.Local).AddTicks(8280), new DateTime(2021, 5, 31, 13, 14, 29, 627, DateTimeKind.Local).AddTicks(922) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 13, 14, 29, 627, DateTimeKind.Local).AddTicks(1445), new DateTime(2021, 5, 31, 13, 14, 29, 627, DateTimeKind.Local).AddTicks(1411), new DateTime(2021, 5, 31, 13, 14, 29, 627, DateTimeKind.Local).AddTicks(1407), new DateTime(2021, 5, 31, 13, 14, 29, 627, DateTimeKind.Local).AddTicks(1451) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1cdc10b3-dcc0-4a16-a071-a862f6b9aa4b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48112c74-1d0e-4225-bbe2-aa7807cac200");

            migrationBuilder.AlterColumn<bool>(
                name: "LoanStatus",
                table: "Loan",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

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
        }
    }
}
