using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddDeleteFieldsToBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "user_skill",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "user_skill",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "user_skill",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "user_settings",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "user_settings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "user_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "user_refresh_token",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "user_refresh_token",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "user_refresh_token",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "user_language",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "user_language",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "user_language",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "time_zone",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "time_zone",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "time_zone",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "task_type",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "task_type",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "task_type",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "task",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "task",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "task",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "subscription_type_feature",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "subscription_type_feature",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "subscription_type_feature",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "subscription_type",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "subscription_type",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "subscription_type",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "subscription",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "subscription",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "subscription",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "story_point",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "story_point",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "story_point",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "skill",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "skill",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "skill",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "seniority",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "seniority",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "seniority",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "role_translation",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "role_translation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "role_translation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "project_group",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "project_group",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "project_group",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "project_company_user",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "project_company_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "project_company_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "project",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "project",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "project",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "priority_level_translation",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "priority_level_translation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "priority_level_translation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "priority_level",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "priority_level",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "priority_level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "policy",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "policy",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "policy",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "language_level",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "language_level",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "language_level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "language",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "language",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "language",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "job_role_translation",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "job_role_translation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "job_role_translation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "job_role_category_translation",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "job_role_category_translation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "job_role_category_translation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "job_role_category_skill",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "job_role_category_skill",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "job_role_category_skill",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "job_role_category",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "job_role_category",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "job_role_category",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "job_role",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "job_role",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "job_role",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "job_offer_talent_skill",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "job_offer_talent_skill",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "job_offer_talent_skill",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "job_offer_talent",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "job_offer_talent",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "job_offer_talent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "job_offer_benefit",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "job_offer_benefit",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "job_offer_benefit",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "job_offer",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "job_offer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "job_offer",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "industry",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "industry",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "industry",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "group_user",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "group_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "group_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "group",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "group",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "group",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "goal_status_category",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "goal_status_category",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "goal_status_category",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "goal_status",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "goal_status",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "goal_status",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "goal_extra_mile",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "goal_extra_mile",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "goal_extra_mile",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "goal",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "goal",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "goal",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "feature",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "feature",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "feature",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "device_type",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "device_type",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "device_type",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "device",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "device",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "device",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "daily_time_block",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "daily_time_block",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "daily_time_block",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "daily_monitoring_block",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "daily_monitoring_block",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "daily_monitoring_block",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "country",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "country",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "country",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "contract_type",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "contract_type",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "contract_type",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "company_user",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "company_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "company_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "company_settings",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "company_settings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "company_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "company",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "company",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "company",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "city",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "city",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "city",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "billing_history",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "billing_history",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "billing_history",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "benefit",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "benefit",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "benefit",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delete_at",
                table: "app_client",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delete_by",
                table: "app_client",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_delete",
                table: "app_client",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "user_skill");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "user_skill");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "user_skill");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "user_refresh_token");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "user_refresh_token");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "user_refresh_token");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "user_language");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "user_language");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "user_language");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "time_zone");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "time_zone");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "time_zone");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "task_type");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "task_type");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "task_type");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "task");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "task");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "task");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "subscription_type_feature");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "subscription_type_feature");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "subscription_type_feature");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "subscription_type");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "subscription_type");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "subscription_type");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "subscription");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "subscription");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "subscription");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "story_point");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "story_point");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "story_point");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "skill");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "skill");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "skill");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "seniority");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "seniority");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "seniority");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "role_translation");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "role_translation");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "role_translation");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "project_group");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "project_group");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "project_group");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "project_company_user");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "project_company_user");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "project_company_user");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "project");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "project");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "project");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "priority_level_translation");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "priority_level_translation");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "priority_level_translation");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "priority_level");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "priority_level");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "priority_level");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "policy");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "policy");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "policy");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "language_level");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "language_level");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "language_level");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "language");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "language");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "language");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "job_role_translation");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "job_role_translation");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "job_role_translation");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "job_role_category_translation");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "job_role_category_translation");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "job_role_category_translation");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "job_role_category_skill");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "job_role_category_skill");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "job_role_category_skill");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "job_role_category");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "job_role_category");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "job_role_category");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "job_role");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "job_role");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "job_role");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "job_offer_talent_skill");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "job_offer_talent_skill");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "job_offer_talent_skill");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "job_offer_talent");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "job_offer_talent");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "job_offer_talent");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "job_offer_benefit");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "job_offer_benefit");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "job_offer_benefit");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "job_offer");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "job_offer");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "job_offer");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "industry");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "industry");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "industry");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "group_user");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "group_user");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "group_user");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "group");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "group");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "group");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "goal_status_category");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "goal_status_category");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "goal_status_category");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "goal_status");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "goal_status");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "goal_status");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "goal_extra_mile");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "goal_extra_mile");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "goal_extra_mile");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "goal");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "goal");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "goal");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "feature");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "feature");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "feature");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "device_type");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "device_type");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "device_type");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "device");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "device");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "device");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "daily_time_block");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "daily_time_block");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "daily_time_block");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "daily_monitoring_block");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "daily_monitoring_block");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "daily_monitoring_block");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "country");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "country");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "country");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "contract_type");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "contract_type");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "contract_type");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "company_settings");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "company_settings");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "company_settings");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "company");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "company");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "company");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "city");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "city");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "city");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "billing_history");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "billing_history");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "billing_history");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "benefit");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "benefit");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "benefit");

            migrationBuilder.DropColumn(
                name: "delete_at",
                table: "app_client");

            migrationBuilder.DropColumn(
                name: "delete_by",
                table: "app_client");

            migrationBuilder.DropColumn(
                name: "is_delete",
                table: "app_client");
        }
    }
}
