using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class ModifiedNameBackToTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "task");

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "task",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_task_company_id",
                table: "task",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "fk_task_company_company_id",
                table: "task",
                column: "company_id",
                principalTable: "company",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_task_company_company_id",
                table: "task");

            migrationBuilder.DropIndex(
                name: "ix_task_company_id",
                table: "task");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "task");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "task",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
