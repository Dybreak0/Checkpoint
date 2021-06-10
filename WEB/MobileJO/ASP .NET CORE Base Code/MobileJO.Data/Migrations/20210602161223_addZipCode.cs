using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class addZipCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85b0e608-a636-439c-9f30-321264335f7c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fbe22bed-f86d-4731-b69c-63570da92ec8");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Loan",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 0, 12, 22, 531, DateTimeKind.Local).AddTicks(7297), new DateTime(2021, 6, 3, 0, 12, 22, 532, DateTimeKind.Local).AddTicks(5521) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "df4174a3-ed52-4d4f-a771-b6edc4329500", "20044e2b-74f1-4a81-b8be-7170d046644a", "Administrator", "ADMINISTRATOR" },
                    { "e660c2af-200f-4497-a5e8-53fd184b042d", "e2b08078-ea41-4208-884a-1a0d68ba083b", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 0, 12, 22, 535, DateTimeKind.Local).AddTicks(3637), new DateTime(2021, 6, 3, 0, 12, 22, 535, DateTimeKind.Local).AddTicks(4218) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 0, 12, 22, 535, DateTimeKind.Local).AddTicks(4830), new DateTime(2021, 6, 3, 0, 12, 22, 535, DateTimeKind.Local).AddTicks(4840) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 0, 12, 22, 534, DateTimeKind.Local).AddTicks(3225), new DateTime(2021, 6, 3, 0, 12, 22, 533, DateTimeKind.Local).AddTicks(9473), new DateTime(2021, 6, 3, 0, 12, 22, 533, DateTimeKind.Local).AddTicks(8850), new DateTime(2021, 6, 3, 0, 12, 22, 534, DateTimeKind.Local).AddTicks(3737) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 0, 12, 22, 534, DateTimeKind.Local).AddTicks(4596), new DateTime(2021, 6, 3, 0, 12, 22, 534, DateTimeKind.Local).AddTicks(4537), new DateTime(2021, 6, 3, 0, 12, 22, 534, DateTimeKind.Local).AddTicks(4530), new DateTime(2021, 6, 3, 0, 12, 22, 534, DateTimeKind.Local).AddTicks(4606) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df4174a3-ed52-4d4f-a771-b6edc4329500");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e660c2af-200f-4497-a5e8-53fd184b042d");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Loan");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 2, 0, 24, 1, 459, DateTimeKind.Local).AddTicks(8068), new DateTime(2021, 6, 2, 0, 24, 1, 460, DateTimeKind.Local).AddTicks(5229) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "fbe22bed-f86d-4731-b69c-63570da92ec8", "a47601df-56bb-47c2-a038-4909bf8ab753", "Administrator", "ADMINISTRATOR" },
                    { "85b0e608-a636-439c-9f30-321264335f7c", "229d8c94-f45c-44e2-95d9-520cb490ee34", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 2, 0, 24, 1, 462, DateTimeKind.Local).AddTicks(2882), new DateTime(2021, 6, 2, 0, 24, 1, 462, DateTimeKind.Local).AddTicks(3267) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 2, 0, 24, 1, 462, DateTimeKind.Local).AddTicks(3639), new DateTime(2021, 6, 2, 0, 24, 1, 462, DateTimeKind.Local).AddTicks(3646) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 2, 0, 24, 1, 461, DateTimeKind.Local).AddTicks(5915), new DateTime(2021, 6, 2, 0, 24, 1, 461, DateTimeKind.Local).AddTicks(3811), new DateTime(2021, 6, 2, 0, 24, 1, 461, DateTimeKind.Local).AddTicks(3585), new DateTime(2021, 6, 2, 0, 24, 1, 461, DateTimeKind.Local).AddTicks(6245) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 2, 0, 24, 1, 461, DateTimeKind.Local).AddTicks(6777), new DateTime(2021, 6, 2, 0, 24, 1, 461, DateTimeKind.Local).AddTicks(6742), new DateTime(2021, 6, 2, 0, 24, 1, 461, DateTimeKind.Local).AddTicks(6738), new DateTime(2021, 6, 2, 0, 24, 1, 461, DateTimeKind.Local).AddTicks(6783) });
        }
    }
}
