using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class RemoveOwnerPropertyOfCompanyEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_users_owner_id",
                table: "company");

            migrationBuilder.DropForeignKey(
                name: "fk_user_settings_time_zones_time_zone_id",
                table: "user_settings");

            migrationBuilder.DropIndex(
                name: "ix_user_settings_time_zone_id",
                table: "user_settings");

            migrationBuilder.DropIndex(
                name: "ix_company_owner_id",
                table: "company");

            migrationBuilder.DropColumn(
                name: "time_zone_id",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "company");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "time_zone_id",
                table: "user_settings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "owner_id",
                table: "company",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_user_settings_time_zone_id",
                table: "user_settings",
                column: "time_zone_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_owner_id",
                table: "company",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_company_users_owner_id",
                table: "company",
                column: "owner_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_settings_time_zones_time_zone_id",
                table: "user_settings",
                column: "time_zone_id",
                principalTable: "time_zone",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
