using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class RemoveRelationBetweenUserEntityAndGOalEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_goal_users_owner_id",
                table: "goal");

            migrationBuilder.DropIndex(
                name: "ix_goal_owner_id",
                table: "goal");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "goal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "owner_id",
                table: "goal",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_goal_owner_id",
                table: "goal",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_goal_users_owner_id",
                table: "goal",
                column: "owner_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
