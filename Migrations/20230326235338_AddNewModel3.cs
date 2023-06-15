using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace newproject.Migrations
{
    /// <inheritdoc />
    public partial class AddNewModel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "loginViewModels",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    superviseurId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    superviseurid_superviseur = table.Column<int>(type: "int", nullable: false),
                    RememberMe = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loginViewModels", x => x.Username);
                    table.ForeignKey(
                        name: "FK_loginViewModels_superviseurs_superviseurid_superviseur",
                        column: x => x.superviseurid_superviseur,
                        principalTable: "superviseurs",
                        principalColumn: "id_superviseur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_loginViewModels_superviseurid_superviseur",
                table: "loginViewModels",
                column: "superviseurid_superviseur");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "loginViewModels");
        }
    }
}
