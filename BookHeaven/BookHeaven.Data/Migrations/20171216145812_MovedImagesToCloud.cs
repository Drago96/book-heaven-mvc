using Microsoft.EntityFrameworkCore.Migrations;

namespace BookHeaven.Data.Migrations
{
    public partial class MovedImagesToCloud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BookPicture",
                table: "Books",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 10485760,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BookListingPicture",
                table: "Books",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 10485760,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureNav",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 2097152,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "BookPicture",
                table: "Books",
                maxLength: 10485760,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "BookListingPicture",
                table: "Books",
                maxLength: 10485760,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePictureNav",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                maxLength: 2097152,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}