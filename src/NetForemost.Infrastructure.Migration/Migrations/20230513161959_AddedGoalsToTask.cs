using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddedGoalsToTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "goal_id",
                table: "task",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_task_goal_id",
                table: "task",
                column: "goal_id");

            migrationBuilder.AddForeignKey(
                name: "fk_task_goal_goal_id",
                table: "task",
                column: "goal_id",
                principalTable: "goal",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_task_goal_goal_id",
                table: "task");

            migrationBuilder.DropIndex(
                name: "ix_task_goal_id",
                table: "task");

            migrationBuilder.DropColumn(
                name: "goal_id",
                table: "task");
        }
    }
}
