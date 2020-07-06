using Microsoft.EntityFrameworkCore.Migrations;

namespace Eagle.EFDataAccess.Migrations
{
    public partial class RemoveSPTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuSPModel",
                schema: "Auth");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                schema: "Auth",
                table: "Action",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldMaxLength: 40,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                schema: "Auth",
                table: "Action",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MenuSPModel",
                schema: "Auth",
                columns: table => new
                {
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Actions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderPriority = table.Column<byte>(type: "tinyint", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RoleNameFa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInMenu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });
        }
    }
}
