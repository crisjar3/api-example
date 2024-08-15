using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddIndustryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "industry_id",
                table: "company",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "industry",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_industry", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_company_industry_id",
                table: "company",
                column: "industry_id");

            migrationBuilder.AddForeignKey(
                name: "fk_company_industries_industry_id",
                table: "company",
                column: "industry_id",
                principalTable: "industry",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_industries_industry_id",
                table: "company");

            migrationBuilder.DropTable(
                name: "industry");

            migrationBuilder.DropIndex(
                name: "ix_company_industry_id",
                table: "company");

            migrationBuilder.DropColumn(
                name: "industry_id",
                table: "company");
        }
    }
}
