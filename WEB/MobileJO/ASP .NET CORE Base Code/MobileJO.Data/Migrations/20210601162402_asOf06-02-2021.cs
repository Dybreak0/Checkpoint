using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class asOf06022021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e61fd0c-b5b0-48f3-92bb-41239593d72d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a197a8e-2931-4ff6-9563-e905592ec7d9");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85b0e608-a636-439c-9f30-321264335f7c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fbe22bed-f86d-4731-b69c-63570da92ec8");

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
    }
}
