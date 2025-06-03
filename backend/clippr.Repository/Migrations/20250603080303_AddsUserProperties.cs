using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clippr.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddsUserProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTokens_Users_UserSubject",
                table: "AppTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Clips_Users_UserSubject",
                table: "Clips");

            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserSubject",
                table: "Clips",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Clips_UserSubject",
                table: "Clips",
                newName: "IX_Clips_UserId");

            migrationBuilder.RenameColumn(
                name: "UserSubject",
                table: "AppTokens",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppTokens_UserSubject",
                table: "AppTokens",
                newName: "IX_AppTokens_UserId");

            migrationBuilder.AddColumn<string>(
                name: "FamilyName",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTokens_Users_UserId",
                table: "AppTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clips_Users_UserId",
                table: "Clips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTokens_Users_UserId",
                table: "AppTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Clips_Users_UserId",
                table: "Clips");

            migrationBuilder.DropColumn(
                name: "FamilyName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "Subject");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Clips",
                newName: "UserSubject");

            migrationBuilder.RenameIndex(
                name: "IX_Clips_UserId",
                table: "Clips",
                newName: "IX_Clips_UserSubject");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AppTokens",
                newName: "UserSubject");

            migrationBuilder.RenameIndex(
                name: "IX_AppTokens_UserId",
                table: "AppTokens",
                newName: "IX_AppTokens_UserSubject");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTokens_Users_UserSubject",
                table: "AppTokens",
                column: "UserSubject",
                principalTable: "Users",
                principalColumn: "Subject",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clips_Users_UserSubject",
                table: "Clips",
                column: "UserSubject",
                principalTable: "Users",
                principalColumn: "Subject",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
