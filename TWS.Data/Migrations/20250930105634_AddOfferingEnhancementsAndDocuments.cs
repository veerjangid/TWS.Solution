using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOfferingEnhancementsAndDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Offerings",
                type: "varchar(450)",
                maxLength: 450,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Offerings",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedByUserId",
                table: "Offerings",
                type: "varchar(450)",
                maxLength: 450,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OfferingType",
                table: "Offerings",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PDFPath",
                table: "Offerings",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalValue",
                table: "Offerings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OfferingDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OfferingId = table.Column<int>(type: "int", nullable: false),
                    DocumentName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UploadDate = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferingDocuments_Offerings_OfferingId",
                        column: x => x.OfferingId,
                        principalTable: "Offerings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "60753267-060e-4e65-925a-dc8ce5f44ffe");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "63ae19c5-32f0-4869-b7d9-33dc46877443");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "80e55899-fdcf-4db7-8cd3-223cad5049ce");

            migrationBuilder.CreateIndex(
                name: "IX_Offerings_CreatedByUserId",
                table: "Offerings",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Offerings_ModifiedByUserId",
                table: "Offerings",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Offerings_OfferingType",
                table: "Offerings",
                column: "OfferingType");

            migrationBuilder.CreateIndex(
                name: "IX_OfferingDocuments_OfferingId",
                table: "OfferingDocuments",
                column: "OfferingId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferingDocuments_UploadDate",
                table: "OfferingDocuments",
                column: "UploadDate");

            migrationBuilder.AddForeignKey(
                name: "FK_Offerings_AspNetUsers_CreatedByUserId",
                table: "Offerings",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Offerings_AspNetUsers_ModifiedByUserId",
                table: "Offerings",
                column: "ModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offerings_AspNetUsers_CreatedByUserId",
                table: "Offerings");

            migrationBuilder.DropForeignKey(
                name: "FK_Offerings_AspNetUsers_ModifiedByUserId",
                table: "Offerings");

            migrationBuilder.DropTable(
                name: "OfferingDocuments");

            migrationBuilder.DropIndex(
                name: "IX_Offerings_CreatedByUserId",
                table: "Offerings");

            migrationBuilder.DropIndex(
                name: "IX_Offerings_ModifiedByUserId",
                table: "Offerings");

            migrationBuilder.DropIndex(
                name: "IX_Offerings_OfferingType",
                table: "Offerings");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Offerings");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Offerings");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Offerings");

            migrationBuilder.DropColumn(
                name: "OfferingType",
                table: "Offerings");

            migrationBuilder.DropColumn(
                name: "PDFPath",
                table: "Offerings");

            migrationBuilder.DropColumn(
                name: "TotalValue",
                table: "Offerings");

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
        }
    }
}
