using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class UpdateUserEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "project_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_subscriptiontype_feature",
                table: "subscription_type_feature");

            migrationBuilder.DropPrimaryKey(
                name: "pk_project_group",
                table: "project_group");

            migrationBuilder.DropPrimaryKey(
                name: "pk_group_user",
                table: "group_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_company_user",
                table: "company_user");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "subscription_type_feature",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "skill",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "project_group",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "group_user",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "company_user",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "company_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "role_id",
                table: "company_user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "teammate_request_id",
                table: "company_user",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "job_role_id",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "registered",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "seniority_id",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_subscription_type_feature",
                table: "subscription_type_feature",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_project_group",
                table: "project_group",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_group_user",
                table: "group_user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_company_user",
                table: "company_user",
                column: "id");

            migrationBuilder.CreateTable(
                name: "project_company_user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    project_id = table.Column<int>(type: "integer", nullable: false),
                    company_user_id = table.Column<int>(type: "integer", nullable: false),
                    job_role_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_company_user", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_company_user_company_user_company_user_id",
                        column: x => x.company_user_id,
                        principalTable: "company_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_project_company_user_job_role_job_role_id",
                        column: x => x.job_role_id,
                        principalTable: "job_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_project_company_user_project_project_id",
                        column: x => x.project_id,
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_subscription_type_feature_feature_id",
                table: "subscription_type_feature",
                column: "feature_id");

            migrationBuilder.CreateIndex(
                name: "ix_skill_user_id",
                table: "skill",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_group_project_id",
                table: "project_group",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_group_user_group_id",
                table: "group_user",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_user_company_id",
                table: "company_user",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_user_role_id",
                table: "company_user",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_user_teammate_request_id",
                table: "company_user",
                column: "teammate_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_job_role_id",
                table: "AspNetUsers",
                column: "job_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_seniority_id",
                table: "AspNetUsers",
                column: "seniority_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_company_user_company_user_id",
                table: "project_company_user",
                column: "company_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_company_user_job_role_id",
                table: "project_company_user",
                column: "job_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_company_user_project_id",
                table: "project_company_user",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_job_role_job_role_id",
                table: "AspNetUsers",
                column: "job_role_id",
                principalTable: "job_role",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_seniority_seniority_id",
                table: "AspNetUsers",
                column: "seniority_id",
                principalTable: "seniority",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_company_user_roles_role_id",
                table: "company_user",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_company_user_teammate_request_teammate_request_id",
                table: "company_user",
                column: "teammate_request_id",
                principalTable: "teammate_request",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_skill_users_user_id",
                table: "skill",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_job_role_job_role_id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_seniority_seniority_id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "fk_company_user_roles_role_id",
                table: "company_user");

            migrationBuilder.DropForeignKey(
                name: "fk_company_user_teammate_request_teammate_request_id",
                table: "company_user");

            migrationBuilder.DropForeignKey(
                name: "fk_skill_users_user_id",
                table: "skill");

            migrationBuilder.DropTable(
                name: "project_company_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_subscription_type_feature",
                table: "subscription_type_feature");

            migrationBuilder.DropIndex(
                name: "ix_subscription_type_feature_feature_id",
                table: "subscription_type_feature");

            migrationBuilder.DropIndex(
                name: "ix_skill_user_id",
                table: "skill");

            migrationBuilder.DropPrimaryKey(
                name: "pk_project_group",
                table: "project_group");

            migrationBuilder.DropIndex(
                name: "ix_project_group_project_id",
                table: "project_group");

            migrationBuilder.DropPrimaryKey(
                name: "pk_group_user",
                table: "group_user");

            migrationBuilder.DropIndex(
                name: "ix_group_user_group_id",
                table: "group_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_company_user",
                table: "company_user");

            migrationBuilder.DropIndex(
                name: "ix_company_user_company_id",
                table: "company_user");

            migrationBuilder.DropIndex(
                name: "ix_company_user_role_id",
                table: "company_user");

            migrationBuilder.DropIndex(
                name: "ix_company_user_teammate_request_id",
                table: "company_user");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_job_role_id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_seniority_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "id",
                table: "subscription_type_feature");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "skill");

            migrationBuilder.DropColumn(
                name: "id",
                table: "project_group");

            migrationBuilder.DropColumn(
                name: "id",
                table: "group_user");

            migrationBuilder.DropColumn(
                name: "id",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "teammate_request_id",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "job_role_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "registered",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "seniority_id",
                table: "AspNetUsers");

            migrationBuilder.AddPrimaryKey(
                name: "pk_subscriptiontype_feature",
                table: "subscription_type_feature",
                columns: new[] { "feature_id", "subscription_type_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_project_group",
                table: "project_group",
                columns: new[] { "project_id", "group_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_group_user",
                table: "group_user",
                columns: new[] { "group_id", "user_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_company_user",
                table: "company_user",
                columns: new[] { "company_id", "user_id" });

            migrationBuilder.CreateTable(
                name: "project_user",
                columns: table => new
                {
                    project_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_user", x => new { x.project_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_project_user_project_project_id",
                        column: x => x.project_id,
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_project_user_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_project_user_user_id",
                table: "project_user",
                column: "user_id");
        }
    }
}
