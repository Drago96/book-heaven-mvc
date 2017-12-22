using Microsoft.EntityFrameworkCore.Migrations;

namespace BookHeaven.Data.Migrations
{
    public partial class AddedResizedBookPictures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PublisherId",
                table: "Books",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "BookListingPicture",
                table: "Books",
                maxLength: 10485760,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookListingPicture",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "PublisherId",
                table: "Books",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}