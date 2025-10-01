using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOfferingAndInvestmentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offerings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Raising")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offerings", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InvestorInvestments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvestorProfileId = table.Column<int>(type: "int", nullable: false),
                    OfferingId = table.Column<int>(type: "int", nullable: false),
                    InvestmentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestorInvestments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestorInvestments_InvestorProfiles_InvestorProfileId",
                        column: x => x.InvestorProfileId,
                        principalTable: "InvestorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestorInvestments_Offerings_OfferingId",
                        column: x => x.OfferingId,
                        principalTable: "Offerings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "ce242276-695a-44a0-9943-fb22638ea537");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "9e62353d-8a95-4a44-af97-98e05125d63e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "93e2c2a1-57f0-4eed-82d6-072903ad2978");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorInvestments_InvestmentDate",
                table: "InvestorInvestments",
                column: "InvestmentDate");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorInvestments_InvestorProfileId",
                table: "InvestorInvestments",
                column: "InvestorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorInvestments_OfferingId",
                table: "InvestorInvestments",
                column: "OfferingId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorInvestments_Status",
                table: "InvestorInvestments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ_InvestorOffering",
                table: "InvestorInvestments",
                columns: new[] { "InvestorProfileId", "OfferingId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offerings_CreatedDate",
                table: "Offerings",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Offerings_Status",
                table: "Offerings",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestorInvestments");

            migrationBuilder.DropTable(
                name: "Offerings");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "27c9a66a-a0ff-4063-9a0c-19e7edf7c2bd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "a42039c5-2240-4f23-a77d-06fb2e477054");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "74625013-34c6-4403-8396-cd9d1a450917");
        }
    }
}
