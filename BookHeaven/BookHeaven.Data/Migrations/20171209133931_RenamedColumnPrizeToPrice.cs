using Microsoft.EntityFrameworkCore.Migrations;

namespace BookHeaven.Data.Migrations
{
    public partial class RenamedColumnPrizeToPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Prize",
                table: "Books",
                newName: "Price");
            migrationBuilder.AddColumn<byte[]>(
                name: "BookPicture",
                table: "Books",
                maxLength: 10485760,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookPicture",
                table: "Books");
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Books",
                newName: "Prize");
        }
    }
}