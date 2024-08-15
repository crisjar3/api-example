using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class deleteDailyEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "daily_entry");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "daily_entry",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    owner_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    keystrokes_min = table.Column<double>(type: "double precision", nullable: false),
                    mouse_movements_min = table.Column<double>(type: "double precision", nullable: false),
                    time_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    time_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    total_tracking_time = table.Column<double>(type: "double precision", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_daily_entry", x => x.id);
                    table.ForeignKey(
                        name: "fk_daily_entry_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_daily_entry_owner_id",
                table: "daily_entry",
                column: "owner_id");
        }
    }
}
