using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddUserFieldsToCompanyUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_user_user_type_user_type_id",
                table: "company_user");

            migrationBuilder.DropTable(
                name: "user_type");

            migrationBuilder.RenameColumn(
                name: "user_type_id",
                table: "company_user",
                newName: "time_zone_id");

            migrationBuilder.RenameIndex(
                name: "ix_company_user_user_type_id",
                table: "company_user",
                newName: "ix_company_user_time_zone_id");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "company_user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "job_role_id",
                table: "company_user",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "company_user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_company_user_job_role_id",
                table: "company_user",
                column: "job_role_id");

            migrationBuilder.AddForeignKey(
                name: "fk_company_user_time_zones_time_zone_id",
                table: "company_user",
                column: "time_zone_id",
                principalTable: "time_zone",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_company_user_work_roles_job_role_id",
                table: "company_user",
                column: "job_role_id",
                principalTable: "job_role",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_user_time_zones_time_zone_id",
                table: "company_user");

            migrationBuilder.DropForeignKey(
                name: "fk_company_user_work_roles_job_role_id",
                table: "company_user");

            migrationBuilder.DropIndex(
                name: "ix_company_user_job_role_id",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "job_role_id",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "company_user");

            migrationBuilder.RenameColumn(
                name: "time_zone_id",
                table: "company_user",
                newName: "user_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_company_user_time_zone_id",
                table: "company_user",
                newName: "ix_company_user_user_type_id");

            migrationBuilder.CreateTable(
                name: "user_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_type", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_company_user_user_type_user_type_id",
                table: "company_user",
                column: "user_type_id",
                principalTable: "user_type",
                principalColumn: "id");
        }
    }
}
