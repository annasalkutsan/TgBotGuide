using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgBotGuide.Infrastructure.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class AddImageURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "address",
                table: "locations",
                newName: "street");

            migrationBuilder.AddColumn<string>(
                name: "house",
                table: "locations",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "locations",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "house",
                table: "locations");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "locations");

            migrationBuilder.RenameColumn(
                name: "street",
                table: "locations",
                newName: "address");
        }
    }
}
