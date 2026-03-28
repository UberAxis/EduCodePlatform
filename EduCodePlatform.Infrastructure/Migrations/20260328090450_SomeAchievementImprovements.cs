using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EduCodePlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SomeAchievementImprovements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AchievementTriggers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AchievementId = table.Column<int>(type: "integer", nullable: false),
                    TriggerType = table.Column<int>(type: "integer", nullable: false),
                    RequiredValue = table.Column<int>(type: "integer", nullable: false),
                    TargetModuleId = table.Column<int>(type: "integer", nullable: true),
                    TargetLessonId = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementTriggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AchievementTriggers_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementTriggers_Lessons_TargetLessonId",
                        column: x => x.TargetLessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AchievementTriggers_Models_TargetModuleId",
                        column: x => x.TargetModuleId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AchievementTriggers_AchievementId",
                table: "AchievementTriggers",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementTriggers_TargetLessonId",
                table: "AchievementTriggers",
                column: "TargetLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementTriggers_TargetModuleId",
                table: "AchievementTriggers",
                column: "TargetModuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AchievementTriggers");
        }
    }
}
