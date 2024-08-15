using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddRelationshipBetweenProjectCompanyUserEntityAndDailyTimeBlockEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "owner_id",
                table: "daily_time_block",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_daily_time_block_owner_id",
                table: "daily_time_block",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_daily_time_block_project_company_users_owner_id",
                table: "daily_time_block",
                column: "owner_id",
                principalTable: "project_company_user",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_daily_time_block_project_company_users_owner_id",
                table: "daily_time_block");

            migrationBuilder.DropIndex(
                name: "ix_daily_time_block_owner_id",
                table: "daily_time_block");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "daily_time_block");
        }
    }
}
