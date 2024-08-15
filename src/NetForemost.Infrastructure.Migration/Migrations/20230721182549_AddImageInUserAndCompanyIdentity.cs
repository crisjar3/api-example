using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetForemost.Infrastructure.Migrations.Migrations
{
    public partial class AddImageInUserAndCompanyIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "company_image_url",
                table: "company",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "user_image_url",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "company_image_url",
                table: "company");

            migrationBuilder.DropColumn(
                name: "user_image_url",
                table: "AspNetUsers");
        }
    }
}
