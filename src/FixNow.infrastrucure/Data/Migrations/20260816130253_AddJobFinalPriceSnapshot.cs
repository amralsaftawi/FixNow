using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddJobFinalPriceSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InspectionFee",
                table: "Jobs",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InspectionFeeCurrency",
                table: "Jobs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ServicePrice",
                table: "Jobs",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicePriceCurrency",
                table: "Jobs",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InspectionFee",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "InspectionFeeCurrency",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ServicePrice",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ServicePriceCurrency",
                table: "Jobs");
        }
    }
}
