using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class RemoveRelationBetweenUserEntityAndTaskEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_task_users_owner_id",
                table: "task");

            migrationBuilder.DropIndex(
                name: "ix_task_owner_id",
                table: "task");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "task");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_task_users_user_id",
                table: "task");

            migrationBuilder.DropIndex(
                name: "ix_task_user_id",
                table: "task");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "task");

            migrationBuilder.AddColumn<string>(
                name: "owner_id",
                table: "task",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_task_owner_id",
                table: "task",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_task_users_owner_id",
                table: "task",
                column: "owner_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
