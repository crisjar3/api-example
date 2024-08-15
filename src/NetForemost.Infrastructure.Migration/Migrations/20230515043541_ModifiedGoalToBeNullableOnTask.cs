using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class ModifiedGoalToBeNullableOnTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_task_goal_goal_id",
                table: "task");

            migrationBuilder.AlterColumn<int>(
                name: "goal_id",
                table: "task",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_task_goal_goal_id",
                table: "task",
                column: "goal_id",
                principalTable: "goal",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_task_goal_goal_id",
                table: "task");

            migrationBuilder.AlterColumn<int>(
                name: "goal_id",
                table: "task",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_task_goal_goal_id",
                table: "task",
                column: "goal_id",
                principalTable: "goal",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
