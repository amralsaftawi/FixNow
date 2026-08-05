using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260805160000_InvalidateLegacyPlaintextRefreshTokens")]
public partial class InvalidateLegacyPlaintextRefreshTokens : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            UPDATE "RefreshTokens"
            SET "IsRevoked" = TRUE,
                "RevokedAt" = CURRENT_TIMESTAMP
            WHERE "IsRevoked" = FALSE;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
