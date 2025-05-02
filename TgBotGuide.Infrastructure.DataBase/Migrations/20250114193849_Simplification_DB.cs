using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgBotGuide.Infrastructure.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class Simplification_DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "locations_categories");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropColumn(
                name: "house",
                table: "locations");

            migrationBuilder.DropColumn(
                name: "street",
                table: "locations");

            migrationBuilder.RenameColumn(
                name: "coordinates",
                table: "locations",
                newName: "mapUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "mapUrl",
                table: "locations",
                newName: "coordinates");

            migrationBuilder.AddColumn<string>(
                name: "house",
                table: "locations",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "street",
                table: "locations",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "locations_categories",
                columns: table => new
                {
                    location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations_categories", x => new { x.location_id, x.category_id });
                    table.ForeignKey(
                        name: "FK_locations_categories_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_locations_categories_locations_location_id",
                        column: x => x.location_id,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_locations_categories_category_id",
                table: "locations_categories",
                column: "category_id");
        }
    }
}
