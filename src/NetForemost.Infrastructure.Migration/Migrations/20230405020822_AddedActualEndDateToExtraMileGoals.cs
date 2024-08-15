using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddedActualEndDateToExtraMileGoals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "extra_mile_end_date",
                table: "goal_extra_mile",
                newName: "extra_mile_target_end_date");

            migrationBuilder.AddColumn<DateTime>(
                name: "actual_end_date",
                table: "goal_extra_mile",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actual_end_date",
                table: "goal_extra_mile");

            migrationBuilder.RenameColumn(
                name: "extra_mile_target_end_date",
                table: "goal_extra_mile",
                newName: "extra_mile_end_date");
        }
    }
}
