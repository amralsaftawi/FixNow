using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "IsActive" },
                values: new object[]
                {
                    new Guid("1c2e0a8d-2b0f-4c7a-8e4d-3a5b6c7d8e9f"),
                    "Admin",
                    true
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1c2e0a8d-2b0f-4c7a-8e4d-3a5b6c7d8e9f"));
        }
    }
}
