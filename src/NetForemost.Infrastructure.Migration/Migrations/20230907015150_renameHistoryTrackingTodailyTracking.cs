using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class renameHistoryTrackingTodailyTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_block_time_tracking_time_history_tracking_time_history_trac",
                table: "block_time_tracking");

            migrationBuilder.DropTable(
                name: "time_history_tracking");

            migrationBuilder.DropTable(
                name: "tracking_block_image");

            migrationBuilder.RenameColumn(
                name: "time_history_tracking_id",
                table: "block_time_tracking",
                newName: "daily_tracking_id");

            migrationBuilder.RenameIndex(
                name: "ix_block_time_tracking_time_history_tracking_id",
                table: "block_time_tracking",
                newName: "ix_block_time_tracking_daily_tracking_id");

            migrationBuilder.CreateTable(
                name: "block_monitoring",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    keystrokes_min = table.Column<double>(type: "double precision", nullable: false),
                    mouse_movements_min = table.Column<double>(type: "double precision", nullable: false),
                    url_image = table.Column<string>(type: "text", nullable: false),
                    block_time_tracking_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_block_monitoring", x => x.id);
                    table.ForeignKey(
                        name: "fk_block_monitoring_block_time_trackings_block_time_tracking_id",
                        column: x => x.block_time_tracking_id,
                        principalTable: "block_time_tracking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "daily_tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    owner_id = table.Column<string>(type: "text", nullable: false),
                    total_tracking_time = table.Column<double>(type: "double precision", nullable: false),
                    keystrokes_min = table.Column<double>(type: "double precision", nullable: false),
                    mouse_movements_min = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_daily_tracking", x => x.id);
                    table.ForeignKey(
                        name: "fk_daily_tracking_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_block_monitoring_block_time_tracking_id",
                table: "block_monitoring",
                column: "block_time_tracking_id");

            migrationBuilder.CreateIndex(
                name: "ix_daily_tracking_owner_id",
                table: "daily_tracking",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_block_time_tracking_daily_tracking_daily_tracking_id",
                table: "block_time_tracking",
                column: "daily_tracking_id",
                principalTable: "daily_tracking",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_block_time_tracking_daily_tracking_daily_tracking_id",
                table: "block_time_tracking");

            migrationBuilder.DropTable(
                name: "block_monitoring");

            migrationBuilder.DropTable(
                name: "daily_tracking");

            migrationBuilder.RenameColumn(
                name: "daily_tracking_id",
                table: "block_time_tracking",
                newName: "time_history_tracking_id");

            migrationBuilder.RenameIndex(
                name: "ix_block_time_tracking_daily_tracking_id",
                table: "block_time_tracking",
                newName: "ix_block_time_tracking_time_history_tracking_id");

            migrationBuilder.CreateTable(
                name: "time_history_tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    date_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    keystrokes_min = table.Column<double>(type: "double precision", nullable: false),
                    mouse_movements_min = table.Column<double>(type: "double precision", nullable: false),
                    total_tracking_time = table.Column<double>(type: "double precision", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_time_history_tracking", x => x.id);
                    table.ForeignKey(
                        name: "fk_time_history_tracking_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tracking_block_image",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    block_time_tracking_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    keystrokes_min = table.Column<double>(type: "double precision", nullable: false),
                    mouse_movements_min = table.Column<double>(type: "double precision", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    url_image = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tracking_block_image", x => x.id);
                    table.ForeignKey(
                        name: "fk_tracking_block_image_block_time_tracking_block_time_trackin",
                        column: x => x.block_time_tracking_id,
                        principalTable: "block_time_tracking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_time_history_tracking_user_id",
                table: "time_history_tracking",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tracking_block_image_block_time_tracking_id",
                table: "tracking_block_image",
                column: "block_time_tracking_id");

            migrationBuilder.AddForeignKey(
                name: "fk_block_time_tracking_time_history_tracking_time_history_trac",
                table: "block_time_tracking",
                column: "time_history_tracking_id",
                principalTable: "time_history_tracking",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
