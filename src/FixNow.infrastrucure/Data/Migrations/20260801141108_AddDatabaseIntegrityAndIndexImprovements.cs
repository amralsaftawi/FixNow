using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixNow.infrastrucure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseIntegrityAndIndexImprovements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId_RoleId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianProfiles_UserId",
                table: "TechnicianProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AssignmentId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_TechnicianProfileId",
                table: "Assignments");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "\"Email\" IS NOT NULL AND \"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_AssignedByUserId",
                table: "UserRoles",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RevokedByUserId",
                table: "UserRoles",
                column: "RevokedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId_Active",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true,
                filter: "\"IsActive\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianProfiles_UserId",
                table: "TechnicianProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_Status_ServiceCategoryId",
                table: "ServiceRequests",
                columns: new[] { "Status", "ServiceCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AssignmentId",
                table: "Reviews",
                column: "AssignmentId",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reviews_Rating",
                table: "Reviews",
                sql: "\"Rating\" BETWEEN 1 AND 5");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_RefreshTokenHash",
                table: "RefreshTokens",
                column: "RefreshTokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status_CreatedAtUtc",
                table: "Payments",
                columns: new[] { "Status", "CreatedAtUtc" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Payments_Amount",
                table: "Payments",
                sql: "\"Amount\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_OtpRecords_Attempts",
                table: "OtpRecords",
                sql: "\"AttemptsCount\" >= 0 AND \"AttemptsCount\" <= \"MaxAttempts\"");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_TechnicianProfileId_Status",
                table: "Assignments",
                columns: new[] { "TechnicianProfileId", "Status" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_AssignedByUserId",
                table: "UserRoles",
                column: "AssignedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_RevokedByUserId",
                table: "UserRoles",
                column: "RevokedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_AssignedByUserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_RevokedByUserId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_AssignedByUserId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_RevokedByUserId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId_RoleId_Active",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_TechnicianProfiles_UserId",
                table: "TechnicianProfiles");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_Status_ServiceCategoryId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AssignmentId",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Reviews_Rating",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_RefreshTokenHash",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_Payments_Status_CreatedAtUtc",
                table: "Payments");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Payments_Amount",
                table: "Payments");

            migrationBuilder.DropCheckConstraint(
                name: "CK_OtpRecords_Attempts",
                table: "OtpRecords");

            migrationBuilder.DropIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_TechnicianProfileId_Status",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianProfiles_UserId",
                table: "TechnicianProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AssignmentId",
                table: "Reviews",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_TechnicianProfileId",
                table: "Assignments",
                column: "TechnicianProfileId");
        }
    }
}
