using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneralInfoEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityGeneralInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntityInvestorDetailId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsUSCompany = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EntityType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfFormation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PurposeOfFormation = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TINEIN = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HasOperatingAgreement = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityGeneralInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityGeneralInfo_EntityInvestorDetails_EntityInvestorDetail~",
                        column: x => x.EntityInvestorDetailId,
                        principalTable: "EntityInvestorDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IndividualGeneralInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IndividualInvestorDetailId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SSN = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DriverLicensePath = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    W9Path = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualGeneralInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualGeneralInfo_IndividualInvestorDetails_IndividualIn~",
                        column: x => x.IndividualInvestorDetailId,
                        principalTable: "IndividualInvestorDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IRAGeneralInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IRAInvestorDetailId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SSN = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustodianName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IRAAccountNumber = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRollingOverToCNB = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CustodianPhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustodianFaxNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HasLiquidatedAssets = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IRAGeneralInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IRAGeneralInfo_IRAInvestorDetails_IRAInvestorDetailId",
                        column: x => x.IRAInvestorDetailId,
                        principalTable: "IRAInvestorDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JointGeneralInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JointInvestorDetailId = table.Column<int>(type: "int", nullable: false),
                    IsJointInvestment = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    JointAccountType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JointGeneralInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JointGeneralInfo_JointInvestorDetails_JointInvestorDetailId",
                        column: x => x.JointInvestorDetailId,
                        principalTable: "JointInvestorDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrustGeneralInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrustInvestorDetailId = table.Column<int>(type: "int", nullable: false),
                    TrustName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsUSTrust = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TrustType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfFormation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PurposeOfFormation = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TINEIN = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrustGeneralInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrustGeneralInfo_TrustInvestorDetails_TrustInvestorDetailId",
                        column: x => x.TrustInvestorDetailId,
                        principalTable: "TrustInvestorDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EntityEquityOwners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntityGeneralInfoId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityEquityOwners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityEquityOwners_EntityGeneralInfo_EntityGeneralInfoId",
                        column: x => x.EntityGeneralInfoId,
                        principalTable: "EntityGeneralInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JointAccountHolders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JointGeneralInfoId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SSN = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JointAccountHolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JointAccountHolders_JointGeneralInfo_JointGeneralInfoId",
                        column: x => x.JointGeneralInfoId,
                        principalTable: "JointGeneralInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrustGrantors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrustGeneralInfoId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrustGrantors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrustGrantors_TrustGeneralInfo_TrustGeneralInfoId",
                        column: x => x.TrustGeneralInfoId,
                        principalTable: "TrustGeneralInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_EntityEquityOwners_EntityGeneralInfoId",
                table: "EntityEquityOwners",
                column: "EntityGeneralInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityGeneralInfo_EntityInvestorDetailId",
                table: "EntityGeneralInfo",
                column: "EntityInvestorDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntityGeneralInfo_EntityType",
                table: "EntityGeneralInfo",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualGeneralInfo_Email",
                table: "IndividualGeneralInfo",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualGeneralInfo_IndividualInvestorDetailId",
                table: "IndividualGeneralInfo",
                column: "IndividualInvestorDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IRAGeneralInfo_AccountType",
                table: "IRAGeneralInfo",
                column: "AccountType");

            migrationBuilder.CreateIndex(
                name: "IX_IRAGeneralInfo_Email",
                table: "IRAGeneralInfo",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_IRAGeneralInfo_IRAInvestorDetailId",
                table: "IRAGeneralInfo",
                column: "IRAInvestorDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JointAccountHolders_Email",
                table: "JointAccountHolders",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_JointAccountHolders_JointGeneralInfoId",
                table: "JointAccountHolders",
                column: "JointGeneralInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_JointAccountHolders_OrderIndex",
                table: "JointAccountHolders",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_JointGeneralInfo_JointInvestorDetailId",
                table: "JointGeneralInfo",
                column: "JointInvestorDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrustGeneralInfo_TrustInvestorDetailId",
                table: "TrustGeneralInfo",
                column: "TrustInvestorDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrustGeneralInfo_TrustType",
                table: "TrustGeneralInfo",
                column: "TrustType");

            migrationBuilder.CreateIndex(
                name: "IX_TrustGrantors_TrustGeneralInfoId",
                table: "TrustGrantors",
                column: "TrustGeneralInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityEquityOwners");

            migrationBuilder.DropTable(
                name: "IndividualGeneralInfo");

            migrationBuilder.DropTable(
                name: "IRAGeneralInfo");

            migrationBuilder.DropTable(
                name: "JointAccountHolders");

            migrationBuilder.DropTable(
                name: "TrustGrantors");

            migrationBuilder.DropTable(
                name: "EntityGeneralInfo");

            migrationBuilder.DropTable(
                name: "JointGeneralInfo");

            migrationBuilder.DropTable(
                name: "TrustGeneralInfo");

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
        }
    }
}
