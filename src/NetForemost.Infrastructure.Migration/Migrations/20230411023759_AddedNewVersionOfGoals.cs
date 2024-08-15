using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddedNewVersionOfGoals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "priority_level",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    level = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    hex_color_code = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_priority_level", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "story_point",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    knowledge_level = table.Column<string>(type: "text", nullable: true),
                    dependencies = table.Column<string>(type: "text", nullable: true),
                    work_effort = table.Column<string>(type: "text", nullable: true),
                    points = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_story_point", x => x.id);
                });

            migrationBuilder.RenameColumn(
              name: "priority_level",
              newName: "priority_level_id",
              table: "goal"
              );

            migrationBuilder.RenameColumn(
                name: "story_points",
                newName: "story_points_id",
                table: "goal"
                );

            migrationBuilder.AlterColumn<int>(
                name: "story_points_id",
                table: "goal",
                type: "integer",
                nullable: false,
                defaultValue: false
                );

            migrationBuilder.AddForeignKey(
                name: "fk_goal_story_points_story_point_id",
                table: "goal",
                column: "story_points_id",
                principalTable: "story_point",
                principalColumn: "id"
                );

            migrationBuilder.AddForeignKey(
                name: "fk_goal_priority_levels_priority_level_id",
                table: "goal",
                column: "priority_level_id",
                principalTable: "priority_level",
                principalColumn: "id"
                );

            migrationBuilder.CreateIndex(
                name: "ix_goal_priority_level_id",
                table: "goal",
                column: "priority_level_id");

            migrationBuilder.CreateIndex(
                name: "ix_goal_story_point_id",
                table: "goal",
                column: "story_points_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "priority_level");

            migrationBuilder.DropTable(
                name: "story_point");
        }
    }
}
