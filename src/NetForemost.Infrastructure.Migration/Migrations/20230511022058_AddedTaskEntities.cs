using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddedTaskEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "owner_id",
                table: "task",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "target_end_date",
                table: "task",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "type_id",
                table: "task",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "task_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_type", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_type_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_task_owner_id",
                table: "task",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_type_id",
                table: "task",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_type_company_id",
                table: "task_type",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "fk_task_task_types_type_id",
                table: "task",
                column: "type_id",
                principalTable: "task_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_task_users_owner_id",
                table: "task",
                column: "owner_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_task_task_types_type_id",
                table: "task");

            migrationBuilder.DropForeignKey(
                name: "fk_task_users_owner_id",
                table: "task");

            migrationBuilder.DropTable(
                name: "task_type");

            migrationBuilder.DropIndex(
                name: "ix_task_owner_id",
                table: "task");

            migrationBuilder.DropIndex(
                name: "ix_task_type_id",
                table: "task");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "task");

            migrationBuilder.DropColumn(
                name: "target_end_date",
                table: "task");

            migrationBuilder.DropColumn(
                name: "type_id",
                table: "task");
        }
    }
}
