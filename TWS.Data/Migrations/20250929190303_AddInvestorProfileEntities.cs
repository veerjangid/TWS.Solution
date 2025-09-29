using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInvestorProfileEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestorProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvestorType = table.Column<int>(type: "int", nullable: false),
                    IsAccredited = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    AccreditationType = table.Column<int>(type: "int", nullable: true),
                    ProfileCompletionPercentage = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestorProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestorProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EntityInvestorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvestorProfileId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsUSCompany = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityInvestorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityInvestorDetails_InvestorProfiles_InvestorProfileId",
                        column: x => x.InvestorProfileId,
                        principalTable: "InvestorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IndividualInvestorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvestorProfileId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsUSCitizen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualInvestorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualInvestorDetails_InvestorProfiles_InvestorProfileId",
                        column: x => x.InvestorProfileId,
                        principalTable: "InvestorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IRAInvestorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvestorProfileId = table.Column<int>(type: "int", nullable: false),
                    IRAType = table.Column<int>(type: "int", nullable: false),
                    NameOfIRA = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsUSCitizen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IRAInvestorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IRAInvestorDetails_InvestorProfiles_InvestorProfileId",
                        column: x => x.InvestorProfileId,
                        principalTable: "InvestorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JointInvestorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvestorProfileId = table.Column<int>(type: "int", nullable: false),
                    IsJointInvestment = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    JointAccountType = table.Column<int>(type: "int", nullable: false),
                    PrimaryFirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrimaryLastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrimaryIsUSCitizen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SecondaryFirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecondaryLastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecondaryIsUSCitizen = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JointInvestorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JointInvestorDetails_InvestorProfiles_InvestorProfileId",
                        column: x => x.InvestorProfileId,
                        principalTable: "InvestorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrustInvestorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvestorProfileId = table.Column<int>(type: "int", nullable: false),
                    TrustName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsUSTrust = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TrustType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrustInvestorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrustInvestorDetails_InvestorProfiles_InvestorProfileId",
                        column: x => x.InvestorProfileId,
                        principalTable: "InvestorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "59d6c945-ed4a-4967-8f37-1bc1cc0034c5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "d0653f07-c62b-4a43-a7a7-811654197b7e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "0f39aab8-1c6a-4c5a-93a9-5a5da29c8242");

            migrationBuilder.CreateIndex(
                name: "IX_EntityInvestorDetails_EntityType",
                table: "EntityInvestorDetails",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_EntityInvestorDetails_InvestorProfileId_Unique",
                table: "EntityInvestorDetails",
                column: "InvestorProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndividualInvestorDetails_InvestorProfileId_Unique",
                table: "IndividualInvestorDetails",
                column: "InvestorProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestorProfiles_InvestorType",
                table: "InvestorProfiles",
                column: "InvestorType");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorProfiles_IsAccredited",
                table: "InvestorProfiles",
                column: "IsAccredited");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorProfiles_IsActive",
                table: "InvestorProfiles",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorProfiles_UserId_Unique",
                table: "InvestorProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IRAInvestorDetails_InvestorProfileId_Unique",
                table: "IRAInvestorDetails",
                column: "InvestorProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IRAInvestorDetails_IRAType",
                table: "IRAInvestorDetails",
                column: "IRAType");

            migrationBuilder.CreateIndex(
                name: "IX_JointInvestorDetails_InvestorProfileId_Unique",
                table: "JointInvestorDetails",
                column: "InvestorProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JointInvestorDetails_JointAccountType",
                table: "JointInvestorDetails",
                column: "JointAccountType");

            migrationBuilder.CreateIndex(
                name: "IX_TrustInvestorDetails_InvestorProfileId_Unique",
                table: "TrustInvestorDetails",
                column: "InvestorProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrustInvestorDetails_TrustType",
                table: "TrustInvestorDetails",
                column: "TrustType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityInvestorDetails");

            migrationBuilder.DropTable(
                name: "IndividualInvestorDetails");

            migrationBuilder.DropTable(
                name: "IRAInvestorDetails");

            migrationBuilder.DropTable(
                name: "JointInvestorDetails");

            migrationBuilder.DropTable(
                name: "TrustInvestorDetails");

            migrationBuilder.DropTable(
                name: "InvestorProfiles");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "6913ae52-8d11-4e45-8ac5-623fd7642a84");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "f5226182-4ebb-43c3-8aae-a8e88fb32279");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "ab54220d-4f65-4955-80e8-ff4574adcdf8");
        }
    }
}
