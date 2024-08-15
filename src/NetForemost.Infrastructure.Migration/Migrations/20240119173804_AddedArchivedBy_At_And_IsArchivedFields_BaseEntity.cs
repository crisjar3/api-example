using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddedArchivedBy_At_And_IsArchivedFields_BaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "user_skill",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "user_skill",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "user_skill",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "user_settings",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "user_settings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "user_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "user_refresh_token",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "user_refresh_token",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "user_refresh_token",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "user_language",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "user_language",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "user_language",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "time_zone",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "time_zone",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "time_zone",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "task_type",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "task_type",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "task_type",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "task",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "task",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "task",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "subscription_type_feature",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "subscription_type_feature",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "subscription_type_feature",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "subscription_type",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "subscription_type",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "subscription_type",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "subscription",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "subscription",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "subscription",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "story_point",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "story_point",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "story_point",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "skill",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "skill",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "skill",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "seniority",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "seniority",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "seniority",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "role_translation",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "role_translation",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "role_translation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "project_group",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "project_group",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "project_group",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "project_company_user",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "project_company_user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "project_company_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "project",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "project",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "project",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "priority_level_translation",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "priority_level_translation",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "priority_level_translation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "priority_level",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "priority_level",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "priority_level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "policy",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "policy",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "policy",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "language_level",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "language_level",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "language_level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "language",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "language",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "language",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "job_role_translation",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "job_role_translation",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "job_role_translation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "job_role_category_translation",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "job_role_category_translation",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "job_role_category_translation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "job_role_category_skill",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "job_role_category_skill",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "job_role_category_skill",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "job_role_category",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "job_role_category",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "job_role_category",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "job_role",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "job_role",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "job_role",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "job_offer_talent_skill",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "job_offer_talent_skill",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "job_offer_talent_skill",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "job_offer_talent",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "job_offer_talent",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "job_offer_talent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "job_offer_benefit",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "job_offer_benefit",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "job_offer_benefit",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "job_offer",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "job_offer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "job_offer",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "industry",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "industry",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "industry",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "group_user",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "group_user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "group_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "group",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "group",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "group",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "goal_status_category",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "goal_status_category",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "goal_status_category",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "goal_status",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "goal_status",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "goal_status",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "goal_extra_mile",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "goal_extra_mile",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "goal_extra_mile",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "goal",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "goal",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "goal",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "feature",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "feature",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "feature",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "device_type",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "device_type",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "device_type",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "device",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "device",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "device",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "daily_time_block",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "daily_time_block",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "daily_time_block",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "daily_monitoring_block",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "daily_monitoring_block",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "daily_monitoring_block",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "country",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "country",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "country",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "contract_type",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "contract_type",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "contract_type",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "company_user",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "company_user",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "company_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "company_settings",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "company_settings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "company_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "company",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "company",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "company",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "city",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "city",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "city",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "billing_history",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "billing_history",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "billing_history",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "benefit",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "benefit",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "benefit",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                table: "app_client",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "archived_by",
                table: "app_client",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                table: "app_client",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "user_skill");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "user_skill");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "user_skill");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "user_refresh_token");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "user_refresh_token");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "user_refresh_token");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "user_language");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "user_language");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "user_language");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "time_zone");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "time_zone");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "time_zone");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "task_type");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "task_type");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "task_type");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "task");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "task");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "task");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "subscription_type_feature");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "subscription_type_feature");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "subscription_type_feature");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "subscription_type");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "subscription_type");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "subscription_type");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "subscription");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "subscription");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "subscription");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "story_point");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "story_point");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "story_point");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "skill");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "skill");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "skill");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "seniority");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "seniority");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "seniority");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "role_translation");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "role_translation");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "role_translation");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "project_group");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "project_group");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "project_group");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "project_company_user");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "project_company_user");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "project_company_user");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "project");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "project");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "project");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "priority_level_translation");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "priority_level_translation");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "priority_level_translation");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "priority_level");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "priority_level");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "priority_level");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "policy");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "policy");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "policy");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "language_level");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "language_level");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "language_level");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "language");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "language");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "language");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "job_role_translation");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "job_role_translation");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "job_role_translation");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "job_role_category_translation");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "job_role_category_translation");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "job_role_category_translation");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "job_role_category_skill");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "job_role_category_skill");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "job_role_category_skill");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "job_role_category");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "job_role_category");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "job_role_category");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "job_role");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "job_role");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "job_role");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "job_offer_talent_skill");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "job_offer_talent_skill");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "job_offer_talent_skill");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "job_offer_talent");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "job_offer_talent");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "job_offer_talent");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "job_offer_benefit");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "job_offer_benefit");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "job_offer_benefit");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "job_offer");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "job_offer");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "job_offer");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "industry");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "industry");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "industry");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "group_user");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "group_user");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "group_user");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "group");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "group");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "group");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "goal_status_category");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "goal_status_category");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "goal_status_category");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "goal_status");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "goal_status");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "goal_status");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "goal_extra_mile");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "goal_extra_mile");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "goal_extra_mile");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "goal");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "goal");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "goal");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "feature");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "feature");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "feature");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "device_type");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "device_type");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "device_type");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "device");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "device");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "device");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "daily_time_block");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "daily_time_block");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "daily_time_block");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "daily_monitoring_block");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "daily_monitoring_block");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "daily_monitoring_block");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "country");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "country");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "country");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "contract_type");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "contract_type");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "contract_type");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "company_user");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "company_settings");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "company_settings");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "company_settings");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "company");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "company");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "company");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "city");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "city");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "city");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "billing_history");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "billing_history");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "billing_history");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "benefit");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "benefit");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "benefit");

            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "app_client");

            migrationBuilder.DropColumn(
                name: "archived_by",
                table: "app_client");

            migrationBuilder.DropColumn(
                name: "is_archived",
                table: "app_client");
        }
    }
}
