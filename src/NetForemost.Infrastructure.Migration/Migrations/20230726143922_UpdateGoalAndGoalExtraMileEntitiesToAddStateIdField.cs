using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateGoalAndGoalExtraMileEntitiesToAddStateIdField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "goal_status_id",
                table: "goal_extra_mile",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "goal_status_id",
                table: "goal",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_goal_extra_mile_goal_status_id",
                table: "goal_extra_mile",
                column: "goal_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_goal_goal_status_id",
                table: "goal",
                column: "goal_status_id");

            migrationBuilder.AddForeignKey(
                name: "fk_goal_statuses_goal_status_id",
                table: "goal",
                column: "goal_status_id",
                principalTable: "goal_status",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_goal_extra_mile_statuses_goal_status_id",
                table: "goal_extra_mile",
                column: "goal_status_id",
                principalTable: "goal_status",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_goal_statuses_goal_status_id",
                table: "goal");

            migrationBuilder.DropForeignKey(
                name: "fk_goal_extra_mile_statuses_goal_status_id",
                table: "goal_extra_mile");

            migrationBuilder.DropIndex(
                name: "ix_goal_extra_mile_goal_status_id",
                table: "goal_extra_mile");

            migrationBuilder.DropIndex(
                name: "ix_goal_goal_status_id",
                table: "goal");

            migrationBuilder.DropColumn(
                name: "goal_status_id",
                table: "goal_extra_mile");

            migrationBuilder.DropColumn(
                name: "goal_status_id",
                table: "goal");
        }
    }
}
