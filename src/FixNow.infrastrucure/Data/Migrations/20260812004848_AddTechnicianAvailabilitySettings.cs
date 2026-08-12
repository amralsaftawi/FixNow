using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnicianAvailabilitySettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailabilitySettings_Status",
                table: "TechnicianProfiles",
                type: "integer",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<DateOnly>(
                name: "AvailabilitySettings_VacationEndDate",
                table: "TechnicianProfiles",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "AvailabilitySettings_VacationStartDate",
                table: "TechnicianProfiles",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TechnicianWorkingDay",
                columns: table => new
                {
                    TechnicianAvailabilitySettingsTechnicianProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Day = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianWorkingDay", x => new { x.TechnicianAvailabilitySettingsTechnicianProfileId, x.Id });
                    table.ForeignKey(
                        name: "FK_TechnicianWorkingDay_TechnicianProfiles_TechnicianAvailabil~",
                        column: x => x.TechnicianAvailabilitySettingsTechnicianProfileId,
                        principalTable: "TechnicianProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechnicianWorkingDay");

            migrationBuilder.DropColumn(
                name: "AvailabilitySettings_Status",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "AvailabilitySettings_VacationEndDate",
                table: "TechnicianProfiles");

            migrationBuilder.DropColumn(
                name: "AvailabilitySettings_VacationStartDate",
                table: "TechnicianProfiles");
        }
    }
}
