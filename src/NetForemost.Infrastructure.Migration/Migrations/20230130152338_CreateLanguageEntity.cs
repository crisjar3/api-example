using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class CreateLanguageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_time_z_ones_time_zone_id",
                table: "company");

            migrationBuilder.DropForeignKey(
                name: "fk_user_settings_time_z_ones_time_zone_id",
                table: "user_settings");

            migrationBuilder.CreateTable(
                name: "language",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    @default = table.Column<bool>(name: "default", type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_language", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_company_time_zones_time_zone_id",
                table: "company",
                column: "time_zone_id",
                principalTable: "time_zone",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_settings_time_zones_time_zone_id",
                table: "user_settings",
                column: "time_zone_id",
                principalTable: "time_zone",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_time_zones_time_zone_id",
                table: "company");

            migrationBuilder.DropForeignKey(
                name: "fk_user_settings_time_zones_time_zone_id",
                table: "user_settings");

            migrationBuilder.DropTable(
                name: "language");

            migrationBuilder.AddForeignKey(
                name: "fk_company_time_z_ones_time_zone_id",
                table: "company",
                column: "time_zone_id",
                principalTable: "time_zone",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_settings_time_z_ones_time_zone_id",
                table: "user_settings",
                column: "time_zone_id",
                principalTable: "time_zone",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
