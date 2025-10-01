using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFinancialTeamMemberEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialTeamMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvestorProfileId = table.Column<int>(type: "int", nullable: false),
                    MemberType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialTeamMembers_InvestorProfiles_InvestorProfileId",
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

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTeamMembers_InvestorProfileId",
                table: "FinancialTeamMembers",
                column: "InvestorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTeamMembers_InvestorProfileId_MemberType",
                table: "FinancialTeamMembers",
                columns: new[] { "InvestorProfileId", "MemberType" });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTeamMembers_MemberType",
                table: "FinancialTeamMembers",
                column: "MemberType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialTeamMembers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "f5310569-541a-43d9-bbaf-4d23bdc9a74e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "e731f7a0-613b-4f1e-bcdf-86c70d730b8e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "399ec171-9020-4c4c-9ff9-b491905cfb51");
        }
    }
}
