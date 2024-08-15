using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddUserCityRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "city_id",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_city_id",
                table: "AspNetUsers",
                column: "city_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_city_city_id",
                table: "AspNetUsers",
                column: "city_id",
                principalTable: "city",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_city_city_id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_city_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "city_id",
                table: "AspNetUsers");
        }
    }
}
