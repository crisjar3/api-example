using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateUserAndUserSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_daily_time_block_work_tasks_task_id",
                table: "daily_time_block");

            migrationBuilder.DropIndex(
                name: "ix_user_settings_user_id",
                table: "user_settings");

            migrationBuilder.CreateIndex(
                name: "ix_user_settings_user_id",
                table: "user_settings",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_daily_time_block_task_task_id",
                table: "daily_time_block",
                column: "task_id",
                principalTable: "task",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_daily_time_block_task_task_id",
                table: "daily_time_block");

            migrationBuilder.DropIndex(
                name: "ix_user_settings_user_id",
                table: "user_settings");

            migrationBuilder.CreateIndex(
                name: "ix_user_settings_user_id",
                table: "user_settings",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_daily_time_block_work_tasks_task_id",
                table: "daily_time_block",
                column: "task_id",
                principalTable: "task",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
