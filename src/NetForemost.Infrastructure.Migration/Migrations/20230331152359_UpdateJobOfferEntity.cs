using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateJobOfferEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job_offer_job_role_skill");

            migrationBuilder.DropTable(
                name: "job_offer_job_role");

            migrationBuilder.DropColumn(
                name: "company_name",
                table: "job_offer");

            migrationBuilder.DropColumn(
                name: "description",
                table: "job_offer");

            migrationBuilder.DropColumn(
                name: "have_development_team",
                table: "job_offer");

            migrationBuilder.DropColumn(
                name: "office_policy",
                table: "job_offer");

            migrationBuilder.DropColumn(
                name: "team_name",
                table: "job_offer");

            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "job_offer",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "job_offer_talent",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    responsibilities = table.Column<string>(type: "text", nullable: false),
                    job_offer_id = table.Column<int>(type: "integer", nullable: false),
                    job_role_id = table.Column<int>(type: "integer", nullable: false),
                    seniority_id = table.Column<int>(type: "integer", nullable: false),
                    policy_id = table.Column<int>(type: "integer", nullable: false),
                    contract_type_id = table.Column<int>(type: "integer", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: false),
                    language_level_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_offer_talent", x => x.id);
                    table.ForeignKey(
                        name: "fk_job_offer_talent_contract_type_contract_type_id",
                        column: x => x.contract_type_id,
                        principalTable: "contract_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_talent_job_offer_job_offer_id",
                        column: x => x.job_offer_id,
                        principalTable: "job_offer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_talent_language_levels_language_level_id",
                        column: x => x.language_level_id,
                        principalTable: "language_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_talent_languages_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_talent_policies_policy_id",
                        column: x => x.policy_id,
                        principalTable: "policy",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_talent_seniorities_seniority_id",
                        column: x => x.seniority_id,
                        principalTable: "seniority",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_talent_work_roles_job_role_id",
                        column: x => x.job_role_id,
                        principalTable: "job_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_offer_talent_skill",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_offer_talent_id = table.Column<int>(type: "integer", nullable: false),
                    skill_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_offer_talent_skill", x => x.id);
                    table.ForeignKey(
                        name: "fk_job_offer_talent_skill_job_offer_talent_job_offer_talent_id",
                        column: x => x.job_offer_talent_id,
                        principalTable: "job_offer_talent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_talent_skill_skills_skill_id",
                        column: x => x.skill_id,
                        principalTable: "skill",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_project_id",
                table: "job_offer",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_talent_contract_type_id",
                table: "job_offer_talent",
                column: "contract_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_talent_job_offer_id",
                table: "job_offer_talent",
                column: "job_offer_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_talent_job_role_id",
                table: "job_offer_talent",
                column: "job_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_talent_language_id",
                table: "job_offer_talent",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_talent_language_level_id",
                table: "job_offer_talent",
                column: "language_level_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_talent_policy_id",
                table: "job_offer_talent",
                column: "policy_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_talent_seniority_id",
                table: "job_offer_talent",
                column: "seniority_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_talent_skill_job_offer_talent_id",
                table: "job_offer_talent_skill",
                column: "job_offer_talent_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_talent_skill_skill_id",
                table: "job_offer_talent_skill",
                column: "skill_id");

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_projects_project_id",
                table: "job_offer",
                column: "project_id",
                principalTable: "project",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_projects_project_id",
                table: "job_offer");

            migrationBuilder.DropTable(
                name: "job_offer_talent_skill");

            migrationBuilder.DropTable(
                name: "job_offer_talent");

            migrationBuilder.DropIndex(
                name: "ix_job_offer_project_id",
                table: "job_offer");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "job_offer");

            migrationBuilder.AddColumn<string>(
                name: "company_name",
                table: "job_offer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "job_offer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "have_development_team",
                table: "job_offer",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "office_policy",
                table: "job_offer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "team_name",
                table: "job_offer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "job_offer_job_role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_offer_id = table.Column<int>(type: "integer", nullable: false),
                    job_role_id = table.Column<int>(type: "integer", nullable: false),
                    seniority_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_offer_job_role", x => x.id);
                    table.ForeignKey(
                        name: "fk_job_offer_job_role_job_offer_job_offer_id",
                        column: x => x.job_offer_id,
                        principalTable: "job_offer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_job_role_job_role_job_role_id",
                        column: x => x.job_role_id,
                        principalTable: "job_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_job_role_seniority_seniority_id",
                        column: x => x.seniority_id,
                        principalTable: "seniority",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_offer_job_role_skill",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_offer_job_role_id = table.Column<int>(type: "integer", nullable: false),
                    skill_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_offer_job_role_skill", x => x.id);
                    table.ForeignKey(
                        name: "fk_job_offer_job_role_skill_job_offer_job_role_job_offer_job_r",
                        column: x => x.job_offer_job_role_id,
                        principalTable: "job_offer_job_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_job_role_skill_skill_skill_id",
                        column: x => x.skill_id,
                        principalTable: "skill",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_job_role_job_offer_id",
                table: "job_offer_job_role",
                column: "job_offer_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_job_role_job_role_id",
                table: "job_offer_job_role",
                column: "job_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_job_role_seniority_id",
                table: "job_offer_job_role",
                column: "seniority_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_job_role_skill_job_offer_job_role_id",
                table: "job_offer_job_role_skill",
                column: "job_offer_job_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_job_role_skill_skill_id",
                table: "job_offer_job_role_skill",
                column: "skill_id");
        }
    }
}
