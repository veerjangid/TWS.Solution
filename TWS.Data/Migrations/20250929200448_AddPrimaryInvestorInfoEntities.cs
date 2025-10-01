using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPrimaryInvestorInfoEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrimaryInvestorInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvestorProfileId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LegalStreetAddress = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Zip = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CellPhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsMarried = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SocialSecurityNumber = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DriversLicenseNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DriversLicenseExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Occupation = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployerName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RetiredProfession = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HasAlternateAddress = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AlternateAddress = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LowestIncomeLastTwoYears = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AnticipatedIncomeThisYear = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsRelyingOnJointIncome = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryInvestorInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimaryInvestorInfos_InvestorProfiles_InvestorProfileId",
                        column: x => x.InvestorProfileId,
                        principalTable: "InvestorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BrokerAffiliations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PrimaryInvestorInfoId = table.Column<int>(type: "int", nullable: false),
                    IsEmployeeOfBrokerDealer = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BrokerDealerName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRelatedToEmployee = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RelatedBrokerDealerName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployeeName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Relationship = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSeniorOfficer = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsManagerMemberExecutive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerAffiliations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrokerAffiliations_PrimaryInvestorInfos_PrimaryInvestorInfoId",
                        column: x => x.PrimaryInvestorInfoId,
                        principalTable: "PrimaryInvestorInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InvestmentExperiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PrimaryInvestorInfoId = table.Column<int>(type: "int", nullable: false),
                    AssetClass = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExperienceLevel = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OtherDescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentExperiences_PrimaryInvestorInfos_PrimaryInvestorIn~",
                        column: x => x.PrimaryInvestorInfoId,
                        principalTable: "PrimaryInvestorInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SourceOfFunds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PrimaryInvestorInfoId = table.Column<int>(type: "int", nullable: false),
                    SourceType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceOfFunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceOfFunds_PrimaryInvestorInfos_PrimaryInvestorInfoId",
                        column: x => x.PrimaryInvestorInfoId,
                        principalTable: "PrimaryInvestorInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaxRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PrimaryInvestorInfoId = table.Column<int>(type: "int", nullable: false),
                    TaxRateRange = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxRates_PrimaryInvestorInfos_PrimaryInvestorInfoId",
                        column: x => x.PrimaryInvestorInfoId,
                        principalTable: "PrimaryInvestorInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "0262155d-ff5c-4248-a380-aec370dde444");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "40da1a58-99ef-4b95-843d-322181f1c69e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "0e0c1ef5-6272-46f9-be21-8df6ffda3deb");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerAffiliations_PrimaryInvestorInfoId",
                table: "BrokerAffiliations",
                column: "PrimaryInvestorInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentExperiences_PrimaryInvestorInfoId",
                table: "InvestmentExperiences",
                column: "PrimaryInvestorInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentExperiences_PrimaryInvestorInfoId_AssetClass",
                table: "InvestmentExperiences",
                columns: new[] { "PrimaryInvestorInfoId", "AssetClass" });

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryInvestorInfos_Email",
                table: "PrimaryInvestorInfos",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryInvestorInfos_InvestorProfileId",
                table: "PrimaryInvestorInfos",
                column: "InvestorProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryInvestorInfos_LastName",
                table: "PrimaryInvestorInfos",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_SourceOfFunds_PrimaryInvestorInfoId",
                table: "SourceOfFunds",
                column: "PrimaryInvestorInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceOfFunds_PrimaryInvestorInfoId_SourceType",
                table: "SourceOfFunds",
                columns: new[] { "PrimaryInvestorInfoId", "SourceType" });

            migrationBuilder.CreateIndex(
                name: "IX_TaxRates_PrimaryInvestorInfoId",
                table: "TaxRates",
                column: "PrimaryInvestorInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRates_PrimaryInvestorInfoId_TaxRateRange",
                table: "TaxRates",
                columns: new[] { "PrimaryInvestorInfoId", "TaxRateRange" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerAffiliations");

            migrationBuilder.DropTable(
                name: "InvestmentExperiences");

            migrationBuilder.DropTable(
                name: "SourceOfFunds");

            migrationBuilder.DropTable(
                name: "TaxRates");

            migrationBuilder.DropTable(
                name: "PrimaryInvestorInfos");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "8b7d69d0-e2f1-4ac3-a96b-61ac854a04e9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "3bc8aaf4-c180-4f71-8b03-655fedf5c4d9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "cc8d155e-dd73-4b49-b9c7-81af4e74639b");
        }
    }
}
