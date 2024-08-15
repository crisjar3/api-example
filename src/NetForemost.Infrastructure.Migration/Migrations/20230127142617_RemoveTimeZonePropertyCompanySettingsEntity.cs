using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class RemoveTimeZonePropertyCompanySettingsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time_zone",
                table: "company_settings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "time_zone",
                table: "company_settings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
