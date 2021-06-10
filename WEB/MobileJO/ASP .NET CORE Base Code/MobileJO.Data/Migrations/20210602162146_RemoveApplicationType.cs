using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class RemoveApplicationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "ApplicationType",
                table: "Loan");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 0, 21, 45, 722, DateTimeKind.Local).AddTicks(167), new DateTime(2021, 6, 3, 0, 21, 45, 723, DateTimeKind.Local).AddTicks(1036) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "50fc633a-1596-4dc7-9606-f6845cffd5be", "f0fd150d-8f0a-4a91-958f-75c56f562708", "Administrator", "ADMINISTRATOR" },
                    { "cb4f8638-208e-45e1-bcad-07d664ed0964", "7ed1fc46-16d6-40b6-92a9-65722b2c477a", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 0, 21, 45, 726, DateTimeKind.Local).AddTicks(4723), new DateTime(2021, 6, 3, 0, 21, 45, 726, DateTimeKind.Local).AddTicks(5303) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 0, 21, 45, 726, DateTimeKind.Local).AddTicks(5908), new DateTime(2021, 6, 3, 0, 21, 45, 726, DateTimeKind.Local).AddTicks(5924) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 0, 21, 45, 725, DateTimeKind.Local).AddTicks(1941), new DateTime(2021, 6, 3, 0, 21, 45, 724, DateTimeKind.Local).AddTicks(8381), new DateTime(2021, 6, 3, 0, 21, 45, 724, DateTimeKind.Local).AddTicks(7983), new DateTime(2021, 6, 3, 0, 21, 45, 725, DateTimeKind.Local).AddTicks(2479) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 0, 21, 45, 725, DateTimeKind.Local).AddTicks(3354), new DateTime(2021, 6, 3, 0, 21, 45, 725, DateTimeKind.Local).AddTicks(3292), new DateTime(2021, 6, 3, 0, 21, 45, 725, DateTimeKind.Local).AddTicks(3286), new DateTime(2021, 6, 3, 0, 21, 45, 725, DateTimeKind.Local).AddTicks(3363) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50fc633a-1596-4dc7-9606-f6845cffd5be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb4f8638-208e-45e1-bcad-07d664ed0964");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationType",
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
    }
}
