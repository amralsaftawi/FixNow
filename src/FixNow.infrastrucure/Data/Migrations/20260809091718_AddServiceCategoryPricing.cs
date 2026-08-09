using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceCategoryPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ServiceCategories",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceCurrency",
                table: "ServiceCategories",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ServiceCategories");

            migrationBuilder.DropColumn(
                name: "PriceCurrency",
                table: "ServiceCategories");
        }
    }
}
