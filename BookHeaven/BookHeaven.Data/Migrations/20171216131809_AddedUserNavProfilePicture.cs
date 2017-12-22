using Microsoft.EntityFrameworkCore.Migrations;

namespace BookHeaven.Data.Migrations
{
    public partial class AddedUserNavProfilePicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePictureNav",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureNav",
                table: "AspNetUsers");
        }
    }
}