using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class CreateWorkRoleAndWorkRoleTranslationsEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "work_role_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_role_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "work_role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    work_role_category_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_role", x => x.id);
                    table.ForeignKey(
                        name: "fk_work_role_work_role_categories_work_role_category_id",
                        column: x => x.work_role_category_id,
                        principalTable: "work_role_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_role_category_translation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    work_role_category_id = table.Column<int>(type: "integer", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_role_category_translation", x => x.id);
                    table.ForeignKey(
                        name: "fk_work_role_category_translation_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_work_role_category_translation_work_role_category_work_role",
                        column: x => x.work_role_category_id,
                        principalTable: "work_role_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_role_translation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    work_role_id = table.Column<int>(type: "integer", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_role_translation", x => x.id);
                    table.ForeignKey(
                        name: "fk_work_role_translation_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_work_role_translation_work_role_work_role_id",
                        column: x => x.work_role_id,
                        principalTable: "work_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_work_role_work_role_category_id",
                table: "work_role",
                column: "work_role_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_role_category_translation_language_id",
                table: "work_role_category_translation",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_role_category_translation_work_role_category_id",
                table: "work_role_category_translation",
                column: "work_role_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_role_translation_language_id",
                table: "work_role_translation",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_role_translation_work_role_id",
                table: "work_role_translation",
                column: "work_role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "work_role_category_translation");

            migrationBuilder.DropTable(
                name: "work_role_translation");

            migrationBuilder.DropTable(
                name: "work_role");

            migrationBuilder.DropTable(
                name: "work_role_category");
        }
    }
}
