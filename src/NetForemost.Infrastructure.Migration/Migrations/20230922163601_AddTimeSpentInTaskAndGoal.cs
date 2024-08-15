using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddTimeSpentInTaskAndGoal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "time_spent_in_second",
                table: "task",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "time_spent_in_second",
                table: "goal",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time_spent_in_second",
                table: "task");

            migrationBuilder.DropColumn(
                name: "time_spent_in_second",
                table: "goal");
        }
    }
}
