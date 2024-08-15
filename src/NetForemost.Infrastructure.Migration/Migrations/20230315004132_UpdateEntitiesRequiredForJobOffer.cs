using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateEntitiesRequiredForJobOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_user_teammate_request_job_offer_id",
                table: "company_user");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_city_city_id",
                table: "job_offer");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_company_company_id",
                table: "job_offer");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_country_country_id",
                table: "job_offer");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_benefit_job_offer_teammate_request_id",
                table: "job_offer_benefit");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_job_role_job_offer_teammate_request_id",
                table: "job_offer_job_role");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_job_role_skill_job_offer_job_role_teammate_reques",
                table: "job_offer_job_role_skill");

            migrationBuilder.DropColumn(
                name: "teammate_request_id",
                table: "company_user");

            migrationBuilder.RenameColumn(
                name: "teammate_request_job_role_id",
                table: "job_offer_job_role_skill",
                newName: "job_offer_job_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_job_offer_job_role_skill_teammate_request_job_role_id",
                table: "job_offer_job_role_skill",
                newName: "ix_job_offer_job_role_skill_job_offer_job_role_id");

            migrationBuilder.RenameColumn(
                name: "teammate_request_id",
                table: "job_offer_job_role",
                newName: "job_offer_id");

            migrationBuilder.RenameIndex(
                name: "ix_job_offer_job_role_teammate_request_id",
                table: "job_offer_job_role",
                newName: "ix_job_offer_job_role_job_offer_id");

            migrationBuilder.RenameColumn(
                name: "teammate_request_id",
                table: "job_offer_benefit",
                newName: "job_offer_id");

            migrationBuilder.RenameIndex(
                name: "ix_job_offer_benefit_teammate_request_id",
                table: "job_offer_benefit",
                newName: "ix_job_offer_benefit_job_offer_id");

            migrationBuilder.AddForeignKey(
                name: "fk_company_user_job_offer_job_offer_id",
                table: "company_user",
                column: "job_offer_id",
                principalTable: "job_offer",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_cities_city_id",
                table: "job_offer",
                column: "city_id",
                principalTable: "city",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_companies_company_id",
                table: "job_offer",
                column: "company_id",
                principalTable: "company",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_countries_country_id",
                table: "job_offer",
                column: "country_id",
                principalTable: "country",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_benefit_job_offer_job_offer_id",
                table: "job_offer_benefit",
                column: "job_offer_id",
                principalTable: "job_offer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_job_role_job_offer_job_offer_id",
                table: "job_offer_job_role",
                column: "job_offer_id",
                principalTable: "job_offer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_job_role_skill_job_offer_job_role_job_offer_job_r",
                table: "job_offer_job_role_skill",
                column: "job_offer_job_role_id",
                principalTable: "job_offer_job_role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_user_job_offer_job_offer_id",
                table: "company_user");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_cities_city_id",
                table: "job_offer");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_companies_company_id",
                table: "job_offer");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_countries_country_id",
                table: "job_offer");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_benefit_job_offer_job_offer_id",
                table: "job_offer_benefit");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_job_role_job_offer_job_offer_id",
                table: "job_offer_job_role");

            migrationBuilder.DropForeignKey(
                name: "fk_job_offer_job_role_skill_job_offer_job_role_job_offer_job_r",
                table: "job_offer_job_role_skill");

            migrationBuilder.RenameColumn(
                name: "job_offer_job_role_id",
                table: "job_offer_job_role_skill",
                newName: "teammate_request_job_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_job_offer_job_role_skill_job_offer_job_role_id",
                table: "job_offer_job_role_skill",
                newName: "ix_job_offer_job_role_skill_teammate_request_job_role_id");

            migrationBuilder.RenameColumn(
                name: "job_offer_id",
                table: "job_offer_job_role",
                newName: "teammate_request_id");

            migrationBuilder.RenameIndex(
                name: "ix_job_offer_job_role_job_offer_id",
                table: "job_offer_job_role",
                newName: "ix_job_offer_job_role_teammate_request_id");

            migrationBuilder.RenameColumn(
                name: "job_offer_id",
                table: "job_offer_benefit",
                newName: "teammate_request_id");

            migrationBuilder.RenameIndex(
                name: "ix_job_offer_benefit_job_offer_id",
                table: "job_offer_benefit",
                newName: "ix_job_offer_benefit_teammate_request_id");

            migrationBuilder.AddColumn<int>(
                name: "teammate_request_id",
                table: "company_user",
                type: "integer",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_company_user_teammate_request_job_offer_id",
                table: "company_user",
                column: "job_offer_id",
                principalTable: "job_offer",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_city_city_id",
                table: "job_offer",
                column: "city_id",
                principalTable: "city",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_company_company_id",
                table: "job_offer",
                column: "company_id",
                principalTable: "company",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_country_country_id",
                table: "job_offer",
                column: "country_id",
                principalTable: "country",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_benefit_job_offer_teammate_request_id",
                table: "job_offer_benefit",
                column: "teammate_request_id",
                principalTable: "job_offer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_job_role_job_offer_teammate_request_id",
                table: "job_offer_job_role",
                column: "teammate_request_id",
                principalTable: "job_offer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_job_offer_job_role_skill_job_offer_job_role_teammate_reques",
                table: "job_offer_job_role_skill",
                column: "teammate_request_job_role_id",
                principalTable: "job_offer_job_role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
