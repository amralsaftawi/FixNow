using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquePaymentAssignmentIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_AssignmentId",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AssignmentId",
                table: "Payments",
                column: "AssignmentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_AssignmentId",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AssignmentId",
                table: "Payments",
                column: "AssignmentId");
        }
    }
}
