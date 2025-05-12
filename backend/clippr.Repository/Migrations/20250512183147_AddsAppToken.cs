using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clippr.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddsAppToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserSubject = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpirationDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Salt = table.Column<byte[]>(type: "longblob", nullable: false),
                    Hash = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTokens_Users_UserSubject",
                        column: x => x.UserSubject,
                        principalTable: "Users",
                        principalColumn: "Subject",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AppTokens_UserSubject",
                table: "AppTokens",
                column: "UserSubject");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppTokens");
        }
    }
}
