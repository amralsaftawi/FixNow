using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnicianPortfolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TechnicianPortfolioItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TechnicianProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianPortfolioItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicianPortfolioItems_TechnicianProfiles_TechnicianProfi~",
                        column: x => x.TechnicianProfileId,
                        principalTable: "TechnicianProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechnicianPortfolioMedia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TechnicianPortfolioItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaKey = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianPortfolioMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicianPortfolioMedia_TechnicianPortfolioItems_Technicia~",
                        column: x => x.TechnicianPortfolioItemId,
                        principalTable: "TechnicianPortfolioItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianPortfolioItems_TechnicianProfileId",
                table: "TechnicianPortfolioItems",
                column: "TechnicianProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianPortfolioMedia_TechnicianPortfolioItemId",
                table: "TechnicianPortfolioMedia",
                column: "TechnicianPortfolioItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechnicianPortfolioMedia");

            migrationBuilder.DropTable(
                name: "TechnicianPortfolioItems");
        }
    }
}
