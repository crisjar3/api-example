using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateBenefitEntityAddCompanyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_teammate_request_countries_country_id",
                table: "teammate_request");

            migrationBuilder.AlterColumn<int>(
                name: "country_id",
                table: "teammate_request",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "benefit",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "benefit",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_benefit_company_id",
                table: "benefit",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "fk_benefit_companies_company_id",
                table: "benefit",
                column: "company_id",
                principalTable: "company",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_teammate_request_countries_country_id",
                table: "teammate_request",
                column: "country_id",
                principalTable: "country",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_benefit_companies_company_id",
                table: "benefit");

            migrationBuilder.DropForeignKey(
                name: "fk_teammate_request_countries_country_id",
                table: "teammate_request");

            migrationBuilder.DropIndex(
                name: "ix_benefit_company_id",
                table: "benefit");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "benefit");

            migrationBuilder.DropColumn(
                name: "is_default",
                table: "benefit");

            migrationBuilder.AlterColumn<int>(
                name: "country_id",
                table: "teammate_request",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_teammate_request_countries_country_id",
                table: "teammate_request",
                column: "country_id",
                principalTable: "country",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
