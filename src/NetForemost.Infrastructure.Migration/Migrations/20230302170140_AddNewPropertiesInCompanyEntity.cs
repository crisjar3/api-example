using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddNewPropertiesInCompanyEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_cities_city_id",
                table: "company");

            migrationBuilder.RenameColumn(
                name: "zip",
                table: "company",
                newName: "zip_code");

            migrationBuilder.AlterColumn<int>(
                name: "employees_number",
                table: "company",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "city_id",
                table: "company",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "address1",
                table: "company",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "company",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "website",
                table: "company",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_company_cities_city_id",
                table: "company",
                column: "city_id",
                principalTable: "city",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_cities_city_id",
                table: "company");

            migrationBuilder.DropColumn(
                name: "description",
                table: "company");

            migrationBuilder.DropColumn(
                name: "website",
                table: "company");

            migrationBuilder.RenameColumn(
                name: "zip_code",
                table: "company",
                newName: "zip");

            migrationBuilder.AlterColumn<int>(
                name: "employees_number",
                table: "company",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "city_id",
                table: "company",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "address1",
                table: "company",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_company_cities_city_id",
                table: "company",
                column: "city_id",
                principalTable: "city",
                principalColumn: "id");
        }
    }
}
