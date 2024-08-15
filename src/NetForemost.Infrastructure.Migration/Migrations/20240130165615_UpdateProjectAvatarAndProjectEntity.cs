using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateProjectAvatarAndProjectEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_project_project_avatars_project_avatar_id",
                table: "project");

            migrationBuilder.DropIndex(
                name: "ix_project_project_avatar_id",
                table: "project");

            migrationBuilder.DropColumn(
                name: "project_avatar_id",
                table: "project");

            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "project_avatar",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "project_image_url",
                table: "project",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_project_avatar_project_id",
                table: "project_avatar",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "fk_project_avatar_project_project_id",
                table: "project_avatar",
                column: "project_id",
                principalTable: "project",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_project_avatar_project_project_id",
                table: "project_avatar");

            migrationBuilder.DropIndex(
                name: "ix_project_avatar_project_id",
                table: "project_avatar");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "project_avatar");

            migrationBuilder.DropColumn(
                name: "project_image_url",
                table: "project");

            migrationBuilder.AddColumn<int>(
                name: "project_avatar_id",
                table: "project",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_project_project_avatar_id",
                table: "project",
                column: "project_avatar_id");

            migrationBuilder.AddForeignKey(
                name: "fk_project_project_avatars_project_avatar_id",
                table: "project",
                column: "project_avatar_id",
                principalTable: "project_avatar",
                principalColumn: "id");
        }
    }
}
