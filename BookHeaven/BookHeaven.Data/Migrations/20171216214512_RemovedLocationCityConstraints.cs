using Microsoft.EntityFrameworkCore.Migrations;

namespace BookHeaven.Data.Migrations
{
    public partial class RemovedLocationCityConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_City_Country",
                table: "Locations");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Locations",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_City_Country",
                table: "Locations",
                columns: new[] { "City", "Country" },
                unique: true,
                filter: "[City] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_City_Country",
                table: "Locations");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Locations",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_City_Country",
                table: "Locations",
                columns: new[] { "City", "Country" },
                unique: true);
        }
    }
}