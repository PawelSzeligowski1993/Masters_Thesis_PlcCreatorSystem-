using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlcCreatorSystem_API.Migrations
{
    /// <inheritdoc />
    public partial class Fill_PLC_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PLCs",
                columns: new[] { "Id", "CreatedDate", "Details", "IP_X1", "IP_X2", "Identyfier", "Name", "Subnet_X1", "Subnet_X2", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 18, 9, 26, 21, 313, DateTimeKind.Local).AddTicks(8959), "PLC1 - This is TEST", "10.101.10.11", "10.100.10.10", "6ES7 516-3AN02-0AB0/V2.9", "PLC1", "Network1_PLC1", "Network2_PLC1", new DateTime(2025, 9, 18, 9, 26, 21, 313, DateTimeKind.Local).AddTicks(9006) },
                    { 2, new DateTime(2025, 9, 18, 9, 26, 21, 313, DateTimeKind.Local).AddTicks(9010), "PLC2 - This is TEST", "10.102.10.21", "10.100.10.20", "6ES7 516-3AN02-0AB0/V2.9", "PLC2", "Network1_PLC2", "Network2_PLC2", new DateTime(2025, 9, 18, 9, 26, 21, 313, DateTimeKind.Local).AddTicks(9013) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
