using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlcCreatorSystem_API.Migrations
{
    /// <inheritdoc />
    public partial class Fill_HMI_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "HMIs",
                columns: new[] { "Id", "CreatedDate", "Details", "IP", "Identyfier", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 18, 9, 41, 41, 777, DateTimeKind.Local).AddTicks(8865), "HMI1 - TEST", "10.101.10.100", "6AV2 124-0UC02-0AX0/17.0.0.0", "HMI1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2025, 9, 18, 9, 41, 41, 777, DateTimeKind.Local).AddTicks(8869), "HMI2 - TEST", "10.102.10.100", "6AV2 124-0UC02-0AX0/17.0.0.0", "HMI2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HMIs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "HMIs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 9, 18, 9, 26, 21, 313, DateTimeKind.Local).AddTicks(8959), new DateTime(2025, 9, 18, 9, 26, 21, 313, DateTimeKind.Local).AddTicks(9006) });

            migrationBuilder.UpdateData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 9, 18, 9, 26, 21, 313, DateTimeKind.Local).AddTicks(9010), new DateTime(2025, 9, 18, 9, 26, 21, 313, DateTimeKind.Local).AddTicks(9013) });
        }
    }
}
