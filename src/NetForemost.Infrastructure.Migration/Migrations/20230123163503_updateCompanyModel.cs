using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class updateCompanyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "employees_number",
                table: "company",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "time_zone_id",
                table: "company",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_company_time_zone_id",
                table: "company",
                column: "time_zone_id");

            migrationBuilder.AddForeignKey(
                name: "fk_company_time_z_ones_time_zone_id",
                table: "company",
                column: "time_zone_id",
                principalTable: "time_zone",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_time_z_ones_time_zone_id",
                table: "company");

            migrationBuilder.DropIndex(
                name: "ix_company_time_zone_id",
                table: "company");

            migrationBuilder.DropColumn(
                name: "employees_number",
                table: "company");

            migrationBuilder.DropColumn(
                name: "time_zone_id",
                table: "company");
        }
    }
}
