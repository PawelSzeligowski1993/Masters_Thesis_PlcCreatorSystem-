using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlcCreatorSystem_API.Migrations
{
    /// <inheritdoc />
    public partial class addUsersToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUsers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "HMIs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 10, 3, 14, 12, 46, 262, DateTimeKind.Local).AddTicks(6850));

            migrationBuilder.UpdateData(
                table: "HMIs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 10, 3, 14, 12, 46, 262, DateTimeKind.Local).AddTicks(6854));

            migrationBuilder.UpdateData(
                table: "HMIs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 10, 3, 14, 12, 46, 262, DateTimeKind.Local).AddTicks(6857));

            migrationBuilder.UpdateData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 3, 14, 12, 46, 262, DateTimeKind.Local).AddTicks(6661), new DateTime(2025, 10, 3, 14, 12, 46, 262, DateTimeKind.Local).AddTicks(6706) });

            migrationBuilder.UpdateData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 3, 14, 12, 46, 262, DateTimeKind.Local).AddTicks(6709), new DateTime(2025, 10, 3, 14, 12, 46, 262, DateTimeKind.Local).AddTicks(6711) });

            migrationBuilder.UpdateData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 3, 14, 12, 46, 262, DateTimeKind.Local).AddTicks(6713), new DateTime(2025, 10, 3, 14, 12, 46, 262, DateTimeKind.Local).AddTicks(6715) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 3, 14, 12, 46, 262, DateTimeKind.Local).AddTicks(6877), new DateTime(2025, 10, 3, 14, 12, 46, 262, DateTimeKind.Local).AddTicks(6879) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalUsers");

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

            migrationBuilder.UpdateData(
                table: "HMIs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5270));

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

            migrationBuilder.UpdateData(
                table: "PLCs",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5126), new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5128) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5290), new DateTime(2025, 9, 18, 10, 3, 37, 240, DateTimeKind.Local).AddTicks(5292) });
        }
    }
}
