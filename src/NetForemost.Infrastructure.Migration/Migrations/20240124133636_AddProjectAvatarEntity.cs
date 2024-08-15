using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddProjectAvatarEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "project_image_url",
                table: "project");

            migrationBuilder.AddColumn<int>(
                name: "project_avatar_id",
                table: "project",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "project_avatar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    project_image_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    archived_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    archived_by = table.Column<string>(type: "text", nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_avatar", x => x.id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_project_project_avatars_project_avatar_id",
                table: "project");

            migrationBuilder.DropTable(
                name: "project_avatar");

            migrationBuilder.DropIndex(
                name: "ix_project_project_avatar_id",
                table: "project");

            migrationBuilder.DropColumn(
                name: "project_avatar_id",
                table: "project");

            migrationBuilder.AddColumn<string>(
                name: "project_image_url",
                table: "project",
                type: "text",
                nullable: true);
        }
    }
}
