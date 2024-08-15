using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateJobRoleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "job_role",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "job_role",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_job_role_company_id",
                table: "job_role",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "fk_job_role_company_company_id",
                table: "job_role",
                column: "company_id",
                principalTable: "company",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_job_role_company_company_id",
                table: "job_role");

            migrationBuilder.DropIndex(
                name: "ix_job_role_company_id",
                table: "job_role");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "job_role");

            migrationBuilder.DropColumn(
                name: "is_default",
                table: "job_role");
        }
    }
}
