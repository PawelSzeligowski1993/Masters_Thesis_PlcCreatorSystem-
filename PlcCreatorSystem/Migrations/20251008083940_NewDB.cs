using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlcCreatorSystem_API.Migrations
{
    /// <inheritdoc />
    public partial class NewDB : Migration
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

            migrationBuilder.CreateTable(
                name: "HMIs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identyfier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HMIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HMIs_LocalUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "LocalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PLCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subnet_X1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IP_X1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subnet_X2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IP_X2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identyfier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PLCs_LocalUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "LocalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PlcID = table.Column<int>(type: "int", nullable: false),
                    HmiID = table.Column<int>(type: "int", nullable: false),
                    CustomerDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_HMIs_HmiID",
                        column: x => x.HmiID,
                        principalTable: "HMIs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_LocalUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "LocalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_PLCs_PlcID",
                        column: x => x.PlcID,
                        principalTable: "PLCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LocalUsers",
                columns: new[] { "Id", "Name", "Password", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "Pawel", "Pawel123", "admin", "PawelAdmin" },
                    { 2, "Jan", "Jan123", "admin", "JanAdmin" },
                    { 3, "Adam", "Adam123", "engineer", "AdamEngineer" },
                    { 4, "Robert", "Robert123", "custom", "RobertCustom" }
                });

            migrationBuilder.InsertData(
                table: "HMIs",
                columns: new[] { "Id", "CreatedDate", "Details", "IP", "Identyfier", "Name", "UpdatedDate", "UserID" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4576), "HMI1 - TEST", "10.101.10.100", "6AV2 124-0UC02-0AX0/17.0.0.0", "HMI1", new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4578), 1 },
                    { 2, new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4582), "HMI2 - TEST", "10.102.10.100", "6AV2 124-0UC02-0AX0/17.0.0.0", "HMI2", new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4583), 1 },
                    { 3, new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4585), "HMI3 - TEST, fill database HMI3, and Project3", "10.103.10.100", "6AV2 124-0UC02-0AX0/17.0.0.0", "HMI3", new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4587), 1 }
                });

            migrationBuilder.InsertData(
                table: "PLCs",
                columns: new[] { "Id", "CreatedDate", "Details", "IP_X1", "IP_X2", "Identyfier", "Name", "Subnet_X1", "Subnet_X2", "UpdatedDate", "UserID" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4498), "PLC1 - This is TEST", "10.101.10.11", "10.100.10.10", "6ES7 516-3AN02-0AB0/V2.9", "PLC1", "Network1_PLC1", "Network2_PLC1", new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4545), 1 },
                    { 2, new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4550), "PLC2 - This is TEST", "10.102.10.21", "10.100.10.20", "6ES7 516-3AN02-0AB0/V2.9", "PLC2", "Network1_PLC2", "Network2_PLC2", new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4551), 1 },
                    { 3, new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4554), "PLC3 - This is TEST, fill database PLC3, and Project3", "10.103.10.31", "10.100.10.30", "6ES7 516-3AN02-0AB0/V2.9", "PLC3", "Network1_PLC3", "Network2_PLC3", new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4556), 1 }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedDate", "CustomerDetails", "HmiID", "Name", "PlcID", "Status", "UpdatedDate", "UserID" },
                values: new object[] { 1, new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4602), "Firma Krzak", 3, "Project1", 3, 3, new DateTime(2025, 10, 8, 10, 39, 40, 449, DateTimeKind.Local).AddTicks(4604), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_HMIs_UserID",
                table: "HMIs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PLCs_UserID",
                table: "PLCs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_HmiID",
                table: "Projects",
                column: "HmiID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PlcID",
                table: "Projects",
                column: "PlcID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserID",
                table: "Projects",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "HMIs");

            migrationBuilder.DropTable(
                name: "PLCs");

            migrationBuilder.DropTable(
                name: "LocalUsers");
        }
    }
}
