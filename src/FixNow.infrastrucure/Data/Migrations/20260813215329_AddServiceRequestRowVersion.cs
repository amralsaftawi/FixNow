using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceRequestRowVersion : Migration
    {
        /// <inheritdoc />
        /// <remarks>
        /// The ServiceRequest concurrency token is mapped to PostgreSQL's
        /// implicit "xmin" system column, which already exists on every
        /// table. No schema change is required; only the model snapshot is
        /// updated.
        /// </remarks>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
