using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class ChangedIsAccessibleForEveryoneFieldName_Project : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_accesible_for_everyone",
                table: "project",
                newName: "is_accessible_for_everyone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_accessible_for_everyone",
                table: "project",
                newName: "is_accesible_for_everyone");
        }
    }
}
