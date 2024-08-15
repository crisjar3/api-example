using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddProjectCompanyUserRelationShipWihtGoalsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "owner_id",
                table: "goal",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_goal_owner_id",
                table: "goal",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_goal_project_company_users_owner_id",
                table: "goal",
                column: "owner_id",
                principalTable: "project_company_user",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_goal_project_company_users_owner_id",
                table: "goal");

            migrationBuilder.DropIndex(
                name: "ix_goal_owner_id",
                table: "goal");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "goal");
        }
    }
}
