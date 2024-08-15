using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateNamingFromDailyTrackingToDailyEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "block_monitoring");

            migrationBuilder.DropTable(
                name: "block_time_tracking");

            migrationBuilder.DropTable(
                name: "daily_tracking");

            migrationBuilder.CreateTable(
                name: "daily_entry",
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
                    table.PrimaryKey("pk_daily_entry", x => x.id);
                    table.ForeignKey(
                        name: "fk_daily_entry_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "daily_time_block",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    time_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    time_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    task_id = table.Column<int>(type: "integer", nullable: false),
                    device_id = table.Column<int>(type: "integer", nullable: true),
                    daily_entry_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_daily_time_block", x => x.id);
                    table.ForeignKey(
                        name: "fk_daily_time_block_daily_entry_daily_entry_id",
                        column: x => x.daily_entry_id,
                        principalTable: "daily_entry",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_daily_time_block_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "device",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_daily_time_block_work_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "daily_monitoring_block",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    keystrokes_min = table.Column<double>(type: "double precision", nullable: false),
                    mouse_movements_min = table.Column<double>(type: "double precision", nullable: false),
                    url_image = table.Column<string>(type: "text", nullable: false),
                    daily_time_block_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_daily_monitoring_block", x => x.id);
                    table.ForeignKey(
                        name: "fk_daily_monitoring_block_block_time_trackings_daily_time_bloc",
                        column: x => x.daily_time_block_id,
                        principalTable: "daily_time_block",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_daily_entry_owner_id",
                table: "daily_entry",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_daily_monitoring_block_daily_time_block_id",
                table: "daily_monitoring_block",
                column: "daily_time_block_id");

            migrationBuilder.CreateIndex(
                name: "ix_daily_time_block_daily_entry_id",
                table: "daily_time_block",
                column: "daily_entry_id");

            migrationBuilder.CreateIndex(
                name: "ix_daily_time_block_device_id",
                table: "daily_time_block",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_daily_time_block_task_id",
                table: "daily_time_block",
                column: "task_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "daily_monitoring_block");

            migrationBuilder.DropTable(
                name: "daily_time_block");

            migrationBuilder.DropTable(
                name: "daily_entry");

            migrationBuilder.CreateTable(
                name: "daily_tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    owner_id = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("pk_daily_tracking", x => x.id);
                    table.ForeignKey(
                        name: "fk_daily_tracking_users_owner_id",
                        column: x => x.owner_id,
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
                    daily_tracking_id = table.Column<int>(type: "integer", nullable: false),
                    device_id = table.Column<int>(type: "integer", nullable: true),
                    task_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    time_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    time_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_block_time_tracking", x => x.id);
                    table.ForeignKey(
                        name: "fk_block_time_tracking_daily_tracking_daily_tracking_id",
                        column: x => x.daily_tracking_id,
                        principalTable: "daily_tracking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                });

            migrationBuilder.CreateTable(
                name: "block_monitoring",
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
                    table.PrimaryKey("pk_block_monitoring", x => x.id);
                    table.ForeignKey(
                        name: "fk_block_monitoring_block_time_trackings_block_time_tracking_id",
                        column: x => x.block_time_tracking_id,
                        principalTable: "block_time_tracking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_block_monitoring_block_time_tracking_id",
                table: "block_monitoring",
                column: "block_time_tracking_id");

            migrationBuilder.CreateIndex(
                name: "ix_block_time_tracking_daily_tracking_id",
                table: "block_time_tracking",
                column: "daily_tracking_id");

            migrationBuilder.CreateIndex(
                name: "ix_block_time_tracking_device_id",
                table: "block_time_tracking",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_block_time_tracking_task_id",
                table: "block_time_tracking",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "ix_daily_tracking_owner_id",
                table: "daily_tracking",
                column: "owner_id");
        }
    }
}
