using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInvestmentTrackerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestmentTrackers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OfferingId = table.Column<int>(type: "int", nullable: false),
                    InvestorProfileId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LeadOwnerLicensedRep = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Relationship = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestmentType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestmentHeldInNamesOf = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateInvestmentClosed = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OriginalEquityInvestmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTWSAUM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RepCommissionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TWSRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DSTRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AltRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxStrategyRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OilAndGasRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InitialVsRecurringRevenue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MarketingSource = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReferredBy = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AltAUM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DSTAUM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OGAUM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxStrategyAUM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentTrackers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentTrackers_InvestorProfiles_InvestorProfileId",
                        column: x => x.InvestorProfileId,
                        principalTable: "InvestorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentTrackers_Offerings_OfferingId",
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
                value: "29560697-e9eb-4b48-b483-44bb1eddaeb5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "76fb1abf-4df2-44ed-b334-92fb8d440812");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "2bdae172-e3cb-49fb-b023-7d7e6983b425");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTrackers_InvestmentType",
                table: "InvestmentTrackers",
                column: "InvestmentType");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTrackers_InvestorProfileId",
                table: "InvestmentTrackers",
                column: "InvestorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTrackers_LeadOwnerLicensedRep",
                table: "InvestmentTrackers",
                column: "LeadOwnerLicensedRep");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTrackers_OfferingId",
                table: "InvestmentTrackers",
                column: "OfferingId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTrackers_OfferingId_InvestorProfileId",
                table: "InvestmentTrackers",
                columns: new[] { "OfferingId", "InvestorProfileId" });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTrackers_Status",
                table: "InvestmentTrackers",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentTrackers");

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
        }
    }
}
