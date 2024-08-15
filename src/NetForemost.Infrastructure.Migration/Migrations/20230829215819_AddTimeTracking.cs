using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddTimeTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_tracking");

            migrationBuilder.DropTable(
                name: "monitoring");

            migrationBuilder.DropTable(
                name: "time_tracking");

            migrationBuilder.CreateTable(
                name: "time_history_tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("pk_time_history_tracking", x => x.id);
                    table.ForeignKey(
                        name: "fk_time_history_tracking_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "block_time_tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    time_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    time_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    task_id = table.Column<int>(type: "integer", nullable: false),
                    device_id = table.Column<int>(type: "integer", nullable: true),
                    time_history_tracking_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_block_time_tracking", x => x.id);
                    table.ForeignKey(
                        name: "fk_block_time_tracking_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "device",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_block_time_tracking_task_task_id",
                        column: x => x.task_id,
                        principalTable: "task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_block_time_tracking_time_history_tracking_time_history_trac",
                        column: x => x.time_history_tracking_id,
                        principalTable: "time_history_tracking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tracking_block_image",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    keystrokes_min = table.Column<double>(type: "double precision", nullable: false),
                    mouse_movements_min = table.Column<double>(type: "double precision", nullable: false),
                    block_time_tracking_id = table.Column<int>(type: "integer", nullable: false),
                    url_image = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
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
                name: "ix_block_time_tracking_device_id",
                table: "block_time_tracking",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_block_time_tracking_task_id",
                table: "block_time_tracking",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "ix_block_time_tracking_time_history_tracking_id",
                table: "block_time_tracking",
                column: "time_history_tracking_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_history_tracking_user_id",
                table: "time_history_tracking",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tracking_block_image_block_time_tracking_id",
                table: "tracking_block_image",
                column: "block_time_tracking_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tracking_block_image");

            migrationBuilder.DropTable(
                name: "block_time_tracking");

            migrationBuilder.DropTable(
                name: "time_history_tracking");

            migrationBuilder.CreateTable(
                name: "time_tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<int>(type: "integer", nullable: false),
                    task_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    started = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    time_tracked = table.Column<double>(type: "double precision", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_time_tracking", x => x.id);
                    table.ForeignKey(
                        name: "fk_time_tracking_device_device_id",
                        column: x => x.device_id,
                        principalTable: "device",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_time_tracking_task_task_id",
                        column: x => x.task_id,
                        principalTable: "task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "monitoring",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    time_tracking_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    keystrokes_min = table.Column<double>(type: "double precision", nullable: false),
                    mouse_movements_min = table.Column<double>(type: "double precision", nullable: false),
                    registered = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    screenshot = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_monitoring", x => x.id);
                    table.ForeignKey(
                        name: "fk_monitoring_time_trackings_time_tracking_id",
                        column: x => x.time_tracking_id,
                        principalTable: "time_tracking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    monitoring_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    time_tracked = table.Column<double>(type: "double precision", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_tracking", x => x.id);
                    table.ForeignKey(
                        name: "fk_app_tracking_monitorings_monitoring_id",
                        column: x => x.monitoring_id,
                        principalTable: "monitoring",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_app_tracking_monitoring_id",
                table: "app_tracking",
                column: "monitoring_id");

            migrationBuilder.CreateIndex(
                name: "ix_monitoring_time_tracking_id",
                table: "monitoring",
                column: "time_tracking_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_tracking_device_id",
                table: "time_tracking",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_tracking_task_id",
                table: "time_tracking",
                column: "task_id");
        }
    }
}
