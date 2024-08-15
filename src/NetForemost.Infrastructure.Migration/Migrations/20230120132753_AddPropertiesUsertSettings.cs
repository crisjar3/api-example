using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddPropertiesUsertSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "blur_screenshots",
                table: "user_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "can_edit_time",
                table: "user_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "delete_screencasts",
                table: "user_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "screencasts_frecuency",
                table: "user_settings",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "show_in_reports",
                table: "user_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "time_out_after",
                table: "user_settings",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "blur_screenshots",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "can_edit_time",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "delete_screencasts",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "screencasts_frecuency",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "show_in_reports",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "time_out_after",
                table: "user_settings");
        }
    }
}
