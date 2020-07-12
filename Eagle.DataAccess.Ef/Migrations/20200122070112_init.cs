using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eagle.DataAccess.Ef.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "Action",
                schema: "Auth",
                columns: table => new
                {
                    ActionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    OrderPriority = table.Column<byte>(nullable: false),
                    ShowInMenu = table.Column<bool>(nullable: false),
                    ControllerName = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    ActionName = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    Name = table.Column<string>(maxLength: 55, nullable: false),
                    Icon = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.ActionId);
                    table.ForeignKey(
                        name: "FK_Action_Action_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Auth",
                        principalTable: "Action",
                        principalColumn: "ActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuSPModel",
                schema: "Auth",
                columns: table => new
                {
                    ParentId = table.Column<int>(nullable: true),
                    ShowInMenu = table.Column<bool>(nullable: false),
                    OrderPriority = table.Column<byte>(nullable: false),
                    ControllerName = table.Column<string>(nullable: true),
                    ActionName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    RoleNameFa = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    Actions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enabled = table.Column<bool>(nullable: false),
                    RoleNameFa = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RoleNameEn = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Auth",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    MobileNumber = table.Column<long>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    LastLoginDateMi = table.Column<DateTime>(nullable: true),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false),
                    LastLoginDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    NewPassword = table.Column<string>(type: "nvarchar(45)", maxLength: 28, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ActionInRole",
                schema: "Auth",
                columns: table => new
                {
                    ActionInRoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    ActionId = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionInRole", x => x.ActionInRoleId);
                    table.ForeignKey(
                        name: "FK_ActionInRole_Action_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "Auth",
                        principalTable: "Action",
                        principalColumn: "ActionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionInRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInRole",
                schema: "Auth",
                columns: table => new
                {
                    UserInRoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInRole", x => x.UserInRoleId);
                    table.ForeignKey(
                        name: "FK_UserInRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_ParentId",
                schema: "Auth",
                table: "Action",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionInRole_ActionId",
                schema: "Auth",
                table: "ActionInRole",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionInRole_RoleId",
                schema: "Auth",
                table: "ActionInRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Email",
                schema: "Auth",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInRole_RoleId",
                schema: "Auth",
                table: "UserInRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRole_UserId",
                schema: "Auth",
                table: "UserInRole",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionInRole",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "MenuSPModel",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserInRole",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Action",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Auth");
        }
    }
}
