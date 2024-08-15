using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class CreateEntityUserType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_type_id",
                table: "company_user",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "user_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_type", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_company_user_user_type_id",
                table: "company_user",
                column: "user_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_company_user_user_type_user_type_id",
                table: "company_user",
                column: "user_type_id",
                principalTable: "user_type",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_company_user_user_type_user_type_id",
                table: "company_user");

            migrationBuilder.DropTable(
                name: "user_type");

            migrationBuilder.DropIndex(
                name: "ix_company_user_user_type_id",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "user_type_id",
                table: "company_user");
        }
    }
}
