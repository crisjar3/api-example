using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateTeammateRequestToJobOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_user_teammate_request_teammate_request_id",
                table: "company_user");

            migrationBuilder.DropTable(
                name: "teammate_request_benefit");

            migrationBuilder.DropTable(
                name: "teammate_request_job_role_skill");

            migrationBuilder.DropTable(
                name: "teammate_request_job_role");

            migrationBuilder.DropTable(
                name: "teammate_request");

            migrationBuilder.DropIndex(
                name: "ix_company_user_teammate_request_id",
                table: "company_user");

            migrationBuilder.AddColumn<int>(
                name: "job_offer_id",
                table: "company_user",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "job_offer",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company_name = table.Column<string>(type: "text", nullable: false),
                    team_name = table.Column<string>(type: "text", nullable: false),
                    office_policy = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    date_expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    have_development_team = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    city_id = table.Column<int>(type: "integer", nullable: true),
                    country_id = table.Column<int>(type: "integer", nullable: true),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_offer", x => x.id);
                    table.ForeignKey(
                        name: "fk_job_offer_city_city_id",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_job_offer_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_country_country_id",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "job_offer_benefit",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teammate_request_id = table.Column<int>(type: "integer", nullable: false),
                    benefit_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_offer_benefit", x => x.id);
                    table.ForeignKey(
                        name: "fk_job_offer_benefit_benefit_benefit_id",
                        column: x => x.benefit_id,
                        principalTable: "benefit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_job_offer_benefit_job_offer_teammate_request_id",
                        column: x => x.teammate_request_id,
                        principalTable: "job_offer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_offer_job_role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teammate_request_id = table.Column<int>(type: "integer", nullable: false),
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
                        name: "fk_job_offer_job_role_job_offer_teammate_request_id",
                        column: x => x.teammate_request_id,
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
                    teammate_request_job_role_id = table.Column<int>(type: "integer", nullable: false),
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
                        name: "fk_job_offer_job_role_skill_job_offer_job_role_teammate_reques",
                        column: x => x.teammate_request_job_role_id,
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
                name: "ix_company_user_job_offer_id",
                table: "company_user",
                column: "job_offer_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_city_id",
                table: "job_offer",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_company_id",
                table: "job_offer",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_country_id",
                table: "job_offer",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_benefit_benefit_id",
                table: "job_offer_benefit",
                column: "benefit_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_benefit_teammate_request_id",
                table: "job_offer_benefit",
                column: "teammate_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_job_role_job_role_id",
                table: "job_offer_job_role",
                column: "job_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_job_role_seniority_id",
                table: "job_offer_job_role",
                column: "seniority_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_job_role_teammate_request_id",
                table: "job_offer_job_role",
                column: "teammate_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_job_role_skill_skill_id",
                table: "job_offer_job_role_skill",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_offer_job_role_skill_teammate_request_job_role_id",
                table: "job_offer_job_role_skill",
                column: "teammate_request_job_role_id");

            migrationBuilder.AddForeignKey(
                name: "fk_company_user_teammate_request_job_offer_id",
                table: "company_user",
                column: "job_offer_id",
                principalTable: "job_offer",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_user_teammate_request_job_offer_id",
                table: "company_user");

            migrationBuilder.DropTable(
                name: "job_offer_benefit");

            migrationBuilder.DropTable(
                name: "job_offer_job_role_skill");

            migrationBuilder.DropTable(
                name: "job_offer_job_role");

            migrationBuilder.DropTable(
                name: "job_offer");

            migrationBuilder.DropIndex(
                name: "ix_company_user_job_offer_id",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "job_offer_id",
                table: "company_user");

            migrationBuilder.CreateTable(
                name: "teammate_request",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city_id = table.Column<int>(type: "integer", nullable: true),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: true),
                    company_name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    end_request = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    have_development_team = table.Column<bool>(type: "boolean", nullable: false),
                    office_policy = table.Column<string>(type: "text", nullable: false),
                    team_name = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teammate_request", x => x.id);
                    table.ForeignKey(
                        name: "fk_teammate_request_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_teammate_request_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teammate_request_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "teammate_request_benefit",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    benefit_id = table.Column<int>(type: "integer", nullable: false),
                    teammate_request_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teammate_request_benefit", x => x.id);
                    table.ForeignKey(
                        name: "fk_teammate_request_benefit_benefit_benefit_id",
                        column: x => x.benefit_id,
                        principalTable: "benefit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teammate_request_benefit_teammate_request_teammate_request_",
                        column: x => x.teammate_request_id,
                        principalTable: "teammate_request",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teammate_request_job_role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_role_id = table.Column<int>(type: "integer", nullable: false),
                    seniority_id = table.Column<int>(type: "integer", nullable: false),
                    teammate_request_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teammate_request_job_role", x => x.id);
                    table.ForeignKey(
                        name: "fk_teammate_request_job_role_job_role_job_role_id",
                        column: x => x.job_role_id,
                        principalTable: "job_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teammate_request_job_role_seniority_seniority_id",
                        column: x => x.seniority_id,
                        principalTable: "seniority",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teammate_request_job_role_teammate_request_teammate_request",
                        column: x => x.teammate_request_id,
                        principalTable: "teammate_request",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teammate_request_job_role_skill",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    skill_id = table.Column<int>(type: "integer", nullable: false),
                    teammate_request_job_role_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teammate_request_job_role_skill", x => x.id);
                    table.ForeignKey(
                        name: "fk_teammate_request_job_role_skill_skill_skill_id",
                        column: x => x.skill_id,
                        principalTable: "skill",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teammate_request_job_role_skill_teammate_request_job_role_t",
                        column: x => x.teammate_request_job_role_id,
                        principalTable: "teammate_request_job_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_company_user_teammate_request_id",
                table: "company_user",
                column: "teammate_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_teammate_request_city_id",
                table: "teammate_request",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_teammate_request_company_id",
                table: "teammate_request",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_teammate_request_country_id",
                table: "teammate_request",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_teammate_request_benefit_benefit_id",
                table: "teammate_request_benefit",
                column: "benefit_id");

            migrationBuilder.CreateIndex(
                name: "ix_teammate_request_benefit_teammate_request_id",
                table: "teammate_request_benefit",
                column: "teammate_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_teammate_request_job_role_job_role_id",
                table: "teammate_request_job_role",
                column: "job_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_teammate_request_job_role_seniority_id",
                table: "teammate_request_job_role",
                column: "seniority_id");

            migrationBuilder.CreateIndex(
                name: "ix_teammate_request_job_role_teammate_request_id",
                table: "teammate_request_job_role",
                column: "teammate_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_teammate_request_job_role_skill_skill_id",
                table: "teammate_request_job_role_skill",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "ix_teammate_request_job_role_skill_teammate_request_job_role_id",
                table: "teammate_request_job_role_skill",
                column: "teammate_request_job_role_id");

            migrationBuilder.AddForeignKey(
                name: "fk_company_user_teammate_request_teammate_request_id",
                table: "company_user",
                column: "teammate_request_id",
                principalTable: "teammate_request",
                principalColumn: "id");
        }
    }
}
