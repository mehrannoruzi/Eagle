using Microsoft.EntityFrameworkCore.Migrations;

namespace Eagle.EFDataAccess.Migrations
{
    public partial class AddForcedChangePassword2User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForcedChangePassword",
                schema: "Auth",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForcedChangePassword",
                schema: "Auth",
                table: "User");
        }
    }
}
