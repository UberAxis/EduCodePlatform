using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduCodePlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLinkCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkCode",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LinkCodeExpiresAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LinkCodeExpiresAt",
                table: "Users");
        }
    }
}
