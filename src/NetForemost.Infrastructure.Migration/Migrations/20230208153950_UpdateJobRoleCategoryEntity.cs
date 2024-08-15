using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateJobRoleCategoryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "job_role_category",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "job_role_category",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_job_role_category_company_id",
                table: "job_role_category",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "fk_job_role_category_company_company_id",
                table: "job_role_category",
                column: "company_id",
                principalTable: "company",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_job_role_category_company_company_id",
                table: "job_role_category");

            migrationBuilder.DropIndex(
                name: "ix_job_role_category_company_id",
                table: "job_role_category");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "job_role_category");

            migrationBuilder.DropColumn(
                name: "is_default",
                table: "job_role_category");
        }
    }
}
