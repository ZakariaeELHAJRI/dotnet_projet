using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace newproject.Migrations
{
    /// <inheritdoc />
    public partial class initialdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "superviseurs",
                columns: table => new
                {
                    id_superviseur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tele = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_superviseurs", x => x.id_superviseur);
                });

            migrationBuilder.CreateTable(
                name: "tablePlanchers",
                columns: table => new
                {
                    type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    cout_matier = table.Column<double>(type: "float", nullable: false),
                    cout_main = table.Column<double>(type: "float", nullable: false),
                    plancher_image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tablePlanchers", x => x.type);
                });

            migrationBuilder.CreateTable(
                name: "demandes",
                columns: table => new
                {
                    Id_demande = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tablePlancherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    langeur = table.Column<double>(type: "float", nullable: false),
                    largeur = table.Column<double>(type: "float", nullable: false),
                    cinClient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nom_client = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address_client = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demandes", x => x.Id_demande);
                    table.ForeignKey(
                        name: "FK_demandes_tablePlanchers_tablePlancherId",
                        column: x => x.tablePlancherId,
                        principalTable: "tablePlanchers",
                        principalColumn: "type",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "offres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    remise = table.Column<double>(type: "float", nullable: false),
                    date_validation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tablePlancherId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_offres_tablePlanchers_tablePlancherId",
                        column: x => x.tablePlancherId,
                        principalTable: "tablePlanchers",
                        principalColumn: "type",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "factures",
                columns: table => new
                {
                    Id_facture = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    superficie = table.Column<double>(type: "float", nullable: false),
                    cout_total = table.Column<double>(type: "float", nullable: false),
                    cout_main = table.Column<double>(type: "float", nullable: false),
                    taxe = table.Column<double>(type: "float", nullable: false),
                    demandeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_factures", x => x.Id_facture);
                    table.ForeignKey(
                        name: "FK_factures_demandes_demandeId",
                        column: x => x.demandeId,
                        principalTable: "demandes",
                        principalColumn: "Id_demande",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_demandes_tablePlancherId",
                table: "demandes",
                column: "tablePlancherId");

            migrationBuilder.CreateIndex(
                name: "IX_factures_demandeId",
                table: "factures",
                column: "demandeId");

            migrationBuilder.CreateIndex(
                name: "IX_offres_tablePlancherId",
                table: "offres",
                column: "tablePlancherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "factures");

            migrationBuilder.DropTable(
                name: "offres");

            migrationBuilder.DropTable(
                name: "superviseurs");

            migrationBuilder.DropTable(
                name: "demandes");

            migrationBuilder.DropTable(
                name: "tablePlanchers");
        }
    }
}
