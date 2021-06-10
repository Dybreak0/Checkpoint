using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class RenameToFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanApplicationType");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50fc633a-1596-4dc7-9606-f6845cffd5be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb4f8638-208e-45e1-bcad-07d664ed0964");

            migrationBuilder.RenameColumn(
                name: "Filename",
                table: "LoanAttachment",
                newName: "FileName");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7789bf02-05a5-4af8-b9b1-e04a1e3e6a4c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "856daa55-14a0-4439-a157-94ade1663091");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "LoanAttachment",
                newName: "Filename");

            migrationBuilder.CreateTable(
                name: "LoanApplicationType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationName = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanApplicationType", x => x.ID);
                });

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

            migrationBuilder.InsertData(
                table: "LoanApplicationType",
                columns: new[] { "ID", "ApplicationName" },
                values: new object[,]
                {
                    { 1, "E-Commerce" },
                    { 2, "Checkpoint" }
                });
        }
    }
}
