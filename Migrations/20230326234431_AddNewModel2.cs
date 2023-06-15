using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace newproject.Migrations
{
    /// <inheritdoc />
    public partial class AddNewModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoginViewModels",
                columns: table => new
                {
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    SuperviseurId = table.Column<string>(nullable: false),
                    RememberMe = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginViewModels", x => x.Username);
                    table.ForeignKey(
                        name: "FK_LoginViewModels_Superviseurs_SuperviseurId",
                        column: x => x.SuperviseurId,
                        principalTable: "Superviseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginViewModels");
        }

    }
}
