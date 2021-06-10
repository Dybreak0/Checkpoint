using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileJO.Data.Migrations
{
    public partial class seedSelections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1cdc10b3-dcc0-4a16-a071-a862f6b9aa4b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48112c74-1d0e-4225-bbe2-aa7807cac200");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Loan",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.CreateTable(
                name: "LoanStatus",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanStatus", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 18, 49, 587, DateTimeKind.Local).AddTicks(5939), new DateTime(2021, 6, 1, 0, 18, 49, 588, DateTimeKind.Local).AddTicks(1745) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cc3aef5e-7ff5-4ad3-8864-448f8f6da0d0", "1de61438-5151-4093-8377-c7a52d68b20a", "Administrator", "ADMINISTRATOR" },
                    { "bda8a3a7-ad6b-477f-b58f-dd699110cdac", "22132b87-15f6-4a7f-af5f-a87694294017", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 18, 49, 591, DateTimeKind.Local).AddTicks(2272), new DateTime(2021, 6, 1, 0, 18, 49, 591, DateTimeKind.Local).AddTicks(2670) });

            migrationBuilder.UpdateData(
                table: "AssignedCase",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 18, 49, 591, DateTimeKind.Local).AddTicks(3099), new DateTime(2021, 6, 1, 0, 18, 49, 591, DateTimeKind.Local).AddTicks(3105) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(4795), new DateTime(2021, 6, 1, 0, 18, 49, 589, DateTimeKind.Local).AddTicks(9874), new DateTime(2021, 6, 1, 0, 18, 49, 589, DateTimeKind.Local).AddTicks(8585), new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(5266) });

            migrationBuilder.UpdateData(
                table: "JobOrder",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "DateTimeEnd", "DateTimeStart", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(5907), new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(5873), new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(5868), new DateTime(2021, 6, 1, 0, 18, 49, 590, DateTimeKind.Local).AddTicks(5913) });

            migrationBuilder.InsertData(
                table: "LoanApplicationType",
                columns: new[] { "ID", "ApplicationName" },
                values: new object[,]
                {
                    { 1, "E-Commerce" },
                    { 2, "Checkpoint" }
                });

            migrationBuilder.InsertData(
                table: "LoanStatus",
                columns: new[] { "ID", "Status" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Denied" },
                    { 3, "Approved" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanApplicationType");

            migrationBuilder.DropTable(
                name: "LoanStatus");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bda8a3a7-ad6b-477f-b58f-dd699110cdac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc3aef5e-7ff5-4ad3-8864-448f8f6da0d0");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Loan");

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
    }
}
