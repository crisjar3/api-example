using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddedGoalEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "goal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    target_end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    actual_end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    approved = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    story_points = table.Column<int>(type: "integer", nullable: false),
                    jira_ticket_id = table.Column<string>(type: "text", nullable: false),
                    priority_level = table.Column<int>(type: "integer", nullable: false),
                    project_id = table.Column<int>(type: "integer", nullable: false),
                    owner_id = table.Column<string>(type: "text", nullable: false),
                    scrum_master_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_goal", x => x.id);
                    table.ForeignKey(
                        name: "fk_goal_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_goal_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_goal_users_scrum_master_id",
                        column: x => x.scrum_master_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "goal_extra_mile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    extra_mile_end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    goal_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_goal_extra_mile", x => x.id);
                    table.ForeignKey(
                        name: "fk_goal_extra_mile_goal_goal_id",
                        column: x => x.goal_id,
                        principalTable: "goal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_goal_owner_id",
                table: "goal",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_goal_project_id",
                table: "goal",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_goal_scrum_master_id",
                table: "goal",
                column: "scrum_master_id");

            migrationBuilder.CreateIndex(
                name: "ix_goal_extra_mile_goal_id",
                table: "goal_extra_mile",
                column: "goal_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "goal_extra_mile");

            migrationBuilder.DropTable(
                name: "goal");
        }
    }
}
