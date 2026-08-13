using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnicianCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "TechnicianProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianProfiles_CityId",
                table: "TechnicianProfiles",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicianProfiles_Cities_CityId",
                table: "TechnicianProfiles",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnicianProfiles_Cities_CityId",
                table: "TechnicianProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianProfiles_CityId",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "TechnicianProfiles");
        }
    }
}
