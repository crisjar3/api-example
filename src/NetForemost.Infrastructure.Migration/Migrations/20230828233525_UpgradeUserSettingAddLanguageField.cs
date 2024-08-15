using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpgradeUserSettingAddLanguageField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "language_id",
                table: "user_settings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_user_settings_language_id",
                table: "user_settings",
                column: "language_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_settings_languages_language_id",
                table: "user_settings",
                column: "language_id",
                principalTable: "language",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_settings_languages_language_id",
                table: "user_settings");

            migrationBuilder.DropIndex(
                name: "ix_user_settings_language_id",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "language_id",
                table: "user_settings");
        }
    }
}
