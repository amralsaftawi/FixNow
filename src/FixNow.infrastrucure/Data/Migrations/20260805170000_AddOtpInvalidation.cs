using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260805170000_AddOtpInvalidation")]
public partial class AddOtpInvalidation : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "InvalidatedAt",
            table: "OtpRecords",
            type: "timestamp with time zone",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "InvalidatedAt",
            table: "OtpRecords");
    }
}
