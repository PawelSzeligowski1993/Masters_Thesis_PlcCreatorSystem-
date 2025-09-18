using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlcCreatorSystem_API.Migrations
{
    /// <inheritdoc />
    public partial class Fill_Project_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "HMIs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5265));

            migrationBuilder.UpdateData(
                table: "HMIs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5268));

            migrationBuilder.InsertData(
                table: "HMIs",
                columns: new[] { "Id", "CreatedDate", "Details", "IP", "Identyfier", "Name", "UpdatedDate" },
                values: new object[] { 3, new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5270), "HMI3 - TEST, fill database HMI3, and Project3", "10.103.10.100", "6AV2 124-0UC02-0AX0/17.0.0.0", "HMI3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5070), new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5116) });

            migrationBuilder.UpdateData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5120), new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5122) });

            migrationBuilder.InsertData(
                table: "PLCs",
                columns: new[] { "Id", "CreatedDate", "Details", "IP_X1", "IP_X2", "Identyfier", "Name", "Subnet_X1", "Subnet_X2", "UpdatedDate" },
                values: new object[] { 3, new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5126), "PLC3 - This is TEST, fill database PLC3, and Project3", "10.103.10.31", "10.100.10.30", "6ES7 516-3AN02-0AB0/V2.9", "PLC3", "Network1_PLC3", "Network2_PLC3", new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5128) });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedDate", "CustomerDetails", "HmiID", "Name", "PlcID", "Status", "UpdatedDate" },
                values: new object[] { 1, new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5290), "Firma Krzak", 3, "Project1", 3, 3, new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5292) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "HMIs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "HMIs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 18, 9, 41, 41, 777, DateTimeKind.Local).AddTicks(8865));

            migrationBuilder.UpdateData(
                table: "HMIs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 18, 9, 41, 41, 777, DateTimeKind.Local).AddTicks(8869));

            migrationBuilder.UpdateData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 9, 18, 9, 41, 41, 777, DateTimeKind.Local).AddTicks(8671), new DateTime(2025, 9, 18, 9, 41, 41, 777, DateTimeKind.Local).AddTicks(8712) });

            migrationBuilder.UpdateData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 9, 18, 9, 41, 41, 777, DateTimeKind.Local).AddTicks(8715), new DateTime(2025, 9, 18, 9, 41, 41, 777, DateTimeKind.Local).AddTicks(8717) });
        }
    }
}
