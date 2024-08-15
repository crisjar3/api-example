using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateCompanyUserFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "first_name",
                table: "company_user");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "company_user",
                newName: "user_name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "company_user",
                newName: "last_name");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "company_user",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
