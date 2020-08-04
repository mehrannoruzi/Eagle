using Microsoft.EntityFrameworkCore.Migrations;

namespace Eagle.Notifier.DataAccess.Ef.Migrations
{
    public partial class AddValidIpToApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ValidIp",
                schema: "Notifier",
                table: "Application",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidIp",
                schema: "Notifier",
                table: "Application");
        }
    }
}
