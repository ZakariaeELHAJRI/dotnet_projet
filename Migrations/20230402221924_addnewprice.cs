using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace newproject.Migrations
{
    /// <inheritdoc />
    public partial class addnewprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "cout_matier_new",
                table: "tablePlanchers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cout_matier_new",
                table: "tablePlanchers");
        }
    }
}
