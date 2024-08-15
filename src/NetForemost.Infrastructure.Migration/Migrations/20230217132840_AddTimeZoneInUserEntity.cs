using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddTimeZoneInUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "time_zone_id",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_time_zone_id",
                table: "AspNetUsers",
                column: "time_zone_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_time_zone_time_zone_id",
                table: "AspNetUsers",
                column: "time_zone_id",
                principalTable: "time_zone",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_time_zone_time_zone_id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_time_zone_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "time_zone_id",
                table: "AspNetUsers");
        }
    }
}
