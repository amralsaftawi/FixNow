using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProblemTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProblemTypeId",
                table: "ServiceRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProblemTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ServiceCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemTypes_ServiceCategories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ProblemTypeId",
                table: "ServiceRequests",
                column: "ProblemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemTypes_ServiceCategoryId_Name",
                table: "ProblemTypes",
                columns: new[] { "ServiceCategoryId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_ProblemTypes_ProblemTypeId",
                table: "ServiceRequests",
                column: "ProblemTypeId",
                principalTable: "ProblemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_ProblemTypes_ProblemTypeId",
                table: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "ProblemTypes");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_ProblemTypeId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "ProblemTypeId",
                table: "ServiceRequests");
        }
    }
}
