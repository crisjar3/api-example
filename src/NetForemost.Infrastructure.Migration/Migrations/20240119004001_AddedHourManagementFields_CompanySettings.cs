using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddedHourManagementFields_CompanySettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "late_hours_in_users_time_zone_after",
                table: "company_settings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "too_many_hours_per_day",
                table: "company_settings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "work_outside_shift_per_day_longer_than",
                table: "company_settings",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "late_hours_in_users_time_zone_after",
                table: "company_settings");

            migrationBuilder.DropColumn(
                name: "too_many_hours_per_day",
                table: "company_settings");

            migrationBuilder.DropColumn(
                name: "work_outside_shift_per_day_longer_than",
                table: "company_settings");
        }
    }
}
