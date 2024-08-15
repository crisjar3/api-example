using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class DeletePropertiesUsertSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "1ac75091-d259-4c31-b277-bb0711802af1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "9ac75097-d259-4c31-b277-bb0711802af5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "role_id", "user_id" },
                keyValues: new object[] { "341743f0-asd2–42de-afbf-59kmkkmk72cf6", "02174cf0–9412–4cfe-afbf-59f706d72cf6" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "role_id", "user_id" },
                keyValues: new object[] { "7def0691-b95b-4ae1-8aef-ee1fa8e2f1c5", "02174cf0–9412–4cfe-afbf-59f706d72cf6" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "341743f0-asd2–42de-afbf-59kmkkmk72cf6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "7def0691-b95b-4ae1-8aef-ee1fa8e2f1c5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6");

            migrationBuilder.DropColumn(
                name: "blur_screenshots",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "can_edit_time",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "delete_screencasts",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "screencasts_frecuency",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "show_in_reports",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "time_out_after",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "time_zone",
                table: "user_settings");

            migrationBuilder.AddColumn<int>(
                name: "time_zone_id",
                table: "user_settings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_user_settings_time_zone_id",
                table: "user_settings",
                column: "time_zone_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_settings_time_z_ones_time_zone_id",
                table: "user_settings",
                column: "time_zone_id",
                principalTable: "time_zone",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_settings_time_z_ones_time_zone_id",
                table: "user_settings");

            migrationBuilder.DropIndex(
                name: "ix_user_settings_time_zone_id",
                table: "user_settings");

            migrationBuilder.DropColumn(
                name: "time_zone_id",
                table: "user_settings");

            migrationBuilder.AddColumn<bool>(
                name: "blur_screenshots",
                table: "user_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "can_edit_time",
                table: "user_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "delete_screencasts",
                table: "user_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "screencasts_frecuency",
                table: "user_settings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "show_in_reports",
                table: "user_settings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "time_out_after",
                table: "user_settings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "time_zone",
                table: "user_settings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { "1ac75091-d259-4c31-b277-bb0711802af1", "1ac75091-d259-4c31-b277-bb0711802af1", "Owner", "OWNER" },
                    { "341743f0-asd2–42de-afbf-59kmkkmk72cf6", "341743f0-asd2–42de-afbf-59kmkkmk72cf6", "Admin", "ADMIN" },
                    { "7def0691-b95b-4ae1-8aef-ee1fa8e2f1c5", "7def0691-b95b-4ae1-8aef-ee1fa8e2f1c5", "Manager", "MANAGER" },
                    { "9ac75097-d259-4c31-b277-bb0711802af5", "9ac75097-d259-4c31-b277-bb0711802af5", "Regular User", "REGULAR USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "id", "access_failed_count", "city_id", "concurrency_stamp", "email", "email_confirmed", "first_name", "is_active", "last_name", "lockout_enabled", "lockout_end", "normalized_email", "normalized_user_name", "password_hash", "phone_number", "phone_number_confirmed", "security_stamp", "two_factor_enabled", "user_name" },
                values: new object[] { "02174cf0–9412–4cfe-afbf-59f706d72cf6", 0, 1, "5c170dce-5571-4107-97ca-d4d033ded47a", "admin@admin", true, "Admin", true, "Admin", false, null, "ADMIN@ADMIN", "ADMIN", "AQAAAAEAACcQAAAAEBu6WJlyRiLmJDW7IpzYnuHdDAol+u8Bx8QibBut0nttZotoua4FO27To5J7TrHpyg==", "0000", false, "8fd674c7-ccfa-42e6-ba3b-3c379b15e42f", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "role_id", "user_id" },
                values: new object[,]
                {
                    { "341743f0-asd2–42de-afbf-59kmkkmk72cf6", "02174cf0–9412–4cfe-afbf-59f706d72cf6" },
                    { "7def0691-b95b-4ae1-8aef-ee1fa8e2f1c5", "02174cf0–9412–4cfe-afbf-59f706d72cf6" }
                });
        }
    }
}
