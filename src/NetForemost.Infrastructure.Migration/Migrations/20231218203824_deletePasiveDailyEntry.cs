using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class deletePasiveDailyEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_daily_time_block_time_history_tracking_daily_entry_id",
                table: "daily_time_block");

            migrationBuilder.DropIndex(
                name: "ix_daily_time_block_daily_entry_id",
                table: "daily_time_block");

            migrationBuilder.DropColumn(
                name: "daily_entry_id",
                table: "daily_time_block");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "daily_entry_id",
                table: "daily_time_block",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_daily_time_block_daily_entry_id",
                table: "daily_time_block",
                column: "daily_entry_id");

            migrationBuilder.AddForeignKey(
                name: "fk_daily_time_block_time_history_tracking_daily_entry_id",
                table: "daily_time_block",
                column: "daily_entry_id",
                principalTable: "daily_entry",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
