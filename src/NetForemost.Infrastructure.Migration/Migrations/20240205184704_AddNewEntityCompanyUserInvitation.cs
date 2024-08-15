using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddNewEntityCompanyUserInvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "company_user_invitation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    invitation_token = table.Column<Guid>(type: "uuid", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    email_invited = table.Column<string>(type: "text", nullable: false),
                    is_accepted = table.Column<bool>(type: "boolean", nullable: false),
                    is_valid = table.Column<bool>(type: "boolean", nullable: false),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    job_role_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    archived_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    archived_by = table.Column<string>(type: "text", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_user_invitation", x => x.id);
                    table.ForeignKey(
                        name: "fk_company_user_invitation_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_company_user_invitation_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_company_user_invitation_work_roles_job_role_id",
                        column: x => x.job_role_id,
                        principalTable: "job_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_company_user_invitation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    project_id = table.Column<int>(type: "integer", nullable: false),
                    company_user_invitation_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    archived_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    archived_by = table.Column<string>(type: "text", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_company_user_invitation", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_company_user_invitation_company_user_invitation_com",
                        column: x => x.company_user_invitation_id,
                        principalTable: "company_user_invitation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_project_company_user_invitation_project_project_id",
                        column: x => x.project_id,
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_company_user_invitation_company_id",
                table: "company_user_invitation",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_user_invitation_job_role_id",
                table: "company_user_invitation",
                column: "job_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_user_invitation_role_id",
                table: "company_user_invitation",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_company_user_invitation_company_user_invitation_id",
                table: "project_company_user_invitation",
                column: "company_user_invitation_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_company_user_invitation_project_id",
                table: "project_company_user_invitation",
                column: "project_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "project_company_user_invitation");

            migrationBuilder.DropTable(
                name: "company_user_invitation");
        }
    }
}
