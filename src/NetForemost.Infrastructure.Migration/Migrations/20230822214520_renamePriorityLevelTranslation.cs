using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class renamePriorityLevelTranslation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "priority_level_traslation");

            migrationBuilder.CreateTable(
                name: "priority_level_translation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    level = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: false),
                    priority_level_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_priority_level_translation", x => x.id);
                    table.ForeignKey(
                        name: "fk_priority_level_translation_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_priority_level_translation_priority_level_priority_level_id",
                        column: x => x.priority_level_id,
                        principalTable: "priority_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_priority_level_translation_language_id",
                table: "priority_level_translation",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_priority_level_translation_priority_level_id",
                table: "priority_level_translation",
                column: "priority_level_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "priority_level_translation");

            migrationBuilder.CreateTable(
                name: "priority_level_traslation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    language_id = table.Column<int>(type: "integer", nullable: false),
                    priority_level_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    level = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_priority_level_traslation", x => x.id);
                    table.ForeignKey(
                        name: "fk_priority_level_traslation_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_priority_level_traslation_priority_level_priority_level_id",
                        column: x => x.priority_level_id,
                        principalTable: "priority_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_priority_level_traslation_language_id",
                table: "priority_level_traslation",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_priority_level_traslation_priority_level_id",
                table: "priority_level_traslation",
                column: "priority_level_id");
        }
    }
}
