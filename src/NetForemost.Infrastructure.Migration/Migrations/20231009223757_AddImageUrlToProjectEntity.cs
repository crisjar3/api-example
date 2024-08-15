using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddImageUrlToProjectEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_daily_monitoring_block_block_time_trackings_daily_time_bloc",
                table: "daily_monitoring_block");

            migrationBuilder.DropForeignKey(
                name: "fk_daily_time_block_daily_entry_daily_entry_id",
                table: "daily_time_block");

            migrationBuilder.DropForeignKey(
                name: "fk_daily_time_block_task_task_id",
                table: "daily_time_block");

            migrationBuilder.AddColumn<string>(
                name: "project_image_url",
                table: "project",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_daily_monitoring_block_daily_time_block_daily_time_block_id",
                table: "daily_monitoring_block",
                column: "daily_time_block_id",
                principalTable: "daily_time_block",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_daily_time_block_time_history_tracking_daily_entry_id",
                table: "daily_time_block",
                column: "daily_entry_id",
                principalTable: "daily_entry",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_daily_time_block_work_tasks_task_id",
                table: "daily_time_block",
                column: "task_id",
                principalTable: "task",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_daily_monitoring_block_daily_time_block_daily_time_block_id",
                table: "daily_monitoring_block");

            migrationBuilder.DropForeignKey(
                name: "fk_daily_time_block_time_history_tracking_daily_entry_id",
                table: "daily_time_block");

            migrationBuilder.DropForeignKey(
                name: "fk_daily_time_block_work_tasks_task_id",
                table: "daily_time_block");

            migrationBuilder.DropColumn(
                name: "project_image_url",
                table: "project");

            migrationBuilder.AddForeignKey(
                name: "fk_daily_monitoring_block_block_time_trackings_daily_time_bloc",
                table: "daily_monitoring_block",
                column: "daily_time_block_id",
                principalTable: "daily_time_block",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_daily_time_block_daily_entry_daily_entry_id",
                table: "daily_time_block",
                column: "daily_entry_id",
                principalTable: "daily_entry",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_daily_time_block_task_task_id",
                table: "daily_time_block",
                column: "task_id",
                principalTable: "task",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
