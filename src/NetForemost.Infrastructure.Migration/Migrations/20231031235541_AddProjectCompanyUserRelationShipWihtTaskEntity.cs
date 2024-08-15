using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddProjectCompanyUserRelationShipWihtTaskEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "owner_id",
                table: "task",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_task_owner_id",
                table: "task",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_task_project_company_user_owner_id",
                table: "task",
                column: "owner_id",
                principalTable: "project_company_user",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_task_project_company_user_owner_id",
                table: "task");

            migrationBuilder.DropIndex(
                name: "ix_task_owner_id",
                table: "task");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "task");
        }
    }
}
