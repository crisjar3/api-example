using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateProjectAvatarEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "project_image_url",
                table: "project_avatar",
                newName: "avatar_image_url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "avatar_image_url",
                table: "project_avatar",
                newName: "project_image_url");
        }
    }
}
