using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class updateNamesOfBaseEntityAtributtes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "user_skill",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "user_skill",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "user_skill",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "user_settings",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "user_settings",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "user_settings",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "user_refresh_token",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "user_refresh_token",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "user_refresh_token",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "user_language",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "user_language",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "user_language",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "time_zone",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "time_zone",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "time_zone",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "task_type",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "task_type",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "task_type",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "task",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "task",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "task",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "subscription_type_feature",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "subscription_type_feature",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "subscription_type_feature",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "subscription_type",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "subscription_type",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "subscription_type",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "subscription",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "subscription",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "subscription",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "story_point",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "story_point",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "story_point",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "skill",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "skill",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "skill",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "seniority",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "seniority",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "seniority",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "role_translation",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "role_translation",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "role_translation",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "project_group",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "project_group",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "project_group",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "project_company_user",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "project_company_user",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "project_company_user",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "project",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "project",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "project",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "priority_level_translation",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "priority_level_translation",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "priority_level_translation",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "priority_level",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "priority_level",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "priority_level",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "policy",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "policy",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "policy",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "language_level",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "language_level",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "language_level",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "language",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "language",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "language",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "job_role_translation",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "job_role_translation",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "job_role_translation",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "job_role_category_translation",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "job_role_category_translation",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "job_role_category_translation",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "job_role_category_skill",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "job_role_category_skill",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "job_role_category_skill",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "job_role_category",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "job_role_category",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "job_role_category",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "job_role",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "job_role",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "job_role",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "job_offer_talent_skill",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "job_offer_talent_skill",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "job_offer_talent_skill",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "job_offer_talent",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "job_offer_talent",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "job_offer_talent",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "job_offer_benefit",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "job_offer_benefit",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "job_offer_benefit",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "job_offer",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "job_offer",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "job_offer",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "industry",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "industry",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "industry",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "group_user",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "group_user",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "group_user",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "group",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "group",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "group",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "goal_status_category",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "goal_status_category",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "goal_status_category",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "goal_status",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "goal_status",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "goal_status",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "goal_extra_mile",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "goal_extra_mile",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "goal_extra_mile",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "goal",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "goal",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "goal",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "feature",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "feature",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "feature",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "device_type",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "device_type",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "device_type",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "device",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "device",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "device",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "daily_time_block",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "daily_time_block",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "daily_time_block",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "daily_monitoring_block",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "daily_monitoring_block",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "daily_monitoring_block",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "country",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "country",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "country",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "contract_type",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "contract_type",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "contract_type",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "company_user",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "company_user",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "company_user",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "company_settings",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "company_settings",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "company_settings",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "company",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "company",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "company",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "city",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "city",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "city",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "billing_history",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "billing_history",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "billing_history",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "benefit",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "benefit",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "benefit",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "is_delete",
                table: "app_client",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "delete_by",
                table: "app_client",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "delete_at",
                table: "app_client",
                newName: "deleted_at");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "user_skill",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "user_skill",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "user_skill",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "user_settings",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "user_settings",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "user_settings",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "user_refresh_token",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "user_refresh_token",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "user_refresh_token",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "user_language",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "user_language",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "user_language",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "time_zone",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "time_zone",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "time_zone",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "task_type",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "task_type",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "task_type",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "task",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "task",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "task",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "subscription_type_feature",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "subscription_type_feature",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "subscription_type_feature",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "subscription_type",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "subscription_type",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "subscription_type",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "subscription",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "subscription",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "subscription",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "story_point",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "story_point",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "story_point",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "skill",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "skill",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "skill",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "seniority",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "seniority",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "seniority",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "role_translation",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "role_translation",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "role_translation",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "project_group",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "project_group",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "project_group",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "project_company_user",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "project_company_user",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "project_company_user",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "project",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "project",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "project",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "priority_level_translation",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "priority_level_translation",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "priority_level_translation",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "priority_level",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "priority_level",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "priority_level",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "policy",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "policy",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "policy",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "language_level",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "language_level",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "language_level",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "language",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "language",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "language",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "job_role_translation",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "job_role_translation",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "job_role_translation",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "job_role_category_translation",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "job_role_category_translation",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "job_role_category_translation",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "job_role_category_skill",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "job_role_category_skill",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "job_role_category_skill",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "job_role_category",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "job_role_category",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "job_role_category",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "job_role",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "job_role",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "job_role",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "job_offer_talent_skill",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "job_offer_talent_skill",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "job_offer_talent_skill",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "job_offer_talent",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "job_offer_talent",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "job_offer_talent",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "job_offer_benefit",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "job_offer_benefit",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "job_offer_benefit",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "job_offer",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "job_offer",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "job_offer",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "industry",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "industry",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "industry",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "group_user",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "group_user",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "group_user",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "group",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "group",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "group",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "goal_status_category",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "goal_status_category",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "goal_status_category",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "goal_status",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "goal_status",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "goal_status",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "goal_extra_mile",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "goal_extra_mile",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "goal_extra_mile",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "goal",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "goal",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "goal",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "feature",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "feature",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "feature",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "device_type",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "device_type",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "device_type",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "device",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "device",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "device",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "daily_time_block",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "daily_time_block",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "daily_time_block",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "daily_monitoring_block",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "daily_monitoring_block",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "daily_monitoring_block",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "country",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "country",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "country",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "contract_type",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "contract_type",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "contract_type",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "company_user",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "company_user",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "company_user",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "company_settings",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "company_settings",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "company_settings",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "company",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "company",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "company",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "city",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "city",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "city",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "billing_history",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "billing_history",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "billing_history",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "benefit",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "benefit",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "benefit",
                newName: "delete_at");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "app_client",
                newName: "is_delete");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "app_client",
                newName: "delete_by");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "app_client",
                newName: "delete_at");
        }
    }
}
