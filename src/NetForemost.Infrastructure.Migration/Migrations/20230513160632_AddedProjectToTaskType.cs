using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddedProjectToTaskType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "task_type",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_task_type_project_id",
                table: "task_type",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "fk_task_type_project_project_id",
                table: "task_type",
                column: "project_id",
                principalTable: "project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_task_type_project_project_id",
                table: "task_type");

            migrationBuilder.DropIndex(
                name: "ix_task_type_project_id",
                table: "task_type");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "task_type");
        }
    }
}
