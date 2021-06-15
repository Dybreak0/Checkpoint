using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "015330a8-59c1-40a1-83a8-ab136287f3b4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b34843a-0f54-4421-83af-fce5abebbd43");

            migrationBuilder.CreateTable(
                name: "LoanSourceOfIncome",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SourceType = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanSourceOfIncome", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 12, 14, 24, 23, 547, DateTimeKind.Local).AddTicks(2987), new DateTime(2021, 6, 12, 14, 24, 23, 548, DateTimeKind.Local).AddTicks(1095) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d065fbca-3d1e-4885-86ad-9fec82307502", "28facc8a-7a93-4790-819d-035343583383", "Administrator", "ADMINISTRATOR" },
                    { "06525815-3284-43ff-80a1-ebb9f680dc30", "4b59678a-b39a-44b2-b19f-1c7daabb9977", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 12, 14, 24, 23, 550, DateTimeKind.Local).AddTicks(5070), new DateTime(2021, 6, 12, 14, 24, 23, 550, DateTimeKind.Local).AddTicks(5863) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 12, 14, 24, 23, 550, DateTimeKind.Local).AddTicks(6666), new DateTime(2021, 6, 12, 14, 24, 23, 550, DateTimeKind.Local).AddTicks(6673) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 12, 14, 24, 23, 549, DateTimeKind.Local).AddTicks(5641), new DateTime(2021, 6, 12, 14, 24, 23, 549, DateTimeKind.Local).AddTicks(891), new DateTime(2021, 6, 12, 14, 24, 23, 549, DateTimeKind.Local).AddTicks(454), new DateTime(2021, 6, 12, 14, 24, 23, 549, DateTimeKind.Local).AddTicks(6422) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 12, 14, 24, 23, 549, DateTimeKind.Local).AddTicks(7601), new DateTime(2021, 6, 12, 14, 24, 23, 549, DateTimeKind.Local).AddTicks(7576), new DateTime(2021, 6, 12, 14, 24, 23, 549, DateTimeKind.Local).AddTicks(7572), new DateTime(2021, 6, 12, 14, 24, 23, 549, DateTimeKind.Local).AddTicks(7605) });

            migrationBuilder.InsertData(
                table: "LoanSourceOfIncome",
                columns: new[] { "ID", "SourceType" },
                values: new object[,]
                {
                    { 1, "Employed" },
                    { 2, "Self Employed" },
                    { 3, "Own Business" },
                    { 4, "Pension" },
                    { 5, "Remittance" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanSourceOfIncome");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06525815-3284-43ff-80a1-ebb9f680dc30");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d065fbca-3d1e-4885-86ad-9fec82307502");

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
    }
}
