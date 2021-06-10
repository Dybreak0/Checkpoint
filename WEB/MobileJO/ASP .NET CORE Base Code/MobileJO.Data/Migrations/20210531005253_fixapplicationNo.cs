using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class fixapplicationNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ae9dad4-8f71-41a0-b4ab-bcda2efcc462");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcaf3e31-2245-4102-86c2-81af14607bea");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationNumber",
                table: "Loan",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(9)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 8, 52, 52, 891, DateTimeKind.Local).AddTicks(5900), new DateTime(2021, 5, 31, 8, 52, 52, 892, DateTimeKind.Local).AddTicks(1947) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1bcebdff-cf51-4052-89e0-575670c6123c", "f912c224-6ebf-4f3d-a03a-1e89a9597269", "Administrator", "ADMINISTRATOR" },
                    { "8f5b680e-824c-4d0f-9656-dd7898dbcb49", "a2114ff3-f036-4c23-9136-38d986005b13", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 8, 52, 52, 893, DateTimeKind.Local).AddTicks(8248), new DateTime(2021, 5, 31, 8, 52, 52, 893, DateTimeKind.Local).AddTicks(8631) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 8, 52, 52, 893, DateTimeKind.Local).AddTicks(8995), new DateTime(2021, 5, 31, 8, 52, 52, 893, DateTimeKind.Local).AddTicks(9001) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 8, 52, 52, 893, DateTimeKind.Local).AddTicks(1857), new DateTime(2021, 5, 31, 8, 52, 52, 892, DateTimeKind.Local).AddTicks(9793), new DateTime(2021, 5, 31, 8, 52, 52, 892, DateTimeKind.Local).AddTicks(9572), new DateTime(2021, 5, 31, 8, 52, 52, 893, DateTimeKind.Local).AddTicks(2179) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 5, 31, 8, 52, 52, 893, DateTimeKind.Local).AddTicks(2823), new DateTime(2021, 5, 31, 8, 52, 52, 893, DateTimeKind.Local).AddTicks(2789), new DateTime(2021, 5, 31, 8, 52, 52, 893, DateTimeKind.Local).AddTicks(2785), new DateTime(2021, 5, 31, 8, 52, 52, 893, DateTimeKind.Local).AddTicks(2829) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1bcebdff-cf51-4052-89e0-575670c6123c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f5b680e-824c-4d0f-9656-dd7898dbcb49");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationNumber",
                table: "Loan",
                type: "varchar(9)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
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
    }
}
