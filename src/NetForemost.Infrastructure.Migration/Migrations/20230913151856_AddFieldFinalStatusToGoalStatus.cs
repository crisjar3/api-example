using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddFieldFinalStatusToGoalStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_final_status",
                table: "goal_status",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_final_status",
                table: "goal_status");
        }
    }
}
