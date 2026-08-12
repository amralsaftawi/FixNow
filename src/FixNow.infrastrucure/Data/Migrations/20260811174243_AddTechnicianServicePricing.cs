using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnicianServicePricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TechnicianServices",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceCurrency",
                table: "TechnicianServices",
                type: "integer",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_TechnicianServices_Price",
                table: "TechnicianServices",
                sql: "\"Price\" > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_TechnicianServices_Price",
                table: "TechnicianServices");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TechnicianServices");

            migrationBuilder.DropColumn(
                name: "PriceCurrency",
                table: "TechnicianServices");
        }
    }
}
