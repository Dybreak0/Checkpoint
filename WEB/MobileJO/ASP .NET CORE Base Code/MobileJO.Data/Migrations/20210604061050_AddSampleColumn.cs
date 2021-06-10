using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class AddSampleColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7789bf02-05a5-4af8-b9b1-e04a1e3e6a4c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "856daa55-14a0-4439-a157-94ade1663091");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { new DateTime(2021, 6, 3, 2, 32, 59, 211, DateTimeKind.Local).AddTicks(9267), new DateTime(2021, 6, 3, 2, 32, 59, 213, DateTimeKind.Local).AddTicks(3365) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "856daa55-14a0-4439-a157-94ade1663091", "a2fbc5d9-5759-40aa-805d-5e011e602f35", "Administrator", "ADMINISTRATOR" },
                    { "7789bf02-05a5-4af8-b9b1-e04a1e3e6a4c", "9d0382ba-4119-4b1a-b79b-c3d4f86a4e6b", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 2, 32, 59, 216, DateTimeKind.Local).AddTicks(1840), new DateTime(2021, 6, 3, 2, 32, 59, 216, DateTimeKind.Local).AddTicks(2384) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 2, 32, 59, 216, DateTimeKind.Local).AddTicks(3021), new DateTime(2021, 6, 3, 2, 32, 59, 216, DateTimeKind.Local).AddTicks(3028) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 2, 32, 59, 215, DateTimeKind.Local).AddTicks(2439), new DateTime(2021, 6, 3, 2, 32, 59, 214, DateTimeKind.Local).AddTicks(9976), new DateTime(2021, 6, 3, 2, 32, 59, 214, DateTimeKind.Local).AddTicks(9668), new DateTime(2021, 6, 3, 2, 32, 59, 215, DateTimeKind.Local).AddTicks(2839) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 3, 2, 32, 59, 215, DateTimeKind.Local).AddTicks(3643), new DateTime(2021, 6, 3, 2, 32, 59, 215, DateTimeKind.Local).AddTicks(3608), new DateTime(2021, 6, 3, 2, 32, 59, 215, DateTimeKind.Local).AddTicks(3603), new DateTime(2021, 6, 3, 2, 32, 59, 215, DateTimeKind.Local).AddTicks(3649) });
        }
    }
}
